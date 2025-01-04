namespace biblioon.ViewModels
{
    public class EdiLivroViewModel
    {
        public string? Isbn { get; set; }
        public string? Titulo { get; set; }
        public string? Autores { get; set; }
        public string? Editor { get; set; }

        public string? DisplayText => $"{Titulo} - {Autores} - {Editor} - {Isbn}";
    }
}
