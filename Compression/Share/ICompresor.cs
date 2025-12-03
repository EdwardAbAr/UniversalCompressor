using System;
using System.Collections.Generic;
using System.Text;

namespace Compressor.Compresion.Share
{
    public interface ICompresor
    {
        byte[] Comprimir(byte[] entrada);
        byte[] Descomprimir(byte[] entrada);
    }
}
