using System.ComponentModel.DataAnnotations;

namespace biblioon.Models
{

    public class Leitor
    {
        [Key]
        public required string ID { get; set; } 

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Contacto { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Morada { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string CodPostal { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Localidade { get; set; }



    }
}
