using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using altCrypt.Core.FileSystem;
using altCrypt.Core.Encryption;
using System.Collections.Generic;

namespace altCrypt.Business.UnitTests
{
    [TestClass]
    public class FileProcessorTests
    {
        private readonly string _processedExtension = ".altCryptTest";

        [TestMethod]
        public void Process_RenamesFiles_WhenFileParamIsValid()
        {
            //Arrange
            var encryptor = Mock.Of<IEncryptFiles>();
            var processor = new FileProcessor(_processedExtension, encryptor);
            var fileMock = new Mock<IFile<Stream>>();
            fileMock.Setup(m => m.Name).Returns("TestFile.txt");

            //Act
            processor.Process(new[] { fileMock.Object });

            //Assert
            fileMock.Verify(x => x.Rename(It.IsAny<string>()));
        }

        [TestMethod]
        public void Process_EncryptsFiles_WhenFileParamIsValid()
        {
            //Arrange
            var encryptorMock = new Mock<IEncryptFiles>();

            var fileMock = new Mock<IFile<Stream>>();
            fileMock.Setup(m => m.Name).Returns("TestFile.txt");

            var processor = new FileProcessor(_processedExtension, encryptorMock.Object);

            //Act
            processor.Process(new[] { fileMock.Object });

            //Assert
            encryptorMock.Verify(x => x.Encrypt(It.IsAny<IEnumerable<IFile<Stream>>>()));
        }

        public void ReverseProcess_RenamesFiles_WhenFileParamIsValid()
        {
            //Arrange
            var encryptor = Mock.Of<IEncryptFiles>();
            var processor = new FileProcessor(_processedExtension, encryptor);
            var fileMock = new Mock<IFile<Stream>>();
            fileMock.Setup(m => m.Name).Returns($"TestFile.txt{_processedExtension}");

            //Act
            processor.ReverseProcess(new[] { fileMock.Object });

            //Assert
            fileMock.Verify(x => x.Rename(It.IsAny<string>()));
        }

        [TestMethod]
        public void ReverseProcess_DecryptsFile_WhenFileParamIsValid()
        {
            //Arrange
            var encryptorMock = new Mock<IEncryptFiles>();

            var fileMock = new Mock<IFile<Stream>>();
            fileMock.Setup(m => m.Name).Returns($"TestFile.txt{_processedExtension}");

            var processor = new FileProcessor(_processedExtension, encryptorMock.Object);

            //Act
            processor.ReverseProcess(new[] { fileMock.Object });

            //Assert
            encryptorMock.Verify(x => x.Decrypt(It.IsAny<IEnumerable<IFile<Stream>>>()));
        }
    }
}
