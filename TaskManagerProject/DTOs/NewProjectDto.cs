namespace TaskManagerProject.Models
{
    public class NewProjectDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public List<string> MembersUniqueCodes { get; set; } = new List<string>();
    }
}
