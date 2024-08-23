using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Models;
using TaskManagerData.Entities;
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

            return View(projectDetails);
        }

    }
}
