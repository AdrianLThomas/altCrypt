using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.x86.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IFileStream = altCrypt.Core.FileSystem.IFile<System.IO.Stream>;

namespace altCrypt.Core.x86.UnitTests.Encryption
{
    [TestClass]
    public class StreamEncryptorTests
    {
        private readonly byte[] _unencryptedData = { 1, 2, 3 };
        private readonly byte[] _encryptedData = { 133, 78, 234, 30, 211, 123, 83, 181, 74, 32, 182, 109, 141, 32, 129, 217 };

        private SymmetricAlgorithm _encryptionProvider;
        private IKey _key;
        private StreamEncryptor _streamEncryptor;
        private IFileStream _unencryptedFile;
        private IFileStream _encryptedFile;

        [TestInitialize]
        public void Initialise()
        {
            _encryptionProvider = Aes.Create();

            _key = Mock.Of<IKey>();
            _streamEncryptor = new StreamEncryptor(new Key("password"), _encryptionProvider);
            _unencryptedFile = Mock.Of<IFileStream>(m => m.Read() == GetUnencryptedTestStream());
            _encryptedFile = Mock.Of<IFileStream>(m => m.Read() == GetEncryptedTestStream());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            new StreamEncryptor(null, _encryptionProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEncryptionProviderIsNull()
        {
            new StreamEncryptor(_key, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptToStream_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _streamEncryptor.EncryptToStream(null, Stream.Null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncryptToStream_ThrowsArgumentNullException_WhenStreamIsNull()
        {
            _streamEncryptor.EncryptToStream(Mock.Of<IFileStream>(), null);
        }

        [TestMethod]
        public void EncryptToStream_ReturnsStreamWithExpectedLength_WhenFileParamIsValid()
        {
            //Arrange
            long expected = _encryptedData.Length;
            long actual;

            using (var memStream = new MemoryStream())
            {
                //Act
                _streamEncryptor.EncryptToStream(_unencryptedFile, memStream);
                actual = memStream.Length;

                //Assert
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void EncryptToStream_OutputsExpectedEncryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = _encryptedData;
            byte[] actual;

            using (var memStream = new MemoryStream())
            {
                //Act
                _streamEncryptor.EncryptToStream(_unencryptedFile, memStream);
                actual = memStream.ToByteArray();

                //Assert
                Assert.IsTrue(actual.SequenceEqual(expected));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptToStream_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _streamEncryptor.DecryptToStream(null, Stream.Null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptToStream_ThrowsArgumentNullException_WhenStreamIsNull()
        {
            _streamEncryptor.DecryptToStream(Mock.Of<IFileStream>(), null);
        }

        [TestMethod]
        public void DecryptToStream_ReturnsExpectedDecryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = _unencryptedData;
            byte[] actual;

            //Act
            using (var memStream = new MemoryStream())
            {
                _streamEncryptor.DecryptToStream(_encryptedFile, memStream);
                actual = memStream.ToByteArray();
            }

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _streamEncryptor.Encrypt(null);
        }

        [TestMethod]
        public void Encrypt_CallsWriteOnFile_WhenFileParamIsValid()
        {
            //Arrange
            var fileMock = new Mock<IFileStream>();
            fileMock.Setup(m => m.Read()).Returns(GetUnencryptedTestStream);

            //Act
            _streamEncryptor.Encrypt(fileMock.Object);

            //Assert
            fileMock.Verify(m => m.Write(It.IsAny<Stream>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Decrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _streamEncryptor.Decrypt(null);
        }

        [TestMethod]
        public void Decrypt_CallsWriteOnFile_WhenFileParamIsValid()
        {
            //Arrange
            var fileMock = new Mock<IFileStream>();
            fileMock.Setup(m => m.Read()).Returns(GetEncryptedTestStream);

            //Act
            _streamEncryptor.Decrypt(fileMock.Object);

            //Assert
            fileMock.Verify(m => m.Write(It.IsAny<Stream>()));
        }


        private MemoryStream GetUnencryptedTestStream()
        {
            byte[] bytes = _unencryptedData;

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
