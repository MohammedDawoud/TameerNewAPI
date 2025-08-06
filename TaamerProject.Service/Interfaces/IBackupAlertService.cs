using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IBackupAlertService
    {
        Task<IEnumerable<BackupAlertVM>> GetAllBackupAlert();

        GeneralMessage SaveBackupalert(List<BackupAlert> backupAlert, int UserId, int BranchId);
        GeneralMessage DeleteBackupalert(int alertid, int UserId, int BranchId);

    }
}
