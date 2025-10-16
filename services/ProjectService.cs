using TeamSyncB.models;
using TeamSyncB.data;
using MongoDB.Driver;

namespace TeamSyncB.services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjects();
        Task<Project?> GetProjectById(Guid projectId);
        Task<Project> CreateProject(Project project);
        Task<List<Project>> GetUserProjects(Guid userId);
    }

    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await _dbContext.Projects.Aggregate().ToListAsync();
        }

        public async Task<Project?> GetProjectById(Guid projectId)
        {
            return await _dbContext.Projects.Find(p => p.ProjectId == projectId).FirstOrDefaultAsync();
        }

        public async Task<Project> CreateProject(Project project)
        {
            project.ProjectId = Guid.NewGuid();
            project.CreatedAt = DateTime.UtcNow;
            await _dbContext.Projects.InsertOneAsync(project);
            return project;
        }

        public async Task<List<Project>> GetUserProjects(Guid userId)
        {
            return await _dbContext.Projects
                .Find(p => p.CreatedBy == userId || p.Members.Contains(userId))
                .ToListAsync();
        }
    }
}
