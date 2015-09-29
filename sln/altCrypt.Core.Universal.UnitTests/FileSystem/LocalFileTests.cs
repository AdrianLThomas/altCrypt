using altCrypt.Core.Universal.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace altCrypt.Core.Universal.UnitTests.FileSystem
{
    [TestClass]
    public class LocalFileTests
    {
        //TODO - copy pasta from x86 libs. Could reuse here in a shared lib too?

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

        //TODO - Mocks in universal apps?
        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void Write_ThrowsArgumentException_WhenStreamIsntReadable()
        //{
        //    //Arrange
        //    Mock<Stream> streamMock = new Mock<Stream>();
        //    streamMock.Setup(m => m.CanRead).Returns(false);

        //    Stream stream = streamMock.Object;
        //    var file = new LocalFile(@"C:\TestPath");

        //    //Act & Assert
        //    file.Write(stream); //Exception
        //}

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Read_ThrowsFileNotFoundException_WhenFileDoesntExist()
        {
            //Arrange
            var file = new LocalFile($@"C:\{Guid.NewGuid()}.txt");

            //Act & Assert
            file.Read(); //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task ReadAsync_ThrowsFileNotFoundException_WhenFileDoesntExist()
        {
            //Arrange
            var file = new LocalFile($@"C:\{Guid.NewGuid()}.txt");

            //Act & Assert
            await file.ReadAsync(); //Exception
        }
    }
}
