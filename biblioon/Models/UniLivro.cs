using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class UniLivro
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required int Numero { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Isbn { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Estado { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required float PrecoAquisicao { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required DateOnly DataAquisicao { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required bool Requisitado { get; set; } 

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required bool Disponivel { get; set; }

        public string? Anotacoes { get; set; }

    }
}
