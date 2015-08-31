using altCrypt.Core.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Core.Tests
{
    [TestClass]
    public class KeyTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEmptyByteArrayPassed()
        {
            //Arrange
            byte[] key = null;

            //Act
            new Key(key);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEmptyStringPassed()
        {
            //Arrange
            //Act
            new Key(string.Empty);

            //Assert
        }

        [TestMethod]
        public void GenerateBlock_Returns16ByteArray_WhenBlockSizeIs128Bit()
        {
            //Arrange
            var key = new Key("password");
            const int expectedLength = 16;

            //Act
            byte[] block = key.GenerateBlock(BlockSize._128Bit);
            int actualLength = block.Length;

            //Assert
            Assert.AreEqual(expectedLength, actualLength);
        }
    }
}
