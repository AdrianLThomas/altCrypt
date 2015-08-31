using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86;
using altCrypt.Core.x86.Encryption;
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
            _fileEncryptor = new FileEncryptor(new Key("password")); //TODO - mock this
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
            byte[] expected = new byte[] { 85, 165, 12, 76, 67, 80, 153, 148, 245, 179, 39, 249, 129, 61, 110, 46, 1, 118, 7, 109, 252, 164, 24, 178, 204, 110, 42, 109, 225, 123, 176, 157, 225, 177, 35, 20, 224, 231, 137, 242, 185, 116, 248, 214, 143, 31, 49, 171 }; //Encrypted: GetTestPassword()
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
