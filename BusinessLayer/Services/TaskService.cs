using BusinessLayer.Contracts;
using DataAccessLayer.Models;
using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Dto;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<DataAccessLayer.Models.Task> _taskRepository;
        private readonly IRepository<UserTask> _userTaskRepository;
        private readonly IRepository<User> _userRepository;

        public TaskService(IRepository<DataAccessLayer.Models.Task> taskRepository, IRepository<UserTask> userTaskRepository, IRepository<User> userRepository)
        {
            _taskRepository = taskRepository;
            _userTaskRepository = userTaskRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<TaskDto> GetAllTasks()
        {
            var tasks = _taskRepository.GetAllAsQueryable().Include(t => t.Owner).Include(t => t.Assignees).ThenInclude(ta => ta.User).ToList();

            // Manually project Task entities to TaskDto objects
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                Deadline = t.Deadline,
                Status = t.Status,
                Categories = t.Categories,
                Tags = t.Tags,
                Owner = new UserDto
                {
                    Id = t.Owner?.Id,
                    Name = t.Owner?.Name,
                    Email = t.Owner?.Email
                },
                Assignees = t.Assignees.Select(a => new UserDto
                {
                    Id = a.User?.Id,
                    Name = a.User?.Name,
                    Email = a.User?.Email
                }).ToList()
            });
        }

        public TaskDto? GetTaskById(Guid taskId)
        {
            var tasks = _taskRepository.FindAsQueryable(t => t.Id == taskId).Include(t => t.Owner).Include(t => t.Assignees).ThenInclude(ta => ta.User).ToList();
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                Deadline = t.Deadline,
                Status = t.Status,
                Categories = t.Categories,
                Tags = t.Tags,
                OwnerId = t.OwnerId,
                Owner = new UserDto
                {
                    Id = t.Owner?.Id,
                    Name = t.Owner?.Name,
                    Email = t.Owner?.Email
                },
                Assignees = t.Assignees.Select(a => new UserDto
                {
                    Id = a.User?.Id,
                    Name = a.User?.Name,
                    Email = a.User?.Email
                }).ToList()
            }).FirstOrDefault();
        }

        public IEnumerable<TaskDto> GetTasksByAssigneeId(Guid userId)
        {
            var assignedTasks = _userRepository.FindAsQueryable(u => u.Id == userId).Include(t => t.AssignedTasks)
                                            .SelectMany(u => u.AssignedTasks).Include(t => t.Task).ThenInclude(t => t.Assignees).ThenInclude(t => t.User)
                                            .ToList();

            return assignedTasks.Select(t => new TaskDto
            {
                Id = t.Task.Id,
                Title = t.Task.Title,
                Description = t.Task.Description,
                Priority = t.Task.Priority,
                Deadline = t.Task.Deadline,
                Status = t.Task.Status,
                Categories = t.Task.Categories,
                Tags = t.Task.Tags,
                Owner = new UserDto
                {
                    Id = t.Task.Owner?.Id,
                    Name = t.Task.Owner?.Name,
                    Email = t.Task.Owner?.Email
                },
                Assignees = t.Task.Assignees.Select(a => new UserDto
                {
                    Id = a.User?.Id,
                    Name = a.User?.Name,
                    Email = a.User?.Email
                }).ToList()
            });
        }

        public void CreateTask(DataAccessLayer.Models.Task task)
        {
            _taskRepository.Add(task);
            _taskRepository.SaveChanges();
        }

        public void AssignUserToTask(Guid userId, Guid taskId)
        {
            UserTask userTask = new() { UserId = userId, TaskId = taskId };
            _userTaskRepository.Add(userTask);
            _userTaskRepository.SaveChanges();
        }

        public void UnassignUserFromTask(Guid userId, Guid taskId)
        {
            var userTask = _userTaskRepository.Find(ut => ut.UserId == userId && ut.TaskId == taskId).FirstOrDefault();
            if (userTask != null)
            {
                _userTaskRepository.Remove(userTask);
                _userTaskRepository.SaveChanges();
            }
        }

        public void DeleteTask(Guid taskId)
        {
            var task = _taskRepository.FindAsQueryable(t => t.Id == taskId).FirstOrDefault();
            if (task != null)
            {
                _taskRepository.Remove(task);
                _taskRepository.SaveChanges();
            }
        }

        public void UpdateTask(EditTaskDto task)
        {
            var taskEntity = new DataAccessLayer.Models.Task
            {
                Id = task.Id ?? new Guid(),
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Deadline = task.Deadline,
                Status = task.Status,
                Categories = task.Categories,
                Tags = task.Tags,
                OwnerId = task.OwnerId ?? new Guid(),
            };

            _taskRepository.Update(taskEntity);
            _taskRepository.SaveChanges();
        }
    }
}
