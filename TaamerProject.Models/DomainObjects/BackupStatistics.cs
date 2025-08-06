using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class BackupStatistics
    {
        public ProjectVM LastProject { get; set; }
        public InvoicesVM LastInvoice { get; set; }
        public InvoicesVM LastInvoiceRet { get; set; }
        public InvoicesVM lastRevoucern { get; set; }
        public InvoicesVM lastpayvoucern { get; set; }
        public InvoicesVM lastEntyvoucher { get; set; }
        public EmpContractVM LastEmpContract { get; set; }
        public CustomerVM LastCustomer { get; set; }
        public int? ProjectCount { get; set; }
        public int? ProjectArchivedCount { get; set; }
        public int? Customercount { get; set; }
        public string? TotalDetailedRevenu { get; set; }
        public string? TotalDetailedExpensed { get; set; }
        public int? BranchesCount { get; set; }
        public int? UsersCount { get; set; }

    }
}
