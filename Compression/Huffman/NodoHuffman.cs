using System;
using System.Collections.Generic;
using System.Text;

namespace Compressor.Compresion.Huffman
{
    public class NodoHuffman : IComparable<NodoHuffman>
    {
        public byte? Simbolo { get; set; }
        public int Frecuencia { get; set; }
        public NodoHuffman Izquierdo { get; set; }
        public NodoHuffman Derecho { get; set; }

        public bool EsHoja => Simbolo.HasValue;

        public int CompareTo(NodoHuffman otro)
        {
            return Frecuencia.CompareTo(otro.Frecuencia);
        }
    }
}