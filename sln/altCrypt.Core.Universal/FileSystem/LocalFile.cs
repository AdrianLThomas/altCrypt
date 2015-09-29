using altCrypt.Core.FileSystem;
using System;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace altCrypt.Core.Universal.FileSystem
{
    public class LocalFile : IFile
    {
        private readonly string _path;

        public string FilePath => _path;

        public string Name => Path.GetFileName(_path);

        public LocalFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            _path = path;
        }

        public async Task<Stream> ReadAsync()
        {
            var file = await StorageFile.GetFileFromPathAsync(_path);
            //Exception expected if not found?

            return await file.OpenStreamForReadAsync();
        }

        public Stream Read()
        {
            return Task.Run(() => ReadAsync()).GetAwaiter().GetResult();
        }

        public void Write(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Can't read from stream"); //TODO add to strings

            using (var fileHandle = File.OpenWrite(_path)) //TODO copy pasta from LocalFile (code is very similar, can code sharing be achieved with a shared lib?)
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
                throw new ArgumentException("Can't read from stream"); //TODO add to strings

            using (var fileHandle = File.OpenWrite(_path)) //TODO more copy pasta
            {
                fileHandle.SetLength(0);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileHandle);
                fileHandle.Flush();
            }
        }
    }
}
