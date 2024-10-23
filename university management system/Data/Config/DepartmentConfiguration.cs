using Microsoft.EntityFrameworkCore;
using university_management_system.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace university_management_system.Data.Config
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);
        }
    }
}
