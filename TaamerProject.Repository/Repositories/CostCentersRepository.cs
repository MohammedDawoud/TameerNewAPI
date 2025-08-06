using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class CostCentersRepository : ICostCenterRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CostCentersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public CostCenters GetById(int id)
        {
            return _TaamerProContext.CostCenters.Where(x => x.CostCenterId == id).FirstOrDefault();
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCenters(string SearchText, string lang,int BranchId)
        {
            var priorities = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                Code = x.Code,
                CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                ParentId = x.ParentId,
                Level = x.Level ?? 0,
                ParentCostCenterCode = x.ParentCostCenter!.Code ?? "",
                ParentCostCenterName = lang == "ltr" ? x.ParentCostCenter!.NameEn : x.ParentCostCenter.NameAr?? "",
            }).ToList().Select(s => new CostCentersVM()
            {
                CostCenterId = s.CostCenterId,
                Code = s.Code,
                CostCenterName = s.CostCenterName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                ParentId = s.ParentId,
                Level = s.Level == null ? 0 : s.Level,
                ParentCostCenterCode = s.ParentCostCenterCode,
                ParentCostCenterName = s.ParentCostCenterName,
            });

            if (SearchText != "")
            {
                priorities = priorities.Where(k => k.Code.Contains(SearchText.Trim()) || k.NameAr.Contains(SearchText.Trim()) || k.NameEn.Contains(SearchText.Trim()));
            }
            return priorities;
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCenters_B(string SearchText, string lang, int BranchId)
        {
            var priorities = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId==BranchId).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                Code = x.Code,
                CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                ParentId = x.ParentId,
                Level = x.Level ?? 0,
                ParentCostCenterCode = x.ParentCostCenter!.Code ?? "",
                ParentCostCenterName = lang == "ltr" ? x.ParentCostCenter!.NameEn : x.ParentCostCenter.NameAr ?? "",
            }).ToList().Select(s => new CostCentersVM()
            {
                CostCenterId = s.CostCenterId,
                Code = s.Code,
                CostCenterName = s.CostCenterName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                ParentId = s.ParentId,
                Level = s.Level == null ? 0 : s.Level,
                ParentCostCenterCode = s.ParentCostCenterCode,
                ParentCostCenterName = s.ParentCostCenterName,
            });

            if (SearchText != "")
            {
                priorities = priorities.Where(k => k.Code.Contains(SearchText.Trim()) || k.NameAr.Contains(SearchText.Trim()) || k.NameEn.Contains(SearchText.Trim()));
            }
            return priorities;
        }

        public async Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCostBranch(string SearchText, string lang, int BranchId, int CostBranchId)
        {
            // var priorities = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CostCentersVM
            var priorities = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == CostBranchId && (s.ProjId==0 || s.ProjId==null)).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                Code = x.Code,
                CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                ParentId = x.ParentId,
                Level = x.Level ?? 0,
                ParentCostCenterCode = x.ParentCostCenter!.Code ?? "",
                ParentCostCenterName = x.ParentCostCenter!.NameEn ?? "",
            }).ToList().Select(s => new CostCentersVM()
            {
                CostCenterId = s.CostCenterId,
                Code = s.Code,
                CostCenterName = s.CostCenterName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                ParentId = s.ParentId,
                Level = s.Level == null ? 0 : s.Level,
                ParentCostCenterCode = s.ParentCostCenterCode,
                ParentCostCenterName = s.ParentCostCenterName,
            });

            if (SearchText != "")
            {
                priorities = priorities.Where(k => k.Code.Contains(SearchText.Trim()) || k.NameAr.Contains(SearchText.Trim()) || k.NameEn.Contains(SearchText.Trim()));
            }
            return priorities;
        }

        public async Task<CostCentersVM> GetCostCenterById(int CostCenterId)
        {
            var costcenter = _TaamerProContext.CostCenters.Where(s => s.CostCenterId == CostCenterId).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CustomerId=x.CustomerId,
                Level = x.Level,
                ParentCostCenterCode = x.ParentCostCenter!.Code,
                ParentCostCenterName = x.ParentCostCenter.NameAr,
            }).First();
            return costcenter;
        }
        public async Task<CostCentersVM> GetCostCenterByProId(int ProjectId)
        {
            var costcenter = _TaamerProContext.CostCenters.Where(s => s.ProjId == ProjectId).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CustomerId = x.CustomerId,
                Level = x.Level,
                ParentCostCenterCode = x.ParentCostCenter!.Code,
                ParentCostCenterName = x.ParentCostCenter.NameAr,
            }).First();
            return costcenter;
        }

        public async Task<CostCentersVM> GetCostCenterByCode(string Code,string Lang,int BranchId)
        {
            var costcenter = _TaamerProContext.CostCenters.Where(s => s.Code.ToLower().Trim() == Code.ToLower().Trim() && s.BranchId == BranchId && s.IsDeleted == false).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                CostCenterName = Lang == "rtl" ? x.NameAr : x.NameEn ,
                Code = x.Code,
                ParentId = x.ParentId,
                CustomerId = x.CustomerId,
                Level = x.Level,
                ParentCostCenterCode = x.ParentCostCenter!.Code,
                ParentCostCenterName = x.ParentCostCenter.NameAr,
            }).FirstOrDefault();
            return costcenter;
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCentersTransactions(string FromDate, string ToDate, int YearId, string lang, int BranchId)
        {
            try
            {
                var allAccountsTrans = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new
                {
                    CostCenterId=x.CostCenterId,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    x.Code,
                    ParentCostCenterCode = x.ParentCostCenter!.Code,
                    ParentCostCenterName = x.ParentCostCenter!.NameEn,
                    x.Transactions,
                }).ToList().Select(s => new CostCentersVM()
                {
                    CostCenterId=s.CostCenterId,
                    CostCenterName = s.CostCenterName,
                    Code = s.Code,
                    ParentCostCenterCode = s.ParentCostCenterCode,
                    ParentCostCenterName = s.ParentCostCenterName,
                    TotalCredit = _TaamerProContext.Transactions.Where(t =>t.CostCenterId==s.CostCenterId && t.IsDeleted == false && t.YearId == YearId && t.BranchId == BranchId &&(DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit) ?? 0,
                    TotalDepit = _TaamerProContext.Transactions.Where(t =>t.CostCenterId==s.CostCenterId && t.IsDeleted == false && t.YearId == YearId && t.BranchId == BranchId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit) ?? 0,
                }).Select(s => new CostCentersVM()
                {
                    CostCenterName = s.CostCenterName,
                    Code = s.Code,
                    ParentCostCenterCode = s.ParentCostCenterCode,
                    ParentCostCenterName = s.ParentCostCenterName,
                    TotalCredit = s.TotalCredit ?? 0,
                    TotalDepit = s.TotalDepit ?? 0,
                    TotalCreditBalance = (s.TotalCredit - s.TotalDepit) > 0 ? (s.TotalCredit - s.TotalDepit) : 0,
                    TotalDepitBalance = (s.TotalDepit - s.TotalCredit) > 0 ? (s.TotalDepit - s.TotalCredit) : 0,
                }).ToList();
                return allAccountsTrans;
            }
            catch 
            {
                var allAccountsTrans = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new
                {
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    x.Code,
                    ParentCostCenterCode = x.ParentCostCenter!.Code,
                    ParentCostCenterName = x.ParentCostCenter!.NameEn,
                    TotalCredit = _TaamerProContext.Transactions.Where(s => s.CostCenterId == x.CostCenterId && s.IsDeleted == false && s.BranchId == BranchId).Sum(s => s.Credit),
                    TotalDepit = _TaamerProContext.Transactions.Where(s => s.CostCenterId == x.CostCenterId && s.IsDeleted == false && s.BranchId == BranchId).Sum(s => s.Depit)
                }).Select(s => new CostCentersVM()
                {
                    CostCenterName = s.CostCenterName,
                    Code = s.Code,
                    ParentCostCenterCode = s.ParentCostCenterCode,
                    ParentCostCenterName = s.ParentCostCenterName,
                    TotalCredit = s.TotalCredit ?? 0,
                    TotalDepit = s.TotalDepit ?? 0,
                    TotalCreditBalance = (s.TotalCredit - s.TotalDepit) > 0 ? (s.TotalCredit - s.TotalDepit) : 0,
                    TotalDepitBalance = (s.TotalDepit - s.TotalCredit) > 0 ? (s.TotalDepit - s.TotalCredit) : 0,
                }).ToList();
                return allAccountsTrans;
            }

        }

        public async Task<IEnumerable<CostCentersVM>> GetCostCenterTransaction(int BranchId, string lang, int YearId, int CostCenterId, string FromDate, string ToDate)
        {
            try
            {
                var allCostCenter = _TaamerProContext.CostCenters.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == CostCenterId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    CustomerId = x.CustomerId,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&(DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId ).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) : 0,
                    ChildCosrCenters = GetChildsCostCenter(x!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                    Transactions = _TaamerProContext.Transactions.Where(t => t.CostCenterId == x.CostCenterId && t.Type !=35).Select(m => new TransactionsVM
                    {
                        TransactionId = m.TransactionId,
                        LineNumber = m.LineNumber,
                        Depit = m.Depit,
                        Credit = m.Credit,
                        CostCenterName = m.CostCenters!.NameAr ?? "",
                        TypeName = m.AccTransactionTypes!.NameAr ?? "",
                        InvoiceReference = m.InvoiceReference ?? "",
                        Notes = m.Notes ?? "",
                        CurrentBalance = m.CurrentBalance,
                        TransactionDate = m.TransactionDate,
                        AccountName = m.Accounts!.NameAr??"",
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                }).ToList();
                return allCostCenter;
            }
            catch (Exception ex)
            {
                var allCostCenter = _TaamerProContext.CostCenters.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == CostCenterId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    CustomerId = x.CustomerId,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildCosrCenters = GetChildsCostCenter(x!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                    Transactions = _TaamerProContext.Transactions.Where(t => t.CostCenterId == x.CostCenterId).Select(m => new TransactionsVM
                    {
                        TransactionId = m.TransactionId,
                        LineNumber = m.LineNumber,
                        Depit = m.Depit,
                        Credit = m.Credit,
                        CostCenterName = m.CostCenters!.NameAr ?? "",
                        TypeName = m.AccTransactionTypes!.NameAr ?? "",
                        InvoiceReference = m.InvoiceReference ?? "",
                        Notes = m.Notes ?? "",
                        CurrentBalance = m.CurrentBalance,
                        TransactionDate = m.TransactionDate,
                        AccountName = m.Accounts!.NameAr??"",
                    }).ToList()
                }).ToList();
                return allCostCenter;
            }
        }
        public async Task<IEnumerable<CostCentersVM>> GetCostCenterAccountTransaction(int BranchId, string lang, int YearId, int CostCenterId, string FromDate, string ToDate)
        {
            try
            {
                var allCostCenter = _TaamerProContext.CostCenters.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == CostCenterId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    CustomerId = x.CustomerId,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId !=null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),// x.ChildsAccount.Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)), //GetAllChildTotalcredit(x.ChildsAccount,x.IsMain,x.Transactions),// x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),//GetAllChildTotaldepit(x.ChildsAccount, x.IsMain,x.Transactions),//x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true &&  t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    ChildCosrCenters = GetChildsCostCenterAccount(x!.ChildsCostCenter?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                    Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == x.CostCenterId && s.AccountId != null). Select(m => new TransactionsVM
                    {
                        TransactionId = m.TransactionId,
                        LineNumber = m.LineNumber,
                        Depit = m.Depit,
                        Credit = m.Credit,
                        CostCenterName = m.CostCenters!.NameAr ?? "",
                        TypeName = m.AccTransactionTypes!.NameAr ?? "",
                        InvoiceReference = m.InvoiceReference ?? "",
                        Notes = m.Notes ?? "",
                        CurrentBalance = m.CurrentBalance,
                        TransactionDate = m.TransactionDate,
                        AccountName = m.Accounts!.NameAr??"",
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                }).ToList();
                return allCostCenter;
            }
            catch (Exception)
            {
                var allCostCenter = _TaamerProContext.CostCenters.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == CostCenterId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    CustomerId = x.CustomerId,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) : 0,
                    ChildCosrCenters = GetChildsCostCenterAccount(x!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                    Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == x.CostCenterId && s.AccountId != null).Select(m => new TransactionsVM
                    {
                        TransactionId = m.TransactionId,
                        LineNumber = m.LineNumber,
                        Depit = m.Depit,
                        Credit = m.Credit,
                        CostCenterName = m.CostCenters!.NameAr ?? "",
                        TypeName = m.AccTransactionTypes!.NameAr ?? "",
                        InvoiceReference = m.InvoiceReference ?? "",
                        Notes = m.Notes ?? "",
                        CurrentBalance = m.CurrentBalance,
                        TransactionDate = m.TransactionDate,
                        AccountName = m.Accounts!.NameAr??"",
                    }).ToList()
                }).ToList();
                return allCostCenter;
            }
        }
        public async Task<IEnumerable<CostCentersVM>> GetCostCenterReport(int BranchId, string lang, int YearId,string FromDate,string ToDate)
        {
            try
            {
                var allAccounts = _TaamerProContext.CostCenters.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    CustomerId = x.CustomerId,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),// x.ChildsAccount.Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)), //GetAllChildTotalcredit(x.ChildsAccount,x.IsMain,x.Transactions),// x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),//GetAllChildTotaldepit(x.ChildsAccount, x.IsMain,x.Transactions),//x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    ChildCosrCenters = GetChildsCostCenter(x!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList()
                }).ToList();
                return allAccounts;
            }
            catch (Exception)
            {
                var allAccounts = _TaamerProContext.CostCenters.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    CustomerId = x.CustomerId,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == x.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId==x.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildCosrCenters = GetChildsCostCenter(x!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, "", "").Result.ToList()
                }).ToList();
                return allAccounts;
            }
            
        }
        private async Task<List<CostCentersVM>> GetChildsCostCenter(List<CostCenters> costCenters, int BranchId, int YearId,string FromDate,string ToDate)
        {
            List<CostCentersVM> costCentersVm = new List<CostCentersVM>();
            foreach (var item in costCenters)
            {
                try
                {
                    if (item.ChildsCostCenter!= null && item.IsDeleted==false)
                    {
                        costCentersVm.Add(new CostCentersVM
                        {
                            CostCenterId = item.CostCenterId,
                            Code = item.Code,
                            CostCenterName = item.NameAr,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            CustomerId = item.CustomerId,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit)),
                            TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) : 0,
                            Transactions =  _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters!.NameAr ?? "",
                                TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                                AccountName = m.Accounts!.NameAr??"",
                            }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                        });
                    }
                    else
                    {

                        if (item.IsDeleted == false)
                        {

                            costCentersVm.Add(new CostCentersVM
                            {
                                CostCenterId = item.CostCenterId,
                                Code = item.Code,
                                CostCenterName = item.NameAr,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                CustomerId = item.CustomerId,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit)),
                                TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t => t.CostCenterId == item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).ToList().Where(t => (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList().Sum(t => t.Credit))) : 0,
                                ChildCosrCenters = GetChildsCostCenter(item!.ChildsCostCenter?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                                Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters!.NameAr ?? "",
                                    TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                    AccountName = m.Accounts!.NameAr??"",
                                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                            });
                        }

                    }
                }
                catch (Exception)
                {
                    if (item.ChildsCostCenter!= null && item.IsDeleted == false)
                    {
                        costCentersVm.Add(new CostCentersVM
                        {
                            CostCenterId = item.CostCenterId,
                            Code = item.Code,
                            CostCenterName = item.NameAr,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            CustomerId = item.CustomerId,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                            Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters!.NameAr ?? "",
                                TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                                AccountName = m.Accounts!.NameAr??"",
                            }).ToList()
                        });
                    }
                    else
                    {
                        if (item.IsDeleted == false)
                        {
                            costCentersVm.Add(new CostCentersVM
                            {
                                CostCenterId = item.CostCenterId,
                                Code = item.Code,
                                CostCenterName = item.NameAr,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                CustomerId = item.CustomerId,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                ChildCosrCenters = GetChildsCostCenter(item!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                                Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters!.NameAr ?? "",
                                    TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                    AccountName = m.Accounts!.NameAr??"",
                                }).ToList()
                            });

                        }

                    }
                }
              
            }
            return costCentersVm;
        }
        private async Task<List<CostCentersVM>> GetChildsCostCenterAccount(List<CostCenters> costCenters, int BranchId, int YearId, string FromDate, string ToDate)
        {
            List<CostCentersVM> costCentersVm = new List<CostCentersVM>();
            foreach (var item in costCenters)
            {
                try
                {
                    if (item.ChildsCostCenter!= null && item.IsDeleted == false)
                    {
                        costCentersVm.Add(new CostCentersVM
                        {
                            CostCenterId = item.CostCenterId,
                            Code = item.Code,
                            CostCenterName = item.NameAr,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            CustomerId = item.CustomerId,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                            Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId && s.AccountId != null).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters!.NameAr ?? "",
                                TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                                AccountName = m.Accounts!.NameAr??"",
                            }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                        });
                    }
                    else
                    {
                        if (item.IsDeleted == false)
                        {
                            costCentersVm.Add(new CostCentersVM
                            {
                                CostCenterId = item.CostCenterId,
                                Code = item.Code,
                                CostCenterName = item.NameAr,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                ChildCosrCenters = GetChildsCostCenter(item!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                                Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId && s.AccountId != null).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters!.NameAr ?? "",
                                    TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                    AccountName = m.Accounts!.NameAr??"",
                                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                            });

                        }

                    }
                }
                catch (Exception)
                {
                    if (item.ChildsCostCenter!= null && item.IsDeleted == false)
                    {
                        costCentersVm.Add(new CostCentersVM
                        {
                            CostCenterId = item.CostCenterId,
                            Code = item.Code,
                            CostCenterName = item.NameAr,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            CustomerId = item.CustomerId,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit)),
                            TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) : 0,
                            Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId && s.AccountId != null).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters!.NameAr ?? "",
                                TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                                AccountName = m.Accounts!.NameAr??"",
                            }).ToList()
                        });
                    }
                    else
                    {
                        if (item.IsDeleted == false)
                        {
                            costCentersVm.Add(new CostCentersVM
                            {
                                CostCenterId = item.CostCenterId,
                                Code = item.Code,
                                CostCenterName = item.NameAr,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                CustomerId = item.CustomerId,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                TotalBalance = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit)),
                                TotalCredit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CostCenterId == item.CostCenterId).Flatten(i => i.ChildsCostCenter).Sum(s => _TaamerProContext.Transactions.Where(t =>t.CostCenterId== item.CostCenterId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && t.AccountId != null).Sum(t => t.Credit))) : 0,
                                ChildCosrCenters = GetChildsCostCenter(item!.ChildsCostCenter ?? new List<CostCenters>(), BranchId, YearId, FromDate, ToDate).Result.ToList(),
                                Transactions = _TaamerProContext.Transactions.Where(s => s.CostCenterId == item.CostCenterId && s.AccountId != null).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters!.NameAr ?? "",
                                    TypeName = m.AccTransactionTypes!.NameAr ?? "",
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                    AccountName = m.Accounts!.NameAr??"",
                                }).ToList()
                            });

                        }


                    }
                }

            }
            return costCentersVm;
        }



        public async Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCustId(string lang, int BranchId,int? CustId)
        {
            var priorities = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.CustomerId== CustId).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                Code = x.Code,
                CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                ParentId = x.ParentId,
                Level = x.Level,
                ParentCostCenterCode = x.ParentCostCenter!.Code ?? "",
                ParentCostCenterName = x.ParentCostCenter!.NameEn ?? "",
            }).ToList();
            return priorities;
        }

        public async Task<IEnumerable<CostCentersVM>> GetAllCostCenterByCustomers(int BranchId, int custID)
        {        

            if (!String.IsNullOrEmpty( Convert.ToString(BranchId))  && !String.IsNullOrEmpty(Convert.ToString(custID)) )
            {
                var costCenter = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return costCenter;
            }
            else
            {
                var costCenter = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.CostCenterId==0).Select(x => new CostCentersVM
                {
                    CostCenterId = x.CostCenterId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn
                }).ToList();
                return costCenter;
            }
        }
        //heba
        public async Task<DataTable> TreeViewOfCostCenter(string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "select * from CostCenters";
                        command.Connection = con;

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }


                return dt;
            }
            catch (Exception ex)
            {
                var x = ex.Message;

                return new DataTable();
            }

        }


        public async Task<CostCentersVM> GetBranch_Costcenter(string lang, int BranchId)
        {
            var priorities = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ProjId == 0 && s.ParentId==null).Select(x => new CostCentersVM
            {
                CostCenterId = x.CostCenterId,
                Code = x.Code,
                CostCenterName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                ParentId = x.ParentId,
                Level = x.Level ?? 0,
                ParentCostCenterCode = x.ParentCostCenter!.Code ?? "",
                ParentCostCenterName = lang == "ltr" ? x.ParentCostCenter!.NameEn : x.ParentCostCenter.NameAr ?? "",
            }).ToList().Select(s => new CostCentersVM()
            {
                CostCenterId = s.CostCenterId,
                Code = s.Code,
                CostCenterName = s.CostCenterName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                ParentId = s.ParentId,
                Level = s.Level == null ? 0 : s.Level,
                ParentCostCenterCode = s.ParentCostCenterCode,
                ParentCostCenterName = s.ParentCostCenterName,
            }).FirstOrDefault();

            return priorities;
        }

        public IEnumerable<CostCenters> GetAll()
        {
            return _TaamerProContext.CostCenters.ToList<CostCenters>();
        }

        public IEnumerable<CostCenters> GetMatching(Func<CostCenters, bool> where)
        {
            return _TaamerProContext.CostCenters.Where(where).ToList<CostCenters>();

        }
    }
}
