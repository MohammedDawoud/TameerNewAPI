using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Service.Interfaces
{
    public interface IDatabaseBackupService  
    {
        Task<IEnumerable<DatabaseBackupVM>> GetAllDBackup();
        GeneralMessage SaveDBackup(DatabaseBackup info, int UserId, string path,int BranchId, string remote, string Con);
        GeneralMessage DeleteBackup(int BackupId, int UserId, int BranchId, string path);
        Task<IEnumerable<DatabaseBackupVM>> GetDBDBackupById(int backupid);
        GeneralMessage sendmailnotification(List<int> user, int UserId, int BranchId, string Dayes, string Time);
        GeneralMessage SaveDBackup2(string path,string Con);
        GeneralMessage UplaodFileOnDrive(string fileName, string paths, string CsPath, string FolderPath, string mimmapping, string bckuppth);
        GeneralMessage UplaodFileOnDriveProject(string fileName, string paths, string CsPath, string FolderPath, string mimmapping, string bckuppth);

        GeneralMessage UploadFiletodrive2(string fileName, string paths, string CsPath, string FolderPath, string mimmapping, string bckuppth);
        GeneralMessage SaveDBackup_ActiveYear(DatabaseBackup info, int UserId, string path, int BranchId, string remote, int yearid, string Con);
        BackupStatistics GetBackupStatistics(string lang);
        Task<BackupStatistics> GetDBackupByIDWithDetails(int backupid, string lang);
    }
}
