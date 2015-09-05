using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptToStream
    {
        void EncryptToStream(IFile<Stream> file, Stream outputStream);
        void DecryptToStream(IFile<Stream> file, Stream outputStream);
    }
}
