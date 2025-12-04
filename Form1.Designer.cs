namespace Compressor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSeleccionarArchivos = new Button();
            lstArchivos = new ListBox();
            cmbAlgoritmo = new ComboBox();
            btnComprimir = new Button();
            btnDescomprimir = new Button();
            lblTiempo = new Label();
            lblMemoria = new Label();
            lblTasa = new Label();
            labeltipo = new Label();
            SuspendLayout();
            // 
            // btnSeleccionarArchivos
            // 
            btnSeleccionarArchivos.Location = new Point(56, 161);
            btnSeleccionarArchivos.Name = "btnSeleccionarArchivos";
            btnSeleccionarArchivos.Size = new Size(160, 45);
            btnSeleccionarArchivos.TabIndex = 0;
            btnSeleccionarArchivos.Text = "Seleccionar Archivo";
            btnSeleccionarArchivos.UseVisualStyleBackColor = true;
            btnSeleccionarArchivos.Click += btnSeleccionarArchivos_Click;
            // 
            // lstArchivos
            // 
            lstArchivos.FormattingEnabled = true;
            lstArchivos.Location = new Point(29, 32);
            lstArchivos.Name = "lstArchivos";
            lstArchivos.Size = new Size(224, 94);
            lstArchivos.TabIndex = 1;
            // 
            // cmbAlgoritmo
            // 
            cmbAlgoritmo.FormattingEnabled = true;
            cmbAlgoritmo.Location = new Point(319, 73);
            cmbAlgoritmo.Name = "cmbAlgoritmo";
            cmbAlgoritmo.Size = new Size(121, 23);
            cmbAlgoritmo.TabIndex = 2;
            // 
            // btnComprimir
            // 
            btnComprimir.Location = new Point(56, 230);
            btnComprimir.Name = "btnComprimir";
            btnComprimir.Size = new Size(160, 47);
            btnComprimir.TabIndex = 3;
            btnComprimir.Text = "Comprimir";
            btnComprimir.UseVisualStyleBackColor = true;
            btnComprimir.Click += btnComprimir_Click;
            // 
            // btnDescomprimir
            // 
            btnDescomprimir.Location = new Point(56, 296);
            btnDescomprimir.Name = "btnDescomprimir";
            btnDescomprimir.Size = new Size(160, 51);
            btnDescomprimir.TabIndex = 4;
            btnDescomprimir.Text = "Descomprimir";
            btnDescomprimir.UseVisualStyleBackColor = true;
            btnDescomprimir.Click += btnDescomprimir_Click;
            // 
            // lblTiempo
            // 
            lblTiempo.AutoSize = true;
            lblTiempo.Location = new Point(307, 176);
            lblTiempo.Name = "lblTiempo";
            lblTiempo.Size = new Size(48, 15);
            lblTiempo.TabIndex = 5;
            lblTiempo.Text = "Tiempo";
            // 
            // lblMemoria
            // 
            lblMemoria.AutoSize = true;
            lblMemoria.Location = new Point(307, 228);
            lblMemoria.Name = "lblMemoria";
            lblMemoria.Size = new Size(55, 15);
            lblMemoria.TabIndex = 6;
            lblMemoria.Text = "Memoria";
            // 
            // lblTasa
            // 
            lblTasa.AutoSize = true;
            lblTasa.Location = new Point(307, 277);
            lblTasa.Name = "lblTasa";
            lblTasa.Size = new Size(30, 15);
            lblTasa.TabIndex = 7;
            lblTasa.Text = "Tasa";
            lblTasa.Click += lblTasa_Click;
            // 
            // labeltipo
            // 
            labeltipo.AutoSize = true;
            labeltipo.Location = new Point(291, 44);
            labeltipo.Name = "labeltipo";
            labeltipo.Size = new Size(181, 15);
            labeltipo.TabIndex = 8;
            labeltipo.Text = "Seleccione el tipo de compresion";
            labeltipo.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(548, 380);
            Controls.Add(labeltipo);
            Controls.Add(lblTasa);
            Controls.Add(lblMemoria);
            Controls.Add(lblTiempo);
            Controls.Add(btnDescomprimir);
            Controls.Add(btnComprimir);
            Controls.Add(cmbAlgoritmo);
            Controls.Add(lstArchivos);
            Controls.Add(btnSeleccionarArchivos);
            ForeColor = SystemColors.ActiveCaptionText;
            Name = "Form1";
            Text = "Compressor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSeleccionarArchivos;
        private ListBox lstArchivos;
        private ComboBox cmbAlgoritmo;
        private Button btnComprimir;
        private Button btnDescomprimir;
        private Label lblTiempo;
        private Label lblMemoria;
        private Label lblTasa;
        private Label labeltipo;
    }
}
