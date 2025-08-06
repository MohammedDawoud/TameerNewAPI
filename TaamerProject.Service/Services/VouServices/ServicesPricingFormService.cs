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
    public class ServicesPricingFormService :  IServicesPricingFormService
    {
        private readonly IServicesPricingFormRepository _ServicesPricingFormRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ServicesPricingFormService(IServicesPricingFormRepository servicesPricingFormRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ServicesPricingFormRepository = servicesPricingFormRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<ServicesPricingFormVM>> GetAllServicesPricingForms(string SearchText, int BranchId)
        {
            var Forms = _ServicesPricingFormRepository.GetAllServicesPricingForms(SearchText, BranchId);
            return Forms;
        }
        public Task<ServicesPricingFormVM> GetServicesPricingFormById(int FormId, int BranchId)
        {
            var Forms = _ServicesPricingFormRepository.GetServicesPricingFormById(FormId, BranchId);
            return Forms;
        }

        public Task<IEnumerable<ServicesPricingFormVM>> GetAllPublicanddesignServicesPricingForms(int Type, int BranchId)
        {
            var Forms = _ServicesPricingFormRepository.GetAllPublicanddesignServicesPricingForms(Type, BranchId);
            return Forms;
        }

        public int GetAllPublicanddesignServicesPricingFormstocount(int Type, int BranchId)
        {
            var Forms =  _ServicesPricingFormRepository.GetAllPublicanddesignServicesPricingFormstocount(Type, BranchId).Result.Count();
            return Forms;
        }


        public Task<IEnumerable<ServicesPricingFormVM>> FilterPublicanddesignServicesPricingForms(int Type, int BranchId, string date)
        {
           //// var dat = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString();

           // DateTime dt = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
           // var result = dt.ToString("yyyy-MM-dd");
            var Forms = _ServicesPricingFormRepository.FilterPublicanddesignServicesPricingForms(Type, BranchId, date);
            return Forms;
        }

        public GeneralMessage SaveServicesPricingForm(ServicesPricingForm Form, int UserId, int BranchId)
        {
            try
            {
                 

                if (Form.FormId == 0)
                {
                    Form.AddUser = UserId;
                    Form.AddDate = DateTime.Now;
                    Form.ServiceDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    _TaamerProContext.ServicesPricingForm.Add(Form);
                    _TaamerProContext.SaveChanges();
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully ,ReturnedParm=Form.FormId};
                }
                else
                {
                    //var FormUpdated = _ServicesPricingFormRepository.GetById(Form.FormId);
                    //if (FormUpdated != null)
                    //{

                    //    FormUpdated.NameAr = Clause.NameAr;
                    //    FormUpdated.NameEn = Clause.NameEn;
                    //    FormUpdated.UpdateUser = UserId;
                    //    FormUpdated.UpdatedDate = DateTime.Now;

                    //}
                    //_TaamerProContext.SaveChanges();
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully, ReturnedParm= Form.FormId };
                }

            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed, ReturnedParm=0 };
            }
        }
        public GeneralMessage UpdateURL(ServicesPricingForm Form, int UserId)
        {
            try
            {
                // var FormUpdated = _ServicesPricingFormRepository.GetById(Form.FormId);
                ServicesPricingForm? FormUpdated =   _TaamerProContext.ServicesPricingForm.Where(s => s.FormId == Form.FormId).FirstOrDefault();
                if (FormUpdated != null)
                {
                    FormUpdated.URLFile = Form.URLFile;                  
                }
                _TaamerProContext.SaveChanges();
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully, ReturnedParm = Form.FormId };

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed, ReturnedParm = 0 };
            }
        }

        public ServicesPricingForm SaveServicesPricingForm_Ret_DATA(ServicesPricingForm Form, int UserId, int BranchId)
        {
            try
            {

                if (Form.FormId == 0)
                {
                    Form.AddUser = UserId;
                    Form.AddDate = DateTime.Now;
                    _TaamerProContext.ServicesPricingForm.Add(Form);
                    _TaamerProContext.SaveChanges();
                    return Form;
                }
                else
                {
                    //var FormUpdated = _ServicesPricingFormRepository.GetById(Form.FormId);
                    //if (FormUpdated != null)
                    //{

                    //    FormUpdated.NameAr = Clause.NameAr;
                    //    FormUpdated.NameEn = Clause.NameEn;
                    //    FormUpdated.UpdateUser = UserId;
                    //    FormUpdated.UpdatedDate = DateTime.Now;

                    //}
                    //_TaamerProContext.SaveChanges();
                    return Form;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public GeneralMessage DeleteServicesPricingForm(int FormId, int UserId, int BranchId)
        {
            try
            {

              //  ServicesPricingForm Form = _ServicesPricingFormRepository.GetById(FormId);
                ServicesPricingForm? Form = _TaamerProContext.ServicesPricingForm.Where(s => s.FormId == FormId).FirstOrDefault();
                if (Form != null)
                {
                    Form.IsDeleted = true;
                    Form.DeleteDate = DateTime.Now;
                    Form.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                }
             

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }




        public GeneralMessage UpdateStatusServicesPricingForm( int FormId,bool status, int UserId)
        {
            try
            {


                if (FormId != 0)
                {
                    //var form = _ServicesPricingFormRepository.GetById(FormId);
                    ServicesPricingForm? form = _TaamerProContext.ServicesPricingForm.Where(s => s.FormId == FormId).FirstOrDefault();
                    if (form != null)
                    {
                        form.UpdateUser = UserId;
                        form.UpdateDate = DateTime.Now;
                        form.FormStatus = status;
                        _TaamerProContext.SaveChanges();
                    }
                   
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                }

            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
    }
}
