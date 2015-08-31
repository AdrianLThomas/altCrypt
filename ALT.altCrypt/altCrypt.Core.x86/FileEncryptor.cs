using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using altCrypt.Core.Extensions;

namespace altCrypt.Core.x86
{
    public class FileEncryptor : IEncryptor
    {
        private readonly string _key;

        public FileEncryptor(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _key = key;
        }

        public Stream Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var provider = Aes.Create();
            byte[] key = GenerateKey();
            ICryptoTransform encryptor = provider.CreateEncryptor(key, key);

            byte[] buffer = file.Data.ReadAll();

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();

            return memoryStream;
        }

        private byte[] GenerateKey()
        {
            var bytes = Encoding.ASCII.GetBytes(_key);

            List<byte> byteList = new List<byte>();
            byteList.AddRange(bytes);
            int keySize = 128 / 8; //128 bit key size
            int rem = keySize - bytes.Length;

            for (int i = 0; i < rem; ++i)
            {
                byteList.Add(0);
            }

            return byteList.ToArray();
        }
    }
}
