using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using altCrypt.Core.FileSystem;
using altCrypt.Core.Encryption;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace altCrypt.Business.UnitTests
{
    [TestClass]
    public class FileProcessorTests
    {
        private readonly string _processedExtension = ".altCryptTest";

        [TestMethod]
        public async Task ProcessAsync_RenamesFiles_WhenFileParamIsValid()
        {
            //Arrange
            var encryptor = Mock.Of<IEncryptFiles>();
            var processor = new FileProcessor(_processedExtension, encryptor);
            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Name).Returns("TestFile.txt");

            //Act
            await processor.ProcessAsync(new[] { fileMock.Object });

            //Assert
            fileMock.Verify(x => x.Rename(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task ProcessAsync_EncryptsFiles_WhenFileParamIsValid()
        {
            //Arrange
            var encryptorMock = new Mock<IEncryptFiles>();

            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Name).Returns("TestFile.txt");

            var processor = new FileProcessor(_processedExtension, encryptorMock.Object);

            //Act
            await processor.ProcessAsync(new[] { fileMock.Object });

            //Assert
            encryptorMock.Verify(x => x.EncryptAsync(It.IsAny<IEnumerable<IFile>>()));
        }

        public async Task ReverseProcessAsync_RenamesFiles_WhenFileParamIsValid()
        {
            //Arrange
            var encryptor = Mock.Of<IEncryptFiles>();
            var processor = new FileProcessor(_processedExtension, encryptor);
            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Name).Returns($"TestFile.txt{_processedExtension}");

            //Act
            await processor.ReverseProcessAsync(new[] { fileMock.Object });

            //Assert
            fileMock.Verify(x => x.Rename(It.IsAny<string>()));
        }

        [TestMethod]
        public async Task ReverseProcessAsync_DecryptsFile_WhenFileParamIsValid()
        {
            //Arrange
            var encryptorMock = new Mock<IEncryptFiles>();

            var fileMock = new Mock<IFile>();
            fileMock.Setup(m => m.Name).Returns($"TestFile.txt{_processedExtension}");

            var processor = new FileProcessor(_processedExtension, encryptorMock.Object);

            //Act
            await processor.ReverseProcessAsync(new[] { fileMock.Object });

            //Assert
            encryptorMock.Verify(x => x.DecryptAsync(It.IsAny<IEnumerable<IFile>>()));
        }
    }
}
