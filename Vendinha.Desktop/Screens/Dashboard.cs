using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Vendinha.Desktop.Screens
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnClientes_MouseDown(object sender, MouseEventArgs e)
        {
            var telaClientes = new ClientesListagem();
            telaClientes.Show();
        }

        private void btnDividas_MouseDown(object sender, MouseEventArgs e)
        {
            var telaDividas = new DividasListagem();
            telaDividas.Show();
        }
    }
}
