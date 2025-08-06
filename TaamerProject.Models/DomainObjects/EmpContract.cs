using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class EmpContract : Auditable
    {
        
        public int ContractId { get; set; }
        public string? ContractCode { get; set; }
        public int? OrgId { get; set; }
        public int? CompanyRepresentativeId { get; set; }
        public string? PerSe { get; set; }
        public int EmpId { get; set; }
        public int? ContTypeId { get; set; }
        public int? ContDuration { get; set; }
        public string? StartDatetxt { get; set; }
        public string? StartWorkDate { get; set; }
        public string? EndWorkDate { get; set; }
        public string? EndDatetxt { get; set; }
        public int? ProbationDuration { get; set; }
        public int? Workingdaysperweek { get; set; }
        public int? Dailyworkinghours { get; set; }
        public int? Workinghoursperweek { get; set; }
        public int Durationofannualleave { get; set; }
        public decimal? FreelanceAmount { get; set; }

        public int? Paycase { get; set; }
        public int? ProbationTypeId { get; set; }
        public int? Restrictedmode { get; set; }
        public int? NotTodivulgeSecrets { get; set; }
        public int? RestrictionDuration { get; set; }
        public string? Identifyplaces { get; set; }
        public string? Withregardtowork { get; set; }
        public int? NotTodivulgeSecretsDuration { get; set; }
        public string? SecretsIdentifyplaces { get; set; }
        public string? SecretsWithregardtowork { get; set; }
        public int? ContractTerminationNotice { get; set; }
        public int? Compensation { get; set; }
        public decimal? CompensationBothParty { get; set; }
        public decimal? Firstpartycompensation { get; set; }
        public decimal? Secondpartycompensation { get; set; }
        public int? Year { get; set; }        
       public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? BranchId { get; set; }
        public int? NationalityId { get; set; }
        public decimal? DailyEmpCost { get; set; }
        public int? ContractSource { get; set; }
        public decimal? EmpHourlyCost { get; set; }
        public Branch? BranchName { get; set; }
        public Nationality? NatName { get; set; }
        public virtual Employees? Employees { get; set; }

      

        public virtual List<EmpContractDetail>? EmpContractDetails { get; set; }
        public virtual Users? AddUsers { get; set; }
        public virtual Users? UpdateUsers { get; set; }

    }
}
