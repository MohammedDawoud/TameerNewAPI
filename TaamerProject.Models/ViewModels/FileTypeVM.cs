using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class FileTypeVM
    {
        public int FileTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
