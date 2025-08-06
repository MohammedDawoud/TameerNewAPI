using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
    public class CommercialActivityVM
    {
        public int CommercialActivityId { get; set; }
        public int? Type { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
