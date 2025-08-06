using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
    public class EndWorkPrintVM
    {
        public int? EmpId{ get; set; }
        public int? ContractId { get; set; }
        public string? reson { get; set; }
        public string? resontxt { get; set; }

        public string? date { get; set; }

    }
}
