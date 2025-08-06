using System;
namespace TaamerProject.Models
{
    public class ProjectComments : Auditable
    {
        public int CommentId { get; set; }
        public string? Comment { get; set; }
        public DateTime? Date { get; set; }
        public int ProjectId { get; set; }
        public int? UserId { get; set; }
        public virtual Users? Users { get; set; }
    }
}
