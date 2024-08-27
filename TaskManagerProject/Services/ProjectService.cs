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
                    Name = $"{m.FirstName} {m.LastName}"
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

        public ServiceResult AddTaskToProject(ProjectTaskDto taskDto)
        {
            try
            {
                var newTask = new TaskManagerData.Entities.ProjectTask
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    StartDate = taskDto.StartDate,
                    EndDate = taskDto.EndDate,
                    AssignedToEmployeeId = taskDto.AssignedToEmployeeId,
                    ProjectId = taskDto.ProjectId
                };

                context.ProjectTasks.Add(newTask);
                context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public ProjectTaskDto GetTaskById(int taskId)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
                return null;

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                AssignedToEmployeeId = task.AssignedToEmployeeId ?? 0,
                ProjectId = task.ProjectId
            };
        }

        public Project GetProjectById(int projectId)
        {
            return context.Projects.Find(projectId); 
        }

        public ServiceResult UpdateTaskStatus(int taskId, string status)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Task not found." };
            }

            context.SaveChanges();
            return new ServiceResult { Success = true };
        }

        public ServiceResult CreateTask(ProjectTaskDto model, int currentUserId)
        {
            var project = GetProjectById(model.ProjectId);

            if (project == null || project.CreatedByUserId != currentUserId)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Unauthorized" };
            }

            var task = new TaskManagerData.Entities.ProjectTask
            {
                Title = model.Title,
                Description = model.Description,
                AssignedToEmployeeId = model.AssignedToEmployeeId,
                ProjectId = model.ProjectId,
                StartDate = DateTime.Now
            };

            context.Add(task);
            context.SaveChanges();

            return new ServiceResult { Success = true };
        }

        [HttpPost]
        public IActionResult DeleteTaskAction(int taskId)
        {
            var result = DeleteTask(taskId);
            if (result.Success)
            {
                return RedirectToAction("Details", new { id = result.ProjectId });
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                var projectDetails = GetProjectDetails(result.ProjectId);
                return View("Details", projectDetails);
            }
        }

        public ServiceResult DeleteTask(int taskId)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Task not found." };
            }

            var projectId = task.ProjectId;

            context.ProjectTasks.Remove(task);
            context.SaveChanges();

            return new ServiceResult { Success = true, ProjectId = projectId }; 
        }

        public ServiceResult UpdateTaskDescription(int taskId, string newDescription)
        {
            var task = context.ProjectTasks.Find(taskId);
            if (task == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Task not found." };
            }

            task.Description = newDescription;
            context.SaveChanges();

            return new ServiceResult { Success = true };
        }
    }
}

