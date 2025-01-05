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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ILogger<BibliotecarioController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)

        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
                    return RedirectToAction("mBibliotecarios");
                }

                if (await _userManager.IsInRoleAsync(user, "PreBibliotecario"))
                {

                    var currUser = await _userManager.GetUserAsync(User);

                    if (currUser == null)
                    {
                        Console.WriteLine("CURRUSER NULL");
                        return RedirectToAction("mBibliotecarios");
                    }


                    var currAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == currUser.Id);

                    if (currAdmin == null)
                    {
                        Console.WriteLine("CURRADMIN NULL");
                        return RedirectToAction("mBibliotecarios");
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
                    return RedirectToAction("mBibliotecarios");
                }


            }
            else if (op == "disable")
            {

                if (await _userManager.IsInRoleAsync(user, "PreBibliotecario"))
                {
                    Console.WriteLine("ESTA NO ROLE PreBibliotecario");
                    return RedirectToAction("mBibliotecarios");
                }

                if (await _userManager.IsInRoleAsync(user, "Bibliotecario"))
                {
                    var bibliotecario = await _context.Bibliotecarios.FirstOrDefaultAsync(b => b.Id == user.Id);
                    if (bibliotecario == null)
                    {
                        Console.WriteLine("BIBLIOTECARIO NULL");
                        return RedirectToAction("mBibliotecarios");
                    }

                    bibliotecario.IsAtivado = false;
                    _context.Bibliotecarios.Update(bibliotecario);
                    await _context.SaveChangesAsync();

                    await _userManager.AddToRoleAsync(user, "PreBibliotecario");
                    await _userManager.RemoveFromRoleAsync(user, "Bibliotecario");

                    Console.WriteLine("MUDOU PARA PREBIBLIOTECARIO");
                    return RedirectToAction("mBibliotecarios");
                }

            }
            else
            {
                Console.WriteLine("ELSE");
                return RedirectToAction("mBibliotecarios");
            }

            Console.WriteLine("FIM");
            return RedirectToAction("mBibliotecarios");
        }


        [HttpGet("Admin/leitorBans")]
        public async Task<IActionResult> leitorBans()
        {
            var users = await _context.Users.ToListAsync();
            var leitores = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Leitor"))
                {
                    leitores.Add(user);
                }
            }

            ViewData["leitores"] = leitores;


            var leitoresBanidos = _context.Bans
                .Include(b => b.User)
                .Include(a => a.Admin)
                    .ThenInclude(a => a.User)
                .Where(b => b.DataFim == null).ToList();

            var bansAnteriores = _context.Bans
                .Include(b => b.User)
                .Include(a => a.Admin)
                    .ThenInclude(a => a.User)
                .Where(b => b.DataFim != null).ToList();

            ViewData["bansanteriores"] = bansAnteriores;

            ViewData["leitoresbanidos"] = leitoresBanidos;

            return View("/Views/Admin/leitorBans/Index.cshtml");
        }

        [HttpPost("Admin/leitorbans")]
        public async Task<IActionResult> ManageBan(string userId, string op) {


            var currUser = await _userManager.GetUserAsync(User);
            if (currUser == null)
            {
                return NotFound();
            }


            var currAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == currUser.Id);
            if (currAdmin == null)
            {
                return NotFound();
            }

            Console.WriteLine("OP: " + op);
            Console.WriteLine("USERID: " + userId);

            if (op == "unban")
            {

                // to unban, remove the role LeitorBanido and add the role Leitor
                // and then set the datafim of the ban to the current date
                // and leitor.isbanido to false



                var ban = _context.Bans
                    .Include(b => b.User)
                    .ThenInclude(b => b.User)
                    .Include(b => b.Admin)
                    .FirstOrDefault(b => b.Id == userId);


                if (ban == null)
                {
                    return NotFound();
                }

                var user = ban.User.User;
                var leitor = ban.User;

                if (await _userManager.IsInRoleAsync(user, "Leitor"))
                {
                    Console.WriteLine("ESTA NO ROLE Leitor");
                    return RedirectToAction("leitorBans");
                }

                if (await _userManager.IsInRoleAsync(user, "LeitorBanido"))
                {

                    if (leitor == null)
                    {
                        Console.WriteLine("leitor NULL");
                        return RedirectToAction("leitorBans");
                    }

                    if (ban == null)
                    {
                        Console.WriteLine("ban NULL");
                        return RedirectToAction("leitorBans");
                    }


                    await _userManager.AddToRoleAsync(user, "Leitor");
                    await _userManager.RemoveFromRoleAsync(user, "LeitorBanido");

                    leitor.IsBanido = false;
                    _context.Leitores.Update(leitor);
                    await _context.SaveChangesAsync();

                    ban.DataFim = DateTime.Now;
                    _context.Bans.Update(ban);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("MUDOU PARA Leitor");
                    return RedirectToAction("leitorBans");
                }

            }
            else if (op == "ban")
            {


                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }


                if (await _userManager.IsInRoleAsync(user, "LeitorBanido"))
                {
                    Console.WriteLine("ESTA NO ROLE LeitorBanido");
                    return RedirectToAction("leitorBans");
                }

                if (await _userManager.IsInRoleAsync(user, "Leitor"))
                {
                    var leitor = await _context.Leitores
                        .Include(l => l.User)
                        .FirstOrDefaultAsync(b => b.Id == user.Id);
                    if (leitor == null)
                    {
                        Console.WriteLine("leitor NULL");
                        return RedirectToAction("leitorBans");
                    }


                    Ban banData = new Ban
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdUser = user.Id,
                        DataInicio = DateTime.Now,
                        Motivo = "Banido por um administrador",
                        IdAdmin = currAdmin.Id,
                        Admin = currAdmin,
                        User = leitor

                    };


                    if (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "LeitorBanido") == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("LeitorBanido"));
                    }

                    await _userManager.AddToRoleAsync(user, "LeitorBanido");
                    await _userManager.RemoveFromRoleAsync(user, "Leitor");

                    _context.Bans.Add(banData);
                    await _context.SaveChangesAsync();

                    leitor.IsBanido = true;
                    _context.Leitores.Update(leitor);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("MUDOU PARA LeitorBanido");
                    return RedirectToAction("leitorBans");
                }

            }
            else
            {
                Console.WriteLine("ELSE");
                return RedirectToAction("leitorBans");
            }

            Console.WriteLine("FIM");
            return RedirectToAction("leitorBans");


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
