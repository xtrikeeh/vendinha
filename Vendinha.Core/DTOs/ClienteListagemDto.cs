namespace Vendinha.Core.DTOs
{
    public class ClienteListagemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Email { get; set; }
        public decimal TotalDividas { get; set; }
    }
}