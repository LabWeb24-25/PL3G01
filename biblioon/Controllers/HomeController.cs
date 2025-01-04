using System.Diagnostics;
using biblioon.Data;
using biblioon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace biblioon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Copyright()
        {
            return View();
        }

        public IActionResult Cookies()
        {
            return View();
        }

        public IActionResult Books()
        {
            return View();
        }

        [HttpGet("Home/Livro/{id?}")]
        public IActionResult Livro(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var livro = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Include(u => u.UniLivros)
                .FirstOrDefault(a => a.Isbn == id);

            if (livro == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["Title"] = $"Livro: {livro.Titulo}";

            return View(livro);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        /// autor
        [HttpGet("Home/Autor/{id?}")]
        public IActionResult Autor(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var autor = _context.Autores
                .Include(a => a.EdiLivros)
                    .ThenInclude(l => l.Autores)
                .FirstOrDefault(a => a.Id == id);

            if (autor == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["Title"] = $"Autor: {autor.Nome}";

            return View(autor);
        }

        public IActionResult historico()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
