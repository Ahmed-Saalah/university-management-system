using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using university_management_system.Models;

namespace university_management_system.Controllers
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
            string? userName = HttpContext.Session.GetString("name");
            if (userName == null) 
            { 
                return RedirectToAction("Login" , "Account");
            }
            ViewBag.UserName = userName;
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
