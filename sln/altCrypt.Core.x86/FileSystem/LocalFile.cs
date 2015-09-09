using altCrypt.Core.FileSystem;
using System;
using System.IO;

namespace altCrypt.Core.x86.FileSystem
{
    public class LocalFile : IFile<FileStream>
    {
        private string _path;
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
                throw new ArgumentException("Can't read from stream");
            
            using (var fileHandle = File.OpenWrite(_path))
            {
                fileHandle.SetLength(0);
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileHandle);
                fileHandle.Flush();
            }
        }

        public FileStream Read() => File.OpenRead(_path);

        public void Rename(string newFilename)
        {
            if (string.IsNullOrEmpty(newFilename))
                throw new ArgumentNullException(nameof(newFilename));

            string directory = Path.GetDirectoryName(_path);
            string destination = Path.Combine(directory, newFilename);
            File.Move(_path, destination);

            _path = destination;
        }
    }
}
