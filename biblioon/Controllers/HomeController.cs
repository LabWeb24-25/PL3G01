using System.Diagnostics;
using biblioon.Data;
using biblioon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Drawing.Printing;

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
            var lExatas = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "exatas"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lExatas"] = lExatas;

            
            var lNaturais = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "naturais"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lNaturais"] = lNaturais;

            var lTecnologicas = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "tecnologicas"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lTecnologicas"] = lTecnologicas;

            var lAgrarias = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "agrarias"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lAgrarias"] = lAgrarias;

            var lHumanas = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "humanasesociais"))               
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lHumanas"] = lHumanas;

            var lSaude = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "saude"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lSaude"] = lSaude;

            var lFiccao = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "ficcao"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lFiccao"] = lFiccao;

            var lNaoFiccao = _context.EdiLivros
                .Include(a => a.Autores)
                .Include(g => g.Generos)
                .Include(e => e.Editor)
                .Where(l => l.Generos.Any(g => g.ShName == "naoficcao"))
                .OrderByDescending(l => l.NEmprestimos)
                .Take(5)
                .ToList();

            ViewData["lNaoFiccao"] = lNaoFiccao;

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

        public IActionResult Books(string q, string npp, string np, string sort, string generos, string autores, string editores, string gsn)
        {
            var cgeneros = _context.Generos.ToList();
            var cautores = _context.Autores.ToList();
            var ceditores = _context.Editores.ToList();
            List<EdiLivro>? livros = null;

            ViewData["Generos"] = cgeneros;
            ViewData["Autores"] = cautores;
            ViewData["Editores"] = ceditores;

            var numlivros = _context.EdiLivros.Count();

            if (!string.IsNullOrEmpty(q))
            {
                livros = _context.EdiLivros
                    .Include(a => a.Autores)
                    .Include(g => g.Generos)
                    .Include(e => e.Editor)
                    .Where(l => l.Titulo.Contains(q) || l.Autores.Any(a => a.Nome.Contains(q)) || l.Isbn.Contains(q))
                    .ToList();

            } else
            {
                livros = _context.EdiLivros
                    .Include(a => a.Autores)
                    .Include(g => g.Generos)
                    .Include(e => e.Editor)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(generos))
            {
                var generosList = generos.Split(";").ToList();
                livros = livros.Where(l => l.Generos.Any(g => generosList.Contains(g.GeneroId))).ToList();
            }

            if (!string.IsNullOrEmpty(gsn))
            {
                var generosList = gsn.Split(";").ToList();
                livros = livros.Where(l => l.Generos.Any(g => g.ShName != null && generosList.Contains(g.ShName))).ToList();
            }


            if (!string.IsNullOrEmpty(autores))
            {
                var autoresList = autores.Split(";").ToList();
                livros = livros.Where(l => l.Autores.Any(a => autoresList.Contains(a.Id))).ToList();
            }

            if (!string.IsNullOrEmpty(editores))
            {
                var editoresList = editores.Split(";").ToList();
                livros = livros.Where(l => editoresList.Contains(l.Editor.Id)).ToList();
            }



            if (!string.IsNullOrEmpty(sort))
            {
                /*
                <div class="seletorOrdenarPor">
                    <label for="sort">Ordenar por:</label>
                    <select id="sort" onchange="updateSortQueryString()">
                        <option value="popular">Mais requisitados</option>
                        <option value="popular-inv" >Menos requisitados</option>
                        <option value="alpha-az" >Alfabeticamente (A-Z)</option>
                        <option value="alpha-za" >Alfabeticamente (Z-A)</option>
                        <option value="data-desc" >Mais recente para mais antigo</option>
                        <option value="data-asc" >Mais antigo para mais recente</option>
                    </select>
                </div>
                */
                switch (sort)
                {
                    case "popular":
                        livros = livros.OrderByDescending(l => l.NEmprestimos).ThenBy(l => l.Titulo).ToList();
                        break;
                    case "popular-inv":
                        livros = livros.OrderBy(l => l.NEmprestimos).ThenBy(l => l.Titulo).ToList();
                        break;
                    case "alpha-az":
                        livros = livros.OrderBy(l => l.Titulo).ToList();
                        break;
                    case "alpha-za":
                        livros = livros.OrderByDescending(l => l.Titulo).ToList();
                        break;
                    case "data-desc":
                        livros = livros.OrderByDescending(l => l.DataPublicacao).ToList();
                        break;
                    case "data-asc":
                        livros = livros.OrderBy(l => l.DataPublicacao).ToList();
                        break;
                    default:
                        livros = livros.OrderByDescending(l => l.NEmprestimos).ThenBy(l => l.Titulo).ToList();
                        break;
                }
            }



            // PAGINAÇÃO

            int numPorPagina = 15; // default
            int numPagina = 1; // default

            if (!string.IsNullOrEmpty(npp) && int.TryParse(npp, out int parsedNpp))
            {
                numPorPagina = parsedNpp;
            }

            if (!string.IsNullOrEmpty(np) && int.TryParse(np, out int parsedNp))
            {
                numPagina = parsedNp;
            }

            int skipQuant = (numPagina - 1) * numPorPagina;

            var numResultados = livros.Count();

            livros = livros.Skip(skipQuant).Take(numPorPagina).ToList();

            var numNaPagina = livros.Count();

            ViewData["textoresultados"] = $"A mostrar {numNaPagina} de {numResultados} resultados";

            ViewData["currentPage"] = numPagina;
            ViewData["totalPages"] = (int)Math.Ceiling((double)numResultados / numPorPagina);

            ViewData["resultados"] = livros;


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

            unit.EdiLivro.NEmprestimos++;
            _context.EdiLivros.Update(unit.EdiLivro);

            _context.SaveChanges();

            return RedirectToAction("Historico");
        }

        [Authorize(Roles = "Leitor")]
        public IActionResult Historico(string sort)
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
                .ToList();

            switch (sort)
            {
                case "asc":
                    emprestimos = emprestimos.OrderBy(e => e.DataRequisitado).ToList();
                    break;
                case "desc":
                default:
                    emprestimos = emprestimos.OrderByDescending(e => e.DataRequisitado).ToList();
                    break;
            }

            return View(emprestimos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
