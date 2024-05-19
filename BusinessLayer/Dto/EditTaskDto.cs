using DataAccessLayer.Models;

namespace BusinessLayer.Dto
{
    public class EditTaskDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }
        public string? Status { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Tags { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
