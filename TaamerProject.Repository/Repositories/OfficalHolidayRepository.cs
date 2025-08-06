using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class OfficalHolidayRepository : IOfficalHolidayRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OfficalHolidayRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<OfficalHolidayVM>> GetAllOfficalHoliday()
        {
            var OfficalHoliday = _TaamerProContext.OfficalHoliday.Where(s => s.IsDeleted == false).Select(x => new OfficalHolidayVM
            {
                Id = x.Id,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                Description = x.Description,
            }); 
            return OfficalHoliday;

        }
        public async Task< IEnumerable<OfficalHolidayVM>> GetAllOfficalHolidaySearch(OfficalHolidayVM Search)
        {
            var OfficalHoliday = _TaamerProContext.OfficalHoliday.Where(s => s.IsDeleted == false && (Search.FromDate <= s.FromDate || Search.FromDate == null ) &&
            (s.ToDate <= Search.ToDate || Search.ToDate == null)).Select(x => new OfficalHolidayVM
            {
                Id = x.Id,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                Description = x.Description,
            });
            return OfficalHoliday;

        }

    } 
    
}
