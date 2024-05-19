namespace DataAccessLayer.Models
{
    public class User : BaseEntity
    {
        public User() { }

        public User(string name, byte[] password, byte[] passwordSalt, string email) : base()
        {
            Name = name;
            Password = password;
            PasswordSalt = passwordSalt;
            Email = email;
        }
        public string Email { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; } 
        public byte[] PasswordSalt { get; set; }
        public ICollection<Task> OwnedTasks { get; set; }
        public ICollection<UserTask> AssignedTasks { get; set; }
    }
}
