using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.ViewModels;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttendenceLocationSettingsService
    {
        Task<IEnumerable<AttendenceLocationSettingsVM>> GetAllAttendencelocations(string SearchText);
        Task<AttendenceLocationSettingsVM> GetlAttendencelocationbyId(int attendenceLocationSettingsId);

        GeneralMessage SaveAttendenceLocation(AttendenceLocationSettings location, int UserId, int BranchId);
        GeneralMessage DeleteAttendenceLocation(int Id, int UserId, int BranchId);
        IEnumerable<object> FillAttendenceLocation(string SearchText = "");

    }
}
