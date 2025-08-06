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
    public class AttAbsentDayRepository :  IAttAbsentDayRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttAbsentDayRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<AttAbsentDayVM>> GetAllAttAbsentDay(int EmpId, int Year, int Month)
        {
            var AttAbsentDay = _TaamerProContext.AttAbsentDay.Where(s => s.EmpId== EmpId && s.Year == Year && s.Month== Month).Select(x => new AttAbsentDayVM
            {
                Id = x.Id,
                EmpId = x.EmpId,
                Year = x.Year,               
                Month = x.Month,
                AbsDays = x.AbsDays,
                SDate = x.SDate,
                EDate = x.EDate              
            });
           

            return AttAbsentDay;
        }
    }
}
