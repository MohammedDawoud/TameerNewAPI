using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ArchiveFilesVM
    {
        public int ArchiveFileId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
