using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using altCrypt.Core.Extensions;
using altCrypt.Core.Encryption;

namespace altCrypt.Core.x86.Encryption
{
    public class FileEncryptor : IEncryptor
    {
        private readonly IKey _key;

        public FileEncryptor(IKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _key = key;
        }

        public Stream Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var provider = Aes.Create();
            byte[] key = _key.GenerateBlock(BlockSize._128Bit);
            ICryptoTransform encryptor = provider.CreateEncryptor(key, key);

            byte[] buffer = file.Data.ReadAll();

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream;
        }
    }
}
