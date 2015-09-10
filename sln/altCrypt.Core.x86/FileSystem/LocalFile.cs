using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Strings;
using System;
using System.IO;
using System.Threading.Tasks;

namespace altCrypt.Core.x86.FileSystem
{
    public class LocalFile : IFile
    {
        private readonly string _path;

        public string Name => Path.GetFileName(_path);
        public string FilePath => _path;

        public LocalFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            _path = path;
        }

        public void Write(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException(ExceptionMessages.CantReadFromStream);
            
            using (var fileHandle = File.OpenWrite(_path))
            {
                fileHandle.SetLength(0);
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileHandle);
                fileHandle.Flush();
            }
        }
        public async Task WriteAsync(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException(ExceptionMessages.CantReadFromStream);

            using (var fileHandle = File.OpenWrite(_path))
            {
                fileHandle.SetLength(0);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileHandle);
                fileHandle.Flush();
            }
        }

        public Stream Read() => File.OpenRead(_path);
    }
}
