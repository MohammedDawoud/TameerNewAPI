using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class FullProjectsReportRepository : IFullProjectsReportRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public FullProjectsReportRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<List<FullProjectsReportVM>> GetAllprojectsreport(string lang)
        {
            var FUllProj = _TaamerProContext.FullProjectsReport.Where(s => s.IsDeleted == false).Select(x => new FullProjectsReportVM
            {
                ReportId = x.ReportId,
                Type = x.Type ?? 0,
                ProjectId = x.ProjectId ?? 0,
                PhaseTaskId = x.PhaseTaskId ?? 0,
                Revenue = x.Revenue ?? 0,
                Expenses = x.Expenses ?? 0,
                Projectpercentage = x.Projectpercentage ?? 0,
                Taskpercentage = x.Taskpercentage ?? 0,
                date = x.date,
                Time = x.Time ?? 0,

            }).ToList();
            return FUllProj;
        }

        public async Task<List<FullProjectsReportVM>> GetPhaseTaskData(int PhaseId)
        {
            var FUllProj = _TaamerProContext.FullProjectsReport.Where(s => s.IsDeleted == false && s.PhaseTaskId==PhaseId).Select(x => new FullProjectsReportVM
            {
                ReportId = x.ReportId,
                Type = x.Type ?? 0,
                ProjectId = x.ProjectId ?? 0,
                PhaseTaskId = x.PhaseTaskId ?? 0,
                Revenue = x.Revenue ?? 0,
                Expenses = x.Expenses ?? 0,
                Projectpercentage = x.Projectpercentage ?? 0,
                Taskpercentage = x.Taskpercentage ?? 0,
                date = x.date,
                Time = x.Time ?? 0,
                //PhaseTimeType = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.TimeType : 0,
                ActualDateTime= x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.TimeType==1?
                x.Time>12?(x.Time-12).ToString()+" PM":x.Time==12?x.Time.ToString()+" PM": x.Time.ToString()+" AM":x.date:x.date,

            }).ToList();
            return FUllProj;
        }
        public async Task<List<FullProjectsReportVM>> GetProjectDataRE(int ProjectId)
        {
            var FUllProj = _TaamerProContext.FullProjectsReport.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new FullProjectsReportVM
            {
                ReportId = x.ReportId,
                Type = x.Type ?? 0,
                ProjectId = x.ProjectId ?? 0,
                PhaseTaskId = x.PhaseTaskId ?? 0,
                Revenue = x.Revenue ?? 0,
                Expenses = x.Expenses ?? 0,
                Projectpercentage = x.Projectpercentage ?? 0,
                Taskpercentage = x.Taskpercentage ?? 0,
                date = x.date,
                Time = x.Time ?? 0,

            }).ToList();
            return FUllProj;
        }


    }
}
