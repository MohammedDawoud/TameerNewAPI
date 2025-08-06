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
    public class Pro_SubMunicipalityRepository : IPro_SubMunicipalityRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_SubMunicipalityRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_SubMunicipalityVM>> GetAllSubMunicipalitys(string SearchText)
        {
            if (SearchText == "")
            {
                var SubMunicipalitys = _TaamerProContext.Pro_SubMunicipality.Where(s => s.IsDeleted == false).Select(x => new Pro_SubMunicipalityVM
                {
                    SubMunicipalityId = x.SubMunicipalityId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return SubMunicipalitys;
            }
            else

            {
                var SubMunicipalitys = _TaamerProContext.Pro_SubMunicipality.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new Pro_SubMunicipalityVM
                {
                    SubMunicipalityId = x.SubMunicipalityId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return SubMunicipalitys;
            }
        }
    }
}
