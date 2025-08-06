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
    public class VacationTypeRepository : IVacationTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public VacationTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<VacationTypeVM>> GetAllVacationsTypes(string SearchText)
        {
            if (SearchText == "")
            {
                var VacationsTypes = _TaamerProContext.VacationType.Where(s => s.IsDeleted == false).Select(x => new VacationTypeVM
                {
                    VacationTypeId = x.VacationTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return VacationsTypes;
            }
            else
            {
                var VacationsTypes = _TaamerProContext.VacationType.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new VacationTypeVM
                {
                    VacationTypeId = x.VacationTypeId,
                    Code = x.Code,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return VacationsTypes;
            }
        }
    }
}


