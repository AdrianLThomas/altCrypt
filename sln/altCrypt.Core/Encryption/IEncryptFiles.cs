using System.Collections.Generic;
using System.IO;
using altCrypt.Core.FileSystem;
using System.Threading.Tasks;

namespace altCrypt.Core.Encryption
{
    public interface IEncryptFiles
    {
        Task EncryptAsync(IEnumerable<IFile> files);
        Task DecryptAsync(IEnumerable<IFile> files);
    }
}
