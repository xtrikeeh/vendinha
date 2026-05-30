namespace Vendinha.Desktop.Screens
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TituloDashboard = new Label();
            btnClientes = new Button();
            btnDividas = new Button();
            SuspendLayout();
            // 
            // TituloDashboard
            // 
            TituloDashboard.AutoSize = true;
            TituloDashboard.Font = new Font("Segoe UI", 20F);
            TituloDashboard.Location = new Point(24, 31);
            TituloDashboard.Name = "TituloDashboard";
            TituloDashboard.Size = new Size(159, 46);
            TituloDashboard.TabIndex = 0;
            TituloDashboard.Text = "Vendinha";
            // 
            // btnClientes
            // 
            btnClientes.BackColor = Color.SeaGreen;
            btnClientes.Font = new Font("Segoe UI", 12F);
            btnClientes.ForeColor = SystemColors.ButtonHighlight;
            btnClientes.Location = new Point(24, 177);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(152, 59);
            btnClientes.TabIndex = 2;
            btnClientes.Text = "Clientes";
            btnClientes.UseVisualStyleBackColor = false;
            btnClientes.MouseDown += btnClientes_MouseDown;
            // 
            // btnDividas
            // 
            btnDividas.BackColor = Color.DarkSeaGreen;
            btnDividas.Font = new Font("Segoe UI", 12F);
            btnDividas.ForeColor = SystemColors.ButtonHighlight;
            btnDividas.Location = new Point(24, 260);
            btnDividas.Name = "btnDividas";
            btnDividas.Size = new Size(152, 59);
            btnDividas.TabIndex = 3;
            btnDividas.Text = "Dívidas";
            btnDividas.UseVisualStyleBackColor = false;
            btnDividas.MouseDown += btnDividas_MouseDown;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDividas);
            Controls.Add(btnClientes);
            Controls.Add(TituloDashboard);
            Name = "Dashboard";
            Text = "Dashboard";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label TituloDashboard;
        private Button btnClientes;
        private Button btnDividas;
    }
}