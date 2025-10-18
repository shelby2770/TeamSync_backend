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
        private readonly CacheService _cache;

        public UserService(ApplicationDbContext dbContext, CacheService cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<List<User>> GetAllUsers()
        {
            string key = "all_users";
            var cachedUsers = await _cache.GetAsync<List<User>>(key);
            if (cachedUsers != null) return cachedUsers;            
            var users = await _dbContext.Users.Aggregate().ToListAsync();
            await _cache.SetAsync(key, users);
            return users;
        }

        public async Task<User?> GetUserById(Guid userId)
        {
            string key = $"user_{userId}";
            var cachedUser = await _cache.GetAsync<User>(key);
            if (cachedUser != null) return cachedUser;            
            var user = await _dbContext.Users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user != null) await _cache.SetAsync(key, user);
            return user;
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
            await _cache.RemoveAsync("all_users");
            return user;
        }

        public async Task<User?> ValidateLogin(string email, string password)
        {
            string key = $"user_email_{email}";            
            var cachedUser = await _cache.GetAsync<User>(key);
            
            User? user;
            if (cachedUser == null){
                user = await _dbContext.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
                if (user != null) await _cache.SetAsync(key, user);
            }
            else user = cachedUser;
            
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) return user;
            return null;
        }
    }
}