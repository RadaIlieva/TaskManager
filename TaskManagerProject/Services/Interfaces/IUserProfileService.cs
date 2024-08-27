using TaskManagerData.Entities;
using TaskManagerProject.Models;

namespace TaskManagerProject.Services
{
    public interface IUserProfileService
    {
        Employee GetEmployeeByEmail(string email);
        UserProfileDto GetUserProfileByEmail(string email);
        int GetUserIdByEmail(string email);
        Employee GetEmployeeByUniqueCode(string uniqueCode);
        UserProfileDto GetUserProfileByUniqueCode(string uniqueCode);
        void UpdateEmployeeProfile(UserProfileDto model, IFormFile profilePicture);
        void UpdateEmployeeProfileWithoutPicture(UserProfileDto model);

    }
}
    