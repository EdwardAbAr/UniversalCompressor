using System;
using System.Collections.Generic;
using System.IO;
using Compressor.Compresion.Share;


namespace Compressor.Compresion.LZ77
{
    public class CompresorLZ77 : ICompresor
    {
        private const int TamVentana = 4096;

        public byte[] Comprimir(byte[] entrada)
        {
            if (entrada == null || entrada.Length == 0)
                return Array.Empty<byte>();

            var tokens = new List<TokenLZ77>();
            int pos = 0;
            int longitud = entrada.Length;

            while (pos < longitud)
            {
                int mejorLongitud = 0;
                int mejorDesplazamiento = 0;

                int inicioVentana = Math.Max(0, pos - TamVentana);

                for (int j = inicioVentana; j < pos; j++)
                {
                    int lenActual = 0;

                    while (pos + lenActual < longitud &&
                           entrada[j + lenActual] == entrada[pos + lenActual])
                    {
                        lenActual++;

                        if (lenActual >= ushort.MaxValue)
                            break;
                    }

                    if (lenActual > mejorLongitud)
                    {
                        mejorLongitud = lenActual;
                        mejorDesplazamiento = pos - j;
                    }
                }

                if (mejorLongitud > 0 && (pos + mejorLongitud) < longitud)
                {
                    byte siguiente = entrada[pos + mejorLongitud];

                    tokens.Add(new TokenLZ77
                    {
                        Desplazamiento = (ushort)mejorDesplazamiento,
                        Longitud = (ushort)mejorLongitud,
                        SiguienteByte = siguiente
                    });

                    pos += mejorLongitud + 1;
                }
                else
                {
                    tokens.Add(new TokenLZ77
                    {
                        Desplazamiento = 0,
                        Longitud = 0,
                        SiguienteByte = entrada[pos]
                    });

                    pos++;
                }
            }

            using var ms = new MemoryStream();
            using (var escritor = new BinaryWriter(ms))
            {
                escritor.Write(tokens.Count);

                foreach (var t in tokens)
                {
                    escritor.Write(t.Desplazamiento);
                    escritor.Write(t.Longitud);
                    escritor.Write(t.SiguienteByte);
                }
            }

            return ms.ToArray();
        }

        public byte[] Descomprimir(byte[] entrada)
        {
            if (entrada == null || entrada.Length == 0)
                return Array.Empty<byte>();

            var salida = new List<byte>();

            using var ms = new MemoryStream(entrada);
            using var lector = new BinaryReader(ms);

            int cantidadTokens = lector.ReadInt32();

            for (int i = 0; i < cantidadTokens; i++)
            {
                ushort desplazamiento = lector.ReadUInt16();
                ushort longitud = lector.ReadUInt16();
                byte siguiente = lector.ReadByte();

                if (desplazamiento > 0 && longitud > 0)
                {
                    int inicio = salida.Count - desplazamiento;

                    for (int k = 0; k < longitud; k++)
                    {
                        salida.Add(salida[inicio + k]);
                    }
                }

                salida.Add(siguiente);
            }

            return salida.ToArray();
        }

        private struct TokenLZ77
        {
            public ushort Desplazamiento;
            public ushort Longitud;
            public byte SiguienteByte;
        }
    }
}