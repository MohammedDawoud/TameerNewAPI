using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttAbsentDayService
    {
        Task<IEnumerable<AttAbsentDayVM>> GetAllAttAbsentDay(int EmpId, int Year, int Month);

    }
}
