using altCrypt.Core.FileSystem;
using System.Collections.Generic;
using System.IO;

namespace altCrypt.Business
{
    public interface IFileProcessor
    {
        void Process(IEnumerable<IFile<Stream>> files);
        void ReverseProcess(IEnumerable<IFile<Stream>> files);
    }
}
