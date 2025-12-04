using System;
using System.Collections.Generic;
using System.Text;
//para saber que algoritmo se va a usar

namespace Compressor.Compresion.Share
{
    public enum AlgoritmoCompresion : byte
    {
        Huffman = 1,
        LZ77 = 2,
        LZ78 = 3
    }
}