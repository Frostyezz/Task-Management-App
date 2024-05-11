namespace DataAccessLayer.Models
{
    public class User(string name, byte[] password, byte[] passwordSalt, string email) : BaseEntity
    {
        public string Email { get; set; } = email;
        public string Name { get; set; } = name;
        public byte[] Password { get; set; } = password;
        public byte[] PasswordSalt { get; set; } = passwordSalt;
    }
}
