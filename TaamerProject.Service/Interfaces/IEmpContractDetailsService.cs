using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IEmpContractDetailsService  
    {
        Task<IEnumerable<EmpContractDetailVM>> GetAllEmpConDetailsByContractId(int? ContractId);
        //IEnumerable<TransactionsVM> GetAllTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int? yearid);
        //IEnumerable<TransactionsVM> GetAllTransactionsSearch(TransactionsVM TransactionsSearch, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetAllTransSearch(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetAllTransSearchByAccIDandCostId(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetAllTransactions(string FromDate, string ToDate, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetAllTransactionsByAccType(int accType,string FromDate,string ToDate, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetAllTransByCustomerId(int? CustomerId,string FromDate, string ToDate, int? yearid);
        //IEnumerable<TransactionsVM> GetAllCustTrans(string FromDate, string ToDate, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetAllCostCenterTrans(string FromDate, string ToDate, int BranchId, int? yearid);
        //List<double> GetValueNeeded(int BranchId, string lang, string FromDate, string ToDate, string Con, int? yearid);
        //IEnumerable<TransactionsVM> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int? yearid);
    }
}
