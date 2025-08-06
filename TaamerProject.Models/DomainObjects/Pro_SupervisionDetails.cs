using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_SupervisionDetails : Auditable
    {
        public int SuperDetId { get; set; }
        public int? SupervisionId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Note { get; set; }
        public int? IsRead { get; set; }
        public int? BranchId { get; set; }
        public string? ImageUrl { get; set; }
        public string? TheNumber { get; set; }
        public string? TheLocation { get; set; }
        public virtual Supervisions? Supervisions { get; set; }
    }
}
