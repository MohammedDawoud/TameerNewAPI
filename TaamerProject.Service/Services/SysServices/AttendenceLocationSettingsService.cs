using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services.SysServices
{
    public class AttendenceLocationSettingsService : IAttendenceLocationSettingsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttendenceLocationSettingsRepository _attendenceLocationSettings;
        private readonly IUsersRepository _usersRepository;


        public AttendenceLocationSettingsService(TaamerProjectContext dataContext, ISystemAction systemAction, IUsersRepository usersRepository, IAttendenceLocationSettingsRepository attendenceLocationSettings)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _attendenceLocationSettings = attendenceLocationSettings;
            this._usersRepository = usersRepository;
        }

        public Task<IEnumerable<AttendenceLocationSettingsVM>> GetAllAttendencelocations(string SearchText)
        {
            return _attendenceLocationSettings.GetAllAttendencelocations(SearchText);
        }
        public Task<AttendenceLocationSettingsVM> GetlAttendencelocationbyId(int attendenceLocationSettingsId)
        {
            return _attendenceLocationSettings.GetlAttendencelocationbyId(attendenceLocationSettingsId);
        }


        public GeneralMessage SaveAttendenceLocation(AttendenceLocationSettings location, int UserId, int BranchId)
        {
            try
            {
                if (location.AttendenceLocationSettingsId == 0)
                {
                    location.AddUser = UserId;
                    location.AddDate = DateTime.Now;
                    _TaamerProContext.AttendenceLocations.Add(location);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة موقع جديد";
                    _SystemAction.SaveAction("SaveAttendenceLocation", "AttendenceLocationSettingsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var locationUpdated = _TaamerProContext.AttendenceLocations.Where(x => x.AttendenceLocationSettingsId == location.AttendenceLocationSettingsId).FirstOrDefault();
                    if (locationUpdated != null)
                    {
                        locationUpdated.Distance = location.Distance;
                        locationUpdated.Name = location.Name;
                        locationUpdated.Longitude = location.Longitude;
                        locationUpdated.Latitude = location.Latitude;
                        locationUpdated.xmax = location.xmax;
                        locationUpdated.xmin = location.xmin;
                        locationUpdated.ymax = location.ymax;
                        locationUpdated.ymin = location.ymin;

                        locationUpdated.UpdateUser = UserId;
                        locationUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل موقع رقم " + locationUpdated.AttendenceLocationSettingsId;
                    _SystemAction.SaveAction("SaveAttendenceLocation", "AttendenceLocationSettingsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ موقع";
                _SystemAction.SaveAction("SaveAttendenceLocation", "AttendenceLocationSettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteAttendenceLocation(int Id, int UserId, int BranchId)
        {
            try
            {
                AttendenceLocationSettings attlocation = _TaamerProContext.AttendenceLocations.Where(x => x.AttendenceLocationSettingsId == Id).FirstOrDefault();
                attlocation.IsDeleted = true;
                attlocation.DeleteDate = DateTime.Now;
                attlocation.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف موقع رقم " + attlocation.AttendenceLocationSettingsId;
                _SystemAction.SaveAction("DeleteAttendenceLocation", "AttendenceLocationSettingsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف موقع رقم " + Id; ;
                _SystemAction.SaveAction("DeleteAttendenceLocation", "AttendenceLocationSettingsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillAttendenceLocation(string SearchText = "")
        {
            return _attendenceLocationSettings.GetAllAttendencelocations(SearchText).Result.Select(s => new
            {
                Id = s.AttendenceLocationSettingsId,
                Name = s.Name
            });
        }
    }
}
