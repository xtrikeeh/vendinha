using Vendinha.Core.Services;

namespace Vendinha.Desktop.Screens
{
    public partial class ClientesListagem : Form
    {
        private readonly ClienteService _clienteService;
        private DataGridView _dgvClientes = null!;
        private TextBox _txtBusca = null!;
        private Label _lblPagina = null!;
        private Button _btnAnterior = null!;
        private Button _btnProximo = null!;

        private int _paginaAtual = 1;
        private const int _registrosPorPagina = 10;
        private bool _estaEmModoBusca = false;

        public ClientesListagem()
        {
            InitializeComponent();
            _clienteService = new ClienteService();
            ConfigurarTela();
            CarregarClientes();

            this.Activated += ClientesListagem_Activated;
        }

        private void ConfigurarTela()
        {
            this.Text = "Gerenciamento de Clientes";
            this.Size = new System.Drawing.Size(900, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Panel pnlTopo = new Panel { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10) };

            Label lblBusca = new Label { Text = "Buscar por nome:", Location = new System.Drawing.Point(15, 22), Size = new System.Drawing.Size(100, 20) };

            _txtBusca = new TextBox { Location = new System.Drawing.Point(120, 19), Size = new System.Drawing.Size(250, 20) };
            _txtBusca.TextChanged += TxtBusca_TextChanged;

            Button btnLimpar = new Button { Text = "Limpar", Location = new System.Drawing.Point(385, 16), Size = new System.Drawing.Size(70, 25) };
            btnLimpar.Click += BtnLimpar_Click;

            Button btnNovo = new Button
            {
                Text = "➕ Novo Cliente",
                Location = new System.Drawing.Point(740, 12),
                Size = new System.Drawing.Size(130, 32),
                BackColor = System.Drawing.Color.LightGreen,
                Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
            };
            btnNovo.Click += BtnNovo_Click;

            pnlTopo.Controls.AddRange(new Control[] { lblBusca, _txtBusca, btnLimpar, btnNovo });

            _dgvClientes = new DataGridView
            {
                Location = new System.Drawing.Point(15, 70),
                Size = new System.Drawing.Size(855, 330),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = false
            };
            _dgvClientes.CellClick += DgvClientes_CellClick;

            _dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50 });
            _dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", Width = 200 });
            _dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Idade", HeaderText = "Idade", Width = 60 });
            _dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "E-mail", Width = 220 });
            _dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Divida", HeaderText = "Dívida Atual", Width = 110 });

            _dgvClientes.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "Ações",
                Width = 90
            });

            _btnAnterior = new Button { Text = "◀ Anterior", Location = new System.Drawing.Point(340, 415), Size = new System.Drawing.Size(90, 28) };
            _btnAnterior.Click += (s, e) => { _paginaAtual--; CarregarClientes(); };

            _lblPagina = new Label { Text = "Página 1", Location = new System.Drawing.Point(440, 420), Size = new System.Drawing.Size(80, 20), TextAlign = System.Drawing.ContentAlignment.MiddleCenter, Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold) };

            _btnProximo = new Button { Text = "Próximo ▶", Location = new System.Drawing.Point(530, 415), Size = new System.Drawing.Size(90, 28) };
            _btnProximo.Click += (s, e) => { _paginaAtual++; CarregarClientes(); };

            this.Controls.AddRange(new Control[] { pnlTopo, _dgvClientes, _btnAnterior, _lblPagina, _btnProximo });
        }

        private void CarregarClientes()
        {
            try
            {
                _dgvClientes.Rows.Clear();
                var dividaService = new DividaService();

                if (_estaEmModoBusca)
                {
                    var resultadoBusca = _clienteService.Pesquisar(_txtBusca.Text.Trim());
                    _btnAnterior.Enabled = _btnProximo.Enabled = false;
                    _lblPagina.Text = "Busca";

                    foreach (var cliente in resultadoBusca)
                    {
                        if (cliente == null) continue;

                        int id = Convert.ToInt32(cliente.GetType().GetProperty("Id")?.GetValue(cliente));
                        string nome = cliente.GetType().GetProperty("Nome")?.GetValue(cliente)?.ToString() ?? "";
                        int idade = Convert.ToInt32(cliente.GetType().GetProperty("Idade")?.GetValue(cliente));
                        string? emailStr = cliente.GetType().GetProperty("Email")?.GetValue(cliente)?.ToString();

                        decimal valorDivida = 0;
                        var dividasDoCliente = dividaService.Listar(id);
                        if (dividasDoCliente != null)
                        {
                            foreach (var d in dividasDoCliente)
                            {
                                if (d.Situacao == false) valorDivida += (decimal)d.Valor;
                            }
                        }

                        string emailExibicao = string.IsNullOrWhiteSpace(emailStr) ? "---" : emailStr;
                        string dividaFormatada = valorDivida.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));

                        int rowIndex = _dgvClientes.Rows.Add(id, nome, idade, emailExibicao, dividaFormatada, "");

                        var cellBotao = _dgvClientes.Rows[rowIndex].Cells["Editar"] as DataGridViewButtonCell;
                        if (cellBotao != null)
                        {
                            cellBotao.Value = "Editar";
                            cellBotao.Style.BackColor = Color.LightSkyBlue;
                            cellBotao.Style.ForeColor = Color.Black;
                        }
                    }
                }
                else
                {
                    var listaPaginada = _clienteService.Listar(_paginaAtual, _registrosPorPagina);
                    _btnAnterior.Enabled = _paginaAtual > 1;
                    _btnProximo.Enabled = listaPaginada.Count == _registrosPorPagina;
                    _lblPagina.Text = $"Página {_paginaAtual}";

                    foreach (var cliente in listaPaginada)
                    {
                        string emailExibicao = string.IsNullOrWhiteSpace(cliente.Email) ? "---" : cliente.Email;
                        string dividaFormatada = cliente.TotalDividas.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));

                        int rowIndex = _dgvClientes.Rows.Add(cliente.Id, cliente.Nome, cliente.Idade, emailExibicao, dividaFormatada, "");

                        var cellBotao = _dgvClientes.Rows[rowIndex].Cells["Editar"] as DataGridViewButtonCell;
                        if (cellBotao != null)
                        {
                            cellBotao.Value = "Editar";
                            cellBotao.Style.BackColor = Color.LightSkyBlue;
                            cellBotao.Style.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ClientesListagem_Activated(object? sender, EventArgs e)
        {
            CarregarClientes();
        }

        private void TxtBusca_TextChanged(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtBusca.Text))
            {
                _estaEmModoBusca = false;
                _paginaAtual = 1;
            }
            else
            {
                _estaEmModoBusca = true;
            }
            CarregarClientes();
        }

        private void BtnLimpar_Click(object? sender, EventArgs e)
        {
            _txtBusca.Clear();
        }

        private void BtnNovo_Click(object? sender, EventArgs e)
        {
            using var telaCadastro = new ClientesCadastro();
            if (telaCadastro.ShowDialog() == DialogResult.OK)
            {
                CarregarClientes();
            }
        }

        private void DgvClientes_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _dgvClientes.Columns[e.ColumnIndex].Name == "Editar")
            {
                int idCliente = (int)_dgvClientes.Rows[e.RowIndex].Cells["Id"].Value;

                using var telaEditar = new ClientesEditar(idCliente);
                if (telaEditar.ShowDialog() == DialogResult.OK)
                {
                    CarregarClientes();
                }
            }
        }
    }
}