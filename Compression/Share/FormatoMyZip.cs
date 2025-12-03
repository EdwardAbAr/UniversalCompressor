using System;
using System.Collections.Generic;
using System.Text;

namespace Compressor.Compresion.Share
{
    public static class FormatoMyZip
    {
        public static void EscribirEntrada(
            BinaryWriter escritor,
            AlgoritmoCompresion algoritmo,
            string nombreArchivo,
            byte[] datosOriginales,
            byte[] datosComprimidos)
        {
            // 1 byte: algoritmo
            escritor.Write((byte)algoritmo);

            // Nombre de archivo
            byte[] nombreBytes = Encoding.UTF8.GetBytes(nombreArchivo);
            escritor.Write(nombreBytes.Length);
            escritor.Write(nombreBytes);

            // Tamaños
            escritor.Write((long)datosOriginales.Length);
            escritor.Write((long)datosComprimidos.Length);

            // Datos comprimidos
            escritor.Write(datosComprimidos);
        }

        public static bool IntentarLeerEntrada(
            BinaryReader lector,
            out AlgoritmoCompresion algoritmo,
            out string nombreArchivo,
            out long longitudOriginal,
            out byte[] datosComprimidos)
        {
            algoritmo = 0;
            nombreArchivo = null;
            longitudOriginal = 0;
            datosComprimidos = null;

            if (lector.BaseStream.Position >= lector.BaseStream.Length)
            {
                return false;
            }

            try
            {
                algoritmo = (AlgoritmoCompresion)lector.ReadByte();

                int longitudNombre = lector.ReadInt32();
                byte[] nombreBytes = lector.ReadBytes(longitudNombre);
                nombreArchivo = Encoding.UTF8.GetString(nombreBytes);

                longitudOriginal = lector.ReadInt64();
                long longitudComprimida = lector.ReadInt64();

                datosComprimidos = lector.ReadBytes((int)longitudComprimida);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}