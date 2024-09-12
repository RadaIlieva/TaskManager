using TaskManagerProject.DTOs;
using TaskManagerProject.Models;

namespace TaskManagerProject.Services.Interfaces
{
    public interface IProjectService
    {
        void CreateProject(NewProjectDto projectDto, int createdByUserId);
        List<ProjectDto> GetProjectsForUser(string uniqueCode);
        ProjectDetailsDto GetProjectDetails(int projectId);
        ServiceResult AddMemberToProject(int projectId, string uniqueCode);
        ServiceResult RemoveMemberFromProject(int projectId, int memberId);

    }
}
