using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class UniLivro
    {
        [Key]
        public required int Id { get; set; }

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
