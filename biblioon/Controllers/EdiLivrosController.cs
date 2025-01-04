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
        public async Task<IActionResult> Create([Bind("Isbn,BarCode,Titulo,Sinopse,Idioma,DescFisica,DataPublicacao,EditorId")] EdiLivro ediLivro, List<string> SelectedAuthorIds, List<string> SelectedGeneroIds, IFormFile? capaFile)
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


            if (capaFile != null && capaFile.Length > 0)
            {
                var fileName = Path.GetFileName(capaFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/covers", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await capaFile.CopyToAsync(stream);
                }

                ediLivro.Capa = "/images/covers/" + fileName;
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
        public async Task<IActionResult> Edit(string id, [Bind("Isbn,BarCode,Titulo,Sinopse,Capa,Idioma,DescFisica,DataPublicacao,EditorId")] EdiLivro ediLivro, List<string> SelectedAuthorIds, List<string> SelectedGeneroIds, IFormFile? capaFile, bool removeCapa)
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

            // Clear existing relationships
            var existingLivro = await _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Generos)
                .FirstOrDefaultAsync(m => m.Isbn == id);

            if (existingLivro == null)
            {
                return NotFound();
            }

            existingLivro.Autores.Clear();
            existingLivro.Generos.Clear();

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

                existingLivro.Generos = gens;
            }
            else
            {
                ModelState.AddModelError("Generos", "Generos está vazio");
            }

            if (SelectedAuthorIds != null)
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

                existingLivro.Autores = auts;
            }

            // Handle the picture upload
            if (capaFile != null && capaFile.Length > 0)
            {
                // Delete the old photo if it exists
                if (!string.IsNullOrEmpty(existingLivro.Capa))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingLivro.Capa.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var fileName = Path.GetFileName(capaFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/covers", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await capaFile.CopyToAsync(stream);
                }

                existingLivro.Capa = "/images/covers/" + fileName;
            }
            else if (removeCapa && !string.IsNullOrEmpty(existingLivro.Capa))
            {
                // Remove the existing picture
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingLivro.Capa.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                existingLivro.Capa = null;
            }


            // Revalidate the ModelState
            TryValidateModel(existingLivro);

            // Log the model state errors
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"ModelState Error: {state.Key} - {error.ErrorMessage}");
                    }
                }

                ViewBag.EditorId = new SelectList(_context.Editores.Select(e => new { e.Id, DisplayName = $"{e.Nome} ({e.Id})" }), "Id", "DisplayName", ediLivro.EditorId);
                ViewBag.Autores = _context.Autores.Select(a => new { a.Id, a.Nome }).ToList();
                ViewBag.Generos = _context.Generos.Select(g => new { g.GeneroId, g.Nome }).ToList();
                return View("/Views/Bibliotecario/EdiLivros/Edit.cshtml", ediLivro);
            }

            try
            {
                _context.Update(existingLivro);
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
                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(ediLivro.Capa))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ediLivro.Capa.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.EdiLivros.Remove(ediLivro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EdiLivroExists(string id)
        {
            return _context.EdiLivros.Any(e => e.Isbn == id);
        }
    }
}
