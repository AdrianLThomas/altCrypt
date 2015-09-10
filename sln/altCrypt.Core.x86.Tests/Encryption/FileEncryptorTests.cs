using System;
using System.Collections.Generic;
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
    public class FileEncryptorTests
    {
        private SymmetricAlgorithm _encryptionProvider;
        private IKey _key;
        private IIV _iv;
        private FileEncryptor _fileEncryptor;

        [TestInitialize]
        public void Initialise()
        {
            _encryptionProvider = Aes.Create();

            var ivMock = new Mock<IIV>();
            ivMock.Setup(m => m.GenerateIV(It.IsAny<int>())).Returns(TestConstants.IvData);

            _iv = ivMock.Object;
            _key = Mock.Of<IKey>();
            _fileEncryptor = new FileEncryptor(new Key("password"), _iv, _encryptionProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _fileEncryptor.Encrypt((IFile)null);
        }

        [TestMethod]
        public void Encrypt_CallsWriteOnFile_WhenFileParamIsValid()
        {
            //Arrange
            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Read()).Returns(GetUnencryptedTestStream);

            //Act
            _fileEncryptor.Encrypt(fileMock.Object);

            //Assert
            fileMock.Verify(m => m.Write(It.IsAny<Stream>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_ThrowsArgumentNullException_WhenIEnumerableIsNull()
        {
            _fileEncryptor.Encrypt((IEnumerable<IFile>)null);
        }

        [TestMethod]
        public void Encrypt_CallsWriteOnAllFiles_WhenIEnumerableIsValid()
        {
            //Arrange
            var fileMocks = new List<Mock<IFile>>();
            for (int i = 0; i < 3; ++i)
            {
                var fileMock = new Mock<IFile>();
                fileMock.Setup(m => m.Read()).Returns(GetUnencryptedTestStream);
                fileMocks.Add(fileMock);
            }
            IEnumerable<IFile> files = fileMocks.Select(x => x.Object);

            //Act
            _fileEncryptor.Encrypt(files);

            //Assert
            fileMocks.ForEach(x => x.Verify(m => m.Write(It.IsAny<Stream>())));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Decrypt_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            _fileEncryptor.Decrypt((IFile)null);
        }

        [TestMethod]
        public void Decrypt_CallsWriteOnFile_WhenFileParamIsValid()
        {
            //Arrange
            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Read()).Returns(GetEncryptedTestStream);

            //Act
            _fileEncryptor.Decrypt(fileMock.Object);

            //Assert
            fileMock.Verify(m => m.Write(It.IsAny<Stream>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Dencrypt_ThrowsArgumentNullException_WhenIEnumerableIsNull()
        {
            _fileEncryptor.Decrypt((IEnumerable<IFile>)null);
        }

        [TestMethod]
        public void Decrypt_CallsWriteOnAllFiles_WhenIEnumerableIsValid()
        {
            //Arrange
            var fileMocks = new List<Mock<IFile>>();
            for (int i = 0; i < 3; ++i)
            {
                var fileMock = new Mock<IFile>();
                fileMock.Setup(m => m.Read()).Returns(GetEncryptedTestStream);
                fileMocks.Add(fileMock);
            }
            IEnumerable<IFile> files = fileMocks.Select(x => x.Object);

            //Act
            _fileEncryptor.Decrypt(files);

            //Assert
            fileMocks.ForEach(x => x.Verify(m => m.Write(It.IsAny<Stream>())));
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
