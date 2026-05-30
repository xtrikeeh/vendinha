using System.ComponentModel.DataAnnotations;
using Vendinha.Core.Services;

namespace Vendinha.Desktop.Screens
{
    public partial class DividasListagem : Form
    {
        private readonly DividaService _dividaService;
        private readonly ClienteService _clienteService;

        private DataGridView _dgvDividas = null!;
        private ComboBox _cmbFiltroCliente = null!;
        private Label _lblTotalAcumulado = null!;

        private int? _clienteIdSelecionado = null;
        private bool _carregandoMenu = true;

        public DividasListagem()
        {
            InitializeComponent();
            _dividaService = new DividaService();
            _clienteService = new ClienteService();

            ConfigurarTela();
            CarregarClientesNoFiltro();
            ExecutarBusca();
        }

        private void ConfigurarTela()
        {
            this.Text = "Consulta de Dívidas";
            this.Size = new System.Drawing.Size(900, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Panel pnlTopo = new Panel { Dock = DockStyle.Top, Height = 60 };

            Label lblCliente = new Label { Text = "Filtrar Cliente:", Location = new System.Drawing.Point(15, 22), Size = new System.Drawing.Size(80, 20) };

            _cmbFiltroCliente = new ComboBox
            {
                Location = new System.Drawing.Point(100, 19),
                Size = new System.Drawing.Size(250, 21),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbFiltroCliente.SelectedIndexChanged += CmbFiltroCliente_SelectedIndexChanged;

            _lblTotalAcumulado = new Label
            {
                Text = "Total em dívidas: R$ 0,00",
                Location = new System.Drawing.Point(370, 20),
                Size = new System.Drawing.Size(340, 22),
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Button btnNovaDivida = new Button
            {
                Text = "💰 Lançar Dívida",
                Location = new System.Drawing.Point(730, 12),
                Size = new System.Drawing.Size(140, 32),
                BackColor = System.Drawing.Color.Khaki,
                Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
            };
            btnNovaDivida.Click += BtnNovaDivida_Click;

            pnlTopo.Controls.AddRange(new Control[] { lblCliente, _cmbFiltroCliente, _lblTotalAcumulado, btnNovaDivida });

            _dgvDividas = new DataGridView
            {
                Location = new System.Drawing.Point(15, 75),
                Size = new System.Drawing.Size(855, 340),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = false
            };
            _dgvDividas.CellClick += DgvDividas_CellClick;

            _dgvDividas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50 });
            _dgvDividas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Valor", HeaderText = "Valor" });
            _dgvDividas.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataCriacao", HeaderText = "Data Criação" });
            _dgvDividas.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataPagamento", HeaderText = "Data Pagamento" });
            _dgvDividas.Columns.Add(new DataGridViewTextBoxColumn { Name = "ClienteId", HeaderText = "Cliente ID", Width = 80 });

            _dgvDividas.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Pagar",
                HeaderText = "Ações",
                Width = 90
            });

            this.Controls.AddRange(new Control[] { pnlTopo, _dgvDividas });
        }

        private void CarregarClientesNoFiltro()
        {
            try
            {
                _carregandoMenu = true;
                var clientes = _clienteService.Listar(1, 1000);

                var listaOpcoes = new List<dynamic>();
                listaOpcoes.Add(new { Id = (int?)null, Nome = "--- Todos os Clientes ---" });

                foreach (var c in clientes)
                {
                    listaOpcoes.Add(new { Id = (int?)c.Id, Nome = c.Nome });
                }

                _cmbFiltroCliente.DataSource = listaOpcoes;
                _cmbFiltroCliente.DisplayMember = "Nome";
                _cmbFiltroCliente.ValueMember = "Id";

                _cmbFiltroCliente.SelectedIndex = 0;
                _clienteIdSelecionado = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar filtro de clientes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _carregandoMenu = false;
            }
        }

        private void CmbFiltroCliente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_carregandoMenu) return;
            _clienteIdSelecionado = _cmbFiltroCliente.SelectedValue as int?;
            ExecutarBusca();
        }

        private void ExecutarBusca()
        {
            _dgvDividas.Rows.Clear();

            var lista = _dividaService.Listar(_clienteIdSelecionado);
            decimal somaTotal = 0;

            foreach (var divida in lista)
            {
                string valorFormatado = divida.Valor.ToString("C2", new System.Globalization.CultureInfo("pt-BR"));
                string dataCriacaoStr = divida.DataCriacao.ToString("dd/MM/yyyy");
                string dataPagamentoStr = divida.Situacao ? (divida.DataPagamento?.ToString("dd/MM/yyyy") ?? "Pago") : "---";

                if (!divida.Situacao)
                {
                    somaTotal += divida.Valor;
                }

                int rowIndex = _dgvDividas.Rows.Add(
                    divida.Id,
                    valorFormatado,
                    dataCriacaoStr,
                    dataPagamentoStr,
                    divida.ClienteId,
                    ""
                );

                var cellBotao = _dgvDividas.Rows[rowIndex].Cells["Pagar"] as DataGridViewButtonCell;
                if (cellBotao != null)
                {
                    if (divida.Situacao)
                    {
                        _dgvDividas.Rows[rowIndex].Cells["Pagar"] = new DataGridViewTextBoxCell { Value = "✓ Paga" };
                        _dgvDividas.Rows[rowIndex].Cells["Pagar"].Style.ForeColor = Color.Green;
                    }
                    else
                    {
                        cellBotao.Value = "Quitar";
                        cellBotao.Style.BackColor = Color.LightGreen;
                        cellBotao.Style.ForeColor = Color.Black;
                    }
                }
            }

            _lblTotalAcumulado.Text = $"Total em dívidas: {somaTotal.ToString("C2", new System.Globalization.CultureInfo("pt-BR"))}";
        }

        private void DgvDividas_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && _dgvDividas.Columns[e.ColumnIndex].Name == "Pagar")
            {
                if (_dgvDividas.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewTextBoxCell) return;

                int idDivida = (int)_dgvDividas.Rows[e.RowIndex].Cells["Id"].Value;

                if (MessageBox.Show("Confirmar pagamento desta dívida?", "Quitar Débito", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_dividaService.Pagar(idDivida, out List<ValidationResult> erros))
                    {
                        MessageBox.Show("Dívida quitada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ExecutarBusca();
                    }
                    else { MessageBox.Show(erros[0].ErrorMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        private void BtnNovaDivida_Click(object? sender, EventArgs e)
        {
            using var telaCadastro = new DividasCadastro();
            if (telaCadastro.ShowDialog() == DialogResult.OK)
            {
                if (_cmbFiltroCliente.Items.Count > 0)
                {
                    _cmbFiltroCliente.SelectedIndex = 0;
                }
                _clienteIdSelecionado = null;
                ExecutarBusca();
            }
        }
    }
}