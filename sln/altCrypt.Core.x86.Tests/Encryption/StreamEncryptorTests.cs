using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.Extensions;
using altCrypt.Core.x86.Encryption;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using altCrypt.Core.FileSystem;

namespace altCrypt.Core.x86.UnitTests.Encryption
{
    [TestClass]
    public class StreamEncryptorTests
    {
        private SymmetricAlgorithm _encryptionProvider;
        private IKey _key;
        private IIV _iv;
        private StreamEncryptor _streamEncryptor;
        private IFile _unencryptedFile;
        private IFile _encryptedFile;

        [TestInitialize]
        public void Initialise()
        {
            _encryptionProvider = Aes.Create();

            var ivMock = new Mock<IIV>();
            ivMock.Setup(m => m.GenerateIV(It.IsAny<int>())).Returns(TestConstants.IvData);

            _iv = ivMock.Object;
            _key = Mock.Of<IKey>();
            _streamEncryptor = new StreamEncryptor(new Key("password"), _iv, _encryptionProvider);
            _unencryptedFile = Mock.Of<IFile>(m => m.Read() == GetUnencryptedTestStream());
            _encryptedFile = Mock.Of<IFile>(m => m.Read() == GetEncryptedTestStream());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenKeyIsNull()
        {
            new StreamEncryptor(null, _iv, _encryptionProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenEncryptionProviderIsNull()
        {
            new StreamEncryptor(_key, _iv, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenIVIsNull()
        {
            new StreamEncryptor(_key, null, _encryptionProvider);
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
            _streamEncryptor.EncryptToStream(Mock.Of<IFile>(), null);
        }

        [TestMethod]
        public void EncryptToStream_ReturnsStreamWithExpectedLength_WhenFileParamIsValid()
        {
            //Arrange
            long expected = TestConstants.EncryptedData.Length;
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
            byte[] expected = TestConstants.EncryptedData;
            byte[] actual;

            using (var memStream = new MemoryStream())
            {
                //Act
                _streamEncryptor.EncryptToStream(_unencryptedFile, memStream);
                actual = memStream.ToByteArray();
            }

            //Assert
            Assert.IsTrue(actual.SequenceEqual(expected));
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
            _streamEncryptor.DecryptToStream(Mock.Of<IFile>(), null);
        }

        [TestMethod]
        public void DecryptToStream_ReturnsExpectedDecryptedStream_WhenFileParamIsValid()
        {
            //Arrange
            byte[] expected = TestConstants.UnencryptedData;
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

        private MemoryStream GetUnencryptedTestStream()
        {
            byte[] bytes = TestConstants.UnencryptedData;

            var memStream = new MemoryStream();
            memStream.Write(bytes, 0, bytes.Length);
            return memStream;
        }

        private MemoryStream GetEncryptedTestStream()
        {
            byte[] bytes = TestConstants.EncryptedData;

            var memStream = new MemoryStream();
            memStream.Write(bytes, 0, bytes.Length);
            return memStream;
        }
    }
}
