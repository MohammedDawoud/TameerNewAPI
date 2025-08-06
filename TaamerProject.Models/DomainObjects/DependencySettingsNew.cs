using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class DependencySettingsNew : Auditable
    {
        public int DependencyId { get; set; }
        public int? PredecessorId { get; set; }
        public int? SuccessorId { get; set; }
        public int? Type { get; set; }
        public int? ProjSubTypeId { get; set; }
        public int? BranchId { get; set; }
        public virtual SettingsNew? SettingsPredecessor { get; set; }
        public virtual SettingsNew? SettingsSuccessor { get; set; }


        //public Tasks Tasks { get; set; }
    }
}
