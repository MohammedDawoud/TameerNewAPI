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
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ProjectRequirementsGoalsService :   IProjectRequirementsGoalsService
    {
        private readonly IProjectRequirementsGoalsRepository _projectRequirementsGoalsRepository;
         private readonly IProjectPhasesTasksRepository _projectPhasesTasksRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ProjectRequirementsGoalsService(IProjectRequirementsGoalsRepository projectRequirementsGoalsRepository,
            IProjectPhasesTasksRepository projectPhasesTasksRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {

            _projectRequirementsGoalsRepository = projectRequirementsGoalsRepository;
             _projectPhasesTasksRepository = projectPhasesTasksRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;


        }

        public Task<IEnumerable<ProjectRequirementsGoalsVM>> GetAllrequirmentbyProjectid(string Lang, int projectid)
        {
            var result = _projectRequirementsGoalsRepository.GetAllrequirmentbyProjectid(Lang, projectid);
            return result;
        }


        public Task<ProjectRequirementsGoalsVM> GetAllrequirmentbyrequireid(string Lang, int projectid)
        {
            var result = _projectRequirementsGoalsRepository.GetAllrequirmentbyrequireid(Lang, projectid);   
            return result;
        }

        public  async Task<ProjectRequirementsGoalsVM> GetFilterdrequirmentbyProjectid(string Lang, int projectid)
        {
            var result =await _projectRequirementsGoalsRepository.GetAllrequirmentbyProjectid(Lang, projectid);
            var pro = await _projectPhasesTasksRepository.GetProjectPhasesTasksbygoalandproject2(projectid, Lang);
            var pro2 = pro.Select(x => new { Id = x.ProjectGoals, projectGoal = x.ProjectGoals }).ToList();
            var filter = pro2.Select(x => x.projectGoal).ToArray();
            var UnmatchedIds = result.Where(x => !filter.Contains(x.RequirementGoalId));
            return UnmatchedIds?.FirstOrDefault()?? new ProjectRequirementsGoalsVM();
        }

        public GeneralMessage saveprojectrequirment(List<ProjectRequirementsGoals> requirementsGoals,int projectid, int UserId, int BranchId)
        {
            try { 
            _TaamerProContext.ProjectRequirementsGoals.RemoveRange(requirementsGoals.ToList());


            foreach (var item in requirementsGoals.ToList())
            {
                item.AddDate = DateTime.Now;
                item.AddUser = UserId;

                item.ProjectId = projectid;
                _TaamerProContext.ProjectRequirementsGoals.Add(item);
            }

            _TaamerProContext.SaveChanges();
            //-----------------------------------------------------------------------------------------------------------------
            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string ActionNote2 = " تعديل اهداف مشروع  " ;
            _SystemAction.SaveAction("saveprojectrequirment", "ProjectRequirementsGoalsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
            //-----------------------------------------------------------------------------------------------------------------
            return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
        }
    
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
              string ActionNote = "فشل في حفظ اهداف مشروع";
                _SystemAction.SaveAction("saveprojectrequirment", "ProjectRequirementsGoalsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }


        }

 
    }
}
