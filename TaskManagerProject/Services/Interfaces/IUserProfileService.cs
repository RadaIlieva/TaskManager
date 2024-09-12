using TaskManagerData.Entities;
using TaskManagerProject.Models;

namespace TaskManagerProject.Services
{
    public interface IUserProfileService
    {
        Employee GetEmployeeByEmail(string email);
        UserProfile GetUserProfileByEmail(string email);
        int GetUserIdByEmail(string email);
        Employee GetEmployeeByUniqueCode(string uniqueCode);
        UserProfile GetUserProfileByUniqueCode(string uniqueCode);
        void UpdateEmployeeProfile(UserProfile model, IFormFile profilePicture);
        void UpdateEmployeeProfileWithoutPicture(UserProfile model);

    }
}
    