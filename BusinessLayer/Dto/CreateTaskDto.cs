namespace BusinessLayer.Dto
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }
        public string? Status { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Tags { get; set; }
    }
}
