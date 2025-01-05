using biblioon.Data;
using biblioon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Bibliotecario, Admin")]
    public class BibliotecarioController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BibliotecarioController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _logger = logger;
            _context = context;
            _userManager = usermanager;
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
                    .ThenInclude(e => e.User)
                .Include(e => e.BibliotecarioLevantamento)
                    .ThenInclude(e => e.User)
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


        [HttpPost("Bibliotecario/Reqs")]
        public async Task<IActionResult> Levantamento(string ReqId, string op)
        {
            var currUser = await _userManager.GetUserAsync(User);

            if (currUser == null) {
                return View("/Views/Bibliotecario/Reqs/Index.cshtml");
            }

            var currBibliotecario = await _context.Bibliotecarios.FirstOrDefaultAsync(b => b.Id == currUser.Id);

            if (currBibliotecario == null)
            {
                return View("/Views/Bibliotecario/Reqs/Index.cshtml");
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.UniLivro)
                .FirstOrDefaultAsync(e => e.Id == ReqId);

            if (emprestimo == null)
            {
                return RedirectToAction("ReqsIndex");
            }

            switch (op)
            {
                case "lev":
                    emprestimo.IsLevantado = true;
                    emprestimo.DataLevantamento = DateTime.Now;
                    emprestimo.IdBibliotecarioLevantamento = currBibliotecario.Id;
                    emprestimo.BibliotecarioLevantamento = currBibliotecario;

                    emprestimo.UniLivro.Disponivel = false;
                    break;
                case "entr":
                    emprestimo.IsEntregue = true;
                    emprestimo.DataEntrega = DateTime.Now;
                    emprestimo.IdBibliotecarioEntrega = currBibliotecario.Id;
                    emprestimo.BibliotecarioEntrega = currBibliotecario;

                    emprestimo.UniLivro.Requisitado = false;
                    emprestimo.UniLivro.Disponivel = true;

                    break;
                default:
                    return RedirectToAction("ReqsIndex");
            }

            _context.Emprestimos.Update(emprestimo);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReqsIndex");
        }

    }

}

