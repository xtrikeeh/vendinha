using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Vendinha.Core.Models;
using Vendinha.Core.Services;

namespace Vendinha.Desktop.Screens
{
    public partial class ClientesCadastro : Form
    {
        private readonly ClienteService _clienteService;
        private TextBox _txtNome = null!;
        private TextBox _txtCpf = null!;
        private TextBox _txtEmail = null!;
        private DateTimePicker _dtpDataNascimento = null!;

        public ClientesCadastro()
        {
            InitializeComponent();
            _clienteService = new ClienteService();
            ConfigurarTela();
        }

        private void ConfigurarTela()
        {
            this.Text = "Cadastrar Novo Cliente";
            this.Size = new System.Drawing.Size(420, 320);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblNome = new Label { Text = "Nome Completo:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(150, 20) };
            _txtNome = new TextBox { Location = new System.Drawing.Point(20, 40), Size = new System.Drawing.Size(360, 20) };

            Label lblCpf = new Label { Text = "CPF:", Location = new System.Drawing.Point(20, 80), Size = new System.Drawing.Size(150, 20) };
            _txtCpf = new TextBox { Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(170, 20) };

            Label lblNascimento = new Label { Text = "Data de Nascimento:", Location = new System.Drawing.Point(210, 80), Size = new System.Drawing.Size(150, 20) };
            _dtpDataNascimento = new DateTimePicker { Location = new System.Drawing.Point(210, 100), Size = new System.Drawing.Size(170, 20), Format = DateTimePickerFormat.Short };

            Label lblEmail = new Label { Text = "E-mail (opcional):", Location = new System.Drawing.Point(20, 140), Size = new System.Drawing.Size(150, 20) };
            _txtEmail = new TextBox { Location = new System.Drawing.Point(20, 160), Size = new System.Drawing.Size(360, 20) };

            Button btnSalvar = new Button { Text = "Salvar Cliente", Location = new System.Drawing.Point(140, 220), Size = new System.Drawing.Size(130, 35), BackColor = System.Drawing.Color.LightGreen };
            btnSalvar.Click += BtnSalvar_Click;

            this.Controls.AddRange(new Control[] { lblNome, _txtNome, lblCpf, _txtCpf, lblNascimento, _dtpDataNascimento, lblEmail, _txtEmail, btnSalvar });
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            var novoCliente = new Cliente
            {
                Nome = _txtNome.Text.Trim(),
                Cpf = _txtCpf.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(_txtEmail.Text) ? null : _txtEmail.Text.Trim(),
                DataNascimento = DateOnly.FromDateTime(_dtpDataNascimento.Value),
                Status = true
            };

            if (_clienteService.Criar(novoCliente, out List<ValidationResult> erros))
            {
                MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK; // Define sucesso e fecha
                this.Close();
            }
            else
            {
                string msg = "Erros de validação:\n";
                foreach (var erro in erros) msg += $"- {erro.ErrorMessage}\n";
                MessageBox.Show(msg, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}