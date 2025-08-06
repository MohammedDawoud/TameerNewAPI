using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAcc_InvoicesRequestsRepository
    {
        Task<Acc_InvoicesRequestsVM> GetInvoiceReqByReqId(int InvoiceReqId);
        Task<Acc_InvoicesRequestsVM> GetInvoiceReq(int InvoiceId);
        Task<IEnumerable<Acc_InvoicesRequestsVM>> GetAllInvoiceRequests(int BranchId);
        Task<IEnumerable<Acc_InvoicesRequestsVM>> GetAllInvoiceRequests(int InvoiceId, int BranchId);

    }
}
