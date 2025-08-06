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
    public class RegionTypesRepository : IRegionTypesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public RegionTypesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<RegionTypesVM>> GetAllRegionTypes(string SearchText)
        {
            if (SearchText == "")
            {
                var regionTypes = _TaamerProContext.RegionTypes.Where(s => s.IsDeleted == false).Select(x => new RegionTypesVM
                {
                    RegionTypeId = x.RegionTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return regionTypes;
            }
            else
              {
                 var regionTypes = _TaamerProContext.RegionTypes.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new RegionTypesVM
                {
                    RegionTypeId = x.RegionTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return regionTypes;
              }
            }
        }
    }
