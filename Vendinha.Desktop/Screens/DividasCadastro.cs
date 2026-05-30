using System.ComponentModel.DataAnnotations;
using Vendinha.Core.Models;
using Vendinha.Core.Services;

namespace Vendinha.Desktop.Screens
{
    public partial class DividasCadastro : Form
    {
        private readonly DividaService _dividaService;
        private readonly ClienteService _clienteService;

        private ComboBox _cmbClientes = null!;
        private NumericUpDown _numValor = null!;

        public int ClienteIdRegistrado { get; private set; }

        public DividasCadastro()
        {
            InitializeComponent();
            _dividaService = new DividaService();
            _clienteService = new ClienteService();

            ConfigurarTela();
            CarregarClientesNoMenu();
        }

        private void ConfigurarTela()
        {
            this.Text = "Lançar Débito (Pendura)";
            this.Size = new System.Drawing.Size(320, 240);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblCliente = new Label { Text = "Selecione o Cliente:", Location = new System.Drawing.Point(30, 25), Size = new System.Drawing.Size(150, 20) };

            _cmbClientes = new ComboBox
            {
                Location = new System.Drawing.Point(30, 45),
                Size = new System.Drawing.Size(240, 21),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblValor = new Label { Text = "Valor do Débito (R$):", Location = new System.Drawing.Point(30, 85), Size = new System.Drawing.Size(150, 20) };
            _numValor = new NumericUpDown { Location = new System.Drawing.Point(30, 105), Size = new System.Drawing.Size(140, 20), DecimalPlaces = 2, Maximum = 500000 };

            Button btnGravar = new Button { Text = "Confirmar Lançamento", Location = new System.Drawing.Point(30, 150), Size = new System.Drawing.Size(240, 35), BackColor = System.Drawing.Color.Khaki };
            btnGravar.Click += BtnGravar_Click;

            this.Controls.AddRange(new Control[] { lblCliente, _cmbClientes, lblValor, _numValor, btnGravar });
        }

        private void CarregarClientesNoMenu()
        {
            try
            {
                var clientesAtivos = _clienteService.Listar(1, 1000);

                _cmbClientes.DataSource = clientesAtivos;
                _cmbClientes.DisplayMember = "Nome";
                _cmbClientes.ValueMember = "Id";

                _cmbClientes.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar lista de clientes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGravar_Click(object? sender, EventArgs e)
        {
            if (_cmbClientes.SelectedValue == null || _cmbClientes.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecione um cliente no menu.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idCliente = (int)_cmbClientes.SelectedValue;

            if (_numValor.Value <= 0)
            {
                MessageBox.Show("O valor do débito deve ser maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var novaDivida = new Divida
            {
                ClienteId = idCliente,
                Valor = _numValor.Value,
                Situacao = false,
                DataCriacao = DateTime.UtcNow
            };

            if (_dividaService.Criar(novaDivida, out List<ValidationResult> erros))
            {
                MessageBox.Show("Dívida registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ClienteIdRegistrado = idCliente;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string msg = "Impossível lançar:\n";
                foreach (var erro in erros) msg += $"- {erro.ErrorMessage}\n";
                MessageBox.Show(msg, "Regra de Negócio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}