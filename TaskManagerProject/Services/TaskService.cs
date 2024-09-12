using TaskManagerProject.DTOs;
using TaskManagerProject.Services.Interfaces;
using TaskManagerData.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerProject.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext context;

        public TaskService(AppDbContext context)
        {
            this.context = context;
        }

        public ServiceResult CreateTask(ProjectTaskDto model, int currentUserId)
        {
            var project = GetProjectById(model.ProjectId);

            if (project == null || project.CreatedByUserId != currentUserId)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Unauthorized" };
            }

            var task = new TaskManagerData.Entities.ProjectTask
            {
                Title = model.Title,
                Description = model.Description,
                AssignedToEmployeeId = model.AssignedToEmployeeId,
                ProjectId = model.ProjectId,
                StartDate = DateTime.Now
            };

            context.Add(task);
            context.SaveChanges();

            return new ServiceResult { Success = true };
        }

        public ServiceResult UpdateTaskStatus(int taskId, string status)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Task not found." };
            }

            context.SaveChanges();
            return new ServiceResult { Success = true };
        }

        public ServiceResult DeleteTask(int taskId)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Task not found." };
            }

            var projectId = task.ProjectId;

            context.ProjectTasks.Remove(task);
            context.SaveChanges();

            return new ServiceResult { Success = true, ProjectId = projectId };
        }
        public ServiceResult UpdateTaskDescription(int taskId, string newDescription)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Task not found." };
            }

            task.Description = newDescription;
            context.SaveChanges();

            return new ServiceResult { Success = true };
        }


        public ServiceResult AddTaskToProject(ProjectTaskDto taskDto)
        {
            try
            {
                var newTask = new TaskManagerData.Entities.ProjectTask
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    StartDate = taskDto.StartDate,
                    EndDate = taskDto.EndDate,
                    AssignedToEmployeeId = taskDto.AssignedToEmployeeId,
                    ProjectId = taskDto.ProjectId
                };

                context.ProjectTasks.Add(newTask);
                context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public ProjectTaskDto GetTaskById(int taskId)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
                return null;

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                AssignedToEmployeeId = task.AssignedToEmployeeId ?? 0,
                ProjectId = task.ProjectId
            };
        }



        public Project GetProjectById(int projectId)
        {
            return context.Projects.Find(projectId);
        }

        //[HttpPost]
        //public IActionResult DeleteTaskAction(int taskId)
        //{
        //    var result = DeleteTask(taskId);
        //    if (result.Success)
        //    {
        //        return RedirectToAction("Details", new { id = result.ProjectId });
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", result.ErrorMessage);
        //        var projectDetails = GetProjectDetails(result.ProjectId);
        //        return View("Details", projectDetails);
        //    }
        //}

    }
}
