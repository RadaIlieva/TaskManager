using TaskManagerData.Entities;

namespace TaskManagerProject.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AssignedToEmployeeId { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Employee AssignedToEmployee { get; set; } = null!;
        public Project Project { get; set; } = null!;
    }
}