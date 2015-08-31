﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Core.Encryption
{
    public interface IKey
    {
        byte[] GenerateBlock(BlockSize keySize);
    }
}
