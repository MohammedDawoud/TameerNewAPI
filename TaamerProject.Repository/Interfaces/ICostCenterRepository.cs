using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICostCenterRepository : IRepository<CostCenters>
    {
        Task<IEnumerable<CostCentersVM>> GetAllCostCenters(string SearchText, string lang, int BranchId);
        Task<IEnumerable<CostCentersVM>> GetAllCostCenters_B(string SearchText, string lang, int BranchId);

        Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCostBranch(string SearchText, string lang, int BranchId, int CostBranchId);

        Task<CostCentersVM> GetCostCenterById(int CostCenterId);
        Task<CostCentersVM> GetCostCenterByProId(int ProjectId);
        Task<CostCentersVM> GetCostCenterByCode(string Code, string Lang, int BranchId);
        Task<IEnumerable<CostCentersVM>> GetAllCostCentersTransactions(string FromDate, string ToDate, int YearId, string lang, int BranchId);
        Task<IEnumerable<CostCentersVM>> GetCostCenterReport(int BranchId, string lang, int YearId, string FromDate, string ToDate);
        Task<IEnumerable<CostCentersVM>> GetCostCenterTransaction(int BranchId, string lang, int YearId, int CostCenterId, string FromDate, string ToDate);
        Task<IEnumerable<CostCentersVM>> GetCostCenterAccountTransaction(int BranchId, string lang, int YearId, int CostCenterId, string FromDate, string ToDate);
        Task<IEnumerable<CostCentersVM>> GetAllCostCenterByCustomers(int BranchId, int custID);
        Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCustId(string lang, int BranchId, int? custID);
        // IEnumerable<CostCentersVM> GetAllCostCenters(string SearchText, string lang, int BranchId);
        //heba
        Task<DataTable> TreeViewOfCostCenter(string Con);
        Task<CostCentersVM> GetBranch_Costcenter(string lang, int BranchId);
    }
}
