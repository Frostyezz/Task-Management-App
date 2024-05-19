using BusinessLayer.Dto;

namespace BusinessLayer.Contracts
{
    public interface ITaskService
    {
        IEnumerable<TaskDto> GetAllTasks();
        TaskDto? GetTaskById(Guid taskId);
        IEnumerable<TaskDto> GetTasksByAssigneeId(Guid userId);
        void CreateTask(DataAccessLayer.Models.Task task);
        void AssignUserToTask(Guid userId, Guid taskId);
        void UnassignUserFromTask(Guid userId, Guid taskId);
        void DeleteTask(Guid taskId);
        void UpdateTask(EditTaskDto task);
    }
}
