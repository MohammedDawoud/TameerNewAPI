using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IFullProjectsReportRepository
    {
        Task<List<FullProjectsReportVM>> GetAllprojectsreport(string lang);
        Task<List<FullProjectsReportVM>> GetPhaseTaskData(int PhaseId);
        Task<List<FullProjectsReportVM>> GetProjectDataRE(int ProjectId);


    }
}
