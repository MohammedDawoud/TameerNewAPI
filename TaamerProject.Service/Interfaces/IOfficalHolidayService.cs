using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOfficalHolidayService
    {
        Task<IEnumerable<OfficalHolidayVM>> GetAllOfficalHoliday();

        Task<IEnumerable<OfficalHolidayVM>> GetAllOfficalHolidaySearch(OfficalHolidayVM Search, int BranchId);

        GeneralMessage SaveOfficalHoliday(OfficalHoliday carMovement, int UserId, int BranchId);
        GeneralMessage DeleteOfficalHoliday(int Id, int UserId, int BranchId);
    }
}
