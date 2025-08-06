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
    public interface IAccountsService
    {
        Task<List<AccountVM>> GetAllAccounts(string SearchText, string lang, int BranchId);
        Task<List<AccountVM>> GetAllAccountsCustomerBranch(string SearchText, string lang, int BranchId);

       Task< IEnumerable<AccountVM>> GetAllAccountsOpening(string SearchText, string lang, int BranchId);

       Task< IEnumerable<AccountVM>> GetAllAccounts2(string SearchText, string lang, int BranchId);

       Task<AccountVM>GetCustMainAccByBranchId(int BranchId);
       Task< IEnumerable<AccountVM>> GetAllSubAccounts(string SearchText, string lang, int BranchId);
        GeneralMessage SaveAccount(Accounts account, int UserId, int BranchId, int yearid);
        GeneralMessage DeleteAccount(int AccountId, int UserId, int BranchId);
        List<AccountTreeVM> GetAccountTree(string Lang, int BranchId);
        List<AccountTreeVM> GetAccountTreeIncome(string Lang, int BranchId);

        GeneralMessage SaveAccountTree(List<int> Privs, int UserId, int BranchId, string Con);
        GeneralMessage SaveAccountTreeEA(List<int> Privs, int UserId, int BranchId, string Con);

        GeneralMessage SaveAccountTreeotherrev(List<int> Privs, int UserId, int BranchId);
        GeneralMessage SaveAccountTreePublicRev(List<int> Privs, int UserId, int BranchId);

        List<int> GetAccountTreeKD();
        List<int> GetAccountTreeEA();

        List<int> GetAccountTreepublicrev();
        List<int> GetAccountTreeotherrev();

       Task<AccountVM>GetAccountById(int accountId);
       Task<AccountVM>GetAccountByClassificationParent(int classification);

       Task<AccountVM>GetAccountByCode(string Code, string Lang, int BranchId);
        Task<IEnumerable<object>> FillCustAccountsSelect(string lang, int BranchId, int param);

        Task<IEnumerable<object>> FillCustAccountsSelect2(string lang, int BranchId, int param);

        Task<IEnumerable<object>> FillSubAccountLoad(string lang, int BranchId);
        string GetAccCodeFormID(int AccID, string lang, int BranchId);


        Task<IEnumerable<object>> FillAccountSelect(string Con, string SelectStetment);
        Task<IEnumerable<object>> FillAccountNewSelect(string Con, string SelectStetment);

        Task<IEnumerable<object>> FillYearsSelect(string Con, string SelectStetment);
        Task<IEnumerable<object>> GetNetValue(string Con, string SelectStetment);

        Task<IEnumerable<object>> FillAccountSelectPurchase(string Con, string SelectStetment);

        IEnumerable<AccountVM> FillAccountSelect2(int AccountID);

        Task<IEnumerable<object>> FillEmpAccountsSelect(string lang, int BranchId);
        GeneralMessage TransFerAccounts(int FromBranchId, int ToBranchId, int UserId);
       Task< IEnumerable<AccountVM>> GetAccountsByType(string accountName, string lang);
       Task< IEnumerable<AccountVM>> GetAllAccountsTransactions(string FromDate, string ToDate, string lang, int BranchId, int? yearid);
       Task< IEnumerable<AccountVM>> GetAllAccountsTransactionsByAccType(int AccType, string FromDate, string ToDate, string lang, int BranchId, int? yearid);
        bool CheckAccTreeExist(int ToBranchId);
       Task< IEnumerable<AccountVM>> GetAllReceiptExchangeAccounts(string SearchText, string lang, int BranchId, int Type);
       Task< IEnumerable<AccountVM>> GetAllHirearchialAccounts(int BranchId, string lang);
       Task< IEnumerable<AccountVM>> GetAccountSatement(int BranchId, string lang, int AccountId, int CostCenterId, string FromDate, string ToDate, int? yearid);
       Task< IEnumerable<AccountVM>> GetGeneralBudget(int BranchId, string lang, string FromDate, string ToDate, int? yearid);
       Task< IEnumerable<AccountVM>> GetGeneralLedger(int BranchId, string lang, string FromDate, string ToDate, int? yearid);
       Task< IEnumerable<AccountVM>> GetGeneralLedgerDGV(int BranchId, string lang, string FromDate, string ToDate, string Con, int? yearid);
       Task< IEnumerable<AccountVM>> GetGeneralBudgetDGV(int BranchId, string lang, string FromDate, string ToDate, string Con, int? yearid);
       Task< IEnumerable<AccountVM>> GetCustomerFinancialDetails(int BranchId, string lang, int? yearid);
       Task< IEnumerable<AccountVM>> GetCustomerFinancialDetailsByFilter(int? CustomerId, string FromDate, string ToDate, int BranchId, string lang, int? yearid, int ZeroCheck);

        //heba
        Task<DataTable> TreeView(String Con);
       Task< IEnumerable<AccountVM>> GetCustomerFinancialDetailsNew(int? CustomerId, string FromDate, string ToDate, int Zerocheck, int BranchId, string lang, int? yearid, string Con);
       Task< IEnumerable<AccountVM>> GetAccsTransByType(int Type, int BranchId, string lang, string FromDate, string ToDate, int? yearid);
        ProfitAndLossesVM GetAccsProfitAndLosses(int BranchId, string lang, string FromDate, string ToDate, int? yearid);
       Task< IEnumerable<AccountVM>> GetAccountSearchValue(string searchName, int BranchId);
       Task< IEnumerable<AccountVM>> GetAllAccount(int BranchId);
       Task< IEnumerable<AccountVM>> GetAllAccountBySearch(AccountVM Account, int BranchId);

        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGV(string FromDate, string ToDate, string CCID, string Con);


       Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds);
        Task<IEnumerable<TrainBalanceVM>> GetGeneralBudgetAMRDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds);

        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds);
        Task<IEnumerable<DetailsMonitorVM>> GetDetailsMonitor(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int FilteringType, string FilteringTypeStr, int AccountId, int Typee, int Type2);

       Task<IEnumerable<CostCenterExpRevVM>> GetProjectExpRev(string CCID, string Con);

       Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGV(string FromDate, string ToDate, string CCID, string Con);
       Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL);

       Task<IEnumerable<IncomeStatmentVMWithLevels>> GetIncomeStatmentDGVLevels(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF);

       Task<IEnumerable<GeneralBudgetVM>> GetGeneralBudgetAMRDGV(string FromDate, string ToDate, string LVL, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck);
        //heba
       Task<DataTable> GetGeneralBudgetFRENCHDGV(string FromDate, string ToDate, string LVL, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck);

        Task<IEnumerable<AccountStatmentVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int? yearid);
        Task<string> GetNewCodeByParentId(int ParentId, int Type);
        Task<IEnumerable<GeneralmanagerRevVM>> GetGeneralManagerRevenueAMRDGV(int? ManagerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid);
       Task<IEnumerable<ClosingVouchers>> GetClosingVouchers(int BranchId, string Con, int? yearid);
       Task<IEnumerable<CostCenterEX_REVM>> GetCostCenterEX_RE(int? CostCenterId, string FromDate, string ToDate, int BranchId, string Con, int? yearid, string FlagTotal);

       Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenu(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid);
       Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenuExtra(int? CustomerId, int? ProjectId, string FromDate, string ToDate, int BranchId, string Con, int? yearid);
        Task<IEnumerable<InvoicedueC>> GetInvoicedue(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? yearid);

        Task<IEnumerable<DetailedExpenseVM>> GetDetailedExpensesd(int? AccountId, string FromDate, string ToDate, string ExpenseType, int BranchId, string Con, int? yearid);

       Task<IEnumerable<IncomeStatmentVM>> GetAllIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL);
        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew_old(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL);
        Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2_old(string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string AccountCode, string LVL);

      Task<IEnumerable<DetailsMonitorVM>> GetIncomeStatmentDGVLevelsdetails(int AccountId, int type, int type2, string FromDate, string ToDate, int CCID, int BranchId, string lang, string Con, int? yearid, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF);

    }
}
