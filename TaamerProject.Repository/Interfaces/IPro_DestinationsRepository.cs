using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_DestinationsRepository
    {
        Task<IEnumerable<Pro_DestinationsVM>> GetAllDestinations(int BranchId, List<int> BranchesList);
        Task<Pro_DestinationsVM> GetDestinationByProjectId(int projectId);
        Task<Pro_DestinationsVM> GetDestinationByProjectIdToReplay(int projectId);

    }
}
