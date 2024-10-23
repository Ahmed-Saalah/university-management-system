using Microsoft.AspNetCore.Mvc;
using university_management_system.Data;
using System.Net;
using university_management_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace university_management_system.Controllers
{
    public class StudentController : Controller
    {
        private UniversityContext _context;

        public StudentController()
        {
            _context = new UniversityContext();
        }
        public ViewResult Index(string? sortOrder, string? currentFilter, string? searchString)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AgeSortParm = sortOrder == "Age" ? "age_desc" : "Age";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "email_desc" : "Email";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                currentFilter = searchString;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = currentFilter;

            var students = _context.Students.AsQueryable();

            // Search by Name
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name != null && s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "Age":
                    students = students.OrderBy(s => s.Age);
                    break;
                case "age_desc":
                    students = students.OrderByDescending(s => s.Age);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.Name);
                    break;
            }

            return View(students);
        }

        // GET: Student/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("Invalid student ID.");
            }

            Student? student = _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefault(m => m.Id == id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }


        // GET: Student/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Student ID is required.");
            }

            Student? student = _context.Students
                .SingleOrDefault(e => e.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            var studentToUpdate =  _context.Students.Find(id);
            if (studentToUpdate == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            if (!ModelState.IsValid)
            {
                return View(student); 
            }

            try
            {
                studentToUpdate.Name = student.Name;
                studentToUpdate.Age = student.Age;
                studentToUpdate.Phone = student.Phone;
                studentToUpdate.EnrollmentDate = student.EnrollmentDate;

                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException dbEx)
            {
                ModelState.AddModelError("", "Unable to save changes to the database. Try again, and if the problem persists, contact your system administrator.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error saving changes: {ex.Message}");
            }

            return View(student);
        }



        // GET: Student/Delete/5
        public IActionResult Delete(int? id)  
        {
            if (id == null)
            {
                return BadRequest();
            }
            Student? student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                Student? student = _context.Students.Find(id);
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            catch (RetryLimitExceededException/* dex */)
            {
                return RedirectToAction("Delete", new { id = id});
            }
            return RedirectToAction("Index");
        }
    }
}
