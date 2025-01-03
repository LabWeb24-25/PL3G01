using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{
    public class Ban
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();

        public required string IdUser { get; set; }

        public required string IdAdmin { get; set; }

        public required DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public required string Motivo { get; set; }

        [ForeignKey("IdUser")]
        public required Leitor User { get; set; }

        [ForeignKey("IdAdmin")]
        public required Admin Admin { get; set; }
    }
}
