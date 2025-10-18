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
        private readonly CacheService _cache;

        public TaskService(ApplicationDbContext dbContext, CacheService cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<List<TaskM>> GetTasksByProject(Guid projectId) 
        {
            string key = $"tasks:project:{projectId}";
            var cachedTasks = await _cache.GetAsync<List<TaskM>>(key);
            if (cachedTasks != null) return cachedTasks;
            
            var tasks = await _dbContext.Tasks.Find(t => t.ProjectId == projectId).ToListAsync();
            await _cache.SetAsync(key, tasks);
            return tasks;
        }

        public async Task<TaskM?> GetTaskById(Guid taskId) 
        {
            string key = $"task_{taskId}";
            var cachedTask = await _cache.GetAsync<TaskM>(key);
            if (cachedTask != null) return cachedTask;
            
            var task = await _dbContext.Tasks.Find(t => t.TaskId == taskId).FirstOrDefaultAsync();
            if (task != null) await _cache.SetAsync(key, task);
            return task;
        }

        public async Task<TaskM> CreateTask(TaskM task)
        {
            task.TaskId = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            await _dbContext.Tasks.InsertOneAsync(task);
            
            await _cache.RemoveAsync($"tasks:project:{task.ProjectId}");            
            return task;
        }

        public async Task<TaskM?> UpdateTask(Guid taskId, TaskM task)
        {
            task.TaskId = taskId;
            var result = await _dbContext.Tasks.ReplaceOneAsync(t => t.TaskId == taskId, task);
            
            if (result.ModifiedCount > 0)
            {
                await _cache.RemoveAsync($"tasks:project:{task.ProjectId}");
                await _cache.RemoveAsync($"task_{taskId}");
                return task;
            }
            return null;
        }

        public async Task<bool> DeleteTask(Guid taskId) 
        {
            var task = await _dbContext.Tasks.Find(t => t.TaskId == taskId).FirstOrDefaultAsync();
            if (task == null) return false;
            var result = await _dbContext.Tasks.DeleteOneAsync(t => t.TaskId == taskId);
            
            if (result.DeletedCount > 0)
            {
                await _cache.RemoveAsync($"tasks:project:{task.ProjectId}");
                await _cache.RemoveAsync($"task_{taskId}");
                return true;
            }
            return false;
        } 
    }
}
