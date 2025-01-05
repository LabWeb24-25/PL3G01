using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblioon.Models
{
    public class EdiLivro
    {
        [Key]
        public required string Isbn { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public string? BarCode { get; set; }
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Sinopse { get; set; }

        public string? Capa { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string Idioma { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string DescFisica { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required DateOnly DataPublicacao { get; set; }

        [Required(ErrorMessage = "Este é um field obrigatório.")]
        public required string EditorId { get; set; }

        [ForeignKey("EditorId")]
        [Required(ErrorMessage = "O Editor (não o id do mesmo) é obrigatório.")]
        public required Editor Editor { get; set; }

        public ICollection<Genero> Generos { get; set; } = new List<Genero>();

        public ICollection<Autor> Autores { get; set; } = new List<Autor>();

        public ICollection<UniLivro> UniLivros { get; set; } = new List<UniLivro>();

        public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

        public required int NEmprestimos { get; set; } = 0;

    }

}
