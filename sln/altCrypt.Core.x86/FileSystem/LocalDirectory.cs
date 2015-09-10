using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using altCrypt.Core.FileSystem;

namespace altCrypt.Core.x86.FileSystem
{
    public class LocalDirectory : IDirectory
    {
        public string Path { get; }

        public LocalDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            Path = path;
        }

        public IEnumerable<IFile> GetFiles() => GetFiles(SearchOption.TopDirectoryOnly);

        public IEnumerable<IFile> GetFilesIncludingSubdirectories() => GetFiles(SearchOption.AllDirectories);

        private IEnumerable<IFile> GetFiles(SearchOption searchOption)
        {
            string[] files = Directory.GetFiles(Path, "*.*", searchOption);

            return files.Select(filePath => new LocalFile(filePath));
        }
    }
}
