using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vendinha.Core.Data;
using Vendinha.Core.Models;

namespace Vendinha.Core.Services
{
    public class DividaService
    {
        public bool Validar(Divida a, out List<ValidationResult> erros)
        {
            var contexto = new ValidationContext(a);
            erros = new List<ValidationResult>();
            var objetoValido = Validator.TryValidateObject(a, contexto, erros, true);
            return objetoValido;
        }
        public List<Divida> Listar()
        {
            using var context = new VendinhaDbContext();

            return context.Dividas.ToList();
        }
        public bool Criar(Divida c, out List<ValidationResult> erros)
        {
            if (!Validar(c, out erros))
            {
                return false;
            }

            using var context = new VendinhaDbContext();
            context.Dividas.Add(c);
            context.SaveChanges();

            return true;
        }
    }
}
