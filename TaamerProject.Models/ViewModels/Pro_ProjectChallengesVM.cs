using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_ProjectChallengesVM
    {
        public int ProjectChallengeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? ProjectId { get; set; }
        public int? StepId { get; set; }
        public int? LineNumber { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
    }
}
