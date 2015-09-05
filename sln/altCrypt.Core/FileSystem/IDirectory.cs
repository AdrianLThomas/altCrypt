﻿using System.Collections.Generic;
using System.IO;

namespace altCrypt.Core.FileSystem
{
    public interface IDirectory<T> where T : Stream
    {
        string Path { get; }
        IEnumerable<IFile<T>> GetFiles();
        IEnumerable<IFile<T>> GetFilesIncludingSubdirectories();
    }
}
