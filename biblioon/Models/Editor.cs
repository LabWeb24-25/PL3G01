using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class Editor
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Nome { get; set; }
        
        public string? Descricao { get; set; }
        public string? Foto { get; set; }

        public string? Site { get; set; }
        public ICollection<EdiLivro> EdiLivros { get; set; } = new List<EdiLivro>();

    }
}
