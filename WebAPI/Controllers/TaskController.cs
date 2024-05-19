using BusinessLayer.Contracts;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Dto;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _taskService = taskService;
            _httpContextAccessor = httpContextAccessor;
            _authService = new AuthService(configuration);
        }

        /// <summary>
        /// Endpoint for retrieving all tasks.
        /// </summary>
        /// <returns>An IEnumerable of TaskDto representing all tasks.</returns>
        [HttpGet]
        public IEnumerable<TaskDto> GetAllTasks()
        {
            return _taskService.GetAllTasks();
        }

        /// <summary>
        /// Endpoint for retrieving a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <returns>An ActionResult containing the TaskDto of the retrieved task.</returns>
        [HttpGet("{id}")]
        public ActionResult<TaskDto> GetTaskById(Guid id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        /// <summary>
        /// Endpoint for retrieving tasks assigned to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An IEnumerable of TaskDto representing tasks assigned to the user.</returns>
        [HttpGet("assignee/{userId}")]
        public IEnumerable<TaskDto> GetTasksByAssigneeId(Guid userId)
        {
            return _taskService.GetTasksByAssigneeId(userId);
        }

        /// <summary>
        /// Endpoint for creating a new task.
        /// </summary>
        /// <param name="taskDto">The data for creating the new task.</param>
        /// <returns>Status 201</returns>
        [HttpPost]
        public IActionResult CreateTask(CreateTaskDto taskDto)
        {
            string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = authorizationHeader?.Split(" ")[1];
            Guid? userId = null;
            bool isValidJwt = _authService.VerifyToken(token, out userId);
            if (isValidJwt == false || userId == null) 
            {
                return Unauthorized();
            }

            var task = new DataAccessLayer.Models.Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = taskDto.Priority,
                Deadline = taskDto.Deadline,
                Status = taskDto.Status,
                Categories = taskDto.Categories,
                Tags = taskDto.Tags,
                OwnerId = userId ?? new Guid(),
            };

            _taskService.CreateTask(task);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, taskDto);
        }

        /// <summary>
        /// Endpoint for assigning a user to a task.
        /// </summary>
        /// <param name="assignmentDto">The data for the assignment.</param>
        /// <returns>Status 200 Ok</returns>
        [HttpPost("assign")]
        public IActionResult AssignUserToTask(TaskAssignmentDto assignmentDto)
        {
            _taskService.AssignUserToTask(assignmentDto.UserId, assignmentDto.TaskId);
            return Ok();
        }

        /// <summary>
        /// Endpoint for unassigning a user from a task.
        /// </summary>
        /// <param name="assignmentDto">The data for the unassignment.</param>
        /// <returns>Status 200 Ok</returns>
        [HttpDelete("assign")]
        public IActionResult UnassignUserFromTask(TaskAssignmentDto assignmentDto)
        {
            _taskService.UnassignUserFromTask(assignmentDto.UserId, assignmentDto.TaskId);
            return Ok();
        }

        /// <summary>
        /// Endpoint for updating a task.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="updateDto">The data for updating the task.</param>
        /// <returns>Status 204 NoContent</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, EditTaskDto updateDto)
        {
            var existingTask = _taskService.GetTaskById(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = updateDto.Title;
            existingTask.Description = updateDto.Description;
            existingTask.Priority = updateDto.Priority;
            existingTask.Deadline = updateDto.Deadline;
            existingTask.Status = updateDto.Status;
            existingTask.Categories = updateDto.Categories;
            existingTask.Tags = updateDto.Tags;

            _taskService.UpdateTask(new EditTaskDto
            {
                Title = existingTask.Title,
                Description = existingTask.Description,
            Priority = existingTask.Priority,
            Deadline = existingTask.Deadline,
            Status = existingTask.Status,
            Categories = existingTask.Categories,
            Tags = existingTask.Tags,
            OwnerId = existingTask.OwnerId
        });

            return NoContent();
        }

        /// <summary>
        /// Endpoint for deleting a task.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>Status 204 NoContent</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            _taskService.DeleteTask(id);
            return NoContent();
        }
    }


}
