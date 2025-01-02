using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class Autor

    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Nome { get; set; }
        public string? Desc { get; set; }

        public string? Foto { get; set; }

        public ICollection<EdiLivro> EdiLivros { get; set; } = new List<EdiLivro>();
    }
}
