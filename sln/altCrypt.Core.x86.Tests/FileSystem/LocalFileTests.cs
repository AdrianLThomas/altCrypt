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
            new LocalFile(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsNull()
        {
            new LocalFile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Write_ThrowsInvalidOperationException_WhenFileDataIsntWritable()
        {
            throw new NotImplementedException();
        }
    }
}
