using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ContractDetails : Auditable
    {

        public int ContractDetailId { get; set; }
        public int? ContractId { get; set; }
        public int? SerialId { get; set; }
        public string? Clause { get; set; }

        //public VacationType VacationTypeName { get; set; }
        public Contracts? Contracts { get; set; }
    }
}

