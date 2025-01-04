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
    public class EditoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EditoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Editores
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View("/Views/Bibliotecario/Editores/Index.cshtml", await _context.Editores.ToListAsync());
        }

        // GET: Editores/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editor = await _context.Editores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editor == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Editores/Details.cshtml", editor);
        }

        // GET: Editores/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("/Views/Bibliotecario/Editores/Create.cshtml");
        }

        // POST: Editores/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Foto,Site")] Editor editor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("/Views/Bibliotecario/Editores/Create.cshtml", editor);
        }

        // GET: Editores/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editor = await _context.Editores.FindAsync(id);
            if (editor == null)
            {
                return NotFound();
            }
            return View("/Views/Bibliotecario/Editores/Edit.cshtml", editor);
        }

        // POST: Editores/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Descricao,Foto,Site")] Editor editor)
        {
            if (id != editor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorExists(editor.Id))
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
            return View("/Views/Bibliotecario/Editores/Edit.cshtml", editor);
        }

        // GET: Editores/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editor = await _context.Editores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editor == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Editores/Delete.cshtml", editor);
        }

        // POST: Editores/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var editor = await _context.Editores.FindAsync(id);
            if (editor != null)
            {
                _context.Editores.Remove(editor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditorExists(string id)
        {
            return _context.Editores.Any(e => e.Id == id);
        }
    }
}
