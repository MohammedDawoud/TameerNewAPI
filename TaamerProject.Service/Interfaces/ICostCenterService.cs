using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICostCenterService
    {
        Task<IEnumerable<CostCentersVM>> GetAllCostCenters(string SearchText, string lang, int BranchId);
        Task<IEnumerable<CostCentersVM>> GetAllCostCenters_B(string SearchText, string lang, int BranchId);

        Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCostBranch(string SearchText, string lang, int BranchId, int CostBranchId);

        GeneralMessage SaveCostCenter(CostCenters costCenter, int UserId, int BranchId);
        GeneralMessage DeleteCostCenter(int CostCenterId, int UserId, int BranchId);
        List<CostCenterTreeVM> GetCostCenterTree(string Lang, int BranchId);
        Task<CostCentersVM> GetCostCenterById(int costCenterId);
        Task<CostCentersVM> GetCostCenterByProId(int ProjectId);
        Task<CostCentersVM> GetCostCenterByCode(string Code, string lang, int BranchId);
        Task<IEnumerable<CostCentersVM>> GetAllCostCentersTransactions(string FromDate, string ToDate, string lang, int BranchId, int? yearid);
        Task<IEnumerable<CostCentersVM>> GetCostCenterReport(int BranchId, string lang, string FromDate, string ToDate, int? yearid);
        Task<IEnumerable<CostCentersVM>> GetCostCenterTransaction(int BranchId, string lang, int CostCenterId, string FromDate, string ToDate, int? yearid);
        Task<IEnumerable<CostCentersVM>> GetCostCenterAccountTransaction(int BranchId, string lang, int CostCenterId, string FromDate, string ToDate, int? yearid);

        Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCustId(string lang, int BranchId, int? custID);
        //heba 
        Task<DataTable> TreeViewOfCostCenter(string Con);
        Task<CostCentersVM> GetBranch_Costcenter(string lang, int BranchId);
    }
}
