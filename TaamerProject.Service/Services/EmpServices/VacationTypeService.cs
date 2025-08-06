using Microsoft.EntityFrameworkCore;
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
    public class VacationTypeService :  IVacationTypeService
    {
        private readonly IVacationTypeRepository _VacationTypeRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public VacationTypeService(IVacationTypeRepository vacationTypeRepository, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _VacationTypeRepository = vacationTypeRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task< IEnumerable<VacationTypeVM>> GetAllVacationsTypes(string SearchText)
        {
            var VacationsTypes = _VacationTypeRepository.GetAllVacationsTypes(SearchText);
            return VacationsTypes;
        }
        public  GeneralMessage SaveVacationType(VacationType vacationType, int UserId, int BranchId)
        {
            try
            {
                if (vacationType.VacationTypeId == 0)
                {
                    vacationType.AddUser = UserId;
                    vacationType.AddDate = DateTime.Now;
                    vacationType.Code = ( _TaamerProContext.VacationType.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.VacationType.Add(vacationType);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع إجازة جديد";
                    _SystemAction.SaveAction("SaveVacationType", "VacationTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                else
                {
                     //var VacationTypeUpdated = _VacationTypeRepository.GetById(vacationType.VacationTypeId);
                    VacationType? VacationTypeUpdated =  _TaamerProContext.VacationType.Where(s => s.VacationTypeId == vacationType.VacationTypeId).FirstOrDefault();
                    if (VacationTypeUpdated != null)
                    {
                        VacationTypeUpdated.Code = vacationType.Code;
                        VacationTypeUpdated.NameAr = vacationType.NameAr;
                        VacationTypeUpdated.NameEn = vacationType.NameEn;
                        VacationTypeUpdated.Notes = vacationType.Notes;
                        VacationTypeUpdated.UpdateUser = UserId;
                        VacationTypeUpdated.UpdateDate = DateTime.Now;
                    }

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع إجازة رقم " + vacationType.VacationTypeId;
                    _SystemAction.SaveAction("SaveVacationType", "VacationTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع إجازة";
                _SystemAction.SaveAction("SaveVacationType", "VacationTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteVacationType(int VacationTypeId, int UserId, int BranchId)
        {
            try
            {
               // VacationType vacationType = _VacationTypeRepository.GetById(VacationTypeId);
                VacationType? vacationType =   _TaamerProContext.VacationType.Where(s => s.VacationTypeId == VacationTypeId).FirstOrDefault();
                if (vacationType != null)
                {
                    vacationType.IsDeleted = true;
                    vacationType.DeleteDate = DateTime.Now;
                    vacationType.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع إجازة رقم " + VacationTypeId;
                    _SystemAction.SaveAction("DeleteVacationType", "VacationTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في نوع إجازة رقم " + VacationTypeId; ;
                _SystemAction.SaveAction("DeleteVacationType", "VacationTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<VacationTypeVM>> FillVacationTypeSelect(string SearchText = "")
        {
            var VacationsTypes = _VacationTypeRepository.GetAllVacationsTypes(SearchText);
            return VacationsTypes;
        }

 
    }
}
