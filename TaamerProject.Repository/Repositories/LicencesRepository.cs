using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
     public class LicencesRepository :ILicencesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public LicencesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<LicencesVM>> GetAllLicences(string SearchText)
        {
            var Licences = _TaamerProContext.Licences.Where(s => s.IsDeleted == false).Select(x => new LicencesVM
            {
                LicenceId = x.LicenceId,
                LicenceContractNo = x.LicenceContractNo??"",
                NoOfUsers = x.NoOfUsers??"",
                Type = x.Type??1,
                Support_Expiry_Date = x.Support_Expiry_Date??"",
                Email = x.Email??"",
                Mobile = x.Mobile??"",
                Mobile2 = x.Mobile2??"",
                Hosting_Expiry_Date = x.Hosting_Expiry_Date?? "E9CT3q6ItKXBskURZSoZZw==",
                Note = x.Note??"",


                Lic_Hosting_Info = x.Lic_Hosting_Info ?? "",
                Lic_HostingCheck_Info = x.Lic_HostingCheck_Info ?? false,
                Lic_HostingTimeType_Info = x.Lic_HostingTimeType_Info ?? 0,
                Lic_Hosting_NextTime_Info = x.Lic_Hosting_NextTime_Info ?? "",
                Lic_Hosting_IsSend_Info = x.Lic_Hosting_IsSend_Info ?? 0,

                Lic_TechSuppCon_Info = x.Lic_TechSuppCon_Info ?? "",
                Lic_TechSuppConCheck_Info = x.Lic_TechSuppConCheck_Info ?? false,
                Lic_TechSuppConTimeType_Info = x.Lic_TechSuppConTimeType_Info ?? 0,
                Lic_TechSuppCon_NextTime_Info = x.Lic_TechSuppCon_NextTime_Info ?? "",
                Lic_TechSuppCon_IsSend_Info = x.Lic_TechSuppCon_IsSend_Info ?? 0,

                Lic_Domain_Info = x.Lic_Domain_Info ?? "",
                Lic_DomainCheck_Info = x.Lic_DomainCheck_Info ?? false,
                Lic_DomainTimeType_Info = x.Lic_DomainTimeType_Info ?? 0,
                Lic_Domain_NextTime_Info = x.Lic_Domain_NextTime_Info ?? "",
                Lic_Domain_IsSend_Info = x.Lic_Domain_IsSend_Info ?? 0,

                Lic_Other_Info = x.Lic_Other_Info ?? "",
                Lic_OtherCheck_Info = x.Lic_OtherCheck_Info ?? false,
                Lic_OtherTimeType_Info = x.Lic_OtherTimeType_Info ?? 0,
                Lic_Other_NextTime_Info = x.Lic_Other_NextTime_Info ?? "",
                Lic_Other_IsSend_Info = x.Lic_Other_IsSend_Info ?? 0,

                Lic_Hosting_Cust = x.Lic_Hosting_Cust ?? "",
                Lic_HostingCheck_Cust = x.Lic_HostingCheck_Cust ?? false,
                Lic_Hosting_TimeType_Cust = x.Lic_Hosting_TimeType_Cust ?? 0,
                Lic_Hosting_NextTime_Cust = x.Lic_Hosting_NextTime_Cust ?? "",
                Lic_Hosting_IsSend_Cust = x.Lic_Hosting_IsSend_Cust ?? 0,

                Lic_TechSuppCon_Cust = x.Lic_TechSuppCon_Cust ?? "",
                Lic_TechSuppConCheck_Cust = x.Lic_TechSuppConCheck_Cust ?? false,
                Lic_TechSuppConTimeType_Cust = x.Lic_TechSuppConTimeType_Cust ?? 0,
                Lic_TechSuppCon_NextTime_Cust = x.Lic_TechSuppCon_NextTime_Cust ?? "",
                Lic_TechSuppCon_IsSend_Cust = x.Lic_TechSuppCon_IsSend_Cust ?? 0,

                Lic_Domain_Cust = x.Lic_Domain_Cust ?? "",
                Lic_DomainCheck_Cust = x.Lic_DomainCheck_Cust ?? false,
                Lic_DomainTimeType_Cust = x.Lic_DomainTimeType_Cust ?? 0,
                Lic_Domain_NextTime_Cust = x.Lic_Domain_NextTime_Cust ?? "",
                Lic_Domain_IsSend_Cust = x.Lic_Domain_IsSend_Cust ?? 0,

                Lic_Other_Cust = x.Lic_Other_Cust ?? "",
                Lic_OtherCheck_Cust = x.Lic_OtherCheck_Cust ?? false,
                Lic_OtherTimeType_Cust = x.Lic_OtherTimeType_Cust ?? 0,
                Lic_Other_NextTime_Cust = x.Lic_Other_NextTime_Cust ?? "",
                Lic_Other_IsSend_Cust = x.Lic_Other_IsSend_Cust ?? 0,

                Lic_Note_Info = x.Lic_Note_Info ?? "",
                Lic_Note_Cust = x.Lic_Note_Cust ?? "",

                Email2 = x.Email2 ?? "",
                Email3 = x.Email3 ?? "",
                Support_Start_Date= x.Support_Start_Date ??"",
                Subscrip_Domain=x.Subscrip_Domain ??false,
                Subscrip_Hosting=x.Subscrip_Hosting ??false,
                
                
                G_UID=x.G_UID ,
                ServerStorage=x.ServerStorage ??"",
                Cost=x.Cost??"",
                TotalCost=x.TotalCost??"",
                Tax = x.Tax ?? "",
                
                
                

            }).ToList();
            return Licences;
        }


    }
}
