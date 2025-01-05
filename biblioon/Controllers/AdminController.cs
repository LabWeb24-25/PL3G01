using biblioon.Data;
using biblioon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<BibliotecarioController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ILogger<BibliotecarioController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)

        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Este é um field obrigatório.")]
            [Display(Name = "Nome completo")]
            public required string NomeCompleto { get; set; }

            [Required(ErrorMessage = "Este é um field obrigatório.")]
            [Display(Name = "Nome de utilizador")]
            public required string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "As passwords não coincidem.")]
            public string ConfirmPassword { get; set; }

        }



        [HttpGet("Admin/mBibliotecarios")]
        public async Task<IActionResult> mBibliotecarios()
        {
            var users = await _context.Users.ToListAsync();
            var bibliotecarios = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "PreBibliotecario"))
                {
                    bibliotecarios.Add(user);
                }
            }

            ViewData["bibliotecarios"] = bibliotecarios;


            var bibliotecariosAtivos = _context.Bibliotecarios
                .Include(b => b.User)
                .Include(a => a.AdminAtivador)
                    .ThenInclude(a => a.User)
                .Where(b => b.IsAtivado == true).ToList();

            ViewData["bibliotecariosAtivos"] = bibliotecariosAtivos;

            return View("/Views/Admin/mBibliotecarios/Index.cshtml");
        }


        [HttpPost("Admin/mBibliotecarios")]
        public async Task<IActionResult> ChangeRole(string userId, string op)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (op == "enable")
            {

                if (await _userManager.IsInRoleAsync(user, "Bibliotecario"))
                {
                    Console.WriteLine("ESTA NO ROLE Bibliotecario");
                    return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                }

                if (await _userManager.IsInRoleAsync(user, "PreBibliotecario"))
                {

                    var currUser = await _userManager.GetUserAsync(User);

                    if (currUser == null)
                    {
                        Console.WriteLine("CURRUSER NULL");
                        return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                    }


                    var currAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == currUser.Id);

                    if (currAdmin == null)
                    {
                        Console.WriteLine("CURRADMIN NULL");
                        return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                    }

                    var existingBibliotecario = await _context.Bibliotecarios.FirstOrDefaultAsync(b => b.Id == user.Id);

                    if (existingBibliotecario == null)
                    {
                        Bibliotecario bInfo = new Bibliotecario
                        {
                            Id = user.Id,
                            DataAtivacao = DateTime.Now,
                            IsAtivado = true,
                            IdAdminAtivador = currUser.Id,
                            AdminAtivador = currAdmin,
                            User = user
                        };

                        _context.Bibliotecarios.Add(bInfo);
                    }
                    else
                    {
                        existingBibliotecario.DataAtivacao = DateTime.Now;
                        existingBibliotecario.IsAtivado = true;
                        existingBibliotecario.IdAdminAtivador = currUser.Id;
                        existingBibliotecario.AdminAtivador = currAdmin;
                        existingBibliotecario.User = user;

                        _context.Bibliotecarios.Update(existingBibliotecario);
                    }


                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, "Bibliotecario");
                    await _userManager.RemoveFromRoleAsync(user, "PreBibliotecario");

                    Console.WriteLine("MUDOU PARA BIBLIOTECARIO");
                    return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                }


            }
            else if (op == "disable")
            {

                if (await _userManager.IsInRoleAsync(user, "PreBibliotecario"))
                {
                    Console.WriteLine("ESTA NO ROLE PreBibliotecario");
                    return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                }

                if (await _userManager.IsInRoleAsync(user, "Bibliotecario"))
                {
                    var bibliotecario = await _context.Bibliotecarios.FirstOrDefaultAsync(b => b.Id == user.Id);
                    if (bibliotecario == null)
                    {
                        Console.WriteLine("BIBLIOTECARIO NULL");
                        return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                    }

                    bibliotecario.IsAtivado = false;
                    _context.Bibliotecarios.Update(bibliotecario);
                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, "PreBibliotecario");
                    await _userManager.RemoveFromRoleAsync(user, "Bibliotecario");

                    Console.WriteLine("MUDOU PARA PREBIBLIOTECARIO");
                    return View("/Views/Admin/mBibliotecarios/Index.cshtml");
                }

            }
            else
            {
                Console.WriteLine("ELSE");
                return View("/Views/Admin/mBibliotecarios/Index.cshtml");
            }

            Console.WriteLine("FIM");
            return View("/Views/Admin/mBibliotecarios/Index.cshtml");
        }


        [HttpGet("Admin/newAdmin")]
        public IActionResult newAdmin()
        {
            return View("/Views/Admin/newAdmin/Index.cshtml");
        }


        [HttpPost("Admin/RegisterAdmin")]
        public async Task<IActionResult> newAdmin(InputModel model)
        {
            if (ModelState.IsValid)
            {
                var currUser = await _userManager.GetUserAsync(User);
                
                if (currUser == null)
                {
                    return NotFound();
                }


                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    DataCriacao = DateTime.Now,
                    DataAtivacao = DateTime.Now,
                    EmailConfirmed = true,
                    IsAdmin = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    Admin adminData = new Admin
                    {
                        Id = user.Id,
                        User = user,
                        DataCriacao = DateTime.Now,
                        IdCriador = currUser.Id,
                        Criador = currUser
                    };

                    _context.Admins.Add(adminData);
                    await _context.SaveChangesAsync();


                    await _userManager.AddToRoleAsync(user, "Admin");
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
