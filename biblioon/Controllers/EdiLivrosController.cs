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
            var applicationDbContext = _context.EdiLivros.Include(e => e.Editor);
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
                .Include(e => e.Editor)
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
            ViewData["EditorId"] = new SelectList(_context.Editores, "Id", "Id");
            return View("/Views/Bibliotecario/EdiLivros/Create.cshtml");
        }

        // POST: EdiLivros/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Isbn,Titulo,Sinopse,Capa,Idioma,DescFisica,DataPublicacao,EditorId")] EdiLivro ediLivro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ediLivro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EditorId"] = new SelectList(_context.Editores, "Id", "Id", ediLivro.EditorId);
            return View("/Views/Bibliotecario/EdiLivros/Create.cshtml", ediLivro);
        }

        // GET: EdiLivros/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ediLivro = await _context.EdiLivros.FindAsync(id);
            if (ediLivro == null)
            {
                return NotFound();
            }
            ViewData["EditorId"] = new SelectList(_context.Editores, "Id", "Id", ediLivro.EditorId);
            return View("/Views/Bibliotecario/EdiLivros/Edit.cshtml", ediLivro);
        }

        // POST: EdiLivros/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Isbn,Titulo,Sinopse,Capa,Idioma,DescFisica,DataPublicacao,EditorId")] EdiLivro ediLivro)
        {
            if (id != ediLivro.Isbn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            ViewData["EditorId"] = new SelectList(_context.Editores, "Id", "Id", ediLivro.EditorId);
            return View("/Views/Bibliotecario/EdiLivros/Edit.cshtml", ediLivro);
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
