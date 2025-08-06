using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<IEnumerable<TransactionsVM>> GetAllTransByVoucherId(int? voucherId);
        Task<IEnumerable<TransactionsVM>> GetAllTransByAccountId(int? AccountId, string FromDate, string ToDate, int YearId);
        Task<IEnumerable<TransactionsVM>> GetAllTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int YearId);
        Task<IEnumerable<TransactionsVM>> GetAllSubCostTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int YearId);
        Task<IEnumerable<TransactionsVM>> GetAllTransactionsSearch(TransactionsVM TransactionsSearch, int YearId,int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearch(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearch_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId,bool? isCheckedBranch);

        Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId);

        Task<IEnumerable<TransactionsVM>> GetAllSubAccTransactionsSearch(TransactionsVM TransactionsSearch, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllTransCustomers(int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllTransByCustomerId(int? CustomerId, string FromDate, string ToDate, int YearId);
        Task<IEnumerable<TransactionsVM>> GetAllTransactions(string FromDate, string ToDate, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllTransactionsByAccType(int AccType, string FromDate, string ToDate, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllCustomerTrans(string FromDate, string ToDate, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllCostCenterTrans(string FromDate, string ToDate, int YearId, int BranchId);
        Task<List<double>> GetValueNeeded(int BranchId, string lang, int YearId, string FromDate, string ToDate, string Con, int? taxID, int? taxID2, int? AccountID_Mb, int? AccountID_MR);

        Task<IEnumerable<TransactionsVM>> GetAllJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvID(int? invId,  int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDPurchase(int? invId, int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDPurchaseOrder(int? invId, int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByReVoucherID(int? invId, int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByPayVoucherID(int? invId, int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByDailyID(int? invId, int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByDailyID_Custody(int? invId, int? YearId, int BranchId);

        Task<IEnumerable<TransactionsVM>> GetAllJournalsByClosingID(int? invId, int? YearId, int BranchId);


        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDRet(int? invId, int? YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDCreditDepitNoti(int? invId, int? YearId, int BranchId);

        Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDRetPurchase(int? invId, int? YearId, int BranchId);

        Task<IEnumerable<TransactionsVM>> GetAllPayJournalsByInvIDRet(int? invId, int? YearId, int BranchId);


        Task<IEnumerable<TransactionsVM>> GetAllTotalJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate, int YearId, int BranchId);
        Task<IEnumerable<TransactionsVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int YearId);
        Task<IEnumerable<TransactionsVM>> gettransbyid(int? jornal);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearch_New_withChild(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId, bool? isCheckedBranch);
        Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId_New_whithchild(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId);


    }
}
