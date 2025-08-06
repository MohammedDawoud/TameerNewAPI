using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_SuperContractorService  
    {
        Task<IEnumerable<Pro_SuperContractorVM>> GetAllSuperContractor(string SearchText);
        Task<Pro_SuperContractorVM> GetContractorData(int? ContractorId, int UserID, int BranchId);

        GeneralMessage SaveSuperContractor(Pro_SuperContractor Contractor, int UserId, int BranchId);
        GeneralMessage DeleteSuperContractor(int ContractorId, int UserId, int BranchId);
    }
}
