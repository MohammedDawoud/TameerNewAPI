using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class EmpContractDetailVM
    {

        public int ContractDetailId { get; set; }
        public int? ContractId { get; set; }
        public int? SerialId { get; set; }
        public string? Clause { get; set; }      
        public bool IsSearch { get; set; }
    }
}
