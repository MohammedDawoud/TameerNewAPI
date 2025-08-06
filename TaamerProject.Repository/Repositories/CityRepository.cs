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
    public class CityRepository :  ICityRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CityRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<CityVM>> GetAllCities(string SearchText)
        {
            if (SearchText == "")
            {
                var cities = _TaamerProContext.City.Where(s => s.IsDeleted == false).Select(x => new CityVM
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
                var cities = _TaamerProContext.City.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new CityVM
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
