using TaskManagerProject.DTOs;
using TaskManagerProject.Models;

namespace TaskManagerProject.Services
{
    public interface IProjectService
    {
        void CreateProject(NewProjectDto projectDto, int createdByUserId);

        public List<ProjectDto> GetProjectsForUser(string uniqueCode);
        ProjectDetailsDto GetProjectDetails(int projectId);

    }
}
