using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class DeviceAttService :  IDeviceAttService
    {
        private readonly IDeviceAttRepository _DeviceAttRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public DeviceAttService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IDeviceAttRepository DeviceAttRepository)
        {
            _DeviceAttRepository = DeviceAttRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }


        public Task<IEnumerable<DeviceAttVM>> GetAllDeviceAttByID(string lang, int BranchId,int Id)
        {
            var attendencedevices = _DeviceAttRepository.GetAllDeviceAttByID(lang, BranchId, Id);
            return attendencedevices;
        }

       


        public GeneralMessage SaveDeviceAtt(DeviceAtt devices, int UserId, int BranchId)
        {
            try
            {
                if (devices.Id == 0)
                {
                    devices.DeviceId = devices.DeviceId;
                 
                    _TaamerProContext.DeviceAtt.Add(devices);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة جهاز بصمة جديد";
                     _SystemAction.SaveAction("SaveDeviceAtt", "DeviceAttService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var attendencedeviceUpdated = _DeviceAttRepository.GetById(devices.Id);
                    DeviceAtt? attendencedeviceUpdated = _TaamerProContext.DeviceAtt.Where(s => s.Id == devices.Id).FirstOrDefault();

                    if (attendencedeviceUpdated != null)
                    {
                        
                        attendencedeviceUpdated.LastUpdate = DateTime.Now;
                       
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل جهاز بصمة رقم " + devices.Id;
                     _SystemAction.SaveAction("SaveDeviceAtt", "DeviceAttService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ جهاز بصمة";
                 _SystemAction.SaveAction("SaveDeviceAtt", "DeviceAttService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
      


    }
}
