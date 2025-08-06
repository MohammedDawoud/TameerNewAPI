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
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class RequirementsandGoalsService :   IRequirementsandGoalsService
    {
        private readonly IRequirementsandGoalsRepository _requirementsandGoalsRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly ISettingsRepository _SettingsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public RequirementsandGoalsService(IRequirementsandGoalsRepository requirementsandGoalsRepository,
            ISys_SystemActionsRepository sys_SystemActionsRepository,
            ISettingsRepository settingsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _requirementsandGoalsRepository = requirementsandGoalsRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _SettingsRepository = settingsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<RequirementsandGoalsVM>> GetAllrequirmentbyProjecttype(string Lang, int projecttypeid)
        {
            var result = _requirementsandGoalsRepository.GetAllrequirmentbyProjecttype(Lang, projecttypeid);


            return result;

        }


        public async Task<IEnumerable<RequirementsandGoalsVM>> GetAllrequirmentbyProjecttype2(string Lang, int projecttypeid, int projectsubtype)
        {
            var result = await _requirementsandGoalsRepository.GetAllrequirmentbyProjecttype(Lang, projecttypeid);

            //var result = _projectRequirementsGoalsRepository.GetAllrequirmentbyProjectid(Lang, projectid);
            var pro = await _SettingsRepository.GetAllSettingsByProjectID(projectsubtype);
            var pro2 = pro.Select(x => new { Id = x.RequirmentId, projectGoal = x.RequirmentId }).ToList();
            var filter = pro2.Select(x => x.projectGoal).ToArray();
            var UnmatchedIds = result.Where(x => !filter.Contains(x.RequirementId));

            return UnmatchedIds;
        }
        public GeneralMessage deleteprojectrequirment(int requirmentid, int UserId, int BranchId)
        {
            try
            {
                //var req =_requirementsandGoalsRepository.GetById(requirmentid);
                RequirementsandGoals? req = _TaamerProContext.RequirementsandGoals.Where(s => s.RequirementId == requirmentid).FirstOrDefault();
                if (req !=null)
                {
                    req.DeleteDate = DateTime.Now;
                    req.DeleteUser = UserId;
                    req.IsDeleted = true;

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "حذف اهداف مشروع  ";
                    _SystemAction.SaveAction("deleteprojectrequirment", "RequirementsandGoalsService", 2, Resources.General_DeletedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }

            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف اهداف مشروع";
                _SystemAction.SaveAction("deleteprojectrequirment", "RequirementsandGoalsService", 1, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }


        }

 

    }
}
