using altCrypt.Core.FileSystem;
using System;
using System.IO;

namespace altCrypt.Core
{
    public class FileEncrypt : IEncrypt
    {
        public Stream Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            //SHA512 shaProvider = System.Security.Cryptography.SHA512CryptoServiceProvider.Create();
            return null;
        }
    }
}
