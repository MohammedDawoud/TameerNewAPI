
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ServicesPriceServiceOfferRepository : IServicesPriceServiceOfferRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ServicesPriceServiceOfferRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentId(int? ParentId, int? OfferId)
        {
            var result = _TaamerProContext.Acc_Services_PriceOffer.Where(x => x.IsDeleted == false && x.ParentId == ParentId && x.OfferId== OfferId).Select(x => new AccServicesPricesOfferVM
            {
                Id = x.ServicesIdVou,
                Name = x.ServiceDetails!=null?x.ServiceDetails.ServicesName??"":"",
                ServicesName = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesNameEn = x.ServiceDetails != null ? x.ServiceDetails.AccountName ?? x.ServiceDetails.ServicesName : "",
                ServicesIdVou = x.ServicesIdVou,
                OfferId = x.OfferId,
                ServicesId = x.ServicesId,
                ParentId = x.ParentId,
                SureService = x.SureService,
                ContractId=x.ContractId,
                LineNumber=x.LineNumber,
                InvoiceId = x.InvoiceId,
            }).OrderByDescending(x => x.ServicesIdVou).ToList();

            return result;
        }
        public async Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByParentIdAndContractId(int? ParentId, int? ContractId)
        {
            var result = _TaamerProContext.Acc_Services_PriceOffer.Where(x => x.IsDeleted == false && x.ParentId == ParentId && x.ContractId == ContractId).Select(x => new AccServicesPricesOfferVM
            {
                Id = x.ServicesIdVou,
                Name = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesName = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesIdVou = x.ServicesIdVou,
                OfferId = x.OfferId,
                ServicesId = x.ServicesId,
                ParentId = x.ParentId,
                SureService = x.SureService,
                ContractId = x.ContractId,
                LineNumber = x.LineNumber,
                InvoiceId = x.InvoiceId,
            }).OrderByDescending(x => x.ServicesIdVou).ToList();

            return result;
        }
        public async Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceVouByParentIdAndInvoiceId(int? ParentId, int? InvoiceId)
        {
            var result = _TaamerProContext.Acc_Services_PriceOffer.Where(x => x.IsDeleted == false && x.ParentId == ParentId && x.InvoiceId == InvoiceId).Select(x => new AccServicesPricesOfferVM
            {
                Id = x.ServicesIdVou,
                Name = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesName = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesNameEn = x.ServiceDetails != null ? x.ServiceDetails.ServiceName_EN ?? x.ServiceDetails.ServicesName : "",
                ServicesIdVou = x.ServicesIdVou,
                OfferId = x.OfferId,
                ServicesId = x.ServicesId,
                ParentId = x.ParentId,
                SureService = x.SureService,
                ContractId = x.ContractId,
                LineNumber = x.LineNumber,
                InvoiceId = x.InvoiceId,
            }).OrderByDescending(x => x.ServicesIdVou).ToList();

            return result;
        }

        public async Task<IEnumerable<AccServicesPricesOfferVM>> GetServicesPriceByOfferId(int? OfferId)
        {
            var result = _TaamerProContext.Acc_Services_PriceOffer.Where(x => x.IsDeleted == false && x.OfferId == OfferId).Select(x => new AccServicesPricesOfferVM
            {
                Id = x.ServicesIdVou,
                Name = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesName = x.ServiceDetails != null ? x.ServiceDetails.ServicesName ?? "" : "",
                ServicesNameEn = x.ServiceDetails != null ? x.ServiceDetails.ServiceName_EN ?? x.ServiceDetails.ServicesName : "",
                ServicesIdVou = x.ServicesIdVou,
                OfferId = x.OfferId,
                ServicesId = x.ServicesId,
                ParentId = x.ParentId,
                SureService = x.SureService,
                ContractId = x.ContractId,
                LineNumber = x.LineNumber,
                InvoiceId = x.InvoiceId,
            }).OrderByDescending(x => x.ServicesIdVou).ToList();

            return result;
        }

    }
}
