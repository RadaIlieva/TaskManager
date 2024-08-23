using Microsoft.EntityFrameworkCore;
using TaskManagerData.Contexts;
using TaskManagerData.Entities;
using TaskManagerProject.DTOs;
using TaskManagerProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerProject.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext context;

        public ProjectService(AppDbContext context)
        {
            this.context = context;
        }

        public void CreateProject(NewProjectDto projectDto, int createdByUserId)
        {
            var project = new Project
            {
                Name = projectDto.ProjectName,
                Description = projectDto.Description,
                CreatedByUserId = createdByUserId,
                Team = new Team { Name = "Default Team Name" },
                MemberUniqueCodes = string.Empty 
            };

            var creator = context.Employees.Find(createdByUserId);
            if (creator != null)
            {
                project.Team.Members.Add(creator);
                project.MemberUniqueCodes += creator.UniqueCode; 
            }

            var memberCodes = projectDto.MembersUniqueCodes.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var code in memberCodes)
            {
                var employee = context.Employees.FirstOrDefault(e => e.UniqueCode == code.Trim());
                if (employee != null && !project.Team.Members.Contains(employee))
                {
                    project.Team.Members.Add(employee);
                    project.MemberUniqueCodes += $",{employee.UniqueCode}"; 
                }
            }

            context.Projects.Add(project);
            context.SaveChanges();
        }


        public List<ProjectDto> GetProjectsForUser(string uniqueCode)
        {
            var projects = context.Projects
                .Include(p => p.Team)
                .ThenInclude(t => t.Members)
                .Where(p => p.MemberUniqueCodes != null && p.MemberUniqueCodes.Contains(uniqueCode))
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

            return projects;
        }



        public ProjectDetailsDto GetProjectDetails(int projectId)
        {
            var project = context.Projects
                .Include(p => p.Team)
                .ThenInclude(t => t.Members)
                .Include(p => p.ProjectTasks)
                .FirstOrDefault(p => p.Id == projectId);

            if (project == null)
                return null;

            var projectDetails = new ProjectDetailsDto
            {
                Id = project.Id,
                ProjectName = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                TeamMembers = project.Team.Members.Select(m => $"{m.FirstName} {m.LastName}").ToList(),
                ProjectTasks = project.ProjectTasks.Select(t => new ProjectTaskDto
                {
                    Title = t.Title,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    AssignedTo = t.AssignedToEmployee?.UniqueCode 
                }).ToList()
            };

            return projectDetails;
        }
    }
}