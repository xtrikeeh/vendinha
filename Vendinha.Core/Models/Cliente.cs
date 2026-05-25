using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vendinha.Core.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Nome { get; set; }
        [Required, StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; }

        public int Idade
        {
            get
            {
                var hoje = DateOnly.FromDateTime(DateTime.Today);
                int idade = hoje.Year - DataNascimento.Year;

                if (hoje < DataNascimento.AddYears(idade))
                {
                    idade--;
                }

                return idade;
            }
        }
        public DateOnly DataNascimento { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
    }
}
