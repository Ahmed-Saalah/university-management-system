using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using university_management_system.Data;
using university_management_system.Models;
using university_management_system.ViewModels;

namespace university_management_system.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UniversityContext _context;
        private readonly InstructorService _instructorService;

        public InstructorController()
        {
            _context = new UniversityContext();
            _instructorService = new InstructorService(_context);
        }

        // Get Instructor
        public IActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();

            viewModel.Instructors = _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                    .ThenInclude(c => c.Department)
                .Include(i => i.Courses)
                    .ThenInclude(c => c.Enrollments)  // Load Enrollments for each course
                    .ThenInclude(e => e.Student)      // Load Students for each enrollment
                .OrderBy(i => i.Name)
                .ToList();

            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors
                    .Where(i => i.Id == id.Value)
                    .Single().Courses;
            }

            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                viewModel.Enrollments = viewModel.Courses
                    .Where(c => c.Id == courseID.Value)
                    .Single().Enrollments;
            }

            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("Invalid Instructor ID.");
            }
            Instructor? instructor = _context.Instructors.FirstOrDefault(i => i.Id == id);
            if (instructor == null)
            {
                return NotFound($"instructor with ID {id} not found.");
            }
            return View(instructor);
        }

        public IActionResult Create()
        {
            var instructor = new Instructor();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Email,Password,HireDate,Age,Phone,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {

            if (selectedCourses != null)
            {
                instructor.Courses = new List<Course>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = _context.Courses.Find(int.Parse(course));
                    instructor.Courses.Add(courseToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
  
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }


        // GET: Instructor/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Instructor ID is required.");

            }

            Instructor? instructor = _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.Id == id)
                .SingleOrDefault(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound($"instructor with ID {id} not found.");
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }


        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, [Bind("Name,Age,Phone,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            if (id == null)
            {
                return BadRequest("instructor ID mismatch.");
            }

            Instructor? instructorToUpdate = _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .SingleOrDefault(i => i.Id == id);

            if (instructorToUpdate == null)
            {
                return NotFound($"Instructor with ID {id} not found.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                PopulateAssignedCourseData(instructorToUpdate);
                return View(instructorToUpdate);
            }

            try
            {
                instructorToUpdate.Name = instructor.Name;
                instructorToUpdate.Age = instructor.Age;
                instructorToUpdate.Phone = instructor.Phone;

                if (instructor.OfficeAssignment != null)
                {
                    if (instructorToUpdate.OfficeAssignment == null)
                    {
                        instructorToUpdate.OfficeAssignment = new OfficeAssignment();
                    }
                    instructorToUpdate.OfficeAssignment.Location = instructor.OfficeAssignment.Location;
                }
                else
                {
                    instructorToUpdate.OfficeAssignment = null;
                }

                // Update Courses
                _instructorService.UpdateInstructorCourses(selectedCourses, instructorToUpdate);
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

            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        // GET: Instructor/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Instructor? instructor = _context.Instructors.Find(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Instructor? instructor = _context.Instructors
              .Include(i => i.OfficeAssignment)
              .SingleOrDefault(i => i.Id == id);

            if ( instructor == null )
            {
                return NotFound();
            }

            instructor.OfficeAssignment = null;
            _context.Instructors.Remove(instructor);

            var department = _context.Departments
                .SingleOrDefault(d => d.InstructorID == id);

            if (department != null)
            {
                department.InstructorID = null;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            ViewBag.Courses = _instructorService.GetAssignedCourseData(instructor);
        }
    }
}
