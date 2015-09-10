using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptToStream
    {
        void EncryptToStream(IFile file, Stream outputStream);
        void DecryptToStream(IFile file, Stream outputStream);
    }
}
