using Microsoft.EntityFrameworkCore;
using TaskManagerData.Contexts;
using TaskManagerData.Entities;
using TaskManagerProject.DTOs;
using TaskManagerProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.Models;
using System.Text;

namespace TaskManagerProject.Services
{
    public class ProjectService : Controller, IProjectService
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
            return context.Projects
                .AsNoTracking()
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
                TeamMembers = project.Team.Members.Select(m => new MemberDto
                {
                    Id = m.Id,
                    Name = $"{m.FirstName} {m.LastName}",
                    ProfilePictureUrl = m.ProfilePictureUrl 
                }).ToList(),
                ProjectTasks = project.ProjectTasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    AssignedToEmployeeId = t.AssignedToEmployeeId ?? 0,
                    AssignedToEmployeeName = project.Team.Members.FirstOrDefault(m => m.Id == t.AssignedToEmployeeId)?.FirstName,
                    ProjectId = t.ProjectId
                }).ToList(),
                CreatedByUserId = project.CreatedByUserId
            };

            return projectDetails;
        }


        public ServiceResult AddMemberToProject(int projectId, string uniqueCode)
        {
            var project = context.Projects
                .Include(p => p.Team)
                .FirstOrDefault(p => p.Id == projectId);

            if (projectId <= 0)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Invalid project ID." };
            }

            if (project == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Project not found." };
            }

            var employee = context.Employees.FirstOrDefault(e => e.UniqueCode == uniqueCode);
            if (employee == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Employee not found." };
            }

            if (project.Team.Members.Contains(employee))
            {
                return new ServiceResult { Success = false, ErrorMessage = "Employee is already a member of the project." };
            }

            if (!project.MemberUniqueCodes.Contains(employee.UniqueCode))
            {
                var existingCodes = project.MemberUniqueCodes.Split(',');
                var builder = new StringBuilder();
                foreach (var code in existingCodes)
                {
                    builder.Append(code).Append(',');
                }
                builder.Append(employee.UniqueCode);
                project.MemberUniqueCodes = builder.ToString();
            }

            project.Team.Members.Add(employee);
            context.SaveChanges();

            return new ServiceResult { Success = true };
        }

        public ServiceResult RemoveMemberFromProject(int projectId, int memberId)
        {
            var project = context.Projects.Include(p => p.Team)
                              .FirstOrDefault(p => p.Id == projectId);

           

            if (project == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Project not found." };
            }

            var member = project.Team.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Member not found." };
            }

            if (member.Id == project.CreatedByUserId)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Cannot remove the project creator." };
            }

            project.Team.Members.Remove(member);

            var updatedUniqueCodes = project.MemberUniqueCodes.Split(',')
                .Where(code => code != member.UniqueCode)
                .ToArray();
            project.MemberUniqueCodes = string.Join(",", updatedUniqueCodes);

            context.SaveChanges();

            return new ServiceResult { Success = true };
        }





    }
}

