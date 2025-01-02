using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{
    public class Notificacao
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Titulo { get; set; }
        public required string Mensagem { get; set; }
        public required int Tipo { get; set; }
        public required string UserId { get; set; }

        [ForeignKey("UserId")]
        public required ApplicationUser User { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Lida { get; set; } = false;
        public DateTime? DataLida { get; set; }
    }
}
