using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptor
    {
        Stream EncryptToStream(IFile<Stream> file);
        Stream DecryptToStream(IFile<Stream> file);
        void Encrypt(IFile<Stream> file);
    }
}
