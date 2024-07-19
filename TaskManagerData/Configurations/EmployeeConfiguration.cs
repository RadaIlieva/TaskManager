using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerData.Entities;

namespace TaskManagerData.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(10);
            builder.Property(e => e.PasswordHash).IsRequired();
            builder.Property(e => e.UniqueCode).IsRequired().HasMaxLength(4);
            builder.Property(e => e.Role).IsRequired();
            builder.Property(e => e.ProfilePictureUrl).HasMaxLength(250); 

            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.PhoneNumber).IsUnique();
            builder.HasIndex(e => e.UniqueCode).IsUnique();
        }
    }
}
