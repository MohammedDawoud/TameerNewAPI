using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Accounts : Auditable
    {
        public int AccountId { get; set; }
        public string? Code { get; set; }
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
        public int? TransferedAccId { get; set; }
        public bool? IsMainCustAcc { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OpeningBalance { get; set; }
        public int? ExpensesAccId { get; set; }
        public int? AccountIdAhlak { get; set; }

        public decimal? OpenAccCredit { get; set; }
        public decimal? OpenAccDepit { get; set; }

        public string? AccountCodeNew { get; set; }

        public int? PublicRev { get; set; }
        public int? OtherRev { get; set; }
        public string? OpenAccCreditDate { get; set; }
        public string? OpenAccDepitDate { get; set; }


        public virtual Accounts? ParentAccount { get; set; }

        public virtual List<Transactions>? Transactions { get; set; }
        public virtual List<Accounts>? ChildsAccount { get; set; }
    }
}
