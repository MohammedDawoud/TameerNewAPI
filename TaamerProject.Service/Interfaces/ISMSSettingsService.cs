using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ISMSSettingsService  
    {
        GeneralMessage SavesmsSetting(SMSSettings sMSSettings, int UserId, int BranchId);
        Task<SMSSettingsVM> GetsmsSetting(int BranchId);
        GeneralMessage SendSMS_Test(int UserId, int BranchId, string Mobile, string Message);
    }
}
