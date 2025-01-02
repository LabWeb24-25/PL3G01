using Microsoft.AspNetCore.Identity;

namespace biblioon.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string NomeCompleto { get; set; }
        public DateTime? DataAtivacao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string? Anotacoes { get; set; }
        public string? MoradaRua { get; set; }
        public string? MoradaCodPostal { get; set; }
        public string? MoradaLocalidade { get; set; }
        public string? Foto { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsBibliotecario { get; set; } = false;

        public ICollection<Notificacao> Notificacoes { get; set; } = new List<Notificacao>();


    }
}
