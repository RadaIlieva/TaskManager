﻿using TaskManagerProject.Enums;

namespace TaskManagerProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string UniqueCode { get; set; }
        public DateTime DateOfBirth { get; set; }  
        public string ProfilePictureUrl { get; set; }  
        public UserRole Role { get; set; }
    }
}
