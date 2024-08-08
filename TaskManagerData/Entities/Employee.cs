using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManagerData.Enums;

namespace TaskManagerData.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string UniqueCode { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string ProfilePictureUrl { get; set; } = string.Empty; 

        public List<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
