using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ProjectRequirementsGoalsRepository : IProjectRequirementsGoalsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectRequirementsGoalsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ProjectRequirementsGoalsVM>> GetAllrequirmentbyProjectid(string Lang, int projectid)
        {

            var requirments = _TaamerProContext.ProjectRequirementsGoals.Where(s => s.IsDeleted == false && s.ProjectId == projectid).Select(x => new ProjectRequirementsGoalsVM
            {
                ProjectId = x.ProjectId,
                RequirementId = x.RequirementId,
            RequirementGoalId=x.RequirementGoalId,
            RequirmentName=x.RequirementsandGoals.RequirmentName,
                timestr = x.RequirementsandGoals.TimeType == "1" ? (x.RequirementsandGoals.TimeNo) + " يوم " :
                          x.RequirementsandGoals.TimeType == "2" ? (x.RequirementsandGoals.TimeNo) + " اسبوع " :
                          x.RequirementsandGoals.TimeType == "3" ? (x.RequirementsandGoals.TimeNo) + " شهر " : x.RequirementsandGoals.TimeType == "4" ? (x.RequirementsandGoals.TimeNo) + " ساعه " : "",


                LineNo=x.RequirementsandGoals.LineNumber,




            }).ToList().OrderBy(x=>x.LineNo);

            return requirments;
        }

        public async Task<ProjectRequirementsGoalsVM> GetAllrequirmentbyrequireid(string Lang, int requireid)
        {

            var requirments = _TaamerProContext.ProjectRequirementsGoals.Where(s => s.IsDeleted == false && s.RequirementGoalId == requireid).Select(x => new ProjectRequirementsGoalsVM
            {
                ProjectId = x.ProjectId,
                RequirementId = x.RequirementId,
                RequirementGoalId = x.RequirementGoalId,
                RequirmentName = x.RequirementsandGoals.RequirmentName,
                timestr = x.RequirementsandGoals.TimeType == "1" ? (x.RequirementsandGoals.TimeNo) + " يوم " :
                          x.RequirementsandGoals.TimeType == "2" ? (x.RequirementsandGoals.TimeNo) + " اسبوع " :
                          x.RequirementsandGoals.TimeType == "3" ? (x.RequirementsandGoals.TimeNo) + " شهر " : x.RequirementsandGoals.TimeType == "4" ? (x.RequirementsandGoals.TimeNo) + " ساعه " : "",


                LineNo = x.RequirementsandGoals.LineNumber,




            }).FirstOrDefault();

            return requirments;
        }

    }
}
