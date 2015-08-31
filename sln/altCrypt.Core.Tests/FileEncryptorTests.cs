using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace altCrypt.Core.Tests
{
    [TestClass]
    public class FileEncryptorTests
    {
        //TODO - This is the test string encrypted with AES - needs refactoring to not depend on AES.
        private readonly byte[] _encryptedData = new byte[] { 85, 165, 12, 76, 67, 80, 153, 148, 245, 179, 39, 249, 129, 61, 110, 46, 1, 118, 7, 109, 252, 164, 24, 178, 204, 110, 42, 109, 225, 123, 176, 157, 225, 177, 35, 20, 224, 231, 137, 242, 185, 116, 248, 214, 143, 31, 49, 171 };

        private SymmetricAlgorithm _encryptionProvider;
        private IKey _key;
        private FileEncryptor _fileEncryptor;
        private MemoryStream _testStream;
        private IFile _file;

        [TestInitialize]
        public void Initialise()
        {
            _encryptionProvider = new AesCryptoServiceProvider();
            _key = Mock.Of<IKey>();
            _fileEncryptor = new FileEncryptor(new Key("password"), _encryptionProvider);
            _testStream = GetUnencryptedTestStream();
            _file = Mock.Of<IFile>(m => m.Data == _testStream);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            new FileEncryptor(null, _encryptionProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEncryptionProviderIsNull()
        {
            new FileEncryptor(_key, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptToStream_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _fileEncryptor.EncryptToStream(null);
        }

        [TestMethod]
        public void EncryptToStream_ReturnsNonNullStream_WhenFileParamIsValid()
        {
            //Arrange
            //Act
            Stream stream;
            using (_testStream)
            {
                stream = _fileEncryptor.EncryptToStream(_file);
            }

            //Assert
            Assert.IsNotNull(stream);
        }

        [TestMethod]
        public void EncryptToStream_ReturnsExpectedEncryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = _encryptedData; //AES Encrypted: GetTestPassword()
            byte[] actual;

            //Act
            using (_testStream)
            {
                using (Stream encryptedResultStream = _fileEncryptor.EncryptToStream(_file))
                {
                    actual = encryptedResultStream.ReadAll();
                }
            }

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptToStream_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _fileEncryptor.DecryptToStream(null);
        }

        [TestMethod]
        public void DecryptToStream_ReturnsExpectedDecryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            var encryptedFile = Mock.Of<IFile>(m => m.Data == GetEncryptedTestStream());
            byte[] expected = GetTestPassword();
            byte[] actual;

            //Act
            using (Stream decryptedStream = _fileEncryptor.DecryptToStream(encryptedFile))
            {
                actual = decryptedStream.ReadAll();
            }

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }


        private MemoryStream GetUnencryptedTestStream()
        {
            byte[] bytes = GetTestPassword();

            var memStream = new MemoryStream();
            memStream.Write(bytes, 0, bytes.Length);
            return memStream;
        }

        private MemoryStream GetEncryptedTestStream()
        {
            byte[] bytes = _encryptedData;

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
