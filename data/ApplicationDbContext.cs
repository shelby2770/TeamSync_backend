using MongoDB.Driver;
using TeamSyncB.models;
using Microsoft.Extensions.Configuration;

namespace TeamSyncB.data
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];
            
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Project> Projects => _database.GetCollection<Project>("Projects");
        public IMongoCollection<TeamSyncB.models.Task> Tasks => _database.GetCollection<TeamSyncB.models.Task>("Tasks");
    }
}