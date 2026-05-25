    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vendinha.Core.Data;
using Vendinha.Core.Models;

namespace Vendinha.Core.Services
{
    public class ClienteService
    {
        public bool Validar(Cliente a, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(a);
            erros = new List<ValidationResult>();
            var objetoValido = Validator.TryValidateObject(a, contexto, erros, true);
            return objetoValido;
        }
        public List<Cliente> Listar()
        {
            using var context = new VendinhaDbContext();
            return context.Clientes.ToList();
        }
        public bool Criar(Cliente c, out List<ValidationResult> erros)
        {
            if (!Validar(c, out erros))
            {
                return false;
            }

            using var context = new VendinhaDbContext();
            context.Clientes.Add(c);
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
    }
}
