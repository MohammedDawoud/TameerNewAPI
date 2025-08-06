using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IServicesPriceOfferService 
    {
        GeneralMessage SaveService(Acc_Services_PriceOffer servicevou, int UserId, int BranchId);
        GeneralMessage DeleteService(int? ServicesIdVou, int UserId, int BranchId);
        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentId(int? ParentId, int? OfferId);
        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentIdAndContractId(int? ParentId, int? ContractId);
        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceVouByParentIdAndInvoiceId(int? ParentId, int? InvoiceId);

        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByOfferId(int? OfferId);

    }
}
