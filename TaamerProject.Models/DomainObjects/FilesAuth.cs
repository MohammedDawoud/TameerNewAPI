using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class FilesAuth : Auditable
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
