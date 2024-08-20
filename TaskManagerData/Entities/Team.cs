using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerData.Entities
{
    public class Team
    {
       
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Employee> Members { get; set; } = new List<Employee>();
        public List<Project> Projects { get; set; } = new List<Project> ();
    }
}
