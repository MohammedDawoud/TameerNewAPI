using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class FilesAuthVM
    {
        public int FilesAuthId { get; set; }
        public string? AppKey { get; set; }
        public string? AppSecret { get; set; }
        public string? RedirectUri { get; set; } 
        public string? Code { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? FolderName { get; set; }
        public int? ExpiresIn { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? TypeId { get; set; }
        public int? BranchId { get; set; }
        public bool? Sendactive { get; set; }

    }
}
