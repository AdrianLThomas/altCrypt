using System.IO;
using System.Threading.Tasks;

namespace altCrypt.Core.FileSystem
{
    public interface IFile
    {
        string Name { get; }
        string FilePath { get; }
        void Write(Stream stream);
        Task WriteAsync(Stream stream);
        Stream Read();
        void Rename(string newFilename);
    }
}
