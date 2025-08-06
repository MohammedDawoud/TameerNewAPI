using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IServicesPriceServiceOfferRepository
    {
        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentId(int? ParentId, int? OfferId);
        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentIdAndContractId(int? ParentId, int? ContractId);
        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceVouByParentIdAndInvoiceId(int? ParentId, int? InvoiceId);

        Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByOfferId(int? OfferId);

    }
}
