using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerProject.DTOs;
using TaskManagerProject.Enums;
using TaskManagerProject.Models;
using TaskManagerProject.Services;
using TaskManagerProject.Services.Interfaces;

namespace TaskManagerProject.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService projectService;
        private readonly IUserProfileService userService;

        public ProjectController(IProjectService projectService, IUserProfileService userService)
        {
            this.projectService = projectService;
            this.userService = userService;
        }

        public IActionResult NewProject()
        {
            var userEmail = User.Identity.Name;
            var currentUser = userService.GetUserProfileByEmail(userEmail);

            if (currentUser != null)
            {
                ViewBag.UserName = $"{currentUser.FirstName} {currentUser.LastName}";
                ViewBag.UserUniqueCode = currentUser.UniqueCode;
                ViewBag.ProfilePictureUrl = currentUser.ProfilePictureUrl; 
            }

            return View(new NewProjectDto());
        }

        [HttpPost]
        public IActionResult NewProject(NewProjectDto model)
        {
            if (ModelState.IsValid)
            {
                var createdByUser = userService.GetEmployeeByEmail(User.Identity.Name);
                if (createdByUser == null)
                {
                    return Unauthorized();
                }

                projectService.CreateProject(model, createdByUser.Id);
                return RedirectToAction("Index", "UserProfile");
            }

            return View(model);
        }

        public IActionResult Details(int id)
        {
            var projectDetails = projectService.GetProjectDetails(id);
            if (projectDetails == null)
            {
                return NotFound();
            }

            var userEmail = User.Identity.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(); 
            }

            var currentUser = userService.GetUserProfileByEmail(userEmail);
            if (currentUser == null)
            {
                return NotFound();
            }

            var isCreator = projectDetails.CreatedByUserId == currentUser.Id; 

            ViewBag.UserName = $"{currentUser.FirstName} {currentUser.LastName}";
            ViewBag.UserUniqueCode = currentUser.UniqueCode;
            ViewBag.ProfilePictureUrl = currentUser.ProfilePictureUrl;
            ViewBag.IsCreator = isCreator; 

            return View(projectDetails);
        }



        [HttpPost]
        public IActionResult AddTask([FromBody] ProjectTaskDto newTaskDto)
        {
            if (newTaskDto == null)
            {
                return Json(new { success = false, message = "Invalid task data." });
            }

            var result = projectService.AddTaskToProject(newTaskDto);
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
            if (updateDto == null)
            {
                return Json(new { success = false, message = "Invalid task data." });
            }

            var task = projectService.GetTaskById(updateDto.TaskId);
            if (task == null || (!User.IsInRole("ProjectAdmin") && task.AssignedToEmployeeId != int.Parse(User.Identity.Name)))
            {
                return Json(new { success = false, message = "Unauthorized or task not found." });
            }

            var result = projectService.UpdateTaskStatus(updateDto.TaskId, updateDto.Status);
            return Json(new { success = result.Success, message = result.ErrorMessage });
        }


        [HttpPost]
        [Authorize]
        public IActionResult CreateTask(ProjectTaskDto model)
        {
            var currentUserId = int.Parse(User.Identity.Name);

            var project = projectService.GetProjectById(model.ProjectId);
            if (project == null || project.CreatedByUserId != currentUserId)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = projectService.CreateTask(model, currentUserId);

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
        public IActionResult DeleteTask(int taskId)
        {
            var result = projectService.DeleteTask(taskId);
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
        public IActionResult UpdateTaskDescription(int taskId, string newDescription)
        {
            var result = projectService.UpdateTaskDescription(taskId, newDescription);
            if (result.Success)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = result.ErrorMessage });
            }
        }

    }
}
