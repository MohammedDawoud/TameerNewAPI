using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IWhatsAppSettingsService
    {
        GeneralMessage SaveWhatsAppSetting(WhatsAppSettings whatsAppSettings, int UserId, int BranchId);
        Task<WhatsAppSettingsVM> GetWhatsAppSetting(int BranchId);
        GeneralMessage SendWhatsApp_Test(int UserId, int BranchId, string Mobile, string Message, string environmentURL, string PDFURL);
    }
}
