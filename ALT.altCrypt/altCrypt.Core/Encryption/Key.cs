using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            List<byte> byteList = new List<byte>();
            byteList.AddRange(_key);
            int keySizeInt = (int)keySize / 8;
            int rem = keySizeInt - _key.Length;

            for (int i = 0; i < rem; ++i)
            {
                byteList.Add(0);
            }

            return byteList.ToArray();
        }
    }
}
