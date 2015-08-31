using System;
using System.Collections.Generic;
using System.Text;

namespace altCrypt.Core.Encryption
{
    public class Key : IKey
    {
        private readonly byte[] _key;

        public Key(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _key = Encoding.UTF8.GetBytes(key);
        }

        public Key(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _key = key;
        }

        public byte[] GenerateBlock(BlockSize keySize)
        {
            int keySizeInt = (int)keySize / 8;
            int rem = keySizeInt - _key.Length;

            List<byte> byteList = new List<byte>(_key);
            byteList.AddRange(new byte[rem]);

            return byteList.ToArray();
        }
    }
}
