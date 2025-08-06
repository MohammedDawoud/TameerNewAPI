using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
   public class DetailedRevenuVM
    {
        public string? CustomerName  { get; set; }
        public string? Date { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? AccountName { get; set; }
        public string? Notes { get; set; }
        public string? Project { get; set; }
        public string? ProjectType { get; set; }
        public string? PayTypeName { get; set; }
        public string? TotalValue { get; set; }
        public string? TotalValueDepit { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? Taxes { get; set; } 
        public string? Total { get; set; } 
        public string? diff { get; set; }
        public string? TransactionId { get; set; }

        public string? Rad { get; set; }
        public string? Type { get; set; }
        public string? Total_TotalValue { get; set; }
        public string? Total_Taxes { get; set; }
        public string? Total_Total { get; set; }
        public string? CustomerName_W { get; set; }


    }


    public class ClosingVouchers
    {
        public int AccountId { get; set; }
        public int LineNumber { get; set; }

        public string? AccountName { get; set; }
        public string? CreditDepit { get; set; }
        public string? CostCenterName { get; set; }
        public int CostCenterId { get; set; }
        public string? Notes { get; set; }
        public string? InvoiceReference { get; set; }

        public string? CreditOrDepitType { get; set; }
        public string? TotalCredit { get; set; }

        public string? TotalDepit { get; set; }
        public int? BranchId { get; set; }

    }

    public class CostCenterEX_REVM
    {
        public int CostCenterId { get; set; }
        public string? ExDepit { get; set; }
        public string? ReCredit { get; set; }
        public string? EX_RE_Diff { get; set; }

        public string? CostCenterCode { get; set; }
        public string? CostCenterName { get; set; }
        public string? TotalExDepit { get; set; }
        public string? TotalReCredit { get; set; }
        public string? TotalEX_RE_Diff { get; set; }

        public string? Flag { get; set; }

    }

    public class InvoicedueC
    {
        public int InvoiceId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerNameW { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? InvoiceDate { get; set; }
        public string? BranchName { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal? ValueCollect { get; set; }
        public decimal? RetinvoiceValue { get; set; }
        public string? AccDate { get; set; }

        public int? DaysV { get; set; }
        public int? BranchId { get; set; }
        public decimal? VueValue { get; set; }


    }
}
