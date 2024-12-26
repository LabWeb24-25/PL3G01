using System.ComponentModel.DataAnnotations;

namespace biblioon.Models

{
    public class Biblioteca
    {
        [Key]
        public required string ID { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Localização { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Telefone { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Horario { get; set; }


    }
}
