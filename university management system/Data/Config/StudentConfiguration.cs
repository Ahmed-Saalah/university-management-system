using Microsoft.EntityFrameworkCore;
using university_management_system.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace university_management_system.Data.Config
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasColumnType("VARCHAR")
                .HasMaxLength(100)
                .IsRequired(); 

            builder.Property(x => x.Password)
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasMaxLength(15);

            builder.HasMany(x => x.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.Id)
            .IsRequired(false);
        }
    }
    
}
