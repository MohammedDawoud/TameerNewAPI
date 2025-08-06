
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
   public class Pro_SuperContractorRepository : IPro_SuperContractorRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_SuperContractorRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_SuperContractorVM>> GetAllSuperContractors(string SearchText)
        {
            if (SearchText == "")
            {
                var Contractors = _TaamerProContext.Pro_SuperContractor.Where(s => s.IsDeleted == false).Select(x => new Pro_SuperContractorVM
                {
                    ContractorId = x.ContractorId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    Email = x.Email ?? "",
                    CommercialRegister = x.CommercialRegister ?? "",
                    PhoneNo = x.PhoneNo ?? "",
                }).ToList();
                return Contractors;
            }
            else

            {
                var Contractors = _TaamerProContext.Pro_SuperContractor.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new Pro_SuperContractorVM
                {
                    ContractorId = x.ContractorId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    Email = x.Email ?? "",
                    CommercialRegister = x.CommercialRegister ?? "",
                    PhoneNo = x.PhoneNo ?? "",
                }).ToList();
                return Contractors;
            }
        }
        public async Task<Pro_SuperContractorVM> GetContractorData(int? ContractorId, int UserID, int BranchId)
        {
            var Contractors = _TaamerProContext.Pro_SuperContractor.Where(s => s.IsDeleted == false).Select(x => new Pro_SuperContractorVM
            {
                ContractorId = x.ContractorId,
                NameAr = x.NameAr??"",
                NameEn = x.NameEn??"",
                Email = x.Email??"",
                CommercialRegister = x.CommercialRegister??"",
                PhoneNo = x.PhoneNo??"",
            }).FirstOrDefault();
            return Contractors;
        }

    }
}
