using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ServiceService :   IServiceService
    {
        private readonly IServicesRepository _ServicesRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ServiceService(IServicesRepository servicesRepository,
            ISys_SystemActionsRepository sys_SystemActionsRepository,
             TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ServicesRepository = servicesRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<ServicesVM>> GetAllServices(int BranchId)
        {
            var Services = _ServicesRepository.GetAllServices(BranchId);
            return Services;
        }
        public GeneralMessage SaveService(TaamerProject.Models.Service service, int UserId, int BranchId)
        {
            try
            {
                //var codeExist = _ServicesRepository.GetMatching(s => s.IsDeleted == false && s.ServiceId != service.ServiceId && s.Number == service.Number).FirstOrDefault();
                var codeExist = _TaamerProContext.Service.Where(s => s.IsDeleted == false && s.ServiceId != service.ServiceId && s.Number == service.Number).FirstOrDefault();
                if (codeExist != null)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                if (service.ServiceId == 0)
                {
                    service.AddUser = UserId;
                    service.BranchId = BranchId;
                    service.AddDate = DateTime.Now;
                    _TaamerProContext.Service.Add(service);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة خدمة جديدة";
                    _SystemAction.SaveAction("SaveService", "ServiceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ServicesUpdated = _ServicesRepository.GetById(service.ServiceId);
                    TaamerProject.Models.Service? ServicesUpdated = _TaamerProContext.Service.Where(s => s.ServiceId == service.ServiceId).FirstOrDefault();
                    if (ServicesUpdated != null)
                    {
                        ServicesUpdated.Number = service.Number;
                        ServicesUpdated.Date = service.Date;
                        ServicesUpdated.ExpireDate = service.ExpireDate;
                        ServicesUpdated.HijriDate = service.HijriDate;
                        ServicesUpdated.ExpireHijriDate = service.ExpireHijriDate;
                        ServicesUpdated.UserId = service.UserId;
                        ServicesUpdated.Notes = service.Notes;
                        if (service.AttachmentUrl != null)
                        {
                            ServicesUpdated.AttachmentUrl = service.AttachmentUrl;
                        }
                        ServicesUpdated.DepartmentId = service.DepartmentId;
                        ServicesUpdated.NotifyCount = service.NotifyCount;
                        ServicesUpdated.AccountId = service.AccountId;
                        ServicesUpdated.BankId = service.BankId;
                        ServicesUpdated.RepeatAlarm = service.RepeatAlarm;
                        ServicesUpdated.RecurrenceRateId = service.RecurrenceRateId;

                        ServicesUpdated.UpdateUser = UserId;
                        ServicesUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل الخدمة رقم " + service.ServiceId;
                    _SystemAction.SaveAction("SaveService", "ServiceService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الخدمة";
                _SystemAction.SaveAction("SaveService", "ServiceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteService(int ServiceId, int UserId,int BranchId)
        {
            try
            {
                // Service Service = _ServicesRepository.GetById(ServiceId);
                TaamerProject.Models.Service? Service =   _TaamerProContext.Service.Where(s => s.ServiceId == ServiceId).FirstOrDefault();
                if(Service!=null)
                {
                    Service.IsDeleted = true;
                    Service.DeleteDate = DateTime.Now;
                    Service.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف الخدمة رقم " + ServiceId;
                    _SystemAction.SaveAction("DeleteService", "ServiceService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الخدمة رقم " + ServiceId; ;
                _SystemAction.SaveAction("DeleteService", "ServiceService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public Task<IEnumerable<ServicesVM>> GetServicesSearch(ServicesVM ServiceSearch, int BranchId)
        {
            var Services = _ServicesRepository.GetServicesSearch(ServiceSearch, BranchId);
            return Services;
        }


        public IEnumerable<ServicesVM> GetServicesToNotified(int BranchId, string lang)
        {
            List<ServicesVM> offDocsList = new List<ServicesVM>();
            //var OfficialDocuments = _ServicesRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId && (DateTime.ParseExact(s.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.Now));
            var OfficialDocuments = _TaamerProContext.Service.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (DateTime.ParseExact(s.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.Now));

            foreach (var item in OfficialDocuments)
            {
               // var notifyCount = _ServicesRepository.GetById(item.ServiceId).NotifyCount;

                int notifyCount = _TaamerProContext.Service.Where(s => s.ServiceId == item.ServiceId).FirstOrDefault()?.NotifyCount??0;
               

                var diff = (DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.Now).Days;
                if (diff <= notifyCount)
                {
                    offDocsList.Add(new ServicesVM
                    {
                        DepartmentName = item.Department != null ? item.Department.DepartmentNameAr : "",
                        Number = item.Number,
                        ExpireDate = item.ExpireDate,
                        Notes = (String.IsNullOrWhiteSpace(item.Notes)) ? "Resources.MA_servicebill" : item.Notes,
                        Date = item.Date,
                        UserId = item.UserId,
                        ServiceId = item.ServiceId,
                        RepeatAlarm = item.RepeatAlarm,
                        RecurrenceRateId = item.RecurrenceRateId,

                    });
                }
            }
           // var repeatedOfficialDocuments = _ServicesRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId && s.RepeatAlarm == true && (DateTime.ParseExact(s.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now));
            var repeatedOfficialDocuments = _TaamerProContext.Service.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.RepeatAlarm == true && (DateTime.ParseExact(s.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now));

            foreach (var item in repeatedOfficialDocuments)
            {
                //var notifyCount = _ServicesRepository.GetById(item.ServiceId).NotifyCount;

                int notifyCount = _TaamerProContext.Service.Where(s => s.ServiceId == item.ServiceId).FirstOrDefault()?.NotifyCount??0;
       
                int? diffDays = null; int diffYears = 0; DateTime newComparisonDate = DateTime.Now;
                switch (item.RecurrenceRateId)
                {
                    case 1://1 month
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;

                        if (diffYears / 365 > 0)
                        {
                            //newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1).AddMonths(1);
                            newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1);
                        }
                        newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths(1);
                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }
                        else
                        {
                            for (int i = 0; i <= 11; i++)
                            {
                                newComparisonDate = newComparisonDate.AddMonths(1);
                                if (newComparisonDate > DateTime.Now)
                                {
                                    diffDays = (newComparisonDate - DateTime.Now).Days;
                                }
                                if (diffDays != null)
                                {
                                    i = 12;
                                }
                            }
                        }
                        break;
                    case 2://3 months
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;

                        if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1);
                        }
                        newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths(3);
                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }
                        else
                        {
                            for (int i = 0; i <= 3; i++)
                            {
                                newComparisonDate = newComparisonDate.AddMonths(3);
                                if (newComparisonDate > DateTime.Now)
                                {
                                    diffDays = (newComparisonDate - DateTime.Now).Days;
                                }
                                if (diffDays != null)
                                {
                                    i = 4;
                                }
                            }
                        }

                        break;
                    case 3://6 months
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;
                        if (diffYears / 365 == 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths((6));
                        }
                        else if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1).AddMonths(6);
                        }
                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }
                        else
                        {
                            newComparisonDate = newComparisonDate.AddMonths(6);
                            if (newComparisonDate > DateTime.Now)
                            {
                                diffDays = (newComparisonDate - DateTime.Now).Days;
                            }
                        }

                        break;
                    case 4://year
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;
                        if (diffYears / 365 == 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((1));
                        }
                        else if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365));
                        }

                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }

                        break;
                }
                if (diffDays != null && diffDays <= notifyCount)
                {
                    offDocsList.Add(new ServicesVM
                    {
                        DepartmentName = item.Department != null ? item.Department.DepartmentNameAr : "",
                        Number = item.Number,
                        ExpireDate = item.ExpireDate,
                        Notes = (String.IsNullOrWhiteSpace(item.Notes)) ? "Resources.MA_servicebill" : item.Notes,
                        Date = item.Date,
                        UserId = item.UserId,
                        ServiceId = item.ServiceId,
                        RepeatAlarm = item.RepeatAlarm,
                        RecurrenceRateId = item.RecurrenceRateId,

                    });
                }
            }

            return offDocsList;
        }
        public Task<int?> GenerateServicesNumber(int BranchId)
        {
            return _ServicesRepository.GenerateNextServicesNumber(BranchId);
        }

        public IEnumerable<rptGetDeservedServicesVM> GetDeservedServices(string Con)
        {
            try
            {
                List<rptGetDeservedServicesVM> lmd = new List<rptGetDeservedServicesVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetDeservedServices";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new rptGetDeservedServicesVM
                            {
                                Number = (dr[0]).ToString(),
                                AccCode = dr[2].ToString(),
                                Department = dr[3].ToString(),
                                ExpireDate = dr[4].ToString(),
                                Branch = dr[5].ToString(),


                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<rptGetDeservedServicesVM> lmd = new List<rptGetDeservedServicesVM>();
                return lmd;
            }


        }

       

    }
}