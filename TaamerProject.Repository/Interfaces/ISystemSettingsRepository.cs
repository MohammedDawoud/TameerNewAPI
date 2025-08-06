using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISystemSettingsRepository
    {
        Task<SystemSettingsVM> GetSystemSettingsByBranchId(int BranchId);

        Task<SystemSettingsVM> GetSystemSettingsByUserId(int BranchId, int UserID, string Con);
        Task<bool> MaintenanceFunc(string Con, string Lang, int BranchId, int UserId, int Status);


    }
}
