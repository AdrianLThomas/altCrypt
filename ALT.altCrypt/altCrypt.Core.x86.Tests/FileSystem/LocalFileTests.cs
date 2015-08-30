using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace altCrypt.Core.Tests.x86.FileSystem
{
    [TestClass]
    public class LocalFileTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsEmpty()
        {
            //Arrange
            //Act
            var file = new LocalFile(string.Empty);

            //Assert
            //Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsNull()
        {
            //Arrange
            //Act
            var file = new LocalFile(null);

            //Assert
            //Exception
        }
    }
}
