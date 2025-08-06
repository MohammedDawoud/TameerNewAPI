using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ServicesPriceService :  IServicesPriceService
    {
        private readonly IServicesPriceServiceRepository _ServicesPriceService;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ServicesPriceService(IServicesPriceServiceRepository servicesPriceService,
            ISys_SystemActionsRepository sys_SystemActionsRepository,
             TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ServicesPriceService = servicesPriceService;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public GeneralMessage SaveService(Acc_Services_Price service, int UserId, int BranchId, List<Acc_Services_Price>? Details)
        {
            try
            {
                if (service.ServicesId == 0)
                {
                    service.AddUser = UserId;
                    service.AddDate = DateTime.Now;
                    service.Amount = service.Amount??0;


                    //Check if there is the same detail object
                    //var same = _ServicesPriceService.GetMatching(x => x.IsDeleted==false && x.ServicesName == service.ServicesName && x.Amount == service.Amount && x.ParentId == service.ParentId ).FirstOrDefault();
                    var same = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && x.ServicesName == service.ServicesName && x.Amount == service.Amount && x.ParentId == service.ParentId).FirstOrDefault();

                    if (same != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote1 = "فشل في حفظ تكلفة خدمة";
                        _SystemAction.SaveAction("SaveService", "ServicesPriceService", 1, "تم إضافتها من قبل", "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                    }

                    _TaamerProContext.Acc_Services_Price.Add(service);


                    _TaamerProContext.SaveChanges();

                    if (Details != null)
                    {
                        foreach (var item in Details)
                        {
                            item.ServicesId = 0;
                            item.ParentId = service.ServicesId;
                            _TaamerProContext.Acc_Services_Price.Add(item);
                        }
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة تكلفة خدمة جديدة" + service.ServicesName;
                    _SystemAction.SaveAction("SaveService", "ServicesPriceService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ServicesUpdated = _ServicesPriceService.GetById(service.ServicesId);
                    Acc_Services_Price? ServicesUpdated =   _TaamerProContext.Acc_Services_Price.Where(s => s.ServicesId == service.ServicesId).FirstOrDefault();
                    if (ServicesUpdated != null)
                    {
                        ServicesUpdated.AccountId = service.AccountId;
                        ServicesUpdated.AccountName = service.AccountName;
                        ServicesUpdated.Amount = service.Amount??0;
                        ServicesUpdated.ServicesName = service.ServicesName;
                        ServicesUpdated.ProjectId = service.ProjectId;
                        ServicesUpdated.ProjectSubTypeID = service.ProjectSubTypeID;
                        ServicesUpdated.UpdateUser = UserId;
                        ServicesUpdated.UpdateDate = DateTime.Now;
                        ServicesUpdated.ParentId = service.ParentId;
                        ServicesUpdated.CostCenterId = service.CostCenterId;
                        ServicesUpdated.PackageId = service.PackageId;
                        ServicesUpdated.ServiceName_EN = service.ServiceName_EN;
                        ServicesUpdated.ServiceType = service.ServiceType;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل تكلفة خدمة  " + service.ServicesName;
                    _SystemAction.SaveAction("SaveService", "ServicesPriceService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            { 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تكلفة خدمة";
                _SystemAction.SaveAction("SaveService", "ServicesPriceService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteService(int? ServicesId, int UserId, int BranchId)
        {
            try
            {
                /*****children first******/

                //var ChildServicesUpdated = _ServicesPriceService.GetMatching(x => x.ParentId == ServicesId).ToList();
                var ChildServicesUpdated = _TaamerProContext.Acc_Services_Price.Where(x => x.ParentId == ServicesId).ToList();

                foreach (var child in ChildServicesUpdated)
                {
                    child.DeleteUser = UserId;
                    child.DeleteDate = DateTime.Now;
                    child.IsDeleted = true;
                }

                //var ServicesUpdated = _ServicesPriceService.GetById(ServicesId);
                Acc_Services_Price? ServicesUpdated = _TaamerProContext.Acc_Services_Price.Where(s => s.ServicesId == ServicesId).FirstOrDefault();

                if (ServicesUpdated != null)
                {
                    ServicesUpdated.DeleteUser = UserId;
                    ServicesUpdated.DeleteDate = DateTime.Now;
                    ServicesUpdated.IsDeleted = true;
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف تكلفة خدمة  " + ServicesUpdated.ServicesName;
                _SystemAction.SaveAction("DeleteService", "ServicesPriceService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في تكلفة خدمة رقم " + ServicesId; ;
                _SystemAction.SaveAction("DeleteService", "ServicesPriceService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public Task<IEnumerable<AccServicesPricesVM>> GetAllServicesPrice()
        {
            return _ServicesPriceService.GetAllServicesPrice();
        }

        public Task<AccServicesPricesVM> GetServicesPriceByServiceId(int ServiceId)
        {
            return _ServicesPriceService.GetServicesPriceByServiceId(ServiceId);
        }

        public Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId(int? ProjectId)
        {
            return _ServicesPriceService.GetServicesPriceByProjectId(ProjectId);
        }
        public Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId2(int? ProjectId, int? ProjectId2)
        {
            return _ServicesPriceService.GetServicesPriceByProjectId2(ProjectId, ProjectId2);
        }
        public Task<IEnumerable<AccServicesPricesVM>> GetServicePriceByProject_Search(int? Project1, int? Project2, string ServiceName, string ServiceDetailName, decimal? Amount) {
            return _ServicesPriceService.GetServicePriceByProject_Search(Project1, Project2, ServiceName, ServiceDetailName, Amount);
        }
        public Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByParentId(int? ParentId)
        {
            return _ServicesPriceService.GetServicesPriceByParentId(ParentId);
        }
        public Task<decimal?> GetServicesPriceAmountByServicesId(int? ServicesId)
        {
            return _ServicesPriceService.GetServicesPriceAmountByServicesId(ServicesId);
        }

 
    }
}
