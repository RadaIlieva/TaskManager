using Microsoft.EntityFrameworkCore;
using TaskManagerData.Configurations;
using TaskManagerData.Entities;

namespace TaskManagerData.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
        }
    }
}
