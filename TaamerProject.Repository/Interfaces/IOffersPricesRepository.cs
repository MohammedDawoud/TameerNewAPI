using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IOffersPricesRepository : IRepository<OffersPrices>
    {

        Task<IEnumerable<OffersPricesVM>> GetAllOffers(int BranchId);
        Task<int> GenerateNextOfferNumber(int BranchId, string codePrefix);
        Task<IEnumerable<OffersPricesVM>> GetOfferByid(int offerid);
        Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerData(int offerid, string NationalId, int ActivationCode);

        Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerId(int Customerid);
        Task<IEnumerable<OffersPricesVM>> GetAllOffersByProjectId(int ProjectId);

        Task<IEnumerable<OffersPricesVM>> GetOfferPrice_Search(string offerno, string date, string customername, string presenter, decimal? Amount,int BranchId);
        Task<IEnumerable<OffersPricesVM>> Getofferconestintroduction(int BranchId);
        Task<IEnumerable<OffersPricesVM>> GetOfferByid2(int offerid);
    }
}
