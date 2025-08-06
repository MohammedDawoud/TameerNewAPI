using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaamerProject.Service.Interfaces
{
    public interface ITransactionsService
    {
        Task<IEnumerable<TransactionsVM>> GetAllTransByAccountId(int? AccountId, string FromDate, string ToDate, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransactionsSearch(TransactionsVM TransactionsSearch, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearch(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearch_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid, bool? isCheckedBranch);

        Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid);

        Task<IEnumerable<TransactionsVM>> GetAllTransactions(string FromDate, string ToDate, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransactionsByAccType(int accType,string FromDate,string ToDate, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllTransByCustomerId(int? CustomerId,string FromDate, string ToDate, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllCustTrans(string FromDate, string ToDate, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllCostCenterTrans(string FromDate, string ToDate, int BranchId, int? yearid);
        Task<List<double>> GetValueNeeded(int BranchId, string lang, string FromDate, string ToDate, string Con, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int? yearid);
        //IEnumerable<TransactionsVM> GetProjectManagerRevene(int? ManagerId, string dateFrom, string dateTo, int BranchId, int? yearid);
        Task<IEnumerable<TransactionsVM>> gettransbyid(int? jornal);
        IEnumerable<object> GetAccCredit_Depit(string Con, string SelectStetment);

    }
}
