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
    public class Pro_ProjectChallengesService: IPro_ProjectChallengesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IPro_ProjectChallengesRepository _Pro_ProjectChallengesRepository;

        public Pro_ProjectChallengesService(IPro_ProjectChallengesRepository Pro_ProjectChallengesRepository, TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Pro_ProjectChallengesRepository = Pro_ProjectChallengesRepository;
        }
        public Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallenges()
        {
            var ProjectChallenges = _Pro_ProjectChallengesRepository.GetAllProjectChallenges();
            return ProjectChallenges;
        }
        public Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectid(int projectid, int stepid)
        {
            var ProjectChallenges = _Pro_ProjectChallengesRepository.GetAllProjectChallengesbyprojectid(projectid,stepid);
            return ProjectChallenges;
        }
        public Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectidOnly(int projectid)
        {
            var ProjectChallenges = _Pro_ProjectChallengesRepository.GetAllProjectChallengesbyprojectidOnly(projectid);
            return ProjectChallenges;
        }
        public GeneralMessage SaveProjectChallenge(List<Pro_ProjectChallenges> ProjectChallenges, int UserId, int BranchId)
        {
            try
            {

                if (ProjectChallenges.Count() > 0)
                {
                    var ProjectChallengeList = _TaamerProContext.Pro_ProjectChallenges.Where(s => s.ProjectId == ProjectChallenges.FirstOrDefault().ProjectId
                    && s.StepId == ProjectChallenges.FirstOrDefault().StepId).ToList();
                    if (ProjectChallengeList.Count() > 0)
                    {
                        _TaamerProContext.Pro_ProjectChallenges.RemoveRange(ProjectChallengeList);
                    }

                    foreach (var item in ProjectChallenges.ToList())
                    {
                        item.ProjectChallengeId = 0;
                        item.ProjectId = item.ProjectId;
                        item.StepId = item.StepId;
                        item.NameAr = item.NameAr ?? "";
                        item.NameEn = item.NameEn;
                        item.LineNumber = item.LineNumber;
                        item.UserId = UserId;
                        item.BranchId = BranchId;
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;
                        _TaamerProContext.Pro_ProjectChallenges.Add(item);
                    }
                }
                _TaamerProContext.SaveChanges();
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.addnewitem;
                _SystemAction.SaveAction("SaveProjectChallenge", "Pro_ProjectChallengesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تحدي المشروع";
                _SystemAction.SaveAction("SaveProjectChallenge", "Pro_ProjectChallengesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteProjectChallenge(int ProjectChallengeid, int UserId, int BranchId)
        {
            try
            {
                Pro_ProjectChallenges? ProjectChallenge = _TaamerProContext.Pro_ProjectChallenges.Where(s => s.ProjectChallengeId == ProjectChallengeid).FirstOrDefault();
                if (ProjectChallenge != null)
                {
                    ProjectChallenge.IsDeleted = true;
                    ProjectChallenge.DeleteDate = DateTime.Now;
                    ProjectChallenge.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف تحدي المشروع رقم " + ProjectChallengeid;
                    _SystemAction.SaveAction("DeleteProjectChallenge", "Pro_ProjectChallengesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف تحدي المشروع رقم " + ProjectChallengeid; ;
                _SystemAction.SaveAction("DeleteProjectChallenge", "Pro_ProjectChallengesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
