using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class AccountVM
    {
        public int AccountId { get; set; }
        public string? Code { get; set; }
        public string? AccountName { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? Type { get; set; }
        public int? Nature { get; set; }
        public int? ParentId { get; set; }
        public int? Level { get; set; }
        public int? CurrencyId { get; set; }
        public int? Classification { get; set; }
        public bool? Halala { get; set; }
        public bool? Active { get; set; }
        public int? BranchId { get; set; }
        public bool? IsMain { get; set; }
        public string? Notes { get; set; }
        public string? ParentAccountCode { get; set; }
        public string?  ParentAccountName { get; set; }
        public string? DepitOrCredit { get; set; }
        public string? TypeName { get; set; }
        public string? ClassificationName { get; set; }

        public decimal? TotalCredit { get; set; }
        public decimal? TotalDepit { get; set; }
        public decimal? TotalCreditOpeningBalance { get; set; }
        public decimal? TotalDepitOpeningBalance { get; set; }
        public decimal? TotalCreditBalance { get; set; }
        public decimal? TotalDepitBalance { get; set; }
        public decimal? TotalBalance { get; set; }
        public int? ExpensesAccId { get; set; }
        public int? AccountIdAhlak { get; set; }


        public decimal? OpenAccCredit { get; set; }
        public decimal? OpenAccDepit { get; set; }
        public string? AccountCodeNew { get; set; }

        public int? PublicRev { get; set; }
        public int? OtherRev { get; set; }
        public string? OpenAccCreditDate { get; set; }
        public string? OpenAccDepitDate { get; set; }

        public List<AccountVM>? ChildAccounts { get; set; }
        public List<TransactionsVM>? Transactions { get; set; }

        public List<TransactionsVM>? CostCenterTransactions { get; set; }
    }
}
