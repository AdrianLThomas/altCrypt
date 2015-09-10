using System.IO;
using altCrypt.Core.FileSystem;
using System.Threading.Tasks;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptFile
    {
        Task EncryptAsync(IFile file);
        Task DecryptAsync(IFile file);
    }
}
