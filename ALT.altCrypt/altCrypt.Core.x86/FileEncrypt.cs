using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Core.x86
{
    public class FileEncrypt : IEncrypt
    {
        public Stream Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            SHA512 shaProvider = System.Security.Cryptography.SHA512.Create();
            return null;
        }
    }
}
