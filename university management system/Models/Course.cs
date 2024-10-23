using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace university_management_system.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int Id { get; set; }
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }
        public int? DepartmentID { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ICollection<Enrollment>? Enrollments { get; set; }
        public virtual ICollection<Instructor>? Instructors { get; set; }
    }
}
