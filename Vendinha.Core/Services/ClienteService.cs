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
            var intervalo = (pagina - 1) * quantidadeRegistros;

            return context.Clientes
                .Where(cliente => cliente.Status == true)
                .Select(cliente => new ClienteListagemDto
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Idade = cliente.Idade,
                    TotalDividas = context.Dividas
                        .Where(divida => divida.ClienteId == cliente.Id && divida.Situacao == false)
                        .Select(divida => (decimal?)divida.Valor)
                        .Sum() ?? 0
                })
                .OrderByDescending(cliente => cliente.TotalDividas)
                // 3º CRITÉRIO DE DESEMPATE: Se a dívida for igual (ex: R$ 0,00), ordena por ID para o cliente não sumir
                .ThenBy(cliente => cliente.Id)
                .Skip(intervalo)
                .Take(quantidadeRegistros)
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

            using var context = new VendinhaDbContext();

            context.Clientes.Add(cliente);
            context.SaveChanges();

            return true;
        }
        public bool Excluir(int clienteId, out List<ValidationResult> erros)
        {
            erros = new List<ValidationResult>();
            using var context = new VendinhaDbContext();

            var clienteEncontrado = context.Clientes.Find(clienteId);

            if (clienteEncontrado == null)
            {
                erros.Add(new ValidationResult("Cliente não encontrado no sistema."));
                return false;
            }

            bool possuiDividaPendente = context.Dividas.Any(divida => divida.ClienteId == clienteId && divida.Situacao == false);

            if (possuiDividaPendente)
            {
                erros.Add(new ValidationResult("Não é possível excluir um cliente com dívida aberta."));
                return false;
            }

            clienteEncontrado.Status = false;

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

        public List<ClienteListagemDto> Pesquisar(string nome)
        {
            using var context = new VendinhaDbContext();

            return context.Clientes
                .Where(cliente => cliente.Status == true && cliente.Nome.ToLower().Contains(nome.ToLower()))
                .Select(cliente => new ClienteListagemDto
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Idade = cliente.Idade,
                    TotalDividas = context.Dividas
                        .Where(divida => divida.ClienteId == cliente.Id && divida.Situacao == false)
                        .Select(divida => (decimal?)divida.Valor)
                        .Sum() ?? 0
                })
                .OrderByDescending(dto => dto.TotalDividas)
                .ThenBy(dto => dto.Id)
                .ToList();
        }
    }
}
