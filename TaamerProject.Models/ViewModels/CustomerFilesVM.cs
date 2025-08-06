using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class CustomerFilesVM
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
        public string? FileTypeName { get; set; }
        public string? UserName { get; set; }
    }
}
