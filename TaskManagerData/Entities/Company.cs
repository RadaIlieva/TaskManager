using System.Collections.Generic;

namespace TaskManagerData.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
