using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;

namespace altCrypt.Core.Tests
{
    [TestClass]
    public class FileEncryptorTests
    {
        private FileEncryptor _fileEncryptor;
        private MemoryStream _testStream;
        private IFile _file;

        [TestInitialize]
        public void Initialise()
        {
            _fileEncryptor = new FileEncryptor("password");
            _testStream = GetTestStream();
            _file = Mock.Of<IFile>(m => m.Data == _testStream);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            new FileEncryptor(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenKeyIsEmpty()
        {
            new FileEncryptor(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _fileEncryptor.Encrypt(null);
        }

        [TestMethod]
        public void Encrypt_ReturnsNonNullStream_WhenFileParamIsValid()
        {
            //Arrange
            //Act
            Stream stream;
            using (_testStream)
            {
                stream = _fileEncryptor.Encrypt(_file);
            }

            //Assert
            Assert.IsNotNull(stream);
        }

        [TestMethod]
        public void Encrypt_ReturnsExpectedEncryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = new byte[] { 4, 97, 154, 112, 214, 121, 7, 58, 244, 91, 177, 45, 97, 0, 29, 22, 54, 103, 204, 167, 120, 22, 5, 205, 151, 197, 106, 25, 27, 29, 233, 61, 241, 59, 227, 146, 27, 45, 178, 86, 135, 27, 93, 140, 148, 55, 216, 215 }; //Encrypted: GetTestPassword()
            byte[] result;

            //Act
            using (_testStream)
            {
                using (Stream encryptedResultStream = _fileEncryptor.Encrypt(_file))
                {
                    result = encryptedResultStream.ReadAll();
                }
            }

            //Assert
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        private MemoryStream GetTestStream()
        {
            byte[] bytes = GetTestPassword();

            var memStream = new MemoryStream();
            memStream.Write(bytes, 0, bytes.Length);
            return memStream;
        }

        private byte[] GetTestPassword()
        {
            return System.Text.Encoding.ASCII.GetBytes("Some secret text I want to encrypt");
        }
    }
}
