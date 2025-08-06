using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ITaskTypeRepository
    {
        Task<IEnumerable<TaskTypeVM>> GetAllTaskType(int BranchId);
        Task<IEnumerable<TaskTypeVM>> GetAllTaskType2(string SearchText);

    }
}
