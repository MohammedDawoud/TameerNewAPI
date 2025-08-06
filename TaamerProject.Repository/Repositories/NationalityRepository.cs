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
    public class NationalityRepository : INationalityRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public NationalityRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task< IEnumerable<NationalityVM>> GetAllNationalities(string SearchText)
        {
            if (SearchText == "")
            {
                var nationalities = _TaamerProContext.Nationality.Where(s => s.IsDeleted == false).Select(x => new NationalityVM
                {
                    NationalityId = x.NationalityId,
                    NationalityCode = x.NationalityCode,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return nationalities;
            }
            else
            {
                var nationalities = _TaamerProContext.Nationality.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new NationalityVM
                {
                    NationalityId = x.NationalityId,
                    NationalityCode = x.NationalityCode,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return nationalities;
            }
        }
        public async Task< IEnumerable<NationalityVM>> GetAllNationalitiesById(int Id)
        {
           
                var nationalities = _TaamerProContext.Nationality.Where(s => s.IsDeleted == false && s.NationalityId==Id).Select(x => new NationalityVM
                {
                    NationalityId = x.NationalityId,
                    NationalityCode = x.NationalityCode,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Notes = x.Notes
                }).ToList();
                return nationalities;
           
        }
    }
}


