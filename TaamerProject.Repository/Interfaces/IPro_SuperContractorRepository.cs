using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_SuperContractorRepository
    {
        Task<IEnumerable<Pro_SuperContractorVM>> GetAllSuperContractors(string SearchText);
        Task<Pro_SuperContractorVM> GetContractorData(int? ContractorId, int UserID, int BranchId);

    }
}
