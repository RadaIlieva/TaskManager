using TaskManagerProject.DTOs;
using TaskManagerProject.Models;

namespace TaskManagerProject.Services
{
    public interface IProjectService
    {
        void CreateProject(NewProjectDto model, int createdByUserId);
        ProjectDetailsDto GetProjectDetails(int projectId);
        ServiceResult AddTaskToProject(ProjectTaskDto taskDto); 
        ProjectTaskDto GetTaskById(int taskId); 
        ServiceResult UpdateTaskStatus(int taskId, string status);
        ServiceResult CreateTask(ProjectTaskDto model, int currentUserId);

        ServiceResult UpdateTaskDescription(int taskId, string newDescription);
        ServiceResult DeleteTask(int taskId);

        List<ProjectDto> GetProjectsForUser(string uniqueCode);
        Project GetProjectById(int projectId);



    }
}
