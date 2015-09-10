using System;
using System.IO;
using System.Linq;
using altCrypt.Core.x86.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace altCrypt.Core.x86.IntegrationTests.FileSystem
{
    [TestClass]
    public class LocalFileTests
    {
        private readonly string _path = @".\TestData.txt";

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(_path);
        }

        [TestMethod]
        public void Write_WritesText123_ToFile()
        {
            //Arrange
            string expected = "123";
            string actual;
            var file = new LocalFile(_path);
            byte[] text = expected.Select(Convert.ToByte).ToArray();

            //Act
            using (var memoryStream = new MemoryStream(text))
            {
                file.Write(memoryStream);
            }

            actual = File.ReadAllText(_path);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task WriteAsync_WritesText123_ToFile()
        {
            //Arrange
            string expected = "123";
            string actual;
            var file = new LocalFile(_path);
            byte[] text = expected.Select(Convert.ToByte).ToArray();

            //Act
            using (var memoryStream = new MemoryStream(text))
            {
                await file.WriteAsync(memoryStream);
            }

            actual = File.ReadAllText(_path);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Read_ReadsText123_FromFile()
        {
            //Arrange
            string expected = "123";
            string actual;
            var file = new LocalFile(_path);
            File.WriteAllText(_path, expected);

            //Act
            using (Stream myFile = file.Read())
            {
                using (var reader = new StreamReader(myFile))
                {
                    actual = reader.ReadToEnd();
                }
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Rename_ChangesFilename_ToFilenameParameter()
        {
            //Arrange
            string filename = "FileName.testdata";
            File.Create(filename).Dispose();

            string newFilename = Path.GetFileName(_path);
            var file = new LocalFile(filename);
            
            //Act
            file.Rename(newFilename);

            //Assert
            Assert.AreEqual(newFilename, file.Name);
        }
    }
}
