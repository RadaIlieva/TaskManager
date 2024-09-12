﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerData.Contexts;
using TaskManagerProject.DTOs;
using TaskManagerProject.Models;
using TaskManagerProject.Services;
using TaskManagerProject.Services.Interfaces;

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

            ViewBag.UserName = $"{model.FirstName} {model.LastName}";
            ViewBag.UserUniqueCode = model.UniqueCode;
            ViewBag.ProfilePictureUrl = model.ProfilePictureUrl;

            return View(model);
        }


        [HttpPost]
        public IActionResult Profile(UserProfileDto model, IFormFile profilePicture)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.Identity.Name;
                var userId = userService.GetUserIdByEmail(userEmail);

                if (userId > 0)
                {
                    // Логика за качване на снимката
                    if (profilePicture != null && profilePicture.Length > 0)
                    {
                        // Задаваш пътя за качване на файла
                        var fileName = Path.GetFileName(profilePicture.FileName);
                        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile_pictures");

                        if (!Directory.Exists(uploadsPath))
                        {
                            Directory.CreateDirectory(uploadsPath);
                        }

                        // Създаване на уникално име на файла (можеш да използваш GUID или уникален идентификатор)
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        var filePath = Path.Combine(uploadsPath, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            profilePicture.CopyTo(stream);
                        }

                        // Запазване на URL-то на снимката в модела
                        model.ProfilePictureUrl = $"/uploads/profile_pictures/{uniqueFileName}";
                    }
                    model.Id = userId;
                    // Обновяване на профила в базата
                    userService.UpdateEmployeeProfile(model, profilePicture);
                    return RedirectToAction("Profile");
                }
            }

            return View(model);
        }



        public IActionResult Index()
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

            ViewBag.UserName = user.FirstName + " " + user.LastName;
            ViewBag.UserUniqueCode = user.UniqueCode;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;

            var userProjects = projectService.GetProjectsForUser(user.UniqueCode);

            return View(userProjects);
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