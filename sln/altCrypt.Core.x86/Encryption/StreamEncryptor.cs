using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Core.x86.Encryption
{
    public class StreamEncryptor : IEncryptToStream, IEncryptFile
    {
        private readonly IKey _key;
        private readonly SymmetricAlgorithm _encryptionProvider;

        public StreamEncryptor(IKey key, SymmetricAlgorithm encryptionProvider)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (encryptionProvider == null)
                throw new ArgumentNullException(nameof(encryptionProvider));

            _key = key;
            _encryptionProvider = encryptionProvider;
        }

        public void EncryptToStream(IFile<Stream> file, Stream outputStream)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));

            byte[] key = _key.GenerateBlock(_encryptionProvider.BlockSize);
            ICryptoTransform encryptor = _encryptionProvider.CreateEncryptor(key, key);

            using (var stream = file.Read())
            {
                var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
                stream.CopyTo(cryptoStream);
                cryptoStream.FlushFinalBlock();
            }
        }

        public void DecryptToStream(IFile<Stream> file, Stream outputStream)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (outputStream == null)
                throw new ArgumentNullException(nameof(outputStream));

            byte[] key = _key.GenerateBlock(_encryptionProvider.BlockSize);
            ICryptoTransform decryptor = _encryptionProvider.CreateDecryptor(key, key);

            var fileData = file.Read();
            fileData.Seek(0, SeekOrigin.Begin);
            using (var cryptoStream = new CryptoStream(fileData, decryptor, CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream);
            }
        }

        public void Encrypt(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string tempFilename = $"{file.FilePath}.temp";
            using (var stream = new FileStream(tempFilename, FileMode.OpenOrCreate))
            {
                this.EncryptToStream(file, stream);
                file.Write(stream);
            }

            File.Delete(tempFilename);
        }

        public void Decrypt(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string tempFilename = $"{file.FilePath}.temp";
            using (var stream = new FileStream(tempFilename, FileMode.OpenOrCreate))
            {
                this.DecryptToStream(file, stream);
                file.Write(stream);
            }

            File.Delete(tempFilename);
        }
    }
}