using System.IO;

namespace altCrypt.Core.FileSystem
{
    public interface IFile
    {
        string Name { get; }
        Stream Data { get; }
    }
}
