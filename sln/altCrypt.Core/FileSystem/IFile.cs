using System.IO;

namespace altCrypt.Core.FileSystem
{
    public interface IFile<out T> where T : Stream
    {
        string Name { get; }
        void Write(Stream stream);
        T Read();
    }
}
