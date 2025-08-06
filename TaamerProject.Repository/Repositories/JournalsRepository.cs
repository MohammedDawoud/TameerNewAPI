using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class JournalsRepository : IJournalsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public JournalsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<IEnumerable<JournalsVM>> GetJournalsbyParam(int InvoiceId, int Year, int Branch, int Type)
        {
            try
            {
                var Journals = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && (s.VoucherId == InvoiceId && s.YearMalia == Year) && s.BranchId == Branch&&s.VoucherType== Type).Select(x => new JournalsVM
                {
                    JournalId = x.JournalId,
                    JournalNo = x.JournalNo,
                    VoucherId = x.VoucherId,
                    VoucherType = x.VoucherType,
                    BranchId = x.BranchId,
                    UserId = x.UserId,                 
                }).ToList();
                return Journals;
            }
            catch (Exception)
            {

                var Journals = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && s.VoucherId == InvoiceId && s.YearMalia == Year && s.BranchId == Branch && s.VoucherType == Type).Select(x => new JournalsVM
                {
                    JournalId = x.JournalId,
                    JournalNo = x.JournalNo,
                    VoucherId = x.VoucherId,
                    VoucherType = x.VoucherType,
                    BranchId = x.BranchId,
                    UserId = x.UserId,
                   
                }).ToList();
                return Journals;
            }

        }


        public async Task<IEnumerable<JournalsVM>> GetAllJournals(int FromJournal, int ToJournal , string FromDate, string ToDate,int BranchId)
        {
            try
            {
                var Journals = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && (s.JournalNo >= FromJournal && s.JournalNo <= ToJournal) && s.BranchId == BranchId).Select(x => new JournalsVM
                {
                    JournalId = x.JournalId,
                    JournalNo = x.JournalNo,
                    VoucherId = x.VoucherId,
                    VoucherType = x.VoucherType,
                    BranchId = x.BranchId,
                    UserId = x.UserId,
                    Transactions = x.Invoice.TransactionDetails.Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = x.JournalNo,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit,
                        Credit = tr.Credit,
                        CostCenterName = tr.CostCenters.NameAr ?? "",
                        TypeName = tr.AccTransactionTypes.NameAr ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.Accounts.NameAr,
                        AccountCode = tr.Accounts.Code,
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                     (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                }).ToList();
                return Journals;
            }
            catch (Exception)
            {

                var Journals = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && (s.JournalNo >= FromJournal && s.JournalNo <= ToJournal)).Select(x => new JournalsVM
                {
                    JournalId = x.JournalId,
                    JournalNo = x.JournalNo,
                    VoucherId = x.VoucherId,
                    VoucherType = x.VoucherType,
                    BranchId = x.BranchId,
                    UserId = x.UserId,
                    Transactions = x.Invoice.TransactionDetails.Select(tr => new TransactionsVM
                    {
                        TransactionId = tr.TransactionId,
                        JournalNo = x.JournalNo,
                        LineNumber = tr.LineNumber,
                        Depit = tr.Depit,
                        Credit = tr.Credit,
                        CostCenterName = tr.CostCenters.NameAr ?? "",
                        TypeName = tr.AccTransactionTypes.NameAr ?? "",
                        InvoiceReference = tr.InvoiceReference ?? "",
                        Notes = tr.Notes ?? "",
                        CurrentBalance = tr.CurrentBalance,
                        TransactionDate = tr.TransactionDate,
                        AccountName = tr.Accounts.NameAr,
                        AccountCode = tr.Accounts.Code,
                    }).ToList()
                });
                return Journals;
            }
           
        }
        public async Task< int> GenerateNextJournalNumber(int year, int BranchId)
        {
            var journals = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && s.YearMalia==year /*&& s.BranchId== BranchId*/);
            if (journals != null)
            {

                var lastRow = _TaamerProContext.Journals.Where(s => s.IsDeleted == false && s.YearMalia == year && s.JournalNo!=1).OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault()?.JournalNo ?? 0;
                //int lastRow = _TaamerProContext.Journals.OrderByDescending(u => u.AddDate).Where(w=>w.IsDeleted==false && w.YearMalia==year /*&& w.BranchId==BranchId*/).Select(s => s.JournalNo).DefaultIfEmpty(0).Max();
                if (lastRow >0)
                {
                    return lastRow + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
