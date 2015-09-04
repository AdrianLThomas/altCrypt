using altCrypt.Core.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace altCrypt.Core.UnitTests
{
    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEmptyByteArrayPassed()
        {
            //Act
            new Key((byte[]) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEmptyStringPassed()
        {
            new Key(string.Empty);
        }

        [TestMethod]
        public void GenerateBlock_Returns16ByteArray_WhenBlockSizeIs128Bit()
        {
            //Arrange
            var key = new Key("password");
            const int expectedLength = 16;
            const int blockSize = 128;

            //Act
            byte[] block = key.GenerateBlock(blockSize);
            int actualLength = block.Length;

            //Assert
            Assert.AreEqual(expectedLength, actualLength);
        }
    }
}
