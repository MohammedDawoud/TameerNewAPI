using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_CategorType : Auditable
    {
        public int CategorTypeId { get; set; }
        public string? NAmeAr { get; set; }
        public string? NAmeEn { get; set; }
    }
}
