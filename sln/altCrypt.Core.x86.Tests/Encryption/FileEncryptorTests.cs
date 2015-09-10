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
using System.Threading.Tasks;
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
        public async Task EncryptAsync_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            await _fileEncryptor.EncryptAsync((IFile)null);
        }

        [TestMethod]
        public async Task EncryptAsync_CallsWriteOnFile_WhenFileParamIsValid()
        {
            //Arrange
            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Read()).Returns(GetUnencryptedTestStream);

            //Act
            await _fileEncryptor.EncryptAsync(fileMock.Object);

            //Assert
            fileMock.Verify(m => m.WriteAsync(It.IsAny<Stream>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task EncryptAsync_ThrowsArgumentNullException_WhenIEnumerableIsNull()
        {
            await _fileEncryptor.EncryptAsync((IEnumerable<IFile>)null);
        }

        [TestMethod]
        public async Task EncryptAsync_CallsWriteOnAllFiles_WhenIEnumerableIsValid()
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
            await _fileEncryptor.EncryptAsync(files);

            //Assert
            fileMocks.ForEach(x => x.Verify(m => m.WriteAsync(It.IsAny<Stream>())));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DecryptAsync_ThrowsArgumentNullException_WhenFileParamIsNull()
        {
            await _fileEncryptor.DecryptAsync((IFile)null);
        }

        [TestMethod]
        public async Task DecryptAsync_CallsWriteOnFile_WhenFileParamIsValid()
        {
            //Arrange
            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Read()).Returns(GetEncryptedTestStream);

            //Act
            await _fileEncryptor.DecryptAsync(fileMock.Object);

            //Assert
            fileMock.Verify(m => m.WriteAsync(It.IsAny<Stream>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Dencrypt_ThrowsArgumentNullException_WhenIEnumerableIsNull()
        {
            await _fileEncryptor.DecryptAsync((IEnumerable<IFile>)null);
        }

        [TestMethod]
        public async Task DecryptAsync_CallsWriteOnAllFiles_WhenIEnumerableIsValid()
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
            await _fileEncryptor.DecryptAsync(files);

            //Assert
            fileMocks.ForEach(x => x.Verify(m => m.WriteAsync(It.IsAny<Stream>())));
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
