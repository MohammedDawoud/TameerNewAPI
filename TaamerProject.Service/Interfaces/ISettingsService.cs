using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ISettingsService  
    {
        Task<IEnumerable<SettingsVM>> GetAllMainPhases(int? ProSubTypeId, int BranchId);
        GeneralMessage SaveSettings(Settings settings, int UserId, int BranchId);
        IEnumerable<object> GetSetting_Statment(string Con, string SelectStetment);
        GeneralMessage ConvertUserSettingsSome(int SettingId, int FromUserId, int ToUserId, int UserId, int BranchId);
        GeneralMessage ConvertMoreUserSettings(List<int> SettingIds, int FromUserId, int ToUserId, int UserId, int BranchId);

        GeneralMessage SaveSettingsList(List<Settings> settings, int UserId, int BranchId);
        int SaveSettings2(Settings settings, int UserId, int BranchId);
        GeneralMessage DeleteSettings(int SettingId, int UserId, int BranchId);
        GeneralMessage ConvertTasksSubToMain(int SettingId, int UserId, int BranchId);

        GeneralMessage MerigTasks(int[] TasksIdArray, string Description, string Note, int UserId, int BranchId);
        Task<IEnumerable<SettingsVM>> GetAllSubPhases(int? ParentId, int BranchIdProjectID);
        Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectID(int ProjectID);
        Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDNew(int ProjectID);

        Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectIDAll();
        Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDAllNew();


        Task<IEnumerable<SettingsVM>> GetAllTasks(int? ProSubTypeId, int BranchId);
            //alaa
        Boolean checkHaveMainPhases(int? Param, int BranchId);
        IEnumerable<object> FillMainPhases(int? Param, int BranchId);
        IEnumerable<object> FillSubPhases(int? Param, int BranchId);
        IEnumerable<SettingsVM> GetAllTasksByPhaseId(int PhaseId);
    }
}
