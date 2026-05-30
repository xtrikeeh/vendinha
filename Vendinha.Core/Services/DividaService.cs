using System.ComponentModel.DataAnnotations;
using Vendinha.Core.Data;
using Vendinha.Core.Models;

namespace Vendinha.Core.Services
{
    public class DividaService
    {
        public bool Validar(Divida divida, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(divida);
            erros = new List<ValidationResult>();
            var objetoValido = Validator.TryValidateObject(divida, contexto, erros, true);
            return objetoValido;
        }

        public List<Divida> Listar(int? clienteId = null)
        {
            using var context = new VendinhaDbContext();

            var busca = context.Dividas.AsQueryable();

            if (clienteId.HasValue)
            {
                busca = busca.Where(divida => divida.ClienteId == clienteId.Value);
            }

            return busca
                .OrderBy(divida => divida.Situacao)
                .ThenByDescending(divida => divida.DataCriacao)
                .ToList();
        }

        public bool Criar(Divida divida, out List<ValidationResult> erros)
        {
            using var context = new VendinhaDbContext();

            erros = new List<ValidationResult>();

            if (!Validar(divida, out erros))
            {
                return false;
            }

            if (context.Dividas.Any(divida_buscada => divida_buscada.ClienteId == divida.ClienteId && divida_buscada.Situacao == false))
            {
                erros.Add(new ValidationResult("Esse cliente já possui uma dívida aberta."));
                return false;
            }

            context.Dividas.Add(divida);
            
            context.SaveChanges();

            return true;
        }
        public bool Pagar(int id, out List<ValidationResult> erros)
        {
            using var context = new VendinhaDbContext();

            erros = new List<ValidationResult>();

            var dividaEncontrada = context.Dividas.Find(id);

            if (dividaEncontrada == null)
            {
                erros.Add(new ValidationResult("Não foi possível encontrar uma dívida com esse Id."));
                return false;
            }

            if (dividaEncontrada.Situacao == true)
            {
                erros.Add(new ValidationResult("Esta dívida já está paga."));
                return false;
            }

            dividaEncontrada.Situacao = true;
            dividaEncontrada.DataPagamento = DateTime.UtcNow;

            if (!Validar(dividaEncontrada, out erros))
            {
                return false;
            }

            context.SaveChanges();

            return true;
        }
        public decimal TotalDivida()
        {
            using var context = new VendinhaDbContext();

            return context.Dividas.Sum(divida => divida.Valor);
        }
    }
}
