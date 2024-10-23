using System.ComponentModel.DataAnnotations;

namespace university_management_system.Models
{
    public class Instructor : User
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public virtual ICollection<Course>? Courses { get; set; } = new List<Course>();
        public virtual OfficeAssignment? OfficeAssignment { get; set; } = new OfficeAssignment();
    }
}
