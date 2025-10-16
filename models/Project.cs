using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TeamSyncB.models
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid ProjectId { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required(ErrorMessage = "CreatedBy is required")]
        [BsonRepresentation(BsonType.String)]
        public Guid CreatedBy { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public List<Guid> Members { get; set; } = new List<Guid>();
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
