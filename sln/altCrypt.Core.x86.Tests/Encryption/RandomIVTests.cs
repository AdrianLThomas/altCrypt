using System;
using altCrypt.Core.x86.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace altCrypt.Core.x86.UnitTests.Encryption
{
    [TestClass]
    public class RandomIVTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GenerateIV_ThrowsArgumentOutOfRangeException_WhenBlockSizeIsZero()
        {
            //Arrange
            var iv = new RandomIV();
            int blockSize = 0;

            //Act
            //Assert
            iv.GenerateIV(blockSize);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GenerateIV_ThrowsArgumentOutOfRangeException_WhenBlockSizeIsLessThanZero()
        {
            //Arrange
            var iv = new RandomIV();
            int blockSize = -1;

            //Act
            //Assert
            iv.GenerateIV(blockSize);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GenerateIV_ThrowsArgumentOutOfRangeException_WhenBlockSizeIsLessThanAByte()
        {
            //Arrange
            var iv = new RandomIV();
            int blockSize = 7;

            //Act
            //Assert
            iv.GenerateIV(blockSize);
        }

        [TestMethod]
        public void GenerateIV_ReturnsNonZeroArray_WhenBlockSizeIsMoreThanOrEqualToOneByte()
        {
            //Arrange
            var iv = new RandomIV();
            int blockSize = 9;
            byte[] actual;

            //Act
            actual = iv.GenerateIV(blockSize);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Length > 0);
        }

        [TestMethod]
        public void GenerateIV_Returns16ByteArray_WhenBlockSizeIs128()
        {
            //Arrange
            var iv = new RandomIV();
            int blockSize = 128;

            const int expected = 16;
            int actual;

            //Act
            actual = iv.GenerateIV(blockSize).Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
