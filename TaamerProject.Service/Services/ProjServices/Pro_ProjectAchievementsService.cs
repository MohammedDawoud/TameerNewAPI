using System.Threading.Tasks;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Service.IGeneric;
using Twilio.Base;
using TaamerP.Service.LocalResources;
using System.Globalization;

namespace TaamerProject.Service.Services
{
    public class Pro_ProjectAchievementsService: IPro_ProjectAchievementsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IPro_ProjectAchievementsRepository _Pro_ProjectAchievementsRepository;

        public Pro_ProjectAchievementsService(IPro_ProjectAchievementsRepository Pro_ProjectAchievementsRepository, TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Pro_ProjectAchievementsRepository = Pro_ProjectAchievementsRepository;
        }
        public Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievements()
        {
            var ProjectAchievements = _Pro_ProjectAchievementsRepository.GetAllProjectAchievements();
            return ProjectAchievements;
        }
        public Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectid(int projectid, int stepid)
        {
            var ProjectAchievements = _Pro_ProjectAchievementsRepository.GetAllProjectAchievementsbyprojectid(projectid, stepid);
            return ProjectAchievements;
        }
        public Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectidOnly(int projectid)
        {
            var ProjectAchievements = _Pro_ProjectAchievementsRepository.GetAllProjectAchievementsbyprojectidOnly(projectid);
            return ProjectAchievements;
        }

        public GeneralMessage SaveProjectAchievement(List<Pro_ProjectAchievements> ProjectAchievement, int UserId, int BranchId)
        {
            try
            {
                if (ProjectAchievement.Count() > 0)
                {

                    var ProjectAchievementList = _TaamerProContext.Pro_ProjectAchievements.Where(s => s.ProjectId == ProjectAchievement.FirstOrDefault().ProjectId
                    && s.StepId == ProjectAchievement.FirstOrDefault().StepId).ToList();
                    if (ProjectAchievementList.Count() > 0)
                    {
                        _TaamerProContext.Pro_ProjectAchievements.RemoveRange(ProjectAchievementList);
                    }
                    foreach (var item in ProjectAchievement.ToList())
                    {
                        item.ProjectAchievementId = 0;
                        item.ProjectId = item.ProjectId;
                        item.StepId = item.StepId;
                        item.NameAr = item.NameAr??"";
                        item.NameEn = item.NameEn;
                        item.LineNumber = item.LineNumber;
                        item.UserId = UserId;
                        item.BranchId = BranchId;
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;
                        _TaamerProContext.Pro_ProjectAchievements.Add(item);
                    }
                }
                _TaamerProContext.SaveChanges();

                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.addnewitem;
                _SystemAction.SaveAction("SaveProjectAchievement", "Pro_ProjectAchievementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ إنجاز المشروع";
                _SystemAction.SaveAction("SaveProjectAchievement", "Pro_ProjectAchievementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteProjectAchievement(int ProjectAchievementid, int UserId, int BranchId)
        {
            try
            {
                Pro_ProjectAchievements? ProjectAchievement = _TaamerProContext.Pro_ProjectAchievements.Where(s => s.ProjectAchievementId == ProjectAchievementid).FirstOrDefault();
                if (ProjectAchievement != null)
                {
                    ProjectAchievement.IsDeleted = true;
                    ProjectAchievement.DeleteDate = DateTime.Now;
                    ProjectAchievement.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف إنجاز المشروع رقم " + ProjectAchievementid;
                    _SystemAction.SaveAction("DeleteProjectAchievement", "Pro_ProjectAchievementsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف إنجاز المشروع رقم " + ProjectAchievementid; ;
                _SystemAction.SaveAction("DeleteProjectAchievement", "Pro_ProjectAchievementsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
