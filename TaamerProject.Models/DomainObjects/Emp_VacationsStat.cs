using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Emp_VacationsStat : Auditable
    {
        public int Id { get; set; }
        public int? EmpID { get; set; }
        public int? EmpId { get; set; }
        public int? Year { get; set; }
        public int? Balance { get; set; }
        public int? Consumed { get; set; }
        public int? UserId  { get; set; }
    }
}
