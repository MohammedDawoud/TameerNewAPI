
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
    public class ServicesPriceServiceRepository :  IServicesPriceServiceRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ServicesPriceServiceRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<AccServicesPricesVM>> GetAllServicesPrice()
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && !x.ParentId.HasValue).Select(x => new AccServicesPricesVM
            {
                ServicesId = x.ServicesId,
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                //Amount = x.Amount,
                Amount = x.PackageId == null ? x.Amount:0,
                Name = x.ServicesName,
                ServicesName = x.ServicesName,
                ProjectId = x.ProjectId,
                ProjectSubTypeID=x.ProjectSubTypeID,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr==null?" ": x.ProjectSubTypes.NameAr,
                ProjectName = x.ProjectParentId.NameAr,
                ParentId = x.ParentId,
                CostCenterId = x.CostCenterId,
                PackageId=x.PackageId,
                AmountAndPackage=x.PackageId==null?x.Amount.ToString():x.Package.PackageName,

                PackageName = x.PackageId != null ? x.Package.PackageName : "",
                MeterPrice1 = x.PackageId != null ? x.Package.MeterPrice1 : 0,
                MeterPrice2 = x.PackageId != null ? x.Package.MeterPrice2 : 0,
                MeterPrice3 = x.PackageId != null ? x.Package.MeterPrice3 : 0,
                PackageRatio1 = x.PackageId != null ? x.Package.PackageRatio1 : 0,
                PackageRatio2 = x.PackageId != null ? x.Package.PackageRatio2 : 0,
                PackageRatio3 = x.PackageId != null ? x.Package.PackageRatio3 : 0,
                ServiceName_EN=x.ServiceName_EN??"",
                ServiceType = x.ServiceType ?? 1,
                ServiceTypeName = x.ServiceType==2 ? "تقرير":"خدمة",

            }).OrderByDescending(x => x.ServicesId).ToList();

            return result;
        }
        public async Task<AccServicesPricesVM> GetServicesPriceByServiceId(int ServiceId)
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && !x.ParentId.HasValue && x.ServicesId == ServiceId).Select(x => new AccServicesPricesVM
            {
                ServicesId = x.ServicesId,
                AccountId = x.AccountId,
                AccountName = x.AccountName,
                //Amount = x.Amount,
                Amount = x.PackageId == null ? x.Amount : 0,
                Name = x.ServicesName,
                ServicesName = x.ServicesName,
                ProjectId = x.ProjectId,
                ProjectSubTypeID = x.ProjectSubTypeID,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr == null ? " " : x.ProjectSubTypes.NameAr,
                ProjectName = x.ProjectParentId.NameAr,
                ParentId = x.ParentId,
                CostCenterId = x.CostCenterId,
                PackageId = x.PackageId,
                AmountAndPackage = x.PackageId == null ? x.Amount.ToString() : x.Package.PackageName,

                PackageName = x.PackageId != null ? x.Package.PackageName : "",
                MeterPrice1 = x.PackageId != null ? x.Package.MeterPrice1 : 0,
                MeterPrice2 = x.PackageId != null ? x.Package.MeterPrice2 : 0,
                MeterPrice3 = x.PackageId != null ? x.Package.MeterPrice3 : 0,
                PackageRatio1 = x.PackageId != null ? x.Package.PackageRatio1 : 0,
                PackageRatio2 = x.PackageId != null ? x.Package.PackageRatio2 : 0,
                PackageRatio3 = x.PackageId != null ? x.Package.PackageRatio3 : 0,
                ServiceName_EN = x.ServiceName_EN ?? "",
                ServiceType = x.ServiceType ?? 1,
                ServiceTypeName = x.ServiceType == 2 ? "تقرير" : "خدمة",

            }).FirstOrDefault();

            return result;
        }

        public async Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId(int? projectId)
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false&& x.ProjectId == projectId && !x.ParentId.HasValue).Select(x => new AccServicesPricesVM
            {
                Id=x.ServicesId,
                Name = x.ServicesName,
                ServicesName = x.ServicesName,
                ServicesId = x.ServicesId,
                //Amount = x.Amount,
                Amount = x.PackageId == null ? x.Amount : 0,
                ProjectSubTypeID = x.ProjectSubTypeID,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr == null ? " " : x.ProjectSubTypes.NameAr,
                ProjectName = x.ProjectParentId.NameAr,
                ParentId = x.ParentId,
                CostCenterId = x.CostCenterId,
                PackageId = x.PackageId,
                AmountAndPackage = x.PackageId == null ? x.Amount.ToString() : x.Package.PackageName,

                PackageName = x.PackageId != null ? x.Package.PackageName : "",
                MeterPrice1 = x.PackageId != null ? x.Package.MeterPrice1 : 0,
                MeterPrice2 = x.PackageId != null ? x.Package.MeterPrice2 : 0,
                MeterPrice3 = x.PackageId != null ? x.Package.MeterPrice3 : 0,
                PackageRatio1 = x.PackageId != null ? x.Package.PackageRatio1 : 0,
                PackageRatio2 = x.PackageId != null ? x.Package.PackageRatio2 : 0,
                PackageRatio3 = x.PackageId != null ? x.Package.PackageRatio3 : 0,
                ServiceName_EN = x.ServiceName_EN ?? "",
                ServiceType = x.ServiceType ?? 1,
                ServiceTypeName = x.ServiceType == 2 ? "تقرير" : "خدمة",

            }).OrderByDescending(x => x.ServicesId).ToList();

            return result;
        }

        public async Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId2(int? projectId, int? projectId2)
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && x.ProjectId == projectId && x.ProjectSubTypeID == projectId2 && !x.ParentId.HasValue).Select(x => new AccServicesPricesVM
            {
                Id = x.ServicesId,
                Name = x.ServicesName,
                ServicesName = x.ServicesName,
                ServicesId = x.ServicesId,
                //Amount = x.Amount,
                Amount = x.PackageId == null ? x.Amount : 0,
                ProjectSubTypeID = x.ProjectSubTypeID,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr == null ? " " : x.ProjectSubTypes.NameAr,
                ProjectName = x.ProjectParentId.NameAr,
                ParentId = x.ParentId,
                CostCenterId = x.CostCenterId,
                PackageId = x.PackageId,
                AmountAndPackage = x.PackageId == null ? x.Amount.ToString() : x.Package.PackageName,

                PackageName = x.PackageId != null ? x.Package.PackageName : "",
                MeterPrice1 = x.PackageId != null ? x.Package.MeterPrice1 : 0,
                MeterPrice2 = x.PackageId != null ? x.Package.MeterPrice2 : 0,
                MeterPrice3 = x.PackageId != null ? x.Package.MeterPrice3 : 0,
                PackageRatio1 = x.PackageId != null ? x.Package.PackageRatio1 : 0,
                PackageRatio2 = x.PackageId != null ? x.Package.PackageRatio2 : 0,
                PackageRatio3 = x.PackageId != null ? x.Package.PackageRatio3 : 0,
                ServiceType = x.ServiceType ?? 1,
                ServiceTypeName = x.ServiceType == 2 ? "تقرير" : "خدمة",

            }).OrderByDescending(x => x.ServicesId).ToList();

            return result;
        }

        public async Task<IEnumerable<AccServicesPricesVM>> GetServicePriceByProject_Search(int? Project1, int? Project2, string ServiceName, string ServiceDetailName, decimal? Amount)
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && 
            ((Project1.HasValue && x.ProjectId == Project1) || (!Project1.HasValue && true)) && 
            ( (Project2.HasValue && x.ProjectSubTypeID == Project2) || (!Project2.HasValue && true)) && 
            ( (!string.IsNullOrEmpty(ServiceName) && x.ServicesName.Contains(ServiceName)) || (string.IsNullOrEmpty(ServiceName) && true) ) &&
            ( (Amount.HasValue && Amount.Value == x.Amount) || (!Amount.HasValue && true) ) &&
            !x.ParentId.HasValue).Select(x => new AccServicesPricesVM
            {
                Id = x.ServicesId,
                Name = x.ServicesName,
                ServicesName = x.ServicesName,
                ServicesId = x.ServicesId,
                //Amount = x.Amount,
                Amount = x.PackageId == null ? x.Amount : 0,
                ProjectId = x.ProjectId,
                AccountId = x.AccountId,
                ProjectSubTypeID = x.ProjectSubTypeID,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr == null ? " " : x.ProjectSubTypes.NameAr,
                ProjectName = x.ProjectParentId.NameAr,
                ParentId = x.ParentId,
                CostCenterId = x.CostCenterId,
                PackageId = x.PackageId,
                AmountAndPackage = x.PackageId == null ? x.Amount.ToString() : x.Package.PackageName,

                PackageName = x.PackageId != null ? x.Package.PackageName : "",
                MeterPrice1 = x.PackageId != null ? x.Package.MeterPrice1 : 0,
                MeterPrice2 = x.PackageId != null ? x.Package.MeterPrice2 : 0,
                MeterPrice3 = x.PackageId != null ? x.Package.MeterPrice3 : 0,
                PackageRatio1 = x.PackageId != null ? x.Package.PackageRatio1 : 0,
                PackageRatio2 = x.PackageId != null ? x.Package.PackageRatio2 : 0,
                PackageRatio3 = x.PackageId != null ? x.Package.PackageRatio3 : 0,
                ServiceName_EN = x.ServiceName_EN ?? "",
                ServiceType = x.ServiceType ?? 1,
                ServiceTypeName = x.ServiceType == 2 ? "تقرير" : "خدمة",

            }).OrderByDescending(x => x.ServicesId).ToList();

            return result;
        }

        public async Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByParentId(int? ParentId)
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && x.ParentId == ParentId).Select(x => new AccServicesPricesVM
            {
                Id = x.ServicesId,
                Name = x.ServicesName,
                ServicesName = x.ServicesName,
                AccountName = x.AccountName,
                ServicesId = x.ServicesId,
                //Amount = x.Amount,
                Amount = x.PackageId == null ? x.Amount : 0,
                ProjectSubTypeID = x.ProjectSubTypeID,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr == null ? " " : x.ProjectSubTypes.NameAr,
                ProjectName = x.ProjectParentId.NameAr,
                ParentId = x.ParentId,
                CostCenterId = x.CostCenterId,
                PackageId = x.PackageId,
                AmountAndPackage = x.PackageId == null ? x.Amount.ToString() : x.Package.PackageName,

                PackageName = x.PackageId != null ? x.Package.PackageName : "",
                MeterPrice1 = x.PackageId != null ? x.Package.MeterPrice1 : 0,
                MeterPrice2 = x.PackageId != null ? x.Package.MeterPrice2 : 0,
                MeterPrice3 = x.PackageId != null ? x.Package.MeterPrice3 : 0,
                PackageRatio1 = x.PackageId != null ? x.Package.PackageRatio1 : 0,
                PackageRatio2 = x.PackageId != null ? x.Package.PackageRatio2 : 0,
                PackageRatio3 = x.PackageId != null ? x.Package.PackageRatio3 : 0,
                ServiceName_EN = x.ServiceName_EN ?? "",
                ServiceType = x.ServiceType ?? 1,
                ServiceTypeName = x.ServiceType == 2 ? "تقرير" : "خدمة",

            }).OrderByDescending(x => x.ServicesId).ToList();

            return result;
        }

        public async Task<decimal?> GetServicesPriceAmountByServicesId(int? ServicesId)
        {
            var result = _TaamerProContext.Acc_Services_Price.Where(x => x.IsDeleted == false && x.ServicesId == ServicesId ).Select(x => x.Amount).FirstOrDefault();

            return result;
        }
    }
}
