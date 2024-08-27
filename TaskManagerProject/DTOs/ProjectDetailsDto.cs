namespace TaskManagerProject.DTOs
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MemberDto> TeamMembers { get; set; }
        public List<ProjectTaskDto> ProjectTasks { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsCreator { get; set; }
    }
}