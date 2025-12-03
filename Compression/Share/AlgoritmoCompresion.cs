using System;
using System.Collections.Generic;
using System.Text;

namespace Compressor.Compresion.Share
{
    public enum AlgoritmoCompresion : byte
    {
        Huffman = 1,
        LZ77 = 2,
        LZ78 = 3
    }
}