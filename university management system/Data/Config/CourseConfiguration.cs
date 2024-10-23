using Microsoft.EntityFrameworkCore;
using university_management_system.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace university_management_system.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.Credits).IsRequired();
            builder.HasCheckConstraint("CK_Course_Credits_Range", "[Credits] >= 0 AND [Credits] <= 5");

            builder.HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentID);

        }

    }

}
