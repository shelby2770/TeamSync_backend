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
        
        public string Status { get; set; } = "To Do";
        
        public Guid? Assignee { get; set; }
        
        public DateTime? DueDate { get; set; }
        
        [Required(ErrorMessage = "ProjectId is required")]
        public Guid ProjectId { get; set; }
        
        [Required(ErrorMessage = "CreatedBy is required")]
        public Guid CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
