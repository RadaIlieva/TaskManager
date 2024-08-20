using TaskManagerProject.Models;
using TaskManagerData.Contexts;
using TaskManagerProject.Services.Interfaces;
using TaskManagerData.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TaskManagerProject.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext context;

        public UserProfileService(AppDbContext context)
        {
            this.context = context;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            return context.Employees.FirstOrDefault(u => u.Email == email);
        }

        public UserProfileDto GetUserProfileByEmail(string email)
        {
            var user = GetEmployeeByEmail(email);
            if (user == null) return null;

            return new UserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UniqueCode = user.UniqueCode,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }

        public void UpdateEmployeeProfile(UserProfileDto model, IFormFile profilePicture)
        {
            var user = context.Employees.Find(model.Id);
            if (user != null)
            {
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    var fileName = Path.GetFileName(profilePicture.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profilePicture.CopyTo(stream);
                    }

                    user.ProfilePictureUrl = $"/images/profiles/{fileName}";
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateOfBirth = model.DateOfBirth;
                user.PhoneNumber = model.PhoneNumber;
                user.UniqueCode = model.UniqueCode;

                context.SaveChanges();
            }
        }

        public void UpdateEmployeeProfileWithoutPicture(UserProfileDto model)
        {
            var user = context.Employees.Find(model.Id);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateOfBirth = model.DateOfBirth;
                user.PhoneNumber = model.PhoneNumber;
                user.UniqueCode = model.UniqueCode;

                context.SaveChanges();
            }
        }

    }
}
