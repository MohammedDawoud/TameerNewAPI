using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class EmpContractDetailsService :   IEmpContractDetailsService
    {
        private readonly IEmpContractDetailRepository _EmpContractDetailRepository;
        private readonly IAccountsRepository _AccountsRepository;
        private readonly IFiscalyearsRepository _fiscalyearsRepository;
        private readonly ICostCenterRepository _CostCenterRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public EmpContractDetailsService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IEmpContractDetailRepository EmpContractDetailRepository,
            IAccountsRepository AccountsRepository, IFiscalyearsRepository fiscalyearsRepository,
            ICostCenterRepository CostCenterRepository
            )
        {
            _EmpContractDetailRepository = EmpContractDetailRepository;
            _AccountsRepository = AccountsRepository;
            _fiscalyearsRepository = fiscalyearsRepository;
            _CostCenterRepository = CostCenterRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<EmpContractDetailVM>> GetAllEmpConDetailsByContractId(int? ContractId)
        {
            var details = _EmpContractDetailRepository.GetAllEmpConDetailsByContractId(ContractId);
            return details; 
        }
        //public IEnumerable<TransactionsVM> GetAllTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int? yearid)
        //{
        //    var CostCenter = _CostCenterRepository.GetById(CostCenterId);
        //    if (CostCenter.ParentId != null)
        //    {
        //      return  _TransactionsRepository.GetAllTransByCostCenter(CostCenterId, FromDate, ToDate, yearid ?? default(int));
        //    }
        //    else
        //    {
        //      return  _TransactionsRepository.GetAllSubCostTransByCostCenter(CostCenterId, FromDate, ToDate, yearid ?? default(int));
        //    }
        //}
        //public IEnumerable<TransactionsVM> GetAllTransactionsSearch(TransactionsVM TransactionsSearch, int BranchId, int? yearid)
        //{
        //    var IsParentAcc = _AccountsRepository.GetById(TransactionsSearch.AccountId).IsMain;
        //    if (IsParentAcc == false)
        //    {
        //        return _TransactionsRepository.GetAllTransactionsSearch(TransactionsSearch, yearid ?? default(int), BranchId);
        //    } 
        //    else
        //    {
        //        return _TransactionsRepository.GetAllSubAccTransactionsSearch(TransactionsSearch, yearid ?? default(int), BranchId);
        //    }
        //}
        //public IEnumerable<TransactionsVM> GetAllTransSearch(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid)
        //{
        //    var IsParentAcc = _AccountsRepository.GetById(AccountId).IsMain;
        //    if (IsParentAcc == false)
        //    {
        //        return _TransactionsRepository.GetAllTransSearch(AccountId, FromDate, ToDate, CostCenterId, yearid ?? default(int), BranchId);
        //    }
        //    else
        //    {
        //        return _TransactionsRepository.GetAllTransByAccountId(AccountId, FromDate, ToDate, yearid ?? default(int));
        //    }
        //}

        //public IEnumerable<TransactionsVM> GetAllTransSearchByAccIDandCostId(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int BranchId, int? yearid)
        //{
        //    var IsParentAcc = _AccountsRepository.GetById(AccountId).IsMain;
        //    if (IsParentAcc == false)
        //    {
        //        return _TransactionsRepository.GetAllTransSearchByAccIDandCostId(AccountId, FromDate, ToDate, CostCenterId, yearid ?? default(int), BranchId);
        //    }
        //    else
        //    {
        //        return _TransactionsRepository.GetAllTransByAccountId(AccountId, FromDate, ToDate, yearid ?? default(int));
        //    }
        //}
        //public IEnumerable<TransactionsVM> GetAllTransactions(string FromDate, string ToDate, int BranchId, int? yearid)
        //{
        //    return  _TransactionsRepository.GetAllTransactions( FromDate,  ToDate, yearid ?? default(int), BranchId);
        //}
        //public IEnumerable<TransactionsVM> GetAllTransactionsByAccType(int accType,string FromDate, string ToDate, int BranchId, int? yearid)
        //{
        //    return _TransactionsRepository.GetAllTransactionsByAccType(accType, FromDate, ToDate, yearid ?? default(int), BranchId);
        //}
        //public IEnumerable<TransactionsVM> GetAllTransByCustomerId(int? CustomerId, string FromDate, string ToDate, int? yearid)
        //{
        //    return _TransactionsRepository.GetAllTransByCustomerId(CustomerId,FromDate, ToDate, yearid ?? default(int));
        //}
        //public IEnumerable<TransactionsVM> GetAllCustTrans(string FromDate, string ToDate, int BranchId, int? yearid)
        //{
        //    return _TransactionsRepository.GetAllCustomerTrans(FromDate, ToDate, yearid ?? default(int), BranchId);
        //}
        //public IEnumerable<TransactionsVM> GetAllCostCenterTrans(string FromDate, string ToDate, int BranchId, int? yearid)
        //{
        //    return _TransactionsRepository.GetAllCostCenterTrans(FromDate, ToDate, yearid ?? default(int), BranchId);
        //}

        //public List<double> GetValueNeeded(int BranchId, string lang, string FromDate, string ToDate,string Con, int? yearid)
        //{
        //    return _TransactionsRepository.GetValueNeeded(BranchId, lang, yearid ?? default(int), FromDate, ToDate, Con);
        //}

        //public IEnumerable<TransactionsVM> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int? yearid)
        //{
        //    //var year = _fiscalyearsRepository.GetCurrentYear();
        //    if (yearid != null)
        //    {
        //        return _TransactionsRepository.GetFullAccountStatmentDGV(FromDate, ToDate, AccountCode, CCID, Con, BranchId, yearid ?? default(int));
        //    }
        //    return new List<TransactionsVM>();
        //    //return null;
        //}
    }
}
