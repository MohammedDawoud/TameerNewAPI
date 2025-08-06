using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class TrailingFilesVM
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public int? TypeId { get; set; }
        public int? ProjectId { get; set; }
        public int? TrailingId { get; set; }
        public string? Notes { get; set; }
        public int? BranchId { get; set; }
      
    }
}
