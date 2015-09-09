using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using altCrypt.Core.FileSystem;
using altCrypt.Core.Encryption;

namespace altCrypt.Business.UnitTests
{
    [TestClass]
    public class FileProcessorTests
    {
        private readonly string _processedExtension = ".altCryptTest";

        [TestMethod]
        public void ProcessFile_RenamesFile_WhenFileParamIsValid()
        {
            //Arrange
            var encryptor = Mock.Of<IEncryptFile>();
            var processor = new FileProcessor(_processedExtension, encryptor);
            var fileMock = new Mock<IFile<Stream>>();
            fileMock.Setup(m => m.Name).Returns("TestFile.txt");
         
            //Act
            processor.Process(fileMock.Object);

            //Assert
            fileMock.Verify(x => x.Rename(It.IsAny<string>()));
        }

        [TestMethod]
        public void ProcessFile_EncryptsFile_WhenFileParamIsValid()
        {
            //Arrange
            var encryptorMock = new Mock<IEncryptFile>();

            var fileMock = new Mock<IFile<Stream>>();
            fileMock.Setup(m => m.Name).Returns("TestFile.txt");

            var processor = new FileProcessor(_processedExtension, encryptorMock.Object);

            //Act
            processor.Process(fileMock.Object);

            //Assert
            encryptorMock.Verify(x => x.Encrypt(It.IsAny<IFile<Stream>>()));
        }
    }
}
