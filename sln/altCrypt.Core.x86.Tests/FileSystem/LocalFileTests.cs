using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;

namespace altCrypt.Core.UnitTests.x86.FileSystem
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
    }
}
