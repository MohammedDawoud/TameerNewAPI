using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class CustomerFiles : Auditable
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? Description { get; set; }
        public string? Extenstion { get; set; }
        public string? OriginalFileName { get; set; }
        public string? UploadDate { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? FileUrl { get; set; }
        public int? TypeId { get; set; }
       
        public FileType? FileType { get; set; }
        public  Users? Users { get; set; }
    }
}
