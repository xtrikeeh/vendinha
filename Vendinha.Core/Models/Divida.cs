using System.ComponentModel.DataAnnotations;

namespace Vendinha.Core.Models
{
    public class Divida
    {
        public int Id { get; set; }
        [Required]
        [Range(0, 1000000)]
        public decimal Valor { get; set; }
        [Required]
        public bool Situacao { get; set; } = false;
        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataPagamento { get; set; }
        [Required(ErrorMessage = "O Id do cliente é obrigatório para cadastrar a dívida")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
