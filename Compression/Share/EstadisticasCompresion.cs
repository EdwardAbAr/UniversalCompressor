using System;
using System.Collections.Generic;
using System.Text;

namespace Compressor.Compresion.Share
{
    public class EstadisticasCompresion
    {
        public TimeSpan TiempoTranscurrido { get; set; }
        public long MemoriaUsadaBytes { get; set; }
        public double TasaCompresion { get; set; }
    }
}