using System;

namespace TaskManagerData.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TaskId { get; set; }
        public ProjectTask ProjectTask { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public int EmployeeId { get; set; }
    }
}
