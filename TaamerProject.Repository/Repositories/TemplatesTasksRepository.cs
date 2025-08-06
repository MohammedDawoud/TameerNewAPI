using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class TemplatesTasksRepository : ITemplatesTasksRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TemplatesTasksRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        ///////////////////////TasksSearch///////////////////////
        public async Task<IEnumerable<TemplatesTasksVM>> GetAllTemplatesTasks(int BranchId)
        {
            var TemplatesTask = _TaamerProContext.TemplatesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId).Select(x => new TemplatesTasksVM
            {
                TemplateTaskId = x.TemplateTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                TemplateId = x.TemplateId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ToUserId = x.ToUserId,
                Notes = x.Notes,
                BranchId = x.BranchId,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase.DescriptionAr,
                UserName = x.Users.FullName,
                SubPhaseName = x.SubPhase.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr,
                ProjectTypeName = x.ProjectSubTypes.ProjectType.NameAr,
                ClientName = x.Project.customer.CustomerNameAr,
                ProjectMangerName = x.Project.Users.FullName,
                TaskTypeName = x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100,
            }).ToList().Select(s => new TemplatesTasksVM()
            {
                TemplateTaskId = s.TemplateTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                TemplateId = s.TemplateId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName,
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                TaskEnd = s.EndDate != null ? s.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
            });

            return TemplatesTask;
        }

        public async Task <IEnumerable<TemplatesTasksVM>> GetAllTemplatesTasksByTemplateId(int TemplateId, int BranchId)
        {
            var TemplatesTask = _TaamerProContext.TemplatesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.TemplateId == TemplateId && s.BranchId == BranchId).Select(x => new TemplatesTasksVM
            {
                TemplateTaskId = x.TemplateTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                TemplateId = x.TemplateId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ToUserId = x.ToUserId,
                Notes = x.Notes,
                BranchId = x.BranchId,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase.DescriptionAr,
                UserName = x.Users.FullName,
                SubPhaseName = x.SubPhase.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes.NameAr,
                ProjectTypeName = x.ProjectSubTypes.ProjectType.NameAr,
                ClientName = x.Project.customer.CustomerNameAr,
                ProjectMangerName = x.Project.Users.FullName,
                TaskTypeName = x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100,
            }).ToList().Select(s => new TemplatesTasksVM()
            {
                TemplateTaskId = s.TemplateTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                TemplateId = s.TemplateId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName,
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                TaskEnd = s.EndDate != null ? s.EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
            });

            return TemplatesTask;
        }

    }
}

    
