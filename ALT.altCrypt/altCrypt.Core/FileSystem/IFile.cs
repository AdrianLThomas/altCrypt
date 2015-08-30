using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Core.FileSystem
{
    public interface IFile
    {
        string Name { get; }
        Stream Data { get; }
    }
}
