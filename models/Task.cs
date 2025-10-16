using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TeamSyncB.models
{
    public class Task
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid TaskId { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Status { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public Guid? Assignee { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [Required(ErrorMessage = "ProjectId is required")]
        [BsonRepresentation(BsonType.String)]
        public Guid ProjectId { get; set; }
        
        [Required(ErrorMessage = "CreatedBy is required")]
        [BsonRepresentation(BsonType.String)]
        public Guid CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
