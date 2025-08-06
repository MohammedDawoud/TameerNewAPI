using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDatabaseBackupRepository 
    {
        Task<IEnumerable<DatabaseBackupVM>> GetAllDBackup();
        Task<IEnumerable<DatabaseBackupVM>> GetDBackupByID(int Bakupid);
        Task<DatabaseBackup> GetBackupByID(int Bakupid);
        BackupStatistics GetBackupStatistics(string lang);
        Task<BackupStatistics> GetDBackupByIDWithDetails(int Bakupid, string lang);
    }
}
