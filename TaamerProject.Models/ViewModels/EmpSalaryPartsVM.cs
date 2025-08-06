using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class EmpSalaryPartsVM
    {
        public Allowance? Communication { get; set; }
        public Allowance? Profession { get; set; }
        public Allowance? Transportation { get; set; }
        public Allowance? HousingAllowance { get; set; }

        public EmpSalaryPartsVM() {
            Communication = new Allowance();
            Profession = new Allowance();
            Transportation = new Allowance();
            HousingAllowance = new Allowance();
        }
    }

}
