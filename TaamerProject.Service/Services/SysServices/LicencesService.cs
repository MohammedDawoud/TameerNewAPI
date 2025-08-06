using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using System.Security.Cryptography;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class LicencesService : ILicencesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ILicencesRepository _LicencesRepository;



        public LicencesService(TaamerProjectContext dataContext, ISystemAction systemAction, ILicencesRepository licencesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _LicencesRepository = licencesRepository;


        }

        public async Task<IEnumerable<LicencesVM>> GetAllLicences(string SearchText)
        {
            var Licences =await _LicencesRepository.GetAllLicences(SearchText);
            var orgdata = _TaamerProContext.Organizations.FirstOrDefault(x => x.IsDeleted == false);
            foreach (var lic in Licences)
            {
                lic.LicenceContractNo = DecryptValue(lic.LicenceContractNo);
                lic.NoOfUsers = DecryptValue(lic.NoOfUsers);
                lic.Support_Expiry_Date = DecryptValue(lic.Support_Expiry_Date);
                lic.Hosting_Expiry_Date = DecryptValue(lic.Hosting_Expiry_Date);
                lic.DBName = _TaamerProContext.GetDatabaseName();
                lic.CustomerName = orgdata.NameAr;
                lic.DomaniName = orgdata.ComDomainLink;
                lic.IPAddress = orgdata.ComDomainAddress;
            }


            return Licences;
        }
        public GeneralMessage SaveLicence(Licences Licence, int UserId, int BranchId)
        {
            try
            {

                if (Licence.LicenceId == 0)
                {

                    Licence.AddUser = UserId;
                    Licence.AddDate = DateTime.Now;
                    _TaamerProContext.Licences.Add(Licence);
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var LicenceUpdated = _TaamerProContext.Licences.Where(x=>x.LicenceId==Licence.LicenceId).FirstOrDefault();
                    if (LicenceUpdated != null)
                    {
                        LicenceUpdated.LicenceContractNo = EncryptValue(Licence.LicenceContractNo);
                        LicenceUpdated.NoOfUsers = EncryptValue(Licence.NoOfUsers);
                        LicenceUpdated.Type = Licence.Type;
                        LicenceUpdated.Support_Expiry_Date = EncryptValue(Licence.Support_Expiry_Date);
                        LicenceUpdated.Email = Licence.Email;
                        LicenceUpdated.Email2 = Licence.Email2;
                        LicenceUpdated.Email3 = Licence.Email3;
                        LicenceUpdated.Mobile = Licence.Mobile;
                        LicenceUpdated.Mobile2 = Licence.Mobile2;
                        LicenceUpdated.Support_Start_Date=Licence.Support_Start_Date;
                        LicenceUpdated.Subscrip_Domain = Licence.Subscrip_Domain;
                        LicenceUpdated.Subscrip_Hosting = Licence.Subscrip_Hosting;

                        LicenceUpdated.ServerStorage = Licence.ServerStorage;
                        LicenceUpdated.Cost = Licence.Cost;
                        LicenceUpdated.Tax = Licence.Tax;
                        LicenceUpdated.TotalCost = Licence.TotalCost;

                        if (Licence.Hosting_Expiry_Date == null || Licence.Hosting_Expiry_Date == "بدون")
                        {
                            LicenceUpdated.Hosting_Expiry_Date = null;
                        }
                        else
                        {
                            LicenceUpdated.Hosting_Expiry_Date = EncryptValue(Licence.Hosting_Expiry_Date);
                        }

                        LicenceUpdated.Note = Licence.Note;
                        LicenceUpdated.UpdateUser = UserId;
                        LicenceUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage UpdatSupportDate(Licences Licence, int UserId, int BranchId)
        {
            try
            {

               
                    var LicenceUpdated = _TaamerProContext.Licences.Where(x => x.LicenceId == Licence.LicenceId).FirstOrDefault();
                    if (LicenceUpdated != null)
                    {

                        LicenceUpdated.Support_Start_Date=Licence.Support_Start_Date;
                        LicenceUpdated.Support_Expiry_Date = Licence.Support_Expiry_Date;
                        LicenceUpdated.UpdateUser = UserId;
                        LicenceUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage UpdateLicenceLabaik(Licences Licence)
        {
            try
            {

                if (Licence.G_UID == null)
                {

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
                else
                {
                    var LicenceUpdated = _TaamerProContext.Licences.Where(x => x.G_UID == Licence.G_UID).FirstOrDefault();
                    if (LicenceUpdated != null)
                    {
                        LicenceUpdated.LicenceContractNo = EncryptValue(Licence.LicenceContractNo);
                        LicenceUpdated.NoOfUsers = EncryptValue(Licence.NoOfUsers);
                        LicenceUpdated.Type = Licence.Type;
                        if (Licence.Support_Expiry_Date != "بدون")
                        {
                            LicenceUpdated.Support_Expiry_Date = EncryptValue(Licence.Support_Expiry_Date);
                        }
                        LicenceUpdated.Email3 = Licence.Email3;
                        LicenceUpdated.Mobile = Licence.Mobile;
                        LicenceUpdated.Email2 = Licence.Email2;
                        LicenceUpdated.Mobile2 = Licence.Mobile2;
                        LicenceUpdated.Cost = Licence.Cost;
                        LicenceUpdated.Tax = Licence.Tax;
                        LicenceUpdated.TotalCost = Licence.TotalCost;
                        LicenceUpdated.ServerStorage = Licence.ServerStorage;

                        LicenceUpdated.Support_Start_Date = Licence.Support_Start_Date;
                        
                        if (Licence.Hosting_Expiry_Date == null || Licence.Hosting_Expiry_Date == "بدون")
                        {
                            LicenceUpdated.Hosting_Expiry_Date = null;
                        }
                        else
                        {
                            LicenceUpdated.Hosting_Expiry_Date = EncryptValue(Licence.Hosting_Expiry_Date);
                        }

                        LicenceUpdated.UpdateDate = DateTime.Now;
                        
                    }
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveLicenceAlerts(Licences Licence, int UserId, int BranchId)
        {
            try
            {
                var LicenceUpdated = _TaamerProContext.Licences.Where(x => x.LicenceId == Licence.LicenceId).FirstOrDefault();
                if (LicenceUpdated != null)
                {

                    DateTime Lic_Hosting_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_TechSuppCon_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_Domain_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_Other_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_Hosting_NextTime_Cust_NextDate = DateTime.Now;
                    DateTime Lic_TechSuppCon_NextTime_Cust_NextDate = DateTime.Now;
                    DateTime Lic_Domain_NextTime_Cust_NextDate = DateTime.Now;
                    DateTime Lic_Other_NextTime_Cust_NextDate = DateTime.Now;

                    string Lic_Hosting_NextTime_Info_ActionDate = "";
                    string Lic_TechSuppCon_NextTime_Info_ActionDate = "";
                    string Lic_Domain_NextTime_Info_ActionDate = "";
                    string Lic_Other_NextTime_Info_ActionDate = "";
                    string Lic_Hosting_NextTime_Cust_ActionDate = "";
                    string Lic_TechSuppCon_NextTime_Cust_ActionDate = "";
                    string Lic_Domain_NextTime_Cust_ActionDate = "";
                    string Lic_Other_NextTime_Cust_ActionDate = "";

                    if (Licence.Lic_HostingTimeType_Info == 1)
                    { Lic_Hosting_NextTime_Info_NextDate = Lic_Hosting_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_HostingTimeType_Info == 2)
                    { Lic_Hosting_NextTime_Info_NextDate = Lic_Hosting_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_Hosting_NextTime_Info_NextDate = Lic_Hosting_NextTime_Info_NextDate.AddDays(30); }
                    Lic_Hosting_NextTime_Info_ActionDate = Lic_Hosting_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Licence.Lic_TechSuppConTimeType_Info == 1)
                    { Lic_TechSuppCon_NextTime_Info_NextDate = Lic_TechSuppCon_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_TechSuppConTimeType_Info == 2)
                    { Lic_TechSuppCon_NextTime_Info_NextDate = Lic_TechSuppCon_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_TechSuppCon_NextTime_Info_NextDate = Lic_TechSuppCon_NextTime_Info_NextDate.AddDays(30); }
                    Lic_TechSuppCon_NextTime_Info_ActionDate = Lic_TechSuppCon_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Licence.Lic_DomainTimeType_Info == 1)
                    { Lic_Domain_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_DomainTimeType_Info == 2)
                    { Lic_Domain_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_Domain_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(30); }
                    Lic_Domain_NextTime_Info_ActionDate = Lic_Domain_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_OtherTimeType_Info == 1)
                    { Lic_Other_NextTime_Info_NextDate = Lic_Other_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_OtherTimeType_Info == 2)
                    { Lic_Other_NextTime_Info_NextDate = Lic_Other_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_Other_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(30); }
                    Lic_Other_NextTime_Info_ActionDate = Lic_Other_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_Hosting_TimeType_Cust == 1)
                    { Lic_Hosting_NextTime_Cust_NextDate = Lic_Hosting_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_Hosting_TimeType_Cust == 2)
                    { Lic_Hosting_NextTime_Cust_NextDate = Lic_Hosting_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_Hosting_NextTime_Cust_NextDate = Lic_Hosting_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_Hosting_NextTime_Cust_ActionDate = Lic_Hosting_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Licence.Lic_TechSuppConTimeType_Cust == 1)
                    { Lic_TechSuppCon_NextTime_Cust_NextDate = Lic_TechSuppCon_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_TechSuppConTimeType_Cust == 2)
                    { Lic_TechSuppCon_NextTime_Cust_NextDate = Lic_TechSuppCon_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_TechSuppCon_NextTime_Cust_NextDate = Lic_TechSuppCon_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_TechSuppCon_NextTime_Cust_ActionDate = Lic_TechSuppCon_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_DomainTimeType_Cust == 1)
                    { Lic_Domain_NextTime_Cust_NextDate = Lic_Domain_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_DomainTimeType_Cust == 2)
                    { Lic_Domain_NextTime_Cust_NextDate = Lic_Domain_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_Domain_NextTime_Cust_NextDate = Lic_Domain_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_Domain_NextTime_Cust_ActionDate = Lic_Domain_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_OtherTimeType_Cust == 1)
                    { Lic_Other_NextTime_Cust_NextDate = Lic_Other_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_OtherTimeType_Cust == 2)
                    { Lic_Other_NextTime_Cust_NextDate = Lic_Other_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_Other_NextTime_Cust_NextDate = Lic_Other_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_Other_NextTime_Cust_ActionDate = Lic_Other_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    LicenceUpdated.Lic_Hosting_Info = Licence.Lic_Hosting_Info;
                    LicenceUpdated.Lic_HostingCheck_Info = Licence.Lic_HostingCheck_Info;
                    LicenceUpdated.Lic_HostingTimeType_Info = Licence.Lic_HostingTimeType_Info ?? 3;
                    LicenceUpdated.Lic_Hosting_NextTime_Info = Lic_Hosting_NextTime_Info_ActionDate;
                    LicenceUpdated.Lic_Hosting_IsSend_Info = 0;

                    LicenceUpdated.Lic_TechSuppCon_Info = Licence.Lic_TechSuppCon_Info;
                    LicenceUpdated.Lic_TechSuppConCheck_Info = Licence.Lic_TechSuppConCheck_Info;
                    LicenceUpdated.Lic_TechSuppConTimeType_Info = Licence.Lic_TechSuppConTimeType_Info ?? 3;
                    LicenceUpdated.Lic_TechSuppCon_NextTime_Info = Lic_TechSuppCon_NextTime_Info_ActionDate;
                    LicenceUpdated.Lic_TechSuppCon_IsSend_Info = 0;

                    LicenceUpdated.Lic_Domain_Info = Licence.Lic_Domain_Info;
                    LicenceUpdated.Lic_DomainCheck_Info = Licence.Lic_DomainCheck_Info;
                    LicenceUpdated.Lic_DomainTimeType_Info = Licence.Lic_DomainTimeType_Info ?? 3;
                    LicenceUpdated.Lic_Domain_NextTime_Info = Lic_Domain_NextTime_Info_ActionDate;
                    LicenceUpdated.Lic_Domain_IsSend_Info = 0;

                    LicenceUpdated.Lic_Other_Info = Licence.Lic_Other_Info;
                    LicenceUpdated.Lic_OtherCheck_Info = Licence.Lic_OtherCheck_Info;
                    LicenceUpdated.Lic_OtherTimeType_Info = Licence.Lic_OtherTimeType_Info ?? 3;
                    LicenceUpdated.Lic_Other_NextTime_Info = Lic_Other_NextTime_Info_ActionDate;
                    LicenceUpdated.Lic_Other_IsSend_Info = 0;

                    LicenceUpdated.Lic_Hosting_Cust = Licence.Lic_Hosting_Cust;
                    LicenceUpdated.Lic_HostingCheck_Cust = Licence.Lic_HostingCheck_Cust;
                    LicenceUpdated.Lic_Hosting_TimeType_Cust = Licence.Lic_Hosting_TimeType_Cust ?? 3;
                    LicenceUpdated.Lic_Hosting_NextTime_Cust = Lic_Hosting_NextTime_Cust_ActionDate;
                    LicenceUpdated.Lic_Hosting_IsSend_Cust = 0;

                    LicenceUpdated.Lic_TechSuppCon_Cust = Licence.Lic_TechSuppCon_Cust;
                    LicenceUpdated.Lic_TechSuppConCheck_Cust = Licence.Lic_TechSuppConCheck_Cust;
                    LicenceUpdated.Lic_TechSuppConTimeType_Cust = Licence.Lic_TechSuppConTimeType_Cust ?? 3;
                    LicenceUpdated.Lic_TechSuppCon_NextTime_Cust = Lic_TechSuppCon_NextTime_Cust_ActionDate;
                    LicenceUpdated.Lic_TechSuppCon_IsSend_Cust = 0;

                    LicenceUpdated.Lic_Domain_Cust = Licence.Lic_Domain_Cust;
                    LicenceUpdated.Lic_DomainCheck_Cust = Licence.Lic_DomainCheck_Cust;
                    LicenceUpdated.Lic_DomainTimeType_Cust = Licence.Lic_DomainTimeType_Cust ?? 3;
                    LicenceUpdated.Lic_Domain_NextTime_Cust = Lic_Domain_NextTime_Cust_ActionDate;
                    LicenceUpdated.Lic_Domain_IsSend_Cust = 0;

                    LicenceUpdated.Lic_Other_Cust = Licence.Lic_Other_Cust;
                    LicenceUpdated.Lic_OtherCheck_Cust = Licence.Lic_OtherCheck_Cust;
                    LicenceUpdated.Lic_OtherTimeType_Cust = Licence.Lic_OtherTimeType_Cust ?? 3;
                    LicenceUpdated.Lic_Other_NextTime_Cust = Lic_Other_NextTime_Cust_ActionDate;
                    LicenceUpdated.Lic_Other_IsSend_Cust = 0;

                    LicenceUpdated.Lic_Note_Info = Licence.Lic_Note_Info;
                    LicenceUpdated.Lic_Note_Cust = Licence.Lic_Note_Cust;

                }
                _TaamerProContext.SaveChanges();
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم حفظ التنبيهات بنجاح" };

            }
            catch (Exception ex)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadGateway, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveLicenceAlertsBtn(Licences Licence, int UserId, int BranchId)
        {
            try
            {
                var LicenceUpdated = _TaamerProContext.Licences.Where(x => x.LicenceId == Licence.LicenceId).FirstOrDefault();
                if (LicenceUpdated != null)
                {
                    var Type = Convert.ToInt32(Licence.Type);
                    DateTime Lic_Hosting_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_TechSuppCon_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_Domain_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_Other_NextTime_Info_NextDate = DateTime.Now;
                    DateTime Lic_Hosting_NextTime_Cust_NextDate = DateTime.Now;
                    DateTime Lic_TechSuppCon_NextTime_Cust_NextDate = DateTime.Now;
                    DateTime Lic_Domain_NextTime_Cust_NextDate = DateTime.Now;
                    DateTime Lic_Other_NextTime_Cust_NextDate = DateTime.Now;

                    string Lic_Hosting_NextTime_Info_ActionDate = "";
                    string Lic_TechSuppCon_NextTime_Info_ActionDate = "";
                    string Lic_Domain_NextTime_Info_ActionDate = "";
                    string Lic_Other_NextTime_Info_ActionDate = "";
                    string Lic_Hosting_NextTime_Cust_ActionDate = "";
                    string Lic_TechSuppCon_NextTime_Cust_ActionDate = "";
                    string Lic_Domain_NextTime_Cust_ActionDate = "";
                    string Lic_Other_NextTime_Cust_ActionDate = "";

                    if (Licence.Lic_HostingTimeType_Info == 1)
                    { Lic_Hosting_NextTime_Info_NextDate = Lic_Hosting_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_HostingTimeType_Info == 2)
                    { Lic_Hosting_NextTime_Info_NextDate = Lic_Hosting_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_Hosting_NextTime_Info_NextDate = Lic_Hosting_NextTime_Info_NextDate.AddDays(30); }
                    Lic_Hosting_NextTime_Info_ActionDate = Lic_Hosting_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Licence.Lic_TechSuppConTimeType_Info == 1)
                    { Lic_TechSuppCon_NextTime_Info_NextDate = Lic_TechSuppCon_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_TechSuppConTimeType_Info == 2)
                    { Lic_TechSuppCon_NextTime_Info_NextDate = Lic_TechSuppCon_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_TechSuppCon_NextTime_Info_NextDate = Lic_TechSuppCon_NextTime_Info_NextDate.AddDays(30); }
                    Lic_TechSuppCon_NextTime_Info_ActionDate = Lic_TechSuppCon_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Licence.Lic_DomainTimeType_Info == 1)
                    { Lic_Domain_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_DomainTimeType_Info == 2)
                    { Lic_Domain_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_Domain_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(30); }
                    Lic_Domain_NextTime_Info_ActionDate = Lic_Domain_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_OtherTimeType_Info == 1)
                    { Lic_Other_NextTime_Info_NextDate = Lic_Other_NextTime_Info_NextDate.AddDays(2); }
                    else if (Licence.Lic_OtherTimeType_Info == 2)
                    { Lic_Other_NextTime_Info_NextDate = Lic_Other_NextTime_Info_NextDate.AddDays(7); }
                    else
                    { Lic_Other_NextTime_Info_NextDate = Lic_Domain_NextTime_Info_NextDate.AddDays(30); }
                    Lic_Other_NextTime_Info_ActionDate = Lic_Other_NextTime_Info_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_Hosting_TimeType_Cust == 1)
                    { Lic_Hosting_NextTime_Cust_NextDate = Lic_Hosting_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_Hosting_TimeType_Cust == 2)
                    { Lic_Hosting_NextTime_Cust_NextDate = Lic_Hosting_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_Hosting_NextTime_Cust_NextDate = Lic_Hosting_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_Hosting_NextTime_Cust_ActionDate = Lic_Hosting_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Licence.Lic_TechSuppConTimeType_Cust == 1)
                    { Lic_TechSuppCon_NextTime_Cust_NextDate = Lic_TechSuppCon_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_TechSuppConTimeType_Cust == 2)
                    { Lic_TechSuppCon_NextTime_Cust_NextDate = Lic_TechSuppCon_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_TechSuppCon_NextTime_Cust_NextDate = Lic_TechSuppCon_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_TechSuppCon_NextTime_Cust_ActionDate = Lic_TechSuppCon_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_DomainTimeType_Cust == 1)
                    { Lic_Domain_NextTime_Cust_NextDate = Lic_Domain_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_DomainTimeType_Cust == 2)
                    { Lic_Domain_NextTime_Cust_NextDate = Lic_Domain_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_Domain_NextTime_Cust_NextDate = Lic_Domain_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_Domain_NextTime_Cust_ActionDate = Lic_Domain_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));


                    if (Licence.Lic_OtherTimeType_Cust == 1)
                    { Lic_Other_NextTime_Cust_NextDate = Lic_Other_NextTime_Cust_NextDate.AddDays(2); }
                    else if (Licence.Lic_OtherTimeType_Cust == 2)
                    { Lic_Other_NextTime_Cust_NextDate = Lic_Other_NextTime_Cust_NextDate.AddDays(7); }
                    else
                    { Lic_Other_NextTime_Cust_NextDate = Lic_Other_NextTime_Cust_NextDate.AddDays(30); }
                    Lic_Other_NextTime_Cust_ActionDate = Lic_Other_NextTime_Cust_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (Type == 1)
                    {
                        LicenceUpdated.Lic_Hosting_Info = Licence.Lic_Hosting_Info;
                        LicenceUpdated.Lic_HostingCheck_Info = Licence.Lic_HostingCheck_Info;
                        LicenceUpdated.Lic_HostingTimeType_Info = Licence.Lic_HostingTimeType_Info ?? 3;
                        LicenceUpdated.Lic_Hosting_NextTime_Info = Lic_Hosting_NextTime_Info_ActionDate;
                        LicenceUpdated.Lic_Hosting_IsSend_Info = 0;
                    }
                    else if (Type == 2)
                    {
                        LicenceUpdated.Lic_TechSuppCon_Info = Licence.Lic_TechSuppCon_Info;
                        LicenceUpdated.Lic_TechSuppConCheck_Info = Licence.Lic_TechSuppConCheck_Info;
                        LicenceUpdated.Lic_TechSuppConTimeType_Info = Licence.Lic_TechSuppConTimeType_Info ?? 3;
                        LicenceUpdated.Lic_TechSuppCon_NextTime_Info = Lic_TechSuppCon_NextTime_Info_ActionDate;
                        LicenceUpdated.Lic_TechSuppCon_IsSend_Info = 0;
                    }
                    else if (Type == 3)
                    {
                        LicenceUpdated.Lic_Domain_Info = Licence.Lic_Domain_Info;
                        LicenceUpdated.Lic_DomainCheck_Info = Licence.Lic_DomainCheck_Info;
                        LicenceUpdated.Lic_DomainTimeType_Info = Licence.Lic_DomainTimeType_Info ?? 3;
                        LicenceUpdated.Lic_Domain_NextTime_Info = Lic_Domain_NextTime_Info_ActionDate;
                        LicenceUpdated.Lic_Domain_IsSend_Info = 0;
                    }
                    else if (Type == 4)
                    {
                        LicenceUpdated.Lic_Other_Info = Licence.Lic_Other_Info;
                        LicenceUpdated.Lic_OtherCheck_Info = Licence.Lic_OtherCheck_Info;
                        LicenceUpdated.Lic_OtherTimeType_Info = Licence.Lic_OtherTimeType_Info ?? 3;
                        LicenceUpdated.Lic_Other_NextTime_Info = Lic_Other_NextTime_Info_ActionDate;
                        LicenceUpdated.Lic_Other_IsSend_Info = 0;
                    }
                    else if (Type == 5)
                    {
                        LicenceUpdated.Lic_Hosting_Cust = Licence.Lic_Hosting_Cust;
                        LicenceUpdated.Lic_HostingCheck_Cust = Licence.Lic_HostingCheck_Cust;
                        LicenceUpdated.Lic_Hosting_TimeType_Cust = Licence.Lic_Hosting_TimeType_Cust ?? 3;
                        LicenceUpdated.Lic_Hosting_NextTime_Cust = Lic_Hosting_NextTime_Cust_ActionDate;
                        LicenceUpdated.Lic_Hosting_IsSend_Cust = 0;
                    }
                    else if (Type == 6)
                    {
                        LicenceUpdated.Lic_TechSuppCon_Cust = Licence.Lic_TechSuppCon_Cust;
                        LicenceUpdated.Lic_TechSuppConCheck_Cust = Licence.Lic_TechSuppConCheck_Cust;
                        LicenceUpdated.Lic_TechSuppConTimeType_Cust = Licence.Lic_TechSuppConTimeType_Cust ?? 3;
                        LicenceUpdated.Lic_TechSuppCon_NextTime_Cust = Lic_TechSuppCon_NextTime_Cust_ActionDate;
                        LicenceUpdated.Lic_TechSuppCon_IsSend_Cust = 0;
                    }
                    else if (Type == 7)
                    {
                        LicenceUpdated.Lic_Domain_Cust = Licence.Lic_Domain_Cust;
                        LicenceUpdated.Lic_DomainCheck_Cust = Licence.Lic_DomainCheck_Cust;
                        LicenceUpdated.Lic_DomainTimeType_Cust = Licence.Lic_DomainTimeType_Cust ?? 3;
                        LicenceUpdated.Lic_Domain_NextTime_Cust = Lic_Domain_NextTime_Cust_ActionDate;
                        LicenceUpdated.Lic_Domain_IsSend_Cust = 0;
                    }
                    else if (Type == 8)
                    {
                        LicenceUpdated.Lic_Other_Cust = Licence.Lic_Other_Cust;
                        LicenceUpdated.Lic_OtherCheck_Cust = Licence.Lic_OtherCheck_Cust;
                        LicenceUpdated.Lic_OtherTimeType_Cust = Licence.Lic_OtherTimeType_Cust ?? 3;
                        LicenceUpdated.Lic_Other_NextTime_Cust = Lic_Other_NextTime_Cust_ActionDate;
                        LicenceUpdated.Lic_Other_IsSend_Cust = 0;
                    }
                    else
                    {

                    }
                    LicenceUpdated.Lic_Note_Info = Licence.Lic_Note_Info;
                    LicenceUpdated.Lic_Note_Cust = Licence.Lic_Note_Cust;
                }
                _TaamerProContext.SaveChanges();
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage CheckLicenceG_UID(Licences Licence)
        {

            try
            {
                var licence = _TaamerProContext.Licences.FirstOrDefault(x => x.LicenceId == Licence.LicenceId);
                if(licence == null) 
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

                }else if(licence !=null && !string.IsNullOrEmpty(licence.G_UID))
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = licence.G_UID };
                }else if(licence !=null && string.IsNullOrEmpty(licence.G_UID))
                {
                    // Generate a new GUID
                    Guid newGuid = Guid.NewGuid();

                    // Convert GUID to string for display
                    string guid = newGuid.ToString();
                    licence.G_UID = guid.ToString();
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = licence.G_UID };
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }

        }


            private string EncryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Encoding.UTF8.GetBytes(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateEncryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }
        private string DecryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

    }
}
