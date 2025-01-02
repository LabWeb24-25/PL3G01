using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{
    public class Admin
    {
        [Key]
        public required string Id { get; set; }

        public required string IdCriador { get; set; }

        public required DateTime? DataCriacao { get; set; }

        [ForeignKey("Id")]
        public required ApplicationUser User { get; set; }

        [ForeignKey("IdCriador")]
        public required ApplicationUser Criador { get; set; }


        public ICollection<Ban> BansAplicados { get; set; } = new List<Ban>();

        public ICollection<Bibliotecario> BibliotecariosAtivados { get; set; } = new List<Bibliotecario>();
    }
}
