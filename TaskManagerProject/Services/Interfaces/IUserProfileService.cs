using TaskManagerData.Entities;
using TaskManagerProject.Models;

namespace TaskManagerProject.Services
{
    public interface IUserProfileService
    {
        Employee GetEmployeeByEmail(string email);
        UserProfileDto GetUserProfileByEmail(string email);
        void UpdateEmployeeProfile(UserProfileDto model, IFormFile profilePicture);
        void UpdateEmployeeProfileWithoutPicture(UserProfileDto model);
    }
}
    