using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptor
    {
        Stream EncryptToStream(IFile file);
        Stream DecryptToStream(IFile file);
        void Encrypt(IFile file);
    }
}
