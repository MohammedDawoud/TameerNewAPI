using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProjectCommentsVM
    {
        public int CommentId { get; set; }
        public string? Comment { get; set; }
        public DateTime? Date { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserImgUrl { get; set; }
    }
}
