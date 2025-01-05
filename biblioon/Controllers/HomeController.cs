using System.Diagnostics;
using biblioon.Data;
using biblioon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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


        // requisitar
        [Authorize(Roles = "Leitor")]
        [HttpGet("Home/Requisitar/{id?}")]
        public IActionResult Requisitar(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var unit = _context.UniLivros
              .Include(u => u.EdiLivro)
                  .ThenInclude(e => e.Autores)
              .Include(u => u.EdiLivro)
                  .ThenInclude(e => e.Generos)
              .Include(u => u.EdiLivro)
                  .ThenInclude(e => e.Editor)
              .FirstOrDefault(u => u.Id == id);

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var leitor = _context.Leitores
                .Include(l => l.User)
                .FirstOrDefault(l => l.Id == userid);

            if (unit == null || userid == null || leitor == null || leitor.IsBanido)
            {

                return RedirectToAction("Index");
            }

            /*
            var newrequisicao = new Emprestimo
            {
                Id = Guid.NewGuid().ToString(),
                UniLivroId = id,
                EdiLivroISBN = unit.Isbn,
                LeitorId = userid,
                DataRequisitado = DateTime.Now,
                DataLimiteEntrega = DateTime.Now.AddDays(15),
                DataEntrega = null,
                IsEntregue = false,
                IsLevantado = false,
                UniLivro = unit,
                EdiLivro = unit.EdiLivro,
                Leitor = user,
            };
            */

            var data = DateTime.Now;
            var dataLimite = data.AddDays(15);

            ViewData["DataRequisitado"] = data.ToString("dd/MM/yyyy");
            ViewData["DataLimiteEntrega"] = dataLimite.ToString("dd/MM/yyyy");
            ViewData["NomeLeitor"] = leitor.User.NomeCompleto;
            ViewData["IdLeitor"] = leitor.Id;
            ViewData["Title"] = $"Requisitar: {unit.EdiLivro.Titulo}";

            return View(unit);
        }


        // requisitar POST
       // [Authorize(Roles = "Leitor")]
        [HttpPost]
        [Route("home/requisitar")]
        public IActionResult RequisitarPost(string? id, string? idLeitor)
        {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(idLeitor))
            {
                Console.WriteLine("idform is null");
                return RedirectToAction("Index");
            }

            var unit = _context.UniLivros
              .Include(u => u.EdiLivro)
                  .ThenInclude(e => e.Autores)
              .Include(u => u.EdiLivro)
                  .ThenInclude(e => e.Generos)
              .Include(u => u.EdiLivro)
                  .ThenInclude(e => e.Editor)
              .FirstOrDefault(u => u.Id == id);

            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var leitor = _context.Leitores
                .Include(l => l.User)
                .FirstOrDefault(l => l.Id == userid);

            if (unit == null || userid == null || leitor == null || leitor.IsBanido)
            {
                return RedirectToAction("Index");
            }

            var borrowedBooksCount = _context.Emprestimos
              .Count(e => e.LeitorId == userid && e.DataEntrega == null);

            var borrowedSameISBNCount = _context.Emprestimos
              .Where(e => e.LeitorId == userid && e.DataEntrega == null && e.EdiLivroISBN == unit.EdiLivro.Isbn)
              .ToList();


            if (borrowedBooksCount >= 2)
            {
                return RedirectToAction("Historico");
            }

            if (borrowedSameISBNCount.Count > 0)
            {
                return RedirectToAction("Historico");
            }

            if (unit.Requisitado)
            {
                return RedirectToAction("Livro", new { id = unit.Id });
            }

            var data = DateTime.Now;
            var dataLimite = data.AddDays(15);

            var newrequisicao = new Emprestimo
            {
                Id = Guid.NewGuid().ToString(),
                UniLivroId = id,
                EdiLivroISBN = unit.Isbn,
                LeitorId = userid,
                DataRequisitado = data,
                DataLimiteEntrega = dataLimite,
                DataEntrega = null,
                IsEntregue = false,
                IsLevantado = false,
                UniLivro = unit,
                EdiLivro = unit.EdiLivro,
                Leitor = leitor,
            };

            Console.WriteLine("newrequisicao: " + newrequisicao);
            _context.Emprestimos.Add(newrequisicao);

            unit.Requisitado = true;
            _context.UniLivros.Update(unit);

            leitor.NRequisicoes++;
            _context.Leitores.Update(leitor);


            _context.SaveChanges();

            return RedirectToAction("Historico");
        }

        [Authorize(Roles = "Leitor")]
        public IActionResult Historico()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var emprestimos = _context.Emprestimos
                .Include(e => e.UniLivro)
                .Include(e => e.EdiLivro)
                    .ThenInclude(e => e.Autores)
                .Include(e => e.EdiLivro)
                    .ThenInclude(e => e.Generos)
                .Include(e => e.EdiLivro)
                    .ThenInclude(e => e.Editor)
                .Where(e => e.LeitorId == userid)
                .OrderByDescending(e => e.DataRequisitado)
                .ToList();

            return View(emprestimos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
