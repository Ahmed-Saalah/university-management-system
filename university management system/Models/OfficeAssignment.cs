using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace university_management_system.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public virtual Instructor? Instructor { get; set; }
    }
}
