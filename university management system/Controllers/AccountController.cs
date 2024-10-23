using Microsoft.AspNetCore.Mvc;
using university_management_system.Models;
using university_management_system.ViewModels;
using university_management_system.Data;
using System.Linq;
using System.Threading.Tasks;

namespace university_management_system.Controllers
{
    public class AccountController : Controller
    {
        private readonly UniversityContext _context;

        public AccountController()
        {
            _context = new UniversityContext();
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            ViewBag.IsLoginPage = true;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = _context.Students
                    .FirstOrDefault(s => s.Email == model.Email && s.Password == model.Password);

                var instructor = student == null ?
                    _context.Instructors.FirstOrDefault(i => i.Email == model.Email && i.Password == model.Password)
                    : null;

                if (student != null)
                {
                    HttpContext.Session.SetString("name", student.Name);
                    HttpContext.Session.SetString("role", "Student");
                    return RedirectToAction("Index", "Home"); 
                }
                else if (instructor != null)
                {
                    HttpContext.Session.SetString("name", instructor.Name);
                    HttpContext.Session.SetString("role", "Instructor");
                    return RedirectToAction("Index", "Home"); 
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            ViewBag.IsLoginPage = true;
            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Account"); 
        }

    }
}
