using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ArchiveFiles : Auditable
    {
        public int ArchiveFileId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int BranchId { get; set; }
    }
}
