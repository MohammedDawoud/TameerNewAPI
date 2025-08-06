using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class EmpNodeVM
    {
        public IEnumerable<EmployeesVM> nodeDataArray { get; set; }
        public IEnumerable<EmpStructureVM> linkDataArray { get; set; }
    }
}
