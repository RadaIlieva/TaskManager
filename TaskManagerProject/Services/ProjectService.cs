using TaskManagerProject.Models;
using TaskManagerData.Contexts;
using TaskManagerData.Entities;

namespace TaskManagerProject.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext context;

        public ProjectService(AppDbContext context)
        {
            this.context = context;
        }

        public void CreateProject(NewProjectDto projectDto, int createdByUserId)
        {
            var project = new Project
            {
                Name = projectDto.ProjectName,
                CreatedByUserId = createdByUserId,
            };

            foreach (var code in projectDto.MembersUniqueCodes)
            {
                var employee = context.Employees.FirstOrDefault(e => e.UniqueCode == code);
                if (employee != null)
                {
                    project.Team.Members.Add(employee);
                }
            }

            context.Projects.Add(project);
            context.SaveChanges();
        }
    }
}
