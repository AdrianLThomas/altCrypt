using System.Collections.Generic;
using System.IO;
using altCrypt.Core.FileSystem;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptFiles
    {
        void Encrypt(IEnumerable<IFile> files);
        void Decrypt(IEnumerable<IFile> files);
    }
}
