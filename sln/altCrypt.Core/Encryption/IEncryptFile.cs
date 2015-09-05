using System.IO;
using altCrypt.Core.FileSystem;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptFile
    {
        void Encrypt(IFile<Stream> file);
        void Decrypt(IFile<Stream> file);
    }
}
