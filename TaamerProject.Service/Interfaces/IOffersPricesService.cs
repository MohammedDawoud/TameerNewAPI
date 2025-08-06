using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOffersPricesService
    {
        GeneralMessage SaveOffer(OffersPrices offerprice, int UserId, int BranchId, int? yearid);
        Task<IEnumerable<OffersPricesVM>> GetAllOffers(int BranchId);
        GeneralMessage Intoduceoffer(int offerpriceid, int UserId, int BranchId, string Url, string Link);
        GeneralMessage IntoduceofferManual(int offerpriceid, int UserId, int BranchId, string Url, string Link);

        GeneralMessage Customeraccept(int offerpriceid, int UserId, int BranchId);
        Task<string> GenerateNextOfferNumber(int BranchId);
        Task<IEnumerable<OffersPricesVM>> GetOfferByid(int offerid);
        Task<string> GetOfferCode_S(int BranchId);
        GeneralMessage CertifyOffer(int offerpriceid, int UserId, int BranchId, string Url);
        GeneralMessage ConfirmCertifyOffer(int offerpriceid, int UserId, int BranchId, string Code);
        Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerData(int offerid, string NationalId, int ActivationCode);

        IEnumerable<OffersPricesVM> GetOfferByCustomerId(int CustomerId);
        IEnumerable<OffersPricesVM> GetOfferByCustomerIdProject(int CustomerId, int ProjectId);

        Task<IEnumerable<OffersPricesVM>> GetAllOffersByProjectId(int ProjectId);

        Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerIdOld(int CustomerId);

        Task<IEnumerable<OffersPricesVM>> GetOfferPrice_Search(string offerno, string Date, string customername, string presenter, decimal? Amount, int BranchId);
        GeneralMessage DeleteOffer(int offerpriceid, int UserId, int BranchId);
        IEnumerable<object> Fillcustomerhavingoffer(int BranchId);
        IEnumerable<OffersPricesVM> GetOfferByCustomerId2(int CustomerId, int offerpriceid);
        Task<IEnumerable<OffersPricesVM>> GetAllOfferByCustomerId(int CustomerId);
        Task<IEnumerable<OffersPricesVM>> Getofferconestintroduction(int BranchId);
        Task<IEnumerable<OffersPricesVM>> GetOfferByid2(int offerid);
    }
}
