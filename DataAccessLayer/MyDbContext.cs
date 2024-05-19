using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

// dotnet ef migrations add Name
// dotnet ef database update

namespace DataAccessLayer
{
    public class MyDbContext : DbContext
    {
        private readonly string _windowsConnectionString = @"Server=.\SQLExpress;Database=TAPDatabase1;Trusted_Connection=True;TrustServerCertificate=true";
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_windowsConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserTask>()
                .HasKey(ut => new { ut.UserId, ut.TaskId });

            builder.Entity<UserTask>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany(t => t.Assignees)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Models.Task>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.OwnedTasks)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            var user1 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Chris",
                Email = "chris@mailinator.com",
                Password = new byte[] {  },
                PasswordSalt = new byte[] {  }
            };
            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Name = "Garrick",
                Email = "garrick@mailinator.com",
                Password = new byte[] {  },
                PasswordSalt = new byte[] {  }
            };

            var task1 = new DataAccessLayer.Models.Task
            {
                Id = Guid.NewGuid(),
                Title = "Task 1",
                Description = "Description for Task 1",
                Priority = "High",
                Deadline = DateTime.UtcNow.AddDays(7),
                OwnerId = user1.Id
            };

            var task2 = new DataAccessLayer.Models.Task
            {
                Id = Guid.NewGuid(),
                Title = "Task 2",
                Description = "Description for Task 2",
                Priority = "Medium",
                Deadline = DateTime.UtcNow.AddDays(14),
                OwnerId = user2.Id
            };

            var userTask1 = new UserTask
            {
                UserId = user1.Id,
                TaskId = task1.Id
            };

            var userTask2 = new UserTask
            {
                UserId = user2.Id,
                TaskId = task1.Id
            };

            builder.Entity<User>().HasData(user1, user2);
            builder.Entity<DataAccessLayer.Models.Task>().HasData(task1, task2);
            builder.Entity<UserTask>().HasData(userTask1, userTask2);
        }
    }
}
