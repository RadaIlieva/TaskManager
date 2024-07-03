using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerData.Entities;

namespace TaskManagerData.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Text).IsRequired().HasMaxLength(1000);
            builder.Property(c => c.CreatedAt).IsRequired();

            builder.HasOne(c => c.ProjectTask)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId);

            builder.HasOne(c => c.Employee)
                .WithMany(e => e.Comments)
                .HasForeignKey(c => c.EmployeeId);
        }
    }
}
