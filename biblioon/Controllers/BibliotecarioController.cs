using biblioon.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Bibliotecario, Admin")]
    public class BibliotecarioController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public BibliotecarioController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Bibliotecario/Reqs")]
        public IActionResult ReqsIndex()
        {
            
            var reqs = _context.Emprestimos
                .Include(e => e.BibliotecarioEntrega)
                .Include(e => e.BibliotecarioLevantamento)
                .Include(e => e.EdiLivro)
                    .ThenInclude(e => e.Autores)
                .Include(e => e.EdiLivro)
                    .ThenInclude(e => e.Generos)
                .Include(e => e.EdiLivro)
                    .ThenInclude(e => e.Editor)
                .Include(e => e.Leitor)
                    .ThenInclude(e => e.User)
                .Include(e => e.UniLivro)
                .OrderByDescending(e => e.DataRequisitado)
                .ToList();

            var porlevantar = reqs.Where(e => e.DataLevantamento == null).ToList();

            var porentregar = reqs.Where(e => e.DataEntrega == null && e.DataLevantamento != null && e.DataLimiteEntrega >= DateTime.Now).ToList();

            var ematraso = reqs.Where(e => e.DataEntrega == null && e.DataLevantamento != null && e.DataLimiteEntrega < DateTime.Now).ToList();

            var restantes = reqs.Where(e => e.DataEntrega != null && e.DataLevantamento != null).ToList();

            ViewData["porlevantar"] = porlevantar;
            ViewData["porentregar"] = porentregar;
            ViewData["ematraso"] = ematraso;
            ViewData["restantes"] = restantes;
            ViewData["reqs"] = reqs;

            return View("/Views/Bibliotecario/Reqs/Index.cshtml");
        }

    }
}
