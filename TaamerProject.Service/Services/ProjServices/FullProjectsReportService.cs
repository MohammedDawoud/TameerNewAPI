using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class FullProjectsReportService : IFullProjectsReportService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IFullProjectsReportRepository _fullProjectsReportRepository;



        public FullProjectsReportService(TaamerProjectContext dataContext, ISystemAction systemAction, IFullProjectsReportRepository fullProjectsReportRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _fullProjectsReportRepository = fullProjectsReportRepository;
        }
        public async Task<List<FullProjectsReportVM>> GetAllprojectsreport(string lang)
        {
            var Prijectsreport =await _fullProjectsReportRepository.GetAllprojectsreport(lang);
            return Prijectsreport;
        }
        public async Task<List<FullProjectsReportVM>> GetPhaseTaskData(int PhaseId)
        {
            var Prijectsreport =await _fullProjectsReportRepository.GetPhaseTaskData(PhaseId);
            return Prijectsreport;
        }
        public async Task<List<FullProjectsReportVM>> GetProjectDataRE(int ProjectId)
        {
            var Prijectsreport = await _fullProjectsReportRepository.GetProjectDataRE(ProjectId);
            return Prijectsreport;
        }
    }
}
