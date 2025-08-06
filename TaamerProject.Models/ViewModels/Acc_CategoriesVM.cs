using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_CategoriesVM
    {
        public int CategoryId { get; set; }
        public string? NAmeAr { get; set; }
        public string? NAmeEn { get; set; }
        public string? Code { get; set; }
        public string? Note { get; set; }
        public decimal? Price { get; set; }

        public int? CategorTypeId { get; set; }
        public int? AccountId { get; set; }

        public string? CategorTypeName { get; set; }
    }
}
