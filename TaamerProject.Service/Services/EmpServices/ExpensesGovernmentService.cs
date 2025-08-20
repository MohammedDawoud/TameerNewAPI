
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
    public class ExpensesGovernmentService :   IExpensesGovernmentService
    {
        private readonly IExpensesGovernmentRepository _ExpensesGovernmentRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ExpensesGovernmentService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IExpensesGovernmentRepository ExpensesGovernmentRepository,
            ISys_SystemActionsRepository Sys_SystemActionsRepository)
        {
            _ExpensesGovernmentRepository = ExpensesGovernmentRepository;
            _Sys_SystemActionsRepository = Sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<ExpensesGovernmentVM>> GetAllExpensesGovernment(int? EmpId, string SearchText)
        {
            var ExpensesGovernment = _ExpensesGovernmentRepository.GetAllExpensesGovernment(EmpId, SearchText);
            return ExpensesGovernment;
        }
        public GeneralMessage SaveExpensesGovernment(ExpensesGovernment expensesGovernment,int UserId, int BranchId)
        {
            try
            {
                if (expensesGovernment.ExpensesId == 0)
                {
                    expensesGovernment.AddUser = UserId;
                    expensesGovernment.AddDate = DateTime.Now;
                    _TaamerProContext.ExpensesGovernment.Add(expensesGovernment);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مصروف حكومي";
                    _SystemAction.SaveAction("SaveExpensesGovernment", "ExpensesGovernmentService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ExpensesGovernmentUpdated = _ExpensesGovernmentRepository.GetById(expensesGovernment.ExpensesId);
                    ExpensesGovernment? ExpensesGovernmentUpdated = _TaamerProContext.ExpensesGovernment.Where(s => s.ExpensesId == expensesGovernment.ExpensesId).FirstOrDefault();

                    if (ExpensesGovernmentUpdated != null)
                    {
                        ExpensesGovernmentUpdated.EmployeeId = expensesGovernment.EmployeeId;
                        ExpensesGovernmentUpdated.TypeId = expensesGovernment.TypeId;
                        ExpensesGovernmentUpdated.StartDate = expensesGovernment.StartDate;
                        ExpensesGovernmentUpdated.StartHijriDate = expensesGovernment.StartHijriDate;
                        ExpensesGovernmentUpdated.EndDate = expensesGovernment.EndDate;
                        ExpensesGovernmentUpdated.EndHijriDate = expensesGovernment.EndHijriDate;
                        ExpensesGovernmentUpdated.Notes = expensesGovernment.Notes;
                        ExpensesGovernmentUpdated.Amount = expensesGovernment.Amount;
                        ExpensesGovernmentUpdated.UserId = expensesGovernment.UserId;
                        ExpensesGovernmentUpdated.Year = expensesGovernment.Year;
                        ExpensesGovernmentUpdated.HijriYear = expensesGovernment.HijriYear;
                        ExpensesGovernmentUpdated.UpdateUser = UserId;
                        ExpensesGovernmentUpdated.UpdateDate = DateTime.Now;
                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مصروف حكومي رقم " + expensesGovernment.ExpensesId;
                    _SystemAction.SaveAction("SaveExpensesGovernment", "ExpensesGovernmentService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مصروف حكومي";
                _SystemAction.SaveAction("SaveExpensesGovernment", "ExpensesGovernmentService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteExpensesGovernment(int ExpensesId,int UserId,int BranchId)
        {
            try
            {
                //  ExpensesGovernment expensesGovernment = _ExpensesGovernmentRepository.GetById(ExpensesId);
                ExpensesGovernment? expensesGovernment = _TaamerProContext.ExpensesGovernment.Where(s => s.ExpensesId == ExpensesId).FirstOrDefault();
                if (expensesGovernment != null)
                {
                    expensesGovernment.IsDeleted = true;
                    expensesGovernment.DeleteDate = DateTime.Now;
                    expensesGovernment.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مصروف حكومي رقم " + ExpensesId;
                    _SystemAction.SaveAction("DeleteExpensesGovernment", "ExpensesGovernmentService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }


                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مصروف حكومي رقم " + ExpensesId; ;
                _SystemAction.SaveAction("DeleteExpensesGovernment", "ExpensesGovernmentService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
     

    }
}
