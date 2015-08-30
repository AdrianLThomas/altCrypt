﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Core.FileSystem
{
    public class LocalFile : IFile
    {
        private readonly string _path;

        public Stream Data
        {
            get
            {
                return File.OpenRead(_path);
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileName(_path);
            }
        }

        public LocalFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            _path = path;
        }
    }
}
