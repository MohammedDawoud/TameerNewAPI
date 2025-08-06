using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using System.Data;
using System.Data.SqlClient;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using Microsoft.IdentityModel.Tokens;

namespace TaamerProject.Repository.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AccountsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public Accounts GetById(int AccountId) //ada 
        {
           return _TaamerProContext.Accounts.Where(x => x.AccountId == AccountId).FirstOrDefault();
        }
        public async Task<List<AccountVM>> GetAllAccounts(string SearchText, string lang, int BranchId)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                Active = x.Active,
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate=x.OpenAccCreditDate,
                OpenAccDepitDate=x.OpenAccDepitDate,
            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Active = s.Active,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",

                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                ExpensesAccId = s.ExpensesAccId ?? 0,
                AccountIdAhlak = s.AccountIdAhlak ?? 0,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,

            });
            if (SearchText != "" || SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code != null && (s.Code.Contains(SearchText.Trim()) || s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())));
            }
            var GroupByMS = allAccounts.GroupBy(s => s.ParentId);
            //Using Query Syntax
            //IEnumerable<IGrouping<string, AccountVM>> GroupByQS = (from std in allAccounts
            //                                                       group std by std.ParentId);
            //It will iterate through each groups
            List<AccountVM> accounts = new List<AccountVM>();
           foreach (var group in GroupByMS)
            {
               
                Console.WriteLine(group.Key + " : " + group.Count());
                //Iterate through each student of a group
                foreach (var ac in group)
                {
                    AccountVM acc = new AccountVM();
                    
                    acc.AccountId = ac.AccountId;
                    acc.AccountName = ac.AccountName;
                    acc.NameAr = ac.NameAr;
                    acc.NameEn = ac.NameEn;
                    acc.Code = ac.Code;
                    acc.ParentId = ac.ParentId;
                acc.CurrencyId = ac.CurrencyId;
                acc.Halala = ac.Halala;
                acc.IsMain = ac.IsMain;
                acc.Active = ac.Active;
                acc.Classification = ac.Classification;
                    acc.ClassificationName = ac.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                              ac.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "ايرادات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : ac.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "";

                    acc.Level = ac.Level;
                acc.Nature = ac.Nature;
                acc.DepitOrCredit = ac.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : ac.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "";
                    acc.Type = ac.Type;
                acc.TypeName = ac.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : ac.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : ac.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           ac.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : ac.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "";
                acc.Notes = ac.Notes;
                acc.ParentAccountCode = ac.ParentAccountCode ?? "";
                acc.ParentAccountName = ac.ParentAccountName ?? "";
                acc.ExpensesAccId = ac.ExpensesAccId ?? 0;
                    acc.AccountIdAhlak = ac.AccountIdAhlak ?? 0;
                    acc.OpenAccCreditDate = ac.OpenAccCreditDate;
                    acc.OpenAccDepitDate = ac.OpenAccDepitDate;
                    accounts.Add(acc); 
                    
                }
            }


                return accounts;
        }
        public async Task<List<AccountVM>> GetAccountTreeIncome(string SearchText, string lang, int BranchId)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.ParentId!=97).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                Active = x.Active,
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Active = s.Active,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",

                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                ExpensesAccId = s.ExpensesAccId ?? 0,
                AccountIdAhlak = s.AccountIdAhlak ?? 0,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,

            });
            if (SearchText != "" || SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code != null && (s.Code.Contains(SearchText.Trim()) || s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())));
            }
            var GroupByMS = allAccounts.GroupBy(s => s.ParentId);
            //Using Query Syntax
            //IEnumerable<IGrouping<string, AccountVM>> GroupByQS = (from std in allAccounts
            //                                                       group std by std.ParentId);
            //It will iterate through each groups
            List<AccountVM> accounts = new List<AccountVM>();
            foreach (var group in GroupByMS)
            {

                Console.WriteLine(group.Key + " : " + group.Count());
                //Iterate through each student of a group
                foreach (var ac in group)
                {
                    AccountVM acc = new AccountVM();

                    acc.AccountId = ac.AccountId;
                    acc.AccountName = ac.AccountName;
                    acc.NameAr = ac.NameAr;
                    acc.NameEn = ac.NameEn;
                    acc.Code = ac.Code;
                    acc.ParentId = ac.ParentId;
                    acc.CurrencyId = ac.CurrencyId;
                    acc.Halala = ac.Halala;
                    acc.IsMain = ac.IsMain;
                    acc.Active = ac.Active;
                    acc.Classification = ac.Classification;
                    acc.ClassificationName = ac.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                              ac.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "ايرادات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : ac.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "";

                    acc.Level = ac.Level;
                    acc.Nature = ac.Nature;
                    acc.DepitOrCredit = ac.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : ac.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "";
                    acc.Type = ac.Type;
                    acc.TypeName = ac.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : ac.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : ac.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                               ac.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : ac.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "";
                    acc.Notes = ac.Notes;
                    acc.ParentAccountCode = ac.ParentAccountCode ?? "";
                    acc.ParentAccountName = ac.ParentAccountName ?? "";
                    acc.ExpensesAccId = ac.ExpensesAccId ?? 0;
                    acc.AccountIdAhlak = ac.AccountIdAhlak ?? 0;
                    acc.OpenAccCreditDate = ac.OpenAccCreditDate;
                    acc.OpenAccDepitDate = ac.OpenAccDepitDate;
                    accounts.Add(acc);

                }
            }


            return accounts;
        }


        public async Task<IEnumerable<AccountVM>> GetAllAccountsWithChild(int AccountID)
        {
            var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && (s.AccountId == AccountID || s.ParentId == AccountID)).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                Code = x.Code,
                AccountName = x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Type = x.Type,
                Nature = x.Nature,
                ParentId = x.ParentId,
                Level = x.Level,
                CurrencyId = x.CurrencyId,
                Classification = x.Classification,
                Halala = x.Halala,
                Active = x.Active,
                BranchId = x.BranchId,
                IsMain = x.IsMain,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                DepitOrCredit = "",
                TypeName = "",
                ClassificationName = "",
                //TotalCredit = x.IsMain == false ? (x.Transactions.Where(t => t.IsDeleted == false).Sum(s => s.Credit)) : (x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false).Sum(t => t.Credit))),
                //TotalDepit = x.IsMain == false ? (x.Transactions.Where(t => t.IsDeleted == false).Sum(s => s.Depit)) : (x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false).Sum(t => t.Depit))),
                TotalCreditOpeningBalance = 0,
                TotalDepitOpeningBalance = 0,
                TotalCreditBalance = 0,
                TotalDepitBalance = 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
                ChildAccounts = x.ChildsAccount!=null? x.ChildsAccount!.Select(p => new AccountVM
                {
                    AccountId = p.AccountId,
                    AccountName = p.NameAr,
                    Code = p.Code,
                    NameAr = p.NameAr,
                    NameEn = p.NameEn,
                    Type = x.Type,
                    Nature = p.Nature,
                    ParentId = p.ParentId,
                    Level = p.Level,
                    CurrencyId = p.CurrencyId,
                    Classification = p.Classification,
                    Halala = p.Halala,
                    Active = p.Active,
                    BranchId = p.BranchId,
                    IsMain = p.IsMain,
                    Notes = p.Notes,
                    ParentAccountCode = p.ParentAccount != null ? p.ParentAccount.Code : "",
                    ParentAccountName = p.ParentAccount != null ? p.ParentAccount.NameEn : "",
                    DepitOrCredit = "",
                    TypeName = "",
                    ClassificationName = "",
                    //TotalCredit = p.IsMain == false ? (p.Transactions.Where(t => t.IsDeleted == false).Sum(s => s.Credit)) : (p.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false).Sum(t => t.Credit))),
                    //TotalDepit = p.IsMain == false ? (p.Transactions.Where(t => t.IsDeleted == false).Sum(s => s.Depit)) : (p.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false).Sum(t => t.Depit))),
                    TotalCreditOpeningBalance = 0,
                    TotalDepitOpeningBalance = 0,
                    TotalCreditBalance = 0,
                    TotalDepitBalance = 0,
                    OpenAccCreditDate = x.OpenAccCreditDate,
                    OpenAccDepitDate = x.OpenAccDepitDate,
                    ChildAccounts = p.ChildsAccount != null ? p.ChildsAccount!.Select(t => new AccountVM
                    {
                        AccountId = t.AccountId,
                        AccountName = t.NameAr,
                        Code = t.Code,
                        NameAr = t.NameAr,
                        NameEn = t.NameEn,
                        Type = t.Type,
                        Nature = t.Nature,
                        ParentId = t.ParentId,
                        Level = t.Level,
                        CurrencyId = t.CurrencyId,
                        Classification = t.Classification,
                        Halala = t.Halala,
                        Active = t.Active,
                        BranchId = t.BranchId,
                        IsMain = t.IsMain,
                        Notes = t.Notes,
                        ParentAccountCode = t.ParentAccount != null ? t.ParentAccount.Code : "",
                        ParentAccountName = t.ParentAccount != null ? t.ParentAccount.NameEn : "",
                        DepitOrCredit = "",
                        TypeName = "",
                        ClassificationName = "",
                        TotalCredit = t.Transactions!=null? t.Transactions!.Where(c => c.IsDeleted == false).Sum(s => s.Credit)??0:0,
                        TotalDepit = t.Transactions != null ? t.Transactions!.Where(c => c.IsDeleted == false).Sum(s => s.Depit)??0:0,
                        TotalCreditOpeningBalance = 0,
                        TotalDepitOpeningBalance = 0,
                        OpenAccCreditDate = x.OpenAccCreditDate,
                        OpenAccDepitDate = x.OpenAccDepitDate,
                        //TotalCreditBalance = (t.Transactions!.Where(c => c.IsDeleted == false).Sum(s => s.Depit) - t.Transactions!.Where(c => c.IsDeleted == false).Sum(s => s.Credit)) < 0 ? -(t.Transactions!.Where(c => t.IsDeleted == false).Sum(s => s.Depit) - t.Transactions!.Where(c => t.IsDeleted == false).Sum(s => s.Credit)) : 0,
                        //TotalDepitBalance = (t.Transactions!.Where(c => c.IsDeleted == false).Sum(s => s.Depit) - t.Transactions!.Where(c => c.IsDeleted == false).Sum(s => s.Credit)) > 0 ? (t.Transactions!.Where(c => t.IsDeleted == false).Sum(s => s.Depit) - t.Transactions!.Where(c => t.IsDeleted == false).Sum(s => s.Credit)) : 0,
                        ChildAccounts = t.ChildsAccount != null ? t.ChildsAccount!.Select(f => new AccountVM
                        {
                        }).ToList() : new List<AccountVM>(),
                    }).ToList() : new List<AccountVM>(),
                }).ToList():new List<AccountVM>()
            });
            return allAccounts;
        }


        public async Task<List<AccountVM>> GetAllAccountsCustomerBranch(string SearchText, string lang, int BranchId,int CustomerParentAcc)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && ((s.AccountId == CustomerParentAcc) || (s.ParentId == CustomerParentAcc && s.BranchId == BranchId))).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                Active = x.Active,
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Active = s.Active,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",

                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                ExpensesAccId = s.ExpensesAccId ?? 0,
                AccountIdAhlak = s.AccountIdAhlak ?? 0,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,

            });
            if (SearchText != "" || SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code != null && (s.Code.Contains(SearchText.Trim()) || s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())));
            }
            var GroupByMS = allAccounts.GroupBy(s => s.ParentId);
            //Using Query Syntax
            //IEnumerable<IGrouping<string, AccountVM>> GroupByQS = (from std in allAccounts
            //                                                       group std by std.ParentId);
            //It will iterate through each groups
            List<AccountVM> accounts = new List<AccountVM>();
            foreach (var group in GroupByMS)
            {

                Console.WriteLine(group.Key + " : " + group.Count());
                //Iterate through each student of a group
                foreach (var ac in group)
                {
                    AccountVM acc = new AccountVM();

                    acc.AccountId = ac.AccountId;
                    acc.AccountName = ac.AccountName;
                    acc.NameAr = ac.NameAr;
                    acc.NameEn = ac.NameEn;
                    acc.Code = ac.Code;
                    acc.ParentId = ac.ParentId;
                    acc.CurrencyId = ac.CurrencyId;
                    acc.Halala = ac.Halala;
                    acc.IsMain = ac.IsMain;
                    acc.Active = ac.Active;
                    acc.Classification = ac.Classification;
                    acc.ClassificationName = ac.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                              ac.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "ايرادات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                               ac.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : ac.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : ac.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "";

                    acc.Level = ac.Level;
                    acc.Nature = ac.Nature;
                    acc.DepitOrCredit = ac.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : ac.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "";
                    acc.Type = ac.Type;
                    acc.TypeName = ac.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : ac.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : ac.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                               ac.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : ac.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "";
                    acc.Notes = ac.Notes;
                    acc.ParentAccountCode = ac.ParentAccountCode ?? "";
                    acc.ParentAccountName = ac.ParentAccountName ?? "";
                    acc.ExpensesAccId = ac.ExpensesAccId ?? 0;
                    acc.AccountIdAhlak = ac.AccountIdAhlak ?? 0;
                    acc.OpenAccCreditDate = ac.OpenAccCreditDate;
                    acc.OpenAccDepitDate = ac.OpenAccDepitDate;

                    accounts.Add(acc);

                }
            }


            return accounts;
        }

        public async Task<IEnumerable<AccountVM>> GetAllAccountsOpening(string SearchText, string lang, int BranchId)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                IsMain=x.IsMain,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                IsMain = s.IsMain,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,
            });
            if (SearchText != "" && SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code != null && (s.Code.Contains(SearchText.Trim()) || s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim())));
            }
            return allAccounts;
        }
        public async Task<IEnumerable<AccountVM>> GetAllDelAccounts(string SearchText)
        {
            // var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new AccountVM
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == true).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                Active = x.Active,
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,

            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Active = s.Active,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",
                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                ExpensesAccId = s.ExpensesAccId ?? 0,
                AccountIdAhlak = s.AccountIdAhlak ?? 0,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,
            });
            if (SearchText != "" || SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code.Contains(SearchText.Trim()) || s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim()));
            }
            return allAccounts.ToList();
        }



        public async Task<IEnumerable<AccountVM>> GetAllAccounts2(string SearchText, string lang, int BranchId)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.Code.Contains("5")).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                Active = x.Active,
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,

            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Active = s.Active,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",
                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                ExpensesAccId = s.ExpensesAccId ?? 0,
                AccountIdAhlak = s.AccountIdAhlak ?? 0,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,

            });
            if (SearchText != "" || SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code!.Contains(SearchText.Trim()) || s.NameAr!.Contains(SearchText.Trim()) || s.NameEn!.Contains(SearchText.Trim()));
            }
            return allAccounts;
        }

        public async Task<IEnumerable<AccountVM>> GetAllSubAccounts(string SearchText, string lang, int BranchId)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.IsMain == false).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = x.NameAr != null ? x.NameEn != null ? (lang == "ltr" ? x.NameEn : x.NameAr):"":"",
                NameAr = x.NameAr != null ? x.NameAr:"",
                NameEn = x.NameEn != null ? x.NameEn:"",
                Code = x.Code != null ? x.Code:"",
                ParentId = x.ParentId != null ? x.ParentId : 0,
                CurrencyId = x.CurrencyId != null ? x.CurrencyId:0,
                Halala = x.Halala != null ? x.Halala : false,
                IsMain = x.IsMain != null ? x.IsMain : false,
                Classification = x.Classification!=null?x.Classification:0,
                Level = x.Level != null ? x.Level : 0,
                Nature = x.Nature != null ? x.Nature : 0,
                Active = x.Active != null ? x.Active : false,
                Type = x.Type != null ? x.Type:0,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,

            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",
                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                ExpensesAccId = s.ExpensesAccId ?? 0,
                AccountIdAhlak = s.AccountIdAhlak ?? 0,
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,

            });
            if (SearchText != "")
            {
                allAccounts = allAccounts.Where(e => e.Code!.Contains(SearchText.Trim()) || e.NameAr!.Contains(SearchText.Trim()) || e.NameEn!.Contains(SearchText.Trim()));
            }
            return allAccounts;
        }
        public async Task<AccountVM> GetAccountById(int accountId)
        {
            var acc = _TaamerProContext.Accounts.Where(s => s.AccountId == accountId).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Active = x.Active,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                ExpensesAccId = x.ExpensesAccId ?? 0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccDepit = x.OpenAccDepit ?? 0,
                OpenAccCredit = x.OpenAccCredit ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,

            }).FirstOrDefault();
            return acc;
        }
        public async Task<AccountVM> GetAccountByClassificationParent(int classification)
        {
            try
            {
                var acc = _TaamerProContext.Accounts.Where(s => s.Classification == classification).Select(x => new AccountVM
                {
                    AccountId = x.AccountId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Code = x.Code,
                    ParentId = x.ParentId,
                    CurrencyId = x.CurrencyId,
                    Halala = x.Halala,
                    IsMain = x.IsMain,
                    Classification = x.Classification,
                    Level = x.Level,
                    Nature = x.Nature,
                    Active = x.Active,
                    Type = x.Type,
                    Notes = x.Notes,
                    ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                    ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                    ExpensesAccId = x.ExpensesAccId ?? 0,
                    AccountIdAhlak = x.AccountIdAhlak ?? 0,
                    OpenAccCreditDate = x.OpenAccCreditDate,
                    OpenAccDepitDate = x.OpenAccDepitDate,

                }).First();
                return acc;
            }
            catch (Exception ex)
            {
                AccountVM lmd = new AccountVM();
                return lmd;
            }
            
        }

        public async Task<int> GetMaxId()
        {
            return (_TaamerProContext.Accounts.Count() > 0) ? _TaamerProContext.Accounts.Max(s => s.AccountId) : 0;
        }
        public async Task<AccountVM> GetAccountByCode(string Code, string Lang, int BranchId)
        {
            var account = _TaamerProContext.Accounts.Where(s => s.Code.ToLower().Trim() == Code.ToLower().Trim() /*&& s.BranchId == BranchId*/ && s.IsDeleted == false).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = Lang == "rtl" ? x.NameAr : x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                Level = x.Level,
                Nature = x.Nature,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                Type =x.Type??0,
                Classification=x.Classification??0,
                AccountIdAhlak = x.AccountIdAhlak ?? 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
            }).FirstOrDefault();
            return account;
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountsTransactions(string FromDate, string ToDate, int YearId, string lang, int BranchId)
        {
            try
            {
                var allAccountsTrans = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new
                {
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    x.Code,
                    ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                    ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                    x.Notes,
                    x.Transactions,
                }).ToList().Select(s => new AccountVM()
                {
                    AccountName = s.AccountName,
                    Code = s.Code,
                    Notes = s.Notes,
                    ParentAccountCode = s.ParentAccountCode,
                    ParentAccountName = s.ParentAccountName,
                    TotalCredit =s.Transactions!=null? s.Transactions!.Where(t => t.IsDeleted == false && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit) ?? 0:0,
                    TotalDepit =s.Transactions!=null? s.Transactions!.Where(t => t.IsDeleted == false && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit) ?? 0:0,
                }).Select(s => new AccountVM()
                {
                    AccountName = s.AccountName,
                    Code = s.Code,
                    Notes = s.Notes,
                    ParentAccountCode = s.ParentAccountCode,
                    ParentAccountName = s.ParentAccountName,
                    TotalCredit = s.TotalCredit ?? 0,
                    TotalDepit = s.TotalDepit ?? 0,
                    TotalCreditBalance = (s.TotalCredit - s.TotalDepit) > 0 ? (s.TotalCredit - s.TotalDepit) : 0,
                    TotalDepitBalance = (s.TotalDepit - s.TotalCredit) > 0 ? (s.TotalDepit - s.TotalCredit) : 0,
                }).ToList();
                return allAccountsTrans;
            }
            catch
            {
                var allAccountsTrans = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new
                {
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    x.Code,
                    ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                    ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                    x.Notes,
                    TotalCredit =x.Transactions!=null?x.Transactions!.Where(s => s.IsDeleted == false).Sum(s => s.Credit):0,
                    TotalDepit =x.Transactions!=null? x.Transactions!.Where(s => s.IsDeleted == false).Sum(s => s.Depit):0
                }).Select(s => new AccountVM()
                {
                    AccountName = s.AccountName,
                    Code = s.Code,
                    Notes = s.Notes,
                    ParentAccountCode = s.ParentAccountCode,
                    ParentAccountName = s.ParentAccountName,
                    TotalCredit = s.TotalCredit ?? 0,
                    TotalDepit = s.TotalDepit ?? 0,
                    TotalCreditBalance = (s.TotalCredit - s.TotalDepit) > 0 ? (s.TotalCredit - s.TotalDepit) : 0,
                    TotalDepitBalance = (s.TotalDepit - s.TotalCredit) > 0 ? (s.TotalDepit - s.TotalCredit) : 0,
                }).ToList();
                return allAccountsTrans;
            }

        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountsTransactionsByAccType(int AccType, string FromDate, string ToDate, int YearId, string lang, int BranchId)
        {
            try
            {
                var allAccountsTrans = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.Type == AccType && s.BranchId == BranchId).Select(x => new
                {
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    x.Code,
                    ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                    ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                    x.Notes,
                    x.Transactions,
                }).ToList().Select(s => new AccountVM()
                {
                    AccountName = s.AccountName,
                    Code = s.Code,
                    Notes = s.Notes,
                    ParentAccountCode = s.ParentAccountCode,
                    ParentAccountName = s.ParentAccountName,
                    TotalCredit =s.Transactions!=null? s.Transactions!.Where(t => t.IsDeleted == false && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit) ?? 0:0,
                    TotalDepit =s.Transactions!=null? s.Transactions!.Where(t => t.IsDeleted == false && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit) ?? 0:0,
                }).Select(s => new AccountVM()
                {
                    AccountName = s.AccountName,
                    Code = s.Code,
                    Notes = s.Notes,
                    ParentAccountCode = s.ParentAccountCode,
                    ParentAccountName = s.ParentAccountName,
                    TotalCredit = s.TotalCredit ?? 0,
                    TotalDepit = s.TotalDepit ?? 0,
                    TotalCreditBalance = (s.TotalCredit - s.TotalDepit) > 0 ? (s.TotalCredit - s.TotalDepit) : 0,
                    TotalDepitBalance = (s.TotalDepit - s.TotalCredit) > 0 ? (s.TotalDepit - s.TotalCredit) : 0,
                }).ToList();
                return allAccountsTrans;
            }
            catch
            {
                var allAccountsTrans = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.Type == AccType && s.BranchId == BranchId).Select(x => new
                {
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    x.Code,
                    ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                    ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                    x.Notes,
                    TotalCredit =x.Transactions!=null? x.Transactions!.Where(s => s.IsDeleted == false).Sum(s => s.Credit):0,
                    TotalDepit =x.Transactions!=null? x.Transactions!.Where(s => s.IsDeleted == false).Sum(s => s.Depit): 0
                }).Select(s => new AccountVM()
                {
                    AccountName = s.AccountName,
                    Code = s.Code,
                    Notes = s.Notes,
                    ParentAccountCode = s.ParentAccountCode,
                    ParentAccountName = s.ParentAccountName,
                    TotalCredit = s.TotalCredit,
                    TotalDepit = s.TotalDepit,
                    TotalCreditBalance = (s.TotalCredit - s.TotalDepit) > 0 ? (s.TotalCredit - s.TotalDepit) : 0,
                    TotalDepitBalance = (s.TotalDepit - s.TotalCredit) > 0 ? (s.TotalDepit - s.TotalCredit) : 0,
                }).ToList();
                return allAccountsTrans;
            }

        }
        public async Task<IEnumerable<AccountVM>> GetAllReceiptExchangeAccounts(string SearchText, string lang, int BranchId, List<int> ReceiptExchangeAccIds)
        {
            var allAccounts = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && ReceiptExchangeAccIds.Contains(s.AccountId) && s.IsMain == false && s.BranchId == BranchId).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                ParentId = x.ParentId,
                CurrencyId = x.CurrencyId,
                Halala = x.Halala,
                IsMain = x.IsMain,
                Classification = x.Classification,
                Level = x.Level,
                Nature = x.Nature,
                Type = x.Type,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameAr : "",
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
            }).ToList().Select(s => new AccountVM()
            {
                AccountId = s.AccountId,
                AccountName = s.AccountName,
                NameAr = s.NameAr,
                NameEn = s.NameEn,
                Code = s.Code,
                ParentId = s.ParentId,
                CurrencyId = s.CurrencyId,
                Halala = s.Halala,
                IsMain = s.IsMain,
                Classification = s.Classification,
                ClassificationName = s.Classification == Convert.ToInt32(AccountClassifyTypes.Box) ? "صندوق" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Customer) ? "عميل" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Supplier) ? " مورد" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Purchases) ? "مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.PurchasesReturns) ? " مردود مشتريات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Bank) ? "بنك" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Employee) ? "موظف" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Otehers) ? "  أخري" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Tax) ? "ضريبة" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Sales) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.SalesReturns) ? "  مردود مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.ProfitablePofits) ? "أرباح مدوره" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.FirstTimeGoods) ? "بضاعة أول المدة" : s.Classification == Convert.ToInt32(AccountClassifyTypes.GeneralCustomer) ? "  عميل عام " : s.Classification == Convert.ToInt32(AccountClassifyTypes.Assets) ? "أصول" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.Liabilities) ? "خصوم" : s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxIn) ? "ضريبة المدخلات" :
                           s.Classification == Convert.ToInt32(AccountClassifyTypes.TaxOut) ? "  ضريبة المخرجات " : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostS) ? "مصروفات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.CostE) ? "مبيعات" : s.Classification == Convert.ToInt32(AccountClassifyTypes.Rights) ? "حقوق ملكية" : "",
                Level = s.Level,
                Nature = s.Nature,
                DepitOrCredit = s.Nature == Convert.ToInt32(AccountNature.Depit) ? "مدين" : s.Nature == Convert.ToInt32(AccountNature.Credit) ? "دائن" : "",
                Type = s.Type,
                TypeName = s.Type == Convert.ToInt32(AccountTypes.None) ? "بدون" : s.Type == Convert.ToInt32(AccountTypes.Budget) ? "ميزانية" : s.Type == Convert.ToInt32(AccountTypes.IncomeStatment) ? "قائمة الدخل" :
                           s.Type == Convert.ToInt32(AccountTypes.Commerce) ? "متاجرة" : s.Type == Convert.ToInt32(AccountTypes.ProfitLoss) ? "أرباح وخسائر" : "",
                Notes = s.Notes,
                ParentAccountCode = s.ParentAccountCode ?? "",
                ParentAccountName = s.ParentAccountName ?? "",
                OpenAccCreditDate = s.OpenAccCreditDate,
                OpenAccDepitDate = s.OpenAccDepitDate,
            });
            if (SearchText != "" || SearchText != null)
            {
                allAccounts = allAccounts.Where(s => s.Code.Contains(SearchText.Trim()) || s.NameAr.Contains(SearchText.Trim()) || s.NameEn.Contains(SearchText.Trim()));
            }
            return allAccounts;
        }
        public async Task<IEnumerable<AccountVM>> GetAllHirearchialAccounts(int BranchId, string lang)
        {
            var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.IsMain == true && s.BranchId == BranchId).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                Code = x.Code,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Type = x.Type,
                Nature = x.Nature,
                ParentId = x.ParentId,
                Level = x.Level,
                CurrencyId = x.CurrencyId,
                Classification = x.Classification,
                Halala = x.Halala,
                Active = x.Active,
                BranchId = x.BranchId,
                IsMain = x.IsMain,
                Notes = x.Notes,
                ParentAccountCode = x.ParentAccount != null ? x.ParentAccount.Code : "",
                ParentAccountName = x.ParentAccount != null ? x.ParentAccount.NameEn : "",
                DepitOrCredit = "",
                TypeName = "",
                ClassificationName = "",
                TotalCredit = x.IsMain == false ? x.Transactions!=null? (x.Transactions!.Where(t => t.IsDeleted == false).Sum(s => s.Credit)) : (x.ChildsAccount!.Sum(s => s.Transactions!.Where(t => t.IsDeleted == false).Sum(t => t.Credit)))??0:0,
                TotalDepit = x.IsMain == false ? x.Transactions != null ? (x.Transactions!.Where(t => t.IsDeleted == false).Sum(s => s.Depit)) : (x.ChildsAccount!.Sum(s => s.Transactions!.Where(t => t.IsDeleted == false).Sum(t => t.Depit)))??0:0,
                TotalCreditOpeningBalance = 0,
                TotalDepitOpeningBalance = 0,
                TotalCreditBalance = 0,
                TotalDepitBalance = 0,
                OpenAccCreditDate = x.OpenAccCreditDate,
                OpenAccDepitDate = x.OpenAccDepitDate,
                ChildAccounts = x.ChildsAccount!=null? x.ChildsAccount.Select(p => new AccountVM
                {
                    AccountId = p.AccountId,
                    AccountName = lang == "ltr" ? p.NameEn : p.NameAr,
                    Code = p.Code,
                    NameAr = p.NameAr,
                    NameEn = p.NameEn,
                    Type = x.Type,
                    Nature = p.Nature,
                    ParentId = p.ParentId,
                    Level = p.Level,
                    CurrencyId = p.CurrencyId,
                    Classification = p.Classification,
                    Halala = p.Halala,
                    Active = p.Active,
                    BranchId = p.BranchId,
                    IsMain = p.IsMain,
                    Notes = p.Notes,
                    ParentAccountCode = p.ParentAccount != null ? p.ParentAccount.Code : "",
                    ParentAccountName = p.ParentAccount != null ? p.ParentAccount.NameEn : "",
                    DepitOrCredit = "",
                    TypeName = "",
                    ClassificationName = "",
                    TotalCredit = p.IsMain == false ? p.Transactions != null ? (p.Transactions.Where(t => t.IsDeleted == false).Sum(s => s.Credit)) : (p.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==p.AccountId && t.IsDeleted == false).Sum(t => t.Credit)))??0:0,
                    TotalDepit = p.IsMain == false ? p.Transactions != null ? (p.Transactions.Where(t => t.IsDeleted == false).Sum(s => s.Depit)) : (p.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==p.AccountId && t.IsDeleted == false).Sum(t => t.Depit)))??0:0,
                    TotalCreditOpeningBalance = 0,
                    TotalDepitOpeningBalance = 0,
                    TotalCreditBalance = 0,
                    TotalDepitBalance = 0,
                    OpenAccCreditDate = p.OpenAccCreditDate,
                    OpenAccDepitDate = p.OpenAccDepitDate,
                    ChildAccounts = p.ChildsAccount != null ? p.ChildsAccount.Select(t => new AccountVM
                    {
                        AccountId = t.AccountId,
                        AccountName = lang == "ltr" ? t.NameEn : t.NameAr,
                        Code = t.Code,
                        NameAr = t.NameAr,
                        NameEn = t.NameEn,
                        Type = t.Type,
                        Nature = t.Nature ,
                        ParentId = t.ParentId,
                        Level = t.Level,
                        CurrencyId = t.CurrencyId,
                        Classification = t.Classification,
                        Halala = t.Halala,
                        Active = t.Active,
                        BranchId = t.BranchId,
                        IsMain = t.IsMain,
                        Notes = t.Notes,
                        ParentAccountCode = t.ParentAccount != null ? t.ParentAccount.Code : "",
                        ParentAccountName = t.ParentAccount != null ? t.ParentAccount.NameEn : "",
                        DepitOrCredit = "",
                        TypeName = "",
                        ClassificationName = "",
                        TotalCredit = t.Transactions!=null? t.Transactions.Where(c => c.IsDeleted == false).Sum(s => s.Credit)??0:0,
                        TotalDepit = t.Transactions != null ? t.Transactions.Where(c => c.IsDeleted == false).Sum(s => s.Depit)??0:0,
                        TotalCreditOpeningBalance = 0,
                        TotalDepitOpeningBalance = 0,
                        OpenAccCreditDate = t.OpenAccCreditDate,
                        OpenAccDepitDate = t.OpenAccDepitDate,
                        TotalCreditBalance = t.Transactions!=null?(t.Transactions.Where(c => c.IsDeleted == false).Sum(s => s.Depit) - t.Transactions.Where(c => c.IsDeleted == false).Sum(s => s.Credit)) < 0 ? -(t.Transactions.Where(c => t.IsDeleted == false).Sum(s => s.Depit) - t.Transactions.Where(c => t.IsDeleted == false).Sum(s => s.Credit)) : 0:0,
                        TotalDepitBalance = t.Transactions != null ? (t.Transactions.Where(c => c.IsDeleted == false).Sum(s => s.Depit) - t.Transactions.Where(c => c.IsDeleted == false).Sum(s => s.Credit)) > 0 ? (t.Transactions.Where(c => t.IsDeleted == false).Sum(s => s.Depit) - t.Transactions.Where(c => t.IsDeleted == false).Sum(s => s.Credit)) : 0:0,
                        ChildAccounts = t.ChildsAccount!=null? t.ChildsAccount.Select(f => new AccountVM
                        {
                        }).ToList():new List<AccountVM>(),
                    }).ToList() : new List<AccountVM>(),
                }).ToList() : new List<AccountVM>()
            });
            return allAccounts;
        }

        public async Task<string> GetNewCodeByParentId(int ParentId,int Type)
        {
            //Type=0 normal acc,, Type=1 cust acc,, Type=2 emp acc,, Type=3 sup acc
            var childList = _TaamerProContext.Accounts.Where(x => x.ParentId == ParentId && !x.IsDeleted).ToList();
            if (childList.Count() == 0)
            {
                var parentAccount = _TaamerProContext.Accounts.Where(x => x.AccountId == ParentId).FirstOrDefault()!;
                var parentAccountCode = parentAccount.Code;
                var Code = "01";
                
                if(parentAccount.Level==1){ Code = "01"; }
                else if (parentAccount.Level == 2) {
                    if (Type == 3){Code = "0001"; }else{Code = "001";}               
                }
                else if (parentAccount.Level == 3)
                {
                    if (Type == 3) { Code = "0001"; } else { Code = "001"; }
                }
                else if (parentAccount.Level == 4)
                {
                    if (Type == 1) { Code = "001"; } else { Code = "01"; }
                }
                else if (parentAccount.Level == 5)
                {
                    if (Type == 1) { Code = "001"; } else { Code = "01"; }
                }
                else { Code = "01"; }
                return parentAccountCode + Code;
            }
            else
            {
                var Code = childList.Max(x => long.Parse(x.Code));
                return (Code + 1).ToString();
            }
        }
        #region Reports
        public async Task<IEnumerable<AccountVM>> GetAccountSatement(int BranchId, string lang, int YearId, int AccountId, int CostCenterId, string FromDate, string ToDate)
        {
            try
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == AccountId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),// x.ChildsAccount.Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)), //GetAllChildTotalcredit(x.ChildsAccount,x.IsMain,x.Transactions),// x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),//GetAllChildTotaldepit(x.ChildsAccount, x.IsMain,x.Transactions),//x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts(x.ChildsAccount != null ? x.ChildsAccount : new List<Accounts>(), x.BranchId??0, YearId, FromDate, ToDate, CostCenterId).Result.ToList(),
                    Transactions = x.Transactions.Where(s => s.CostCenterId == CostCenterId || CostCenterId == 0).Select(m => new TransactionsVM
                    {
                        TransactionId = m.TransactionId,
                        LineNumber = m.LineNumber,
                        Depit = m.Depit,
                        Credit = m.Credit,
                        CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                        TypeName = m.AccTransactionTypes.NameAr ?? "",
                        InvoiceReference = m.InvoiceReference ?? "",
                        Notes = m.Notes ?? "",
                        CurrentBalance = m.CurrentBalance,
                        TransactionDate = m.TransactionDate,
                    }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                        (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                }).ToList();
                return allAccounts;
                //List<AccountVM> result = await Task.FromResult<List<AccountVM>>(allAccounts);
                //return result;
            }
            catch (Exception)
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == AccountId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts(x.ChildsAccount != null ? x.ChildsAccount : new List<Accounts>(), x.BranchId??0, YearId, FromDate, ToDate, CostCenterId).Result.ToList(),
                    Transactions = x.Transactions!=null? x.Transactions.Where(s => s.CostCenterId == CostCenterId || CostCenterId == 0).Select(m => new TransactionsVM
                    {
                        TransactionId = m.TransactionId,
                        LineNumber = m.LineNumber,
                        Depit = m.Depit,
                        Credit = m.Credit,
                        CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                        TypeName = m.AccTransactionTypes.NameAr ?? "",
                        InvoiceReference = m.InvoiceReference ?? "",
                        Notes = m.Notes ?? "",
                        CurrentBalance = m.CurrentBalance,
                        TransactionDate = m.TransactionDate,
                    }).ToList():new List<TransactionsVM>()
                }).ToList();
                return allAccounts;
            }
        }
        public async Task<IEnumerable<AccountVM>> GetGeneralBudget(int BranchId, string lang, int YearId, string FromDate, string ToDate)
        {

            try
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),// x.ChildsAccount.Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)), //GetAllChildTotalcredit(x.ChildsAccount,x.IsMain,x.Transactions),// x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),//GetAllChildTotaldepit(x.ChildsAccount, x.IsMain,x.Transactions),//x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts(x.ChildsAccount != null ? x.ChildsAccount:new List<Accounts>(), x.BranchId ?? 0, YearId, FromDate, ToDate, 0).Result.ToList()
                }).ToList();
                return allAccounts;
            }
            catch (Exception)
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),// x.ChildsAccount.Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)), //GetAllChildTotalcredit(x.ChildsAccount,x.IsMain,x.Transactions),// x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),//GetAllChildTotaldepit(x.ChildsAccount, x.IsMain,x.Transactions),//x.ChildsAccount.Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts =  GetChildsAccounnts(x.ChildsAccount != null ? x.ChildsAccount : new List<Accounts>(), x.BranchId ?? 0, YearId, FromDate, ToDate, 0).Result.ToList()
                }).ToList();
                return allAccounts;
            }
        }
        public async Task<IEnumerable<AccountVM>> GetGeneralLedger(int BranchId, string lang, int YearId, string FromDate, string ToDate)
        {
            try
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    ChildAccounts =  GetChildsAccounnts2(x!.ChildsAccount ?? new List<Accounts>(), x.BranchId ?? 0, YearId, FromDate, ToDate).Result.ToList()
                }).ToList();
                return allAccounts;
            }
            catch (Exception)
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts2(x!.ChildsAccount ?? new List<Accounts>(), x.BranchId ?? 0, YearId, FromDate, ToDate).Result.ToList()
                }).ToList();
                return allAccounts;
            }

        }
        public async Task<IEnumerable<AccountVM>> GetGeneralLedgerDGV(int BranchId, string lang, int YearId, string FromDate, string ToDate, string Con)
        {
            try
            {
                List<AccountVM> lmd = new List<AccountVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GeneralLedgerReport";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@startdate", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@startdate", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@enddate", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@enddate", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        command.Parameters.Add(new SqlParameter("@accountlevel", 1));
                        command.Parameters.Add(new SqlParameter("@branch", BranchId));
                        command.Parameters.Add(new SqlParameter("@yearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new AccountVM
                            {
                                // adding data from dataset row in to list<modeldata>  

                                AccountId = Convert.ToInt32(dr["AccountId"]),
                                AccountName = (dr["AccountName"]).ToString(),
                                Code = (dr["AccountCode"]).ToString(),
                                IsMain = Convert.ToBoolean(dr["IsMain"]),
                                TotalCredit = Convert.ToDecimal(dr["TotalCredit"].ToString()),
                                TotalDepit = Convert.ToDecimal(dr["TotalDepit"].ToString()),
                                TotalCreditOpeningBalance = Convert.ToDecimal(dr["TotalCreditOpeningBalance"].ToString()),
                                TotalDepitOpeningBalance = Convert.ToDecimal(dr["TotalDepitOpeningBalance"].ToString()),
                                TotalCreditBalance = Convert.ToDecimal(dr["TotalCreditBalance"].ToString()),
                                TotalDepitBalance = Convert.ToDecimal(dr["TotalDepitBalance"].ToString()),
                                Notes = (dr["Notes"]).ToString(),
                                ParentId = Convert.ToInt32(dr["ParentId"])
                            });
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                List<AccountVM> lmd = new List<AccountVM>();
                return lmd;
            }

        }
        //heba
        public async Task<DataTable> TreeView(string Con)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "select * from Account_Tree";
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
            catch
            {

                return new DataTable();
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew_old(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL)
        {
            try
            {
                string accountcod2 = "";
                if (AccountCode != null && AccountCode != "")
                {
                    var Result_ST = await GetAccountById(Convert.ToInt32(AccountCode));
                    accountcod2 = Result_ST.Code;
                }

                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //if(LVL=="10")
                        //{
                        //    command.CommandText = "rptGetTrailBalanceAll";
                        //}
                        //else
                        //{
                        //    command.CommandText = "rptGetTrailBalance";
                        //}
                        command.CommandText = "rptGetTrailBalanceAllWithLevel";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int CostId = 0;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                        }


                        command.Parameters.Add(new SqlParameter("@CCID", CCID));


                        //dawoud

                        if (accountcod2 == "")
                        {
                            accountcod2 = null;
                            command.Parameters.Add(new SqlParameter("@AccountCode", DBNull.Value));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AccountCode", accountcod2));
                        }
                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchID", BranchId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            //if (dr[0].ToString() == "0")
                            //{

                            //}
                            //else {
                            double EndTotalDebit, EndTotalCredit;
                            double PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                            try
                            {
                                PeriodDebit = double.Parse(dr[4].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[5].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodCredit = 0;
                            }

                            try
                            {
                                DebitOP = double.Parse(dr[2].ToString());
                            }
                            catch (Exception)
                            {
                                DebitOP = 0;
                            }

                            try
                            {
                                CreditOP = double.Parse(dr[3].ToString());
                            }
                            catch (Exception)
                            {
                                CreditOP = 0;
                            }


                            try
                            {
                                EndTotalDebit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception)
                            {
                                EndTotalDebit = 0;
                            }
                            try
                            {
                                EndTotalCredit = double.Parse(dr[7].ToString());
                            }
                            catch (Exception)
                            {
                                EndTotalCredit = 0;
                            }


                            var checkValueDEPIT = DebitOP + PeriodDebit;
                            var checkValueCREDIT = CreditOP + PeriodCredit;

                            //if (checkValueDEPIT> checkValueCREDIT)
                            //{

                            //    EndTotalDebit = (checkValueDEPIT - checkValueCREDIT).ToString();
                            //    EndTotalCredit = "0";
                            //}
                            //else
                            //{
                            //    EndTotalCredit = (checkValueCREDIT - checkValueDEPIT).ToString();
                            //    EndTotalDebit = "0";

                            //}

                            if (ZeroCheck == 1)
                            {
                                if (PeriodCredit == 0 && PeriodDebit == 0 && CreditOP == 0 && DebitOP == 0 && EndTotalDebit == 0 && EndTotalCredit == 0)
                                {

                                }
                                else
                                {
                                    lmd.Add(new TrainBalanceVM
                                    {
                                        Acc_NameAr = (dr[1]).ToString(),
                                        AccCode = (dr[0]).ToString(),
                                        CreditTotal = PeriodCredit.ToString(),
                                        DebitTotal = PeriodDebit.ToString(),
                                        OpCredit = CreditOP.ToString(),
                                        OpDipet = DebitOP.ToString(),
                                        AccNature = dr[2].ToString(),
                                        TotalDebitEnd = EndTotalDebit.ToString(),
                                        TotalCriditEnd = EndTotalCredit.ToString(),


                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new TrainBalanceVM
                                {
                                    Acc_NameAr = (dr[1]).ToString(),
                                    AccCode = (dr[0]).ToString(),
                                    CreditTotal = PeriodCredit.ToString(),
                                    DebitTotal = PeriodDebit.ToString(),
                                    OpCredit = CreditOP.ToString(),
                                    OpDipet = DebitOP.ToString(),
                                    AccNature = dr[2].ToString(),
                                    TotalDebitEnd = EndTotalDebit.ToString(),
                                    TotalCriditEnd = EndTotalCredit.ToString(),

                                });

                            }
                            //}
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2_old(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL)
        {
            try
            {
                string accountcod2 = "";
                if (AccountCode != null && AccountCode != "")
                {
                    var Result_ST = await GetAccountById(Convert.ToInt32(AccountCode));
                    accountcod2 = Result_ST.Code;
                }

                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //if(LVL=="10")
                        //{
                        //    command.CommandText = "rptGetTrailBalanceAll";
                        //}
                        //else
                        //{
                        //    command.CommandText = "rptGetTrailBalance";
                        //}
                        command.CommandText = "rptGetTrailBalanceAllWithLevel";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int CostId = 0;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                        }
                        command.Parameters.Add(new SqlParameter("@CCID", CCID));


                        //dawoud

                        if (accountcod2 == "")
                        {
                            accountcod2 = null;
                            command.Parameters.Add(new SqlParameter("@AccountCode", DBNull.Value));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AccountCode", accountcod2));
                        }
                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchID", BranchId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            if (dr[0].ToString() == "0")
                            {

                            }
                            else
                            {
                                double EndTotalDebit, EndTotalCredit;
                                double PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                                try
                                {
                                    PeriodDebit = double.Parse(dr[4].ToString());
                                }
                                catch (Exception)
                                {
                                    PeriodDebit = 0;
                                }
                                try
                                {
                                    PeriodCredit = double.Parse(dr[5].ToString());
                                }
                                catch (Exception)
                                {
                                    PeriodCredit = 0;
                                }

                                try
                                {
                                    DebitOP = double.Parse(dr[2].ToString());
                                }
                                catch (Exception)
                                {
                                    DebitOP = 0;
                                }

                                try
                                {
                                    CreditOP = double.Parse(dr[3].ToString());
                                }
                                catch (Exception)
                                {
                                    CreditOP = 0;
                                }


                                try
                                {
                                    EndTotalDebit = double.Parse(dr[6].ToString());
                                }
                                catch (Exception)
                                {
                                    EndTotalDebit = 0;
                                }
                                try
                                {
                                    EndTotalCredit = double.Parse(dr[7].ToString());
                                }
                                catch (Exception)
                                {
                                    EndTotalCredit = 0;
                                }


                                var checkValueDEPIT = DebitOP + PeriodDebit;
                                var checkValueCREDIT = CreditOP + PeriodCredit;

                                //if (checkValueDEPIT> checkValueCREDIT)
                                //{

                                //    EndTotalDebit = (checkValueDEPIT - checkValueCREDIT).ToString();
                                //    EndTotalCredit = "0";
                                //}
                                //else
                                //{
                                //    EndTotalCredit = (checkValueCREDIT - checkValueDEPIT).ToString();
                                //    EndTotalDebit = "0";

                                //}

                                if (ZeroCheck == 1)
                                {
                                    if (PeriodCredit == 0 && PeriodDebit == 0 && CreditOP == 0 && DebitOP == 0 && EndTotalDebit == 0 && EndTotalCredit == 0)
                                    {

                                    }
                                    else
                                    {
                                        lmd.Add(new TrainBalanceVM
                                        {
                                            Acc_NameAr = (dr[1]).ToString(),
                                            AccCode = (dr[0]).ToString(),
                                            CreditTotal = PeriodCredit.ToString(),
                                            DebitTotal = PeriodDebit.ToString(),
                                            OpCredit = CreditOP.ToString(),
                                            OpDipet = DebitOP.ToString(),
                                            AccNature = dr[2].ToString(),
                                            TotalDebitEnd = EndTotalDebit.ToString(),
                                            TotalCriditEnd = EndTotalCredit.ToString(),


                                        });

                                    }
                                }
                                else
                                {
                                    lmd.Add(new TrainBalanceVM
                                    {
                                        Acc_NameAr = (dr[1]).ToString(),
                                        AccCode = (dr[0]).ToString(),
                                        CreditTotal = PeriodCredit.ToString(),
                                        DebitTotal = PeriodDebit.ToString(),
                                        OpCredit = CreditOP.ToString(),
                                        OpDipet = DebitOP.ToString(),
                                        AccNature = dr[2].ToString(),
                                        TotalDebitEnd = EndTotalDebit.ToString(),
                                        TotalCriditEnd = EndTotalCredit.ToString(),

                                    });

                                }
                            }
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                return lmd;
            }

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //ahmedatia
        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGV(string FromDate, string ToDate, string CCID, string Con)
        {
            try
            {
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetTrailBalance";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@to", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (CCID == "")
                        {
                            CCID = null;
                            command.Parameters.Add(new SqlParameter("@CCID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }


                        //command.Parameters.Add(new SqlParameter("@branch", BranchId));
                        //command.Parameters.Add(new SqlParameter("@yearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            string EndTotalDebit, EndTotalCredit;
                            double PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                            try
                            {
                                PeriodDebit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[5].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodCredit = 0;
                            }

                            try
                            {
                                DebitOP = double.Parse(dr[3].ToString());
                            }
                            catch (Exception)
                            {
                                DebitOP = 0;
                            }

                            try
                            {
                                CreditOP = double.Parse(dr[4].ToString());
                            }
                            catch (Exception)
                            {
                                CreditOP = 0;
                            }
                            if (dr[2].ToString() == "1")
                            {//debitFinalBalance = opdebit+PeriodOPerationsDebit-PeriodOperationsCredit

                                EndTotalDebit = (DebitOP + PeriodDebit - PeriodCredit).ToString();
                                EndTotalCredit = "0";
                            }
                            else
                            {
                                EndTotalCredit = (DebitOP - PeriodDebit + PeriodCredit).ToString();
                                EndTotalDebit = "0";

                            }


                            lmd.Add(new TrainBalanceVM
                            {
                                Acc_NameAr = (dr[1]).ToString(),
                                AccCode = (dr[0]).ToString(),
                                CreditTotal = PeriodCredit.ToString(),
                                DebitTotal = PeriodDebit.ToString(),
                                OpCredit = CreditOP.ToString(),
                                OpDipet = DebitOP.ToString(),
                                AccNature = dr[2].ToString(),
                                TotalDebitEnd = EndTotalDebit,
                                TotalCriditEnd = EndTotalCredit,

                            });
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                return lmd;
            }

        }



        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL, int FilteringType,string FilteringTypeStr,string AccountIds)
        {
            try
            {
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetTrailBalanceAllWithLevel";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                        }
                       
                        
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));

                        //dawoud

                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));

                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        if(FilteringTypeStr=="")
                        {
                            FilteringTypeStr = "0";
                        }
                        var valfiltertype = 0;
                        if (FilteringType == 1 || FilteringType == 2 || FilteringType == 3 || FilteringType == 4 || FilteringType == 5 || FilteringType == 6)
                        {
                            valfiltertype = FilteringType;
                            command.Parameters.Add(new SqlParameter("@FillterData", FilteringTypeStr));
                        }
                        else
                        {
                            valfiltertype = 0;
                            command.Parameters.Add(new SqlParameter("@FillterData", "0"));
                        }
                        command.Parameters.Add(new SqlParameter("@FillterType", valfiltertype));

                        if (AccountIds != "")
                        {
                            command.Parameters.Add(new SqlParameter("@AccountIDS", AccountIds));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AccountIDS", "0"));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        double NetCreditOPSum = 0;
                        double NetDebitOPSum = 0;
                        int counter = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            counter++;
                            double EndTotalDebit, EndTotalCredit;
                            double PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                            try
                            {
                                PeriodDebit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[7].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodCredit = 0;
                            }

                            try
                            {
                                DebitOP = double.Parse(dr[4].ToString());
                            }
                            catch (Exception)
                            {
                                DebitOP = 0;
                            }

                            try
                            {
                                CreditOP = double.Parse(dr[5].ToString());
                            }
                            catch (Exception)
                            {
                                CreditOP = 0;
                            }


                                try
                                {
                                    EndTotalDebit = double.Parse(dr[10].ToString());
                                }
                                catch (Exception)
                                {
                                    EndTotalDebit = 0;
                                }
                                try
                                {
                                    EndTotalCredit = double.Parse(dr[11].ToString());
                                }
                                catch (Exception)
                                {
                                    EndTotalCredit = 0;
                                }


                                var checkValueDEPIT = DebitOP + PeriodDebit;
                                var checkValueCREDIT = CreditOP + PeriodCredit;


                            double NetCredit;
                            double NetDepit;
                            double NetCreditOP;
                            double NetDebitOP;

                            if (Convert.ToInt32((dr[0]).ToString())!=0)
                            {
                                if (DebitOP >= CreditOP)
                                {
                                    NetDebitOP = Convert.ToDouble(Convert.ToDecimal(DebitOP) - Convert.ToDecimal(CreditOP));
                                    NetCreditOP = 0;
                                    if((dr[12]).ToString()=="1")
                                    {
                                        NetDebitOPSum = Convert.ToDouble(Convert.ToDecimal(NetDebitOPSum) + Convert.ToDecimal(NetDebitOP));
                                        NetCreditOPSum = Convert.ToDouble(Convert.ToDecimal(NetCreditOPSum) + Convert.ToDecimal(NetCreditOP));
                                    }

                                }
                                else
                                {
                                    NetCreditOP = Convert.ToDouble(Convert.ToDecimal(CreditOP) - Convert.ToDecimal(DebitOP));
                                    NetDebitOP = 0;
                                    if ((dr[12]).ToString() == "1")
                                    {
                                        NetDebitOPSum = Convert.ToDouble(Convert.ToDecimal(NetDebitOPSum) + Convert.ToDecimal(NetDebitOP));
                                        NetCreditOPSum = Convert.ToDouble(Convert.ToDecimal(NetCreditOPSum) + Convert.ToDecimal(NetCreditOP));
                                    }

                                }
                            }
                            else
                            {
                                //var NetDebitOPSum_2 = NetDebitOPSum;
                                //var NetCreditOPSum_2 = NetCreditOPSum;

                                //NetDebitOP = Convert.ToDouble(Convert.ToDecimal(DebitOP));
                                //NetCreditOP = Convert.ToDouble(Convert.ToDecimal(CreditOP));

                                NetDebitOP = Convert.ToDouble(Convert.ToDecimal(NetDebitOPSum));
                                NetCreditOP = Convert.ToDouble(Convert.ToDecimal(NetCreditOPSum));
                            }



                            try
                            {
                                NetDepit = double.Parse(dr[8].ToString());
                            }
                            catch (Exception)
                            {
                                NetDepit = 0;
                            }
                            try
                            {
                                NetCredit = double.Parse(dr[9].ToString());
                            }
                            catch (Exception)
                            {
                                NetCredit = 0;
                            }




                            if (ZeroCheck == 1)
                            {
                                if (NetCredit == 0 && NetDepit == 0 && NetCreditOP == 0 && NetDebitOP == 0 /*&& EndTotalDebit == 0 && EndTotalCredit == 0*/)
                                {

                                }
                                else
                                {
                                    lmd.Add(new TrainBalanceVM
                                    {
                                        AccountId = Convert.ToInt32((dr[0]).ToString()),
                                        AccCode = (dr[1]).ToString(),
                                        Acc_NameAr = (dr[3]).ToString(),
                                        CreditTotal = PeriodCredit.ToString(),
                                        DebitTotal = PeriodDebit.ToString(),
                                        OpCredit = NetCreditOP.ToString(),
                                        OpDipet = NetDebitOP.ToString(),
                                        TotalDebitEnd = EndTotalDebit.ToString(),
                                        TotalCriditEnd = EndTotalCredit.ToString(),
                                        Level = (dr[12]).ToString(),
                                        NetCreditTotal = NetCredit.ToString(),
                                        NetDebitTotal = NetDepit.ToString(),
                                        LineNumber= counter,
                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new TrainBalanceVM
                                {
                                    AccountId=Convert.ToInt32(dr[0].ToString()),
                                    Acc_NameAr = (dr[3]).ToString(),
                                    AccCode = (dr[1]).ToString(),
                                    CreditTotal = PeriodCredit.ToString(),
                                    DebitTotal = PeriodDebit.ToString(),
                                    OpCredit = NetCreditOP.ToString(),
                                    OpDipet = NetDebitOP.ToString(),
                                    TotalDebitEnd = EndTotalDebit.ToString(),
                                    TotalCriditEnd = EndTotalCredit.ToString(),
                                    Level = (dr[12]).ToString(),
                                    NetCreditTotal = NetCredit.ToString(),
                                    NetDebitTotal = NetDepit.ToString(),
                                    LineNumber = counter,
                                });

                            }
                        //}
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<TrainBalanceVM>> GetGeneralBudgetAMRDGVNew(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds)
        {
            try
            {
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetGeneralBudgetWithLevel";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                        }


                        command.Parameters.Add(new SqlParameter("@CCID", CCID));

                        //dawoud

                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));

                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        if (FilteringTypeStr == "")
                        {
                            FilteringTypeStr = "0";
                        }
                        var valfiltertype = 0;
                        if (FilteringType == 1 || FilteringType == 2 || FilteringType == 3 || FilteringType == 4 || FilteringType == 5 || FilteringType == 6)
                        {
                            valfiltertype = FilteringType;
                            command.Parameters.Add(new SqlParameter("@FillterData", FilteringTypeStr));
                        }
                        else
                        {
                            valfiltertype = 0;
                            command.Parameters.Add(new SqlParameter("@FillterData", "0"));
                        }
                        command.Parameters.Add(new SqlParameter("@FillterType", valfiltertype));

                        if (AccountIds != "")
                        {
                            command.Parameters.Add(new SqlParameter("@AccountIDS", AccountIds));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AccountIDS", "0"));
                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        double NetCreditOPSum = 0;
                        double NetDebitOPSum = 0;
                        int counter = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            counter++;
                            double EndTotalDebit, EndTotalCredit;
                            double AhlakDebit, AhlakCredit, PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                            try
                            {
                                AhlakDebit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception)
                            {
                                AhlakDebit = 0;
                            }
                            try
                            {
                                AhlakCredit = double.Parse(dr[7].ToString());
                            }
                            catch (Exception)
                            {
                                AhlakCredit = 0;
                            }

                            try
                            {
                                PeriodDebit = double.Parse(dr[8].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[9].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodCredit = 0;
                            }

                            try
                            {
                                DebitOP = double.Parse(dr[4].ToString());
                            }
                            catch (Exception)
                            {
                                DebitOP = 0;
                            }

                            try
                            {
                                CreditOP = double.Parse(dr[5].ToString());
                            }
                            catch (Exception)
                            {
                                CreditOP = 0;
                            }


                            try
                            {
                                EndTotalDebit = double.Parse(dr[12].ToString());
                            }
                            catch (Exception)
                            {
                                EndTotalDebit = 0;
                            }
                            try
                            {
                                EndTotalCredit = double.Parse(dr[13].ToString());
                            }
                            catch (Exception)
                            {
                                EndTotalCredit = 0;
                            }


                            var checkValueDEPIT = DebitOP + PeriodDebit;
                            var checkValueCREDIT = CreditOP + PeriodCredit;


                            double NetCredit;
                            double NetDepit;
                            double NetCreditOP;
                            double NetDebitOP;
                            double TotalFinalSum;
                            if (Convert.ToInt32((dr[0]).ToString()) != 0)
                            {
                                if (DebitOP >= CreditOP)
                                {
                                    NetDebitOP = Convert.ToDouble(Convert.ToDecimal(DebitOP) - Convert.ToDecimal(CreditOP));
                                    NetCreditOP = 0;
                                    if ((dr[14]).ToString() == "1")
                                    {
                                        NetDebitOPSum = Convert.ToDouble(Convert.ToDecimal(NetDebitOPSum) + Convert.ToDecimal(NetDebitOP));
                                        NetCreditOPSum = Convert.ToDouble(Convert.ToDecimal(NetCreditOPSum) + Convert.ToDecimal(NetCreditOP));
                                    }

                                }
                                else
                                {
                                    NetCreditOP = Convert.ToDouble(Convert.ToDecimal(CreditOP) - Convert.ToDecimal(DebitOP));
                                    NetDebitOP = 0;
                                    if ((dr[14]).ToString() == "1")
                                    {
                                        NetDebitOPSum = Convert.ToDouble(Convert.ToDecimal(NetDebitOPSum) + Convert.ToDecimal(NetDebitOP));
                                        NetCreditOPSum = Convert.ToDouble(Convert.ToDecimal(NetCreditOPSum) + Convert.ToDecimal(NetCreditOP));
                                    }

                                }
                            }
                            else
                            {
                                //var NetDebitOPSum_2 = NetDebitOPSum;
                                //var NetCreditOPSum_2 = NetCreditOPSum;

                                //NetDebitOP = Convert.ToDouble(Convert.ToDecimal(DebitOP));
                                //NetCreditOP = Convert.ToDouble(Convert.ToDecimal(CreditOP));

                                NetDebitOP = Convert.ToDouble(Convert.ToDecimal(NetDebitOPSum));
                                NetCreditOP = Convert.ToDouble(Convert.ToDecimal(NetCreditOPSum));
                            }



                            try
                            {
                                NetDepit = double.Parse(dr[10].ToString());
                            }
                            catch (Exception)
                            {
                                NetDepit = 0;
                            }
                            try
                            {
                                NetCredit = double.Parse(dr[11].ToString());
                            }
                            catch (Exception)
                            {
                                NetCredit = 0;
                            }


                            //TotalFinalSum = NetDepit - NetCredit - AhlakDebit - AhlakCredit;
                           // TotalFinalSum = NetDepit - NetCredit;
                            TotalFinalSum = EndTotalDebit - EndTotalCredit;

                            
                            if (ZeroCheck == 1)
                            {
                                if (NetCredit == 0 && NetDepit == 0 && NetCreditOP == 0 && NetDebitOP == 0 /*&& EndTotalDebit == 0 && EndTotalCredit == 0*/)
                                {

                                }
                                else
                                {
                                    lmd.Add(new TrainBalanceVM
                                    {
                                        AccountId = Convert.ToInt32((dr[0]).ToString()),
                                        AccCode = (dr[1]).ToString(),
                                        Acc_NameAr = (dr[3]).ToString(),
                                        CreditTotal = PeriodCredit.ToString(),
                                        DebitTotal = PeriodDebit.ToString(),
                                        OpCredit = NetCreditOP.ToString(),
                                        OpDipet = NetDebitOP.ToString(),
                                        AhCredit = AhlakCredit.ToString(),
                                        AhDipet = AhlakDebit.ToString(),
                                        TotalDebitEnd = EndTotalDebit.ToString(),
                                        TotalCriditEnd = EndTotalCredit.ToString(),
                                        Level = (dr[14]).ToString(),
                                        Classification = Convert.ToInt32(dr[15].ToString()),
                                        AccountIdAhlak = Convert.ToInt32(dr[16].ToString()),
                                        ParentId = Convert.ToInt32(dr[17].ToString()),
                                        NetCreditTotal = NetCredit.ToString(),
                                        NetDebitTotal = NetDepit.ToString(),
                                        LineNumber = counter,
                                        TotalFinal= TotalFinalSum.ToString(),
                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new TrainBalanceVM
                                {
                                    AccountId = Convert.ToInt32(dr[0].ToString()),
                                    Acc_NameAr = (dr[3]).ToString(),
                                    AccCode = (dr[1]).ToString(),
                                    CreditTotal = PeriodCredit.ToString(),
                                    DebitTotal = PeriodDebit.ToString(),
                                    OpCredit = NetCreditOP.ToString(),
                                    OpDipet = NetDebitOP.ToString(),
                                    AhCredit = AhlakCredit.ToString(),
                                    AhDipet = AhlakDebit.ToString(),
                                    TotalDebitEnd = EndTotalDebit.ToString(),
                                    TotalCriditEnd = EndTotalCredit.ToString(),
                                    Level = (dr[14]).ToString(),
                                    Classification = Convert.ToInt32(dr[15].ToString()),
                                    AccountIdAhlak = Convert.ToInt32(dr[16].ToString()),
                                    ParentId = Convert.ToInt32(dr[17].ToString()),
                                    NetCreditTotal = NetCredit.ToString(),
                                    NetDebitTotal = NetDepit.ToString(),
                                    LineNumber = counter,
                                    TotalFinal = TotalFinalSum.ToString(),
                                });

                            }
                            //}
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<TrainBalanceVM>> GetTrailBalanceDGVNew2(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string AccountCode, string LVL, int FilteringType,string FilteringTypeStr,string AccountIds)
        {
            try
            {
                //dawoudfunc2
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetTrailBalanceAllWithLevel";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                        }
                        command.Parameters.Add(new SqlParameter("@CCID", CCID));


                        //dawoud

                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        //command.Parameters.Add(new SqlParameter("@BranchID", BranchId));
                        //command.Parameters.Add(new SqlParameter("@BranchID", DBNull.Value));

                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        if (FilteringTypeStr == "")
                        {
                            FilteringTypeStr = "0";
                        }
                        var valfiltertype = 0;

                        if (FilteringType==1 || FilteringType == 2 || FilteringType == 3 || FilteringType == 4 || FilteringType == 5 || FilteringType == 6)
                        {
                            valfiltertype = FilteringType;
                            command.Parameters.Add(new SqlParameter("@FillterData", FilteringTypeStr));
                        }
                        else
                        {
                            valfiltertype = 0;
                            command.Parameters.Add(new SqlParameter("@FillterData", "0"));
                        }

                        command.Parameters.Add(new SqlParameter("@FillterType", valfiltertype));

                        if (AccountIds != "")
                        {
                            command.Parameters.Add(new SqlParameter("@AccountIDS", AccountIds));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AccountIDS", "0"));
                        }


                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            if (dr[3].ToString() == "Total")
                            {

                            }
                            else
                            {
                            double EndTotalDebit, EndTotalCredit;
                            double PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                            try
                            {
                                PeriodDebit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[7].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodCredit = 0;
                            }

                            try
                            {
                                DebitOP = double.Parse(dr[4].ToString());
                            }
                            catch (Exception)
                            {
                                DebitOP = 0;
                            }

                            try
                            {
                                CreditOP = double.Parse(dr[5].ToString());
                            }
                            catch (Exception)
                            {
                                CreditOP = 0;
                            }


                            try
                            {
                                EndTotalDebit = double.Parse(dr[10].ToString());
                            }
                            catch (Exception)
                            {
                                EndTotalDebit = 0;
                            }
                            try
                            {
                                EndTotalCredit = double.Parse(dr[11].ToString());
                            }
                            catch (Exception)
                            {
                                EndTotalCredit = 0;
                            }


                            var checkValueDEPIT = DebitOP + PeriodDebit;
                            var checkValueCREDIT = CreditOP + PeriodCredit;
                                double NetCredit;
                                double NetDepit;

                                try
                                {
                                    NetDepit = double.Parse(dr[8].ToString());
                                }
                                catch (Exception)
                                {
                                    NetDepit = 0;
                                }
                                try
                                {
                                    NetCredit = double.Parse(dr[9].ToString());
                                }
                                catch (Exception)
                                {
                                    NetCredit = 0;
                                }


                                if (ZeroCheck == 1)
                            {
                                if (PeriodCredit == 0 && PeriodDebit == 0 && CreditOP == 0 && DebitOP == 0 /*&& EndTotalDebit == 0 && EndTotalCredit == 0*/)
                                {

                                }
                                else
                                {
                                    lmd.Add(new TrainBalanceVM
                                    {
                                        AccountId=Convert.ToInt32(dr[0].ToString()),
                                        Acc_NameAr = (dr[3]).ToString(),
                                        AccCode = (dr[1]).ToString(),
                                        CreditTotal = PeriodCredit.ToString(),
                                        DebitTotal = PeriodDebit.ToString(),
                                        OpCredit = CreditOP.ToString(),
                                        OpDipet = DebitOP.ToString(),
                                        TotalDebitEnd = EndTotalDebit.ToString(),
                                        TotalCriditEnd = EndTotalCredit.ToString(),
                                        Level= (dr[12]).ToString(),
                                        NetCreditTotal=NetCredit.ToString(),
                                        NetDebitTotal=NetDepit.ToString(),

                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new TrainBalanceVM
                                {
                                    AccountId = Convert.ToInt32(dr[0].ToString()),
                                    Acc_NameAr = (dr[3]).ToString(),
                                    AccCode = (dr[1]).ToString(),
                                    CreditTotal = PeriodCredit.ToString(),
                                    DebitTotal = PeriodDebit.ToString(),
                                    OpCredit = CreditOP.ToString(),
                                    OpDipet = DebitOP.ToString(),
                                    TotalDebitEnd = EndTotalDebit.ToString(),
                                    TotalCriditEnd = EndTotalCredit.ToString(),
                                    Level = (dr[12]).ToString(),
                                    NetCreditTotal = NetCredit.ToString(),
                                    NetDebitTotal = NetDepit.ToString(),

                                });

                            }
                        }
                    }
                    }
                }
                return lmd;
            }
            catch(Exception ex)
            {
                List<TrainBalanceVM> lmd = new List<TrainBalanceVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<DetailsMonitorVM>> GetDetailsMonitor(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con,int FilteringType, string FilteringTypeStr, int AccountId,int Type,int Type2)
        {
            try
            {
                List<DetailsMonitorVM> lmd = new List<DetailsMonitorVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandText = "GetFilterd_Transactions";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@from", DBNull.Value));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@from", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", DBNull.Value));

                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                        }
                        if(Convert.ToInt32(CCID)>0)
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", DBNull.Value));
                        }
                        command.Parameters.Add(new SqlParameter("@YearId", YearId));


                        if (FilteringTypeStr == "")
                        {
                            FilteringTypeStr = "0";
                        }
                        if (FilteringType == 1)
                        {
                            command.Parameters.Add(new SqlParameter("@ProjectIds", FilteringTypeStr));
                            command.Parameters.Add(new SqlParameter("@BranchIds", "0"));
                            command.Parameters.Add(new SqlParameter("@SeviceIds", "0"));

                        }
                        else if (FilteringType == 4)
                        {
                            command.Parameters.Add(new SqlParameter("@ProjectIds", "0"));
                            command.Parameters.Add(new SqlParameter("@BranchIds", FilteringTypeStr));
                            command.Parameters.Add(new SqlParameter("@SeviceIds", "0"));
                        }
                        else if (FilteringType == 6)
                        {
                            command.Parameters.Add(new SqlParameter("@ProjectIds", "0"));
                            command.Parameters.Add(new SqlParameter("@BranchIds", "0"));
                            command.Parameters.Add(new SqlParameter("@SeviceIds", FilteringTypeStr));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ProjectIds", "0"));
                            command.Parameters.Add(new SqlParameter("@BranchIds", "0"));
                            command.Parameters.Add(new SqlParameter("@SeviceIds", "0"));
                        }

                        if (AccountId >0)
                        {
                            command.Parameters.Add(new SqlParameter("@Accountid", AccountId));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@Accountid", 0));
                        }
                        command.Parameters.Add(new SqlParameter("@Type", Type));
                        if(Type2 ==1 || Type2 == 2) {
                        command.Parameters.Add(new SqlParameter("@Type2", 1));
                        }
                        else if(Type2==3 || Type2 == 4 || Type2 == 5 || Type2 == 6)
                        {
                            command.Parameters.Add(new SqlParameter("@Type2", 2));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@Type2", 3));

                        }

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            double PeriodDebit, PeriodCredit;
                            try
                            {
                                PeriodDebit = double.Parse(dr[7].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception)
                            {
                                PeriodCredit = 0;
                            }

                            lmd.Add(new DetailsMonitorVM
                            {
                                TransactionId = Convert.ToInt32(dr[0].ToString()),
                                TransactionDate = (dr[1]).ToString(),
                                InvoiceId = Convert.ToInt32(dr[2].ToString()),
                                Type = Convert.ToInt32(dr[3].ToString()),
                                AccountId = Convert.ToInt32(dr[4].ToString()),
                                CostCenterId = dr[5].ToString(),
                                credit = PeriodCredit.ToString(),
                                depit = PeriodDebit.ToString(),
                                details = dr[8].ToString(),
                                notes = dr[9].ToString(),
                                branchid = Convert.ToInt32(dr[10].ToString()),
                                TypeName = dr[11].ToString(),
                                ProjectNo = dr[12].ToString(),
                                customername = dr[13].ToString(),
                                ServicesName = dr[14].ToString(),
                                AccountName=dr[15].ToString(),
                                BranchName=dr[16].ToString(),
                                Cus_Emp_Sup = Convert.ToInt32(dr[17].ToString()),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<DetailsMonitorVM> lmd = new List<DetailsMonitorVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<CostCenterExpRevVM>> GetProjectExpRev(string CCID, string Con)
        {
            try
            {
                List<CostCenterExpRevVM> lmd = new List<CostCenterExpRevVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetCostCenterExpRev";
                        command.Connection = con;
                        if (CCID == "")
                        {
                            CCID = null;
                            command.Parameters.Add(new SqlParameter("@CCID", "1"));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new CostCenterExpRevVM
                            {
                                CashIncome = (dr[0]).ToString(),
                                BankIncome = (dr[1]).ToString(),
                                TotalIncome = (dr[2]).ToString(),
                                OperationalExpenses = (dr[3]).ToString(),
                                GeneralExpenses = (dr[4]).ToString(),
                                TotalExpenses = (dr[5]).ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<CostCenterExpRevVM> lmd = new List<CostCenterExpRevVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGV(string FromDate, string ToDate, string CCID, string Con)
        {
            try
            {
                List<IncomeStatmentVM> lmd = new List<IncomeStatmentVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetIncomeStatment";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@to", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (CCID == "")
                        {
                            CCID = null;
                            command.Parameters.Add(new SqlParameter("@CCID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }


                        //command.Parameters.Add(new SqlParameter("@branch", BranchId));
                        //command.Parameters.Add(new SqlParameter("@yearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new IncomeStatmentVM
                            {
                                CatName = (dr[0]).ToString(),
                                IncomePartual = (dr[1]).ToString(),
                                IncomeTotal = dr[2].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<IncomeStatmentVM> lmd = new List<IncomeStatmentVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<IncomeStatmentVM>> GetIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string LVL, int? taxID, int? taxID2, int? GeneralAdmExpenses, int? DepFixedAssets, int? AccountID_Mb, int? AccountID_MR)
        {
            try
            {
                List<IncomeStatmentVM> lmd = new List<IncomeStatmentVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetIncomeStatment";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int CostId = 0;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        if (CCID == 0)
                        {
                            CostId = 0;
                            command.Parameters.Add(new SqlParameter("@CCID", CostId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }


                        //dawoud1

                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", 2));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));
                        command.Parameters.Add(new SqlParameter("@GeneralAdmExpenses", 1));
                        command.Parameters.Add(new SqlParameter("@DepFixedAssets", DepFixedAssets));



                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));
                        command.Parameters.Add(new SqlParameter("@AccountID_Mb", AccountID_Mb));
                        command.Parameters.Add(new SqlParameter("@AccountID_MR", AccountID_MR));

                        //command.Parameters.Add(new SqlParameter("@yearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            //daw
                            if (ZeroCheck == 1)
                            {
                                if ((dr[1]).ToString() == "0")
                                {

                                }
                                else
                                {
                                    lmd.Add(new IncomeStatmentVM
                                    {
                                        ID = (dr[2]).ToString(),
                                        CatName = (dr[0]).ToString(),
                                        IncomePartual = (dr[1]).ToString(),
                                        //IncomeTotal = dr[2].ToString(),

                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new IncomeStatmentVM
                                {
                                    ID = (dr[2]).ToString(),
                                    CatName = (dr[0]).ToString(),
                                    IncomePartual = (dr[1]).ToString(),
                                    //IncomeTotal = dr[2].ToString(),

                                });

                            }
                        }
                    }
                }
                return lmd;
            }
            catch(Exception ex)
            {
                //string msg = ex.Message;
                List<IncomeStatmentVM> lmd = new List<IncomeStatmentVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<IncomeStatmentVMWithLevels>> GetIncomeStatmentDGVLevels(string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string LVL, int FilteringType,int FilteringTypeAll, string FilteringTypeStr,string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter,int TypeF)
        {
            try
            {
                if(FromDate=="" || ToDate == "")
                {
                    List<IncomeStatmentVMWithLevels> lmd2 = new List<IncomeStatmentVMWithLevels>();
                    return lmd2;
                }
                if(PeriodFillterType==7 || PeriodFillterType == 0)
                {
                    PeriodFillterType = 1;//مقارنة بفترة سابقة
                }
                if (PeriodCounter == 0)
                {
                    PeriodCounter = 1; 
                }
                //dawoudfunc1
                List<IncomeStatmentVMWithLevels> lmd = new List<IncomeStatmentVMWithLevels>();
                List<string> TotalResultList = new List<string>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetIncomeStatmentWithLevel";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));
                        }
                        else
                        {
                            to = ToDate;
                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        command.Parameters.Add(new SqlParameter("@CCID", CCID));


                        command.Parameters.Add(new SqlParameter("@PeriodFillterType", PeriodFillterType));
                        command.Parameters.Add(new SqlParameter("@PeriodCounter", PeriodCounter));
                        command.Parameters.Add(new SqlParameter("@FillterHeaderType", TypeF));

                        
                        if (LVL == ""|| LVL == "0")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        //command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        if (FilteringTypeStr == "")
                        {
                            FilteringTypeStr = "0";
                        }

                        if (FilteringType == 11)
                        {
                            command.Parameters.Add(new SqlParameter("@Projects", FilteringTypeStr));
                        }
                        else if (FilteringType == 12)
                        {
                            command.Parameters.Add(new SqlParameter("@Customers", FilteringTypeStr));
                        }
                        else if (FilteringType == 13)
                        {
                            command.Parameters.Add(new SqlParameter("@Suppliers", FilteringTypeStr));
                        }
                        else if (FilteringType == 14)
                        {
                            command.Parameters.Add(new SqlParameter("@Branchs", FilteringTypeStr));
                        }
                        else if (FilteringType == 15)
                        {
                            command.Parameters.Add(new SqlParameter("@Employees", FilteringTypeStr));
                        }
                        else if (FilteringType == 16)
                        {
                            command.Parameters.Add(new SqlParameter("@Services", FilteringTypeStr));
                        }
                        else
                        {

                        }

                        if (FilteringTypeAllStr == "")
                        {
                            FilteringTypeAllStr = "0";
                        }

                        if (FilteringTypeAll == 11)
                        {
                            command.Parameters.Add(new SqlParameter("@Projects", FilteringTypeAllStr));
                            if(FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }
                        else if (FilteringTypeAll == 12)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            command.Parameters.Add(new SqlParameter("@Customers", FilteringTypeAllStr));
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }
                        else if (FilteringTypeAll == 13)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            command.Parameters.Add(new SqlParameter("@Suppliers", FilteringTypeAllStr));
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }
                        else if (FilteringTypeAll == 14)
                        {

                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            command.Parameters.Add(new SqlParameter("@Branchs", FilteringTypeAllStr));
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }

                        }
                        else if (FilteringTypeAll == 15)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            command.Parameters.Add(new SqlParameter("@Employees", FilteringTypeAllStr));
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }

                        }
                        else if (FilteringTypeAll == 16)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            command.Parameters.Add(new SqlParameter("@Services", FilteringTypeAllStr));
                        }
                        else
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }



                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        var len = dt.Rows[0].ItemArray.Length;

                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            TotalResultList = new List<string>();
                            bool checkZero = true;

                            for (int i = 6; i < len; i++)
                            {
                                var ayy = (dr[i]).ToString();
                                if ((dr[i]).ToString() == "0.00")
                                {
                                    
                                    TotalResultList.Add((dr[i]).ToString());
                                    if (checkZero != false)
                                    {
                                        checkZero = true;
                                    }
                                }
                                else
                                {
                                    TotalResultList.Add((dr[i]).ToString());
                                    checkZero = false;
                                }
                            }
                            //daw
                            if (ZeroCheck == 1)
                            {
                                if (checkZero==true)
                                {
                                    
                                }
                                else
                                {
                                    lmd.Add(new IncomeStatmentVMWithLevels
                                    {
                                        ID = Convert.ToInt32((dr[0]).ToString()),
                                        AccountID = (dr[1]).ToString(),
                                        AccountCode = (dr[2]).ToString(),
                                        AccountCodeNew = (dr[3]).ToString(),
                                        AccountName = (dr[4]).ToString(),
                                        AccountLevel = (dr[5]).ToString(),
                                        AccountNameCode = (dr[2]).ToString() + " - " + (dr[4]).ToString(),
                                        TotalResult = TotalResultList

                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new IncomeStatmentVMWithLevels
                                {
                                    ID = Convert.ToInt32((dr[0]).ToString()),
                                    AccountID = (dr[1]).ToString(),
                                    AccountCode = (dr[2]).ToString(),
                                    AccountCodeNew = (dr[3]).ToString(),
                                    AccountName = (dr[4]).ToString(),
                                    AccountLevel = (dr[5]).ToString(),
                                    AccountNameCode= (dr[2]).ToString()+" - "+ (dr[4]).ToString(),
                                    TotalResult = TotalResultList
                                });

                            }
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<IncomeStatmentVMWithLevels> lmd = new List<IncomeStatmentVMWithLevels>();
                return lmd;
            }

        }




        public async Task<IEnumerable<DetailsMonitorVM>> GetIncomeStatmentDGVLevelsdetails(int Accountid,int Type,int Type2,string FromDate, string ToDate, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF)
        {
            try
            {
                if (FromDate == "" || ToDate == "")
                {
                    List<DetailsMonitorVM> lmd2 = new List<DetailsMonitorVM>();
                    return lmd2;
                }
                if (PeriodFillterType == 7 || PeriodFillterType == 0)
                {
                    PeriodFillterType = 1;//مقارنة بفترة سابقة
                }
                if (PeriodCounter == 0)
                {
                    PeriodCounter = 1;
                }
                //dawoudfunc1
                List<DetailsMonitorVM> lmd = new List<DetailsMonitorVM>();
                List<string> TotalResultList = new List<string>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GetincomeFilterd_Transactions";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", DBNull.Value));
                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", DBNull.Value));
                        }
                        else
                        {
                            to = ToDate;
                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        command.Parameters.Add(new SqlParameter("@CCID", CCID));


                        command.Parameters.Add(new SqlParameter("@PeriodFillterType", PeriodFillterType));
                        command.Parameters.Add(new SqlParameter("@PeriodCounter", PeriodCounter));
                        command.Parameters.Add(new SqlParameter("@FillterHeaderType", TypeF));


                        if (LVL == "" || LVL == "0")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        //command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        command.Parameters.Add(new SqlParameter("@Accountid", Accountid));
                        command.Parameters.Add(new SqlParameter("@Type", Type));
                        command.Parameters.Add(new SqlParameter("@Type2", Type2));

                        if (FilteringTypeStr == "")
                        {
                            FilteringTypeStr = "0";
                        }

                        if (FilteringType == 11)
                        {
                            command.Parameters.Add(new SqlParameter("@Projects", FilteringTypeStr));
                        }
                        else if (FilteringType == 12)
                        {
                            command.Parameters.Add(new SqlParameter("@Customers", FilteringTypeStr));
                        }
                        else if (FilteringType == 13)
                        {
                            command.Parameters.Add(new SqlParameter("@Suppliers", FilteringTypeStr));
                        }
                        else if (FilteringType == 14)
                        {
                            command.Parameters.Add(new SqlParameter("@Branchs", FilteringTypeStr));
                        }
                        else if (FilteringType == 15)
                        {
                            command.Parameters.Add(new SqlParameter("@Employees", FilteringTypeStr));
                        }
                        else if (FilteringType == 16)
                        {
                            command.Parameters.Add(new SqlParameter("@Services", FilteringTypeStr));
                        }
                        else
                        {

                        }

                        if (FilteringTypeAllStr == "")
                        {
                            FilteringTypeAllStr = "0";
                        }

                        if (FilteringTypeAll == 11)
                        {
                            command.Parameters.Add(new SqlParameter("@Projects", FilteringTypeAllStr));
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }
                        else if (FilteringTypeAll == 12)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            command.Parameters.Add(new SqlParameter("@Customers", FilteringTypeAllStr));
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }
                        else if (FilteringTypeAll == 13)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            command.Parameters.Add(new SqlParameter("@Suppliers", FilteringTypeAllStr));
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }
                        else if (FilteringTypeAll == 14)
                        {

                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            command.Parameters.Add(new SqlParameter("@Branchs", FilteringTypeAllStr));
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }

                        }
                        else if (FilteringTypeAll == 15)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            command.Parameters.Add(new SqlParameter("@Employees", FilteringTypeAllStr));
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }

                        }
                        else if (FilteringTypeAll == 16)
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); } 
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            command.Parameters.Add(new SqlParameter("@Services", FilteringTypeAllStr));
                        }
                        else 
                        {
                            if (FilteringType != 11) { command.Parameters.Add(new SqlParameter("@Projects", "0")); }
                            if (FilteringType != 12) { command.Parameters.Add(new SqlParameter("@Customers", "0")); }
                            if (FilteringType != 13) { command.Parameters.Add(new SqlParameter("@Suppliers", "0")); }
                            if (FilteringType != 14) { command.Parameters.Add(new SqlParameter("@Branchs", "0")); }
                            if (FilteringType != 15) { command.Parameters.Add(new SqlParameter("@Employees", "0")); }
                            if (FilteringType != 16) { command.Parameters.Add(new SqlParameter("@Services", "0")); }
                        }



                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[1];
                        var len = dt.Rows[0].ItemArray.Length;

                        foreach (DataRow dr in dt.Rows)
                        {
                            double PeriodDebit, PeriodCredit;
                            try
                            {
                                PeriodDebit = double.Parse(dr[7].ToString());
                            }
                            catch (Exception ex)
                            {
                                PeriodDebit = 0;
                            }
                            try
                            {
                                PeriodCredit = double.Parse(dr[6].ToString());
                            }
                            catch (Exception ex)
                            {
                                PeriodCredit = 0;
                            }

                            lmd.Add(new DetailsMonitorVM
                            {
                                TransactionId = Convert.ToInt32(dr[0].ToString()),
                                TransactionDate = (dr[1]).ToString(),
                                InvoiceId = Convert.ToInt32(dr[2].ToString()),
                                Type = Convert.ToInt32(dr[3].ToString()),
                                AccountId = Convert.ToInt32(dr[4].ToString()),
                                CostCenterId = dr[5].ToString(),
                                credit = PeriodCredit.ToString(),
                                depit = PeriodDebit.ToString(),
                                details = dr[8].ToString(),
                                notes = dr[9].ToString(),
                                branchid = Convert.ToInt32(dr[10].ToString()),
                                TypeName = dr[11].ToString(),
                                ProjectNo = dr[12].ToString(),
                                customername = dr[13].ToString(),
                                ServicesName = dr[14].ToString(),
                                AccountName = dr[15].ToString(),
                                BranchName = dr[16].ToString(),
                                Cus_Emp_Sup = Convert.ToInt32(dr[17].ToString()),
                                suppliername = dr[18].ToString(),
                                employeename = dr[19].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<DetailsMonitorVM> lmd = new List<DetailsMonitorVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<IncomeStatmentVM>> GetALLIncomeStatmentDGVNew(string FromDate, string ToDate, int CCID, int YearId, string lang, string Con, int ZeroCheck, string LVL)
        {
            try
            {
                List<IncomeStatmentVM> lmd = new List<IncomeStatmentVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetALLIncomeStatments";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int CostId = 0;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            string fdate = String.Format("{0:yyyy-MM-dd}", from);
                            command.Parameters.Add(new SqlParameter("@From", fdate));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@To", (YearId + 1) + "-12-31"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            string tdate = String.Format("{0:yyyy-MM-dd}", to);
                            command.Parameters.Add(new SqlParameter("@To", tdate));
                        }

                        if (CCID == 0)
                        {
                            CostId = 0;
                            command.Parameters.Add(new SqlParameter("@CCID", CostId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }


                        //dawoud1

                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", 2));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }




                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));
                        //command.Parameters.Add(new SqlParameter("@yearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            //daw
                            if (ZeroCheck == 1)
                            {
                                if ((dr[1]).ToString() == "0")
                                {

                                }
                                else
                                {
                                    lmd.Add(new IncomeStatmentVM
                                    {
                                        ID = (dr[2]).ToString(),
                                        CatName = (dr[0]).ToString(),
                                        IncomePartual = (dr[1]).ToString(),
                                        //IncomeTotal = dr[2].ToString(),

                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new IncomeStatmentVM
                                {
                                    ID = (dr[2]).ToString(),
                                    CatName = (dr[0]).ToString(),
                                    IncomePartual = (dr[1]).ToString(),
                                    //IncomeTotal = dr[2].ToString(),

                                });

                            }
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                //string msg = ex.Message;
                List<IncomeStatmentVM> lmd = new List<IncomeStatmentVM>();
                return lmd;
            }

        }




        public async Task<AccountVM> GetAccDataFunc(int AccountId)
        {
            AccountVM Data = new AccountVM();
            try
            {
                var AccountData = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.AccountId == AccountId).Select(x => new AccountVM
                {
                    AccountId = x.AccountId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Code = x.Code,
                    IsMain = x.IsMain,
                    AccountIdAhlak=x.AccountIdAhlak??0,
                    ParentId = x.ParentId ?? 0

                }).ToList().Select(s => new AccountVM()
                {
                    AccountId = s.AccountId,
                    NameAr = s.NameAr,
                    NameEn = s.NameEn,
                    Code = s.Code,
                    IsMain = s.IsMain,
                    AccountIdAhlak = s.AccountIdAhlak??0,
                    ParentId = s.ParentId ?? 0


                });

                if (AccountData.Count() > 0)
                {
                    Data = AccountData.FirstOrDefault();
                }
                else
                {
                    Data = new AccountVM();
                }
                return Data;
            }
            catch (Exception ex)
            {
                return new AccountVM();
            }


        }
        public async Task<AccountVM> GetAccDataFuncParentOfParent(int AccountId)
        {
            try
            {
                var Acc1 = await GetAccDataFunc(AccountId);
                if (Acc1 != null)
                {
                    var Acc2 =await GetAccDataFunc(Acc1.ParentId??0);
                    if (Acc2 != null)
                    {
                        return Acc2;
                    }
                    else
                    {
                        return Acc1;
                    }
                }
                else
                {
                    return Acc1;
                }
            }
            catch (Exception ex)
            {

                return new AccountVM();
            }
            

        }

        public async Task<decimal> GetAccAhlakFunc()
        {
            decimal Sum = 0;
            try
            {
                var AccountData = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.AccountIdAhlak!=null).Select(x => new AccountVM
                {
                    AccountId = x.AccountId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Code = x.Code,
                    IsMain = x.IsMain,
                    AccountIdAhlak = x.AccountIdAhlak ?? 0,
                    ParentId=x.ParentId??0
                }).ToList().Select(s => new AccountVM()
                {
                    AccountId = s.AccountId,
                    NameAr = s.NameAr,
                    NameEn = s.NameEn,
                    Code = s.Code,
                    IsMain = s.IsMain,
                    AccountIdAhlak = s.AccountIdAhlak ?? 0,
                    ParentId = s.ParentId ?? 0


                });



                if (AccountData.Count() > 0)
                {
                    foreach (var val in AccountData)
                    {
                        Sum += (val.TotalCredit??0 - val.TotalDepit??0);                       
                    }
                }
                return Convert.ToDecimal(Math.Round(double.Parse(Sum.ToString()), 2));
            }
            catch (Exception ex)
            {
                return 0;
            }


        }

        public class DataAhlakReturn
        {
            public int TransactionId { get; set; }
            public int? AccountId { get; set; }
            public int? AccountIdAhlak { get; set; }
            public decimal? Balance { get; set; }
        }
        public async Task<decimal> GetAhlakFunc(string Con)
        {
            decimal Sum = 0;
            using (SqlConnection con = new SqlConnection(Con))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "rptGetAhlakData";
                    command.Connection = con;

                    command.Parameters.Add(new SqlParameter("@Type", 1));
                    command.Parameters.Add(new SqlParameter("@AccountId", DBNull.Value));


                    con.Open();

                    SqlDataAdapter a = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    a.Fill(ds);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        Sum += (Convert.ToDecimal(Math.Round(double.Parse(dr[2].ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(dr[3].ToString()), 2)));

                    }

                }
            }
            return Sum;
        }
        public async Task<decimal> GetAhlakFuncSpacifcAcc(string Con,int AccountId)
        {
            decimal Sum = 0;
            using (SqlConnection con = new SqlConnection(Con))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "rptGetAhlakData";
                    command.Connection = con;

                    command.Parameters.Add(new SqlParameter("@Type", 2));
                    command.Parameters.Add(new SqlParameter("@AccountId", AccountId));


                    con.Open();

                    SqlDataAdapter a = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    a.Fill(ds);
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        Sum += (Convert.ToDecimal(Math.Round(double.Parse(dr[2].ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(dr[3].ToString()), 2)));

                    }

                }
            }
            return Sum;
        }


        public async Task<decimal> GetAhlakParent(string Con,int AccountId)
        {
            decimal Sum = 0;
            var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.ParentId== AccountId).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                NameAr=x.NameAr,
                Code = x.Code,
                AccountIdAhlak=x.AccountIdAhlak??0,
                //ChildAccounts = GetChildAhlak(x.ChildsAccount)
            }).ToList();

            if(allAccounts.Count()>0)
            {
                foreach (var item in allAccounts)
                {
                    if(item.AccountIdAhlak!=0)
                    {
                        Sum += await GetAhlakFuncSpacifcAcc(Con, item.AccountIdAhlak??0);
                    }
                    var Accitem = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.ParentId == item.AccountId).Select(x => new AccountVM
                    {
                        AccountId = x.AccountId,
                        NameAr = x.NameAr,
                        Code = x.Code,
                        AccountIdAhlak = x.AccountIdAhlak ?? 0,
                        //ChildAccounts = GetChildAhlak(x.ChildsAccount)
                    }).ToList();
                    if (Accitem.Count() > 0)
                    {
                        foreach (var item2 in Accitem)
                        {
                            if (item2.AccountIdAhlak != 0)
                            {
                                Sum += await GetAhlakFuncSpacifcAcc(Con, item2.AccountIdAhlak ?? 0);
                            }
                        }
                    }
                       
                }
            }
            else
            {
                var allAccounts2 = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.AccountId == AccountId).Select(x => new AccountVM
                {
                    AccountId = x.AccountId,
                    NameAr = x.NameAr,
                    Code = x.Code,
                    AccountIdAhlak = x.AccountIdAhlak ?? 0,
                    //ChildAccounts = GetChildAhlak(x.ChildsAccount)
                }).ToList();

                if (allAccounts2.FirstOrDefault().AccountIdAhlak != 0)
                {
                    Sum = await GetAhlakFuncSpacifcAcc(Con, allAccounts2.FirstOrDefault().AccountIdAhlak ?? 0);
                }

            }


            return Sum;
        }
        private async Task<List<AccountVM>> GetChildAhlak(IEnumerable<Accounts> accounts,string Con)
        {
            decimal SumChildAhlak = 0;
            List<AccountVM> accountsVm = new List<AccountVM>();
            foreach (var item in accounts)
            {
                if (item!.ChildsAccount.Count == 0 && item.IsMain == false && item.IsDeleted == false)
                {
                    try
                    {
                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            NameAr = item.NameAr,
                            Code = item.Code,
                            AccountIdAhlak = item.AccountIdAhlak??0,
                        });
                        SumChildAhlak += await GetAhlakFuncSpacifcAcc(Con, item.AccountIdAhlak??0);
                    }
                    catch (Exception)
                    {
                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            NameAr = item.NameAr,
                            Code = item.Code,
                            AccountIdAhlak = item.AccountIdAhlak??0,
                        });
                        SumChildAhlak = 0;
                    }
                }
                else
                {
                    if (item.IsDeleted == false)
                    {
                        try
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                NameAr = item.NameAr,
                                Code = item.Code,
                                AccountIdAhlak = item.AccountIdAhlak??0,
                                ChildAccounts = await GetChildAhlak(item.ChildsAccount.Where(c => c.IsDeleted == false),Con),
                            });
                        }
                        catch (Exception)
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                NameAr = item.NameAr,
                                Code = item.Code,
                                AccountIdAhlak = item.AccountIdAhlak??0,
                                ChildAccounts = await GetChildAhlak(item.ChildsAccount.Where(c => c.IsDeleted == false),Con),
                            });
                        }

                    }


                }
            }
            return accountsVm;
        }

        public async Task<IEnumerable<object>> FuncConectionSelectAhlak(string Con, string SelectStetment)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString()
            });
        }
        public async Task<string> FuncConectionSelectTotalAhlak(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();
            var Val=myDataTable.AsEnumerable().Select(row => new
            {
                Sum = row[0].ToString(),
            });
            if (Val.Count() > 0)
            {
                string TotalSum = Val.FirstOrDefault().Sum.ToString();
                return TotalSum;
            }
            else
            {
                return "0";
            }
        //    string TotalSum=Val.FirstOrDefault().Sum.ToString();
        //    return TotalSum;
        }

        public async Task<bool> checkParentOneChild(int AccountId)
        {
            try
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.ParentId == AccountId).Select(x => new AccountVM
                {
                    AccountId = x.AccountId,
                    NameAr = x.NameAr,
                    Code = x.Code,
                    AccountIdAhlak = x.AccountIdAhlak ?? 0,
                }).ToList();
                if (allAccounts.Count() <= 1)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IEnumerable<GeneralBudgetVM>> GetGeneralBudgetAMRDGV(string FromDate, string ToDate, string LVL, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck)
        {
            try
            {
                List<GeneralBudgetVM> lmd = new List<GeneralBudgetVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetBudgetBalanceAMR";
                        command.Connection = con;
                        string from = null; string to = null; int CostId = 0;

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@FromDate", "2000-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", "2100-12-12"));
                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@ToDate", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        if (CCID == 0)
                        {
                            CostId = 0;
                            command.Parameters.Add(new SqlParameter("@CCID", CostId));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        string TempParent = "Temp"; string TempParentOsol = "Temp"; int TempParentInt = 0; int TempParentIntAcc = 0;
                        decimal ParSum=0; decimal TotSum = 0; decimal TotSum5somandh2o2 = 0; int Counter = 0; bool CheckTemp = true;
                        decimal AccValOsol = 0; bool CheckTempOsol = true; bool CheckOsolSum = false; decimal AhlakAcc = 0;
                        decimal ValueAfterAhlak=0; decimal AhlakAcc2 = 0; decimal ValueAfterAhlak2 = 0;
                        bool EnterHereAgainOneChild = true;

                        foreach (DataRow dr in dt.Rows)
                        {
                            Counter++;
                            string EndTotalDebit, EndTotalCredit;
                            double PeriodDebit, PeriodCredit, DebitOP, CreditOP;
                            try
                            {PeriodDebit = double.Parse(dr[10].ToString());}
                            catch (Exception)
                            {PeriodDebit = 0;}
                            try
                            {PeriodCredit = double.Parse(dr[9].ToString());}
                            catch (Exception)
                            {PeriodCredit = 0;}
                            try
                            {DebitOP = double.Parse(dr[7].ToString());}
                            catch (Exception)
                            {DebitOP = 0;}
                            try
                            {CreditOP = double.Parse(dr[8].ToString());}
                            catch (Exception)
                            {CreditOP = 0;}
                            var checkValueDEPIT = DebitOP + PeriodDebit;
                            var checkValueCREDIT = CreditOP + PeriodCredit;

                            EndTotalDebit = (checkValueDEPIT - checkValueCREDIT).ToString();

                            string AccNameValue=""; string AccNameValuePre = ""; int ParValChecl = 0;
                            if (dr[0].ToString()!="")
                            {
                                if (LVL.ToString() != "1")
                                {
                                    if (TempParent == "Temp")
                                    {
                                        if (dr[5].ToString() == "") { ParValChecl = 0; }
                                        else { ParValChecl = Convert.ToInt32(dr[5].ToString()); }
                                        AccNameValue = GetAccDataFunc(ParValChecl).Result.NameAr;
                                        //var Result_ST = await GetAccDataFunc(ParValChecl);
                                        //AccNameValue = Result_ST.NameAr;
                                        lmd.Add(new GeneralBudgetVM
                                        {
                                            GBName = AccNameValue,
                                            AccCode = "",
                                            GBBalance = "",
                                            isfixed = "1",

                                        });
                                    }
                                }
                            }

                            if (CheckTemp==true)
                            {
                                if(dr[5].ToString()=="")
                                {
                                    TempParent = "Temp";
                                }
                                else
                                {
                                    TempParent = dr[5].ToString();
                                }
                            }
                           
                            if (ZeroCheck == 1)
                            {
                                if (EndTotalDebit == "0" || EndTotalDebit == "")
                                { }
                                else
                                {
                                    //3 -- classification
                                    //5 -- Parent
                                    //6 -- accountIdAhlak
                                    //11 -- AccountId
                                    if (dr[3].ToString() == "10" || dr[3].ToString() == "19" || dr[3].ToString() == "11" /*|| dr[5].ToString() == "27" || dr[11].ToString() == "27"*/)
                                    { }
                                    else
                                    {

                                        if (dr[5].ToString() == "27" || dr[11].ToString() == "27")
                                        {
                                            var SelectStetment = "";
                                            SelectStetment = "select Sum(Credit)-SUM(Depit) from Acc_Transactions tra join Acc_Accounts acc on tra.AccountId=acc.AccountIdAhlak where tra.BranchId= " + BranchId + " and tra.YearId= " + YearId + "";
                                            //var Value = FuncConectionSelectTotalAhlak(Con, SelectStetment);
                                            var Value = GetAhlakFunc(Con).Result;

                                            EndTotalDebit = (double.Parse(EndTotalDebit) + double.Parse(Value.ToString())).ToString();
                                        }

                                        if (dr[0].ToString() != "")
                                        {
                                            if (LVL.ToString() != "1")
                                            {


                                                if (Counter == 15)
                                                { }
                                                if (Counter == 30)
                                                { }

                                                int temponeChild = 0;
                                                if (dr[5].ToString() == "") { temponeChild = 0; }
                                                else { temponeChild = Convert.ToInt32(dr[5].ToString()); }
                                                var temp2 = TempParent;
                                                var temp3 = TempParentInt;

                                                if ((dr[5].ToString() == TempParent) && (EnterHereAgainOneChild == true)) //dr[5].ToString()--> parent l hsab l wa2fen 3leh delwa2ty
                                                {
                                                    CheckTemp = false;
                                                }
                                                else
                                                {
                                                    if (dr[5].ToString() == "") { ParValChecl = 0; }
                                                    else { ParValChecl = Convert.ToInt32(dr[5].ToString()); }

                                                    AccNameValue =  GetAccDataFunc(ParValChecl).Result.NameAr;
                                                    //var Result_ST = await GetAccDataFunc(ParValChecl);
                                                    //AccNameValue = Result_ST.NameAr;
                                                    try
                                                    {
                                                        TempParentInt = Convert.ToInt32(TempParent);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        TempParentInt = 0;
                                                    }
                                                    AccNameValuePre = GetAccDataFunc(TempParentInt).Result.NameAr;
                                                    //var Result_STPre = await GetAccDataFunc(TempParentInt);
                                                    //AccNameValuePre = Result_STPre.NameAr;

                                                    if (LVL.ToString() == "3")
                                                    {
                                                        bool CheckRetoneChild =await checkParentOneChild(temponeChild);
                                                        if (CheckRetoneChild == true)
                                                        {
                                                            AccNameValuePre = "";
                                                            EnterHereAgainOneChild = false;
                                                            CheckTemp = false;

                                                        }
                                                        else
                                                        {
                                                            EnterHereAgainOneChild = true;
                                                            CheckTemp = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        EnterHereAgainOneChild = true;
                                                        CheckTemp = true;

                                                    }

                                                    AhlakAcc2 = 0;
                                                    ValueAfterAhlak2 = 0;

                                                    AhlakAcc2 = await GetAhlakParent(Con, int.Parse(dr[11].ToString()));
                                                    ValueAfterAhlak2 = Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc2.ToString()), 2));

                                                    lmd.Add(new GeneralBudgetVM
                                                    {
                                                        GBName = " أجمالي " + AccNameValuePre,
                                                        AccCode = "",
                                                        GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ValueAfterAhlak2.ToString()), 2))).ToString(),
                                                        isfixed = "2",

                                                    });

                                                    if (CheckTempOsol == true)
                                                    {
                                                        if (dr[5].ToString() == "")
                                                        {
                                                            TempParentOsol = "Temp";
                                                        }
                                                        else
                                                        {
                                                            TempParentOsol = dr[5].ToString();
                                                        }
                                                    }

                                                    var AccualParentNow =  GetAccDataFunc(TempParentInt).Result.ParentId.ToString();
                                                    //var Result_STParentNow = await GetAccDataFunc(TempParentInt);
                                                    //var AccualParentNow = Result_STParentNow.ParentId.ToString();
                                                    if ((AccualParentNow == TempParentOsol) && (TempParentOsol == "1"))
                                                    {
                                                        AccValOsol += Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2));
                                                    }
                                                    if (CheckTempOsol == true)
                                                    {
                                                        TempParentOsol =  GetAccDataFunc(TempParentInt).Result.ParentId.ToString();
                                                        //var Result_STTempParentOsol = await GetAccDataFunc(TempParentInt);
                                                        //TempParentOsol = Result_STTempParentOsol.ParentId.ToString();
                                                        AccValOsol = Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2));
                                                        CheckTempOsol = false;
                                                    }

                                                    ParSum = 0;
                                                    try
                                                    {
                                                        TempParentIntAcc = Convert.ToInt32(AccualParentNow);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        TempParentIntAcc = 0;
                                                    }
                                                    if (LVL.ToString() == "3")
                                                    {
                                                        int checkvalosol = GetAccDataFuncParentOfParent(int.Parse(dr[11].ToString()))?.Result.ParentId ?? 0;
                                                        if (checkvalosol > 1)
                                                        {
                                                            if (CheckOsolSum == false)
                                                            {
                                                                lmd.Add(new GeneralBudgetVM
                                                                {
                                                                    GBName = " إجمالي الأصول ",
                                                                    AccCode = "",
                                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(AccValOsol.ToString()), 2))).ToString(),
                                                                    isfixed = "3",

                                                                });
                                                                CheckOsolSum = true;
                                                            }

                                                        }
                                                    }

                                                    lmd.Add(new GeneralBudgetVM
                                                    {
                                                        GBName = AccNameValue,
                                                        AccCode = "",
                                                        GBBalance = "",
                                                        isfixed = "1",

                                                    });
                                                    CheckTemp = true;
                                                }
                                            }

                                            if (LVL.ToString() == "1")
                                            {
                                                if ((dr[11]).ToString() == "1")
                                                {
                                                    decimal SumAsol = 0;
                                                    var SumValAhlak = GetAhlakFunc(Con).Result;

                                                    SumAsol = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2)) - Convert.ToDecimal(Math.Round(double.Parse(SumValAhlak.ToString()), 2));
                                                    lmd.Add(new GeneralBudgetVM
                                                    {
                                                        GBName = (dr[1]).ToString(),
                                                        AccCode = (dr[0]).ToString(),
                                                        GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(SumAsol.ToString()), 2))).ToString(),
                                                        isfixed = "0",

                                                    });
                                                }
                                                else
                                                {

                                                    lmd.Add(new GeneralBudgetVM
                                                    {
                                                        GBName = (dr[1]).ToString(),
                                                        AccCode = (dr[0]).ToString(),
                                                        GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2))).ToString(),
                                                        isfixed = "0",

                                                    });
                                                }

                                            }
                                            else
                                            {
                                                AhlakAcc = 0;
                                                ValueAfterAhlak = 0;

                                                AhlakAcc = await GetAhlakParent(Con, int.Parse(dr[11].ToString()));
                                                ValueAfterAhlak = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));
                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = (dr[1]).ToString(),
                                                    AccCode = (dr[0]).ToString(),
                                                    //GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2))).ToString(),
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ValueAfterAhlak.ToString()), 2))).ToString(),
                                                    isfixed = "0",

                                                });
                                            }

                                            ParSum = ParSum + Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));
                                            TotSum = TotSum + Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));

                                            try
                                            {
                                                TempParentInt = Convert.ToInt32((dr[5]).ToString());
                                            }
                                            catch (Exception)
                                            {
                                                TempParentInt = 0;
                                            }

                                        }

                                    }

                                }
                                if (LVL.ToString() != "1")
                                {
                                    if (Counter == dt.Rows.Count)
                                    {
                                        AccNameValuePre =  GetAccDataFunc(TempParentInt).Result.NameAr;
                                        //var Result_STpre = await GetAccDataFunc(TempParentInt);
                                        //AccNameValuePre = Result_STpre.NameAr.ToString();
                                        lmd.Add(new GeneralBudgetVM
                                        {
                                            GBName = " أجمالي " + AccNameValuePre,
                                            AccCode = "",
                                            GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2))).ToString(),
                                            isfixed = "2",

                                        });
                                        ParSum = 0;
                                        TotSum5somandh2o2 = Convert.ToDecimal(Math.Round(double.Parse(TotSum.ToString()), 2)) - (AccValOsol);
                                        lmd.Add(new GeneralBudgetVM
                                        {
                                            GBName = " إجمالي الالتزمات وحقوق الملكية ",
                                            AccCode = "",
                                            GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(TotSum5somandh2o2.ToString()), 2))).ToString(),
                                            isfixed = "3",

                                        });

                                    }

                                }

                            }
                            else
                            {
                                if (dr[3].ToString() == "10" || dr[3].ToString() == "19" || dr[3].ToString() == "11" /*|| dr[5].ToString()=="27" || dr[11].ToString() == "27"*/)
                                {}
                                else
                                {
                                    if (dr[5].ToString() == "27" || dr[11].ToString() == "27")
                                    {
                                        var SelectStetment = "";
                                        SelectStetment = "select Sum(Credit)-SUM(Depit) from Acc_Transactions tra join Acc_Accounts acc on tra.AccountId=acc.AccountIdAhlak where tra.BranchId= " + BranchId + " and tra.YearId= " + YearId + "";
                                        //var Value = FuncConectionSelectTotalAhlak(Con, SelectStetment);
                                        var Value=GetAhlakFunc(Con).Result;

                                        EndTotalDebit = (double.Parse(EndTotalDebit) + double.Parse(Value.ToString())).ToString();

                                    }
                                    if (dr[0].ToString() != "")
                                    {
                                        if (LVL.ToString() != "1")
                                        {


                                            if (Counter == 15)
                                            { }
                                            if (Counter == 30)
                                            { }

                                            int temponeChild = 0;
                                            if (dr[5].ToString() == "") { temponeChild = 0; }
                                            else { temponeChild = Convert.ToInt32(dr[5].ToString()); }
                                            var temp2 = TempParent;
                                            var temp3 = TempParentInt;

                                            if ( (dr[5].ToString() == TempParent) && (EnterHereAgainOneChild==true) ) //dr[5].ToString()--> parent l hsab l wa2fen 3leh delwa2ty
                                            {
                                                CheckTemp = false;
                                            }
                                            else
                                            {
                                                if (dr[5].ToString() == "") { ParValChecl = 0; }
                                                else { ParValChecl = Convert.ToInt32(dr[5].ToString()); }
                                                
                                                AccNameValue = GetAccDataFunc(ParValChecl).Result.NameAr;
                                                //var Result_ST = await GetAccDataFunc(ParValChecl);
                                                //AccNameValue = Result_ST.NameAr.ToString();
                                                try
                                                {
                                                    TempParentInt = Convert.ToInt32(TempParent);
                                                }
                                                catch (Exception)
                                                {
                                                    TempParentInt = 0;
                                                }

                                                AccNameValuePre = GetAccDataFunc(TempParentInt).Result.NameAr;
                                                //var Result_STPre = await GetAccDataFunc(TempParentInt);
                                                //AccNameValuePre = Result_STPre.NameAr.ToString();


                                                if (LVL.ToString() == "3")
                                                {
                                                    bool CheckRetoneChild = await checkParentOneChild(temponeChild);
                                                    if (CheckRetoneChild == true)
                                                    {
                                                        AccNameValuePre = "";
                                                        EnterHereAgainOneChild = false;
                                                        CheckTemp = false;

                                                    }
                                                    else
                                                    {
                                                        EnterHereAgainOneChild = true;
                                                        CheckTemp = true;
                                                    }
                                                }
                                                else
                                                {
                                                    EnterHereAgainOneChild = true;
                                                    CheckTemp = true;

                                                }

                                                AhlakAcc2 = 0;
                                                ValueAfterAhlak2 = 0;

                                                AhlakAcc2 = await GetAhlakParent(Con, int.Parse(dr[11].ToString()));
                                                ValueAfterAhlak2 = Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc2.ToString()), 2));

                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = " أجمالي " + AccNameValuePre,
                                                    AccCode = "",
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ValueAfterAhlak2.ToString()), 2))).ToString(),
                                                    isfixed = "2",

                                                });

                                                if (CheckTempOsol == true)
                                                {
                                                    if (dr[5].ToString() == "")
                                                    {
                                                        TempParentOsol = "Temp";
                                                    }
                                                    else
                                                    {
                                                        TempParentOsol = dr[5].ToString();
                                                    }
                                                }

                                                var AccualParentNow =  GetAccDataFunc(TempParentInt).Result.ParentId.ToString();
                                                //var Result_STParentNow = await GetAccDataFunc(TempParentInt);
                                                //var AccualParentNow = Result_STParentNow.ParentId.ToString();
                                                if ((AccualParentNow == TempParentOsol) && (TempParentOsol == "1"))
                                                {
                                                    AccValOsol += Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2));
                                                }
                                                if (CheckTempOsol==true)
                                                {
                                                    TempParentOsol =  GetAccDataFunc(TempParentInt).Result.ParentId.ToString();
                                                    //var Result_STParentOsol = await GetAccDataFunc(TempParentInt);
                                                    //TempParentOsol = Result_STParentOsol.ParentId.ToString();
                                                    AccValOsol = Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2));
                                                    CheckTempOsol = false;
                                                }

                                                ParSum = 0;
                                                try
                                                {
                                                    TempParentIntAcc = Convert.ToInt32(AccualParentNow);
                                                }
                                                catch (Exception)
                                                {
                                                    TempParentIntAcc = 0;
                                                }
                                                if(LVL.ToString() == "3")
                                                {
                                                    int checkvalosol =  GetAccDataFuncParentOfParent(int.Parse(dr[11].ToString()))?.Result.ParentId ?? 0;
                                                    //var Result_STcheckvalosol = await GetAccDataFuncParentOfParent(int.Parse(dr[11].ToString()));
                                                    //int checkvalosol = Result_STcheckvalosol?.ParentId??0;
                                                    if (checkvalosol > 1)
                                                    {
                                                        if (CheckOsolSum == false)
                                                        {
                                                            lmd.Add(new GeneralBudgetVM
                                                            {
                                                                GBName = " إجمالي الأصول ",
                                                                AccCode = "",
                                                                GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(AccValOsol.ToString()), 2))).ToString(),
                                                                isfixed = "3",

                                                            });
                                                            CheckOsolSum = true;
                                                        }

                                                    }
                                                }

                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = AccNameValue,
                                                    AccCode = "",
                                                    GBBalance = "",
                                                    isfixed = "1",

                                                });
                                                CheckTemp = true;
                                            }
                                        }

                                        if(LVL.ToString() == "1")
                                        {
                                            if ((dr[11]).ToString() == "1")                                                  
                                            {
                                                decimal SumAsol = 0;
                                                var SumValAhlak = GetAhlakFunc(Con).Result;

                                                SumAsol = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2))-Convert.ToDecimal(Math.Round(double.Parse(SumValAhlak.ToString()), 2));
                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = (dr[1]).ToString(),
                                                    AccCode = (dr[0]).ToString(),
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(SumAsol.ToString()), 2))).ToString(),
                                                    isfixed = "0",

                                                });
                                            }
                                            else if ((dr[11]).ToString() == "2")
                                            {
                                                decimal Sum5som = 0;
                                                var SumValAhlak5som = GetAhlakFunc(Con).Result;

                                                Sum5som = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2)) + Convert.ToDecimal(Math.Round(double.Parse(SumValAhlak5som.ToString()), 2));
                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = (dr[1]).ToString(),
                                                    AccCode = (dr[0]).ToString(),
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(Sum5som.ToString()), 2))).ToString(),
                                                    isfixed = "0",

                                                });
                                            }
                                            else
                                            {

                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = (dr[1]).ToString(),
                                                    AccCode = (dr[0]).ToString(),
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2))).ToString(),
                                                    isfixed = "0",

                                                });
                                            }
                                            
                                        }
                                        else if (LVL.ToString() == "2")
                                        {

                                            if ((dr[11]).ToString() == "20")
                                            {
                                                AhlakAcc = 0;
                                                ValueAfterAhlak = 0;

                                                decimal Sum5som = 0;
                                                AhlakAcc =  GetAhlakFunc(Con).Result;

                                                Sum5som = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2)) + Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));
                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = (dr[1]).ToString(),
                                                    AccCode = (dr[0]).ToString(),
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(Sum5som.ToString()), 2))).ToString(),
                                                    isfixed = "0",

                                                });
                                                AhlakAcc = -(AhlakAcc);
                                            }    
                                            else
                                            {
                                                AhlakAcc = 0;
                                                ValueAfterAhlak = 0;

                                                AhlakAcc = await GetAhlakParent(Con, int.Parse(dr[11].ToString()));
                                                ValueAfterAhlak = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));
                                                lmd.Add(new GeneralBudgetVM
                                                {
                                                    GBName = (dr[1]).ToString(),
                                                    AccCode = (dr[0]).ToString(),
                                                    //GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2))).ToString(),
                                                    GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ValueAfterAhlak.ToString()), 2))).ToString(),
                                                    isfixed = "0",

                                                });
                                            }
                                        }
                                        else
                                        {
                                            AhlakAcc = 0;
                                            ValueAfterAhlak = 0;

                                            AhlakAcc = await GetAhlakParent(Con, int.Parse(dr[11].ToString()));
                                            ValueAfterAhlak = Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));
                                            lmd.Add(new GeneralBudgetVM
                                            {
                                                GBName = (dr[1]).ToString(),
                                                AccCode = (dr[0]).ToString(),
                                                //GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit), 2))).ToString(),
                                                GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ValueAfterAhlak.ToString()), 2))).ToString(),
                                                isfixed = "0",

                                            });
                                        }

                                        ParSum = ParSum + Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));
                                        TotSum = TotSum + Convert.ToDecimal(Math.Round(double.Parse(EndTotalDebit.ToString()), 2)) - Convert.ToDecimal(Math.Round(double.Parse(AhlakAcc.ToString()), 2));

                                        try
                                        {
                                            TempParentInt = Convert.ToInt32((dr[5]).ToString());
                                        }
                                        catch (Exception)
                                        {
                                            TempParentInt = 0;
                                        }

                                    }

                                }
                                if (LVL.ToString() != "1")
                                {
                                    if (Counter == dt.Rows.Count)
                                    {

                                        AccNameValuePre =  GetAccDataFunc(TempParentInt).Result.NameAr;
                                        //var Result_STPre = await GetAccDataFunc(TempParentInt);
                                        //AccNameValuePre = Result_STPre.NameAr;
                                        lmd.Add(new GeneralBudgetVM
                                        {
                                            GBName = " أجمالي " + AccNameValuePre,
                                            AccCode = "",
                                            GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(ParSum.ToString()), 2))).ToString(),
                                            isfixed = "2",

                                        });
                                        ParSum = 0;
                                        TotSum5somandh2o2 = Convert.ToDecimal(Math.Round(double.Parse(TotSum.ToString()), 2)) - (AccValOsol);
                                        lmd.Add(new GeneralBudgetVM
                                        {
                                            GBName = " إجمالي الالتزمات وحقوق الملكية ",
                                            AccCode = "",
                                            GBBalance = (Convert.ToDecimal(Math.Round(double.Parse(TotSum5somandh2o2.ToString()), 2))).ToString(),
                                            isfixed = "3",

                                        });
                                    }

                                }
                            }
                        }
                    }
                }
                return lmd;
            }
            catch(Exception ex)
            {
                var v = ex;
                List<GeneralBudgetVM> lmd = new List<GeneralBudgetVM>();
                return lmd;
            }

        }
        //heba
        public async Task<DataTable> GetGeneralBudgetFRENCHDGV(string FromDate, string ToDate, string LVL, int CCID, int YearId, int BranchId, string lang, string Con, int ZeroCheck)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetBudgetBalanceFr";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int CostId = 0;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@FromDate", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@ToDate", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }


                        if (LVL == "")
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", DBNull.Value));
                        }

                        else if (int.Parse(LVL) < 2)
                        {
                            LVL = null;
                            command.Parameters.Add(new SqlParameter("@lvl", 2));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@lvl", int.Parse(LVL)));
                        }

                        //dawoud2

                        //if (CCID == "")
                        //{

                        //    command.Parameters.Add(new SqlParameter("@CCID", "0"));
                        //}
                        //else
                        //{
                        //    command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        //}



                        if (CCID == 0)
                        {
                            CostId = 0;
                            command.Parameters.Add(new SqlParameter("@CCID", CostId));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CCID", CCID));
                        }



                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));



                        //command.Parameters.Add(new SqlParameter("@branch", BranchId));
                        //command.Parameters.Add(new SqlParameter("@yearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);

                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {


                            if (ZeroCheck == 1)
                            {
                                if ((dr[2]).ToString() == "0" && dr[3].ToString() == "0")
                                {

                                }
                                else if ((dr[2]).ToString() == "" && dr[3].ToString() == "0")
                                {

                                }
                                else if ((dr[2]).ToString() == "0" && dr[3].ToString() == "")
                                {

                                }
                                else if ((dr[2]).ToString() == "" && dr[3].ToString() == "")
                                {

                                }
                                else
                                {
                                    dt = new DataTable();

                                }
                            }

                        }
                    }
                }
                return dt;
            }
            catch
            {
                //string msg = ex.Message;
                DataTable dt = new DataTable();
                return dt;
            }

        }
        //salah
        public async Task<IEnumerable<GeneralmanagerRevVM>> GetGeneralManagerRevenueAMRDGV(int? ManagerId, string FromDate, string ToDate, int BranchId, string Con, int? YearId, int? taxID, int? taxID2)
        {
            try
            {
                List<GeneralmanagerRevVM> lmd = new List<GeneralmanagerRevVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetManagerRevenue";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        int CostId = 0;


                        if (FromDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@FromDate", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", from));

                        }
                        if (ToDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;

                            command.Parameters.Add(new SqlParameter("@ToDate", to));
                        }


                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@MId", ManagerId));
                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new GeneralmanagerRevVM
                            {
                                InvId = (dr[1]).ToString(),
                                ProjNum = (dr[2]).ToString(),
                                Date = (dr[0]).ToString(),
                                amount = dr[3].ToString(),
                                Taxes = (string.IsNullOrEmpty((dr[4]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[4].ToString())) + "",

                            });
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<GeneralmanagerRevVM> lmd = new List<GeneralmanagerRevVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<ClosingVouchers>> GetClosingVouchers(int BranchId, string Con, int? YearId)
        {
            try
            {
                List<ClosingVouchers> lmd = new List<ClosingVouchers>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetClosingVouchers";
                        command.Connection = con;

                        command.Parameters.Add(new SqlParameter("@FromDate", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@ToDate", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        //command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        int Counter = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            var ValueAmount = (string.IsNullOrEmpty((dr[2]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[2].ToString())) + "";
                            var CreditOrDepitType_V = "C";
                            decimal TotalCreditV = 0; decimal TotalDepitV = 0;
                            if (Convert.ToDouble(ValueAmount) != 0)
                            {
                                Counter += 1;
                                if(Convert.ToDouble(ValueAmount)>0)
                                {
                                    CreditOrDepitType_V = "D";
                                    TotalCreditV = TotalCreditV + Convert.ToDecimal(ValueAmount);
                                }
                                else
                                {
                                    CreditOrDepitType_V = "C";
                                    ValueAmount = (Convert.ToDecimal(ValueAmount) * -1).ToString();
                                    TotalDepitV = TotalDepitV + Convert.ToDecimal(ValueAmount);

                                }
                                lmd.Add(new ClosingVouchers
                                {
                                    LineNumber = Counter,
                                    AccountId = Convert.ToInt32((dr[0]).ToString()),
                                    AccountName = (dr[1]).ToString(),
                                    CreditDepit = ValueAmount,
                                    CostCenterName = (dr[5]).ToString(),
                                    CostCenterId = Convert.ToInt32((dr[4]).ToString()),
                                    Notes = "قيد اقفال",
                                    InvoiceReference = "",
                                    CreditOrDepitType= CreditOrDepitType_V,
                                    TotalCredit=TotalCreditV.ToString(),
                                    TotalDepit=TotalDepitV.ToString(),
                                    BranchId = Convert.ToInt32((dr[3]).ToString())

                                });
                            }

                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<ClosingVouchers> lmd = new List<ClosingVouchers>();
                return lmd;
            }

        }

        public async Task<IEnumerable<CostCenterEX_REVM>> GetCostCenterEX_RE(int? CostCenterId, string FromDate, string ToDate, int BranchId, string Con, int? YearId, int? taxID, int? taxID2, int? AccountID_MR, string FlagTotal)
        {
            try
            {
                List<CostCenterEX_REVM> lmd = new List<CostCenterEX_REVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetCostCenterEX_RE";
                        command.Connection = con;
                        string from = null;
                        string to = null;

                        if (FromDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            command.Parameters.Add(new SqlParameter("@FromDate", DBNull.Value));
                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", from));

                        }
                        if (ToDate == "" || String.IsNullOrEmpty(ToDate))
                        {
                            command.Parameters.Add(new SqlParameter("@ToDate", DBNull.Value));
                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@ToDate", to));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        if(CostCenterId>0)
                        {
                            command.Parameters.Add(new SqlParameter("@CostId", CostCenterId));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CostId", DBNull.Value));
                        }
                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));
                        command.Parameters.Add(new SqlParameter("@AccountID_MR", AccountID_MR));
                        command.Parameters.Add(new SqlParameter("@ExpenseType", 1));
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        decimal ExDepit_V = 0; decimal ReCredit_V = 0; decimal EX_RE_Diff_V = 0;

                        decimal ExDepit_Total = 0; decimal ReCredit_Total = 0; decimal EX_RE_Diff_Total = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            ExDepit_V = Convert.ToDecimal((dr[0]).ToString());
                            ReCredit_V = Convert.ToDecimal((dr[1]).ToString());
                            EX_RE_Diff_V = Convert.ToDecimal((dr[0]).ToString()) - Convert.ToDecimal((dr[1]).ToString());
                            ExDepit_Total = ExDepit_Total + ExDepit_V;
                            ReCredit_Total = ReCredit_Total + ReCredit_V;
                            EX_RE_Diff_Total = EX_RE_Diff_Total + EX_RE_Diff_V;

                            if(FlagTotal== "0" || FlagTotal == "2")
                            {
                                lmd.Add(new CostCenterEX_REVM
                                {
                                    ExDepit = ExDepit_V.ToString(),
                                    ReCredit = ReCredit_V.ToString(),
                                    EX_RE_Diff = EX_RE_Diff_V.ToString(),
                                    CostCenterId = Convert.ToInt32(dr[2].ToString()),
                                    CostCenterCode = (dr[3]).ToString(),
                                    CostCenterName = (dr[4]).ToString(),
                                    TotalExDepit = "0",
                                    TotalReCredit = "0",
                                    TotalEX_RE_Diff = "0",
                                    Flag = "0"

                                });
                            }
                            
                        }
                        if (FlagTotal == "1" || FlagTotal == "2")
                        {
                            lmd.Add(new CostCenterEX_REVM
                            {
                                ExDepit = ExDepit_Total.ToString(),
                                ReCredit = ReCredit_Total.ToString(),
                                EX_RE_Diff = EX_RE_Diff_Total.ToString(),
                                CostCenterId = 0,
                                CostCenterCode = "",
                                CostCenterName = "الأجمالي",
                                TotalExDepit = ExDepit_Total.ToString(),
                                TotalReCredit = ReCredit_Total.ToString(),
                                TotalEX_RE_Diff = EX_RE_Diff_Total.ToString(),
                                Flag = "1"
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<CostCenterEX_REVM> lmd = new List<CostCenterEX_REVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenu(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? YearId, int? taxID, int? taxID2, int? AccountID_Mb, int? AccountID_MR)
        {
            try
            {
                List<DetailedRevenuVM> lmd = new List<DetailedRevenuVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetDetailedRevenu";
                        command.Connection = con;
                        string from = null;
                        string to = null;

                        if (FromDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@FromDate", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", from));

                        }
                        if (ToDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;

                            command.Parameters.Add(new SqlParameter("@ToDate", to));
                        }
                        //if (ExpenseType == "")
                        //{
                        //    ExpenseType = null;
                        //    command.Parameters.Add(new SqlParameter("@ExpenseType", 1));

                        //}
                        //else
                        //{
                        //    command.Parameters.Add(new SqlParameter("@ExpenseType", Convert.ToInt32(ExpenseType)));
                        //}

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@CusId", CustomerId));
                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));
                        command.Parameters.Add(new SqlParameter("@AccountID_Mb", AccountID_Mb));
                        command.Parameters.Add(new SqlParameter("@AccountID_MR", AccountID_MR));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
//salah
                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new DetailedRevenuVM
                            {
                                CustomerName_W = (dr[0]).ToString(),
                                Date = (dr[1]).ToString(),
                                InvoiceNumber = (dr[2]).ToString(),
                                AccountName = dr[3].ToString(),
                                Notes = (dr[4]).ToString(),
                                Project = (dr[5]).ToString(),
                                //ProjectType = (dr[6]).ToString()+"-"+ (dr[7]).ToString(),
                                ProjectType = (dr[7]).ToString(),
                                TotalValue = (string.IsNullOrEmpty((dr[8]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[8].ToString())) + "",
                                Taxes = (string.IsNullOrEmpty((dr[9]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[9].ToString())) + "",
                                diff = ((string.IsNullOrEmpty((dr[8]).ToString()) == true ? 0 : Convert.ToDouble(dr[8].ToString())) -
                                        (string.IsNullOrEmpty((dr[9]).ToString()) == true ? 0 : Convert.ToDouble(dr[9].ToString()))) + "",
                                //Total = Convert.ToDouble(String.Format("{0:0.00}", dr[9].ToString())) + ""
                                Total = ((string.IsNullOrEmpty((dr[8]).ToString()) == true ? 0 : Convert.ToDouble(dr[8].ToString())) +
                                        (string.IsNullOrEmpty((dr[9]).ToString()) == true ? 0 : Convert.ToDouble(dr[9].ToString()))) + "",
                                TransactionId = (dr[10]).ToString(),
                                CustomerName = (dr[11]).ToString(),


                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<DetailedRevenuVM> lmd = new List<DetailedRevenuVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<DetailedRevenuVM>> GetDetailedRevenuExtra(int? CustomerId, int? ProjectId, string FromDate, string ToDate, int BranchId, string Con, int? YearId, int? taxID, int? taxID2)
        {
            try
            {
                List<DetailedRevenuVM> lmd = new List<DetailedRevenuVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetDetailedRevenuExtra";
                        command.Connection = con;
                        string from = null;
                        string to = null;

                        if (FromDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@FromDate", "2000-01-01"));
                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", from));
                        }
                        if (ToDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", "2100-12-12"));
                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@ToDate", to));
                        }
                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@CusId", CustomerId));
                        command.Parameters.Add(new SqlParameter("@ProId", ProjectId));

                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new DetailedRevenuVM
                            {
                                CustomerName = (dr[0]).ToString(),
                                Date = (dr[1]).ToString(),
                                InvoiceNumber = (dr[2]).ToString(),
                                AccountName = dr[3].ToString(),
                                Notes = (dr[4]).ToString(),
                                Project = (dr[5]).ToString(),
                                PayTypeName = (dr[7]).ToString(),



                                TotalValue = (dr[8]).ToString(),
                                TotalValueDepit = (dr[10]).ToString(),
                                TransactionTypeName= (dr[11]).ToString(),
                                Rad = (dr[12]).ToString(),
                                Type = (dr[13]).ToString(),
                                //Taxes = (dr[9]).ToString(),
                                //diff = (Convert.ToDouble(dr[8].ToString()) - Convert.ToDouble(dr[9].ToString())).ToString(),
                                //Total = (Convert.ToDouble(dr[8].ToString()) + Convert.ToDouble(dr[9].ToString())).ToString(),


                                //TotalValue = (string.IsNullOrEmpty((dr[8]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[8].ToString())) + "",
                                //TotalValueDepit = (string.IsNullOrEmpty((dr[10]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[10].ToString())) + "",

                                //Taxes = (string.IsNullOrEmpty((dr[9]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[9].ToString())) + "",
                                //diff = ((string.IsNullOrEmpty((dr[8]).ToString()) == true ? 0 : Convert.ToDouble(dr[8].ToString())) -
                                //        (string.IsNullOrEmpty((dr[9]).ToString()) == true ? 0 : Convert.ToDouble(dr[9].ToString()))) + "",
                                //Total = ((string.IsNullOrEmpty((dr[8]).ToString()) == true ? 0 : Convert.ToDouble(dr[8].ToString())) +
                                //        (string.IsNullOrEmpty((dr[9]).ToString()) == true ? 0 : Convert.ToDouble(dr[9].ToString()))) + "",

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<DetailedRevenuVM> lmd = new List<DetailedRevenuVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<InvoicedueC>> GetInvoicedue(int? CustomerId, string FromDate, string ToDate, int BranchId, string Con, int? YearId)
        {
            try
            {
                List<InvoicedueC> lmd = new List<InvoicedueC>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetInvoicedue";
                        command.Connection = con;

                        if (FromDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            command.Parameters.Add(new SqlParameter("@FromDate", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@FromDate", FromDate));
                        }
                        if (ToDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            command.Parameters.Add(new SqlParameter("@ToDate", DBNull.Value));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ToDate", ToDate));
                        }

                        if (BranchId == 0)
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        }
                        if (CustomerId == 0 || CustomerId==null)
                        {
                            command.Parameters.Add(new SqlParameter("@CustomerId", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CustomerId", CustomerId));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            if(Convert.ToDecimal((dr["VueValue"]).ToString())>0)
                            {
                                lmd.Add(new InvoicedueC
                                {
                                    InvoiceId = Convert.ToInt32((dr["InvoiceId"]).ToString()),
                                    CustomerName = (dr["CustomerName"]).ToString(),
                                    CustomerNameW = (dr["CustomerNameW"]).ToString(),
                                    InvoiceNumber = (dr["InvoiceNumber"]).ToString(),
                                    InvoiceDate = (dr["InvoiceDate"]).ToString(),
                                    BranchName = (dr["BranchName"]).ToString(),
                                    InvoiceValue = Convert.ToDecimal((dr["InvoiceValue"]).ToString()),
                                    ValueCollect = Convert.ToDecimal((dr["ValueCollect"]).ToString()),
                                    RetinvoiceValue = Convert.ToDecimal((dr["RetinvoiceValue"]).ToString()),
                                    AccDate = (dr["AccDate"]).ToString(),
                                    DaysV = Convert.ToInt32((dr["DaysV"]).ToString()),
                                    VueValue = Convert.ToDecimal((dr["VueValue"]).ToString()),
                                });
                            }
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<InvoicedueC> lmd = new List<InvoicedueC>();
                return lmd;
            }

        }


        public async Task<IEnumerable<DetailedExpenseVM>> GetDetailedExpensesd(int? AccountId, string FromDate, string ToDate, string ExpenseType, int BranchId, string Con, int? YearId, int? taxID, int? taxID2)
        {
            try
            {
                List<DetailedExpenseVM> lmd = new List<DetailedExpenseVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetDetailedExpensesd";
                        command.Connection = con;
                        string from = null;
                        string to = null;


                        if (FromDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@FromDate", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@FromDate", from));

                        }
                        if (ToDate == "" || String.IsNullOrEmpty(FromDate))
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@ToDate", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;

                            command.Parameters.Add(new SqlParameter("@ToDate", to));
                        }
                        if (ExpenseType == "")
                        {
                            ExpenseType = null;
                            command.Parameters.Add(new SqlParameter("@ExpenseType", 1));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@ExpenseType", Convert.ToInt32(ExpenseType)));
                        }

                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@AccountId", AccountId));
                        command.Parameters.Add(new SqlParameter("@taxID", taxID));
                        command.Parameters.Add(new SqlParameter("@taxID2", taxID2));




                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new DetailedExpenseVM
                            {
                                InvoiceReference = (dr[0]).ToString(),
                                SupplierName = dr[1].ToString(),
                                TaxCode = (string.IsNullOrEmpty((dr[6]).ToString())) == true ? "" : (Convert.ToInt32((dr[6]).ToString()) == 2 ? (dr[2]).ToString() : ""),
                                Details = (dr[3]).ToString(),
                                Price = (string.IsNullOrEmpty((dr[4]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[4].ToString())) + "",
                                Taxes = (string.IsNullOrEmpty((dr[5]).ToString())) == true ? "" : Convert.ToDouble(String.Format("{0:0.00}", dr[5].ToString())) + "",
                                //Total = Convert.ToDouble(String.Format("{0:0.00}", dr[6].ToString())) + "",//; Convert.ToDouble(dr[6].ToString()).ToString("0.00") + "",
                                Total = ((string.IsNullOrEmpty((dr[4]).ToString()) == true ? 0 : Convert.ToDouble(dr[4].ToString())) +
                                        (string.IsNullOrEmpty((dr[5]).ToString()) == true ? 0 : Convert.ToDouble(dr[5].ToString()))) + "",
                                Date = (dr[7]).ToString(),
                                AccSupplierName = (dr[8]).ToString(),
                                SupplierTaxNo = (dr[9]).ToString(),
                                SupplierInvoiceNo = string.IsNullOrEmpty((dr[10]).ToString()) == true ? "" : (dr[10]).ToString(),
                                TransactionId = (dr[11]).ToString(),
                                AccClauseName= (dr[12]).ToString(),
                                JorNo = (dr[13]).ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<DetailedExpenseVM> lmd = new List<DetailedExpenseVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<AccountStatmentVM>> GetFullAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID, string Con, int BranchId, int YearId)
        {
            try
            {


                //  var accounts= AccountStatmentVM.
                List<AccountStatmentVM> lmd = new List<AccountStatmentVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "MAcc_AccountStatement";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@Datefrom", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@Datefrom", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@Dateto", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@Dateto", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (AccountCode == "")
                        {
                            AccountCode = null;
                            command.Parameters.Add(new SqlParameter("@AccId", "121"));
                        }

                        else
                        {
                            command.Parameters.Add(new SqlParameter("@AccId", AccountCode));
                        }

                        if (CCID == "")
                        {

                            command.Parameters.Add(new SqlParameter("@CostId", "1"));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@CostId", CCID));
                        }


                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@YearID", YearId));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new AccountStatmentVM
                            {
                                TransID = (dr[1]).ToString(),
                                CDate = (dr[2]).ToString(),
                                Hdate = (dr[3]).ToString(),
                                Description = dr[12].ToString(),
                                Debit = (dr[10]).ToString(),
                                Credit = (dr[9]).ToString(),
                                CostCenter = (dr[8]).ToString(),
                                Balance = 0.ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch
            {
                //string msg = ex.Message;
                List<AccountStatmentVM> lmd = new List<AccountStatmentVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<AccountVM>> GetGeneralBudgetDGV(int BranchId, string lang, int YearId, string FromDate, string ToDate, string Con)
        {
            try
            {
                List<AccountVM> lmd = new List<AccountVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "GeneralBudget";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@startdate", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@startdate", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@enddate", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@enddate", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        command.Parameters.Add(new SqlParameter("@accountlevel", 1));
                        command.Parameters.Add(new SqlParameter("@branch", 1));
                        command.Parameters.Add(new SqlParameter("@yearID", 2019));
                        //command.Parameters.Add(new SqlParameter("@startdate", Convert.ToDateTime(from)));
                        //command.Parameters.Add(new SqlParameter("@enddate", Convert.ToDateTime(to)));

                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)

                        // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new AccountVM
                            {
                                // adding data from dataset row in to list<modeldata>  

                                AccountId = Convert.ToInt32(dr["AccountId"]),
                                AccountName = (dr["AccountName"]).ToString(),
                                Code = (dr["AccountCode"]).ToString(),
                                IsMain = Convert.ToBoolean(dr["IsMain"]),
                                TotalCredit = Convert.ToDecimal(dr["TotalCredit"].ToString()),
                                TotalDepit = Convert.ToDecimal(dr["TotalDepit"].ToString()),
                                //TotalCreditOpeningBalance = Convert.ToDecimal(dr["TotalCreditOpeningBalance"].ToString()),
                                //TotalDepitOpeningBalance = Convert.ToDecimal(dr["TotalDepitOpeningBalance"].ToString()),
                                TotalCreditBalance = Convert.ToDecimal(dr["TotalCreditBalance"].ToString()),
                                TotalDepitBalance = Convert.ToDecimal(dr["TotalDepitBalance"].ToString()),
                                Notes = (dr["Notes"]).ToString(),
                                ParentId = Convert.ToInt32(dr["ParentId"])
                            });
                        }
                    }
                }

                return lmd;
            }
            catch
            {
                List<AccountVM> lmd = new List<AccountVM>();
                return lmd;
            }

        }


        public async Task<IEnumerable<AccountVM>> GetAccountSearchValue(string SearchName, int BranchId)
        {

            var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.NameAr.Contains(SearchName)).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return allAccounts;
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccount(int BranchId)
        {

            var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                Level = x.Level
            }).ToList();
            return allAccounts;
        }
        public async Task<IEnumerable<AccountVM>> GetAllAccountBySearch(AccountVM Account, int BranchId)
        {

            var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Code == Account.Code || Account.Code == null || s.Code == (Account.Code)) && (s.NameAr == Account.NameAr || Account.NameAr == null || s.NameAr.Contains(Account.NameAr)) && (s.NameEn == Account.NameEn || Account.NameEn == null || s.NameEn.Contains(Account.NameEn))).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Code = x.Code,
                Level = x.Level
            }).ToList();


            //var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.Code == Account.Code || Account.Code == null || s.Code.Contains(Account.Code)) && (s.NameAr == Account.NameAr || Account.NameAr == null || s.NameAr.Contains(Account.NameAr)) && (s.NameEn == Account.NameEn || Account.NameEn == null || s.NameEn.Contains(Account.NameEn))).Select(x => new AccountVM
            //{
            //    AccountId = x.AccountId,
            //    NameAr = x.NameAr,
            //    NameEn = x.NameEn,
            //    Code = x.Code,
            //    Level = x.Level
            //}).ToList();
            return allAccounts;
        }
        public async Task<IEnumerable<AccountVM>> GetAccsTransByType(int Type, int BranchId, string lang, int YearId, string FromDate, string ToDate)
        {
            try
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.Type == Type && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts3(x.ChildsAccount != null ? x.ChildsAccount.Where(c => c.Type == Type) : new List<Accounts>(), x.BranchId ?? 0, YearId, Type, FromDate, ToDate).Result.ToList()
                }).ToList();
                return allAccounts;
            }
            catch
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.Type == Type && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts3(x.ChildsAccount!=null ?x.ChildsAccount.Where(c => c.Type == Type):new List<Accounts>(), x.BranchId ?? 0, YearId, Type, FromDate, ToDate).Result.ToList()
                }).ToList();
                return allAccounts;
            }
        }
        //heba
        public async Task<IEnumerable<AccountVM>> GetCustomerFinancialDetailsNew(int? AccountId, string FromDate, string ToDate, int Zerocheck, int YearId, int BranchId, string lang, string Con)
        {
            try
            {
                string from = null;
                string to = null;
                List<AccountVM> lmd = new List<AccountVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetCustomerFinancialDetails";
                        command.Connection = con;


                        command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        command.Parameters.Add(new SqlParameter("@YearId", YearId));
                        command.Parameters.Add(new SqlParameter("@dir", lang));
                        command.Parameters.Add(new SqlParameter("@AccountId", AccountId));

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", "2000-01-01"));
                            //command.Parameters.Add(new SqlParameter("@From", YearId + "-01-01"));


                        }
                        else
                        {
                            from = FromDate;
                            // command.Parameters.Add(new SqlParameter("@From", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            command.Parameters.Add(new SqlParameter("@From", from));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", "2100-12-12"));
                            //command.Parameters.Add(new SqlParameter("@From", (YearId+1) + "-12-31"));


                        }
                        else
                        {
                            to = ToDate;
                            command.Parameters.Add(new SqlParameter("@to", to));
                            // command.Parameters.Add(new SqlParameter("@to", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }




                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)

                        {

                            double TotalCredit_V, TotalDepit_V, TotalCreditOpeningBalance_V, TotalDepitOpeningBalance_V, TotalCreditBalance_V, TotalDepitBalance_V;
                            try
                            {
                                TotalCredit_V = double.Parse(dr["CreditTotal"].ToString());
                            }
                            catch (Exception)
                            {
                                TotalCredit_V = 0;
                            }
                            try
                            {
                                TotalDepit_V = double.Parse(dr["DepitTotal"].ToString());
                            }
                            catch (Exception)
                            {
                                TotalDepit_V = 0;
                            }

                            try
                            {
                                TotalCreditOpeningBalance_V = double.Parse(dr["OPCredit"].ToString());
                            }
                            catch (Exception)
                            {
                                TotalCreditOpeningBalance_V = 0;
                            }

                            try
                            {
                                TotalDepitOpeningBalance_V = double.Parse(dr["OPDibit"].ToString());
                            }
                            catch (Exception)
                            {
                                TotalDepitOpeningBalance_V = 0;
                            }

                            if(TotalDepitOpeningBalance_V- TotalCreditOpeningBalance_V+ TotalDepit_V- TotalCredit_V >= 0)
                            {
                                TotalDepitBalance_V = (TotalDepitOpeningBalance_V - TotalCreditOpeningBalance_V + TotalDepit_V - TotalCredit_V);
                                TotalCreditBalance_V = 0;
                            }
                            else
                            {
                                TotalCreditBalance_V = -(TotalDepitOpeningBalance_V - TotalCreditOpeningBalance_V + TotalDepit_V - TotalCredit_V);
                                TotalDepitBalance_V = 0;
                            }




                            if (Zerocheck == 1)
                            {
                                if (TotalCredit_V == 0 && TotalDepit_V == 0 && TotalCreditOpeningBalance_V == 0 && TotalDepitOpeningBalance_V == 0 && TotalDepitBalance_V == 0 && TotalCreditBalance_V == 0)
                                {

                                }
                                else
                                {
                                    lmd.Add(new AccountVM
                                    {
                                        AccountName = (dr["NameAr"]).ToString(),
                                        Code = (dr["AccountCode"]).ToString(),
                                        IsMain = Convert.ToBoolean(dr["nature"]),
                                        TotalCredit = dr["CreditTotal"].ToString() == "" ? 0 : Convert.ToDecimal(dr["CreditTotal"].ToString()),
                                        TotalDepit = dr["DepitTotal"].ToString() == "" ? 0 : Convert.ToDecimal(dr["DepitTotal"].ToString()),
                                        TotalCreditOpeningBalance = dr["OPCredit"].ToString() == "" ? 0 : Convert.ToDecimal(dr["OPCredit"].ToString()),
                                        TotalDepitOpeningBalance = dr["OPDibit"].ToString() == "" ? 0 : Convert.ToDecimal(dr["OPDibit"].ToString()),
                                        TotalCreditBalance =Convert.ToDecimal(TotalCreditBalance_V),
                                        TotalDepitBalance = Convert.ToDecimal(TotalDepitBalance_V)
                                    });

                                }
                            }
                            else
                            {
                                lmd.Add(new AccountVM
                                {
                                    AccountName = (dr["NameAr"]).ToString(),
                                    Code = (dr["AccountCode"]).ToString(),
                                    IsMain = Convert.ToBoolean(dr["nature"]),
                                    TotalCredit = dr["CreditTotal"].ToString() == "" ? 0 : Convert.ToDecimal(dr["CreditTotal"].ToString()),
                                    TotalDepit = dr["DepitTotal"].ToString() == "" ? 0 : Convert.ToDecimal(dr["DepitTotal"].ToString()),
                                    TotalCreditOpeningBalance = dr["OPCredit"].ToString() == "" ? 0 : Convert.ToDecimal(dr["OPCredit"].ToString()),
                                    TotalDepitOpeningBalance = dr["OPDibit"].ToString() == "" ? 0 : Convert.ToDecimal(dr["OPDibit"].ToString()),
                                    TotalCreditBalance = Convert.ToDecimal(TotalCreditBalance_V),
                                    TotalDepitBalance = Convert.ToDecimal(TotalDepitBalance_V)
                                });
                            }

                           
                        }
                    }
                }

                return lmd;
            }
            catch(Exception ex)
            {
                List<AccountVM> lmd = new List<AccountVM>();
                return lmd;
            }

        }
        //dawoudEditArsda
        public async Task<IEnumerable<AccountVM>> GetCustomerFinancialDetails(int? AccountId, int YearId, int BranchId, string lang)
        {
            try
            {

                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.AccountId == AccountId /*&& s.BranchId == BranchId*/).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts2(x.ChildsAccount ?? new List<Accounts>(), x.BranchId ?? 0, YearId, "", "").Result.ToList()
                }).ToList();
                return allAccounts;
            } 
            catch (Exception ex)
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad!=true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts =  GetChildsAccounnts2(x.ChildsAccount ?? new List<Accounts>(), x.BranchId ?? 0, YearId, "", "").Result.ToList()
                }).ToList();
                return allAccounts;
            }
        }
        //aadawoud
        public async Task<IEnumerable<AccountVM>> GetCustomerFinancialDetailsByFilter(int? AccountId, string FromDate, string ToDate, int YearId, int BranchId, string lang, int ZeroCheck)
        {
            try
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false && s.AccountId == AccountId /*&& s.BranchId == BranchId*/).Select(x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    //Transactions =_TaamerProContext.Transactions.ToList().Where(tr => tr.IsDeleted == false && tr.AccountId == x.AccountId).ToList().Select(m => new TransactionsVM
                    //{
                    //    TransactionId = m.TransactionId,
                    //    //LineNumber = m.LineNumber,
                    //    //Depit = m.Depit,
                    //    //Credit = m.Credit,
                    //    //CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                    //    //InvoiceReference = m.InvoiceReference ?? "",
                    //    //Notes = m.Notes ?? "",
                    //    //CurrentBalance = m.CurrentBalance,
                    //    //TransactionDate = m.TransactionDate,
                    //}).ToList(),
                    //(DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))                    //(string.IsNullOrEmpty(FromDate)|| (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate)||(!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false/* && s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && (t.Invoices.Rad != true) && t.YearId == YearId && (string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(ToDate) || (!string.IsNullOrEmpty(t.TransactionDate) && DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))).Sum(t => t.Credit))) : 0,
                    ChildAccounts = GetChildsAccounnts2(x.ChildsAccount??new List<Accounts>(), x.BranchId ?? 0, YearId, FromDate, ToDate).Result.ToList()
                }).ToList();

                if (ZeroCheck == 1)
                {
                    if (allAccounts[0].ChildAccounts.Count > 0)
                    {
                        allAccounts[0].ChildAccounts = allAccounts[0].ChildAccounts.Where(z => z.TotalCredit > 0 || z.TotalDepit > 0 || z.TotalCreditOpeningBalance > 0 || z.TotalDepitOpeningBalance > 0
                        || z.TotalCreditBalance > 0 || z.TotalDepitBalance > 0).ToList();
                    }
                }

                return allAccounts;
                //return Task.FromResult<IEnumerable<AccountVM>>(allAccounts);

            }
            catch (Exception ex)
            {
                var allAccounts = _TaamerProContext.Accounts.ToList().Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/).Select( x => new AccountVM
                {
                    AccountId = x.AccountId,
                    Code = x.Code,
                    AccountName = lang == "ltr" ? x.NameEn : x.NameAr,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Type = x.Type,
                    Nature = x.Nature,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    Classification = x.Classification,
                    Active = x.Active,
                    IsMain = x.IsMain,
                    Notes = x.Notes ?? "",
                    ClassificationName = "",
                    TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                    TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                    TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == x.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==x.AccountId && t.IsDeleted == false && t.IsPost == true && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                    ChildAccounts =  GetChildsAccounnts2(x.ChildsAccount?? new List<Accounts>(), x.BranchId ?? 0, YearId, FromDate, ToDate).Result.ToList()
                }).ToList();

                return allAccounts;
                //return Task.FromResult<IEnumerable<AccountVM>>(allAccounts);
            }
        }
        private async Task<List<AccountVM>> GetChildsAccounnts(IEnumerable<Accounts> accounts, int BranchId, int YearId, string FromDate, string ToDate, int CostCenterId)
        {
            List<AccountVM> accountsVm = new List<AccountVM>();
            foreach (var item in accounts)
            {
                if (item!.ChildsAccount.Count == 0 && item.IsMain == false && item.IsDeleted==false)
                {
                    try
                    {
                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            AccountName = item.NameAr,
                            Code = item.Code,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            Type = item.Type,
                            Nature = item.Nature,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            CurrencyId = item.CurrencyId,
                            Classification = item.Classification,
                            Halala = item.Halala,
                            Active = item.Active,
                            BranchId = item.BranchId,
                            IsMain = item.IsMain,
                            Notes = item.Notes ?? "",
                            ParentAccountCode = item.ParentAccount != null ? item.ParentAccount.Code : "",
                            ParentAccountName = item.ParentAccount != null ? item.ParentAccount.NameEn : "",
                            DepitOrCredit = "",
                            TypeName = "",
                            ClassificationName = "",
                            TotalBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                            TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                            Transactions = item.Transactions.Where(s => s.CostCenterId == CostCenterId || CostCenterId == 0).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                TypeName = m.AccTransactionTypes.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                            }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                            CostCenterTransactions = item.Transactions.Where(s => s.CostCenterId != null).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                TypeName = m.AccTransactionTypes.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                            }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                        });
                    }
                    catch (Exception)
                    {

                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            AccountName = item.NameAr,
                            Code = item.Code,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            Type = item.Type,
                            Nature = item.Nature,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            CurrencyId = item.CurrencyId,
                            Classification = item.Classification,
                            Halala = item.Halala,
                            Active = item.Active,
                            BranchId = item.BranchId,
                            IsMain = item.IsMain,
                            Notes = item.Notes ?? "",
                            ParentAccountCode = item.ParentAccount != null ? item.ParentAccount.Code : "",
                            ParentAccountName = item.ParentAccount != null ? item.ParentAccount.NameEn : "",
                            DepitOrCredit = "",
                            TypeName = "",
                            ClassificationName = "",
                            TotalBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                            TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                            Transactions = item.Transactions.Where(s => s.CostCenterId == CostCenterId || CostCenterId == 0).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                TypeName = m.AccTransactionTypes.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                            }).ToList(),
                            CostCenterTransactions = item.Transactions.Where(s => s.CostCenterId != null).Select(m => new TransactionsVM
                            {
                                TransactionId = m.TransactionId,
                                LineNumber = m.LineNumber,
                                Depit = m.Depit,
                                Credit = m.Credit,
                                CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                TypeName = m.AccTransactionTypes.NameAr ?? "",
                                InvoiceReference = m.InvoiceReference ?? "",
                                Notes = m.Notes ?? "",
                                CurrentBalance = m.CurrentBalance,
                                TransactionDate = m.TransactionDate,
                            }).ToList()
                        });
                    }

                }
                else
                {
                    if (item.IsDeleted==false)
                    {
                        try
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                AccountName = item.NameAr,
                                Code = item.Code,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                Type = item.Type,
                                Nature = item.Nature,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                Classification = item.Classification,
                                Active = item.Active,
                                IsMain = item.IsMain,
                                Notes = item.Notes ?? "",
                                ClassificationName = "",
                                TotalBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                                TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                ChildAccounts =await GetChildsAccounnts(item.ChildsAccount, BranchId, YearId, FromDate, ToDate, CostCenterId),
                                Transactions = item.Transactions.Where(s => s.CostCenterId == CostCenterId || CostCenterId == 0).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                    TypeName = m.AccTransactionTypes.NameAr,
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                                CostCenterTransactions = item.Transactions.Where(s => s.CostCenterId != null).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                    TypeName = m.AccTransactionTypes.NameAr,
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                }).ToList().Where(s => (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                                    (DateTime.ParseExact(s.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
                            });
                        }
                        catch (Exception)
                        {

                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                AccountName = item.NameAr,
                                Code = item.Code,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                Type = item.Type,
                                Nature = item.Nature,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                Classification = item.Classification,
                                Active = item.Active,
                                IsMain = item.IsMain,
                                Notes = item.Notes ?? "",
                                ClassificationName = "",
                                TotalBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)),
                                TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && (t.Type == 10 || t.Type == 19) && t.YearId == YearId).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                ChildAccounts =await GetChildsAccounnts(item.ChildsAccount, BranchId, YearId, FromDate, ToDate, CostCenterId),
                                Transactions = item.Transactions.Where(s => s.CostCenterId == CostCenterId || CostCenterId == 0).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                    TypeName = m.AccTransactionTypes!.NameAr??"",
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                }).ToList(),
                                CostCenterTransactions = item.Transactions.Where(s => s.CostCenterId != null).Select(m => new TransactionsVM
                                {
                                    TransactionId = m.TransactionId,
                                    LineNumber = m.LineNumber,
                                    Depit = m.Depit,
                                    Credit = m.Credit,
                                    CostCenterName = m.CostCenters != null ? m.CostCenters.NameAr : "",
                                    TypeName = m.AccTransactionTypes!.NameAr??"",
                                    InvoiceReference = m.InvoiceReference ?? "",
                                    Notes = m.Notes ?? "",
                                    CurrentBalance = m.CurrentBalance,
                                    TransactionDate = m.TransactionDate,
                                }).ToList()
                            });
                        }

                    }


                }
            }
            return accountsVm.ToList();
        }
        private async Task<List<AccountVM>> GetChildsAccounnts2(IEnumerable<Accounts> accounts, int BranchId, int YearId, string FromDate, string ToDate)
        {

            //if(FromDate=="")
            //{
            //    FromDate = "2010-01-01";
            //}
            //if (ToDate == "")
            //{
            //    ToDate = "2040-01-01";
            //}

            List<AccountVM> accountsVm = new List<AccountVM>();
            foreach (var item in accounts)
            {
                if (item!.ChildsAccount != null && item.IsMain == false && item.IsDeleted==false)
                {
                    try
                    {
                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            AccountName = item.NameAr,
                            Code = item.Code,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            Type = item.Type,
                            Nature = item.Nature,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            Classification = item.Classification,
                            Active = item.Active,
                            IsMain = item.IsMain,
                            Notes = item.Notes ?? "",
                            ClassificationName = "",
                            TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false/* && s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                            TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                        });
                    }
                    catch (Exception ex)
                    {

                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            AccountName = item.NameAr,
                            Code = item.Code,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            Type = item.Type,
                            Nature = item.Nature,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            CurrencyId = item.CurrencyId,
                            Classification = item.Classification,
                            Halala = item.Halala,
                            Active = item.Active,
                            BranchId = item.BranchId,
                            IsMain = item.IsMain,
                            Notes = item.Notes ?? "",
                            ParentAccountCode = item.ParentAccount != null ? item.ParentAccount.Code : "",
                            ParentAccountName = item.ParentAccount != null ? item.ParentAccount.NameEn : "",
                            DepitOrCredit = "",
                            TypeName = "",
                            ClassificationName = "",
                            TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                            TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId */&& s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,

                        });
                    }
                }
                else
                {
                    if (item.IsDeleted == false)
                    {
                        try
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                AccountName = item.NameAr,
                                Code = item.Code,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                Type = item.Type,
                                Nature = item.Nature,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                Classification = item.Classification,
                                Active = item.Active,
                                IsMain = item.IsMain,
                                Notes = item.Notes ?? "",
                                ClassificationName = "",
                                TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                                TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                ChildAccounts = await GetChildsAccounnts2(item!.ChildsAccount??new List<Accounts>(), BranchId, YearId, FromDate, ToDate),
                            });
                        }
                        catch (Exception ex)
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                AccountName = item.NameAr,
                                Code = item.Code,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                Type = item.Type,
                                Nature = item.Nature,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                Classification = item.Classification,
                                Active = item.Active,
                                IsMain = item.IsMain,
                                Notes = item.Notes ?? "",
                                ClassificationName = "",
                                TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                                TotalCreditOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalDepitOpeningBalance = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && (t.Type == 10 || t.Type == 26) && t.YearId == YearId).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false/* && s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.Type != 12 && ( t.Invoices.Rad != true) && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                ChildAccounts = await GetChildsAccounnts2(item!.ChildsAccount ?? new List<Accounts>(), BranchId, YearId, FromDate, ToDate),
                            });
                        }

                    }

                }
            }
            return accountsVm;
        }
        private async Task<List<AccountVM>> GetChildsAccounnts3(IEnumerable<Accounts> accounts, int BranchId, int YearId, int Type, string FromDate, string ToDate)
        {
            List<AccountVM> accountsVm = new List<AccountVM>();
            foreach (var item in accounts)
            {
                if (item!.ChildsAccount != null && item.IsMain == false && item.IsDeleted==false)
                {
                    try
                    {
                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            AccountName = item.NameAr,
                            Code = item.Code,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            Type = item.Type,
                            Nature = item.Nature,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            CurrencyId = item.CurrencyId,
                            Classification = item.Classification,
                            Halala = item.Halala,
                            Active = item.Active,
                            BranchId = item.BranchId,
                            IsMain = item.IsMain,
                            Notes = item.Notes ?? "",
                            ParentAccountCode = item.ParentAccount != null ? item.ParentAccount.Code : "",
                            ParentAccountName = item.ParentAccount != null ? item.ParentAccount.NameEn : "",
                            DepitOrCredit = "",
                            TypeName = "",
                            ClassificationName = "",
                            TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                        });
                    }
                    catch (Exception)
                    {
                        accountsVm.Add(new AccountVM
                        {
                            AccountId = item.AccountId,
                            AccountName = item.NameAr,
                            Code = item.Code,
                            NameAr = item.NameAr,
                            NameEn = item.NameEn,
                            Type = item.Type,
                            Nature = item.Nature,
                            ParentId = item.ParentId,
                            Level = item.Level,
                            CurrencyId = item.CurrencyId,
                            Classification = item.Classification,
                            Halala = item.Halala,
                            Active = item.Active,
                            BranchId = item.BranchId,
                            IsMain = item.IsMain,
                            Notes = item.Notes ?? "",
                            ParentAccountCode = item.ParentAccount != null ? item.ParentAccount.Code : "",
                            ParentAccountName = item.ParentAccount != null ? item.ParentAccount.NameEn : "",
                            DepitOrCredit = "",
                            TypeName = "",
                            ClassificationName = "",
                            TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit)),
                            TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)),
                            TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                            TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                        });
                    }
                }
                else
                {
                    if (item.IsDeleted == false)
                    {
                        try
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                AccountName = item.NameAr,
                                Code = item.Code,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                Type = item.Type,
                                Nature = item.Nature,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                CurrencyId = item.CurrencyId,
                                Classification = item.Classification,
                                Halala = item.Halala,
                                Active = item.Active,
                                BranchId = item.BranchId,
                                IsMain = item.IsMain,
                                Notes = item.Notes ?? "",
                                ParentAccountCode = item.ParentAccount != null ? item.ParentAccount.Code : "",
                                ParentAccountName = item.ParentAccount != null ? item.ParentAccount.NameEn : "",
                                DepitOrCredit = "",
                                TypeName = "",
                                ClassificationName = "",
                                TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit))) : 0,
                                ChildAccounts = await GetChildsAccounnts3(item.ChildsAccount.Where(c => c.Type == Type), BranchId, YearId, Type, FromDate, ToDate),
                            });
                        }
                        catch (Exception)
                        {
                            accountsVm.Add(new AccountVM
                            {
                                AccountId = item.AccountId,
                                AccountName = item.NameAr,
                                Code = item.Code,
                                NameAr = item.NameAr,
                                NameEn = item.NameEn,
                                Type = item.Type,
                                Nature = item.Nature,
                                ParentId = item.ParentId,
                                Level = item.Level,
                                Classification = item.Classification,
                                Active = item.Active,
                                IsMain = item.IsMain,
                                Notes = item.Notes ?? "",
                                ClassificationName = "",
                                TotalCredit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit)),
                                TotalDepit = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)),
                                TotalCreditBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) < 0 ? -(_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                TotalDepitBalance = (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) > 0 ? (_TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Depit)) - _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == Type && s.AccountId == item.AccountId).Flatten(i => i.ChildsAccount.Where(c => c.Type == Type)).Sum(s => _TaamerProContext.Transactions.Where(t => t.AccountId==item.AccountId && t.IsDeleted == false && t.IsPost == true && t.AccountType == Type && t.YearId == YearId).Sum(t => t.Credit))) : 0,
                                ChildAccounts = await GetChildsAccounnts3(item.ChildsAccount.Where(c => c.Type == Type), BranchId, YearId, Type, FromDate, ToDate),
                            });
                        }

                    }


                }
            }
            return accountsVm;
        }
        #endregion




        public async Task<IEnumerable<AccountVM>> GetAccountsByType(string accountName, string lang)
        {
            var ParentId = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.NameAr == accountName).Select(x => x.AccountId).FirstOrDefault();

            var result = _TaamerProContext.Accounts.Where(s => s.IsDeleted == false && s.ParentId == ParentId).Select(x => new AccountVM
            {
                AccountId = x.AccountId,
                AccountName = lang == "ltr" ? x.NameEn : x.NameAr
            }).ToList();

            return result;
        }

        public IEnumerable<Accounts> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Accounts> GetMatching(Func<Accounts, bool> where)
        {
            return _TaamerProContext.Accounts.Where(where).ToList<Accounts>();
        }
    }
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> items,
            Func<T, IEnumerable<T>> getChildren)
        {
            if (items == null)
                yield break;

            var stack = new Stack<T>(items);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                if (current == null) continue;

                var children = getChildren(current);
                if (children == null) continue;

                foreach (var child in children)
                    stack.Push(child);
            }
        }
    }


}


