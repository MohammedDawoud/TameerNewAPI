using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectStatusTasksRepository
    {

        Task<IEnumerable<ProjectVM>> GetAllProjectsStatusTasks(string Lang, int BranchId);
        Task<IEnumerable<ProjectVM>> GetProjectsStatusTasksSearch(ProjectVM ProjectsSearch, string Lang, string Con, int BranchId);




    }
}
