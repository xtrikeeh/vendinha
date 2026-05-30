using System;
using System.Windows.Forms;
using Vendinha.Core.Services;

namespace Vendinha.Desktop.Screens
{
    public partial class SelecaoCliente : Form
    {
        private readonly ClienteService _clienteService;
        private DataGridView _dgvClientes = null!;
        private TextBox _txtBusca = null!;

        // Propriedades para retornar à tela que chamou esta modal
        public int ClienteIdSelecionado { get; private set; }
        public string ClienteNomeSelecionado { get; private set; } = string.Empty;

        public SelecaoCliente()
        {
            InitializeComponent();
            _clienteService = new ClienteService();
            ConfigurarTela();
            CarregarClientes();
        }

        private void ConfigurarTela()
        {
            this.Text = "Selecione o Cliente";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Panel pnlTopo = new Panel { Dock = DockStyle.Top, Height = 50 };
            _txtBusca = new TextBox { Location = new System.Drawing.Point(15, 15), Size = new System.Drawing.Size(350, 20) };
            _txtBusca.TextChanged += (s, e) => CarregarClientes();
            Label lblDica = new Label { Text = "Digite para filtrar...", Location = new System.Drawing.Point(15, 35), ForeColor = System.Drawing.Color.Gray, Size = new System.Drawing.Size(200, 15) };

            pnlTopo.Controls.AddRange(new Control[] { _txtBusca, lblDica });

            _dgvClientes = new DataGridView
            {
                Location = new System.Drawing.Point(15, 60),
                Size = new System.Drawing.Size(455, 240),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Evento de duplo clique para selecionar o cliente mais rápido
            _dgvClientes.CellDoubleClick += DgvClientes_CellDoubleClick;

            Button btnConfirmar = new Button { Text = "Selecionar", Location = new System.Drawing.Point(260, 315), Size = new System.Drawing.Size(100, 30), BackColor = System.Drawing.Color.LightBlue };
            btnConfirmar.Click += BtnConfirmar_Click;

            Button btnCancelar = new Button { Text = "Cancelar", Location = new System.Drawing.Point(370, 315), Size = new System.Drawing.Size(100, 30) };
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { pnlTopo, _dgvClientes, btnConfirmar, btnCancelar });
        }

        private void CarregarClientes()
        {
            if (string.IsNullOrWhiteSpace(_txtBusca.Text))
            {
                // Carrega os primeiros 50 clientes para não travar a listagem inicial
                _dgvClientes.DataSource = _clienteService.Listar(1, 50);
            }
            else
            {
                _dgvClientes.DataSource = _clienteService.Pesquisar(_txtBusca.Text.Trim());
            }

            // Oculta colunas desnecessárias para a seleção ficar limpa
            if (_dgvClientes.Columns["Status"] != null) _dgvClientes.Columns["Status"].Visible = false;
            if (_dgvClientes.Columns["Email"] != null) _dgvClientes.Columns["Email"].Visible = false;
        }

        private void SelecionarERetornar()
        {
            if (_dgvClientes.CurrentRow != null)
            {
                ClienteIdSelecionado = (int)_dgvClientes.CurrentRow.Cells["Id"].Value;
                ClienteNomeSelecionado = _dgvClientes.CurrentRow.Cells["Nome"].Value?.ToString() ?? "";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void BtnConfirmar_Click(object? sender, EventArgs e) => SelecionarERetornar();
        private void DgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) SelecionarERetornar();
        }
    }
}