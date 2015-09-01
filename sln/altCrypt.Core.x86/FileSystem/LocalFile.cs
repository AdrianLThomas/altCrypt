using altCrypt.Core.FileSystem;
using System;
using System.IO;

namespace altCrypt.Core.x86.FileSystem
{
    public class LocalFile : IFile<FileStream>
    {
        private readonly string _path;

        public string Name
        {
            get
            { 
                return Path.GetFileName(_path);
            }
        }

        public FileStream Data
        {
            get
            {
                return File.OpenRead(_path);
            }
        }

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
            if (!Data.CanWrite)
                throw new InvalidOperationException($"Can't write to file: {_path}");
                
            Data.SetLength(0);
            stream.CopyTo(Data);
            Data.Flush();
        }
    }
}
