namespace Vendinha.Desktop
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
            label_1 = new Label();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // label_1
            // 
            label_1.AutoSize = true;
            label_1.Font = new Font("Segoe UI", 24F);
            label_1.Location = new Point(12, 9);
            label_1.Name = "label_1";
            label_1.Size = new Size(189, 54);
            label_1.TabIndex = 0;
            label_1.Text = "Vendinha";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.Highlight;
            button1.Font = new Font("Segoe UI", 12F);
            button1.ForeColor = SystemColors.ButtonHighlight;
            button1.Location = new Point(19, 77);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(120, 50);
            button1.TabIndex = 1;
            button1.Text = "Clientes";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.Highlight;
            button2.Font = new Font("Segoe UI", 12F);
            button2.ForeColor = SystemColors.ButtonHighlight;
            button2.Location = new Point(19, 139);
            button2.Name = "button2";
            button2.Size = new Size(120, 50);
            button2.TabIndex = 2;
            button2.Text = "Dívidas";
            button2.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label_1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_1;
        private Button button1;
        private Button button2;
    }
}
