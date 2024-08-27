using AutenticationAutorizationAPI.Models;
using AutenticationAutorizationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public async Task<bool> RegisterAsync(RegisterUserDto registerUserDto)
        {
            if (await dbContext.Employees.AnyAsync(e => e.Email == registerUserDto.UserEmail))
            {
                throw new ArgumentException("User with this email or phone number already exists.");
            }

            var employee = new Employee
            {
                FirstName = ExtractFirstName(registerUserDto.Name),
                LastName = ExtractLastName(registerUserDto.Name),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password), 
                UniqueCode = GenerateUniqueCode(),
                Role = TaskManagerData.Enums.UserRole.Employee 
            };

            if (IsValidEmail(registerUserDto.UserEmail))
            {
                employee.Email = registerUserDto.UserEmail;
            }
            
            else
            {
                throw new ArgumentException("Invalid Email or Phone Number");
            }

            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<string> Login(LoginUserDto request)
        {
            
            var employee = await dbContext.Employees
                .FirstOrDefaultAsync(e => e.Email == request.UserEmail);

            
            if (employee == null || !BCrypt.Net.BCrypt.Verify(request.Password, employee.PasswordHash))
            {
                return null;
            }

            string token = CreateToken(employee);

            return token;
        }


        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

       

        private string CreateToken(Employee employee)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.FirstName),
                new Claim("UniqueCode", employee.UniqueCode),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

      

        private string ExtractFirstName(string fullName) => fullName.Split(' ')[0];
        private string ExtractLastName(string fullName) => fullName.Split(' ').Length > 1 ? fullName.Split(' ')[1] : string.Empty;

        private string GenerateUniqueCode() => new Random().Next(1000, 9999).ToString();
    }
}
