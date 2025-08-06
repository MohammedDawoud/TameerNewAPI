using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class AllowanceTypeRepository : IAllowanceTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public AllowanceTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<AllowanceTypeVM>> GetAllAllowancesTypes(string SearchText, bool? IsSalaryPart = false)
        {
            if (SearchText == "")
            {
                var AllowancesTypes = _TaamerProContext.AllowanceType.Where(s => s.IsDeleted == false && 
                (IsSalaryPart.HasValue? s.IsSalaryPart == IsSalaryPart.Value : true)).Select(x => new AllowanceTypeVM
                {
                    AllowanceTypeId = x.AllowanceTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                    UserId = x.UserId
                });
                return AllowancesTypes;
            }
            else
            {
                var AllowancesTypes = _TaamerProContext.AllowanceType.Where(s => s.IsDeleted == false && 
                (IsSalaryPart.HasValue ? s.IsSalaryPart == IsSalaryPart.Value : true) && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new AllowanceTypeVM
                {
                    AllowanceTypeId = x.AllowanceTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                    UserId = x.UserId
                });
                return AllowancesTypes;
            }
        }
    }
}


