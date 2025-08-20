
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
    public class ExpRevenuExpensesService :   IExpRevenuExpensesService
    {
        private readonly IExpRevenuExpensesRepository _revenuExpensesRepository;
         private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public ExpRevenuExpensesService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IExpRevenuExpensesRepository revenuExpensesRepository)
        {
            _revenuExpensesRepository = revenuExpensesRepository;
             _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpRevenuExpenses(int BranchId)
        {
            var revenuExpenses = _revenuExpensesRepository.GetAllExpRevenuExpenses(BranchId);
            return revenuExpenses;
        }
     public Task<IEnumerable<ExpRevenuExpensesVM>> GetAllExpBysearchObject(ExpRevenuExpensesVM expsearch, int BranchId)
        {
           
          return _revenuExpensesRepository.GetAllExpBysearchObject(expsearch, BranchId);
        }
        public GeneralMessage SaveExpRevenuExpenses(ExpRevenuExpenses revenuExpenses, int UserId, int BranchId)
        {
            try
            {
                if (revenuExpenses.ExpecteId == 0)
                {
                    revenuExpenses.BranchId = BranchId;
                    revenuExpenses.AddUser = UserId;
                    revenuExpenses.AddDate = DateTime.Now;
                    _TaamerProContext.ExpRevenuExpenses.Add(revenuExpenses);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نفقات موظف";
                    _SystemAction.SaveAction("SaveExpRevenuExpenses", "ExpRevenuExpensesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var CheckUpdated = _revenuExpensesRepository.GetById(revenuExpenses.ExpecteId);
                    ExpRevenuExpenses? CheckUpdated = _TaamerProContext.ExpRevenuExpenses.Where(s => s.ExpecteId == revenuExpenses.ExpecteId).FirstOrDefault();
                    
                    if (CheckUpdated != null)
                    {
                        CheckUpdated.Amount = revenuExpenses.Amount;
                        CheckUpdated.IsDone = revenuExpenses.IsDone;
                        CheckUpdated.Notes = revenuExpenses.Notes;
                        CheckUpdated.CollectionDate = revenuExpenses.CollectionDate;
                        CheckUpdated.CostCenterId = revenuExpenses.CostCenterId;
                        CheckUpdated.Notes = revenuExpenses.Notes;
                        CheckUpdated.AccountId = revenuExpenses.AccountId;
                        CheckUpdated.ToAccountId = revenuExpenses.ToAccountId;
                        CheckUpdated.Type = revenuExpenses.Type;
                        CheckUpdated.UpdateUser = UserId;
                        CheckUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نفقات موظف رقم " + revenuExpenses.ExpecteId;
                    _SystemAction.SaveAction("SaveExpRevenuExpenses", "ExpRevenuExpensesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نفقات موظف";
                _SystemAction.SaveAction("SaveExpRevenuExpenses", "ExpRevenuExpensesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteRevenuExpenses(int ExpectedId,int UserId, int BranchId)
        {
            try
            {
                // ExpRevenuExpenses expRevenu = _revenuExpensesRepository.GetById(ExpectedId);
                ExpRevenuExpenses? expRevenu = _TaamerProContext.ExpRevenuExpenses.Where(s => s.ExpecteId == ExpectedId).FirstOrDefault();
                if (expRevenu != null)
                {
                    expRevenu.IsDeleted = true;
                    expRevenu.DeleteDate = DateTime.Now;
                    expRevenu.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نفقات موظف رقم " + ExpectedId;
                    _SystemAction.SaveAction("DeleteRevenuExpenses", "ExpRevenuExpensesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نفقات موظف رقم " + ExpectedId; ;
                _SystemAction.SaveAction("DeleteRevenuExpenses", "ExpRevenuExpensesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage FinishRestoreRevenuExpenses(int ExpectedId, int UserId, int BranchId)
        {
            try
            {
                //  ExpRevenuExpenses expRevenu = _revenuExpensesRepository.GetById(ExpectedId);
                ExpRevenuExpenses? expRevenu = _TaamerProContext.ExpRevenuExpenses.Where(s => s.ExpecteId == ExpectedId).FirstOrDefault();
                if (expRevenu != null)
                {
                    expRevenu.IsDone = expRevenu.IsDone ? false : true;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " انهاء نفقات موظف رقم " + ExpectedId;
                    _SystemAction.SaveAction("FinishRestoreRevenuExpenses", "ExpRevenuExpensesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نفقات موظف";
                _SystemAction.SaveAction("FinishRestoreRevenuExpenses", "ExpRevenuExpensesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public Task<IEnumerable< GetTotalExpRevByCCVM>> GetTotalExpRevByCC(string Con)
        {
            var revenuExpenses = _revenuExpensesRepository.GetTotalExpRevByCC(Con);
            return revenuExpenses;
        }
       


    }
}
