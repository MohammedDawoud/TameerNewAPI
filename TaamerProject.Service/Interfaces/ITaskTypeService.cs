using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ITaskTypeService  
    {
        Task<IEnumerable<TaskTypeVM>> GetAllTaskType(int BranchId);
        Task<IEnumerable<TaskTypeVM>> GetAllTaskType2(string SearchText);

        GeneralMessage SaveTaskType(TaskType taskType, int UserId, int BranchId);
        GeneralMessage DeleteTaskType(int TaskTypeId, int UserId,int BranchId);
        Task<IEnumerable<TaskTypeVM>> FillTaskTypeSelect(int BranchId);
        Task<IEnumerable<TaskTypeVM>> FillTaskTypeSelectAE(int BranchId);




    }
}
