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
    public class OutMovementsService : IOutMovementsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOutMovementsRepository _OutMovementsRepository;
        private readonly IUsersRepository _UsersRepository;


        public OutMovementsService(TaamerProjectContext dataContext, ISystemAction systemAction, IOutMovementsRepository outMovementsRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OutMovementsRepository = outMovementsRepository;
            _UsersRepository = usersRepository;
        }

        public async Task<IEnumerable<OutMovementsVM>> GetAllOutMovements(int? TrailingId)
        {
            var OutMovements = await _OutMovementsRepository.GetAllOutMovements(TrailingId);
            return OutMovements;
        }
        public GeneralMessage SaveOutMovements(OutMovements outMovements, int UserId, int BranchId)
        {
            try
            {
                if (outMovements.MoveId == 0)
                {
                    outMovements.BranchId = BranchId;
                    outMovements.AddUser = UserId;
                    outMovements.AddDate = DateTime.Now;
                    _TaamerProContext.OutMovements.Add(outMovements);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة حركة ";
                   _SystemAction.SaveAction("SaveOutMovements", "OutMovementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var OutMovementsUpdated = _TaamerProContext.OutMovements.Where(x=>x.MoveId==outMovements.MoveId).FirstOrDefault();
                    if (OutMovementsUpdated != null)
                    {
                        OutMovementsUpdated.ConstraintNo = outMovements.ConstraintNo;
                        OutMovementsUpdated.EmpId = outMovements.EmpId;
                        OutMovementsUpdated.OrderNo = outMovements.OrderNo;
                        OutMovementsUpdated.RequiredWork = outMovements.RequiredWork;
                        OutMovementsUpdated.FinishedWork = outMovements.FinishedWork;
                        OutMovementsUpdated.Date = outMovements.Date;
                        OutMovementsUpdated.HijriDate = outMovements.HijriDate;
                        OutMovementsUpdated.ExpeditorId = outMovements.ExpeditorId;
                        OutMovementsUpdated.TrailingId = outMovements.TrailingId;
                        OutMovementsUpdated.UpdateUser = UserId;
                        OutMovementsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل الحركة رقم " + outMovements.MoveId;
                    _SystemAction.SaveAction("SaveOutMovements", "OutMovementsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الحركة";
                _SystemAction.SaveAction("SaveOutMovements", "OutMovementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage ConfirmOutMovements(int? TrailingId, int UserId, int BranchId)
        {
            try
            {
                var ProjectTrailingUpdated = _TaamerProContext.ProjectTrailing.Where(x => x.TrailingId == TrailingId).FirstOrDefault();
                var project = _TaamerProContext.Project.Where(x => x.ProjectId == ProjectTrailingUpdated.ProjectId).FirstOrDefault();
                if (ProjectTrailingUpdated != null)
                {
                    ProjectTrailingUpdated.Active = false;

                    var ProTrailing = new ProjectTrailing();
                    ProTrailing.ProjectId = ProjectTrailingUpdated.ProjectId;
                    ProTrailing.DepartmentId = ProjectTrailingUpdated.DepartmentId;
                    ProTrailing.Active = true;
                    ProTrailing.Status = 2;
                    ProTrailing.TypeId = 0;
                    ProTrailing.Date = ProjectTrailingUpdated.Date;
                    ProTrailing.BranchId = BranchId;
                    ProTrailing.AddUser = UserId;
                    ProTrailing.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectTrailing.Add(ProTrailing);
                    try
                    {
                        var user = _UsersRepository.GetById(UserId);
                        var ProNotification = new Notification();
                        ProNotification.Name = "اشعار علي مشروع رقم " + project.ProjectNo + "";
                        ProNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        ProNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        ProNotification.SendUserId = UserId;
                        ProNotification.Type = 1; // notification
                        ProNotification.Description = "تم رفع المشروع رقم " + project.ProjectNo + " للأمانه بواسطة " + user.FullName + "";
                        ProNotification.AllUsers = true;
                        ProNotification.SendDate = DateTime.Now;
                        ProNotification.ProjectId = ProjectTrailingUpdated.ProjectId;
                        ProNotification.AddUser = UserId;
                        ProNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(ProNotification);
                        _TaamerProContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "ارسال المشروع للجهة الخارجية ";
                _SystemAction.SaveAction("ConfirmOutMovements", "OutMovementsService", 1, Resources.projectSentToThirdParty, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.projectSentToThirdParty, ReturnedStr = project.ProjectNo };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("ConfirmOutMovements", "OutMovementsService", 1, Resources.Not_sent, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Not_sent };
            }
        }
        public GeneralMessage DeleteOutMovements(int MoveId, int UserId, int BranchId)
        {
            try
            {
                OutMovements outMovements = _TaamerProContext.OutMovements.Where(x => x.MoveId == MoveId).FirstOrDefault();
                outMovements.IsDeleted = true;
                outMovements.DeleteDate = DateTime.Now;
                outMovements.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف الارسال رقم " + MoveId;
                _SystemAction.SaveAction("DeleteOutMovements", "OutMovementsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الارسال رقم " + MoveId; ;
                _SystemAction.SaveAction("DeleteOutMovements", "OutMovementsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
