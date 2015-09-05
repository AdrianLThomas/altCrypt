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

        [TestMethod] //TODO - This is an integration test. Seperate out.
        public void Write_WritesText123_ToFile()
        {
            //Arrange
            string expected = "123";
            string actual;
            string path = @"C:\temp\MyFile.txt";
            var file = new LocalFile(path);
            byte[] text = expected.Select(Convert.ToByte).ToArray();

            //Act
            using (var memoryStream = new MemoryStream(text))
            {
                file.Write(memoryStream);
            }

            actual = File.ReadAllText(path);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod] //TODO - This is an integration test. Seperate out.
        public void Read_ReadsText123_FromFile()
        {
            //Arrange
            string expected = "123";
            string actual;
            string path = @"C:\temp\MyFile.txt";
            var file = new LocalFile(path);
            File.WriteAllText(path, expected);

            //Act
            using (FileStream myFile = file.Read())
            {
                using (var reader = new StreamReader(myFile))
                {
                    actual = reader.ReadToEnd();
                }
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
