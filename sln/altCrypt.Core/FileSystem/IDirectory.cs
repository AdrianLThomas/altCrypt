using System.Collections.Generic;
using System.IO;

namespace altCrypt.Core.FileSystem
{
    public interface IDirectory
    {
        string Path { get; }
        IEnumerable<IFile> GetFiles();
        IEnumerable<IFile> GetFilesIncludingSubdirectories();
    }
}
