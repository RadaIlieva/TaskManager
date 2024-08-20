using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            public UserProfileController(IUserProfileService userService)
            {
                this.userService = userService;
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

                return View(model);
            }

            [HttpPost]
            public IActionResult Profile(UserProfileDto model, IFormFile ProfilePicture)
            {
                if (ModelState.IsValid)
                {
                    if (ProfilePicture != null && ProfilePicture.Length > 0)
                    {
                        userService.UpdateEmployeeProfile(model, ProfilePicture);
                    }
                    else
                    {
                        userService.UpdateEmployeeProfileWithoutPicture(model);
                    }

                    return RedirectToAction("Index", "UserProfile");
                }

                return View(model);
            }

            public IActionResult Index()
            {
                try
                {
                    var userEmail = User.Identity.Name;
                    var user = userService.GetEmployeeByEmail(userEmail);

                    if (user == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    ViewBag.UserName = user.FirstName + " " + user.LastName;
                    ViewBag.UserUniqueCode = user.UniqueCode;

                    return View();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
        }
    }
}