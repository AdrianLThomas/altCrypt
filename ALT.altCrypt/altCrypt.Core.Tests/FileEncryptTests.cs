using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using altCrypt.Core.FileSystem;
using Moq;

namespace altCrypt.Core.Tests
{
    [TestClass]
    public class FileEncryptTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            //Arrange
            var fileEncryptor = new FileEncrypt();

            //Act
            fileEncryptor.Encrypt(null);

            //Assert
            //Exception
        }

        [TestMethod]
        public void Encrypt_ReturnsNonNullStream_WhenFileParamIsValid()
        {
            //Arrange
            var fileEncryptor = new FileEncrypt();
            IFile file = new Mock<IFile>().Object;

            //Act
            Stream stream = fileEncryptor.Encrypt(file);

            //Assert
            Assert.IsNotNull(stream);
        }
    }
}
