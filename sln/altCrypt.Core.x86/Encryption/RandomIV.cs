using System;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;

namespace altCrypt.Core.x86.Encryption
{
    public class RandomIV : IIV
    {
        public byte[] GenerateIV(int blockSize)
        {
            if (blockSize <= 8)
                throw new ArgumentOutOfRangeException(nameof(blockSize));
                
            var output = new byte[blockSize / 8];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(output);
            }

            return output;
        }
    }
}
