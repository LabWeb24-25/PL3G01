using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{
    public class Emprestimo
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();

        public required string LeitorId { get; set; }
        public required string UniLivroId { get; set; }
        public required string EdiLivroISBN { get; set; }
        public required DateTime DataRequisitado { get; set; } = DateTime.Now;
        public required DateTime DataLimiteEntrega { get; set; }



        public DateTime? DataLevantamento { get; set; }
        public DateTime? DataEntrega { get; set; }

        public required bool IsLevantado { get; set; } = false;
        public required bool IsEntregue { get; set; } = false;

        public string? IdBibliotecarioLevantamento { get; set; }
        public string? IdBibliotecarioEntrega { get; set; }

        [ForeignKey("LeitorId")]
        public required Leitor Leitor { get; set; }

        [ForeignKey("UniLivroId")]
        public required UniLivro UniLivro { get; set; }

        [ForeignKey("EdiLivroISBN")]
        public required EdiLivro EdiLivro { get; set; }

        [ForeignKey("IdBibliotecarioLevantamento")]
        public Bibliotecario? BibliotecarioLevantamento { get; set; }

        [ForeignKey("IdBibliotecarioEntrega")]
        public Bibliotecario? BibliotecarioEntrega { get; set; }



    }
}
