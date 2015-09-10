using altCrypt.Core.FileSystem;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace altCrypt.Business
{
    public interface IFileProcessor
    {
        Task ProcessAsync(IEnumerable<IFile> files);
        Task ReverseProcessAsync(IEnumerable<IFile> files);
    }
}
