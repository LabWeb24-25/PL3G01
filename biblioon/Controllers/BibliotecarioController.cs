using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace biblioon.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class BibliotecarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
