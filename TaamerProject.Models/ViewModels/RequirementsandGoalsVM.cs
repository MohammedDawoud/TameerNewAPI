using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{

    public class RequirementsandGoalsVM
    {

        public int RequirementId { get; set; }

        public string? RequirmentName { get; set; }
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectId { get; set; }

        public int? LineNumber { get; set; }
        public string? TimeNo { get; set; }
        public string? TimeType { get; set; }
        public string? TimeTypeName { get; set; }

        public string? timestr { get; set; }
        public string? EmployeeName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Name { get; set; }

    }
}
