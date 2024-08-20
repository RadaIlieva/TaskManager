using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerData.Entities;

namespace TaskManagerData.Configurations
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pt => pt.Description)
                .HasMaxLength(500);

            builder.Property(pt => pt.StartDate)
                .IsRequired();

            builder.Property(pt => pt.EndDate)
                .IsRequired();

            builder.HasOne(pt => pt.AssignedToEmployee)
                .WithMany(e => e.ProjectTasks)
                .HasForeignKey(pt => pt.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pt => pt.Project)
                .WithMany(p => p.ProjectTasks)
                .HasForeignKey(pt => pt.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
