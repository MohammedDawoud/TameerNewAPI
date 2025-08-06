using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface ILicencesService
    {
        Task<IEnumerable<LicencesVM>> GetAllLicences(string SearchText);
        GeneralMessage SaveLicence(Licences Licence, int UserId, int BranchId);
        GeneralMessage UpdatSupportDate(Licences Licence, int UserId, int BranchId);
        GeneralMessage UpdateLicenceLabaik(Licences Licence);

        GeneralMessage SaveLicenceAlerts(Licences Licence, int UserId, int BranchId);
        GeneralMessage SaveLicenceAlertsBtn(Licences Licence, int UserId, int BranchId);
        GeneralMessage CheckLicenceG_UID(Licences Licence);
    }
}
