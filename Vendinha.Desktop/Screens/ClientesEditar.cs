using System.ComponentModel.DataAnnotations;
using Vendinha.Core.Services;

namespace Vendinha.Desktop.Screens
{
    public partial class ClientesEditar : Form
    {
        private readonly ClienteService _clienteService;
        private readonly int _clienteId;

        private TextBox _txtNome = null!;
        private TextBox _txtEmail = null!;

        public ClientesEditar(int clienteId)
        {
            _clienteService = new ClienteService();
            _clienteId = clienteId;

            ConfigurarTela();
            CarregarDadosCliente();
        }

        private void ConfigurarTela()
        {
            this.Text = $"Editar Cliente #{_clienteId}";
            this.Size = new System.Drawing.Size(400, 240);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblNome = new Label { Text = "Nome:", Location = new Point(20, 20), Size = new Size(80, 20) };
            _txtNome = new TextBox { Location = new Point(120, 17), Size = new Size(230, 20) };

            Label lblEmail = new Label { Text = "E-mail:", Location = new Point(20, 60), Size = new Size(80, 20) };
            _txtEmail = new TextBox { Location = new Point(120, 57), Size = new Size(230, 20) };

            Button btnSalvar = new Button
            {
                Text = "💾 Salvar Alterações",
                Location = new Point(120, 100),
                Size = new Size(230, 35),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnSalvar.Click += BtnSalvar_Click;

            Button btnExcluir = new Button
            {
                Text = "🗑️ Excluir/Inativar Cliente",
                Location = new Point(120, 145),
                Size = new Size(230, 30),
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnExcluir.Click += BtnExcluir_Click;

            this.Controls.AddRange(new Control[] { lblNome, _txtNome, lblEmail, _txtEmail, btnSalvar, btnExcluir });
        }

        private void CarregarDadosCliente()
        {
            try
            {
                var todos = _clienteService.Listar(1, 1000);
                var cliente = todos.Find(c => c.Id == _clienteId);

                if (cliente != null)
                {
                    _txtNome.Text = cliente.Nome;
                    _txtEmail.Text = cliente.Email == "---" ? "" : cliente.Email;
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            string nome = _txtNome.Text.Trim();
            string? email = string.IsNullOrWhiteSpace(_txtEmail.Text) ? null : _txtEmail.Text.Trim();

            if (_clienteService.Atualizar(_clienteId, nome, email, out List<ValidationResult> erros))
            {
                MessageBox.Show("Cliente atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(erros[0].ErrorMessage, "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show($"Tem certeza absoluta que deseja inativar este cliente?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (_clienteService.Excluir(_clienteId, out List<ValidationResult> erros))
                {
                    MessageBox.Show("Cliente inativado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(erros[0].ErrorMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}