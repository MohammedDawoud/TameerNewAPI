
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class Sys_SystemActionsRepository : ISys_SystemActionsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Sys_SystemActionsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<Sys_SystemActionsVM>> GetAllSystemActionsAll(string DateFrom, string DateTo)
        {
            var Sys_SystemActions = _TaamerProContext.Sys_SystemActions.Where(s => s.IsDeleted == false && s.UserId!=1).Select(x => new
            {
                SysID = x.SysID,
                FunctionName = x.FunctionName,
                ServiceName = x.ServiceName,
                ActionType = x.ActionType,
                MessageName = x.MessageName,
                ModuleName = x.ModuleName,
                PageName = x.PageName,
                ActionDate = x.ActionDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                Note = x.Note,
                Success = x.Success ?? 0,
                UserName = x.Users == null ? "" : x.Users.FullName,
                BranchName = x.Branches == null ? "" : x.Branches.NameAr,
                ActionTypeName = x.ActionType == 1 ? "حفظ" : x.ActionType == 2 ? "تعديل" : x.ActionType == 3 ? "حذف" : "",
                SuccessName = x.Success == null ? "عملية فاشلة" : x.Success == 1 ? "عملية ناجحة" : "عملية فاشلة",
                FullDate = x.AddDate,
            }).Select(m => new Sys_SystemActionsVM
            {
                SysID = m.SysID,
                FunctionName = m.FunctionName,
                ServiceName = m.ServiceName,
                ActionType = m.ActionType,
                MessageName = m.MessageName,
                ModuleName = m.ModuleName,
                PageName = m.PageName,
                ActionDate = m.ActionDate,
                UserId = m.UserId,
                BranchId = m.BranchId,
                Note = m.Note,
                Success = m.Success,
                UserName = m.UserName,
                BranchName = m.BranchName,
                ActionTypeName = m.ActionTypeName,
                SuccessName = m.SuccessName,
                FullDate = m.FullDate,

            }).ToList().Where(s => DateTime.ParseExact(s.ActionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.ActionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            ;
            return Sys_SystemActions;
        }

        public async Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActions(string SearchText, string DateFrom, string DateTo, int BranchId,int UserId,int ActionType)
        {
            if (SearchText == "" || SearchText==null)
            {
                if (DateFrom != "" && DateTo != "" && DateFrom != null && DateTo != null)
                {
                    var Sys_SystemActions = _TaamerProContext.Sys_SystemActions.Where(s => s.IsDeleted == false && s.UserId != 1 && (s.UserId==UserId || UserId==0) && (s.ActionType==ActionType || ActionType==0)).Select(x => new
                    {
                        SysID = x.SysID,
                        FunctionName = x.FunctionName,
                        ServiceName = x.ServiceName,
                        ActionType = x.ActionType,
                        MessageName = x.MessageName,
                        ModuleName = x.ModuleName,
                        PageName = x.PageName,
                        ActionDate = x.ActionDate,
                        UserId = x.UserId,
                        BranchId = x.BranchId,
                        Note = x.Note,
                        Success = x.Success ?? 0,
                        UserName = x.Users == null ? "" : x.Users.FullName,
                        BranchName = x.Branches == null ? "" : x.Branches.NameAr,
                        ActionTypeName = x.ActionType == 1 ? "حفظ" : x.ActionType == 2 ? "تعديل" : x.ActionType == 3 ? "حذف" : "",
                        SuccessName = x.Success == null ? "عملية فاشلة" : x.Success == 1 ? "عملية ناجحة" : "عملية فاشلة",
                        FullDate = x.AddDate,
                    }).Select(m => new Sys_SystemActionsVM
                    {
                        SysID = m.SysID,
                        FunctionName = m.FunctionName,
                        ServiceName = m.ServiceName,
                        ActionType = m.ActionType,
                        MessageName = m.MessageName,
                        ModuleName = m.ModuleName,
                        PageName = m.PageName,
                        ActionDate = m.ActionDate,
                        UserId = m.UserId,
                        BranchId = m.BranchId,
                        Note = m.Note,
                        Success = m.Success,
                        UserName = m.UserName,
                        BranchName = m.BranchName,
                        ActionTypeName = m.ActionTypeName,
                        SuccessName = m.SuccessName,
                        FullDate = m.FullDate,

                    }).ToList().Where(s => DateTime.ParseExact(s.ActionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.ActionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
;
                    return Sys_SystemActions;
                }
                else
                {
                    var Sys_SystemActions = _TaamerProContext.Sys_SystemActions.Where(s => s.IsDeleted == false && s.UserId != 1 && (s.UserId == UserId || UserId == 0) && (s.ActionType == ActionType || ActionType == 0)).Select(x => new
                    {
                        SysID = x.SysID,
                        FunctionName = x.FunctionName,
                        ServiceName = x.ServiceName,
                        ActionType = x.ActionType,
                        MessageName = x.MessageName,
                        ModuleName = x.ModuleName,
                        PageName = x.PageName,
                        ActionDate = x.ActionDate,
                        UserId = x.UserId,
                        BranchId = x.BranchId,
                        Note = x.Note,
                        Success = x.Success ?? 0,
                        UserName = x.Users == null ? "" : x.Users.FullName,
                        BranchName = x.Branches == null ? "" : x.Branches.NameAr,
                        ActionTypeName = x.ActionType == 1 ? "حفظ" : x.ActionType == 2 ? "تعديل" : x.ActionType == 3 ? "حذف" : "",
                        SuccessName = x.Success == null ? "عملية فاشلة" : x.Success == 1 ? "عملية ناجحة" : "عملية فاشلة",
                        FullDate = x.AddDate,

                    }).Select(m => new Sys_SystemActionsVM
                    {
                        SysID = m.SysID,
                        FunctionName = m.FunctionName,
                        ServiceName = m.ServiceName,
                        ActionType = m.ActionType,
                        MessageName = m.MessageName,
                        ModuleName = m.ModuleName,
                        PageName = m.PageName,
                        ActionDate = m.ActionDate,
                        UserId = m.UserId,
                        BranchId = m.BranchId,
                        Note = m.Note,
                        Success = m.Success,
                        UserName = m.UserName,
                        BranchName = m.BranchName,
                        ActionTypeName = m.ActionTypeName,
                        SuccessName = m.SuccessName,
                        FullDate = m.FullDate,

                    }).ToList();
;
                    return Sys_SystemActions;
                }

            }
            else
            {
                if (DateFrom != "" && DateTo != "" && DateFrom != null && DateTo != null)
                {
                    var Sys_SystemActions = _TaamerProContext.Sys_SystemActions.Where(s => s.IsDeleted == false && s.UserId != 1 && (s.UserId == UserId || UserId == 0) && (s.ActionType == ActionType || ActionType == 0)).Select(x => new
                    {
                        SysID = x.SysID,
                        FunctionName = x.FunctionName,
                        ServiceName = x.ServiceName,
                        ActionType = x.ActionType,
                        MessageName = x.MessageName,
                        ModuleName = x.ModuleName,
                        PageName = x.PageName,
                        ActionDate = x.ActionDate,
                        UserId = x.UserId,
                        BranchId = x.BranchId,
                        Note = x.Note,
                        Success = x.Success ?? 0,
                        UserName = x.Users == null ? "" : x.Users.FullName,
                        BranchName = x.Branches == null ? "" : x.Branches.NameAr,
                        ActionTypeName = x.ActionType == 1 ? "حفظ" : x.ActionType == 2 ? "تعديل" : x.ActionType == 3 ? "حذف" : "",
                        SuccessName = x.Success == null ? "عملية فاشلة" : x.Success == 1 ? "عملية ناجحة" : "عملية فاشلة",
                        FullDate = x.AddDate,

                    }).Select(m => new Sys_SystemActionsVM
                    {
                        SysID = m.SysID,
                        FunctionName = m.FunctionName,
                        ServiceName = m.ServiceName,
                        ActionType = m.ActionType,
                        MessageName = m.MessageName,
                        ModuleName = m.ModuleName,
                        PageName = m.PageName,
                        ActionDate = m.ActionDate,
                        UserId = m.UserId,
                        BranchId = m.BranchId,
                        Note = m.Note,
                        Success = m.Success,
                        UserName = m.UserName,
                        BranchName = m.BranchName,
                        ActionTypeName = m.ActionTypeName,
                        SuccessName = m.SuccessName,
                        FullDate = m.FullDate,

                    }).ToList().Where(s => DateTime.ParseExact(s.ActionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.ActionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))
;
                    return Sys_SystemActions;
                }
                else
                {
                    var Sys_SystemActions = _TaamerProContext.Sys_SystemActions.Where(s => s.IsDeleted == false && s.UserId != 1 && (s.UserId == UserId || UserId == 0) && (s.ActionType == ActionType || ActionType == 0) && (s.MessageName.Contains(SearchText) || s.Note.Contains(SearchText))).Select(x => new
                    {
                        SysID = x.SysID,
                        FunctionName = x.FunctionName,
                        ServiceName = x.ServiceName,
                        ActionType = x.ActionType,
                        MessageName = x.MessageName,
                        ModuleName = x.ModuleName,
                        PageName = x.PageName,
                        ActionDate = x.ActionDate,
                        UserId = x.UserId,
                        BranchId = x.BranchId,
                        Note = x.Note,
                        Success = x.Success ?? 0,
                        UserName = x.Users == null ? "" : x.Users.FullName,
                        BranchName = x.Branches == null ? "" : x.Branches.NameAr,
                        ActionTypeName = x.ActionType == 1 ? "حفظ" : x.ActionType == 2 ? "تعديل" : x.ActionType == 3 ? "حذف" : "",
                        SuccessName = x.Success == null ? "عملية فاشلة" : x.Success == 1 ? "عملية ناجحة" : "عملية فاشلة",
                        FullDate = x.AddDate,

                    }).Select(m => new Sys_SystemActionsVM
                    {
                        SysID = m.SysID,
                        FunctionName = m.FunctionName,
                        ServiceName = m.ServiceName,
                        ActionType = m.ActionType,
                        MessageName = m.MessageName,
                        ModuleName = m.ModuleName,
                        PageName = m.PageName,
                        ActionDate = m.ActionDate,
                        UserId = m.UserId,
                        BranchId = m.BranchId,
                        Note = m.Note,
                        Success = m.Success,
                        UserName = m.UserName,
                        BranchName = m.BranchName,
                        ActionTypeName = m.ActionTypeName,
                        SuccessName = m.SuccessName,
                        FullDate = m.FullDate,

                    }).ToList();
                    ;
                    return Sys_SystemActions;
                }
            }
        }

    }
}
