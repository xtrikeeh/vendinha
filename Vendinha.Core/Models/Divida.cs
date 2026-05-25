using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Vendinha.Core.Models
{
    public class Divida
    {
        public int Id { get; set; }
        [Range(0, 1000000000)]
        public decimal Valor { get; set; }
        public bool Situacao {  get; set; } 
        [Required]
        public DateTime DataCriacao { get; set;}
        public DateTime? DataPagamento { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
