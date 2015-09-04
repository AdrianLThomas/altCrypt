﻿using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Core.x86.Encryption
{
    public class StreamEncryptor : IEncryptor
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

        public Stream EncryptToStream(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            byte[] key = _key.GenerateBlock(_encryptionProvider.BlockSize);
            ICryptoTransform encryptor = _encryptionProvider.CreateEncryptor(key, key);

            byte[] buffer = file.Data.ToByteArray();

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream;
        }

        public Stream DecryptToStream(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            byte[] key = _key.GenerateBlock(_encryptionProvider.BlockSize);
            ICryptoTransform decryptor = _encryptionProvider.CreateDecryptor(key, key);

            var decryptedMemoryStream = new MemoryStream();

            using (var cryptoStream = new CryptoStream(file.Data, decryptor, CryptoStreamMode.Read))
            {
                file.Data.Seek(0, SeekOrigin.Begin);
                string encryptedString = new StreamReader(cryptoStream).ReadToEnd();
                StreamWriter fsDecrypted = new StreamWriter(decryptedMemoryStream);
                fsDecrypted.Write(encryptedString);
                fsDecrypted.Flush();
            }

            return decryptedMemoryStream;
        }

        public void Encrypt(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (Stream encryptedStream = this.EncryptToStream(file))
            {
                file.Write(encryptedStream);
            }
        }

        public void Decrypt(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (Stream decryptedStream = this.DecryptToStream(file))
            {
                file.Write(decryptedStream);
            }
        }
    }
}