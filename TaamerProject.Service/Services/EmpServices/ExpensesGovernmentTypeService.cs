
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
    public class ExpensesGovernmentTypeService :   IExpensesGovernmentTypeService
    {
        private readonly IExpensesGovernmentTypeRepository _ExpensesGovernmentTypeRepository;
         private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ExpensesGovernmentTypeService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IExpensesGovernmentTypeRepository ExpensesGovernmentTypeRepository)
        {
            _ExpensesGovernmentTypeRepository = ExpensesGovernmentTypeRepository;
             _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<ExpensesGovernmentTypeVM>> GetAllExpensesGovernmentTypes(string SearchText, int BranchId)
        {
            var GovernmentTypes = _ExpensesGovernmentTypeRepository.GetAllExpensesGovernmentTypes(SearchText, BranchId);
            return GovernmentTypes;
        }
        public GeneralMessage SaveExpensesGovernmentType(ExpensesGovernmentType expensesGovernmentType,int UserId, int BranchId)
        {
            try
            {
                if (expensesGovernmentType.ExpensesGovernmentTypeId == 0)
                {
                    expensesGovernmentType.BranchId = BranchId;
                    expensesGovernmentType.AddUser = UserId;
                    expensesGovernmentType.AddDate = DateTime.Now;
                    expensesGovernmentType.Code = (_TaamerProContext.ExpensesGovernmentType.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.ExpensesGovernmentType.Add(expensesGovernmentType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع مصروف حكومي";
                    _SystemAction.SaveAction("SaveExpensesGovernmentType", "ExpensesGovernmentTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var ExpensesGovernmentTypeUpdated = _ExpensesGovernmentTypeRepository.GetById(expensesGovernmentType.ExpensesGovernmentTypeId);
                    ExpensesGovernmentType? ExpensesGovernmentTypeUpdated = _TaamerProContext.ExpensesGovernmentType.Where(s => s.ExpensesGovernmentTypeId == expensesGovernmentType.ExpensesGovernmentTypeId).FirstOrDefault();

                    if (ExpensesGovernmentTypeUpdated != null)
                    {
                        ExpensesGovernmentTypeUpdated.Code = expensesGovernmentType.Code;
                        ExpensesGovernmentTypeUpdated.NameAr = expensesGovernmentType.NameAr;
                        ExpensesGovernmentTypeUpdated.NameEn = expensesGovernmentType.NameEn;
                        ExpensesGovernmentTypeUpdated.Notes = expensesGovernmentType.Notes;
                        ExpensesGovernmentTypeUpdated.UserId = expensesGovernmentType.UserId;
                        ExpensesGovernmentTypeUpdated.UpdateUser = UserId;
                        ExpensesGovernmentTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع مصروف حكومي رقم " + expensesGovernmentType.ExpensesGovernmentTypeId;
                    _SystemAction.SaveAction("SaveExpensesGovernmentType", "ExpensesGovernmentTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع مصروف حكومي";
                _SystemAction.SaveAction("SaveExpensesGovernmentType", "ExpensesGovernmentTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteExpensesGovernmentType(int ExpensesGovernmentTypeId,int UserId,int BranchId)
        {
            try
            {
               // ExpensesGovernmentType expensesGovernmentType = _ExpensesGovernmentTypeRepository.GetById(ExpensesGovernmentTypeId);
                ExpensesGovernmentType? expensesGovernmentType = _TaamerProContext.ExpensesGovernmentType.Where(s => s.ExpensesGovernmentTypeId == ExpensesGovernmentTypeId).FirstOrDefault();
                if (expensesGovernmentType != null)
                {
                    expensesGovernmentType.IsDeleted = true;
                    expensesGovernmentType.DeleteDate = DateTime.Now;
                    expensesGovernmentType.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع مصروف حكومي رقم " + ExpensesGovernmentTypeId;
                    _SystemAction.SaveAction("DeleteExpensesGovernmentType", "ExpensesGovernmentTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نوع مصروف حكومي رقم " + ExpensesGovernmentTypeId; ;
                _SystemAction.SaveAction("DeleteExpensesGovernmentType", "ExpensesGovernmentTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillExpensesGovernmentTypeSelect(int BranchId)
        {
            return _ExpensesGovernmentTypeRepository.GetAllExpensesGovernmentTypes("",BranchId).Result.Select(s => new
            {
                Id = s.ExpensesGovernmentTypeId,
                Name = s.NameAr
            });
        }
       

    }
}
