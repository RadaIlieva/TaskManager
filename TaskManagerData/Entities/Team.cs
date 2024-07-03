using System.Collections.Generic;

namespace TaskManagerData.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public List<Employee> Members { get; set; } = new List<Employee>();
        public Employee TeamLeader { get; set; } = null!;
        public int TeamLeaderId { get; set; }
    }
}
