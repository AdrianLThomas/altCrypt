using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptToStream
    {
        Stream EncryptToStream(IFile<Stream> file);
        Stream DecryptToStream(IFile<Stream> file);
    }
}
