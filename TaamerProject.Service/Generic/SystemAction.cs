using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;

namespace TaamerProject.Service.Generic
{
    public class SystemAction : ISystemAction
    {

        private readonly TaamerProjectContext _TaamerProContext;
        public SystemAction(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public void SaveAction(string FunctionName, string ServiceName, int ActionType, string MessageName,
            string ModuleName, string PageName, string ActionDate, int UserId, int BranchId, string Note, int Success)
        {
            try
            {
                Sys_SystemActions SysAction = new Sys_SystemActions();
                SysAction.FunctionName = FunctionName;
                SysAction.ServiceName = ServiceName;
                SysAction.ActionType = ActionType;
                SysAction.MessageName = MessageName;
                SysAction.ModuleName = ModuleName;
                SysAction.PageName = PageName;
                SysAction.ActionDate = ActionDate;
                SysAction.UserId = UserId;
                SysAction.BranchId = BranchId;
                SysAction.Note = Note;
                SysAction.Success = Success;
                SysAction.AddUser = UserId;
                SysAction.AddDate = DateTime.Now;
                SysAction.IsDeleted = false;
                _TaamerProContext.Sys_SystemActions.Add(SysAction);
                _TaamerProContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
            }

        }

        public void SaveTaskOperations(Pro_TaskOperations TaskOperations, int UserId, int BranchId)
        {
            try
            {
                TaskOperations.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                TaskOperations.BranchId = BranchId;
                TaskOperations.AddUser = UserId;
                TaskOperations.AddDate = DateTime.Now;
                _TaamerProContext.Pro_TaskOperations.Add(TaskOperations);
                _TaamerProContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
            }

        }
    }
}
