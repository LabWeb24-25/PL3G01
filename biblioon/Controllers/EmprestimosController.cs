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
    public class EmprestimosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmprestimosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emprestimos
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Emprestimos.Include(e => e.BibliotecarioEntrega).Include(e => e.BibliotecarioLevantamento).Include(e => e.EdiLivro).Include(e => e.Leitor).Include(e => e.UniLivro);
            return View("/Views/Bibliotecario/Emprestimos/Index.cshtml", await applicationDbContext.ToListAsync());
        }

        // GET: Emprestimos/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.BibliotecarioEntrega)
                .Include(e => e.BibliotecarioLevantamento)
                .Include(e => e.EdiLivro)
                .Include(e => e.Leitor)
                .Include(e => e.UniLivro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Emprestimos/Details.cshtml", emprestimo);
        }


        // GET: Emprestimos/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            ViewData["IdBibliotecarioEntrega"] = new SelectList(_context.Bibliotecarios, "Id", "Id", emprestimo.IdBibliotecarioEntrega);
            ViewData["IdBibliotecarioLevantamento"] = new SelectList(_context.Bibliotecarios, "Id", "Id", emprestimo.IdBibliotecarioLevantamento);
            ViewData["EdiLivroISBN"] = new SelectList(_context.EdiLivros, "Isbn", "Isbn", emprestimo.EdiLivroISBN);
            ViewData["LeitorId"] = new SelectList(_context.Leitores, "Id", "Id", emprestimo.LeitorId);
            ViewData["UniLivroId"] = new SelectList(_context.UniLivros, "Id", "Id", emprestimo.UniLivroId);
            return View("/Views/Bibliotecario/Emprestimos/Edit.cshtml", emprestimo);
        }

        // POST: Emprestimos/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,LeitorId,UniLivroId,EdiLivroISBN,DataRequisitado,DataLimiteEntrega,DataLevantamento,DataEntrega,IsLevantado,IsEntregue,IdBibliotecarioLevantamento,IdBibliotecarioEntrega")] Emprestimo emprestimo)
        {
            if (id != emprestimo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprestimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoExists(emprestimo.Id))
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
            ViewData["IdBibliotecarioEntrega"] = new SelectList(_context.Bibliotecarios, "Id", "Id", emprestimo.IdBibliotecarioEntrega);
            ViewData["IdBibliotecarioLevantamento"] = new SelectList(_context.Bibliotecarios, "Id", "Id", emprestimo.IdBibliotecarioLevantamento);
            ViewData["EdiLivroISBN"] = new SelectList(_context.EdiLivros, "Isbn", "Isbn", emprestimo.EdiLivroISBN);
            ViewData["LeitorId"] = new SelectList(_context.Leitores, "Id", "Id", emprestimo.LeitorId);
            ViewData["UniLivroId"] = new SelectList(_context.UniLivros, "Id", "Id", emprestimo.UniLivroId);
            return View("/Views/Bibliotecario/Emprestimos/Edit.cshtml", emprestimo);
        }

        // GET: Emprestimos/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.BibliotecarioEntrega)
                .Include(e => e.BibliotecarioLevantamento)
                .Include(e => e.EdiLivro)
                .Include(e => e.Leitor)
                .Include(e => e.UniLivro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Emprestimos/Delete.cshtml", emprestimo);
        }

        // POST: Emprestimos/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoExists(string id)
        {
            return _context.Emprestimos.Any(e => e.Id == id);
        }
    }
}
