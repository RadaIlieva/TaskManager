using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerData.Contexts;
using TaskManagerData.Entities;
using TaskManagerProject.DTOs;



namespace TaskManagerProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext context;

        public DashboardController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var currentUserEmail = User.Identity.Name;
            var currentUser = context.Employees.FirstOrDefault(e => e.Email == currentUserEmail);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var userProjects = context.Projects
                                       .Include(p => p.Team)
                                       .ThenInclude(t => t.Members)
                                       .Where(p => p.MemberUniqueCodes != null && p.MemberUniqueCodes.Contains(currentUser.UniqueCode))
                                       .Select(p => new ProjectDto
                                       {
                                           Id = p.Id,
                                           Name = p.Name,
                                           Description = p.Description,
                                           Members = p.Team.Members.Select(m => new MemberDto
                                           {
                                               Id = m.Id,
                                               Name = $"{m.FirstName} {m.LastName}"
                                           }).ToList()
                                       })
                                       .ToList();

            ViewBag.UserName = currentUser.FirstName + " " + currentUser.LastName;
            ViewBag.UserUniqueCode = currentUser.UniqueCode;
            ViewBag.ProfilePictureUrl = currentUser.ProfilePictureUrl;

            return View(userProjects);
        }
    }
}