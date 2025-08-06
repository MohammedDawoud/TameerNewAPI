using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContractStage : Auditable
    {
        public int ContractStageId { get; set; }
        public string? Stage { get; set; }
        public string? StageDescreption { get; set; }
        public string? Stagestartdate { get; set; }
        public string? Stageenddate { get; set; }



        public int? ContractId { get; set; }
      

    
        public Contracts? Contracts { get; set; }
    }
}
