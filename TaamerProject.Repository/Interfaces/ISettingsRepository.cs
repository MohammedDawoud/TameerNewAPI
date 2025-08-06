using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISettingsRepository
    {
        Task<IEnumerable<SettingsVM>> GetAllMainPhases(int? ProSubTypeId, int BranchId);
        Task<IEnumerable<SettingsVM>> GetAllSubPhases(int? ParentId, int BranchId);

        Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectID(int ProSubTypeId);
        Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDNew(int ProSubTypeId);

        Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectIDAll();
        Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDAllNew();

        Task<IEnumerable<SettingsVM>> GetAllTasks(int? ProSubTypeId, int BranchId);
        Task<IEnumerable<SettingsVM>> GetAllTasksByPhaseId(int PhaseId);
    }
}
