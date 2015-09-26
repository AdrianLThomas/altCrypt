using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Core.x86.Encryption
{
    public class StreamEncryptor : IEncryptToStream
    {
        private readonly IKey _key;
        private readonly IIV _iv;
        private readonly SymmetricAlgorithm _encryptionProvider;

        public StreamEncryptor(IKey key, IIV iv, SymmetricAlgorithm encryptionProvider)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (encryptionProvider == null)
                throw new ArgumentNullException(nameof(encryptionProvider));
            if (iv == null)
                throw new ArgumentNullException(nameof(iv));

            _key = key;
            _iv = iv;
            _encryptionProvider = encryptionProvider;
        }

        public void EncryptToStream(IFile file, Stream outputStream)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));

            byte[] key = _key.GenerateBlock(_encryptionProvider.BlockSize);
            byte[] iv = _iv.GenerateIV(_encryptionProvider.BlockSize);
            ICryptoTransform encryptor = _encryptionProvider.CreateEncryptor(key, iv);

            //Write IV in plain text to beginning of stream
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(iv, 0, iv.Length);

            //Write encrypted data
            using (var stream = file.Read())
            {
                var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
                stream.CopyTo(cryptoStream);
                cryptoStream.FlushFinalBlock();
            }
        }

        public void DecryptToStream(IFile file, Stream outputStream)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));

            //Read IV
            int ivSize = _encryptionProvider.BlockSize / 8;
            byte[] iv = new byte[ivSize];
            var fileData = file.Read();

            fileData.Seek(0, SeekOrigin.Begin);
            fileData.Read(iv, 0, iv.Length);

            //Write decrypted data
            byte[] key = _key.GenerateBlock(_encryptionProvider.BlockSize);
            ICryptoTransform decryptor = _encryptionProvider.CreateDecryptor(key, iv);

            using (var cryptoStream = new CryptoStream(fileData, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream);
            }
        }
    }
}