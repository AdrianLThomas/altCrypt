using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Client.CommandLine.Input
{
    [Flags]
    public enum Switch
    {
        None = 0,
        Key = 2,
        Directory = 4,
        File = 8,
    }
}
