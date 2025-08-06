using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class OutInImagesTo : Auditable
    {
        public int ImageToId { get; set; }
        public int? OutInboxId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
