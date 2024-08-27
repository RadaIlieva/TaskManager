using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerData.Contexts;
using TaskManagerProject.DTOs;
using TaskManagerProject.Models;
using TaskManagerProject.Services;
using TaskManagerProject.Services.Interfaces;


namespace TaskManagerProject.Controllers
{


    namespace TaskManagerProject.Controllers
    {
        [Authorize]
        public class UserProfileController : Controller
        {
            private readonly IUserProfileService userService;
            private readonly IProjectService projectService;
            private readonly AppDbContext context;

            public UserProfileController(IUserProfileService userService, IProjectService projectService, AppDbContext context)
            {
                this.userService = userService;
                this.projectService = projectService;
                this.context = context;
            }


            [HttpGet]
            public IActionResult Profile()
            {
                var userEmail = User.Identity.Name;
                var model = userService.GetUserProfileByEmail(userEmail);

                if (model == null)
                {
                    return RedirectToAction("Index", "UserProfile");
                }
                ViewBag.ProfilePictureUrl = model.ProfilePictureUrl;

                return View(model);
            }

            [HttpPost]
            public IActionResult Profile(UserProfileDto model, IFormFile profilePicture)
            {
                if (ModelState.IsValid)
                {
                    var currentUser = userService.GetEmployeeByEmail(User.Identity.Name);

                    if (currentUser != null)
                    {
                        currentUser.FirstName = string.IsNullOrEmpty(model.FirstName) ? currentUser.FirstName : model.FirstName;
                        currentUser.LastName = string.IsNullOrEmpty(model.LastName) ? currentUser.LastName : model.LastName;
                        currentUser.DateOfBirth = model.DateOfBirth ?? currentUser.DateOfBirth;
                        currentUser.Email = string.IsNullOrEmpty(model.Email) ? currentUser.Email : model.Email;
                        currentUser.PhoneNumber = string.IsNullOrEmpty(model.PhoneNumber) ? currentUser.PhoneNumber : model.PhoneNumber;

                        if (profilePicture != null && profilePicture.Length > 0)
                        {
                            var fileName = Path.GetFileName(profilePicture.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                profilePicture.CopyTo(stream);
                            }

                            currentUser.ProfilePictureUrl = $"/images/profiles/{fileName}";
                        }

                        context.SaveChanges();
                    }

                    return RedirectToAction("Profile");
                }

                return View(model);
            }


            public IActionResult Index()
            {
                try
                {
                    var userEmail = User.Identity?.Name;
                    if (string.IsNullOrEmpty(userEmail))
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var user = userService.GetEmployeeByEmail(userEmail);
                    if (user == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    if (string.IsNullOrEmpty(user.UniqueCode))
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    if (projectService == null)
                    {
                        return RedirectToAction("Error", "Home");
                    }

                    ViewBag.UserName = user.FirstName + " " + user.LastName;
                    ViewBag.UserUniqueCode = user.UniqueCode;

                    var userProjects = projectService.GetProjectsForUser(user.UniqueCode);

                    return View(userProjects);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return RedirectToAction("Error", "Home");
                }
            }



            public IActionResult Dashboard()
            {
                var userEmail = User.Identity.Name;
                var user = userService.GetUserProfileByEmail(userEmail);

                if (user == null)
                {
                    return Unauthorized();
                }

                var userUniqueCode = user.UniqueCode; 
                var userProjects = projectService.GetProjectsForUser(userUniqueCode); 

                ViewBag.UserName = user.FirstName + " " + user.LastName;

                return View("Index", userProjects); 
            }


        }
    }
}