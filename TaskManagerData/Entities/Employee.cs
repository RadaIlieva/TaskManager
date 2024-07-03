using System;
using System.Collections.Generic;
using TaskManagerData.Enums;

namespace TaskManagerData.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int? CompanyID { get; set; }
        public Company Company { get; set; } = null!;
        public List<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
