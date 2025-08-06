using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAttAbsentDayRepository
    {
        Task<IEnumerable<AttAbsentDayVM>> GetAllAttAbsentDay(int EmpId, int Year, int Month);
    }
}
