using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using IFileStream = altCrypt.Core.FileSystem.IFile<System.IO.Stream>;

namespace altCrypt.Core.x86.Encryption.UnitTests
{
    [TestClass]
    public class StreamEncryptorTests
    {
        private readonly byte[] _uncryptedData = { 1, 2, 3, 0, 0, 0, 0, 0 };
        private readonly byte[] _encryptedData = { 0, 0, 0, 0, 0, 3, 2, 1 };

        private SymmetricAlgorithm _encryptionProvider;
        private IKey _key;
        private StreamEncryptor _streamEncryptor;
        private IFileStream _unencryptedFile;
        private IFileStream _encryptedFile;

        [TestInitialize]
        public void Initialise()
        {
            var cryptoTransform = new Mock<ICryptoTransform>();
            cryptoTransform.Setup(m => m.InputBlockSize).Returns(8);
            cryptoTransform.Setup(m => m.OutputBlockSize).Returns(8);
            cryptoTransform.Setup(m => m.TransformFinalBlock(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                           .Returns<byte[], int, int>((input, offset, count) => input.Reverse().ToArray());

            var encryptionMock = new Mock<SymmetricAlgorithm>();
            encryptionMock.Setup(m => m.CreateEncryptor(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                          .Returns(cryptoTransform.Object);
            encryptionMock.Setup(m => m.CreateDecryptor(It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                          .Returns(cryptoTransform.Object);

            _encryptionProvider = encryptionMock.Object;
            _key = Mock.Of<IKey>();
            _streamEncryptor = new StreamEncryptor(new Key("password"), _encryptionProvider);
            _unencryptedFile = Mock.Of<IFileStream>(m => m.Data == GetUnencryptedTestStream());
            _encryptedFile = Mock.Of<IFileStream>(m => m.Data == GetEncryptedTestStream());
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
            _streamEncryptor.EncryptToStream(null);
        }

        [TestMethod]
        public void EncryptToStream_ReturnsNonNullStream_WhenFileParamIsValid()
        {
            //Arrange
            //Act
            Stream stream = _streamEncryptor.EncryptToStream(_unencryptedFile);

            //Assert
            Assert.IsNotNull(stream);
        }

        [TestMethod]
        public void EncryptToStream_ReturnsExpectedEncryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = _encryptedData;
            byte[] actual;

            //Act
            using (Stream encryptedResultStream = _streamEncryptor.EncryptToStream(_unencryptedFile))
            {
                actual = encryptedResultStream.ToByteArray();
            }

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DecryptToStream_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _streamEncryptor.DecryptToStream(null);
        }

        [TestMethod]
        public void DecryptToStream_ReturnsExpectedDecryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = _uncryptedData;
            byte[] actual;

            //Act
            using (Stream decryptedStream = _streamEncryptor.DecryptToStream(_encryptedFile))
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
            _streamEncryptor.Encrypt(null);
        }

        [TestMethod]
        public void Encrypt_EncryptsStream_WhenFileParamIsValid()
        {
            //Arrange
            var fileToEncrypt = Mock.Of<IFileStream>(m => m.Data == GetUnencryptedTestStream());
            byte[] expected = _encryptedData;
            byte[] actual;

            //Act
            _streamEncryptor.Encrypt(fileToEncrypt);
            actual = fileToEncrypt.Data.ToByteArray();

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
        }


        private MemoryStream GetUnencryptedTestStream()
        {
            byte[] bytes = _uncryptedData;

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
