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
using TaamerProject.Repository.Repositories;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Sys_SystemActionsService :  ISys_SystemActionsService
    {
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;

        private readonly TaamerProjectContext _TaamerProContext;
        public Sys_SystemActionsService(TaamerProjectContext dataContext
            , ISys_SystemActionsRepository sys_SystemActionsRepository)
        {

            _TaamerProContext = dataContext;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
        }
         public Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActionsAll()
        {
            string DateFrom = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string DateTo = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            var SystemActions = _Sys_SystemActionsRepository.GetAllSystemActionsAll( DateFrom,  DateTo);
            return SystemActions;
        }
        public Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActions(string SearchText, string DateFrom, string DateTo, int BranchId,int UserId,int ActionType)
        {
            var SystemActions = _Sys_SystemActionsRepository.GetAllSystemActions(SearchText, DateFrom, DateTo, BranchId,UserId,ActionType);
            return SystemActions;
        }
       
        public GeneralMessage SaveSystemActions(Sys_SystemActions SystemActions, int UserId, int BranchId)
        {
            try
            {

                SystemActions.FunctionName = SystemActions.FunctionName;
                SystemActions.ServiceName = SystemActions.ServiceName;
                SystemActions.ActionType = SystemActions.ActionType;
                SystemActions.MessageName = SystemActions.MessageName;
                SystemActions.ModuleName = SystemActions.ModuleName;
                SystemActions.PageName = SystemActions.PageName;
                SystemActions.ActionDate = SystemActions.ActionDate;
                SystemActions.UserId = UserId;
                SystemActions.BranchId = BranchId;
                SystemActions.Note = SystemActions.Note;
                SystemActions.Success = SystemActions.Success;

                SystemActions.AddUser = UserId;
                SystemActions.AddDate = DateTime.Now;
                _TaamerProContext.Sys_SystemActions.Add(SystemActions);
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }

    }
}
