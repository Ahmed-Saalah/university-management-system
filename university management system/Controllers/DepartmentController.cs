using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Net;
using university_management_system.Data;
using university_management_system.Models;

namespace university_management_system.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly UniversityContext _context;

        public DepartmentController()
        {
            _context = new UniversityContext();
        }

        public IActionResult Index()
        {
            var departments = _context.Departments
                .Include(d => d.Administrator)
                .ToList();
            return View(departments);
        }

        // GET: Department/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest($"Department with {id} Not Exist");
            }

            var department = _context.Departments
                .Include(d => d.Administrator)
                .SingleOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound($"Department with {id} Not Found");
            }
            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(_context.Instructors, "Id", "Name");
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InstructorID = new SelectList(_context.Instructors, "Id", "Name", department.InstructorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Department? department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewBag.InstructorID = new SelectList(_context.Instructors, "Id", "Name", department.InstructorID);
            return View(department);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id , Department department)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.InstructorID = new SelectList(_context.Instructors, "Id", "Name", department.InstructorID);
                return View(department);
            }

            var departmentToUpdate = _context.Departments.Find(id);

            if (departmentToUpdate == null)
            {
                return NotFound();
            }

            departmentToUpdate.Name = department.Name;
            departmentToUpdate.StartDate = department.StartDate;
            departmentToUpdate.InstructorID = department.InstructorID;

            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", $"Unable to save changes. An error occurred: {ex.Message}");
                ViewBag.InstructorID = new SelectList(_context.Instructors, "Id", "Name", department.InstructorID);
                return View(department);
            }
        }

        // GET: Course/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Department? departemnt = _context.Departments.Find(id);
            if (departemnt == null)
            {
                return NotFound();
            }
            return View(departemnt);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Department? departemnt = _context.Departments.Find(id);
            _context.Departments.Remove(departemnt);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
