using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IBranchesService
    {
        Task<IEnumerable<BranchesVM>> GetAllBranches(string lang);
        Task<IEnumerable<BranchesVM>> FillBranchSelectNew(string lang);

        Task<IEnumerable<BranchesVM>> GetBranchByBranchId(string lang, int BranchId);
        Task<BranchesVM> GetBranchByBranchIdCheck(string lang, int BranchId);

        Task<IEnumerable<BranchesVM>> GetAllBranchesByUserId(string Lang, int UserId);
        Task<IEnumerable<BranchesVM>> GetAllBranchesAndMainByUserId(string Lang, int UserId);

        GeneralMessage SaveBranches(Branch branches, int UserId, string Lang, int BranchId);
        GeneralMessage DeleteBranches(int BranchId, int UserId, string Lang);
        GeneralMessage SaveBranchesInvoiceCode(Branch branches, int UserId, int BranchId);
        GeneralMessage SaveBrancheAccs(Branch branches, int UserId, int BranchId);

        GeneralMessage SaveCSIDBranch(int BranchId, string CSR, string PrivateKey, string CSID, string SecretKey, int UserId, int BranchIdO);

        GeneralMessage SaveBranchesAccsBS(Branch branches, int UserId, int BranchId);
        GeneralMessage SaveBranchesAccsKD(Branch branches, int UserId, int BranchId);


        GeneralMessage ActivateBranches(int BranchId, int UserId);
        Task<BranchesVM> GetActiveBranch();
        Task<Branch> GetBranchById(int BranchId);
        Task<int> GenerateNextBranchNumber();
        Task<int> GetOrganizationId(int BranchId);
    }
}
