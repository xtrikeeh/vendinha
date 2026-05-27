using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vendinha.Core.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 10)]
        [RegularExpression(@"^[A-Za-zÀ-ÿ]+(\s+[A-Za-zÀ-ÿ]+)+$", ErrorMessage = "Nome inválido")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF inválido")]
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
        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateOnly DataNascimento { get; set; }
        public bool Status { get; set; } = true;
        [EmailAddress]
        public string? Email { get; set; }
    }
}
