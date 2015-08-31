using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Core.x86.Encryption
{
    public class FileEncryptor : IEncryptor
    {
        private readonly IKey _key;
        private readonly SymmetricAlgorithm _encryptionProvider;

        public FileEncryptor(IKey key, SymmetricAlgorithm encryptionProvider)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (encryptionProvider == null)
                throw new ArgumentNullException(nameof(encryptionProvider));

            _key = key;
            _encryptionProvider = encryptionProvider;
        }

        public Stream Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            byte[] key = _key.GenerateBlock(BlockSize._128Bit);
            ICryptoTransform encryptor = _encryptionProvider.CreateEncryptor(key, key);

            byte[] buffer = file.Data.ReadAll();

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream;
        }
    }
}
