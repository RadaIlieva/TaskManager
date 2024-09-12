using TaskManagerProject.DTOs;

namespace TaskManagerProject.Services.Interfaces
{
    public interface ITaskService
    {
        ServiceResult CreateTask(ProjectTaskDto model, int currentUserId);
        ServiceResult UpdateTaskStatus(int taskId, string status);
        ServiceResult DeleteTask(int taskId);
        ServiceResult UpdateTaskDescription(int taskId, string newDescription);

        ServiceResult AddTaskToProject(ProjectTaskDto taskDto);
        ProjectTaskDto GetTaskById(int taskId);
        Project GetProjectById(int projectId);
    }
}
