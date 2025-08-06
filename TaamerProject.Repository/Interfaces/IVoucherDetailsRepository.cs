using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IVoucherDetailsRepository
    {
        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByVoucherId(int? voucherId);
        Task<VoucherDetailsVM> GetInvoiceIDByProjectID(int? ProjectId);

        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceId(int? voucherId);
        Task<VoucherDetailsVM> GetAllDetailsByVoucherDetailsId(int? VoucherDetailsId);

        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceIdFirstOrDef(int? voucherId);

        Task<IEnumerable<VoucherDetailsVM>> GetAllDetailsByInvoiceIdPurchase(int? voucherId);


        Task<IEnumerable<VoucherDetailsVM>> GetAllTransByLineNo(int LineNo);
        Task<IEnumerable<VoucherDetailsVM>> GetAllTrans(int VouDetailsID);
    }
}
