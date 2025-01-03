using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{
    public class Comentario
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string EdiLivroISBN { get; set; }
        public required string LeitorId { get; set; }
        public required string Texto { get; set; }
        public required DateTime? DataCriacao { get; set; }
        public required int? Nota { get; set; }
        public required bool? Aprovado { get; set; }
        public required bool? Denunciado { get; set; }

        public required bool? Shown { get; set; } = true;

        [ForeignKey("LivroISBN")]
        public required EdiLivro EdiLivro { get; set; }


        [ForeignKey("IdLeitor")]
        public required Leitor Leitor { get; set; }

    }
}
