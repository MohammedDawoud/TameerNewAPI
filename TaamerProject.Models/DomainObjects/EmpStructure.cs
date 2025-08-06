using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class EmpStructure : Auditable
    {
        public long StructureId { get; set; }
        public int EmpId { get; set; }
        public int? ManagerId { get; set; }
        public int BranchId { get; set; }
        public Employees? Employees { get; set; }
        public Employees? Managers { get; set; }

    }
}
