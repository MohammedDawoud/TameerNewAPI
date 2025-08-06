using System.Collections.Generic;
using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Generic;

namespace TaamerProject.Service.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly IAccountsRepository _AccountsRepository;
        private readonly IFiscalyearsRepository _fiscalyearsRepository;
        private readonly ICostCenterRepository _CostCenterRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public TransactionsService(ITransactionsRepository transactionsRepository, IAccountsRepository accountsRepository
            , IFiscalyearsRepository fiscalyearsRepository, ICostCenterRepository costCenterRepository
            ,IBranchesRepository branchesRepository, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _TransactionsRepository = transactionsRepository; _AccountsRepository = accountsRepository;
            _fiscalyearsRepository = fiscalyearsRepository; _CostCenterRepository = costCenterRepository;
            _BranchesRepository = branchesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransByAccountId(int? AccountId,string FromDate, string ToDate, int? yearid)
        {
            var details = _TransactionsRepository.GetAllTransByAccountId(AccountId, FromDate, ToDate, yearid ?? 0);
            return details;
        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int? yearid)
        {
            //var CostCenter = _CostCenterRepository.GetById(CostCenterId);
            var CostCenter = _TaamerProContext.CostCenters.Where(s=>s.CostCenterId== CostCenterId)?.FirstOrDefault();
            if(CostCenter!=null)
            {
                if (CostCenter.ParentId != null)
                {
                    return _TransactionsRepository.GetAllTransByCostCenter(CostCenterId, FromDate, ToDate, yearid ?? 0);
                }
                else
                {
                    return _TransactionsRepository.GetAllSubCostTransByCostCenter(CostCenterId, FromDate, ToDate, yearid ?? 0);
                }
            }
            return _TransactionsRepository.GetAllSubCostTransByCostCenter(CostCenterId, FromDate, ToDate, yearid ?? 0);

        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransactionsSearch(TransactionsVM TransactionsSearch, int BranchId, int? yearid)
        {
            var IsParentAcc = _AccountsRepository.GetById(TransactionsSearch.AccountId??0).IsMain;
            if (IsParentAcc == false)
            {
                return _TransactionsRepository.GetAllTransactionsSearch(TransactionsSearch, yearid ?? 0, BranchId);
            } 
            else
            {
                return _TransactionsRepository.GetAllSubAccTransactionsSearch(TransactionsSearch, yearid ?? 0, BranchId);
            }
        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransSearch(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid)
        {
            return _TransactionsRepository.GetAllTransSearch(AccountId, FromDate, ToDate, CostCenterId, yearid ?? 0, BranchId);

        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransSearch_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid, bool? isCheckedBranch)
        {
            if(AccountId ==null || AccountId == 0) {
            var Fatra = _TransactionsRepository.GetAllTransSearch_New(AccountId, FromDate, ToDate, CostCenterId, yearid ?? 0, BranchId, isCheckedBranch);
            return Fatra;
            }
            else
            {
                var Fatra = _TransactionsRepository.GetAllTransSearch_New_withChild(AccountId, FromDate, ToDate, CostCenterId, yearid ?? 0, BranchId, isCheckedBranch);

                return Fatra;
            }
        }

        public Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid)
        {
            return _TransactionsRepository.GetAllTransSearchByAccIDandCostId(AccountId, FromDate, ToDate, CostCenterId, yearid ?? 0, BranchId);
        }

        public Task<IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid)
        {
            if(AccountId ==null || AccountId == 0) { 
            return _TransactionsRepository.GetAllTransSearchByAccIDandCostId_New(AccountId, FromDate, ToDate, CostCenterId, yearid ?? 0, BranchId);
            }
            else
            {
                return _TransactionsRepository.GetAllTransSearchByAccIDandCostId_New_whithchild(AccountId, FromDate, ToDate, CostCenterId, yearid ?? 0, BranchId);
            }
        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransactions(string FromDate, string ToDate, int BranchId, int? yearid)
        {
            return  _TransactionsRepository.GetAllTransactions( FromDate,  ToDate, yearid ?? 0, BranchId);
        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransactionsByAccType(int accType,string FromDate, string ToDate, int BranchId, int? yearid)
        {
            return _TransactionsRepository.GetAllTransactionsByAccType(accType, FromDate, ToDate, yearid ?? 0, BranchId);
        }
        public Task<IEnumerable<TransactionsVM>> GetAllTransByCustomerId(int? CustomerId, string FromDate, string ToDate, int? yearid)
        {
            return _TransactionsRepository.GetAllTransByCustomerId(CustomerId,FromDate, ToDate, yearid ?? 0);
        }
        public Task<IEnumerable<TransactionsVM>> GetAllCustTrans(string FromDate, string ToDate, int BranchId, int? yearid)
        {
            return _TransactionsRepository.GetAllCustomerTrans(FromDate, ToDate, yearid ?? 0, BranchId);
        }
        public Task<IEnumerable<TransactionsVM>> GetAllCostCenterTrans(string FromDate, string ToDate, int BranchId, int? yearid)
        {
            return _TransactionsRepository.GetAllCostCenterTrans(FromDate, ToDate, yearid ?? 0, BranchId);
        }

        public Task<List<double>> GetValueNeeded(int BranchId, string lang, string FromDate, string ToDate,string Con, int? yearid)
        {
            //Task<List<double>> lmd = new Task<List<double>>();
            Task<List<double>> lmd = Task.FromResult<List<double>>(new List<double>());
            var Branch = _BranchesRepository.GetById(BranchId);
            if (Branch == null || Branch.TaxsAccId == null)
            {
                return lmd;
            }
            else if (Branch == null || Branch.SuspendedFundAccId == null)
            {
                return lmd;
            }
            else if (Branch == null || Branch.ContractsAccId == null)
            {
                return lmd;
            }
            else if (Branch == null || Branch.PurchaseReturnCashAccId == null)
            {
                return lmd;
            }
            return _TransactionsRepository.GetValueNeeded(BranchId, lang, yearid ?? 0, FromDate, ToDate, Con, Branch.TaxsAccId, Branch.SuspendedFundAccId, Branch.ContractsAccId, Branch.PurchaseReturnCashAccId);
        }

        public Task<IEnumerable<TransactionsVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int? yearid)
        {
            if (yearid != null)
            {
                return _TransactionsRepository.GetFullAccountStatmentDGV(FromDate, ToDate, AccountCode, CCID, Con, BranchId, yearid ?? 0);
            }
            //return new List<TransactionsVM>();
            return Task.FromResult<IEnumerable<TransactionsVM>>(new List<TransactionsVM>());
        }

        public Task<IEnumerable<TransactionsVM>> gettransbyid(int? jornal)
        {
            return _TransactionsRepository.gettransbyid(jornal);

        }
        public IEnumerable<object> GetAccCredit_Depit(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Credit = row[0].ToString(),
                Depit = row[1].ToString(),
            });
        }
    }
}
