using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptor
    {
        Stream Encrypt(IFile file);
        Stream Decrypt(IFile file);
    }
}
