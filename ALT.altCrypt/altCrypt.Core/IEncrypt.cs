using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Core
{
    public interface IEncrypt
    {
        Stream Encrypt(IFile file);
    }
}
