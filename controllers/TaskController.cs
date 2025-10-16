using Microsoft.AspNetCore.Mvc;
using TeamSyncB.models;
using TeamSyncB.services;
using TaskM = TeamSyncB.models.Task;

namespace TeamSyncB.controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProject(projectId);
            return Ok(tasks);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(Guid taskId)
        {
            var task = await _taskService.GetTaskById(taskId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskM task)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newTask = await _taskService.CreateTask(task);
            return Ok(newTask);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] TaskM task)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedTask = await _taskService.UpdateTask(taskId, task);
            return updatedTask == null ? NotFound() : Ok(updatedTask);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var deleted = await _taskService.DeleteTask(taskId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
