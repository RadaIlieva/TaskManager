using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerData.Entities;

namespace TaskManagerData.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).HasMaxLength(500);
            builder.Property(t => t.StartDate).IsRequired();
            builder.Property(t => t.EndDate).IsRequired();
            builder.Property(t => t.AssignedToEmployeeId).IsRequired();

            builder.HasOne(t => t.AssignedToEmployee)
                .WithMany(e => e.ProjectTasks)
                .HasForeignKey(t => t.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
