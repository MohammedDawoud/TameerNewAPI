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
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class AttendaceTimeService : IAttendaceTimeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttendaceTimeRepository _AttendaceTimeRepository;

        public AttendaceTimeService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttendaceTimeRepository attendaceTimeRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttendaceTimeRepository = attendaceTimeRepository;

        }
        public async Task<IEnumerable<AttendaceTimeVM>> GetAllAttendaceTime()
        {
            var AttendaceTime =await _AttendaceTimeRepository.GetAllAttendaceTime();
            return AttendaceTime;
        }
        public GeneralMessage SaveAttendaceTime(AttendaceTime attendaceTime, int UserId, int BranchId, string Lang)
        {
            try
            {

                var codeExist = _TaamerProContext.AttendaceTime.Where(s => s.IsDeleted == false && s.TimeId != attendaceTime.TimeId && s.Code == attendaceTime.Code && s.BranchId == BranchId).FirstOrDefault();
                if (codeExist != null)
                {
                    var messageBefore = "";
                    if (Lang == "rtl")
                    {
                        messageBefore = Resources.TheCodeAlreadyExists;
                    }
                    else if (Lang == "ltr")
                    {
                        messageBefore = "The code already exists";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حفظ وقت الحضور ";
                    _SystemAction.SaveAction("SaveAttendaceTime", "AttendaceTimeService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                if (attendaceTime.TimeId == 0)
                {
                    attendaceTime.AddUser = UserId;
                    attendaceTime.BranchId = attendaceTime.BranchId;
                    attendaceTime.AddDate = DateTime.Now;
                    attendaceTime.Code = _TaamerProContext.AttendaceTime.AsQueryable().Count() + 1.ToString();
                    _TaamerProContext.AttendaceTime.Add(attendaceTime);
                    _TaamerProContext.SaveChanges();
                    var message = "";
                    if (Lang == "rtl")
                    {
                        message = Resources.General_SavedSuccessfully;
                    }
                    else if (Lang == "ltr")
                    {
                        message = "Saved Successfully";
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة وقت حضور ";
                    _SystemAction.SaveAction("SaveAttendaceTime", "AttendaceTimeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var AttendaceTimeUpdated = _TaamerProContext.AttendaceTime.Where(x=>x.TimeId==attendaceTime.TimeId).FirstOrDefault();
                    if (AttendaceTimeUpdated != null)
                    {
                        //AttendaceTimeUpdated.Code = attendaceTime.Code;
                        AttendaceTimeUpdated.NameAr = attendaceTime.NameAr;
                        AttendaceTimeUpdated.NameEn = attendaceTime.NameEn;
                        AttendaceTimeUpdated.Notes = attendaceTime.Notes;
                        AttendaceTimeUpdated.UserId = attendaceTime.UserId;
                        AttendaceTimeUpdated.UpdateUser = UserId;
                        AttendaceTimeUpdated.BranchId = attendaceTime.BranchId;
                        AttendaceTimeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    var message = "";
                    if (Lang == "rtl")
                    {
                        message = Resources.General_SavedSuccessfully;
                    }
                    else if (Lang == "ltr")
                    {
                        message = "Saved Successfully";
                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل وقت الحضور رقم " + attendaceTime.TimeId;
                    _SystemAction.SaveAction("SaveAttendaceTime", "AttendaceTimeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------


                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                var message = "";
                if (Lang == "rtl")
                {
                    message = Resources.General_SavedFailed;
                }
                else if (Lang == "ltr")
                {
                    message = "Saved Falied";
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ وقت الحضور";
                _SystemAction.SaveAction("SaveAttendaceTime", "AttendaceTimeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public async Task<int> GenerateNextAttendaceTimeNumber(int BranchId)
        {
            return await _AttendaceTimeRepository.GenerateNextAttendaceTimeNumber(BranchId);
        }
        public GeneralMessage DeleteAttendaceTime(int TimeId, int UserId, int BranchId)
        {
            try
            {
                AttendaceTime Atten = _TaamerProContext.AttendaceTime.Where(x=>x.TimeId==TimeId).FirstOrDefault();
                Atten.IsDeleted = true;
                Atten.DeleteDate = DateTime.Now;
                Atten.DeleteUser = UserId;
                // delete details
                var details = _TaamerProContext.AttTimeDetails.Where(S => S.AttTimeId == TimeId);
                if (details != null)
                {
                    _TaamerProContext.AttTimeDetails.RemoveRange(details);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف وقت الحضور رقم " + TimeId;
                _SystemAction.SaveAction("DeleteAttendaceTime", "AttendaceTimeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحذف" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف وقت الحضور رقم " + TimeId; ;
                _SystemAction.SaveAction("DeleteAttendaceTime", "AttendaceTimeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
