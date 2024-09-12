using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.DTOs;
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
            var currentUser = userService.GetUserProfileByEmail(User.Identity.Name);
            ViewBag.UserInfo = currentUser;
            return View(new NewProjectDto());
        }

        [HttpPost]
        public IActionResult NewProject(NewProjectDto model)
        {
            if (ModelState.IsValid)
            {
                var createdByUser = userService.GetEmployeeByEmail(User.Identity.Name);
                if (createdByUser == null) return Unauthorized();

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
        public IActionResult AddTeamMember([FromBody] AddMemberRequestDto request)
        {
            if (request == null || request.ProjectId <= 0 || string.IsNullOrEmpty(request.UniqueCode))
            {
                return Json(new { success = false, message = "Invalid request." });
            }

            var result = projectService.AddMemberToProject(request.ProjectId, request.UniqueCode);
            if (result.Success)
            {
                var newMember = userService.GetUserProfileByUniqueCode(request.UniqueCode);
                return Json(new
                {
                    success = true,
                    name = $"{newMember.FirstName} {newMember.LastName}",
                    profilePictureUrl = newMember.ProfilePictureUrl
                });
            }

            return Json(new { success = false, message = result.ErrorMessage });
        }


        [HttpPost]
        public IActionResult RemoveTeamMember(int projectId, int memberId)
        {
            var result = projectService.RemoveMemberFromProject(projectId, memberId);
            return Json(result);
        }


    }
}
