namespace TaskManagerProject.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
    }
}
