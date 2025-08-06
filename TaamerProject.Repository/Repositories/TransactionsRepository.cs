using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using System.Data.SqlClient;
using System.Data;
using TaamerProject.Models.Enums;

namespace TaamerProject.Repository.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TransactionsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransByVoucherId(int? voucherId)
        {
            var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.InvoiceId == voucherId && s.Type!=17).Select(x => new TransactionsVM
            {
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                Amounttax = x.Amounttax??null,
                DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                AccountName = x.Accounts!=null? x.Accounts.Code+" - "+x.Accounts.NameAr:"",
                CostCenterId = x.CostCenterId,
                Depit = x.Depit,
                Credit = x.Credit,
                CostCenterName = x.CostCenters != null ? x.CostCenters.Code +" - "+x.CostCenters.NameAr: "",
                InvoiceReference = x.InvoiceReference ?? "",
                Notes = x.Notes ?? "",
                AccCalcExpen=x.AccCalcExpen??false,
                AccCalcIncome = x.AccCalcIncome ?? false,
                Classification = x.Accounts != null ? x.Accounts.Classification:0,
                //AccCalcAll= x.AccCalcIncome==true?true:x.AccCalcExpen==true?true:false,
                AccCalcAll = x.AccCalcIncome == true ? "O" : x.AccCalcExpen == true ? "I" : "0",


            });
            return details;
        }
       
        public async Task< IEnumerable<TransactionsVM>> GetAllTransByVoucherId2(int? voucherId)
        {
            var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.InvoiceId == voucherId).Select(x => new TransactionsVM
            {
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                Amounttax = x.Amounttax ?? null,
                DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                AccountName =x.Accounts!=null?x.Accounts.NameAr:"",
                CostCenterId = x.CostCenterId,
                Depit = x.Depit,
                Credit = x.Credit,
                CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                InvoiceReference = x.InvoiceReference ?? "",
                Notes = x.Notes ?? "",
                Classification = x.Accounts != null ? x.Accounts.Classification : 0,
                //AccCalcAll = x.AccCalcIncome == true ? true : x.AccCalcExpen == true ? true : false,
                AccCalcAll = x.AccCalcIncome == true ? "O" : x.AccCalcExpen == true ? "I" : "0",

            });
            return details;
        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransByAccountId(int? AccountId, string FromDate, string ToDate,int YearId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == AccountId && s.Type!=12 && s.YearId == YearId).Select(x => new
                {
                    x.TransactionId,
                    InvoiceId=x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    x.Accounts.Code,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    x.JournalNo,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.AccTransactionTypes!=null? x.AccTransactionTypes.NameAr:"",
                    x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    IsPost = x.IsPost,
                    
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    x.Type,
                    PayType = x.Invoices!=null?x.Invoices.PayType:0,

                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    JournalNo=s.JournalNo??0,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    AccountCode = s.Code,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    Balance = s.Depit - s.Credit,
                    TransactionDate = s.TransactionDate,
                    Type=s.Type,
                    IsPost = s.IsPost,
                    PayType=s.PayType,

                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).OrderByDescending(a=>a.TransactionDate).ToList();
            }
            catch (Exception)
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == AccountId && s.YearId == YearId).Select(x => new TransactionsVM
                {
                    TransactionId = x.TransactionId,
                    InvoiceId = x.InvoiceId,
                    LineNumber = x.LineNumber,
                    AccountId = x.AccountId,
                    JournalNo = x.JournalNo??0,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterId = x.CostCenterId,
                    Depit = x.Depit??0,
                    Credit = x.Credit??0,
                    InvoiceReference = x.InvoiceReference ?? "",
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    TransactionDate = x.TransactionDate,
                    Type=x.Type,
                    IsPost = x.IsPost,
                    PayType = x.Invoices != null ? x.Invoices.PayType : 0,


                }).ToList().OrderByDescending(s=>s.TransactionDate).ToList();
            }
        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int YearId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterId && s.YearId == YearId).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    Balance = s.Depit - s.Credit,
                    TransactionDate = s.TransactionDate,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(); 
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterId && s.YearId == YearId).Select(x => new TransactionsVM
                {
                    TransactionId = x.TransactionId,
                    InvoiceId = x.InvoiceId,
                    LineNumber = x.LineNumber,
                    AccountId = x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterId = x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    InvoiceReference = x.InvoiceReference ?? "",
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    TransactionDate = x.TransactionDate,
                });
                return details;
            }
           
        }
        public async Task< IEnumerable<TransactionsVM>> GetAllSubCostTransByCostCenter(int? CostCenterId, string FromDate, string ToDate, int YearId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CostCenters.ParentId == CostCenterId  && s.YearId == YearId).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    Balance = s.Depit - s.Credit,
                    TransactionDate = s.TransactionDate,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CostCenters.ParentId == CostCenterId && s.YearId == YearId).Select(x => new TransactionsVM
                {
                    TransactionId = x.TransactionId,
                    InvoiceId = x.InvoiceId,
                    LineNumber = x.LineNumber,
                    AccountId = x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterId = x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    InvoiceReference = x.InvoiceReference ?? "",
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    TransactionDate = x.TransactionDate,
                });
                return details;
            }

        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransByCustomerId(int? CustomerId, string FromDate, string ToDate,int YearId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CustomerId == CustomerId && s.YearId == YearId).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    CustomerName =x.Customer!=null? x.Customer.CustomerNameAr:"",
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    DebitBalance = (s.Depit - s.Credit) > 0 ? (s.Depit - s.Credit) : 0,
                    CreditBalance = (s.Credit - s.Depit) > 0 ? (s.Credit - s.Depit) : 0,
                    TransactionDate = s.TransactionDate,
                    CustomerName = s.CustomerName,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
            }
            catch (Exception)
            {

                var CustTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CustomerId == CustomerId && s.YearId == YearId
               ).Select(x => new TransactionsVM
               {
                   TransactionId = x.TransactionId,
                   InvoiceId = x.InvoiceId,
                   LineNumber = x.LineNumber,
                   AccountId = x.AccountId,
                   Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                   DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                   AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                   CostCenterId = x.CostCenterId,
                   Depit = x.Depit ?? 0,
                   Credit = x.Credit ?? 0,
                   InvoiceReference = x.InvoiceReference ?? "",
                   Notes = x.Notes ?? "",
                   DebitBalance = (x.Depit - x.Credit) > 0 ? (x.Depit - x.Credit) : 0,
                   CreditBalance = (x.Credit - x.Depit) > 0 ? (x.Credit - x.Depit) : 0,
                   CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                   TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                   TransactionDate = x.TransactionDate,
                   CustomerName =x.Customer!=null?x.Customer.CustomerNameAr:"",
               });
                return CustTrans;
            }
          
        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransactionsSearch(TransactionsVM TransactionsSearch, int YearId,int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                }).ToList();

                if (!String.IsNullOrEmpty(Convert.ToString(TransactionsSearch.AccountId)))
                {
                    details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/ && ((s.AccountId == TransactionsSearch.AccountId ) )).Select(x => new
                                                               {
                                                                   x.TransactionId,
                                                                   x.InvoiceId,
                                                                   x.LineNumber,
                                                                   x.AccountId,
                                                                   Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                   DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                   x.CostCenterId,
                                                                    Depit = x.Depit ?? 0,
                                                                    Credit = x.Credit ?? 0,

                                                                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                        x.InvoiceReference,
                                                                   Notes = x.Notes ?? "",
                                                                   Balance = x.Depit - x.Credit,
                                                                   x.TransactionDate,
                                                               }).Select(s => new TransactionsVM
                                                               {
                                                                   TransactionId = s.TransactionId,
                                                                   InvoiceId = s.InvoiceId,
                                                                   LineNumber = s.LineNumber,
                                                                   AccountId = s.AccountId,
                                                                   Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                                                                   DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                                                                   AccountName = s.AccountName,
                                                                   CostCenterId = s.CostCenterId,
                                                                   Depit = s.Depit,
                                                                   Credit = s.Credit,
                                                                   CostCenterName = s.CostCenterName ?? "",
                                                                   InvoiceReference = s.InvoiceReference ?? "",
                                                                   Notes = s.Notes,
                                                                   Balance = s.Balance,
                                                                   TypeName = s.TypeName,
                                                                   TransactionDate = s.TransactionDate,
                                                               }).ToList();


                }
                if(!String.IsNullOrEmpty(Convert.ToString(TransactionsSearch.CostCenterId)))
                {
                    details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/ && (
                                                                                    s.CostCenterId == TransactionsSearch.CostCenterId )).Select(x => new
                                                                                    {
                                                                                        x.TransactionId,
                                                                                        x.InvoiceId,
                                                                                        x.LineNumber,
                                                                                        x.AccountId,
                                                                                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                                        x.CostCenterId,
                                                                                        Depit = x.Depit ?? 0,
                                                                                        Credit = x.Credit ?? 0,
                                                                                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                                                                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                                                                                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                                                                                        x.InvoiceReference,
                                                                                        Notes = x.Notes ?? "",
                                                                                        Balance = x.Depit - x.Credit,
                                                                                        x.TransactionDate,
                                                                                    }).Select(s => new TransactionsVM
                                                                                    {
                                                                                        TransactionId = s.TransactionId,
                                                                                        InvoiceId = s.InvoiceId,
                                                                                        LineNumber = s.LineNumber,
                                                                                        AccountId = s.AccountId,
                                                                                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                                                                                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                                                                                        AccountName = s.AccountName,
                                                                                        CostCenterId = s.CostCenterId,
                                                                                        Depit = s.Depit,
                                                                                        Credit = s.Credit,
                                                                                        CostCenterName = s.CostCenterName ?? "",
                                                                                        InvoiceReference = s.InvoiceReference ?? "",
                                                                                        Notes = s.Notes,
                                                                                        Balance = s.Balance,
                                                                                        TypeName = s.TypeName,
                                                                                        TransactionDate = s.TransactionDate,
                                                                                    }).ToList();



                }
                if(!String.IsNullOrEmpty(Convert.ToString(TransactionsSearch.DateFrom)) &&!String.IsNullOrEmpty(Convert.ToString(TransactionsSearch.DateTo)) &&(DateTime.ParseExact(TransactionsSearch.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)>=DateTime.ParseExact(TransactionsSearch.DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {

              
                details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/ ).Select(x => new
                                                                 {
                                                                     x.TransactionId,
                                                                     x.InvoiceId,
                                                                     x.LineNumber,
                                                                     x.AccountId,
                                                                     Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                     DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                     x.CostCenterId,
                                                                    Depit = x.Depit ?? 0,
                                                                    Credit = x.Credit ?? 0,
                                                                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                                                                     Notes = x.Notes ?? "",
                                                                     Balance = x.Depit - x.Credit,
                                                                     x.TransactionDate,
                                                                 }).Select(s => new TransactionsVM
                                                                 {
                                                                     TransactionId = s.TransactionId,
                                                                     InvoiceId = s.InvoiceId,
                                                                     LineNumber = s.LineNumber,
                                                                     AccountId = s.AccountId,
                                                                     Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                                                                     DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                                                                     AccountName = s.AccountName,
                                                                     CostCenterId = s.CostCenterId,
                                                                     Depit = s.Depit,
                                                                     Credit = s.Credit,
                                                                     CostCenterName = s.CostCenterName ?? "",
                                                                     InvoiceReference = s.InvoiceReference ?? "",
                                                                     Notes = s.Notes,
                                                                     Balance = s.Balance,
                                                                     TypeName = s.TypeName,
                                                                     TransactionDate = s.TransactionDate,
                                                                 }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(TransactionsSearch.DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                                                 (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(TransactionsSearch.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();


                }
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/ && ((s.AccountId == TransactionsSearch.AccountId || TransactionsSearch.AccountId == null) &&
                                                                    (s.CostCenterId == TransactionsSearch.CostCenterId || TransactionsSearch.CostCenterId == null || TransactionsSearch.CostCenterId == 0))).Select(x => new TransactionsVM
                                                                    {
                                                                        TransactionId = x.TransactionId,
                                                                        InvoiceId = x.InvoiceId,
                                                                        LineNumber = x.LineNumber,
                                                                        AccountId = x.AccountId,
                                                                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                                                        CostCenterId = x.CostCenterId,
                                                                        Depit = x.Depit ?? 0,
                                                                        Credit = x.Credit ?? 0,
                                                                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                                                                        InvoiceReference = x.InvoiceReference ?? "",
                                                                        Notes = x.Notes ?? "",
                                                                        Balance = x.Depit - x.Credit,
                                                                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                                                                        TransactionDate = x.TransactionDate,
                                                                    }).ToList();
                return details;
            }
           
        }


        public async Task< IEnumerable<TransactionsVM>> GetAllTransSearch(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId)
        {
            try
            {

                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type!=12 /*&& s.BranchId == BranchId */&& s.IsPost==true && ((s.AccountId == AccountId)) /*&& (s.Invoices!=null?s.Invoices.Rad!=true:false)*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit=x.Depit??0,
                    Credit=x.Credit??0,
                    JournalNo=x.JournalNo,
                    AccountName = x.Accounts!=null?x.Accounts.NameAr:"",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.IsDeleted==false? x.CostCenters.NameAr +"-"+ x.CostCenters.Code: "":"",
                    TypeName = x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr:"",
                    x.InvoiceReference,
                    Notes = x.Invoices != null ? string.IsNullOrEmpty(x.Invoices.InvoiceNotes) ? x.Notes : x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    JournalNo= s.JournalNo,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    AccountCode = s.AccountCode,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    //CurrentBalance = Convert.ToDouble(s.Depit.ToString()) - Convert.ToDouble(s.Credit.ToString()),
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate=s.InvoiceDate,
                }).ToList().OrderByDescending(s=>s.TransactionDate).ToList();



                if (!String.IsNullOrEmpty(Convert.ToString(CostCenterId)))
                {
                    details = details.Where(s=> s.CostCenterId == CostCenterId).Select(x => new
                                                                                    {
                                                                                        x.TransactionId,
                                                                                        x.InvoiceId,
                                                                                        x.LineNumber,
                                                                                        x.AccountId,
                                                                                                            x.AccountCode,
                                                                                                            x.InvoiceDate,
                                                                                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                                        x.CostCenterId,
                                                                                        Depit = x.Depit ?? 0,
                                                                                        Credit = x.Credit ?? 0,
                                                                                        AccountName = x.AccountName,
                                                                                        CostCenterName = x.CostCenterName,
                                                                                        TypeName = x.TypeName,
                                                                                        JournalNo = x.JournalNo,

                                                                                        x.InvoiceReference,
                                                                                        Notes = x.Notes ?? "",
                                                                                        Balance = x.Depit - x.Credit,
                                                                                        x.TransactionDate,
                                                                                    }).Select(s => new TransactionsVM
                                                                                    {
                                                                                        TransactionId = s.TransactionId,
                                                                                        InvoiceId = s.InvoiceId,
                                                                                        LineNumber = s.LineNumber,
                                                                                        AccountId = s.AccountId,
                                                                                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                                                                                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                                                                                        AccountName = s.AccountName,
                                                                                        CostCenterId = s.CostCenterId,
                                                                                        Depit = s.Depit,
                                                                                        Credit = s.Credit,
                                                                                        //CurrentBalance = Convert.ToDouble(s.Depit - s.Credit),
                                                                                        JournalNo = s.JournalNo,
                                                                                        AccountCode=s.AccountCode,
                                                                                        CostCenterName = s.CostCenterName ?? "",
                                                                                        InvoiceReference = s.InvoiceReference ?? "",
                                                                                        Notes = s.Notes,
                                                                                        Balance = s.Balance,
                                                                                        TypeName = s.TypeName,
                                                                                        TransactionDate = s.TransactionDate,
                                                                                        InvoiceDate=s.InvoiceDate,
                                                                                    }).ToList().OrderByDescending(s=>s.TransactionDate).ToList();



                }
                if (!String.IsNullOrEmpty(Convert.ToString(FromDate)) && !String.IsNullOrEmpty(Convert.ToString(ToDate)) && (DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {


                    details = details.Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        x.LineNumber,
                        x.AccountId,
                        x.AccountCode,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        x.InvoiceReference,
                        JournalNo = x.JournalNo,
                        x.InvoiceDate,
                        Notes = x.Notes ?? "",
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        Depit = s.Depit,
                        Credit = s.Credit,
                        //CurrentBalance = Convert.ToDouble(s.Depit - s.Credit),
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        JournalNo = s.JournalNo,
                        AccountCode=s.AccountCode,
                        Notes = s.Notes,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate=s.InvoiceDate,
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().OrderByDescending(s=>s.TransactionDate).ToList();


                }
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type != 12 && s.YearId == YearId /*&& s.BranchId == BranchId */&& ((s.AccountId == AccountId || AccountId == null) &&
                                                                    (s.CostCenterId == CostCenterId || CostCenterId == null || CostCenterId == 0)) && (s.Invoices != null ? s.Invoices.Rad != true : false)).Select(x => new TransactionsVM
                                                                    {
                                                                        TransactionId = x.TransactionId,
                                                                        InvoiceId = x.InvoiceId,
                                                                        LineNumber = x.LineNumber,
                                                                        AccountId = x.AccountId,
                                                                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                                                                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                                                        CostCenterId = x.CostCenterId,
                                                                        Depit = x.Depit,
                                                                        Credit = x.Credit,
                                                                        //CurrentBalance = Convert.ToDouble(Convert.ToDouble(x.Depit) - Convert.ToDouble(x.Credit)),
                                                                        CostCenterName = x.CostCenters != null ? x.CostCenters.IsDeleted == false ? x.CostCenters.NameAr + "-" + x.CostCenters.Code : "" : "",
                                                                        InvoiceReference = x.InvoiceReference ?? "",
                                                                        JournalNo = x.JournalNo,

                                                                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                                                                        Balance = x.Depit - x.Credit,
                                                                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                                                                        TransactionDate = x.TransactionDate,
                                                                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",

                                                                    }).ToList().OrderByDescending(s=>s.TransactionDate).ToList();
                return details;
            }

        }

        public async Task< IEnumerable<TransactionsVM>> GetAllTransSearch_New(int? AccountId, string? FromDate, string? ToDate, int? CostCenterId, int YearId, int BranchId, bool? isCheckedBranch)
        {
            try
            {

                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && (YearId==0 ||  s.YearId == YearId) && s.Type != 12  && s.Type !=35
                && ((isCheckedBranch==false && BranchId ==1) || s.BranchId == BranchId) && s.IsPost == true &&
                (AccountId == null||s.AccountId == AccountId) 
                && (CostCenterId == null || s.CostCenterId == CostCenterId || CostCenterId==0)
                /*&& (s.Invoices!=null?s.Invoices.Rad!=true:false)*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    InvoiceNumber = x.Invoices!=null?x.Invoices.InvoiceNumber??"":"",
                    x.LineNumber,
                    x.AccountId,
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    JournalNo = x.JournalNo,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.IsDeleted == false ? x.CostCenters.NameAr + "-" + x.CostCenters.Code : "" : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                                                                        Details = x.Details ?? "",
                                                                        Type = x.Type,
                                                                        Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                    AccTransactionDate = x.AccTransactionDate ??"",
                    AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",

                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    InvoiceNumber=s.InvoiceNumber,
                    JournalNo = s.JournalNo,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    AccountCode = s.AccountCode,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    //CurrentBalance = Convert.ToDouble(s.Depit.ToString()) - Convert.ToDouble(s.Credit.ToString()),
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Details = s.Details ?? "",
                    Type = s.Type,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate = s.InvoiceDate,
                    AccTransactionDate = s.AccTransactionDate ?? "",
                    AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                }).ToList().OrderByDescending(s => s.AccTransactionDate).ToList();

                if (!String.IsNullOrEmpty(Convert.ToString(CostCenterId)))
                {
                    details = details.Where(s => s.CostCenterId == CostCenterId).Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        x.LineNumber,
                        x.AccountId,
                        x.AccountCode,
                        x.InvoiceDate,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        JournalNo = x.JournalNo,

                        x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        Details = x.Details ?? "",
                        Type = x.Type,
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        InvoiceNumber=s.InvoiceNumber,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        Depit = s.Depit,
                        Credit = s.Credit,
                        //CurrentBalance = Convert.ToDouble(s.Depit - s.Credit),
                        JournalNo = s.JournalNo,
                        AccountCode = s.AccountCode,
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        Notes = s.Notes,
                        Details = s.Details ?? "",
                        Type = s.Type,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate = s.InvoiceDate,
                        AccTransactionDate = s.AccTransactionDate ?? "",
                        AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                    }).ToList().OrderByDescending(s => s.AccTransactionDate).ToList();



                }
                if (!String.IsNullOrEmpty(Convert.ToString(FromDate)) && !String.IsNullOrEmpty(Convert.ToString(ToDate)) && (DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {


                    details = details.Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        x.LineNumber,
                        x.AccountId,
                        x.AccountCode,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        x.InvoiceReference,
                        JournalNo = x.JournalNo,
                        x.InvoiceDate,
                        Notes = x.Notes ?? "",
                        Details = x.Details ?? "",
                        Type = x.Type,
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        InvoiceNumber=s.InvoiceNumber,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        Depit = s.Depit,
                        Credit = s.Credit,
                        //CurrentBalance = Convert.ToDouble(s.Depit - s.Credit),
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        JournalNo = s.JournalNo,
                        AccountCode = s.AccountCode,
                        Notes = s.Notes,
                        Details = s.Details ?? "",
                        Type = s.Type,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate = s.InvoiceDate,
                        AccTransactionDate = s.AccTransactionDate ?? "",
                        AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().OrderByDescending(s => s.TransactionDate).ToList();


                }
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type != 12 && (YearId == 0 || s.YearId == YearId) && (BranchId == 1 || s.BranchId == BranchId) && ((s.AccountId == AccountId || AccountId == null) &&
                    (s.CostCenterId == CostCenterId || CostCenterId == null || CostCenterId == 0)) && (s.Invoices != null ? s.Invoices.Rad != true : false)).Select(x => new TransactionsVM
                    {
                        TransactionId = x.TransactionId,
                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber ?? "" : "",
                        LineNumber = x.LineNumber,
                        AccountId = x.AccountId,
                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                        CostCenterId = x.CostCenterId,
                        Depit = x.Depit,
                        Credit = x.Credit,
                        //CurrentBalance = Convert.ToDouble(Convert.ToDouble(x.Depit) - Convert.ToDouble(x.Credit)),
                        CostCenterName = x.CostCenters != null ? x.CostCenters.IsDeleted == false ? x.CostCenters.NameAr + "-" + x.CostCenters.Code : "" : "",
                        InvoiceReference = x.InvoiceReference ?? "",
                        JournalNo = x.JournalNo,

                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                        Details = x.Details ?? "",
                        Type = x.Type,
                        Balance = x.Depit - x.Credit,
                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                        TransactionDate = x.TransactionDate,
                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",

                    }).ToList().OrderByDescending(s => s.AccTransactionDate).ToList();
                return details;
            }

        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransSearch_New_withChild(int? AccountId, string? FromDate, string? ToDate, int? CostCenterId, int YearId, int BranchId, bool? isCheckedBranch)
        {
            try
            {
               
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && (YearId == 0 || s.YearId == YearId) && s.Type != 12 && s.Type !=35 && ((isCheckedBranch == false && BranchId == 1) || s.BranchId == BranchId)
                && s.IsPost == true && ((s.AccountId == AccountId) || s.Accounts.ParentId == AccountId ||
                s.Accounts.ParentAccount.ParentId == AccountId || s.Accounts.ParentAccount.ParentAccount.ParentId == AccountId ||
                s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId ||
                s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId ||
                s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId ||
                s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId)
                /*&& (s.Invoices!=null?s.Invoices.Rad!=true:false)*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber ?? "" : "",
                    x.LineNumber,
                    x.AccountId,
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    JournalNo = x.JournalNo,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.IsDeleted == false ? x.CostCenters.NameAr + "-" + x.CostCenters.Code : "" : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                                                                        Details = x.Details ?? "",
                                                                        Type = x.Type,

                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                    AccTransactionDate = x.AccTransactionDate ?? "",
                    AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                    project = x.Invoices != null ? x.Invoices.Project : null,
                    pieceNo=x.Invoices!=null? x.Invoices.Project != null ? x.Invoices.ProjectId!=null?_TaamerProContext.ProjectPieces.Where(s=>s.PieceId==x.Invoices.Project.PieceNo)!.FirstOrDefault()!.PieceNo:"":"":"",


                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    InvoiceNumber=s.InvoiceNumber,
                    JournalNo = s.JournalNo,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    AccountCode = s.AccountCode,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    //CurrentBalance = Convert.ToDouble(s.Depit.ToString()) - Convert.ToDouble(s.Credit.ToString()),
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Details = s.Details ?? "",
                    Type = s.Type,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate = s.InvoiceDate,
                    AccTransactionDate = s.AccTransactionDate ?? "",
                    AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                    project=s.project,
                    pieceNo=s.pieceNo,
                }).ToList().OrderByDescending(s => s.AccTransactionDate).ToList();

                if (!String.IsNullOrEmpty(Convert.ToString(CostCenterId)))
                {
                    details = details.Where(s => s.CostCenterId == CostCenterId).Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        x.InvoiceNumber,
                        x.LineNumber,
                        x.AccountId,
                        x.AccountCode,
                        x.InvoiceDate,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        JournalNo = x.JournalNo,

                        x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        Details = x.Details ?? "",
                        Type = x.Type,
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                        project = x.project,
                        pieceNo = x.pieceNo,

                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        InvoiceNumber=s.InvoiceNumber,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        Depit = s.Depit,
                        Credit = s.Credit,
                        //CurrentBalance = Convert.ToDouble(s.Depit - s.Credit),
                        JournalNo = s.JournalNo,
                        AccountCode = s.AccountCode,
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        Notes = s.Notes,
                        Details = s.Details ?? "",
                        Type = s.Type,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate = s.InvoiceDate,
                        AccTransactionDate = s.AccTransactionDate ?? "",
                        AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                        project = s.project,
                        pieceNo = s.pieceNo,
                    }).ToList().OrderByDescending(s => s.AccTransactionDate).ToList();



                }
                if (!String.IsNullOrEmpty(Convert.ToString(FromDate)) && !String.IsNullOrEmpty(Convert.ToString(ToDate)) && (DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {


                    details = details.Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        x.InvoiceNumber,
                        x.LineNumber,
                        x.AccountId,
                        x.AccountCode,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        x.InvoiceReference,
                        JournalNo = x.JournalNo,
                        x.InvoiceDate,
                        Notes = x.Notes ?? "",
                        Details = x.Details ?? "",
                        Type = x.Type,
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                        project = x.project,
                        pieceNo = x.pieceNo,
                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        InvoiceNumber=s.InvoiceNumber,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        Depit = s.Depit,
                        Credit = s.Credit,
                        //CurrentBalance = Convert.ToDouble(s.Depit - s.Credit),
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        JournalNo = s.JournalNo,
                        AccountCode = s.AccountCode,
                        Notes = s.Notes,
                        Details = s.Details ?? "",
                        Type = s.Type,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate = s.InvoiceDate,
                        AccTransactionDate = s.AccTransactionDate ?? "",
                        AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                        project = s.project,
                        pieceNo = s.pieceNo,
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().OrderByDescending(s => s.TransactionDate).ToList();


                }
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type != 12 && (YearId == 0 || s.YearId == YearId) && (BranchId == 1 || s.BranchId == BranchId) && ((s.AccountId == AccountId || AccountId == null) &&
                    (s.CostCenterId == CostCenterId || CostCenterId == null || CostCenterId == 0)) && (s.Invoices != null ? s.Invoices.Rad != true : false)).Select(x => new TransactionsVM
                    {
                        TransactionId = x.TransactionId,
                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber ?? "" : "",
                        LineNumber = x.LineNumber,
                        AccountId = x.AccountId,
                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                        CostCenterId = x.CostCenterId,
                        Depit = x.Depit,
                        Credit = x.Credit,
                        //CurrentBalance = Convert.ToDouble(Convert.ToDouble(x.Depit) - Convert.ToDouble(x.Credit)),
                        CostCenterName = x.CostCenters != null ? x.CostCenters.IsDeleted == false ? x.CostCenters.NameAr + "-" + x.CostCenters.Code : "" : "",
                        InvoiceReference = x.InvoiceReference ?? "",
                        JournalNo = x.JournalNo,

                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                        Details = x.Details ?? "",
                        Type = x.Type,
                        Balance = x.Depit - x.Credit,
                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                        TransactionDate = x.TransactionDate,
                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                        project = x.Invoices != null ? x.Invoices.Project : null,
                        pieceNo = x.Invoices != null ? x.Invoices.Project != null ? x.Invoices.ProjectId != null ? _TaamerProContext.ProjectPieces.Where(s => s.PieceId == x.Invoices.Project.PieceNo)!.FirstOrDefault()!.PieceNo : "" : "" : "",

                    }).ToList().OrderByDescending(s => s.AccTransactionDate).ToList();
                return details;
            }

        }


        public async Task< IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId)
        {
            try
            {

                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type!=12 /*&& s.BranchId == BranchId*/ &&s.IsPost==true && ((s.AccountId == AccountId)) /*&& (s.Invoices != null ? s.Invoices.Rad != true : false)*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    x.JournalNo,
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",

                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    JournalNo = s.JournalNo,
                    AccountCode=s.AccountCode,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate=s.InvoiceDate,
                }).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(CostCenterId)))
                { 
                     details = details.Where(s =>s.CostCenterId == CostCenterId).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                         x.JournalNo,
                         x.AccountCode,
                         Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.AccountName,
                    CostCenterName = x.CostCenterName,
                    TypeName = x.TypeName,
                    x.InvoiceReference,
                    Notes = x.Notes ?? "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    x.InvoiceDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    JournalNo = s.JournalNo,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    AccountCode = s.AccountCode,

                    Depit = s.Depit,
                    Credit = s.Credit,
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate=s.InvoiceDate,
                }).ToList();

                }

         
     
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type != 12 /*&& s.BranchId == BranchId*/ && ((s.AccountId == AccountId || AccountId == null) &&
                                                                    (s.CostCenterId == CostCenterId || CostCenterId == null || CostCenterId == 0)) && (s.Invoices != null ? s.Invoices.Rad != true : false)).Select(x => new TransactionsVM
                                                                    {
                                                                        TransactionId = x.TransactionId,
                                                                        InvoiceId = x.InvoiceId,
                                                                        LineNumber = x.LineNumber,
                                                                        AccountId = x.AccountId,
                                                                        JournalNo = x.JournalNo,
                                                                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                                                                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                                                        CostCenterId = x.CostCenterId,
                                                                        Depit = x.Depit,
                                                                        Credit = x.Credit,
                                                                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                                                                        InvoiceReference = x.InvoiceReference ?? "",
                                                                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                                                                        Balance = x.Depit - x.Credit,
                                                                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                                                                        TransactionDate = x.TransactionDate,
                                                                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",

                                                                    }).ToList();
                return details;
            }

        }


        public async Task< IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId_New(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId)
        {
            try
            {

                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type != 12 && (BranchId == 1 || s.BranchId == BranchId) && s.IsPost == true && s.AccountId == AccountId /*&& (s.Invoices != null ? s.Invoices.Rad != true : false)*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    x.JournalNo,
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,

                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                    AccTransactionDate = x.AccTransactionDate ?? "",
                    AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",

                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    JournalNo = s.JournalNo,
                    AccountCode = s.AccountCode,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate = s.InvoiceDate,
                    AccTransactionDate = s.AccTransactionDate ?? "",
                    AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().OrderByDescending(s => s.TransactionDate).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(CostCenterId)))
                {
                    details = details.Where(s => s.CostCenterId == CostCenterId).Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        x.LineNumber,
                        x.AccountId,
                        x.JournalNo,
                        x.AccountCode,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                        x.InvoiceDate,
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        JournalNo = s.JournalNo,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        AccountCode = s.AccountCode,

                        Depit = s.Depit,
                        Credit = s.Credit,
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        Notes = s.Notes,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate = s.InvoiceDate,
                        AccTransactionDate = s.AccTransactionDate ?? "",
                        AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                    }).ToList();

                }



                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type != 12 && (BranchId == 1 || s.BranchId == BranchId) && ((s.AccountId == AccountId || AccountId == null) &&
                    (s.CostCenterId == CostCenterId || CostCenterId == null || CostCenterId == 0)) && (s.Invoices != null ? s.Invoices.Rad != true : false)).Select(x => new TransactionsVM
                    {
                        TransactionId = x.TransactionId,
                        InvoiceId = x.InvoiceId,
                        LineNumber = x.LineNumber,
                        AccountId = x.AccountId,
                        JournalNo = x.JournalNo,
                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                        CostCenterId = x.CostCenterId,
                        Depit = x.Depit,
                        Credit = x.Credit,
                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                        InvoiceReference = x.InvoiceReference ?? "",
                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                        Balance = x.Depit - x.Credit,
                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                        TransactionDate = x.TransactionDate,
                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",

                    }).ToList();
                return details;
            }

        }

        public async Task< IEnumerable<TransactionsVM>> GetAllTransSearchByAccIDandCostId_New_whithchild(int? AccountId, string FromDate, string ToDate, int? CostCenterId, int YearId, int BranchId)
        {
            try
            {

                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type != 12 && (BranchId == 1 || s.BranchId == BranchId) && s.IsPost == true && ((s.AccountId == AccountId) || s.Accounts.ParentId == AccountId || s.Accounts.ParentAccount.ParentId == AccountId || s.Accounts.ParentAccount.ParentAccount.ParentId == AccountId || s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId || s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId || s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId || s.Accounts.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentAccount.ParentId == AccountId) /*&& (s.Invoices != null ? s.Invoices.Rad != true : false)*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    x.JournalNo,
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,

                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                    AccTransactionDate = x.AccTransactionDate ?? "",
                    AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",

                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    JournalNo = s.JournalNo,
                    AccountCode = s.AccountCode,
                    Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                    DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                    AccountName = s.AccountName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    CostCenterName = s.CostCenterName ?? "",
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes,
                    Balance = s.Balance,
                    TypeName = s.TypeName,
                    TransactionDate = s.TransactionDate,
                    InvoiceDate = s.InvoiceDate,
                    AccTransactionDate = s.AccTransactionDate ?? "",
                    AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().OrderByDescending(s => s.TransactionDate).ToList();


                if (!String.IsNullOrEmpty(Convert.ToString(CostCenterId)))
                {
                    details = details.Where(s => s.CostCenterId == CostCenterId).Select(x => new
                    {
                        x.TransactionId,
                        x.InvoiceId,
                        x.LineNumber,
                        x.AccountId,
                        x.JournalNo,
                        x.AccountCode,
                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        x.CostCenterId,
                        Depit = x.Depit ?? 0,
                        Credit = x.Credit ?? 0,
                        AccountName = x.AccountName,
                        CostCenterName = x.CostCenterName,
                        TypeName = x.TypeName,
                        x.InvoiceReference,
                        Notes = x.Notes ?? "",
                        Balance = x.Depit - x.Credit,
                        x.TransactionDate,
                        x.InvoiceDate,
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",
                    }).Select(s => new TransactionsVM
                    {
                        TransactionId = s.TransactionId,
                        InvoiceId = s.InvoiceId,
                        LineNumber = s.LineNumber,
                        AccountId = s.AccountId,
                        JournalNo = s.JournalNo,
                        Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                        DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                        AccountName = s.AccountName,
                        CostCenterId = s.CostCenterId,
                        AccountCode = s.AccountCode,

                        Depit = s.Depit,
                        Credit = s.Credit,
                        CostCenterName = s.CostCenterName ?? "",
                        InvoiceReference = s.InvoiceReference ?? "",
                        Notes = s.Notes,
                        Balance = s.Balance,
                        TypeName = s.TypeName,
                        TransactionDate = s.TransactionDate,
                        InvoiceDate = s.InvoiceDate,
                        AccTransactionDate = s.AccTransactionDate ?? "",
                        AccTransactionHijriDate = s.AccTransactionHijriDate ?? "",
                    }).ToList();

                }



                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId && s.Type != 12 && (BranchId == 1 || s.BranchId == BranchId) && ((s.AccountId == AccountId || AccountId == null) &&
                    (s.CostCenterId == CostCenterId || CostCenterId == null || CostCenterId == 0)) && (s.Invoices != null ? s.Invoices.Rad != true : false)).Select(x => new TransactionsVM
                    {
                        TransactionId = x.TransactionId,
                        InvoiceId = x.InvoiceId,
                        LineNumber = x.LineNumber,
                        AccountId = x.AccountId,
                        JournalNo = x.JournalNo,
                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                        CostCenterId = x.CostCenterId,
                        Depit = x.Depit,
                        Credit = x.Credit,
                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                        InvoiceReference = x.InvoiceReference ?? "",
                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                        Balance = x.Depit - x.Credit,
                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                        TransactionDate = x.TransactionDate,
                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",
                        AccTransactionDate = x.AccTransactionDate ?? "",
                        AccTransactionHijriDate = x.AccTransactionHijriDate ?? "",

                    }).ToList();
                return details;
            }

        }
        public async Task< IEnumerable<TransactionsVM>> GetAllSubAccTransactionsSearch(TransactionsVM TransactionsSearch, int YearId, int BranchId)
        {
            try
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId */&& (((s.Accounts.ParentId == TransactionsSearch.AccountId && s.Accounts.IsMain == false) || TransactionsSearch.AccountId == null) &&
                                                                  (s.CostCenterId == TransactionsSearch.CostCenterId || TransactionsSearch.CostCenterId == null || TransactionsSearch.CostCenterId == 0))).Select(x => new
                                                                  {
                                                                      x.TransactionId,
                                                                      x.InvoiceId,
                                                                      x.LineNumber,
                                                                      x.AccountId,
                                                                      AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                                                                      Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                      DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                      x.CostCenterId,
                                                                      Depit = x.Depit ?? 0,
                                                                      Credit = x.Credit ?? 0,
                                                                      AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                                                      CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                                                                      TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                                                                      x.InvoiceReference,
                                                                      Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                                                                      Balance = x.Depit - x.Credit,
                                                                      x.TransactionDate,
                                                                      InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",

                                                                  }).Select(s => new TransactionsVM
                                                                  {
                                                                      TransactionId = s.TransactionId,
                                                                      InvoiceId = s.InvoiceId,
                                                                      LineNumber = s.LineNumber,
                                                                      AccountId = s.AccountId,
                                                                      AccountCode=s.AccountCode,
                                                                      Amount = (s.Depit > s.Credit) ? s.Depit : s.Credit,
                                                                      DepitOrCreditName = s.Depit > s.Credit ? "مدين" : "دائن",
                                                                      AccountName = s.AccountName,
                                                                      CostCenterId = s.CostCenterId,
                                                                      Depit = s.Depit,
                                                                      Credit = s.Credit,
                                                                      CostCenterName = s.CostCenterName ?? "",
                                                                      InvoiceReference = s.InvoiceReference ?? "",
                                                                      Notes = s.Notes,
                                                                      Balance = s.Balance,
                                                                      TypeName = s.TypeName,
                                                                      TransactionDate = s.TransactionDate,
                                                                      InvoiceDate = s.InvoiceDate,

                                                                  }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(TransactionsSearch.DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                                                  (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(TransactionsSearch.DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                return details;
            }
            catch (Exception ex)
            {
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/ && ((s.Accounts.ParentId == TransactionsSearch.AccountId && s.Accounts.IsMain == false || TransactionsSearch.AccountId == null) &&
                                                                    (s.CostCenterId == TransactionsSearch.CostCenterId || TransactionsSearch.CostCenterId == null || TransactionsSearch.CostCenterId == 0))).Select(x => new TransactionsVM
                                                                    {
                                                                        TransactionId = x.TransactionId,
                                                                        InvoiceId = x.InvoiceId,
                                                                        LineNumber = x.LineNumber,
                                                                        AccountId = x.AccountId,
                                                                        AccountCode = x.Accounts != null ? x.Accounts.Code : "",

                                                                        Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                                                        DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                                                        AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                                                        CostCenterId = x.CostCenterId,
                                                                        Depit = x.Depit ?? 0,
                                                                        Credit = x.Credit ?? 0,
                                                                        CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                                                                        InvoiceReference = x.InvoiceReference ?? "",
                                                                        Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                                                                        Balance = x.Depit - x.Credit,
                                                                        TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                                                                        TransactionDate = x.TransactionDate,
                                                                        InvoiceDate = x.Invoices != null ? x.Invoices.Date : "",

                                                                    }).ToList();
                return details;
            }

        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransCustomers(int YearId,int BranchId)
        {
            var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Accounts.Classification == 2 && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new TransactionsVM
            {
                TransactionId = x.TransactionId,
                InvoiceId = x.InvoiceId,
                LineNumber = x.LineNumber,
                AccountId = x.AccountId,
                Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                CostCenterId = x.CostCenterId,
                Depit = x.Depit ?? 0,
                Credit = x.Credit ?? 0,
                InvoiceReference = x.InvoiceReference ?? "",
                Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                DebitBalance = (x.Depit - x.Credit) > 0 ? (x.Depit - x.Credit) : 0,
                CreditBalance = (x.Credit - x.Depit) > 0 ? (x.Credit - x.Depit) : 0,
                CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                TransactionDate = x.TransactionDate,
            });
            return details;
        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransactions(string FromDate, string ToDate, int YearId, int BranchId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    Balance = s.Depit - s.Credit,
                    DebitBalance = (s.Depit - s.Credit) > 0 ? (s.Depit - s.Credit) : 0,
                    CreditBalance = (s.Credit - s.Depit) > 0 ? (s.Credit - s.Depit) : 0,
                    TransactionDate = s.TransactionDate,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
            }
            catch (Exception)
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new TransactionsVM
                          {
                              TransactionId = x.TransactionId,
                              InvoiceId = x.InvoiceId,
                              LineNumber = x.LineNumber,
                              AccountId = x.AccountId,
                              Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                              DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                            AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                            CostCenterId = x.CostCenterId,
                                Depit = x.Depit ?? 0,
                                Credit = x.Credit ?? 0,
                              InvoiceReference = x.InvoiceReference ?? "",
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    TransactionDate = x.TransactionDate,
                              DebitBalance = (x.Depit - x.Credit) > 0 ? (x.Depit - x.Credit) : 0,
                              CreditBalance = (x.Credit - x.Depit) > 0 ? (x.Credit - x.Depit) : 0,
                });
            }
        }
        public async Task< IEnumerable<TransactionsVM>> GetAllTransactionsByAccType(int AccType,string FromDate, string ToDate, int YearId, int BranchId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountType == AccType && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    Balance = s.Depit - s.Credit,
                    TransactionDate = s.TransactionDate,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(); 
            }
            catch (Exception)
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountType == AccType && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new TransactionsVM
                        {
                                 TransactionId = x.TransactionId,
                                 InvoiceId = x.InvoiceId,
                                 LineNumber = x.LineNumber,
                                 AccountId = x.AccountId,
                                 Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                                 DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                                AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                                CostCenterId = x.CostCenterId,
                                Depit = x.Depit ?? 0,
                                Credit = x.Credit ?? 0,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    InvoiceReference = x.InvoiceReference ?? "",
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                                 TransactionDate = x.TransactionDate,
                      });
            }
           
        }

        public async Task< IEnumerable<TransactionsVM>> GetAllCustomerTrans(string FromDate, string ToDate,int YearId, int BranchId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CustomerId != null && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    CustomerName =x.Customer!=null? x.Customer.CustomerNameAr:"",
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName,
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    DebitBalance = (s.Depit - s.Credit) > 0 ? (s.Depit - s.Credit) : 0,
                    CreditBalance = (s.Credit - s.Depit) > 0 ? (s.Credit - s.Depit) : 0,
                    TransactionDate = s.TransactionDate,
                    CustomerName = s.CustomerName,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
            }
            catch (Exception)
            {
                var CustTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CustomerId != null && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new TransactionsVM
                {
                    TransactionId = x.TransactionId,
                    InvoiceId = x.InvoiceId,
                    LineNumber = x.LineNumber,
                    AccountId = x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterId = x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    InvoiceReference = x.InvoiceReference ?? "",
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    DebitBalance = (x.Depit - x.Credit) > 0 ? (x.Depit - x.Credit) : 0,
                    CreditBalance = (x.Credit - x.Depit) > 0 ? (x.Credit - x.Depit) : 0,
                    TransactionDate = x.TransactionDate,
                    CustomerName =x.Customer!=null? x.Customer.CustomerNameAr:"",
                });
                return CustTrans;
            }
        }

        public async Task< List<double> >GetValueNeeded(int BranchId, string lang, int YearId, string FromDate, string ToDate, string Con,int? taxID,int? taxID2, int? AccountID_Mb, int? AccountID_MR)
        {
            try
            {
                List<double> lmd = new List<double>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "ValueAdded";
                        command.Connection = con;
                        string from = null;
                        string to = null;

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@startdate", YearId + "-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@startdate", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@enddate", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;
                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@enddate", tdate));
                        }

                        var AllTaxesCredit = 0.0;
                        var AllTaxesCredit_NotInclude = 0.0;
                        var AllTaxesCredit_Include=0.0;
                        var AllTaxesDepit = 0.0;
                        var AllTaxesDepit_NotInclude = 0.0;
                        var AllTaxesDepit_Include = 0.0;

                        command.Parameters.Add(new SqlParameter("@yearID", YearId));
                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));

                        command.Parameters.Add(new SqlParameter("@AccountID_Mb", AccountID_Mb));
                        command.Parameters.Add(new SqlParameter("@AccountID_MR", AccountID_MR));

                        var BranchIdCheck = _TaamerProContext.SystemSettings.FirstOrDefault()!.ValueAddedSeparated ?? false;
                        if(BranchIdCheck==true)
                        {
                            command.Parameters.Add(new SqlParameter("@branchId", BranchId));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@branchId", DBNull.Value));

                        }




                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        AllTaxesCredit = Convert.ToDouble(ds.Tables[0].Rows[0]["AllTaxesCredit"]);
                        AllTaxesCredit_NotInclude = Convert.ToDouble(ds.Tables[1].Rows[0]["AllTaxesCredit_NotInclude"]);
                        AllTaxesCredit_Include = Convert.ToDouble(ds.Tables[2].Rows[0]["AllTaxesCredit_Include"]);
                        AllTaxesDepit = Convert.ToDouble(ds.Tables[3].Rows[0]["AllTaxesDepit"]);
                        AllTaxesDepit_NotInclude = Convert.ToDouble(ds.Tables[4].Rows[0]["AllTaxesDepit_NotInclude"]);
                        AllTaxesDepit_Include = Convert.ToDouble(ds.Tables[5].Rows[0]["AllTaxesDepit_Include"]);

                        lmd.Add(AllTaxesCredit);
                        lmd.Add(AllTaxesCredit_NotInclude);
                        lmd.Add(AllTaxesCredit_Include);
                        lmd.Add(AllTaxesDepit);
                        lmd.Add(AllTaxesDepit_NotInclude);
                        lmd.Add(AllTaxesDepit_Include);
                    }
                }

                return lmd;
            }
            catch (Exception ex)
            {
                List<double> lmd = new List<double>();
                return lmd;
            }
        }




        public async Task< IEnumerable<TransactionsVM>> GetAllCostCenterTrans(string FromDate, string ToDate, int YearId, int BranchId)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CostCenterId != null && s.YearId == YearId /*&& s.BranchId == BranchId*/).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    x.InvoiceReference,
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                    CustomerName =x.Customer!=null? x.Customer.CustomerNameAr:"",
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    DebitBalance = (s.Depit - s.Credit) > 0 ? (s.Depit - s.Credit) : 0,
                    CreditBalance = (s.Credit - s.Depit) > 0 ? (s.Credit - s.Depit) : 0,
                    TransactionDate = s.TransactionDate,
                    CustomerName = s.CustomerName,
                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
            }
            catch (Exception)
            {
                var CostCenterTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.CostCenterId != null && s.YearId == YearId && s.BranchId == BranchId).Select(x => new TransactionsVM
                {
                    TransactionId = x.TransactionId,
                    InvoiceId = x.InvoiceId,
                    LineNumber = x.LineNumber,
                    AccountId = x.AccountId,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterId = x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    InvoiceReference = x.InvoiceReference ?? "",
                    Notes=x.Invoices!=null? string.IsNullOrEmpty(x.Invoices.InvoiceNotes)? x.Notes: x.Invoices.InvoiceNotes : "",
                    DebitBalance = (x.Depit - x.Credit) > 0 ? (x.Depit - x.Credit) : 0,
                    CreditBalance = (x.Credit - x.Depit) > 0 ? (x.Credit - x.Depit) : 0,
                    TransactionDate = x.TransactionDate,
                    CustomerName =x.Customer!=null? x.Customer.CustomerNameAr:"",
                });
                return CostCenterTrans;
            }
        }



        public async Task< IEnumerable<TransactionsVM>> GetAllJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate, int YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type!=12 && s.Type !=(int)VoucherType.PurchesOrder /*&& s.BranchId == BranchId*/ && s.YearId== YearId && s.IsPost==true /*&& s.Invoices.Rad!=true*/).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                JournalNo = tr.JournalNo,
                InvoiceId=tr.InvoiceId,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit??0,
                Credit = tr.Credit??0,
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                Notes =tr.Invoices!=null? tr.Invoices.InvoiceNotes == null ? tr.Notes : tr.Invoices.InvoiceNotes ?? "" ?? "":"",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                AccountCode =tr.Accounts!=null? tr.Accounts.Code:"",
                InvoiceNumber = tr.Invoices!=null? tr.Invoices.InvoiceNumber:"",
            }).ToList();
            try
            {


                if (!String.IsNullOrEmpty(Convert.ToString(FromDate)) && !String.IsNullOrEmpty(Convert.ToString(ToDate)) && (DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {
                    Journals= Journals.Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        InvoiceId = tr.InvoiceId,

                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenterName ?? "",
                        TypeName = tr.TypeName ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.AccountName,
                        AccountCode = tr.AccountCode,
                        InvoiceNumber = tr.InvoiceNumber,

                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                         (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                }

                if (!String.IsNullOrEmpty(Convert.ToString(FromJournal)) && !String.IsNullOrEmpty(Convert.ToString(ToJournal)))
                {
                    Journals = Journals.Where(s => s.JournalNo >= FromJournal && s.JournalNo <= ToJournal).Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        InvoiceId = tr.InvoiceId,

                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenterName ?? "",
                        TypeName = tr.TypeName ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.AccountName,
                        AccountCode = tr.AccountCode,
                        InvoiceNumber = tr.InvoiceNumber,

                    }).ToList();
                }

            
              
                    return Journals;
                }
                catch (Exception ex)
                {
                    var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type != 12 && /*s.BranchId == BranchId &&*/ s.YearId==YearId).Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        InvoiceId = tr.InvoiceId,

                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                        TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Invoices != null ? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes) ? tr.Notes : tr.Invoices.InvoiceNotes : "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                        AccountCode =tr.Accounts!=null? tr.Accounts.Code:"",
                        InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",

                    }).ToList();
                    return Journals2;
                }
           
            

        }





        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByInvID(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 2 /*&&  s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId== invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId=tr.InvoiceId,
                InvoiceNumber =tr.InvoiceId!=null? tr.Invoices.InvoiceNumber:"",
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit??0,
                Credit = tr.Credit??0,
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                Notes = tr.Invoices != null ? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes) ? tr.Notes : tr.Invoices.InvoiceNotes : "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                AccountCode =tr.Accounts!=null? tr.Accounts.Code:"",
            }).ToList();
            try
            {    

                return Journals;
            }
            catch (Exception ex)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 2 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    InvoiceNumber =tr.Invoices!=null? tr.Invoices.InvoiceNumber:"",
                    Depit = tr.Depit??0,
                    Credit = tr.Credit??0,
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    Notes = tr.Invoices != null ? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes) ? tr.Notes : tr.Invoices.InvoiceNotes : "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                    AccountCode =tr.Accounts!=null? tr.Accounts.Code:"",
                }).ToList();
                return Journals2;
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByInvIDPurchase(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 1 /*&&  s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",

                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception ex)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 1 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }

        public async Task<IEnumerable<TransactionsVM>> GetAllJournalsByInvIDPurchaseOrder(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 35 /*&&  s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes = tr.Invoices != null ? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes) ? tr.Notes : tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",

                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception ex)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 1 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes = tr.Invoices != null ? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes) ? tr.Notes : tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }

        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByReVoucherID(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 6 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 6 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByPayVoucherID(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 5 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList().OrderByDescending(t=>t.TransactionId);
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 5 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2.OrderByDescending(t => t.TransactionId);
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByDailyID(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && (s.Type == 8 || s.Type==17) /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.Type == 17 ? "سند يومية" : tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",                
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 8 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.Type == 17 ? "سند يومية" : tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByDailyID_Custody(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 8 /*&& s.BranchId == BranchId*/ /*&& s.YearId == YearId*/ && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 8 /*&& s.BranchId == BranchId*/ /*&& s.YearId == YearId*/ && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }

        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByClosingID(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 25 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 25 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }


        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByInvIDRet(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 4 /*&& s.BranchId == BranchId*/ /*&& s.YearId == YearId*/ && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 4 /*&& s.BranchId == BranchId*/ /*&& s.YearId == YearId*/ && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    InvoiceId = tr.InvoiceId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByInvIDCreditDepitNoti(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && (s.Type == 29|| s.Type == 30|| s.Type == 32|| s.Type == 33) /*&& s.BranchId == BranchId && s.YearId == YearId*/ && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 4 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    InvoiceId = tr.InvoiceId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetAllJournalsByInvIDRetPurchase(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 3 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 3 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    InvoiceId = tr.InvoiceId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }


        public async Task< IEnumerable<TransactionsVM>> GetAllPayJournalsByInvIDRet(int? invId, int? YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 23 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                InvoiceId = tr.InvoiceId,
                JournalNo = tr.JournalNo,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit ?? 0,
                Credit = tr.Credit ?? 0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                return Journals;
            }
            catch (Exception)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.Type == 23 /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.InvoiceId == invId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    InvoiceId = tr.InvoiceId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit ?? 0,
                    Credit = tr.Credit ?? 0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                return Journals2;
            }



        }


        public async Task< IEnumerable<TransactionsVM>> GetAllTotalJournals(int? FromJournal, int? ToJournal, string FromDate, string ToDate, int YearId, int BranchId)
        {
            var Journals = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.YearId == YearId && s.IsPost == true && s.Invoices.Rad != true).Select(tr => new TransactionsVM
            {
                TransactionId = tr.TransactionId,
                JournalNo = tr.JournalNo??0,
                LineNumber = tr.LineNumber,
                Depit = tr.Depit??0,
                Credit = tr.Credit??0,
                InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                InvoiceReference = tr.InvoiceReference ?? "",
                CurrentBalance = tr.CurrentBalance,
                TransactionDate = tr.TransactionDate,
                AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
            }).ToList();
            try
            {

                if (!String.IsNullOrEmpty(Convert.ToString(FromDate)) && !String.IsNullOrEmpty(Convert.ToString(ToDate)) && (DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {
                    Journals = Journals.Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo??0,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenterName ?? "",
                        TypeName = tr.TypeName ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.AccountName,
                        AccountCode = tr.AccountCode,
                        InvoiceNumber = tr.InvoiceNumber,

                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                         (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                }

                if (!String.IsNullOrEmpty(Convert.ToString(FromJournal)) && !String.IsNullOrEmpty(Convert.ToString(ToJournal)))
                {
                    Journals = Journals.Where(s => s.JournalNo >= FromJournal && s.JournalNo <= ToJournal).Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo??0,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenterName ?? "",
                        TypeName = tr.TypeName ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.AccountName,
                        AccountCode = tr.AccountCode,
                        InvoiceNumber = tr.InvoiceNumber,


                    }).ToList();
                }



                return Journals;
            }
            catch (Exception ex)
            {
                var Journals2 = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.YearId == YearId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo??0,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit??0,
                    Credit = tr.Credit??0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",

                }).ToList();
                return Journals2;
            }



        }
        public async Task< IEnumerable<TransactionsVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int YearId)
        {
            try
            {
                // && (s.JournalNo >= FromJournal && s.JournalNo <= ToJournal)
                var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.YearId==YearId).Select(tr => new TransactionsVM
                {
                    TransactionId = tr.TransactionId,
                    JournalNo = tr.JournalNo,
                    LineNumber = tr.LineNumber,
                    Depit = tr.Depit??0,
                    Credit = tr.Credit??0,
                    InvoiceNumber = tr.Invoices != null ? tr.Invoices.InvoiceNumber : "",
                    CostCenterName = tr.CostCenters != null ? tr.CostCenters.NameAr : "",
                    TypeName = tr.AccTransactionTypes != null ? tr.AccTransactionTypes.NameAr : "",
                    Notes=tr.Invoices!=null? string.IsNullOrEmpty(tr.Invoices.InvoiceNotes)? tr.Notes: tr.Invoices.InvoiceNotes : "",
                    AccountCode = tr.Accounts != null ? tr.Accounts.Code : "",
                    InvoiceReference = tr.InvoiceReference ?? "",
                    CurrentBalance = tr.CurrentBalance,
                    TransactionDate = tr.TransactionDate,
                    AccountName = tr.Accounts != null ? tr.Accounts.NameAr : "",
                }).ToList();
                if (!String.IsNullOrEmpty(AccountCode))
                {

                   // int AccountID = Convert.ToInt32(AccountCode);
                    details = details.Where(w => w.AccountCode == AccountCode).Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenterName ?? "",
                        TypeName = tr.TypeName ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.AccountName,
                        AccountCode = tr.AccountCode,
                    }).ToList();
                }
                if (!String.IsNullOrEmpty(CCID) && CCID!="NaN")
                {
                    int CostID = Convert.ToInt32(CCID);
                    details = details.Where(w => w.CostCenterId == CostID).Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = tr.JournalNo,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit??0,
                        Credit = tr.Credit??0,
                        CostCenterName = tr.CostCenterName ?? "",
                        TypeName = tr.TypeName ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.AccountName,
                        AccountCode = tr.AccountCode,
                    }).ToList();
                }
               


                    //  .Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    //     (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();


                    //  var accounts= AccountStatmentVM.
                //    List<TransactionsVM> lmd = new List<TransactionsVM>();
                //using (SqlConnection con = new SqlConnection(Con))
                //{
                //    using (SqlCommand command = new SqlCommand())
                //    {
                //        command.CommandType = CommandType.StoredProcedure;
                //        command.CommandText = "MAcc_AccountStatement";
                //        command.Connection = con;
                //        string from = null;
                //        string to = null;
                //        //  command.Parameters.AddWithValue("@SubjectId", b);

                //        if (FromDate == "")
                //        {
                //            from = null;
                //            command.Parameters.Add(new SqlParameter("@Datefrom", "2000-01-01"));

                //        }
                //        else
                //        {
                //            from = FromDate;
                //            command.Parameters.Add(new SqlParameter("@Datefrom", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                //            //command.Parameters.Add(new SqlParameter("@enddate", to));
                //        }
                //        if (ToDate == "")
                //        {
                //            to = null;
                //            command.Parameters.Add(new SqlParameter("@Dateto", "2100-12-12"));

                //        }
                //        else
                //        {
                //            to = ToDate;
                //            //command.Parameters.Add(new SqlParameter("@startdate",from));
                //            command.Parameters.Add(new SqlParameter("@Dateto", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                //        }

                //        if (AccountCode == "")
                //        {
                //            AccountCode = null;
                //            command.Parameters.Add(new SqlParameter("@AccId", "121"));
                //        }

                //        else
                //        {
                //            command.Parameters.Add(new SqlParameter("@AccId", AccountCode));
                //        }

                //        if (CCID == "")
                //        {

                //            command.Parameters.Add(new SqlParameter("@CostId", "1"));
                //        }
                //        else
                //        {
                //            command.Parameters.Add(new SqlParameter("@CostId", CCID));
                //        }


                //        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                //        command.Parameters.Add(new SqlParameter("@YearID", YearId));

                //        con.Open();

                //        SqlDataAdapter a = new SqlDataAdapter(command);
                //        DataSet ds = new DataSet();
                //        a.Fill(ds);
                //        DataTable dt = new DataTable();
                //        dt = ds.Tables[0];
                //        foreach (DataRow dr in dt.Rows)

                //        // loop for adding add from dataset to list<modeldata>  
                //        {
                //            lmd.Add(new TransactionsVM
                //            {
                //                //TransID = (dr[1]).ToString(),
                //                //CDate = (dr[2]).ToString(),
                //                //Hdate = (dr[3]).ToString(),
                //                //Description = dr[12].ToString(),
                //                //Debit = (dr[10]).ToString(),
                //                //Credit = (dr[9]).ToString(),
                //                //CostCenter = (dr[8]).ToString(),
                //                //Balance = 0.ToString(),

                //            });
                //        }
                //    }
                //}
                return details;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<TransactionsVM> lmd = new List<TransactionsVM>();
                return lmd;
            }

        }

        //public TransactionsVM gettransbyid(int? jornal)
        //{
        //    var details = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.JournalNo == jornal).FirstOrDefault();
        //    TransactionsVM transvm = new TransactionsVM();
        //    transvm.TransactionId = details.TransactionId;
        //    transvm.InvoiceId = details.InvoiceId;
        //    transvm.LineNumber = details.LineNumber;
        //    transvm.AccountId = details.AccountId;
        //    transvm.Amount = (details.Depit > details.Credit) ? details.Depit : details.Credit;
        //    transvm.DepitOrCreditName = details.Depit > details.Credit ? "مدين" : "دائن";
        //    transvm.AccountName = details.Accounts != null ? details.Accounts.Code + " - " + details.Accounts.NameAr : "";
        //    transvm.CostCenterId = details.CostCenterId;
        //    transvm.Depit = details.Depit;
        //    transvm.Credit = details.Credit;
        //    transvm.CostCenterName = details.CostCenters != null ? details.CostCenters.Code + " - " + details.CostCenters.NameAr : "";
        //    transvm.InvoiceReference = details.InvoiceReference ?? "";
        //    transvm.Notes = details.Notes ?? "";
        //        //.Select(x => new TransactionsVM
        //    //{
        //    //    TransactionId = x.TransactionId,
        //    //    InvoiceId = x.InvoiceId,
        //    //    LineNumber = x.LineNumber,
        //    //    AccountId = x.AccountId,
        //    //    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
        //    //    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
        //    //    AccountName = x.Accounts != null ? x.Accounts.Code + " - " + x.Accounts.NameAr : "",
        //    //    CostCenterId = x.CostCenterId,
        //    //    Depit = x.Depit,
        //    //    Credit = x.Credit,
        //    //    CostCenterName = x.CostCenters != null ? x.CostCenters.Code + " - " + x.CostCenters.NameAr : "",
        //    //    InvoiceReference = x.InvoiceReference ?? "",
        //    //    Notes = x.Notes ?? "",
        //    //});
        //    return transvm;
        //}
        public async Task< IEnumerable<TransactionsVM>> gettransbyid(int? jornal)
        {
            try
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.TransactionId == jornal && s.Type != 12).Select(x => new
                {
                    x.TransactionId,
                    x.InvoiceId,
                    x.LineNumber,
                    x.AccountId,
                    x.Accounts.Code,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    x.JournalNo,
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    Notes = x.Invoices != null ? string.IsNullOrEmpty(x.Invoices.InvoiceNotes) ? x.Notes : x.Invoices.InvoiceNotes : "",
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                    x.InvoiceReference,
                    Balance = x.Depit - x.Credit,
                    x.TransactionDate,
                }).Select(s => new TransactionsVM
                {
                    TransactionId = s.TransactionId,
                    InvoiceId = s.InvoiceId,
                    JournalNo = s.JournalNo ?? 0,
                    LineNumber = s.LineNumber,
                    AccountId = s.AccountId,
                    AccountCode = s.Code,
                    Amount = s.Amount,
                    DepitOrCreditName = s.DepitOrCreditName,
                    CostCenterId = s.CostCenterId,
                    Depit = s.Depit,
                    Credit = s.Credit,
                    AccountName = s.AccountName,
                    CostCenterName = s.CostCenterName ?? "",
                    TypeName = s.TypeName,
                    InvoiceReference = s.InvoiceReference ?? "",
                    Notes = s.Notes ?? "",
                    Balance = s.Depit - s.Credit,
                    TransactionDate = s.TransactionDate,
                }).ToList();//.Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
            //        (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).OrderByDescending(a => a.TransactionDate).ToList();
            }
            catch (Exception)
            {
                return _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.JournalNo == jornal).Select(x => new TransactionsVM
                {
                    TransactionId = x.TransactionId,
                    //InvoiceId = x.InvoiceId,
                    LineNumber = x.LineNumber,
                    AccountId = x.AccountId,
                    JournalNo = x.JournalNo ?? 0,
                    Amount = (x.Depit > x.Credit) ? x.Depit : x.Credit,
                    DepitOrCreditName = x.Depit > x.Credit ? "مدين" : "دائن",
                    AccountName = x.Accounts != null ? x.Accounts.NameAr : "",
                    CostCenterId = x.CostCenterId,
                    Depit = x.Depit ?? 0,
                    Credit = x.Credit ?? 0,
                    InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber : "",
                    CostCenterName = x.CostCenters != null ? x.CostCenters.NameAr : "",
                    TypeName = x.Type == 17 ? "سند يومية": x.AccTransactionTypes != null ? x.AccTransactionTypes.NameAr : "",
                    Notes = x.Invoices != null ? string.IsNullOrEmpty(x.Invoices.InvoiceNotes) ? x.Notes : x.Invoices.InvoiceNotes : "",
                    AccountCode = x.Accounts != null ? x.Accounts.Code : "",
                    InvoiceReference = x.InvoiceReference ?? "",
                    Balance = x.Depit - x.Credit,
                    TransactionDate = x.TransactionDate,
                }).ToList();//.OrderByDescending(s => s.TransactionDate).ToList();
            }
        }


        //public async Task< IEnumerable<TransactionsVM>> GetProjectManagerRevene(int? ManagerId, string dateFrom, string dateTo, int YearId, int BranchId)
        //{
        //    try
        //    {
        //        var details = _TaamerProContext.Transactions.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId != null && s.YearId == YearId && s.Type == 6 && DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture)
        //                                                                       && DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture) && (s.Project.MangerId == ManagerId || ManagerId == null)
        //                                                                       ).Select(x => new TransactionsVM
        //                                                                       {
        //                                                                           InvoiceNumber = x.InvoiceNumber,
        //                                                                           Type = x.Type,
        //                                                                           IsPost = x.IsPost,
        //                                                                           PostDate = x.PostDate,
        //                                                                           PostHijriDate = x.PostHijriDate,
        //                                                                           StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
        //                                                                           HijriDate = x.HijriDate,
        //                                                                           Date = x.Date,
        //                                                                           InvoiceReference = x.InvoiceReference,
        //                                                                           Notes = x.Notes,
        //                                                                           TotalValue = x.TotalValue,
        //                                                                           InvoiceValue = x.InvoiceValue,
        //                                                                           TaxAmount = x.TaxAmount,
        //                                                                           PayType = x.PayType,
        //                                                                           ProjectNo = x.Project.ProjectNo,
        //                                                                       }).ToList().OrderBy(s => s.InvoiceNumber);

        //        return details;
        //    }
        //    catch (Exception)
        //    {
        //        var details = _TaamerProContext.Transactions.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ProjectId != null && s.YearId == YearId && s.Type == 6 && (s.Project.MangerId == ManagerId || ManagerId == null)).Select(x => new TransactionsVM
        //        {
        //            InvoiceNumber = x.InvoiceNumber,
        //            InvoiceId = x.InvoiceId,
        //            Type = x.Type,
        //            IsPost = x.IsPost,
        //            PostDate = x.PostDate,
        //            PostHijriDate = x.PostHijriDate,
        //            StatusName = x.IsPost == true ? "تم الترحيل" : "غير مرحل",
        //            HijriDate = x.HijriDate,
        //            Date = x.Date,
        //            InvoiceReference = x.InvoiceReference,
        //            Notes = x.Notes,
        //            TotalValue = x.TotalValue,
        //            InvoiceValue = x.InvoiceValue,
        //            TaxAmount = x.TaxAmount,
        //            PayType = x.PayType,
        //            ProjectNo = x.Project.ProjectNo,
        //        }).ToList().OrderBy(s => s.InvoiceNumber);
        //        return details;
        //    }

        //}
    }
}
