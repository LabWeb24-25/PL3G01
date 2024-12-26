using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class Genero
    {
        [Key]
        public string GeneroId { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Nome { get; set; }

        public ICollection<EdiLivro> EdiLivros { get; set; } = new List<EdiLivro>();
    }
}
