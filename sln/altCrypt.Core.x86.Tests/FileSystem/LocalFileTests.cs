using System;
using System.IO;
using System.Linq;
using altCrypt.Core.x86.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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

            //Act & Assert
            file.Write(stream); //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task WriteAsync_ThrowsArgumentNullException_WhenStreamIsNull()
        {
            //Arrange
            var file = new LocalFile(@"C:\TestPath");

            //Act
            await file.WriteAsync(null); //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task WriteAsync_ThrowsArgumentException_WhenStreamIsntReadable()
        {
            //Arrange
            Mock<Stream> streamMock = new Mock<Stream>();
            streamMock.Setup(m => m.CanRead).Returns(false);

            Stream stream = streamMock.Object;
            var file = new LocalFile(@"C:\TestPath");

            //Act & Assert
            await file.WriteAsync(stream); //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Read_ThrowsFileNotFoundException_WhenFileDoesntExist()
        {
            //Arrange
            var file = new LocalFile($@"C:\{Guid.NewGuid()}.txt");

            //Act & Assert
            file.Read(); //Exception
        }
    }
}
