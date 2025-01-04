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
    public class GenerosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenerosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Generos
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View("/Views/Bibliotecario/Generos/Index.cshtml", await _context.Generos.ToListAsync());
        }

        // GET: Generos/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos
                .FirstOrDefaultAsync(m => m.GeneroId == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Generos/Details.cshtml", genero);
        }

        // GET: Generos/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("/Views/Bibliotecario/Generos/Create.cshtml");
        }

        // POST: Generos/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GeneroId,Nome")] Genero genero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("/Views/Bibliotecario/Generos/Create.cshtml", genero);
        }

        // GET: Generos/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
            {
                return NotFound();
            }
            return View("/Views/Bibliotecario/Generos/Edit.cshtml", genero);
        }

        // POST: Generos/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GeneroId,Nome")] Genero genero)
        {
            if (id != genero.GeneroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GeneroExists(genero.GeneroId))
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
            return View("/Views/Bibliotecario/Generos/Edit.cshtml", genero);
        }

        // GET: Generos/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Generos
                .FirstOrDefaultAsync(m => m.GeneroId == id);
            if (genero == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Generos/Delete.cshtml", genero);
        }

        // POST: Generos/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero != null)
            {
                _context.Generos.Remove(genero);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GeneroExists(string id)
        {
            return _context.Generos.Any(e => e.GeneroId == id);
        }
    }
}
