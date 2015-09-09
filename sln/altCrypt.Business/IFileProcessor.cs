using altCrypt.Core.FileSystem;
using System.IO;

namespace altCrypt.Business
{
    public interface IFileProcessor
    {
        void Process(IFile<Stream> file);
    }
}
