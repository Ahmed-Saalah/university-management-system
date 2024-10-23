using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using university_management_system.Data;
using university_management_system.Models;

namespace university_management_system.Controllers
{
    public class CourseController : Controller
    {
        private readonly UniversityContext _context;

        public CourseController()
        {
            _context = new UniversityContext();
        }
        public IActionResult Index(int? SelectedDepartment)
        {
            var departments = _context.Departments
                .OrderBy(q => q.Name)
                .ToList();
            ViewBag.SelectedDepartment = new SelectList(departments, "Id", "Name", SelectedDepartment);
            int departmentID = SelectedDepartment.GetValueOrDefault();

            var courses = _context.Courses
                .Where(c => !SelectedDepartment.HasValue || c.DepartmentID == departmentID)
                .OrderBy(d => d.Id)
                .Include(d => d.Department)
                .ToList();
            return View(courses);
        }

        // GET: Course/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course? course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Credits,DepartmentID")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Courses.Add(course);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your administrator.");
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Course/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Course? course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Credits,DepartmentID")] Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var CourseToUpdate = _context.Courses.Find(id);
                    if (CourseToUpdate == null)
                    {
                        return NotFound();
                    }
                    CourseToUpdate.Title = course.Title;
                    CourseToUpdate.Credits = course.Credits;
                    CourseToUpdate.DepartmentID = course.DepartmentID;

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your administrator.");
                }

            }

            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }


        // GET: Course/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Course? course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Course? course = _context.Courses.Find(id);
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var dept = _context.Departments.OrderBy(d => d.Name);
            ViewBag.DepartmentID = new SelectList(dept, "Id", "Name", selectedDepartment);
        }
    }
}
