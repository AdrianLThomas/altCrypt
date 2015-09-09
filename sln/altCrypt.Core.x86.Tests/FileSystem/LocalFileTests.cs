using System;
using System.IO;
using System.Linq;
using altCrypt.Core.x86.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace altCrypt.Core.x86.UnitTests.FileSystem
{
    [TestClass]
    public class LocalFileTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsEmpty()
        {
            new LocalFile(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsNull()
        {
            new LocalFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Write_ThrowsArgumentNullException_WhenStreamIsNull()
        {
            //Arrange
            var file = new LocalFile(@"C:\TestPath");

            //Act
            file.Write(null); //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Write_ThrowsArgumentException_WhenStreamIsntReadable()
        {
            //Arrange
            Mock<Stream> streamMock = new Mock<Stream>();
            streamMock.Setup(m => m.CanRead).Returns(false);

            Stream stream = streamMock.Object;
            var file = new LocalFile(@"C:\TestPath");

            //Act
            file.Write(stream); //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Rename_ThrowsArgumentNullException_WhenFilenameIsNull()
        {
            //Arrange
            var file = new LocalFile(@"C:\TestPath");

            //Act
            file.Rename(null); //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Rename_ThrowsArgumentNullException_WhenFilenameIsEmpty()
        {
            //Arrange
            var file = new LocalFile(@"C:\TestPath");

            //Act
            file.Rename(string.Empty); //Assert
        }
    }
}
