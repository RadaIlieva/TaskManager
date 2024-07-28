using AutenticationAutorizationAPI.Models;
using AutenticationAutorizationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskManagerData.Contexts;
using TaskManagerData.Entities;

namespace AutenticationAutorizationAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly AppDbContext dbContext;

        public AuthenticationService(IConfiguration configuration, AppDbContext dbContext)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        public async Task<Employee> Register(RegisterUserDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ArgumentException("Passwords do not match");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            string uniqueCode = await GenerateUniqueCode();

            var employee = new Employee
            {
                FirstName = request.Username.Split(' ')[0],
                LastName = request.Username.Split(' ').Length > 1 ? request.Username.Split(' ')[1] : string.Empty,
                Email = IsValidEmail(request.Email) ? request.Email : null,
                PhoneNumber = IsValidPhoneNumber(request.Email) ? request.Email : null,
                PasswordHash = passwordHash,
                UniqueCode = uniqueCode,
                Role = TaskManagerData.Enums.UserRole.Employee // assuming default role is Employee, adjust accordingly
            };

            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<string> Login(LoginUserDto request)
        {
            Employee employee;
            if (IsValidEmail(request.EmailOrPhone))
            {
                employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Email == request.EmailOrPhone);
            }
            else if (IsValidPhoneNumber(request.EmailOrPhone))
            {
                employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.PhoneNumber == request.EmailOrPhone);
            }
            else
            {
                throw new ArgumentException("Invalid Email or Phone Number");
            }

            if (employee == null || !BCrypt.Net.BCrypt.Verify(request.Password, employee.PasswordHash))
            {
                return null;
            }

            string token = CreateToken(employee);

            return token;
        }

        private async Task<string> GenerateUniqueCode()
        {
            string uniqueCode;
            bool exists;

            do
            {
                uniqueCode = new Random().Next(1000, 9999).ToString();
                exists = await dbContext.Employees.AnyAsync(e => e.UniqueCode == uniqueCode);
            } while (exists);

            return uniqueCode;
        }

        private string CreateToken(Employee employee)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.Name, employee.FirstName),
                new Claim("UniqueCode", employee.UniqueCode),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\d{10}$");
        }
    }
}
