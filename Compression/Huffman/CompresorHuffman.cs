using Compressor.Compresion.Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compressor.Compresion.Huffman
{
    public class CompresorHuffman : ICompresor
    {
        public byte[] Comprimir(byte[] entrada)
        {
            if (entrada == null || entrada.Length == 0)
                return Array.Empty<byte>();

            // 1. Contar frecuencias
            var frecuencias = new Dictionary<byte, int>();
            foreach (var b in entrada)
            {
                if (!frecuencias.ContainsKey(b))
                    frecuencias[b] = 0;
                frecuencias[b]++;
            }

            // 2. Construir árbol con PriorityQueue
            var cola = new PriorityQueue<NodoHuffman, int>();
            foreach (var kv in frecuencias)
            {
                var nodo = new NodoHuffman
                {
                    Simbolo = kv.Key,
                    Frecuencia = kv.Value
                };
                cola.Enqueue(nodo, nodo.Frecuencia);
            }

            while (cola.Count > 1)
            {
                cola.TryDequeue(out var n1, out int f1);
                cola.TryDequeue(out var n2, out int f2);

                var padre = new NodoHuffman
                {
                    Simbolo = null,
                    Frecuencia = f1 + f2,
                    Izquierdo = n1,
                    Derecho = n2
                };

                cola.Enqueue(padre, padre.Frecuencia);
            }

            cola.TryDequeue(out var raiz, out _);

            // 3. Generar códigos
            var codigos = new Dictionary<byte, string>();
            ConstruirCodigos(raiz, "", codigos);

            // 4. Guardar frecuencias + bits
            using var ms = new MemoryStream();
            using (var escritor = new BinaryWriter(ms, Encoding.UTF8, true))
            {
                // Cantidad de símbolos
                escritor.Write(frecuencias.Count);
                foreach (var kv in frecuencias)
                {
                    escritor.Write(kv.Key);    // símbolo
                    escritor.Write(kv.Value);  // frecuencia
                }

                // Codificar datos
                var cadenaBits = new StringBuilder();
                foreach (var b in entrada)
                {
                    cadenaBits.Append(codigos[b]);
                }

                // Bits -> bytes
                byte byteActual = 0;
                int conteoBits = 0;
                var listaBytes = new List<byte>();

                foreach (char bit in cadenaBits.ToString())
                {
                    byteActual <<= 1;
                    if (bit == '1')
                        byteActual |= 1;

                    conteoBits++;

                    if (conteoBits == 8)
                    {
                        listaBytes.Add(byteActual);
                        byteActual = 0;
                        conteoBits = 0;
                    }
                }

                int relleno = 0;
                if (conteoBits > 0)
                {
                    byteActual <<= (8 - conteoBits);
                    listaBytes.Add(byteActual);
                    relleno = 8 - conteoBits;
                }

                escritor.Write((byte)relleno);
                escritor.Write(listaBytes.Count);
                escritor.Write(listaBytes.ToArray());
            }

            return ms.ToArray();
        }

        private void ConstruirCodigos(NodoHuffman nodo, string prefijo, Dictionary<byte, string> codigos)
        {
            if (nodo.EsHoja)
            {
                codigos[nodo.Simbolo.Value] = prefijo.Length > 0 ? prefijo : "0";
                return;
            }

            ConstruirCodigos(nodo.Izquierdo, prefijo + "0", codigos);
            ConstruirCodigos(nodo.Derecho, prefijo + "1", codigos);
        }

        public byte[] Descomprimir(byte[] entrada)
        {
            if (entrada == null || entrada.Length == 0)
                return Array.Empty<byte>();

            using var ms = new MemoryStream(entrada);
            using var lector = new BinaryReader(ms, Encoding.UTF8);

            int cantidadSimbolos = lector.ReadInt32();
            var frecuencias = new Dictionary<byte, int>();

            for (int i = 0; i < cantidadSimbolos; i++)
            {
                byte simbolo = lector.ReadByte();
                int frecuencia = lector.ReadInt32();
                frecuencias[simbolo] = frecuencia;
            }

            // Reconstruir árbol
            var cola = new PriorityQueue<NodoHuffman, int>();
            foreach (var kv in frecuencias)
            {
                var nodo = new NodoHuffman
                {
                    Simbolo = kv.Key,
                    Frecuencia = kv.Value
                };
                cola.Enqueue(nodo, nodo.Frecuencia);
            }

            while (cola.Count > 1)
            {
                cola.TryDequeue(out var n1, out int f1);
                cola.TryDequeue(out var n2, out int f2);

                var padre = new NodoHuffman
                {
                    Simbolo = null,
                    Frecuencia = f1 + f2,
                    Izquierdo = n1,
                    Derecho = n2
                };

                cola.Enqueue(padre, padre.Frecuencia);
            }

            cola.TryDequeue(out var raiz, out _);

            byte rellenoFinal = lector.ReadByte();
            int longitudDatos = lector.ReadInt32();
            byte[] datosBytes = lector.ReadBytes(longitudDatos);

            // Bytes -> bits
            var bits = new StringBuilder();
            foreach (var b in datosBytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    bits.Append(((b >> i) & 1) == 1 ? '1' : '0');
                }
            }

            if (rellenoFinal > 0)
            {
                bits.Length -= rellenoFinal;
            }

            var resultado = new List<byte>();
            var actual = raiz;

            foreach (char bit in bits.ToString())
            {
                actual = (bit == '0') ? actual.Izquierdo : actual.Derecho;

                if (actual.EsHoja)
                {
                    resultado.Add(actual.Simbolo.Value);
                    actual = raiz;
                }
            }

            return resultado.ToArray();
        }
    }
}