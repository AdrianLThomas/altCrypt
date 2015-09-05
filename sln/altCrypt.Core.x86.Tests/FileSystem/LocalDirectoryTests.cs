using System;
using altCrypt.Core.x86.FileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace altCrypt.Core.x86.UnitTests.FileSystem
{
    [TestClass]
    public class LocalDirectoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsEmpty()
        {
            new LocalDirectory(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsArgumentNullException_WhenPathIsNull()
        {
            new LocalDirectory(null);
        }
    }
}
