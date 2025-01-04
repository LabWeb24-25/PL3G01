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
using biblioon.ViewModels;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Bibliotecario, Admin")]
    [Route("Bibliotecario/[controller]")]
    public class UniLivrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UniLivrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UniLivros
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string? isbn)
        {
            var applicationDbContext = _context.UniLivros
                .Include(u => u.EdiLivro)
                .Include(e => e.EdiLivro.Autores)
                .Include(e => e.EdiLivro.Editor);

            var count = await applicationDbContext.CountAsync();

            ViewData["amostrar"] = "A mostrar todas as " + count + " unidades";

            if (!string.IsNullOrEmpty(isbn))
            {
                applicationDbContext = _context.UniLivros
                    .Include(u => u.EdiLivro)
                    .Where(u => u.EdiLivro.Isbn == isbn)
                    .Include(e => e.EdiLivro.Autores)
                    .Include(e => e.EdiLivro.Editor);

                var fcount = await applicationDbContext.CountAsync();

                ViewData["amostrar"] = "A mostrar todas as " + count + " unidades com ISBN \"" + isbn + "\", de um total de " + count + " unidades.";
            }

            return View("/Views/Bibliotecario/UniLivros/Index.cshtml", await applicationDbContext.ToListAsync());
        }



        // GET: UniLivros/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uniLivro = await _context.UniLivros
                .Include(u => u.EdiLivro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uniLivro == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/UniLivros/Details.cshtml", uniLivro);
        }

        // GET: UniLivros/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var ediLivros = _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Editor)
                .Select(e => new EdiLivroViewModel
                {
                    Isbn = e.Isbn,
                    Titulo = e.Titulo,
                    Autores = string.Join(", ", e.Autores.Select(a => a.Nome)),
                    Editor = e.Editor.Nome
                }).ToList();

            ViewBag.Isbn = new SelectList(ediLivros, "Isbn", "DisplayText");
            return View("/Views/Bibliotecario/UniLivros/Create.cshtml");
        }

        // POST: UniLivros/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Isbn,Estado,PrecoAquisicao,DataAquisicao,Requisitado,Disponivel,Anotacoes")] UniLivro uniLivro)
        {
            var ediLivros = _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Editor)
                .Select(e => new EdiLivroViewModel
                {
                    Isbn = e.Isbn,
                    Titulo = e.Titulo,
                    Autores = string.Join(", ", e.Autores.Select(a => a.Nome)),
                    Editor = e.Editor.Nome
                }).ToList();

            Console.WriteLine(uniLivro.Isbn);

            // Use Include to ensure Editor is loaded
            var ediLivro = await _context.EdiLivros
                .Include(e => e.Editor)
                .Include(e => e.Autores)
                .Include(e => e.Generos)
                .FirstOrDefaultAsync(e => e.Isbn == uniLivro.Isbn);

            if (ediLivro == null)
            {
                ModelState.AddModelError("Isbn", "The selected ISBN does not exist.");
                ViewBag.Isbn = new SelectList(ediLivros, "Isbn", "DisplayText", uniLivro.Isbn);
                return View("/Views/Bibliotecario/UniLivros/Create.cshtml");
            }

            Console.WriteLine(ediLivro.Titulo);
            Console.WriteLine(ediLivro.Isbn);
            Console.WriteLine(ediLivro.Editor.Nome);

            ModelState.Remove("EdiLivro");
            uniLivro.EdiLivro = ediLivro;

            TryValidateModel(uniLivro);

            if (ModelState.IsValid)
            {
                _context.Add(uniLivro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Add debug information on why the model is invalid
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            ViewBag.Isbn = new SelectList(ediLivros, "Isbn", "DisplayText", uniLivro.Isbn);
            return View("/Views/Bibliotecario/UniLivros/Create.cshtml", uniLivro);
        }

        // GET: UniLivros/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uniLivro = await _context.UniLivros.FindAsync(id);
            if (uniLivro == null)
            {
                return NotFound();
            }
            var ediLivros = _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Editor)
                .Select(e => new EdiLivroViewModel
                {
                    Isbn = e.Isbn,
                    Titulo = e.Titulo,
                    Autores = string.Join(", ", e.Autores.Select(a => a.Nome)),
                    Editor = e.Editor.Nome
                }).ToList();

            ViewBag.Isbn = new SelectList(ediLivros, "Isbn", "DisplayText", uniLivro.Isbn);
            return View("/Views/Bibliotecario/UniLivros/Edit.cshtml", uniLivro);
        }

        // POST: UniLivros/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Isbn,Estado,PrecoAquisicao,DataAquisicao,Requisitado,Disponivel,Anotacoes")] UniLivro uniLivro)
        {
            if (id != uniLivro.Id)
            {
                return NotFound();
            }

            var ediLivros = _context.EdiLivros
                .Include(e => e.Autores)
                .Include(e => e.Editor)
                .Select(e => new EdiLivroViewModel
                {
                    Isbn = e.Isbn,
                    Titulo = e.Titulo,
                    Autores = string.Join(", ", e.Autores.Select(a => a.Nome)),
                    Editor = e.Editor.Nome
                }).ToList();

            // Use Include to ensure Editor is loaded
            var ediLivro = await _context.EdiLivros
                .Include(e => e.Editor)
                .Include(e => e.Autores)
                .Include(e => e.Generos)
                .FirstOrDefaultAsync(e => e.Isbn == uniLivro.Isbn);

            if (ediLivro == null)
            {
                ModelState.AddModelError("Isbn", "The selected ISBN does not exist.");
                ViewBag.Isbn = new SelectList(ediLivros, "Isbn", "DisplayText", uniLivro.Isbn);
                return View("/Views/Bibliotecario/UniLivros/Edit.cshtml", uniLivro);
            }

            ModelState.Remove("EdiLivro");
            uniLivro.EdiLivro = ediLivro;

            TryValidateModel(uniLivro);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uniLivro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniLivroExists(uniLivro.Id))
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
            else
            {
                // Add debug information on why the model is invalid
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            ViewBag.Isbn = new SelectList(ediLivros, "Isbn", "DisplayText", uniLivro.Isbn);
            return View("/Views/Bibliotecario/UniLivros/Edit.cshtml", uniLivro);
        }

        // GET: UniLivros/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uniLivro = await _context.UniLivros
                .Include(u => u.EdiLivro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uniLivro == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/UniLivros/Delete.cshtml", uniLivro);
        }

        // POST: UniLivros/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uniLivro = await _context.UniLivros.FindAsync(id);
            if (uniLivro != null)
            {
                _context.UniLivros.Remove(uniLivro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniLivroExists(string id)
        {
            return _context.UniLivros.Any(e => e.Id == id);
        }
    }
}
