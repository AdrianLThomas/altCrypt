﻿using System;
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

        public byte[] GenerateBlock(int blockSize)
        {
            int byteSize = (int)blockSize / 8;
            int rem = byteSize - _key.Length;

            List<byte> byteList = new List<byte>(_key);
            for(int i = 0; i < rem; ++i)
                byteList.Add(0);

            return byteList.ToArray();
        }
    }
}
