using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ITasksDependencyService  
    {
        Task<IEnumerable<TasksDependencyVM>> GetAllTasksDependencies(int BranchId);
        GeneralMessage SaveTasksDependency(TasksDependency TasksDependency, int UserId, int BranchId);
        GeneralMessage DeleteTasksDependency(int DependencyId, int UserId, int BranchId);
        ProjectTasksNodeVM GetTasksNodeByProjectId(int ProjectId);
        ProjectTasksNodeVM GetTasksNodeByProjectIdNew(int ProjectId);

        TasksDependency GetTasksDependency(int ProjectId);
        List<AccountTreeVM> GetProjectPhasesTaskTree(int ProjectId);
        GeneralMessage SaveDependencyPhasesTask(int ProjectId, List<TasksDependency> TaskLink, List<NodeLocations> NodeLocList, int UserId, int BranchId);
        GeneralMessage SaveDependencyPhasesTaskNew(int ProjectId, List<TasksDependency> TaskLink, List<ProjectPhasesTasks> TasksList, int UserId, int BranchId);

    }
}
