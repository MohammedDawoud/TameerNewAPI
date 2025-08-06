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
using TaamerProject.Service.Generic;
using Twilio.TwiML.Voice;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Pro_SuperContractorService :   IPro_SuperContractorService
    {
        private readonly IPro_SuperContractorRepository _Pro_SuperContractorRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public Pro_SuperContractorService(IPro_SuperContractorRepository Pro_SuperContractorRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _Pro_SuperContractorRepository = Pro_SuperContractorRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<Pro_SuperContractorVM>> GetAllSuperContractor(string SearchText)
        {
            var Contractors = _Pro_SuperContractorRepository.GetAllSuperContractors(SearchText);
            return Contractors;
        }
        public Task<Pro_SuperContractorVM> GetContractorData(int? ContractorId, int UserID, int BranchId)
        {
            var ContractorsData = _Pro_SuperContractorRepository.GetContractorData(ContractorId, UserID, BranchId);
            return ContractorsData;
        }
        public GeneralMessage SaveSuperContractor(Pro_SuperContractor Contractor, int UserId, int BranchId)
        {
            try
            {

                if (Contractor.ContractorId == 0)
                {


                    Contractor.AddUser = UserId;
                    Contractor.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_SuperContractor.Add(Contractor);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مقاول جديد";
                    _SystemAction.SaveAction("SaveSuperContractor", "Pro_SuperContractorService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ContractorUpdated = _Pro_SuperContractorRepository.GetById(Contractor.ContractorId);
                    Pro_SuperContractor? ContractorUpdated = _TaamerProContext.Pro_SuperContractor.Where(s => s.ContractorId == Contractor.ContractorId).FirstOrDefault();
                   
                    if (ContractorUpdated != null)
                    {


                        ContractorUpdated.NameAr = Contractor.NameAr;
                        ContractorUpdated.NameEn = Contractor.NameEn;
                        ContractorUpdated.Email = Contractor.Email;
                        ContractorUpdated.CommercialRegister = Contractor.CommercialRegister;
                        ContractorUpdated.PhoneNo = Contractor.PhoneNo;
                        ContractorUpdated.UpdateUser = UserId;
                        ContractorUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مقاول رقم " + Contractor.ContractorId;
                    _SystemAction.SaveAction("SaveSuperContractor", "Pro_SuperContractorService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المقاول";
                _SystemAction.SaveAction("SaveSuperContractor", "Pro_SuperContractorService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteSuperContractor(int ContractorId, int UserId, int BranchId)
        {
            try
            {


                // Pro_SuperContractor Clause = _Pro_SuperContractorRepository.GetById(ContractorId);
                Pro_SuperContractor? Clause = _TaamerProContext.Pro_SuperContractor.Where(s => s.ContractorId == ContractorId).FirstOrDefault();
                if (Clause != null)
                {
                    Clause.IsDeleted = true;
                    Clause.DeleteDate = DateTime.Now;
                    Clause.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مقاول رقم " + ContractorId;
                    _SystemAction.SaveAction("DeleteSuperContractor", "Pro_SuperContractorService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مقاول رقم " + ContractorId; ;
                _SystemAction.SaveAction("DeleteSuperContractor", "Pro_SuperContractorService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }



       


    }
}
