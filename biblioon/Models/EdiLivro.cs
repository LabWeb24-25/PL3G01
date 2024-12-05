using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class EdiLivro
    {
        [Key]
        public required int Isbn { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Sinopse { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Capa { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string [] Idioma { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string DescFisica { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required DateOnly DataPublicacao { get; set; }

    }

}
