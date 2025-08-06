using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Licences : Auditable
    {
        public int LicenceId { get; set; }
        public string? G_UID { get; set; }
        public string? LicenceContractNo { get; set; }
        public string? NoOfUsers { get; set; }
        public int? Type { get; set; }
        public string? Support_Start_Date { get; set; }
        public string? Support_Expiry_Date { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Hosting_Expiry_Date { get; set; }
        public string? Note { get; set; }

        public string? Lic_Hosting_Info { get; set; }
        public bool? Lic_HostingCheck_Info { get; set; }
        public int? Lic_HostingTimeType_Info { get; set; }
        public string? Lic_Hosting_NextTime_Info { get; set; }
        public int? Lic_Hosting_IsSend_Info { get; set; }


        public string? Lic_TechSuppCon_Info { get; set; }
        public bool? Lic_TechSuppConCheck_Info { get; set; }
        public int? Lic_TechSuppConTimeType_Info { get; set; }
        public string? Lic_TechSuppCon_NextTime_Info { get; set; }
        public int? Lic_TechSuppCon_IsSend_Info { get; set; }


        public string? Lic_Domain_Info { get; set; }
        public bool? Lic_DomainCheck_Info { get; set; }
        public int? Lic_DomainTimeType_Info { get; set; }
        public string? Lic_Domain_NextTime_Info { get; set; }
        public int? Lic_Domain_IsSend_Info { get; set; }


        public string? Lic_Other_Info { get; set; }
        public bool? Lic_OtherCheck_Info { get; set; }
        public int? Lic_OtherTimeType_Info { get; set; }
        public string? Lic_Other_NextTime_Info { get; set; }
        public int? Lic_Other_IsSend_Info { get; set; }


        public string? Lic_Hosting_Cust { get; set; }
        public bool? Lic_HostingCheck_Cust { get; set; }
        public int? Lic_Hosting_TimeType_Cust { get; set; }
        public string? Lic_Hosting_NextTime_Cust { get; set; }
        public int? Lic_Hosting_IsSend_Cust { get; set; }


        public string? Lic_TechSuppCon_Cust { get; set; }
        public bool? Lic_TechSuppConCheck_Cust { get; set; }
        public int? Lic_TechSuppConTimeType_Cust { get; set; }
        public string? Lic_TechSuppCon_NextTime_Cust { get; set; }
        public int? Lic_TechSuppCon_IsSend_Cust { get; set; }


        public string? Lic_Domain_Cust { get; set; }
        public bool? Lic_DomainCheck_Cust { get; set; }
        public int? Lic_DomainTimeType_Cust { get; set; }
        public string? Lic_Domain_NextTime_Cust { get; set; }
        public int? Lic_Domain_IsSend_Cust { get; set; }


        public string? Lic_Other_Cust { get; set; }
        public bool? Lic_OtherCheck_Cust { get; set; }
        public int? Lic_OtherTimeType_Cust { get; set; }
        public string? Lic_Other_NextTime_Cust { get; set; }
        public int? Lic_Other_IsSend_Cust { get; set; }


        public string? Lic_Note_Info { get; set; }
        public string? Lic_Note_Cust { get; set; }

        public string? Email2 { get; set; }
        public string? Email3 { get; set; }

        public bool? Subscrip_Domain { get; set; }
        public bool? Subscrip_Hosting { get; set; }

        public string? ServerStorage { get; set; }
        public string? Cost { get; set; }
        public string? Tax { get; set; }
        public string? TotalCost { get; set; }
        public string? Mobile2 { get; set; }
    }
}
