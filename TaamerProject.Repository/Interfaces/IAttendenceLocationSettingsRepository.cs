using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAttendenceLocationSettingsRepository
    {
        Task<IEnumerable<AttendenceLocationSettingsVM>> GetAllAttendencelocations(string SearchText);
        Task<AttendenceLocationSettingsVM> GetlAttendencelocationbyId(int attendenceLocationSettingsId);

    }
}
