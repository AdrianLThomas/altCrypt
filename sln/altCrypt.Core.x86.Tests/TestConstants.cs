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
        public static readonly byte[] UnencryptedData = { 1, 2, 3 };
        public static readonly byte[] EncryptedData = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, /*<- IV bytes*/
                                                         255, 129, 97, 155, 60, 235, 240, 133, 241, 13, 67, 72, 241, 82, 10, 0 /*<- Encrypted content*/};
    }
}
