using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class BackupAlertService : IBackupAlertService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IBackupAlertRepository _backupAlertRepository;
        private readonly IUsersRepository _usersRepository;


        public BackupAlertService(TaamerProjectContext dataContext, ISystemAction systemAction, IBackupAlertRepository backupAlertRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _backupAlertRepository = backupAlertRepository;
            this._usersRepository = usersRepository;
        }
        public async Task<IEnumerable<BackupAlertVM>> GetAllBackupAlert()
        {
            var backupalert = await _backupAlertRepository.GetAllBackupAlert();
            return backupalert;
        }

        public GeneralMessage SaveBackupalert(List<BackupAlert> backupAlert, int UserId, int BranchId)
        {
            try
            {


                foreach (var item in backupAlert)
                {
                    DateTime Alert_NextDate = DateTime.Now;
                    string nextdayalert = "";
                    if (item.AlertTimeType == 1)
                    {
                        Alert_NextDate = Alert_NextDate.AddDays(1);
                    }
                    else if (item.AlertTimeType == 2)
                    {

                        Alert_NextDate = Alert_NextDate.AddDays(7);
                    }
                    else if (item.AlertTimeType == 3)
                    {
                        Alert_NextDate = Alert_NextDate.AddDays(30);
                    }

                    nextdayalert = Alert_NextDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                    if (item.AlertId == 0)
                    {

                        item.AddUser = UserId;
                        item.AddDate = DateTime.Now;
                        item.AlertNextTime = nextdayalert;
                        item.Alert_IsSent = 0;

                        _TaamerProContext.BackupAlert.Add(item);

                        //-----------------------------------------------------------------------------------------------------------------

                    }
                    else
                    {
                        var AlertUpdated = _TaamerProContext.BackupAlert.Where(x=>x.AlertId==item.AlertId).FirstOrDefault();
                        if (AlertUpdated != null)
                        {
                            AlertUpdated.AlertNextTime = item.AlertNextTime;
                            AlertUpdated.AlertSms = item.AlertSms;
                            AlertUpdated.AlertTime = item.AlertTime;
                            AlertUpdated.AlertTimeType = item.AlertTimeType;
                            AlertUpdated.UpdateUser = UserId;
                            AlertUpdated.UpdateDate = DateTime.Now;
                            AlertUpdated.AlertNextTime = nextdayalert;
                            AlertUpdated.Alert_IsSent = 0;

                        }



                    }

                }
                _TaamerProContext.SaveChanges();
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة تنبية نسخة احتياطية";
               _SystemAction.SaveAction("SaveBackupalert", "BackupAlertService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تنبيهات النسخة الاحتياطية";
                _SystemAction.SaveAction("SaveBackupalert", "BackupAlertService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }


        public GeneralMessage DeleteBackupalert(int alertid, int UserId, int BranchId)
        {
            try
            {


                if (alertid != null)
                {
                    var deletealert = _TaamerProContext.BackupAlert.Where(x => x.AlertId == alertid).FirstOrDefault();
                    deletealert.IsDeleted = true;
                    deletealert.DeleteUser = UserId;
                    deletealert.DeleteDate = DateTime.Now;


                }
                _TaamerProContext.SaveChanges();
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حذف تنبية نسخة احتياطية";
                _SystemAction.SaveAction("DeleteBackupalert", "BackupAlertService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف تنبيهات النسخة الاحتياطية";
                _SystemAction.SaveAction("DeleteBackupalert", "BackupAlertService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }

    }
}
