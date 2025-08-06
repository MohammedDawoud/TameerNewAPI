using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDependencySettingsService  
    {
        Task<IEnumerable<DependencySettingsVM>> GetAllDependencySettings(int? SuccessorId, int BranchId);
        GeneralMessage SaveDependencySettings(int ProjectSubTypeId, List<DependencySettings> TaskLink, List<NodeLocations> NodeLocList, int UserId, int BranchId);
        GeneralMessage SaveDependencySettingsNew(int ProjectSubTypeId, List<DependencySettingsNew> TaskLinkList, List<SettingsNew> TasksList, int UserId, int BranchId);

        GeneralMessage DeleteDependencySettings(int DependencyId,int UserId,int BranchId);
        TasksNodeVM GetTasksNodeByProSubTypeId(int ProjSubTypeId, int BranchId, int UserId);
        TasksNodeNewVM GetTasksNodeByProSubTypeIdNew(int ProjSubTypeId, int BranchId, int UserId);

        List<AccountTreeVM> GetProjSubTypeIdSettingTree(int ProjectSubTypeId, int BranchId);
        GeneralMessage TransferSetting(int ProjSubTypeFromId, int ProjSubTypeToId, int BranchId, int UserId);
        GeneralMessage TransferSettingNEW(int ProjSubTypeFromId, int ProjSubTypeToId, int BranchId, int UserId);
        GeneralMessage TransferSettingNewGrantt(int ProjSubTypeFromId, int ProjSubTypeToId, int BranchId, int UserId);

    }
}
