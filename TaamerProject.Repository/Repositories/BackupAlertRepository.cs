using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class BackupAlertRepository :IBackupAlertRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public BackupAlertRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<BackupAlertVM>> GetAllBackupAlert()
        {
            var alert = _TaamerProContext.BackupAlert.Where(s => s.IsDeleted == false).Select(x => new BackupAlertVM
            {
             AlertId=x.AlertId,
             AlertNextTime=x.AlertNextTime,
             AlertSms=x.AlertSms,
             AlertTime=x.AlertTime,
             AlertTimeType=x.AlertTimeType,
             UserId=x.UserId,
             Email=x.Users.Email,
             Mobile=x.Users.Mobile,
            });
   
            return alert;
        }

    }
}
