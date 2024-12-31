using System.Diagnostics;
using biblioon.Models;
using Microsoft.AspNetCore.Mvc;

namespace biblioon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }
        public IActionResult Copyright()
        {
            return View();
        }

        public IActionResult Cookies()
        {
            return View();
        }

        public IActionResult Books()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
