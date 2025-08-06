using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProjectArchivesReVM
    {
        public int ProArchReID { get; set; }
        public int? ProjectId { get; set; }
        public string? ReDate { get; set; }
        public string? ProjectNo { get; set; }
        public string? CustomerName { get; set; }
        public string? PhasesName { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? ProjectSubTypeName { get; set; }
        public int? Re_TypeID { get; set; }
        public string? Re_TypeName { get; set; }
        public int? Re_PhasesTaskId { get; set; }




    }
}
