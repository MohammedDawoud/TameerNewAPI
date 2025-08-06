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
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class PrivFollowersServices :  IPrivFollowersServices
    {
        private readonly IPrivFollowersRepository _PrivFollowersRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public PrivFollowersServices(IPrivFollowersRepository PrivFollowersRepository, 
            ISys_SystemActionsRepository Sys_SystemActionsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _PrivFollowersRepository = PrivFollowersRepository;
            _Sys_SystemActionsRepository = Sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public GeneralMessage SavePrivFollowers(PrivFollowers PrivFollowers, int UserId,int BranchId)
        {
            try
            {
                //switch (settings.TimeType)
                //{
                //    //case 1:
                //    //    settings.TimeMinutes = settings.TimeMinutes;
                //    //    break;
                //    //case 2:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24;
                //    //    break;
                //    //case 3:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24 * 7;
                //    //    break;
                //    //case 4:
                //    //    settings.TimeMinutes = settings.TimeMinutes * 24 * 7 * 30;
                //    //    break;
                //}
                if (PrivFollowers.PrivFollowerID == 0)
                {
                    PrivFollowers.AddUser = UserId;
                    PrivFollowers.AddDate = DateTime.Now;
                    PrivFollowers.IsDeleted = false;
                    _TaamerProContext.PrivFollowers.Add(PrivFollowers);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة متابعة مهمة ";
                    _SystemAction.SaveAction("SavePrivFollowers", "PrivFollowersServices", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully  };
                    }
                else
                {
                    //var PrivFollowersUpdated = _PrivFollowersRepository.GetById(PrivFollowers.PrivFollowerID);
                    var PrivFollowersUpdated = _TaamerProContext.PrivFollowers.Where(s => s.PrivFollowerID == PrivFollowers.PrivFollowerID).FirstOrDefault();

                    if (PrivFollowersUpdated != null)
                    {
                        PrivFollowersUpdated.TaskID = PrivFollowers.TaskID;
                        PrivFollowersUpdated.UserID = PrivFollowers.UserID;
                        PrivFollowersUpdated.Flag = PrivFollowers.Flag;
                      
                        PrivFollowersUpdated.UpdateUser = UserId;
                        PrivFollowersUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل متابعة المهمة رقم " + PrivFollowers.PrivFollowerID;
                    _SystemAction.SaveAction("SaveOutMovements", "OutMovementsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                    }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ متابعة المهمة";
                _SystemAction.SaveAction("SaveOutMovements", "OutMovementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailedFollowers  };
            }
        }
       

    }
}
