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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UniLivros.Include(u => u.EdiLivro);
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
            if (ModelState.IsValid)
            {
                _context.Add(uniLivro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
