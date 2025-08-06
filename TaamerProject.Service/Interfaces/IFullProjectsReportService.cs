using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IFullProjectsReportService
    {
       Task<List<FullProjectsReportVM>> GetAllprojectsreport(string lang);
        Task<List<FullProjectsReportVM>> GetPhaseTaskData(int PhaseId);
        Task<List<FullProjectsReportVM>> GetProjectDataRE(int ProjectId);
    }
}
