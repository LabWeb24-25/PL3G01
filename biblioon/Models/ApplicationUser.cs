using Microsoft.AspNetCore.Identity;

namespace biblioon.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string NomeCompleto { get; set; }
        public DateTime? DataAtivacao { get; set; }
        public string? Anotacoes { get; set; }
    }
}
