using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAccountsRepository : IRepository<Accounts>
    {
        Task<List<AccountVM>> GetAllAccounts(string SearchText, string lang, int BranchId);
        Task<List<AccountVM>> GetAccountTreeIncome(string SearchText, string lang, int BranchId);

        Task<IEnumerable<AccountVM>> GetAllAccountsWithChild(int AccountID);

        Task<List<AccountVM>> GetAllAccountsCustomerBranch(string SearchText, string lang, int BranchId, int CustomerParentAcc);

        Task<IEnumerable<AccountVM>> GetAllAccountsOpening(string SearchText, string lang, int BranchId);

        Task<IEnumerable<AccountVM>> GetAllDelAccounts(string SearchText);
        Task<IEnumerable<AccountVM>> GetAllAccounts2(string SearchText, string lang, int BranchId);


        Task<AccountVM> GetAccountById(int accountId);
        Task<AccountVM> GetAccountByClassificationParent(int classification);

        Task<AccountVM> GetAccountByCode(string Code, string Lang, int BranchId);
        Task<int> GetMaxId();
        Task<IEnumerable<AccountVM>> GetAllAccountsTransactions(string FromDate, string ToDate, int YearId, string lang, int BranchId);
        Task<IEnumerable<AccountVM>> GetAllAccountsTransactionsByAccType(int AccType, string FromDate, string ToDate, int YearId, string lang, int BranchId);
        Task<IEnumerable<AccountVM>> GetAllSubAccounts(string SearchText, string lang, int BranchId);
        Task<IEnumerable<AccountVM>> GetAllReceiptExchangeAccounts(string SearchText, string lang, int BranchId, List<int> ReceiptExchangeAccIds);
        Task<IEnumerable<AccountVM>> GetAllHirearchialAccounts(int BranchId, string lang);
        Task<string> GetNewCodeByParentId(int ParentId,int Type);
        Task<IEnumerable<AccountVM>> GetAccountsByType(string accountName, string lang);
        Task<IEnumerable<AccountVM>> GetAccountSatement(int BranchId, string lang, int YearId, int AccountId, int CostCenterId, string FromDate, string ToDate);
        Task<IEnumerable<AccountVM>> GetGeneralBudget(int BranchId, string lang, int YearId, string FromDate, string ToDate);
        Task<IEnumerable<AccountVM>> GetGeneralLedger(int BranchId, string lang, int YearId, string FromDate, string ToDate);
        Task<IEnumerable<AccountVM>> GetGeneralLedgerDGV(int BranchId, string lang, int YearId, string FromDate, string ToDate, string Con);
        Task<IEnumerable<AccountVM>> GetGeneralBudgetDGV(int BranchId, string lang, int YearId, string FromDate, string ToDate, string Con);
        Task<IEnumerable<AccountVM>> GetCustomerFinancialDetails(int? AccountId, int YearId, int BranchId, string lang);
        Task<IEnumerable<AccountVM>> GetCustomerFinancialDetailsByFilter(int? CustomerId, string FromDate, string ToDate, int YearId, int BranchId, string lang, int ZeroCheck);

        //heba
        Task<DataTable> TreeView(string Con);
        Task<IEnumerable<AccountVM>> GetCustomerFinancialDetailsNew(int? AccountId, string FromDate, string ToDate, int Zerocheck, int YearId, int BranchId, string lang, string Con);
        Task<IEnumerable<AccountVM>> GetAccsTransByType(int Type, int BranchId, string lang, int YearId, string FromDate, string ToDate);
        Task<IEnumerable<AccountVM>> GetAccountSearchValue(string SearchName, int BranchId);
        Task<IEnumerable<AccountVM>> GetAllAccount(int BranchId);
        Task<IEnumerable<AccountVM>> GetAllAccountBySearch(AccountVM Account, int BranchId);

        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGV(string FromDate, string ToDate, string CCID, string Con);
        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con,int ZeroCheck, string AccountCode, string LVL, int FilteringType,string FilteringTypeStr,string AccountIds);
        Task<IEnumerable<TrainBalanceVM>> GetGeneralBudgetAMRDGVNew(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds);

        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL, int FilteringType,string FilteringTypeStr,string AccountIds);
        Task<IEnumerable<DetailsMonitorVM>> GetDetailsMonitor(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int FilteringType, string FilteringTypeStr, int AccountId,int Type, int Type2);

        Task<IEnumerable<CostCenterExpRevVM>> GetProjectExpRev(string CCID, string Con);

        Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGV(string FromDate, string ToDate, string CCID, string Con);
        Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con,int ZeroCheck, string LVL, int? taxID, int? taxID2, int? GeneralAdmExpenses, int? DepFixedAssets, int? AccountID_Mb, int? AccountID_MR);
        Task<IEnumerable<IncomeStatmentVMWithLevels>> GetIncomeStatmentDGVLevels(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string LVL, int FilteringType,int FilteringTypeAll, string FilteringTypeStr,string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter,int TypeF);

        Task<IEnumerable<GeneralBudgetVM>> GetGeneralBudgetAMRDGV(string FromDate, string ToDate,string LVL, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck);
        //heba
        Task<DataTable> GetGeneralBudgetFRENCHDGV(string FromDate, string ToDate,string LVL, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck);
                                      
        Task<IEnumerable<AccountStatmentVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId,int  YearId);

  
        Task<IEnumerable<GeneralmanagerRevVM>> GetGeneralManagerRevenueAMRDGV(int? ManagerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid, int? taxID, int? taxID2);
        Task<IEnumerable<ClosingVouchers>> GetClosingVouchers(int BranchId, string Con, int? yearid);
        Task<IEnumerable<CostCenterEX_REVM>> GetCostCenterEX_RE(int? CostCenterId, string FromDate, string ToDate, int BranchId, string Con, int? yearid, int? taxID, int? taxID2, int? AccountID_MR, string FlagTotal);

        Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenu(int? CustomerId, string FromDate, string ToDate,int BranchId, string Con, int? yearid,int? taxID, int? taxID2, int? AccountID_Mb, int? AccountID_MR);
        Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenuExtra(int? CustomerId, int? ProjectId, string FromDate, string ToDate, int BranchId, string Con, int? yearid, int? taxID, int? taxID2);
        Task<IEnumerable<InvoicedueC>> GetInvoicedue(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid);

        Task<IEnumerable<DetailedExpenseVM>> GetDetailedExpensesd(int? AccountId, string FromDate, string ToDate, string ExpenseType, int BranchId, string Con, int? yearid, int? taxID, int? taxID2);
        Task<IEnumerable<IncomeStatmentVM>> GetALLIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int YearId, string lang, string Con, int ZeroCheck, string LVL);
        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew_old(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL);

        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2_old(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL);
        Task<IEnumerable<DetailsMonitorVM>> GetIncomeStatmentDGVLevelsdetails(int Accountid, int Type, int Type2, string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF);
        Accounts GetById(int AccountId);

    }
}
