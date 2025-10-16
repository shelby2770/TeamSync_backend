using TeamSyncB.models;
using TeamSyncB.data;
using MongoDB.Driver;
using TaskM = TeamSyncB.models.Task;

namespace TeamSyncB.services
{
    public interface ITaskService
    {
        Task<List<TaskM>> GetTasksByProject(Guid projectId);
        Task<TaskM?> GetTaskById(Guid taskId);
        Task<TaskM> CreateTask(TaskM task);
        Task<TaskM?> UpdateTask(Guid taskId, TaskM task);
        Task<bool> DeleteTask(Guid taskId);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskService(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public async Task<List<TaskM>> GetTasksByProject(Guid projectId) 
        {
            return await _dbContext.Tasks.Find(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskM?> GetTaskById(Guid taskId) 
        {
            return await _dbContext.Tasks.Find(t => t.TaskId == taskId).FirstOrDefaultAsync();
        }

        public async Task<TaskM> CreateTask(TaskM task)
        {
            task.TaskId = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            await _dbContext.Tasks.InsertOneAsync(task);
            return task;
        }

        public async Task<TaskM?> UpdateTask(Guid taskId, TaskM task)
        {
            task.TaskId = taskId;
            var result = await _dbContext.Tasks.ReplaceOneAsync(t => t.TaskId == taskId, task);
            return result.ModifiedCount > 0 ? task : null;
        }

        public async Task<bool> DeleteTask(Guid taskId) 
        {
            return (await _dbContext.Tasks.DeleteOneAsync(t => t.TaskId == taskId)).DeletedCount > 0;
        } 
    }
}
