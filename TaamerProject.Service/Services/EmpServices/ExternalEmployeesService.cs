
using System.Globalization;
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
    public class ExternalEmployeesService :   IExternalEmployeesService
    {
        private readonly IExternalEmployeesRepository _ExternalEmployeesRepository;
         private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ExternalEmployeesService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IExternalEmployeesRepository ExternalEmployeesRepository)
        {
            _ExternalEmployeesRepository = ExternalEmployeesRepository;
             _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<ExternalEmployeesVM>> GetAllExternalEmployees(int? DepartmentId, string SearchText,int BranchId)
        {
            var ExternalEmployees = _ExternalEmployeesRepository.GetAllExternalEmployees(DepartmentId, SearchText, BranchId);
            return ExternalEmployees;
        }
        public GeneralMessage SaveExternalEmployees(ExternalEmployees externalEmployees, int UserId, int BranchId)
        {
            try
            {
                if (externalEmployees.EmpId == 0)
                {
                    externalEmployees.BranchId = BranchId;
                    externalEmployees.AddUser = UserId;
                    externalEmployees.AddDate = DateTime.Now;
                    _TaamerProContext.ExternalEmployees.Add(externalEmployees);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة موظف خارجي جديد";
                    _SystemAction.SaveAction("SaveExternalEmployees", "ExternalEmployeesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    // var ExternalEmployeesUpdated = _ExternalEmployeesRepository.GetById(externalEmployees.EmpId);
                    ExternalEmployees? ExternalEmployeesUpdated = _TaamerProContext.ExternalEmployees.Where(s => s.EmpId == externalEmployees.EmpId).FirstOrDefault();

                    if (ExternalEmployeesUpdated != null)
                    {
                        ExternalEmployeesUpdated.NameAr = externalEmployees.NameAr;
                        ExternalEmployeesUpdated.NameEn = externalEmployees.NameEn;
                        ExternalEmployeesUpdated.DepartmentId = externalEmployees.DepartmentId;
                        ExternalEmployeesUpdated.Description = externalEmployees.Description;
                        ExternalEmployeesUpdated.UpdateUser = UserId;
                        ExternalEmployeesUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل موظف خارجي رقم " + externalEmployees.EmpId;
                    _SystemAction.SaveAction("SaveExternalEmployees", "ExternalEmployeesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ موظف خارجي";
                _SystemAction.SaveAction("SaveExternalEmployees", "ExternalEmployeesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
            
        }
        public GeneralMessage DeleteExternalEmployees( int EmpId, int UserId,int BranchId)
        {
            try
            {
               // ExternalEmployees externalEmployees = _ExternalEmployeesRepository.GetById(EmpId);
                ExternalEmployees? externalEmployees = _TaamerProContext.ExternalEmployees.Where(s => s.EmpId == EmpId).FirstOrDefault();
                if (externalEmployees != null)
                {
                    externalEmployees.IsDeleted = true;
                    externalEmployees.DeleteDate = DateTime.Now;
                    externalEmployees.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف موظف خارجي رقم " + EmpId;
                    _SystemAction.SaveAction("DeleteExternalEmployees", "ExternalEmployeesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف موظف خارجي رقم " + EmpId; ;
                _SystemAction.SaveAction("DeleteExternalEmployees", "ExternalEmployeesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillExternalEmployeeSelect(int? DepartmentId, int BranchId)
        {
            return _ExternalEmployeesRepository.GetAllExternalEmployees(DepartmentId, "", BranchId).Result.Select(s => new
            {
                Id = s.EmpId,
                Name = s.NameAr
            });
        }
       

    }
}
