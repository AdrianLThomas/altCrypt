using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core
{
    public interface IEncryptor
    {
        Stream Encrypt(IFile file);
    }
}
