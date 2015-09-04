using altCrypt.Core.FileSystem;
using System;
using System.IO;

namespace altCrypt.Core.x86.FileSystem
{
    public class LocalFile : IFile<FileStream>
    {
        private readonly string _path;
        private FileStream _data;

        public string Name => Path.GetFileName(_path);

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

            GetData().SetLength(0);
            stream.CopyTo(_data);
            GetData().Flush();
        }

        public FileStream Read() => GetData();

        private FileStream GetData()
        {
            return _data ?? (_data = File.OpenRead(_path));
        }
    }
}
