using System.ComponentModel.DataAnnotations;
using Vendinha.Core.Data;
using Vendinha.Core.DTOs;
using Vendinha.Core.Models;

namespace Vendinha.Core.Services
{
    public class ClienteService
    {
        public bool Validar(Cliente cliente, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(cliente);
            erros = new List<ValidationResult>();
            var objetoValido = Validator.TryValidateObject(cliente, contexto, erros, true);
            return objetoValido;
        }
        public bool CpfValido(string cpf)
        {
            using var context = new VendinhaDbContext();

            if (context.Clientes.Any(cliente => cliente.Cpf == cpf))
            {
                return false;
            }

            if (cpf.Distinct().Count() == 1)
            {
                return false;
            }

            return true;
        }
        public List<ClienteListagemDto> Listar(int pagina = 1, int quantidadeRegistros = 10)
        {
            using var context = new VendinhaDbContext();
            var intervalo = (pagina- 1) * quantidadeRegistros;

            return context.Clientes
                .Where(cliente => cliente.Status == true)
                .Skip(intervalo)
                .Take(quantidadeRegistros)
                .Select(cliente => new ClienteListagemDto
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Idade = cliente.Idade,
                    TotalDividas = context.Dividas
                        .Where(divida => divida.ClienteId == cliente.Id)
                        .Sum(divida => divida.Valor)
                }).OrderByDescending(cliente => cliente.TotalDividas)
                .ToList();
        }
        public bool Criar(Cliente cliente, out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
            {
                return false;
            }

            if (!CpfValido(cliente.Cpf))
            {
                erros.Add(new ValidationResult("CPF inválido"));
                return false;
            }

            var idadeMinima = DateOnly.FromDateTime(DateTime.Today).AddYears(-18);

            if (cliente.DataNascimento > idadeMinima)
            {
                erros.Add(new ValidationResult("Cliente deve ter pelo menos 18 anos."));
                return false;
            }

            using var context = new VendinhaDbContext();

            context.Clientes.Add(cliente);
            context.SaveChanges();

            return true;
        }

        public bool Excluir(int id, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            using var context = new VendinhaDbContext();

            var clienteBuscado = context.Clientes.Find(id);

            if (clienteBuscado == null)
            {
                erros.Add(new ValidationResult("Cliente não encontrado."));
                return false;
            }

            clienteBuscado.Status = false;

            context.SaveChanges();

            return true;
        }
        public bool Atualizar(int id, string? nome, string? email, out List<ValidationResult> erros)
        {
            using var context = new VendinhaDbContext();

            erros = new List<ValidationResult>();

            var clienteEncontrado = context.Clientes.Find(id);

            if (clienteEncontrado == null)
            {
                erros.Add(new ValidationResult("Não foi possível encontrar um cliente com esse Id."));
                return false;
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                clienteEncontrado.Nome = nome;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                clienteEncontrado.Email = email;
            }

            if (!Validar(clienteEncontrado, out erros))
            {
                return false;
            }

            context.SaveChanges();

            return true;
        }

        public List<Cliente> Pesquisar(string nome)
        {
            using var context = new VendinhaDbContext();

            var busca = context.Clientes.Where(cliente => cliente.Status == true && cliente.Nome.ToLower().Contains(nome.ToLower())).ToList();

            return busca;
        }
    }
}
