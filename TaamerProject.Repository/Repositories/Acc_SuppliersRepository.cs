using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;

namespace TaamerProject.Repository.Repositories
{
    public class Acc_SuppliersRepository : IAcc_SuppliersRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_SuppliersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public IEnumerable<Acc_Suppliers> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Acc_SuppliersVM>> GetAllSuppliers(string SearchText, int BranchId, int? YearId)
        {
            if (SearchText == "")
            {
                var Suppliers = _TaamerProContext.Acc_Suppliers.Where(s => s.IsDeleted == false).Select(x => new Acc_SuppliersVM
                {
                    SupplierId = x.SupplierId,
                    NameAr = x.NameAr ??"",
                    NameEn = x.NameEn??"",
                    TaxNo = x.TaxNo == null ? "" : x.TaxNo,
                    PhoneNo = x.PhoneNo == null ? "" : x.PhoneNo,
                    AccountId=x.AccountId,
                    CompAddress = x.CompAddress??"",
                    PostalCodeFinal = x.PostalCodeFinal??"",
                    ExternalPhone = x.ExternalPhone??"",
                    Country = x.Country??"",
                    Neighborhood = x.Neighborhood ?? "",
                    StreetName = x.StreetName??"",
                    BuildingNumber = x.BuildingNumber??"",
                    CityId = x.CityId??0,
                    CityName = x.city != null ? x.city.NameAr : "",
                    TotalBalance = (_TaamerProContext.Transactions.Where(c => c.IsDeleted == false && c.AccountId==x.AccountId && c.YearId == YearId && c.IsPost == true && c.BranchId == BranchId && c.Type != 12 && c.Type !=(int)VoucherType.PurchesOrder).Sum(s => s.Depit)) - (_TaamerProContext.Transactions.Where(c => c.IsDeleted == false && c.AccountId == x.AccountId && c.YearId == YearId && c.IsPost == true && c.BranchId == BranchId && c.Type != 12).Sum(s => s.Credit)),

                }).ToList();
                return Suppliers;
            }
            else

            {
                var Suppliers = _TaamerProContext.Acc_Suppliers.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText) || s.TaxNo.Contains(SearchText) || s.PhoneNo.Contains(SearchText))).Select(x => new Acc_SuppliersVM
                {
                    SupplierId = x.SupplierId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    TaxNo = x.TaxNo == null ? "" : x.TaxNo,
                    PhoneNo = x.PhoneNo == null ? "" : x.PhoneNo,
                    AccountId = x.AccountId,
                    CompAddress = x.CompAddress ?? "",
                    PostalCodeFinal = x.PostalCodeFinal ?? "",
                    ExternalPhone = x.ExternalPhone ?? "",
                    Country = x.Country ?? "",
                    Neighborhood = x.Neighborhood ?? "",
                    StreetName = x.StreetName ?? "",
                    BuildingNumber = x.BuildingNumber ?? "",
                    CityId = x.CityId ?? 0,
                    CityName = x.city != null ? x.city.NameAr : "",
                    TotalBalance = (_TaamerProContext.Transactions.Where(c => c.IsDeleted == false && c.AccountId == x.AccountId && c.YearId == YearId && c.IsPost == true && c.BranchId == BranchId && c.Type != 12).Sum(s => s.Depit)) - (_TaamerProContext.Transactions.Where(c => c.IsDeleted == false && c.AccountId == x.AccountId && c.YearId == YearId && c.IsPost == true && c.BranchId == BranchId && c.Type != 12).Sum(s => s.Credit)),

                }).ToList();
                return Suppliers;
            }
        }

        public Acc_Suppliers GetById(int Id)
        {
           return _TaamerProContext.Acc_Suppliers.Where(x => x.SupplierId == Id).FirstOrDefault();
        }

        public IEnumerable<Acc_Suppliers> GetMatching(Func<Acc_Suppliers, bool> where)
        {
            return _TaamerProContext.Acc_Suppliers.Where(where).ToList<Acc_Suppliers>();
        }

        public async Task<Acc_SuppliersVM> GetSupplierByID(int SupplierId)
        {
            var Suppliers = _TaamerProContext.Acc_Suppliers.Where(s => s.IsDeleted == false && s.SupplierId == SupplierId).Select(x => new Acc_SuppliersVM
            {
                SupplierId = x.SupplierId,
                NameAr = x.NameAr ?? "",
                NameEn = x.NameEn ?? "",
                TaxNo = x.TaxNo == null ? "" : x.TaxNo,
                PhoneNo = x.PhoneNo == null ? "" : x.PhoneNo,
                AccountId = x.AccountId,
                CompAddress = x.CompAddress ?? "",
                PostalCodeFinal = x.PostalCodeFinal ?? "",
                ExternalPhone = x.ExternalPhone ?? "",
                Country = x.Country ?? "",
                Neighborhood = x.Neighborhood ?? "",
                StreetName = x.StreetName ?? "",
                BuildingNumber = x.BuildingNumber ?? "",
                CityId = x.CityId ?? 0,
                CityName = x.city != null ? x.city.NameAr : "",
            }).FirstOrDefault();
            return Suppliers;
        }


    }
}
