using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Bibliotecario, Admin")]
    public class BibliotecarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListarEdiLivros()
        {
            return View();
        }

        public IActionResult CreateEdiLivro()
        {
            return View();
        }
    }
}
