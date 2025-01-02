using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{
    public class Bibliotecario
    {
        [Key]
        public required string Id { get; set; }

        public required string IdAdminAtivador { get; set; }

        public required DateTime DataAtivacao { get; set; }

        public required bool IsAtivado { get; set; } = false;


        [ForeignKey("IdAdminAtivador")]
        public required Admin AdminAtivador { get; set; }

        [ForeignKey("Id")]
        public required ApplicationUser User { get; set; }

        public ICollection<Emprestimo> EmprestimosLevantamento { get; set; } = new List<Emprestimo>();

        public ICollection<Emprestimo> EmprestimosEntrega { get; set; } = new List<Emprestimo>();

    }
}
