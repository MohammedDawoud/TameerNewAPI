using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaamerProject.Models
{
    public class EmpContractDetail : Auditable
    {
       
        public int ContractDetailId { get; set; }
        public int? ContractId { get; set; }
        [JsonPropertyName("clauseId")]
        public int? SerialId { get; set; }
        public string? Clause { get; set; }
       
        //public VacationType VacationTypeName { get; set; }
        public EmpContract? EmpContracts { get; set; }
    }
}
