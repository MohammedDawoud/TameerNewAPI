using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ISystemSettingsService  
    {
        Task<SystemSettingsVM> GetSystemSettingsByBranchId(int BranchId);

        SystemSettingsVM GetSystemSettingsByUserId(int BranchId, int UserID, string Con);
        GeneralMessage SaveSystemSettings(SystemSettings systemSettings, int UserId, int BranchId);
        GeneralMessage UpdateOrgDataRequired(bool isreq, int UserId, int BranchId);
        GeneralMessage ValidateZatcaRequests(bool iszatcacheck, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage ValidateDestinationRequest(int UserId, OrganizationsVM Organization, Branch Branch, string Url, string ImgUrl,int ProjectId,int UploadType, string DesName);

        GeneralMessage ValidateZatcaCode(bool iszatcacheck, string Sentcode, int UserId, int BranchId);
        GeneralMessage MaintenanceFunc(string con, string Lang, int BranchId, int UserId, int Status);



    }
}
