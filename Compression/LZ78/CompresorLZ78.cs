using Compressor.Compresion.Share;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compressor.Compresion.LZ78
{
    public class CompresorLZ78 : ICompresor
    {
        public byte[] Comprimir(byte[] entrada)
        {
            if (entrada == null || entrada.Length == 0)
                return Array.Empty<byte>();

            var diccionario = new Dictionary<string, ushort>();
            diccionario[string.Empty] = 0;

            var tokens = new List<TokenLZ78>();
            int siguienteIndice = 1;

            int pos = 0;
            int longitud = entrada.Length;

            while (pos < longitud)
            {
                string actual = string.Empty;
                ushort ultimoIndice = 0;

                while (pos < longitud)
                {
                    string candidato = actual + (char)entrada[pos];

                    if (diccionario.TryGetValue(candidato, out ushort idx))
                    {
                        actual = candidato;
                        ultimoIndice = idx;
                        pos++;
                    }
                    else
                    {
                        byte siguiente = entrada[pos];

                        tokens.Add(new TokenLZ78
                        {
                            Indice = ultimoIndice,
                            SiguienteByte = siguiente
                        });

                        diccionario[candidato] = (ushort)siguienteIndice;
                        siguienteIndice++;

                        pos++;
                        break;
                    }
                }

                if (pos == longitud && !string.IsNullOrEmpty(actual))
                {
                    tokens.Add(new TokenLZ78
                    {
                        Indice = ultimoIndice,
                        SiguienteByte = 0
                    });
                }
            }

            using var ms = new MemoryStream();
            using (var escritor = new BinaryWriter(ms, Encoding.UTF8, true))
            {
                escritor.Write(tokens.Count);

                foreach (var t in tokens)
                {
                    escritor.Write(t.Indice);
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
            using var lector = new BinaryReader(ms, Encoding.UTF8);

            int cantidadTokens = lector.ReadInt32();

            var diccionario = new List<byte[]>();
            diccionario.Add(Array.Empty<byte>()); // índice 0 = vacío

            for (int i = 0; i < cantidadTokens; i++)
            {
                ushort indice = lector.ReadUInt16();
                byte siguiente = lector.ReadByte();

                byte[] fraseBase = diccionario[indice];
                byte[] nuevaEntrada;

                if (siguiente == 0 && fraseBase.Length > 0)
                {
                    nuevaEntrada = fraseBase;
                }
                else
                {
                    nuevaEntrada = new byte[fraseBase.Length + 1];
                    Buffer.BlockCopy(fraseBase, 0, nuevaEntrada, 0, fraseBase.Length);
                    nuevaEntrada[fraseBase.Length] = siguiente;
                }

                diccionario.Add(nuevaEntrada);
                salida.AddRange(nuevaEntrada);
            }

            return salida.ToArray();
        }

        private struct TokenLZ78
        {
            public ushort Indice;
            public byte SiguienteByte;
        }
    }
}