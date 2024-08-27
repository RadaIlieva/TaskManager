namespace TaskManagerProject.DTOs
{
    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AssignedToEmployeeId { get; set; }
        public string AssignedToEmployeeName { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
    }

}
