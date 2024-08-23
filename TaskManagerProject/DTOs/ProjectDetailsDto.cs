namespace TaskManagerProject.DTOs
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MembersUniqueCodes { get; set; }
        public List<string> TeamMembers { get; set; } = new List<string>();
        public List<ProjectTaskDto> ProjectTasks { get; set; } = new List<ProjectTaskDto>();
    }
}
