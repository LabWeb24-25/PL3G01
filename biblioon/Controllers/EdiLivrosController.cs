using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using biblioon.Data;
using biblioon.Models;
using Microsoft.AspNetCore.Authorization;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Bibliotecario, Admin")]
    [Route("Bibliotecario/[controller]")]
    public class EdiLivrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EdiLivrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EdiLivros
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EdiLivros
                .Include(e => e.Editor)
                .Include(e => e.Autores)
                .Include(e => e.Generos)
                ;
            return View("/Views/Bibliotecario/EdiLivros/Index.cshtml", await applicationDbContext.ToListAsync());
        }

        // GET: EdiLivros/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ediLivro = await _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Editor)
                .Include(e => e.Generos)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (ediLivro == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/EdiLivros/Details.cshtml", ediLivro);
        }

        // GET: EdiLivros/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName");
            ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
            ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
            return View("/Views/Bibliotecario/EdiLivros/Create.cshtml");
        }

        // POST: EdiLivros/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,BarCode,Titulo,Sinopse,Capa,Idioma,DescFisica,DataPublicacao,EditorId")] EdiLivro ediLivro, List<string> SelectedAuthorIds, List<string> SelectedGeneroIds)
        {
            var editor = await _context.Editores.FindAsync(ediLivro.EditorId);

            if (editor == null)
            {
                ModelState.AddModelError("EditorId", "Invalid Editor ID.");
                ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName", ediLivro.EditorId);
                ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
                ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
                return View("/Views/Bibliotecario/EdiLivros/Create.cshtml", ediLivro);
            }

            ModelState.Remove("Editor");
            ediLivro.Editor = editor;

            if (SelectedAuthorIds != null && SelectedAuthorIds.Any())
            {
                List<Autor> auts = new();
                foreach (var authorId in SelectedAuthorIds)
                {
                    var author = await _context.Autores.FindAsync(authorId);
                    if (author != null)
                    {
                        auts.Add(author);
                    }
                }

                ediLivro.Autores = auts;
            }
            else
            {
                ModelState.AddModelError("Autores", "Autores está vazio");
            }



            if (SelectedGeneroIds != null && SelectedGeneroIds.Any())
            {
                List<Genero> gens = new();
                foreach (var generoId in SelectedGeneroIds)
                {
                    var genero = await _context.Generos.FindAsync(generoId);
                    if (genero != null)
                    {
                        gens.Add(genero);
                    }
                }

                ediLivro.Generos = gens;
            }
            else
            {
                ModelState.AddModelError("Generos", "Generos está vazio");
            }



            // Revalidate the ModelState
            TryValidateModel(ediLivro);

            if (!ModelState.IsValid)
            {
                ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName", ediLivro.EditorId);
                ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
                ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
                return View("/Views/Bibliotecario/EdiLivros/Create.cshtml", ediLivro);
            }

            _context.Add(ediLivro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: EdiLivros/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ediLivro = await _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Editor)
                .Include(e => e.Generos)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (ediLivro == null)
            {
                return NotFound();
            }

            ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName", ediLivro.EditorId);
            ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
            ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
            return View("/Views/Bibliotecario/EdiLivros/Edit.cshtml", ediLivro);
        }

        // POST: EdiLivros/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Isbn,BarCode,Titulo,Sinopse,Capa,Idioma,DescFisica,DataPublicacao,EditorId")] EdiLivro ediLivro, List<string> SelectedAuthorIds, List<string> SelectedGeneroIds)
        {
            if (id != ediLivro.Isbn)
            {
                return NotFound();
            }

            var editor = await _context.Editores.FindAsync(ediLivro.EditorId);

            if (editor == null)
            {
                ModelState.AddModelError("EditorId", "Invalid Editor ID.");
                ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName", ediLivro.EditorId);
                ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
                ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
                return View("/Views/Bibliotecario/EdiLivros/Edit.cshtml", ediLivro);
            }

            ModelState.Remove("Editor");
            ediLivro.Editor = editor;


            if (SelectedGeneroIds != null && SelectedGeneroIds.Any())
            {
                List<Genero> gens = new();
                foreach (var generoId in SelectedGeneroIds)
                {
                    var genero = await _context.Generos.FindAsync(generoId);
                    if (genero != null)
                    {
                        gens.Add(genero);
                    }
                }

                ediLivro.Generos = gens;
            }
            else
            {
                ModelState.AddModelError("Generos", "Generos está vazio");
            }

            if (SelectedAuthorIds != null)
            {
                // Get the current authors
                var currentAuthors = ediLivro.Autores.Select(a => a.Id).ToList();

                // Remove authors that are not in the selected list
                foreach (var author in currentAuthors)
                {
                    if (!SelectedAuthorIds.Contains(author))
                    {
                        var authorToRemove = ediLivro.Autores.FirstOrDefault(a => a.Id == author);
                        if (authorToRemove != null)
                        {
                            ediLivro.Autores.Remove(authorToRemove);
                        }
                    }
                }

                // Add new authors that are not already in the list
                foreach (var authorId in SelectedAuthorIds)
                {
                    if (!currentAuthors.Contains(authorId))
                    {
                        var author = await _context.Autores.FindAsync(authorId);
                        if (author != null)
                        {
                            ediLivro.Autores.Add(author);
                        }
                    }
                }
            }

            // Revalidate the ModelState
            TryValidateModel(ediLivro);

            if (!ModelState.IsValid)
            {
                ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName", ediLivro.EditorId);
                ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
                ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
                return View("/Views/Bibliotecario/EdiLivros/Edit.cshtml", ediLivro);
            }

            try
            {
                _context.Update(ediLivro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EdiLivroExists(ediLivro.Isbn))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: EdiLivros/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ediLivro = await _context.EdiLivros
                .Include(e => e.Editor)
                .Include(e => e.Autores)
                .Include(e => e.Generos)
                .FirstOrDefaultAsync(m => m.Isbn == id);
            if (ediLivro == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/EdiLivros/Delete.cshtml", ediLivro);
        }

        // POST: EdiLivros/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ediLivro = await _context.EdiLivros.FindAsync(id);
            if (ediLivro != null)
            {
                _context.EdiLivros.Remove(ediLivro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EdiLivroExists(string id)
        {
            return _context.EdiLivros.Any(e => e.Isbn == id);
        }
    }
}
