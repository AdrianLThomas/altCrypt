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
        private readonly byte[] _decryptedData = System.Text.Encoding.ASCII.GetBytes("Some secret text I want to encrypt");

        private SymmetricAlgorithm _encryptionProvider;
        private IKey _key;
        private FileEncryptor _fileEncryptor;
        private IFile _unencryptedFile;
        private IFile _encryptedFile;

        [TestInitialize]
        public void Initialise()
        {
            _encryptionProvider = new AesCryptoServiceProvider();
            _key = Mock.Of<IKey>();
            _fileEncryptor = new FileEncryptor(new Key("password"), _encryptionProvider);
            _unencryptedFile = Mock.Of<IFile>(m => m.Data == GetUnencryptedTestStream());
            _encryptedFile = Mock.Of<IFile>(m => m.Data == GetEncryptedTestStream());
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
            using (GetUnencryptedTestStream())
            {
                stream = _fileEncryptor.EncryptToStream(_unencryptedFile);
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
            using (GetUnencryptedTestStream())
            {
                using (Stream encryptedResultStream = _fileEncryptor.EncryptToStream(_unencryptedFile))
                {
                    actual = encryptedResultStream.ToByteArray();
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
            byte[] expected = _decryptedData;
            byte[] actual;

            //Act
            using (Stream decryptedStream = _fileEncryptor.DecryptToStream(_encryptedFile))
            {
                actual = decryptedStream.ToByteArray();
            }

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _fileEncryptor.Encrypt(null);
        }

        [TestMethod]
        public void Encrypt_EncryptsStream_WhenFileParamIsValid()
        {
            //Arrange
            var fileToEncrypt = Mock.Of<IFile>(m => m.Data == GetUnencryptedTestStream());
            byte[] expected = _encryptedData;
            byte[] actual;

            //Act
            _fileEncryptor.Encrypt(fileToEncrypt);
            actual = fileToEncrypt.Data.ToByteArray();

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }


        private MemoryStream GetUnencryptedTestStream()
        {
            byte[] bytes = _decryptedData;

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
    }
}
