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
    public class Pro_MunicipalRepository : IPro_MunicipalRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_MunicipalRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_MunicipalVM>> GetAllMunicipals(string SearchText)
        {
            if (SearchText == "")
            {
                var Pro_Municipals = _TaamerProContext.Pro_Municipal.Where(s => s.IsDeleted == false).Select(x => new Pro_MunicipalVM
                {
                    MunicipalId = x.MunicipalId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return Pro_Municipals;
            }
            else

            {
                var Pro_Municipals = _TaamerProContext.Pro_Municipal.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new Pro_MunicipalVM
                {
                    MunicipalId = x.MunicipalId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return Pro_Municipals;
            }
        }
    }
}
