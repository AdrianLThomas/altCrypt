using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Core.x86.UnitTests
{
    public static class TestConstants
    {
        public static readonly byte[] IvData = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
        public static readonly byte[] UnencryptedData = { 49, 50, 51 };
        public static readonly byte[] EncryptedData = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, /*<- IV - 16 bytes*/
                                                        255, 129, 097, 155, 060, 235, 240, 133, 241, 013, 067, 072, 241, 082, 010, 000 /*<- Encrypted content - 16 bytes*/};
    }
}
