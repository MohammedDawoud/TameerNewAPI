using System.Globalization;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using System.Net;
using TaamerProject.Service.IGeneric;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_ClausesService : IAcc_ClausesService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAcc_ClausesRepository _Acc_ClausesRepository;
        public Acc_ClausesService(IAcc_ClausesRepository acc_ClausesRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Acc_ClausesRepository = acc_ClausesRepository;
        }

        public Task<IEnumerable<Acc_ClausesVM>> GetAllClauses(string SearchText)
        {
            var Clauses = _Acc_ClausesRepository.GetAllClauses(SearchText);
            return Clauses;
        }
        public GeneralMessage SaveClause(Acc_Clauses Clause, int UserId, int BranchId)
        {
            try
            {

                if (Clause.ClauseId == 0)
                {


                    Clause.AddUser = UserId;
                    Clause.AddDate = DateTime.Now;
                    _TaamerProContext.Acc_Clauses.Add(Clause);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة بند جديد";
                    _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
                }
                else
                {
                    //var SupplierUpdated = _Acc_ClausesRepository.GetById(Clause.ClauseId);
                    var clausesUpdated = _TaamerProContext.Acc_Clauses.Where(s => s.ClauseId == Clause.ClauseId).FirstOrDefault();

                    if (clausesUpdated != null)
                    {
                       

                        if(Clause.ClauseId==1 || Clause.ClauseId == 2 || Clause.ClauseId == 3 )
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ البند";
                            _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.CanNotEditThisItem, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.CanNotEditThisItem };
                        }

                        clausesUpdated.NameAr = Clause.NameAr;
                        clausesUpdated.NameEn = Clause.NameEn;
                        clausesUpdated.UpdateUser = UserId;
                        clausesUpdated.UpdateDate = DateTime.Now;
                       
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل بند رقم " + Clause.ClauseId;
                    _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ البند";
                _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteClause(int ClauseId, int UserId,int BranchId)
        {
            try
            {
                if (ClauseId == 1 || ClauseId == 2 || ClauseId == 3)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف بند رقم " + ClauseId; ;
                    _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.CanNotDeleteThisItem, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.CanNotDeleteThisItem };
                }

                //Acc_Clauses Clause = _Acc_ClausesRepository.GetById(ClauseId);
                Acc_Clauses? Clause = _TaamerProContext.Acc_Clauses.Where(s => s.ClauseId == ClauseId).FirstOrDefault();
                if(Clause!=null)
                {
                    Clause.IsDeleted = true;
                    Clause.DeleteDate = DateTime.Now;
                    Clause.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف بند رقم " + ClauseId;
                    _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف بند رقم " + ClauseId; ;
                _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

    }
}
