using System.Collections.Generic;
using System.Linq;
using university_management_system.Data;
using university_management_system.Models;
using university_management_system.ViewModels;

public class InstructorService
{
    private readonly UniversityContext _context;

    public InstructorService(UniversityContext context)
    {
        _context = context;
    }
    public List<AssignedCourseData> GetAssignedCourseData(Instructor instructor)
    {
        var allCourses = _context.Courses.ToList();
        var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.Id));
        var viewModel = new List<AssignedCourseData>();

        foreach (var course in allCourses)
        {
            viewModel.Add(new AssignedCourseData
            {
                CourseID = course.Id,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.Id)
            });
        }

        return viewModel;
    }
    public void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
    {
        if (selectedCourses == null)
        {
            instructorToUpdate.Courses = new List<Course>();
            return;
        }

        var selectedCoursesHS = new HashSet<string>(selectedCourses);
        var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.Id));

        foreach (var course in _context.Courses)
        {
            if (selectedCoursesHS.Contains(course.Id.ToString()))
            {
                if (!instructorCourses.Contains(course.Id))
                {
                    instructorToUpdate.Courses.Add(course);
                }
            }
            else
            {
                if (instructorCourses.Contains(course.Id))
                {
                    instructorToUpdate.Courses.Remove(course);
                }
            }
        }
    }
}
