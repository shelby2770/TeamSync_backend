using TeamSyncB.models;
using TeamSyncB.data;
using MongoDB.Driver;
using BCrypt.Net;

namespace TeamSyncB.services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserById(Guid userId);
        Task<User> CreateUser(User user);
        Task<User?> ValidateLogin(string email, string password);
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
            
            if (exist != null) throw new Exception("Email already exists");
            
            user.UserId = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _dbContext.Users.InsertOneAsync(user);
            return user;
        }

        public async Task<User?> ValidateLogin(string email, string password)
        {
            var user = await _dbContext.Users
                .Find(u => u.Email == email)
                .FirstOrDefaultAsync();
            
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) return user;            
            return null;
        }
    }
}