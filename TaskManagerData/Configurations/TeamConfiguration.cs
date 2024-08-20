using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerData.Entities;

namespace TaskManagerData.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(t => t.Members).WithMany(e => e.Teams)
                .UsingEntity<Dictionary<string, object>>(
                    "TeamEmployee",
                    j => j
                        .HasOne<Employee>()
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Team>()
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                );

            builder.HasMany(t => t.Projects)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
