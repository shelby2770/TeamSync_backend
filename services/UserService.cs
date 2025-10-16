using TeamSyncB.models;
using TeamSyncB.data;
using MongoDB.Driver;

namespace TeamSyncB.services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserById(Guid userId);
        Task<User> CreateUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.Aggregate().ToListAsync();
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            return await _dbContext.Users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUser(User user)
        {
            var exist = await _dbContext.Users
                .Find(u => u.Email == user.Email)
                .FirstOrDefaultAsync();
            
            if (exist!=null) throw new Exception("Email already exists");
            
            user.UserId = Guid.NewGuid();
            await _dbContext.Users.InsertOneAsync(user);
            return user;
        }
    }
}