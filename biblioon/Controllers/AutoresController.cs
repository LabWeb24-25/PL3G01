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
    public class AutoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Autores
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View("/Views/Bibliotecario/Autores/Index.cshtml", await _context.Autores.ToListAsync());
        }

        // GET: Autores/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Autores/Details.cshtml", autor);
        }

        // GET: Autores/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("/Views/Bibliotecario/Autores/Create.cshtml");
        }

        // POST: Autores/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Desc")] Autor autor, IFormFile? fotoFile)
        {
            if (ModelState.IsValid)
            {
                if (fotoFile != null && fotoFile.Length > 0)
                {
                    var fileName = Path.GetFileName(fotoFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/authors", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fotoFile.CopyToAsync(stream);
                    }

                    autor.Foto = "/images/authors/" + fileName;
                }

                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View("/Views/Bibliotecario/Autores/Create.cshtml", autor);
            }
        }

        // GET: Autores/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View("/Views/Bibliotecario/Autores/Edit.cshtml", autor);
        }

        // POST: Autores/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Desc,Foto")] Autor autor, IFormFile? fotoFile, bool removePhoto)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAutor = await _context.Autores.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                    if (existingAutor == null)
                    {
                        return NotFound();
                    }

                    if (removePhoto)
                    {
                        // Delete the old photo if it exists
                        if (!string.IsNullOrEmpty(existingAutor.Foto))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAutor.Foto.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }
                        autor.Foto = null;
                    }
                    else if (fotoFile != null && fotoFile.Length > 0)
                    {
                        // Delete the old photo if it exists
                        if (!string.IsNullOrEmpty(existingAutor.Foto))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingAutor.Foto.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Save the new photo
                        var fileName = Path.GetFileName(fotoFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/authors", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await fotoFile.CopyToAsync(stream);
                        }

                        autor.Foto = "/images/authors/" + fileName;
                    }
                    else
                    {
                        // Preserve the existing photo if no new photo is uploaded and removePhoto is not checked
                        autor.Foto = existingAutor.Foto;
                    }

                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id))
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
            return View("/Views/Bibliotecario/Autores/Edit.cshtml", autor);
        }

        // GET: Autores/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View("/Views/Bibliotecario/Autores/Delete.cshtml", autor);
        }

        // POST: Autores/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                // Delete the associated image file if it exists
                if (!string.IsNullOrEmpty(autor.Foto))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", autor.Foto.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(string id)
        {
            return _context.Autores.Any(e => e.Id == id);
        }
    }
}
