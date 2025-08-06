using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.ViewModels
{
    public class Emp_AbsenceListVM
    {
        public int ID { get; set; }
        public string? AbsenceTime { get; set; }
        public decimal? First { get; set; }
        public decimal? Second { get; set; }
        public decimal? Third { get; set; }
        public decimal? Fourth { get; set; }
    }
}
