namespace TaskManagerProject.DTOs
{
    public class GnattChatraTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int AssignedToEmployeeId { get; set; }  
        public int ProjectId { get; set; }  

    }
}
