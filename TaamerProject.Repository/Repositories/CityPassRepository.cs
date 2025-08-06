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
    public class CityPassRepository : ICityPassRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public CityPassRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<CityPassVM>> GetAllCities(string SearchText)
        {
            if (SearchText == "")
            {
                var cities = _TaamerProContext.CityPass.Where(s => s.IsDeleted == false).Select(x => new CityPassVM
                {
                    CityId = x.CityId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                }).ToList();
                return cities;
            }
            else
            {
                var cities = _TaamerProContext.CityPass.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new CityPassVM
                {
                    CityId = x.CityId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes,
                }).ToList();
                return cities;

            }
        }
    }
}
