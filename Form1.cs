using Compressor.Compresion.Huffman;
using Compressor.Compresion.LZ77;
using Compressor.Compresion.LZ78;
using Compressor.Compresion.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Compressor
{
    public partial class Form1 : Form
    {
        private List<string> _archivosSeleccionados = new List<string>();

        public Form1()
        {
            InitializeComponent();
            InicializarComboAlgoritmo();
        }

        private void InicializarComboAlgoritmo()
        {
            cmbAlgoritmo.Items.Clear();
            cmbAlgoritmo.Items.Add("Huffman");
            cmbAlgoritmo.Items.Add("LZ77");
            cmbAlgoritmo.Items.Add("LZ78");
            cmbAlgoritmo.SelectedIndex = 0;
        }

        private void btnSeleccionarArchivos_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de texto|*.txt|Todos los archivos|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _archivosSeleccionados.Clear();
                _archivosSeleccionados.AddRange(ofd.FileNames);

                lstArchivos.Items.Clear();
                foreach (var archivo in _archivosSeleccionados)
                {
                    lstArchivos.Items.Add(archivo);
                }
            }
        }

        private ICompresor ObtenerCompresorSeleccionado()
        {
            string seleccionado = cmbAlgoritmo.SelectedItem?.ToString() ?? "Huffman";

            return seleccionado switch
            {
                "Huffman" => new CompresorHuffman(),
                "LZ77" => new CompresorLZ77(),
                "LZ78" => new CompresorLZ78(),
                _ => new CompresorHuffman()
            };
        }

        private AlgoritmoCompresion ObtenerAlgoritmoEnum()
        {
            string seleccionado = cmbAlgoritmo.SelectedItem?.ToString() ?? "Huffman";

            return seleccionado switch
            {
                "Huffman" => AlgoritmoCompresion.Huffman,
                "LZ77" => AlgoritmoCompresion.LZ77,
                "LZ78" => AlgoritmoCompresion.LZ78,
                _ => AlgoritmoCompresion.Huffman
            };
        }

        private void btnComprimir_Click(object sender, EventArgs e)
        {
            if (_archivosSeleccionados.Count == 0)
            {
                MessageBox.Show(
                    "Por favor seleccione al menos un archivo.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "Archivo comprimido|*.CE";

            // Nombre  para el comprimido
            string nombreBase;

            if (_archivosSeleccionados.Count == 1)
            {
                nombreBase = Path.GetFileNameWithoutExtension(_archivosSeleccionados[0]);
            }
            else
            {
                    //Por si hay varios archivos
                nombreBase = Path.GetFileNameWithoutExtension(_archivosSeleccionados[0]) + "_varios";
            }

            sfd.FileName = nombreBase + ".CE";

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            var compresor = ObtenerCompresorSeleccionado();
            var algoritmoEnum = ObtenerAlgoritmoEnum();

            long totalOriginal = 0;
            long totalComprimido = 0;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long memoriaAntes = GC.GetTotalMemory(true);

            var reloj = Stopwatch.StartNew();

            using (var fs = new FileStream(sfd.FileName, FileMode.Create))
            using (var escritor = new BinaryWriter(fs))
            {
                foreach (var archivo in _archivosSeleccionados)
                {
                    byte[] datosOriginales = File.ReadAllBytes(archivo);
                    byte[] datosComprimidos = compresor.Comprimir(datosOriginales);

                    totalOriginal += datosOriginales.Length;
                    totalComprimido += datosComprimidos.Length;

                    string nombreSolo = Path.GetFileName(archivo);

                    FormatoCE.EscribirEntrada(
                        escritor,
                        algoritmoEnum,
                        nombreSolo,
                        datosOriginales,
                        datosComprimidos);
                }
            }

            reloj.Stop();
            long memoriaDespues = GC.GetTotalMemory(true);

            var estadisticas = new EstadisticasCompresion
            {
                TiempoTranscurrido = reloj.Elapsed,
                MemoriaUsadaBytes = memoriaDespues - memoriaAntes,
                TasaCompresion = totalOriginal > 0
                    ? (double)totalComprimido / totalOriginal
                    : 0.0
            };

            lblTiempo.Text = $"Tiempo: {estadisticas.TiempoTranscurrido.TotalMilliseconds:F2} ms";
            lblMemoria.Text = $"Memoria: {estadisticas.MemoriaUsadaBytes} bytes";
            lblTasa.Text = $"Tasa: {estadisticas.TasaCompresion:P2}";
        }

        private void btnDescomprimir_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Archivo comprimido|*.CE";

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(
                    "Por favor seleccione un archivo .CE para descomprimir.",
                    "Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string archivoEntrada = ofd.FileName;

            // Carpeta de salida
            string carpetaSalida = Path.Combine(
                Path.GetDirectoryName(archivoEntrada),
                Path.GetFileNameWithoutExtension(archivoEntrada) + "_out");

            Directory.CreateDirectory(carpetaSalida);

            // Acumuladores para estadísticas
            long totalOriginal = 0;
            long totalComprimido = 0;

            // Medir memoria/tiempo
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long memoriaAntes = GC.GetTotalMemory(true);

            var reloj = Stopwatch.StartNew();

            using (var fs = new FileStream(archivoEntrada, FileMode.Open))
            using (var lector = new BinaryReader(fs))
            {
                while (FormatoCE.IntentarLeerEntrada(
                           lector,
                           out AlgoritmoCompresion algoritmo,
                           out string nombreArchivo,
                           out long longitudOriginal,
                           out byte[] datosComprimidos))
                {
                    ICompresor compresor = algoritmo switch
                    {
                        AlgoritmoCompresion.Huffman => new CompresorHuffman(),
                        AlgoritmoCompresion.LZ77 => new CompresorLZ77(),
                        AlgoritmoCompresion.LZ78 => new CompresorLZ78(),
                        _ => new CompresorHuffman()
                    };

                    byte[] datosDescomprimidos = compresor.Descomprimir(datosComprimidos);

                    string rutaSalida = Path.Combine(carpetaSalida, nombreArchivo);
                    File.WriteAllBytes(rutaSalida, datosDescomprimidos);


                    totalOriginal += longitudOriginal;
                    totalComprimido += datosComprimidos.Length;
                }
            }

            reloj.Stop();
            long memoriaDespues = GC.GetTotalMemory(true);

            var estadisticas = new EstadisticasCompresion
            {
                TiempoTranscurrido = reloj.Elapsed,
                MemoriaUsadaBytes = memoriaDespues - memoriaAntes,
                TasaCompresion = totalOriginal > 0
                    ? (double)totalComprimido / totalOriginal
                    : 0.0
            };

            // Mostrar estadísticas
            lblTiempo.Text = $"Tiempo (descompresión): {estadisticas.TiempoTranscurrido.TotalMilliseconds:F2} ms";
            lblMemoria.Text = $"Memoria (descompresión): {estadisticas.MemoriaUsadaBytes} bytes";
            lblTasa.Text = $"Tasa: {estadisticas.TasaCompresion:P2}";

            MessageBox.Show($"Archivos descomprimidos en: {carpetaSalida}");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblTasa_Click(object sender, EventArgs e)
        {

        }
    }
}
