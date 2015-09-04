using System.IO;

namespace altCrypt.Core.FileSystem
{
    public interface IFile<out T> where T : Stream
    {
        string Name { get; }
        T Data { get; }
        void Write(Stream stream);
    }
}
