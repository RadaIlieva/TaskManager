using System.Collections.Generic;

namespace TaskManagerData.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeamLeaderId { get; set; }
        public Employee TeamLeader { get; set; }
        public List<Employee> Members { get; set; } = new List<Employee>();
    }
}
