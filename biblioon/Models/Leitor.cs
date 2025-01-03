using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{

    public class Leitor
    {
        [Key]
        public required string Id { get; set; } 

        [ForeignKey("Id")]
        public required ApplicationUser User { get; set; }

        public required int LivrosLidos { get; set; } = 0;
        public required int NRequisicoes { get; set; } = 0;
        public required bool IsBanido { get; set; } = false;

        public ICollection<Ban> Bans { get; set; } = new List<Ban>();

        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    }
}
