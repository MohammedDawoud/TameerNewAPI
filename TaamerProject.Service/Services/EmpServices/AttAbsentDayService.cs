using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class AttAbsentDayService : IAttAbsentDayService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttAbsentDayRepository _AttAbsentDayRepository;

        public AttAbsentDayService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttAbsentDayRepository attAbsentDay)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttAbsentDayRepository = attAbsentDay;

        }
        public async Task<IEnumerable<AttAbsentDayVM>> GetAllAttAbsentDay(int EmpId, int Year, int Month)
        {
            var AttAbsentDay = await _AttAbsentDayRepository.GetAllAttAbsentDay(EmpId, Year, Month);
            return AttAbsentDay;
        }
    }
}
