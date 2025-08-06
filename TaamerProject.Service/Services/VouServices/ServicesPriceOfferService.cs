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
    public class ServicesPriceOfferService :  IServicesPriceOfferService
    {
        private readonly IServicesPriceServiceOfferRepository _ServicesPriceOfferRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ServicesPriceOfferService(IServicesPriceServiceOfferRepository servicesPriceOfferRepository,
            ISys_SystemActionsRepository sys_SystemActionsRepository,
             TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ServicesPriceOfferRepository = servicesPriceOfferRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public GeneralMessage SaveService(Acc_Services_PriceOffer service, int UserId, int BranchId)
        {
            try
            {
                if (service.ServicesIdVou == 0)
                {
                    service.AddUser = UserId;
                    service.AddDate = DateTime.Now;

                    _TaamerProContext.Acc_Services_PriceOffer.Add(service);


                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة تفاصيل خدمة جديدة";
                    _SystemAction.SaveAction("SaveService", "ServicesPriceOfferService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    Acc_Services_PriceOffer? ServicesUpdated =   _TaamerProContext.Acc_Services_PriceOffer.Where(s => s.ServicesIdVou == service.ServicesIdVou).FirstOrDefault();
                    if (ServicesUpdated != null)
                    {
                        ServicesUpdated.OfferId = service.OfferId;
                        ServicesUpdated.ServicesId = service.ServicesId;
                        ServicesUpdated.ParentId = service.ParentId;
                        ServicesUpdated.SureService = service.SureService;
                        ServicesUpdated.UpdateUser = UserId;
                        ServicesUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل تفاصيل خدمة رقم " + service.ServicesId;
                    _SystemAction.SaveAction("SaveService", "ServicesPriceOfferService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception ex)
            { 
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تفاصيل خدمة";
                _SystemAction.SaveAction("SaveService", "ServicesPriceOfferService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteService(int? ServicesIdVou, int UserId, int BranchId)
        {
            try
            {
                Acc_Services_PriceOffer? ServicesUpdated = _TaamerProContext.Acc_Services_PriceOffer.Where(s => s.ServicesIdVou == ServicesIdVou).FirstOrDefault();

                if (ServicesUpdated != null)
                {
                    ServicesUpdated.DeleteUser = UserId;
                    ServicesUpdated.DeleteDate = DateTime.Now;
                    ServicesUpdated.IsDeleted = true;
                }
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف تفاصيل خدمة رقم " + ServicesIdVou;
                _SystemAction.SaveAction("DeleteService", "ServicesPriceOfferService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف تفاصيل خدمة رقم " + ServicesIdVou; ;
                _SystemAction.SaveAction("DeleteService", "ServicesPriceOfferService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }



        public Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentId(int? ParentId,int? OfferId)
        {
            return _ServicesPriceOfferRepository.GetServicesPriceByParentId(ParentId, OfferId);
        }
        public Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentIdAndContractId(int? ParentId, int? ContractId)
        {
            return _ServicesPriceOfferRepository.GetServicesPriceByParentIdAndContractId(ParentId, ContractId);
        }
        public Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceVouByParentIdAndInvoiceId(int? ParentId, int? InvoiceId)
        {
            return _ServicesPriceOfferRepository.GetServicesPriceVouByParentIdAndInvoiceId(ParentId, InvoiceId);
        }
        public Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByOfferId( int? OfferId)
        {
            return _ServicesPriceOfferRepository.GetServicesPriceByOfferId( OfferId);
        }


    }
}
