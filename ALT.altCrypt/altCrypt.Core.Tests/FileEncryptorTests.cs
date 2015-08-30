using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using altCrypt.Core.FileSystem;
using Moq;
using altCrypt.Core.x86;

namespace altCrypt.Core.Tests
{
    [TestClass]
    public class FileEncryptorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            //Arrange
            var fileEncryptor = new FileEncryptor();

            //Act
            fileEncryptor.Encrypt(null);

            //Assert
            //Exception
        }

        [TestMethod]
        public void Encrypt_ReturnsNonNullStream_WhenFileParamIsValid()
        {
            //Arrange
            var fileEncryptor = new FileEncryptor();
            var memStream = new MemoryStream();
            IFile file = Mock.Of<IFile>(m => m.Data == memStream);

            //Act
            Stream stream;
            using (memStream)
            {
                stream = fileEncryptor.Encrypt(file);
            }

            //Assert
            Assert.IsNotNull(stream);
        }
    }
}
