namespace DataAccessLayer.Models
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }
        public string? Status { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Tags { get; set; }
        public ICollection<UserTask> Assignees { get; set; } = new List<UserTask>();
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
    }
}
