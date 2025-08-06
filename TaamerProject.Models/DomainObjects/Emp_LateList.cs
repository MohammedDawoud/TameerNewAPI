using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Models.DomainObjects
{
    public class Emp_LateList : Auditable
    {
        public int ID { get; set; }
        public string? LateTime { get; set; }
        public decimal? First { get; set; }
        public decimal? Second { get; set; }
        public decimal? Third { get; set; }
        public decimal? Fourth { get; set; }
    }

   public class Law_Regulations
    {
        public List<Emp_AbsenceListVM> emp_AbsenceLists { get; set; }
        public List<Emp_LateListVM> emp_LateLists { get; set; }
    }
}
