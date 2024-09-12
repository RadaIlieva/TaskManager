using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.DTOs;
using TaskManagerProject.Services;
using TaskManagerProject.Services.Interfaces;

namespace TaskManagerProject.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public IActionResult CreateTask(ProjectTaskDto model)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            var project = taskService.GetProjectById(model.ProjectId);
            if (project == null || project.CreatedByUserId != currentUserId)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = taskService.CreateTask(model, currentUserId);

            if (result.Success)
            {
                return RedirectToAction("Details", new { id = model.ProjectId });
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] ProjectTaskDto newTaskDto)
        {
            if (newTaskDto == null)
            {
                return Json(new { success = false, message = "Invalid task data." });
            }

            var result = taskService.AddTaskToProject(newTaskDto);
            if (result.Success)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = result.ErrorMessage });
            }
        }

        [HttpPost]
        public IActionResult UpdateTaskStatus([FromBody] TaskStatusUpdateDto updateDto)
        {
            var result = taskService.UpdateTaskStatus(updateDto.TaskId, updateDto.Status);
            return Json(result);
        }


        [HttpPost]
        public IActionResult DeleteTask(int taskId)
        {
            var result = taskService.DeleteTask(taskId);
            return Json(result);
        }

        [HttpPost]
        public IActionResult UpdateTaskDescription(int taskId, string newDescription)
        {
            var result = taskService.UpdateTaskDescription(taskId, newDescription);
            return Json(result);
        }
    }
}
