using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public class ProjectPhasesTasksRepository : IProjectPhasesTasksRepository
    {

        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectPhasesTasksRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        ///////////////////////TasksSearch///////////////////////
        //EditD6

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectPhasesTasksbygoalandproject2(int projectid, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == projectid && s.ProjectGoals != null).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc=x.EndDateCalc??"",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                  : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                              x.Status == 1 ? " لم تبدأ " :
                              x.Status == 2 ? " قيد التشغيل " :
                              x.Status == 3 ? " متوقفة " :
                              x.Status == 4 ? " منتهية " :
                              x.Status == 5 ? " ملغاة " :
                              x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                PercentComplete = x.PercentComplete,
                ProjectGoals = x.ProjectGoals??0,
                Lastgoal = x.ProjectRequirementsGoals!.RequirementsandGoals!.LineNumber,
                TaskTimeFrom=x.TaskTimeFrom??"",
                TaskTimeTo=x.TaskTimeTo??"",
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,

            }).ToList();

            return projectPhasesTasks;
        }

         
        public async Task<ProjectPhasesTasksVM> GetProjectPhasesTasksbygoalandproject(int projectid,int projectgoal, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false &&  s.ProjectId == projectid &&s.ProjectGoals == projectgoal && s.Status==4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                PercentComplete = x.PercentComplete,
                 ProjectGoals=x.ProjectGoals,
                 Lastgoal=x.ProjectRequirementsGoals!.RequirementsandGoals!.LineNumber,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,

            }).FirstOrDefault();

            return projectPhasesTasks ;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesAndTasks(int projectId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == projectId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,
                NumAdded = x.NumAdded ?? 0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDateCalc ?? "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                  : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                              x.Status == 1 ? " لم تبدأ " :
                              x.Status == 2 ? " قيد التشغيل " :
                              x.Status == 3 ? " متوقفة " :
                              x.Status == 4 ? " منتهية " :
                              x.Status == 5 ? " ملغاة " :
                              x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                PercentComplete = x.PercentComplete,
                ProjectGoals = x.ProjectGoals ?? 0,
                Lastgoal = x.ProjectRequirementsGoals!.RequirementsandGoals!.LineNumber,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,

            }).ToList();
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks(string searchtext, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 /*&& s.BranchId == BranchId*/ && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn  : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType??0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
            });
            if (searchtext != "")
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext ?? "").Trim()) || s.DescriptionEn.Contains((searchtext ?? "").Trim())).ToList();
            }
            return projectPhasesTasks;
        }
         
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_WithB(string searchtext, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
            });
            if (searchtext != "")
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext ?? "").Trim()) || s.DescriptionEn.Contains((searchtext ?? "").Trim())).ToList();
            }
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU(int UserId, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && s.UserId == UserId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
            });

            return projectPhasesTasks;
        }





         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU2(int UserId, int BranchId, string Lang, string DateFrom, string DateTo)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && s.UserId == UserId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
            });

            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksUPage(int UserId, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && s.UserId == UserId && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId=x.SettingId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",

            });

            return projectPhasesTasks;
        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId,int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList)
        {
            if (status == 7)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && BranchesList.Contains(s.BranchId ?? 0) && s.IsMerig == -1 && s.IsRetrieved == 1 && (UserId == null || s.UserId == UserId)).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",

                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo=x.TaskNo??null,
                    //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                                    TaskStart = s.TaskStart,
                                    TaskEnd = s.TaskEnd,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate=s.FirstProjectDate,
                    TimeStrProject=s.TimeStrProject,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo,
                });


                return projectPhasesTasks;

            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && BranchesList.Contains(s.BranchId ?? 0) && s.IsMerig == -1 && (status == null|| s.Status == status) && (UserId == null || s.UserId == UserId)).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDateCalc != null ? x.EndDateCalc : x.EndDate!=null ? x.EndDate:x.ExcpectedEndDate?? "",
                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo = x.TaskNo ?? null,
                    //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                                    TaskStart = s.TaskStart,
                                    TaskEnd = s.EndDate,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate=s.FirstProjectDate,
                    TimeStrProject=s.TimeStrProject,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo,
                });


                return projectPhasesTasks;
            }
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo,string? searchtext)
        {
            if (status == 7)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && s.IsRetrieved == 1 && (UserId == null || s.UserId == UserId) &&
            (s.Project.customer.CustomerNameAr.Contains((searchtext ?? "")) || s.Project.ProjectDescription.Contains((searchtext ?? "")) ||
            s.DescriptionAr.Contains((searchtext ?? "")) || s.Project.ProjectNo.Contains((searchtext ?? "")) || s.Project.ProjectName.Contains((searchtext ?? "")) ||
            s.Project.projecttype.NameAr.Contains((searchtext ?? "")) || searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDateCalc != null ? x.EndDateCalc : x.EndDate != null ? x.EndDate : x.ExcpectedEndDate ?? "",

                ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",

                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                    TaskStart = s.TaskStart,
                    TaskEnd = s.TaskEnd,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate = s.FirstProjectDate,
                    TimeStrProject = s.TimeStrProject,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo,
            });


                return projectPhasesTasks;

            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && (status == null || s.Status == status) && (UserId == null || s.UserId == UserId) &&
            (s.Project.customer.CustomerNameAr.Contains((searchtext ?? "")) || s.Project.ProjectDescription.Contains((searchtext ?? "")) ||
            s.DescriptionAr.Contains((searchtext ?? "")) || s.Project.ProjectNo.Contains((searchtext ?? "")) || s.Project.ProjectName.Contains((searchtext ?? "")) ||
            s.Project.projecttype.NameAr.Contains((searchtext ?? "")) || searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                    TaskStart = s.TaskStart,
                    TaskEnd = s.EndDate,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate = s.FirstProjectDate,
                    TimeStrProject = s.TimeStrProject,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo ?? null,
            });


                return projectPhasesTasks;
            }
        }



        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId,int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList)
        {
            if (status == 7)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && BranchesList.Contains(s.BranchId ?? 0) && s.IsMerig == -1 && s.IsRetrieved == 1 && (UserId == null || s.UserId == UserId)).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo=x.TaskNo??null,
                    //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                                    TaskStart = s.TaskStart,
                                    TaskEnd = s.TaskEnd,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate = s.FirstProjectDate,
                    TimeStrProject = s.TimeStrProject,

                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo,
                });


                return projectPhasesTasks;

            }
            else { 
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && BranchesList.Contains(s.BranchId ?? 0) && s.IsMerig == -1 && s.Status == status && (UserId == null || s.UserId == UserId)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo = x.TaskNo ?? null,
                    //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.TaskStart,
                                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                FirstProjectDate = s.FirstProjectDate,
                TimeStrProject = s.TimeStrProject,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                              s.Status == 1 ? " لم تبدأ " :
                              s.Status == 2 ? " قيد التشغيل " :
                              s.Status == 3 ? " متوقفة " :
                              s.Status == 4 ? " منتهية " :
                              s.Status == 5 ? " ملغاة " :
                              s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                            s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                            s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                            s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                            s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskNo = s.TaskNo,
                });
           

            return projectPhasesTasks;
            }
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo,string? searchtext)
        {
            if (status == 7)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && (BranchId == 0 || s.BranchId == BranchId) && s.IsMerig == -1 && s.IsRetrieved == 1 && (UserId == null || s.UserId == UserId) &&
            (s.Project.customer.CustomerNameAr.Contains((searchtext ?? "")) || s.Project.ProjectDescription.Contains((searchtext ?? "")) ||
            s.DescriptionAr.Contains((searchtext ?? "")) || s.Project.ProjectNo.Contains((searchtext ?? "")) || s.Project.ProjectName.Contains((searchtext ?? "")) ||
            s.Project.projecttype.NameAr.Contains((searchtext ?? "")) || searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo=x.TaskNo??null,
                    //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                    TaskStart = s.TaskStart,
                    TaskEnd = s.TaskEnd,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate = s.FirstProjectDate,
                    TimeStrProject = s.TimeStrProject,

                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo,
                });


                return projectPhasesTasks;

            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && s.Status == status && (UserId == null || s.UserId == UserId) &&
            (s.Project.customer.CustomerNameAr.Contains((searchtext ?? "")) || s.Project.ProjectDescription.Contains((searchtext ?? "")) ||
            s.DescriptionAr.Contains((searchtext ?? "")) || s.Project.ProjectNo.Contains((searchtext ?? "")) || s.Project.ProjectName.Contains((searchtext ?? "")) ||
            s.Project.projecttype.NameAr.Contains((searchtext ?? "")) || searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                    TaskStart = s.TaskStart,
                    TaskEnd = s.TaskEnd,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate = s.FirstProjectDate,
                    TimeStrProject = s.TimeStrProject,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    TaskNo = s.TaskNo,
                });


                return projectPhasesTasks;
            }
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksW(int BranchId, string Lang)
        {
            try { 
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project!.ProjectDescription,
                FirstProjectDate = x.Project!.FirstProjectDate,
                TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",

                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete,
                TaskNo=x.TaskNo??null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.TaskStart,
                                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,
                ProjectDescription=s.ProjectDescription,
                FirstProjectDate = s.FirstProjectDate,
                TimeStrProject = s.TimeStrProject,
                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskNo = s.TaskNo,
            });

            return projectPhasesTasks;
            }
            catch(Exception ex)
            {
                return new List<ProjectPhasesTasksVM>();
            }
        }

        ///////////////////////TasksSearch///////////////////////
        //EditD7
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasks(string EndDateP, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && (s.Status == 2 || s.Status == 1) && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                              x.Status == 1 ? " لم تبدأ " :
                              x.Status == 2 ? " قيد التشغيل " :
                              x.Status == 3 ? " متوقفة " :
                              x.Status == 4 ? " منتهية " :
                              x.Status == 5 ? " ملغاة " :
                              x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                IsRetrieved=x.IsRetrieved,
                PercentComplete = x.PercentComplete,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                EndTime = x.EndTime,
                ProjectTypeId=x.Project.ProjectTypeId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",

                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                //           }).ToList().Where(a => DateTime.ParseExact(a.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(a => (DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(a.EndTime, "h:mm", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()

            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.TaskStart,
                                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                ProjectTypeId=s.ProjectTypeId
                //order by Status desc, orderNOO, PhasePriority desc, Remaining
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }

        /// //////////new Ehab

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksWithoutUser(int ? DepartmentId ,int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 &&  s.IsMerig == -1 &&( s.DepartmentId == DepartmentId || DepartmentId==null) && s.UserId==null).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,

                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                              x.Status == 1 ? " لم تبدأ " :
                              x.Status == 2 ? " قيد التشغيل " :
                              x.Status == 3 ? " متوقفة " :
                              x.Status == 4 ? " منتهية " :
                              x.Status == 5 ? " ملغاة " :
                              x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                ProjectExpireDate = x.Project.ProjectExpireDate,

                IsRetrieved = x.IsRetrieved,
                PercentComplete = x.PercentComplete,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                EndTime = x.EndTime,
                ProjectTypeId = x.Project.ProjectTypeId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",

            }).ToList().Select(s => new ProjectPhasesTasksVM()

            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.TaskStart,
                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                ProjectExpireDate=s.ProjectExpireDate,
                PlayingTime = s.PlayingTime,
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                ProjectTypeId = s.ProjectTypeId,
                TimeStr = s.TimeStr,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTaskswithtime(string EndDateP, int BranchId,string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 &&s.Status !=4 &&// (s.Status == 2 || s.Status == 1) && 
            s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)&&(s.BranchId==BranchId || BranchId==0 || BranchId==null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,

                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                              x.Status == 1 ? " لم تبدأ " :
                              x.Status == 2 ? " قيد التشغيل " :
                              x.Status == 3 ? " متوقفة " :
                              x.Status == 4 ? " منتهية " :
                              x.Status == 5 ? " ملغاة " :
                              x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                IsRetrieved = x.IsRetrieved,
                PercentComplete = x.PercentComplete,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                EndTime = x.EndTime,
                ProjectTypeId = x.Project.ProjectTypeId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                StartDateNew=x.StartDateNew,
                EndDateNew=x.EndDateNew,
                TaskNo=x.TaskNo??null,

                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                //           }).ToList().Where(a => DateTime.ParseExact(a.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(a => (DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(a.EndTime, "h:mm", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()

            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.TaskStart,
                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,

                PlayingTime = s.PlayingTime,
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                ProjectTypeId = s.ProjectTypeId,
                TimeStr=s.TimeStr,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                StartDateNew = s.StartDateNew,
                EndDateNew = s.EndDateNew,
                TaskNo = s.TaskNo,
                //order by Status desc, orderNOO, PhasePriority desc, Remaining
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTaskswithtime(string EndDateP, int BranchId,string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1 && s.Status != 4 && s.Remaining < 0
            &&(s.BranchId==BranchId || BranchId==0 || BranchId==null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                                  x.Status == 1 ? " لم تبدأ " :
                                  x.Status == 2 ? " قيد التشغيل " :
                                  x.Status == 3 ? " متوقفة " :
                                  x.Status == 4 ? " منتهية " :
                                  x.Status == 5 ? " ملغاة " :
                                  x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",

                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.PercentComplete,
                EndTime = x.EndTime,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsRetrieved = x.IsRetrieved,
                StartDateNew = x.StartDateNew,
                EndDateNew = x.EndDateNew,
                TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,

                PlayingTime = s.PlayingTime,
                EndTime = s.EndTime,
                IsRetrieved = s.IsRetrieved,
                TimeStr=s.TimeStr,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                StartDateNew = s.StartDateNew,
                EndDateNew = s.EndDateNew,
                TaskNo = s.TaskNo,
            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasksbyprojectid(string EndDateP, int BranchId,int projectid)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.ProjectId== projectid && (s.Status == 2 || s.Status == 1) && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                              x.Status == 1 ? " لم تبدأ " :
                              x.Status == 2 ? " قيد التشغيل " :
                              x.Status == 3 ? " متوقفة " :
                              x.Status == 4 ? " منتهية " :
                              x.Status == 5 ? " ملغاة " :
                              x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                IsRetrieved = x.IsRetrieved,
                PercentComplete = x.PercentComplete,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                EndTime = x.EndTime,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                StartDateNew = x.StartDateNew,
                EndDateNew = x.EndDateNew,
                TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                //           }).ToList().Where(a => DateTime.ParseExact(a.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(a => (DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(a.EndTime, "h:mm", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()

            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.TaskStart,
                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                StartDateNew = s.StartDateNew,
                EndDateNew = s.EndDateNew,
                TaskNo = s.TaskNo,
                //order by Status desc, orderNOO, PhasePriority desc, Remaining
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }




        ///////////////////////TasksSearch///////////////////////





         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasksByUserId(string EndDateP, int BranchId, int USERID)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && (s.Status == 2 || s.Status == 1) && s.UserId == USERID && s.IsMerig == -1 && s.Remaining > 0).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                                   x.Status == 1 ? " لم تبدأ " :
                                   x.Status == 2 ? " قيد التشغيل " :
                                   x.Status == 3 ? " متوقفة " :
                                   x.Status == 4 ? " منتهية " :
                                   x.Status == 5 ? " ملغاة " :
                                   x.Status == 6 ? " محذوفة ":x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                PercentComplete = x.PercentComplete,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                EndTime = x.EndTime,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                IsRetrieved =x.IsRetrieved,
                TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                //           }).ToList().Where(a => DateTime.ParseExact(a.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(a => (DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(a.EndTime, "h:mm", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(a.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()

            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,

                PlayingTime = s.PlayingTime,
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                IsRetrieved = s.IsRetrieved,
                TaskNo = s.TaskNo,
            }).ToList();
            return projectPhasesTasks;
        }

        //EditD8
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasks(string EndDateP, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1 && s.Status != 4 && s.Remaining < 0).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                                  x.Status == 1 ? " لم تبدأ " :
                                  x.Status == 2 ? " قيد التشغيل " :
                                  x.Status == 3 ? " متوقفة " :
                                  x.Status == 4 ? " منتهية " :
                                  x.Status == 5 ? " ملغاة " :
                                  x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.PercentComplete,
                EndTime = x.EndTime,
                IsRetrieved=x.IsRetrieved,
                TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
                EndTime = s.EndTime,
                IsRetrieved=s.IsRetrieved,
                TaskNo = s.TaskNo,
            }).ToList();
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId(string EndDateP, int BranchId, int UserId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1 && s.Status != 4 && s.UserId == UserId && s.Remaining < 0).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                                x.Status == 1 ? " لم تبدأ " :
                                x.Status == 2 ? " قيد التشغيل " :
                                x.Status == 3 ? " متوقفة " :
                                x.Status == 4 ? " منتهية " :
                                x.Status == 5 ? " ملغاة " :
                                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.PercentComplete,
                EndTime = x.EndTime,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsRetrieved=x.IsRetrieved,
                TaskNo = x.TaskNo ?? null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
                EndTime = s.EndTime,
                IsConverted = s.IsConverted,
                PlusTime = s.PlusTime,
                TaskNo = s.TaskNo,
            }).ToList();
            return projectPhasesTasks;
        }




         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId,string Lang, List<int> BranchesList)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && BranchesList.Contains(s.BranchId ?? 0) && s.Type == 3 && s.IsMerig == -1 && s.Status != 4 && ( UserId==null || s.UserId == UserId) && s.Remaining < 0).Select(x => new ProjectPhasesTasksVM
            {
             PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project!.ProjectDescription,
                FirstProjectDate = x.Project!.FirstProjectDate,
                TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete,
                TaskNo=x.TaskNo??null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                FirstProjectDate = s.FirstProjectDate,
                TimeStrProject = s.TimeStrProject,

                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskNo = s.TaskNo,
            }).ToList();
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang,string? searchtext)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1 && s.Status != 4 && (UserId == null || s.UserId == UserId) && s.Remaining < 0 &&
            (s.Project.customer.CustomerNameAr.Contains((searchtext ?? "")) || s.Project.ProjectDescription.Contains((searchtext ?? "")) ||
            s.DescriptionAr.Contains((searchtext ?? "")) || s.Project.ProjectNo.Contains((searchtext ?? "")) || s.Project.ProjectName.Contains((searchtext ?? "")) ||
            s.Project.projecttype.NameAr.Contains((searchtext ?? "")) || searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project!.ProjectDescription,
                FirstProjectDate = x.Project!.FirstProjectDate,
                TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.PercentComplete,
                TaskNo=x.TaskNo??null,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
            }).ToList().Select(s => new ProjectPhasesTasksVM()//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                FirstProjectDate = s.FirstProjectDate,
                TimeStrProject = s.TimeStrProject,

                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskNo = s.TaskNo,
            }).ToList();
            return projectPhasesTasks;
        }

        ///////////////////////TasksSearch///////////////////////
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksByUserId(string searchtext, int? UserId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,

                PlayingTime = x.TimeMinutes - x.Remaining,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TaskNo = x.TaskNo ?? null,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
                TaskNo = s.TaskNo,
            });
            if (searchtext != "")
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim())).ToList();
            }
            return projectPhasesTasks;
        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks2(string searchtext, int BranchId, string Lang)
        {

            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete =x.TimeMinutes!=0?x.TimeMinutes!=null?(((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100:0:0,
                TaskNo = x.TaskNo ?? null,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskNo = s.TaskNo,
            });
            if (searchtext != "")
            {
                //projectPhasesTasks = projectPhasesTasks.Where(s => s.ProjectNumber.Equals(searchtext.Trim())).ToList();
                projectPhasesTasks = projectPhasesTasks.Where(s => s.ProjectNumber == searchtext.Trim()).ToList();
                //projectPhasesTasks = projectPhasesTasks.Where(s => s.ProjectNumber.Equals(searchtext.Trim())).ToList();

            }
            var running = projectPhasesTasks.Where(x => x.Status == 2 || x.Status == 1).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining); 
            var stopped = projectPhasesTasks.Where(x => x.Status == 3 || x.Status == 4).ToList().OrderBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            var total = running.Union(stopped);
            return total;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectPhasesTasks2(int ProjectId, int BranchId, string Lang)
        {

            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId== ProjectId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TaskNo=x.TaskNo??null,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskNo = s.TaskNo,
            });
            var running = projectPhasesTasks.Where(x => x.Status == 2 || x.Status == 1).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            var stopped = projectPhasesTasks.Where(x => x.Status == 3 || x.Status == 4).ToList().OrderBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            var total = running.Union(stopped);
            return total;
        }

        ///////////////////////TasksSearch///////////////////////
        //time
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks(string searchtext, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == 2 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                TaskLongDesc = x.TaskLongDesc,
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",


                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlusTime = x.PlusTime,
                SettingId=x.SettingId,
                IsConverted= x.IsConverted
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                PhasesTaskName = s.PhasesTaskName,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                TaskLongDesc = s.TaskLongDesc,
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",

                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,
                StatusName = s.Status == 0 ? "غير معلومة" :
                         s.Status == 1 ? " لم تبدأ " :
                         s.Status == 2 ? " قيد التشغيل " :
                         s.Status == 3 ? " متوقفة " :
                         s.Status == 4 ? " منتهية " :
                         s.Status == 5 ? " ملغاة " :
                         s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlusTime = s.PlusTime,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                IsConverted = s.IsConverted

            });
            if (searchtext != "")
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim())).ToList();
            }
            return projectPhasesTasks.ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining); 
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == 2 && s.IsMerig == -1 && s.BranchId==BranchId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                TaskLongDesc = x.TaskLongDesc,
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = Lang == "rtl" ? x.Users!.FullNameAr==null ? x.Users!.FullName : x.Users!.FullNameAr : x.Users!.FullName==null ? x.Users!.FullNameAr : x.Users!.FullName ??"",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",


                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlusTime = x.PlusTime,
                SettingId = x.SettingId,
                IsConverted = x.IsConverted
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                PhasesTaskName = s.PhasesTaskName,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                TaskLongDesc = s.TaskLongDesc,
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",

                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,
                StatusName = s.Status == 0 ? "غير معلومة" :
                         s.Status == 1 ? " لم تبدأ " :
                         s.Status == 2 ? " قيد التشغيل " :
                         s.Status == 3 ? " متوقفة " :
                         s.Status == 4 ? " منتهية " :
                         s.Status == 5 ? " ملغاة " :
                         s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlusTime = s.PlusTime,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                IsConverted = s.IsConverted

            });
            return projectPhasesTasks.ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
        }



        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branchesfilterd(int BranchId, string Lang,int? CustomerId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == 2 && s.IsMerig == -1 && s.BranchId == BranchId
            &&(CustomerId ==null || CustomerId==0 || s.Project.CustomerId== CustomerId)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                TaskLongDesc = x.TaskLongDesc,
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = Lang == "rtl" ? x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr : x.Users!.FullName == null ? x.Users!.FullNameAr : x.Users!.FullName ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",


                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlusTime = x.PlusTime,
                SettingId = x.SettingId,
                IsConverted = x.IsConverted,
                ContactLists = x.ContactLists.Where(x => x.IsDeleted == false).ToList(),

            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                PhasesTaskName = s.PhasesTaskName,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                TaskLongDesc = s.TaskLongDesc,
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",

                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,
                StatusName = s.Status == 0 ? "غير معلومة" :
                         s.Status == 1 ? " لم تبدأ " :
                         s.Status == 2 ? " قيد التشغيل " :
                         s.Status == 3 ? " متوقفة " :
                         s.Status == 4 ? " منتهية " :
                         s.Status == 5 ? " ملغاة " :
                         s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlusTime = s.PlusTime,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                IsConverted = s.IsConverted

            });
            return projectPhasesTasks.ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branchesfilterd(int BranchId, string Lang, int? CustomerId,string? searchtext)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == 2 && s.IsMerig == -1 && s.BranchId == BranchId
            && (CustomerId == null || CustomerId == 0 || s.Project.CustomerId == CustomerId) && (s.DescriptionAr.Contains((searchtext??"")) ||
            s.Project.customer.CustomerNameAr.Contains((searchtext??"")) || s.Users.FullNameAr.Contains((searchtext??"")) ||
            searchtext ==null || searchtext=="")).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                TaskLongDesc = x.TaskLongDesc,
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = Lang == "rtl" ? x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr : x.Users!.FullName == null ? x.Users!.FullNameAr : x.Users!.FullName ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",


                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlusTime = x.PlusTime,
                SettingId = x.SettingId,
                IsConverted = x.IsConverted,
                ContactLists = x.ContactLists.Where(x => x.IsDeleted == false).ToList(),
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                PhasesTaskName = s.PhasesTaskName,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                TaskLongDesc = s.TaskLongDesc,
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",

                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,
                StatusName = s.Status == 0 ? "غير معلومة" :
                         s.Status == 1 ? " لم تبدأ " :
                         s.Status == 2 ? " قيد التشغيل " :
                         s.Status == 3 ? " متوقفة " :
                         s.Status == 4 ? " منتهية " :
                         s.Status == 5 ? " ملغاة " :
                         s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlusTime = s.PlusTime,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                IsConverted = s.IsConverted,
                ContactLists = s.ContactLists.Where(x => x.IsDeleted == false).ToList(),

            });
            return projectPhasesTasks.ToList().OrderByDescending(x => x.IsNew).ThenBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome(string searchtext, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == 2 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,


                //edit
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",


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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskLongDesc = x.TaskLongDesc,
                SettingId = x.SettingId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                PhasesTaskName = s.PhasesTaskName,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,



                //edit
                TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",

                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                UserName = s.UserName ?? "",
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,
                SettingId = s.SettingId,
                TaskLongDesc = s.TaskLongDesc,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
            });
            if (searchtext != "")
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim())).ToList();
            }
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome_Search(ProjectPhasesTasksVM Search , string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (Search.BranchId==0 ||s.BranchId==Search.BranchId) && s.Type == 3 && s.Status== 2 /*(s.Status == 1 || s.Status == 2)*/ && s.IsMerig == -1 && s.Remaining >= 0 && s.UserId == Search.UserId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,


                //edit
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",


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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                TaskLongDesc = x.TaskLongDesc,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList();
            
            if(!string.IsNullOrEmpty(Search.StartDate)&& !string.IsNullOrEmpty(Search.EndDate))
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => (string.IsNullOrEmpty(s.StartDate) || 
                (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) &&
                                                       DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture)) )).ToList();
            }
            return projectPhasesTasks.OrderByDescending(x => x.Status);
        }
        public async Task<decimal?> GetProjectPhasesTasksCountByStatus(int? UserId, int Status, int BranchId)
        {
            if (UserId == null)
            {
                // var value = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status);
                // var SumValue = value.Sum(x => x.PercentComplete);
                var Count = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.IsMerig == -1).Count();
                var result = Count;

                return result;
            }
            else
            {
                //return _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId ).Sum(x => x.PercentComplete) / _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId ).Count();

                var Value1 = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId && s.IsMerig == -1);
                var val2 = Value1.Sum(x => x.PercentComplete);
                var count = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId && s.IsMerig == -1).Count();
                //var Result = val2 / count;
                //return Result;
                decimal result = 0;
                if (count != 0)
                {
                    result = (val2 / count) * 100 ?? 0;

                }
                return result;

            }
        }
        public async Task<decimal?> GetProjectPhasesTasksCountByStatusPercent(int? UserId, int Status, int BranchId)
        {
            if (UserId == null)
            {
                decimal AllTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1).Count();
                decimal UserTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.IsMerig == -1).Count();
                decimal result = 0;
                if (AllTask != 0)
                {
                    result = (UserTasks / AllTask) * 100;

                }
                return result;
            }
            else
            {
                decimal AllTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1).Count();
                decimal UserTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId && s.IsMerig == -1).Count();
                //decimal result = (UserTasks / AllTask) * 100;
                //return result;
                decimal result = 0;
                if (AllTask != 0)
                {
                    result = (UserTasks / AllTask) * 100;

                }
                return result;
                //var Value1 = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId );
                //var val2=Value1.Sum(x => x.PercentComplete);
                //var count = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == Status && s.UserId == UserId ).Count();
                //var Result = val2 / count;
                //return Result;

            }
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserId(int? UserId, int Status, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && (s.Status == Status || (Status == 0 && s.Status != 4)) && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,

                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month ",

                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Cost = x.Cost,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlayingTime = x.TimeMinutes - x.Remaining,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100,
            }).ToList().OrderBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining).ThenBy(x=> x.Status);
            //if (searchtext != "")
            //{
            //    projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim()) || s.ProjectNumber.Equals(searchtext.Trim())).ToList();
            //}
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId,int? ProjectId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status != 4 && s.IsMerig == -1 && s.Remaining >= 0 &&
             (UserId.HasValue? s.UserId == UserId : true) && (ProjectId.HasValue? s.ProjectId == ProjectId : true) &&
             (CustomerId.HasValue? s.Project.CustomerId == CustomerId.Value : true)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,

                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month ",

                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Cost = x.Cost,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                 UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                 OrderNo = x.OrderNo  ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                 EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                 TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                 ClientName_W = x.Project!.customer!.CustomerNameAr,
                 ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                 ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                 TaskLongDesc = x.TaskLongDesc,
                 StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
               IsRetrieved=x.IsRetrieved,
                 TaskTimeFrom = x.TaskTimeFrom ?? "",
                 TaskTimeTo = x.TaskTimeTo ?? "",
                 PlayingTime = x.TimeMinutes - x.Remaining,
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            //projectPhasesTasks = projectPhasesTasks.OrderBy(s => s.Remaining).ToList();
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdCustomerIdProjectIdwithtime(int? UserId, int? CustomerId, int? ProjectId,int? BrancId,string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status != 4 && s.IsMerig == -1 && (s.Remaining >= 0 ||s.Remaining==null) &&
             (UserId.HasValue ? s.UserId == UserId : true) && (ProjectId.HasValue ? s.ProjectId == ProjectId : true) &&
             (CustomerId.HasValue ? s.Project.CustomerId == CustomerId.Value : true)&&
              (BrancId.HasValue ? s.BranchId == BrancId.Value : true)).Select(x => new ProjectPhasesTasksVM
             {
                 PhaseTaskId = x.PhaseTaskId,
                 DescriptionAr = x.DescriptionAr,
                 DescriptionEn = x.DescriptionEn,
                 ParentId = x.ParentId,
                 ProjSubTypeId = x.ProjSubTypeId,
                 Type = x.Type,
                 UserId = x.UserId,NumAdded=x.NumAdded??0,
                 ProjectId = x.ProjectId,
                 TimeMinutes = x.TimeMinutes,
                 TimeType = x.TimeType,


                 Remaining = x.Remaining,
                 IsUrgent = x.IsUrgent,
                 IsTemp = x.IsTemp,
                 TaskType = x.TaskType,
                 Cost = x.Cost,
                 Status = x.Status,
                 OldStatus = x.OldStatus,
                 Active = x.Active,
                 StopCount = x.StopCount,
                 UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                 OrderNo = x.OrderNo ?? 1000000,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 ExcpectedStartDate = x.ExcpectedStartDate,
                 ExcpectedEndDate = x.ExcpectedEndDate,
                 EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                 TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                 ToUserId = x.ToUserId,
                 Notes = x.Notes ?? "",
                 BranchId = x.BranchId,
                 PhasePriority = x.PhasePriority,
                 ExecPercentage = x.ExecPercentage,
                 //MainPhaseName = x.MainPhase!.DescriptionAr,
                 SubPhaseName = x.SubPhase!.DescriptionAr,
                 ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                 ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                 ClientName_W = x.Project!.customer!.CustomerNameAr,
                 IsConverted = x.IsConverted,
                 PlusTime = x.PlusTime,
                 ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                 ProjectMangerName = x.Project!.Users!.FullName,
                 ProjectNumber = x.Project!.ProjectNo,
                 TaskLongDesc = x.TaskLongDesc,
                 StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                 StopProjectType = x.Project.StopProjectType ?? 0,
                 IsRetrieved = x.IsRetrieved,
                 TaskTimeFrom = x.TaskTimeFrom ?? "",
                 TaskTimeTo = x.TaskTimeTo ?? "",
                 PlayingTime = x.TimeMinutes - x.Remaining,
                 TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                 TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                  StartDateNew = x.StartDateNew,
                  EndDateNew = x.EndDateNew,
                  TaskNo = x.TaskNo ?? null,
              }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            //projectPhasesTasks = projectPhasesTasks.OrderBy(s => s.Remaining).ToList();
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdCustomerIdProjectIdwithtime(int? UserId, int? CustomerId, int? ProjectId,int? BrancId,string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Remaining < 0 && s.Status != 4 && s.IsMerig == -1 &&
             (UserId.HasValue ? s.UserId == UserId : true) && (ProjectId.HasValue ? s.ProjectId == ProjectId : true) &&
             (CustomerId.HasValue ? s.Project.CustomerId == CustomerId.Value : true)&&
              (BrancId.HasValue ? s.BranchId == BrancId.Value : true)).Select(x => new ProjectPhasesTasksVM
             {
                 PhaseTaskId = x.PhaseTaskId,
                 DescriptionAr = x.DescriptionAr,
                 DescriptionEn = x.DescriptionEn,
                 ParentId = x.ParentId,
                 ProjSubTypeId = x.ProjSubTypeId,
                 Type = x.Type,
                 UserId = x.UserId,NumAdded=x.NumAdded??0,
                 ProjectId = x.ProjectId,
                 TimeMinutes = x.TimeMinutes,
                 TimeType = x.TimeType,
                 Remaining = x.Remaining,
                 IsUrgent = x.IsUrgent,
                 IsTemp = x.IsTemp,
                 TaskType = x.TaskType,
                 Cost = x.Cost,
                 Status = x.Status,
                 OldStatus = x.OldStatus,
                 Active = x.Active,
                 StopCount = x.StopCount,
                 UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                 OrderNo = x.OrderNo,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 ExcpectedStartDate = x.ExcpectedStartDate,
                 ExcpectedEndDate = x.ExcpectedEndDate,
                 EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                 TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                 ToUserId = x.ToUserId,
                 Notes = x.Notes ?? "",
                 BranchId = x.BranchId,
                 PhasePriority = x.PhasePriority,
                 ExecPercentage = x.ExecPercentage,
                 //MainPhaseName = x.MainPhase!.DescriptionAr,
                 SubPhaseName = x.SubPhase!.DescriptionAr,
                 ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                 ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                 ClientName_W = x.Project!.customer!.CustomerNameAr,
                 ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                 ProjectMangerName = x.Project!.Users!.FullName,
                 ProjectNumber = x.Project!.ProjectNo,
                 TaskLongDesc = x.TaskLongDesc,
                 StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                 StopProjectType = x.Project.StopProjectType ?? 0,
                 SettingId = x.SettingId,
                 PlayingTime = x.TimeMinutes - x.Remaining,
                 IsRetrieved = x.IsRetrieved,
                 IsConverted = x.IsConverted,
                 PlusTime = x.PlusTime,
                 TaskTimeFrom = x.TaskTimeFrom ?? "",
                 TaskTimeTo = x.TaskTimeTo ?? "",
                 TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                 TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                 StartDateNew=x.StartDateNew,
                 EndDateNew=x.EndDateNew,
                  TaskNo = x.TaskNo ?? null,
              }).ToList();
            projectPhasesTasks = projectPhasesTasks.OrderBy(s => s.Remaining).ToList();
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdHome(int? UserId, int Status, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && (s.Status == Status || Status == 0) && s.IsRead == false && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month ",
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Cost = x.Cost,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PlayingTime = x.TimeMinutes - x.Remaining,
                //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100,
            });
            //if (searchtext != "")
            //{
            //    projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim()) || s.ProjectNumber.Equals(searchtext.Trim())).ToList();
            //}
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdUser(int? UserId, string lang, int Status, int BranchId)
        {

            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Cost = x.Cost,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                  x.Status == 1 ? " لم تبدأ " :
                  x.Status == 2 ? " قيد التشغيل " :
                  x.Status == 3 ? " متوقفة " :
                  x.Status == 4 ? " منتهية " :
                  x.Status == 5 ? " ملغاة " :
                  x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                PhasesTaskName = s.PhasesTaskName,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                UserName = s.UserName ?? "",
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo ?? 1000000,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = //s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية "
                            //s.Status == 5 ? " ملغاة " :
                            //s.Status == 6 ? " محذوفة " 
                            : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);

            if (Status == 0)
            {
                projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    PhasesTaskName = lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Cost = x.Cost,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    OrderNo = x.OrderNo ?? 1000000,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                  x.Status == 1 ? " لم تبدأ " :
                  x.Status == 2 ? " قيد التشغيل " :
                  x.Status == 3 ? " متوقفة " :
                  x.Status == 4 ? " منتهية " :
                  x.Status == 5 ? " ملغاة " :
                  x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                }).ToList().Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    PhasesTaskName = s.PhasesTaskName,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
                    ProjectId = s.ProjectId,
                    TimeMinutes = s.TimeMinutes,
                    TimeType = s.TimeType,
                    Remaining = s.Remaining,
                    IsUrgent = s.IsUrgent,
                    UserName = s.UserName ?? "",
                    IsTemp = s.IsTemp,
                    TaskType = s.TaskType,
                    Status = s.Status,
                    OldStatus = s.OldStatus,
                    Active = s.Active,
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                                    TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                    TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = //s.Status == 0 ? "غير معلومة" :
                                s.Status == 1 ? " لم تبدأ " :
                                s.Status == 2 ? " قيد التشغيل " :
                                s.Status == 3 ? " متوقفة " :
                                s.Status == 4 ? " منتهية "
                                //s.Status == 5 ? " ملغاة " :
                                //s.Status == 6 ? " محذوفة " 
                                : " تم تحويلها",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    PlayingTime = s.PlayingTime,
                }).ToList().OrderBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);

                var Late = projectPhasesTasks.Where(x => x.Remaining < 0).ToList().OrderByDescending(x => x.PhasePriority).ThenByDescending(x => x.PhasePriority).ToList();
                var Rest = projectPhasesTasks.Where(x => x.Remaining >= 0 || !x.Remaining.HasValue).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining).ToList();
                var total = Late.Union(Rest);
                return total;
            }
            else
            {
                projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status == Status && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    PhasesTaskName = lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Cost = x.Cost,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    OrderNo = x.OrderNo ?? 1000000,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                }).ToList().Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    PhasesTaskName = s.PhasesTaskName,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
                    ProjectId = s.ProjectId,
                    TimeMinutes = s.TimeMinutes,
                    TimeType = s.TimeType,
                    Remaining = s.Remaining,
                    IsUrgent = s.IsUrgent,
                    UserName = s.UserName ?? "",
                    IsTemp = s.IsTemp,
                    TaskType = s.TaskType,
                    Status = s.Status,
                    OldStatus = s.OldStatus,
                    Active = s.Active,
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo ?? 1000000,
                                    TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                    TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = //s.Status == 0 ? "غير معلومة" :
                                s.Status == 1 ? " لم تبدأ " :
                                s.Status == 2 ? " قيد التشغيل " :
                                s.Status == 3 ? " متوقفة " :
                                s.Status == 4 ? " منتهية "
                                //s.Status == 5 ? " ملغاة " :
                                //s.Status == 6 ? " محذوفة " 
                                : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",

                }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);

                if (Status == 2)
                {
                    projectPhasesTasks = projectPhasesTasks.OrderBy(x => x.Remaining).ThenBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority);
                }
                //.OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining); 
            }


            //if (searchtext != "")
            //{
            //    projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim()) || s.ProjectNumber.Equals(searchtext.Trim())).ToList();
            //}
            return projectPhasesTasks;//.OrderByDescending(x=> x.PhasePriority);
        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserId(int? UserId, int Status, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId

            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId
            });
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Remaining < 0 && s.Status != 4 && s.IsMerig == -1 &&
             (UserId.HasValue ? s.UserId == UserId : true) && (ProjectId.HasValue ? s.ProjectId == ProjectId : true) &&
             (CustomerId.HasValue ? s.Project.CustomerId == CustomerId.Value : true)).Select(x => new ProjectPhasesTasksVM
             {
                 PhaseTaskId = x.PhaseTaskId,
                 DescriptionAr = x.DescriptionAr,
                 DescriptionEn = x.DescriptionEn,
                 ParentId = x.ParentId,
                 ProjSubTypeId = x.ProjSubTypeId,
                 Type = x.Type,
                 UserId = x.UserId,NumAdded=x.NumAdded??0,
                 ProjectId = x.ProjectId,
                 TimeMinutes = x.TimeMinutes,
                 TimeType = x.TimeType,

                 TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month ",

                 Remaining = x.Remaining,
                 IsUrgent = x.IsUrgent,
                 IsTemp = x.IsTemp,
                 TaskType = x.TaskType,
                 Cost = x.Cost,
                 Status = x.Status,
                 OldStatus = x.OldStatus,
                 Active = x.Active,
                 StopCount = x.StopCount,
                 UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                 OrderNo = x.OrderNo,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 ExcpectedStartDate = x.ExcpectedStartDate,
                 ExcpectedEndDate = x.ExcpectedEndDate,
                 EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                 TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                 ToUserId = x.ToUserId,
                 Notes = x.Notes ?? "",
                 BranchId = x.BranchId,
                 PhasePriority = x.PhasePriority,
                 ExecPercentage = x.ExecPercentage,
                 //MainPhaseName = x.MainPhase!.DescriptionAr,
                 SubPhaseName = x.SubPhase!.DescriptionAr,
                 ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                 ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                 ClientName_W = x.Project!.customer!.CustomerNameAr,
                 ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                 ProjectMangerName = x.Project!.Users!.FullName,
                 ProjectNumber = x.Project!.ProjectNo,
                 TaskLongDesc = x.TaskLongDesc,
                 StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                 StopProjectType = x.Project.StopProjectType ?? 0,
                 SettingId=x.SettingId,
                 PlayingTime = x.TimeMinutes - x.Remaining,
                 IsRetrieved=x.IsRetrieved,
                 TaskTimeFrom = x.TaskTimeFrom ?? "",
                 TaskTimeTo = x.TaskTimeTo ?? "",
                 TaskNo = x.TaskNo ?? null,
             }).ToList();
            projectPhasesTasks = projectPhasesTasks.OrderBy(s => s.Remaining).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHome(string EndDateP, int? UserId, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                             PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
               EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

               ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
               ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
               ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

               ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

               TaskLongDesc = x.TaskLongDesc,
               PlayingTime = x.TimeMinutes - x.Remaining,
               StopProjectType = x.Project.StopProjectType ?? 0,
               SettingId=x.SettingId,
               PercentComplete = x.PercentComplete,
               IsRetrieved=x.IsRetrieved,
               AskDetails=x.AskDetails??0,
               IsConverted = x.IsConverted,
               PlusTime = x.PlusTime,
               IsNew = x.IsNew ?? 0,
               TaskTimeFrom = x.TaskTimeFrom ?? "",
               TaskTimeTo = x.TaskTimeTo ?? "",
                TaskNo = x.TaskNo ?? null,
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList();//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return projectPhasesTasks;
        }



        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang,int? ProjectId,int? CustomerId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4
             && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                PercentComplete = x.PercentComplete,
                IsRetrieved = x.IsRetrieved,
                AskDetails = x.AskDetails ?? 0,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsNew = x.IsNew ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskNo = x.TaskNo ?? null,
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList();//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return projectPhasesTasks;
        }





        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang, int? ProjectId, int? CustomerId,string? searchtext)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4
             && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId) && (s.DescriptionAr.Contains((searchtext??"")) || s.Project.ProjectNo.Contains((searchtext ?? ""))
            )).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                PercentComplete = x.PercentComplete,
                IsRetrieved = x.IsRetrieved,
                AskDetails = x.AskDetails ?? 0,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsNew = x.IsNew ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskNo=x.TaskNo??null,
                ContactLists = x.ContactLists.Where(x => x.IsDeleted == false).ToList(),
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList();//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHome_Search(ProjectPhasesTasksVM Search, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == Search.UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                DescriptionAr = x.DescriptionAr,
                ProjectNumber = x.Project!.ProjectNo,
                Remaining = x.Remaining,
                EndTime = x.EndTime,
                TaskFullTime = x.TaskFullTime,
                ExecPercentage = x.ExecPercentage,
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                TaskLongDesc = x.TaskLongDesc,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskNo = x.TaskNo ?? null,
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList().Where(s => (string.IsNullOrEmpty(s.EndDateCalc) || (!string.IsNullOrEmpty(s.EndDateCalc) && (DateTime.ParseExact(s.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Search.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) ) ) )&&
                                   (string.IsNullOrEmpty(s.StartDate) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(Search.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))));
            return projectPhasesTasks;
        }
        public async Task<decimal?> GetLateTasksByUserIdCount(string EndDateP, int? UserId, int BranchId, string Lang)
        {
            decimal projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                EndDate = x.EndDate,
                EndTime = x.EndTime,

            }).Count();//.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Count();
            decimal AllTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.IsMerig == -1).Count();
            decimal result = 0;
            if (AllTasks != 0)
            {
                result = (projectPhasesTasks / AllTasks) * 100;
            }
            return result;
        }
        //edit day week month
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetDayWeekMonth_Tasks(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate)
        {
            if (Flag == 1)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Cost = x.Cost,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    OrderNo = x.OrderNo,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    SettingId = x.SettingId,
                    IsRetrieved=x.IsRetrieved,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    TaskLongDesc = x.TaskLongDesc,
                    AddDate = x.AddDate ?? DateTime.Now,
                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                    //}).ToList().Where(m => DateTime.Now >= DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                }).ToList().Where(m => DateTime.Now >= m.AddDate);


                return projectPhasesTasks;
            }
            else if (Flag == 2)
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Cost = x.Cost,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    OrderNo = x.OrderNo,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    SettingId = x.SettingId,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    TaskLongDesc = x.TaskLongDesc,
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    AddDate = x.AddDate ?? DateTime.Now,
                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                    //}).ToList().Where(m => DateTime.Now > DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(6));
                    //}).ToList().Where(m => DateTime.Now > DateTime.ParseExact(m.AddDate.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(6));

                }).ToList().Where(m => DateTime.Now > m.AddDate.AddDays(6));
                return projectPhasesTasks;
            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Cost = x.Cost,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    OrderNo = x.OrderNo,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    SettingId = x.SettingId,

                    TaskLongDesc = x.TaskLongDesc,
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    AddDate = x.AddDate ?? DateTime.Now,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                    //}).ToList().Where(m => DateTime.Now > DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddDays(29));
                }).ToList().Where(m => DateTime.Now > m.AddDate.AddDays(29));
                return projectPhasesTasks;
            }

        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByDate(string StartDate, string EndDate, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Cost = x.Cost,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                TaskNo = x.TaskNo ?? null,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Where(m => DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                UserName = s.UserName ?? "",
                TaskTypeName = s.TaskTypeName,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                Cost = s.Cost,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskLongDesc = s.TaskLongDesc,
                PlayingTime = s.PlayingTime,
                TaskNo = s.TaskNo,
            }); ;
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByDateByUserId(string StartDate, string EndDate, int? UserId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Cost = x.Cost,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                TaskNo = x.TaskNo ?? null,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Where(m => DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                UserName = s.UserName ?? "",
                TaskTypeName = s.TaskTypeName,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                Cost = s.Cost,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                SettingId = s.SettingId,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                TaskLongDesc = s.TaskLongDesc,
                PlayingTime = s.PlayingTime,
                TaskNo = s.TaskNo,
            });
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId(string EndDateP, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null)
        {
           var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                       x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                       x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
               EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

               ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
               UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
               SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
               ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
               ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

               ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

               TaskLongDesc = x.TaskLongDesc,
               PlayingTime = x.TimeMinutes - x.Remaining,
               StopProjectType = x.Project.StopProjectType ?? 0,
               SettingId=x.SettingId,
               PercentComplete = x.PercentComplete,
               IsRetrieved=x.IsRetrieved,
               AskDetails=x.AskDetails??0,
               IsConverted = x.IsConverted,
               PlusTime = x.PlusTime,
               IsNew = x.IsNew ?? 0,
               TaskTimeFrom = x.TaskTimeFrom ?? "",
               TaskTimeTo = x.TaskTimeTo ?? "",
               TaskNo = x.TaskNo ?? null,
           })
                .ToList().OrderByDescending(x => x.IsNew).ThenBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining); 
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang,int? ProjectId,int? CustomerId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)
            &&(ProjectId == null || ProjectId == 0 || s.ProjectId==ProjectId) && (CustomerId == null || CustomerId == 0
            ||s.Project.CustomerId==CustomerId)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                PercentComplete = x.PercentComplete,
                IsRetrieved = x.IsRetrieved,
                AskDetails = x.AskDetails ?? 0,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsNew = x.IsNew ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                ProjectDescription=x.Project.ProjectDescription??"",
                TaskNo=x.TaskNo??null,
                
            })
                 .ToList().OrderByDescending(x => x.IsNew).ThenBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }



        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang, int? ProjectId, int? CustomerId,string? searchtext)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)
            && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && (CustomerId == null || CustomerId == 0
            || s.Project.CustomerId == CustomerId) && (s.DescriptionAr.Contains((searchtext??"")) || s.DescriptionEn.Contains((searchtext??""))||
            s.Project.customer.CustomerNameAr.Contains((searchtext??"")) || s.Project.ProjectNo.Contains((searchtext??"")) ||
            s.Project.ProjectDescription.Contains((searchtext??"")) || s.ProjectSubTypes.NameAr.Contains((searchtext??"")) ||
            searchtext ==null || searchtext =="")).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                PercentComplete = x.PercentComplete,
                IsRetrieved = x.IsRetrieved,
                AskDetails = x.AskDetails ?? 0,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsNew = x.IsNew ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                ProjectDescription = x.Project.ProjectDescription ?? "",
                ContactLists=x.ContactLists.Where(x=>x.IsDeleted==false).ToList(),
                StartDateNew=x.StartDateNew,
                EndDateNew=x.EndDateNew,
                TaskNo = x.TaskNo ?? null,
            })
                 .ToList().OrderByDescending(x => x.IsNew).ThenByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserIdSearchProj(string EndDateP, int? ProjectId, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId  && (s.ProjectId == ProjectId || ProjectId == null) && s.Type == 3 && s.Status != 4 && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,

                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                PercentComplete = x.PercentComplete,
                IsRetrieved = x.IsRetrieved,
                AskDetails = x.AskDetails ?? 0,
                IsConverted = x.IsConverted,
                PlusTime = x.PlusTime,
                IsNew = x.IsNew ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskNo = x.TaskNo ?? null,
            })
                 .ToList().OrderByDescending(x => x.IsNew).ThenBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            return projectPhasesTasks;
        }

        public async Task<int> GetNewTasksCountByUserId(string EndDateP, int? UserId, int BranchId)
        {
            if (EndDateP != "")
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    TaskNotStarted = _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3).Sum(s => s.PercentComplete) / _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3).Count() ?? 0,
                    TaskInProgress = x.PercentComplete ?? 0,
                    TaskDone = x.PercentComplete ?? 0,
                    TaskLate = x.PercentComplete ?? 0,
                    ProjectNumber = x.Project!.ProjectNo,
                    EndDate = x.EndDate
                }).Count();
                return projectPhasesTasks;
            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    TaskNotStarted = _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Sum(s => s.PercentComplete) / _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Count() ?? 0,
                    TaskInProgress = x.PercentComplete ?? 0,
                    TaskDone = x.PercentComplete ?? 0,
                    TaskLate = x.PercentComplete ?? 0,
                    ProjectNumber = x.Project!.ProjectNo,
                    EndDate = x.EndDate
                }).Count();
                return projectPhasesTasks;
            };
            //return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserId(string EndDateP, int? UserId, int BranchId)
        {
            if (EndDateP != "")
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    PercentComplete = x.PercentComplete,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    TaskLongDesc = x.TaskLongDesc,
                    EndTime = x.EndTime
                }).ToList().Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return projectPhasesTasks;
            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    DescriptionAr = x.DescriptionAr,
                    TimeMinutes = x.TimeMinutes,
                    StopProjectType = x.Project.StopProjectType ?? 0,

                    ProjectNumber = x.Project.ProjectDescription
                }).ToList();
                return projectPhasesTasks;
            };
        }
        public async Task<int> GetLateTasksCountByUserId(string EndDateP, int? UserId, int BranchId)
        {
            if (EndDateP != "")
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).ToList().Where(s => (DateTime.ParseExact(s.EndDate == null ? EndDateP : s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndDate == null ? EndDateP : s.EndDate, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate == null ? EndDateP : s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Count();
                return projectPhasesTasks;
            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Count();
                return projectPhasesTasks;
            }
            //return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdAndProjectId(string EndDateP, int? UserId, int BranchId)
        {
            if (EndDateP != "")
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    TaskNotStarted = _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Sum(s => s.PercentComplete) / _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Count() ?? 0,
                    TaskInProgress = x.PercentComplete ?? 0,
                    TaskDone = x.PercentComplete ?? 0,
                    TaskLate = x.PercentComplete ?? 0,
                    ProjectNumber = x.Project!.ProjectNo,
                    StopProjectType = x.Project.StopProjectType ?? 0,

                    EndDate = x.EndDate
                }).ToList();
                return projectPhasesTasks;
            }
            else
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    TaskNotStarted = _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Sum(s => s.PercentComplete) / _TaamerProContext.ProjectPhasesTasks.Where(s => s.Status == 1 && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Count() ?? 0,
                    TaskInProgress = x.PercentComplete ?? 0,
                    TaskDone = x.PercentComplete ?? 0,
                    TaskLate = x.PercentComplete ?? 0,
                    ProjectNumber = x.Project!.ProjectNo,
                    StopProjectType = x.Project.StopProjectType ?? 0,

                    EndDate = x.EndDate
                }).ToList();
                return projectPhasesTasks;
            };
        }
        public async Task<decimal> GetTasksPercentByUserIdAndProjectId(int? UserId, int BranchId)
        {
            decimal projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3 && s.IsMerig == -1).Count();
            decimal AllProjectPhasesTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1).Count();
            decimal result = 0;
            if (AllProjectPhasesTask != 0)
            {
                result = (projectPhasesTasks / AllProjectPhasesTask) * 100;
            }
            return result;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByStatus(int? StatusId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == StatusId && s.BranchId == BranchId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                Cost = x.Cost,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(****)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                TaskTypeName = s.TaskTypeName,
                Status = s.Status,
                UserName = s.UserName ?? "",
                Cost = s.Cost,
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
            }); 
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByProjectNo(string ProjectNo, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.Project.ProjectNo == ProjectNo && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                Cost = x.Cost,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                TaskTypeName = x.TaskTypeModel!.NameAr, //x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                Cost = s.Cost,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                TaskTypeName = s.TaskTypeName,
                Status = s.Status,
                UserName = s.UserName ?? "",
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                PlayingTime = s.PlayingTime,
            });
            if (ProjectNo != "")
            {
                projectPhasesTasks = projectPhasesTasks.Where(s => s.ProjectNumber.Equals(ProjectNo.Trim())).ToList();
            }
            return projectPhasesTasks;
        }
         public async Task<ProjectPhasesTasksVM> GetProjectPhasesTaskByPerdecessorId(int? PerdecessorId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.PhaseTaskId == PerdecessorId && s.IsMerig == -1)
                                                                      .Select(x => new ProjectPhasesTasksVM
                                                                      {
                                                                          PhaseTaskId = x.PhaseTaskId,
                                                                          DescriptionAr = x.DescriptionAr,
                                                                          DescriptionEn = x.DescriptionEn,
                                                                          ParentId = x.ParentId,
                                                                          ProjSubTypeId = x.ProjSubTypeId,
                                                                          Type = x.Type,
                                                                          UserId = x.UserId,NumAdded=x.NumAdded??0,
                                                                          ProjectId = x.ProjectId,
                                                                          TimeMinutes = x.TimeMinutes,
                                                                          TimeType = x.TimeType,
                                                                          Remaining = x.Remaining,
                                                                          Cost = x.Cost,
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
                                                                          ExcpectedStartDate = x.ExcpectedStartDate,
                                                                          ExcpectedEndDate = x.ExcpectedEndDate,
                                                                          EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                                                                          ToUserId = x.ToUserId,
                                                                          Notes = x.Notes ?? "",
                                                                          BranchId = x.BranchId,
                                                                          PhasePriority = x.PhasePriority,
                                                                          ExecPercentage = x.ExecPercentage,
                                                                          //MainPhaseName = x.MainPhase!.DescriptionAr,
                                                                          UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                                                                          SubPhaseName = x.SubPhase!.DescriptionAr,
                                                                          ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                                                                          ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                                                                          ClientName_W = x.Project!.customer!.CustomerNameAr,
                                                                          ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                                                                          ProjectMangerName = x.Project!.Users!.FullName ?? "",
                                                                          TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                                                                          ProjectNumber = x.Project!.ProjectNo,
                                                                          TaskLongDesc = x.TaskLongDesc,
                                                                          StatusName = x.Status == 0 ? "غير معلومة" :
                                                                            x.Status == 1 ? " لم تبدأ " :
                                                                            x.Status == 2 ? " قيد التشغيل " :
                                                                            x.Status == 3 ? " متوقفة " :
                                                                            x.Status == 4 ? " منتهية " :
                                                                            x.Status == 5 ? " ملغاة " :
                                                                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                                                                          StopProjectType = x.Project.StopProjectType ?? 0,

                                                                          PlayingTime = x.TimeMinutes - x.Remaining,
                                                                          PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                                                                      }).First();
            return projectPhasesTasks;
        }
         public async Task<ProjectPhasesTasksVM> GetProjectPhasesTaskByParentId(int? ParentId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.PhaseTaskId == ParentId && s.IsMerig == -1)
                                                                      .Select(x => new ProjectPhasesTasksVM
                                                                      {
                                                                          PhaseTaskId = x.PhaseTaskId,
                                                                          DescriptionAr = x.DescriptionAr,
                                                                          DescriptionEn = x.DescriptionEn,
                                                                          ParentId = x.ParentId,
                                                                          ProjSubTypeId = x.ProjSubTypeId,
                                                                          Type = x.Type,
                                                                          UserId = x.UserId,NumAdded=x.NumAdded??0,
                                                                          ProjectId = x.ProjectId,
                                                                          TimeMinutes = x.TimeMinutes,
                                                                          TimeType = x.TimeType,
                                                                          Remaining = x.Remaining,
                                                                          Cost = x.Cost,
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
                                                                          ExcpectedStartDate = x.ExcpectedStartDate,
                                                                          ExcpectedEndDate = x.ExcpectedEndDate,
                                                                          EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                                                                          ToUserId = x.ToUserId,
                                                                          Notes = x.Notes ?? "",
                                                                          BranchId = x.BranchId,
                                                                          PhasePriority = x.PhasePriority,
                                                                          ExecPercentage = x.ExecPercentage,
                                                                          //MainPhaseName = x.MainPhase!.DescriptionAr,
                                                                          UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                                                                          SubPhaseName = x.SubPhase!.DescriptionAr,
                                                                          ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                                                                          ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                                                          ClientName_W = x.Project!.customer!.CustomerNameAr ?? "",
                                                                          ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                                                                          ProjectMangerName = x.Project!.Users!.FullName ?? "",
                                                                          TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                                                                          ProjectNumber = x.Project!.ProjectNo,
                                                                          TaskLongDesc = x.TaskLongDesc,
                                                                          StatusName = x.Status == 0 ? "غير معلومة" :
                                                                            x.Status == 1 ? " لم تبدأ " :
                                                                            x.Status == 2 ? " قيد التشغيل " :
                                                                            x.Status == 3 ? " متوقفة " :
                                                                            x.Status == 4 ? " منتهية " :
                                                                            x.Status == 5 ? " ملغاة " :
                                                                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                                                                          StopProjectType = x.Project.StopProjectType ?? 0,

                                                                          PlayingTime = x.TimeMinutes - x.Remaining,
                                                                          PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                                                                      }).First();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksSearch(ProjectPhasesTasksVM TasksSearch, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId &&
                                                                      (s.Project.ProjectNo == TasksSearch.ProjectNumber || TasksSearch.ProjectNumber == null) &&
                                                                      (s.DescriptionAr == TasksSearch.DescriptionAr || TasksSearch.DescriptionAr == null) &&
                                                                      (s.Status == TasksSearch.Status || TasksSearch.Status == null) && s.IsMerig == -1)
                                                                      .Select(x => new ProjectPhasesTasksVM
                                                                      {
                                                                          PhaseTaskId = x.PhaseTaskId,
                                                                          DescriptionAr = x.DescriptionAr,
                                                                          DescriptionEn = x.DescriptionEn,
                                                                          ParentId = x.ParentId,
                                                                          ProjSubTypeId = x.ProjSubTypeId,
                                                                          Type = x.Type,
                                                                          UserId = x.UserId,NumAdded=x.NumAdded??0,
                                                                          ProjectId = x.ProjectId,
                                                                          TimeMinutes = x.TimeMinutes,
                                                                          TimeType = x.TimeType,
                                                                          Remaining = x.Remaining,
                                                                          Cost = x.Cost,
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
                                                                          ExcpectedStartDate = x.ExcpectedStartDate,
                                                                          ExcpectedEndDate = x.ExcpectedEndDate,
                                                                          EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                                                                          ToUserId = x.ToUserId,
                                                                          Notes = x.Notes ?? "",
                                                                          BranchId = x.BranchId,
                                                                          PhasePriority = x.PhasePriority,
                                                                          ExecPercentage = x.ExecPercentage,
                                                                          //MainPhaseName = x.MainPhase!.DescriptionAr,
                                                                          UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                                                                          SubPhaseName = x.SubPhase!.DescriptionAr,
                                                                          ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                                                                          ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                                                          ClientName_W = x.Project!.customer!.CustomerNameAr ?? "",
                                                                          ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                                                                          ProjectMangerName = x.Project!.Users!.FullName ?? "",
                                                                          TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                                                                          ProjectNumber = x.Project!.ProjectNo,
                                                                          TaskLongDesc = x.TaskLongDesc,
                                                                          StatusName = x.Status == 0 ? "غير معلومة" :
                                                                            x.Status == 1 ? " لم تبدأ " :
                                                                            x.Status == 2 ? " قيد التشغيل " :
                                                                            x.Status == 3 ? " متوقفة " :
                                                                            x.Status == 4 ? " منتهية " :
                                                                            x.Status == 5 ? " ملغاة " :
                                                                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                                                                          StopProjectType = x.Project.StopProjectType ?? 0,
                                                                          IsRetrieved=x.IsRetrieved,

                                                                          PlayingTime = x.TimeMinutes - x.Remaining,
                                                                          PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                                                                      }).ToList().Select(s => new ProjectPhasesTasksVM()
                                                                      {
                                                                          PhaseTaskId = s.PhaseTaskId,
                                                                          DescriptionAr = s.DescriptionAr,
                                                                          DescriptionEn = s.DescriptionEn,
                                                                          ParentId = s.ParentId,
                                                                          ProjSubTypeId = s.ProjSubTypeId,
                                                                          Type = s.Type,
                                                                          UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                                                                          UserName = s.UserName ?? "",
                                                                          StopCount = s.StopCount,
                                                                          TaskTypeName = s.TaskTypeName,
                                                                          Cost = s.Cost,
                                                                          OrderNo = s.OrderNo,
                                                                                          TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                                                                          TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                                                                          ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                                                                          ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                                                                          EndDateCalc = s.EndDateCalc ?? "",

                                                                          PercentComplete = s.PercentComplete,
                                                                          ToUserId = s.ToUserId,
                                                                          Notes = s.Notes,
                                                                          BranchId = s.BranchId,
                                                                          PhasePriority = s.PhasePriority,
                                                                          ExecPercentage = s.ExecPercentage,
                                                                          MainPhaseName = s.MainPhaseName,
                                                                          SubPhaseName = s.SubPhaseName,
                                                                          ProjectSubTypeName = s.ProjectSubTypeName,
                                                                          ProjectTypeName = s.ProjectTypeName,
                                                                          ClientName = s.ClientName,
                                                                          ClientName_W = s.ClientName_W,

                                                                          ProjectMangerName = s.ProjectMangerName,
                                                                          ProjectNumber = s.ProjectNumber,
                                                                          TaskLongDesc = s.TaskLongDesc,
                                                                          StatusName = s.Status == 0 ? "غير معلومة" :
                                                                                      s.Status == 1 ? " لم تبدأ " :
                                                                                      s.Status == 2 ? " قيد التشغيل " :
                                                                                      s.Status == 3 ? " متوقفة " :
                                                                                      s.Status == 4 ? " منتهية " :
                                                                                      s.Status == 5 ? " ملغاة " :
                                                                                      s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                                                                          StopProjectType = s.StopProjectType,

                                                                          PlayingTime = s.PlayingTime,
                                                                          IsRetrieved=s.IsRetrieved,
                                                                      });
            return projectPhasesTasks; ;
        }
        ////////////////////////////////////////////////////////
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectMainPhasesByProjectId(int ProjectId)
        {
            var mainPahses = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false & s.ProjectId == ProjectId && s.ParentId == null && s.Type == 1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                Cost = x.Cost,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                PercentComplete = x.PercentComplete,
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
            }).ToList();
            return mainPahses;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectSubPhasesByProjectId(int MainPhaseId)
        {
            var mainPahses = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ParentId == MainPhaseId && s.Type == 2).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                Cost = x.Cost,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                PercentComplete = x.PercentComplete,
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
            }).ToList();
            return mainPahses;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasks(int SubPhaseId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.ParentId == SubPhaseId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,


                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                           x.TimeType == 2 ? (x.TimeMinutes) ://* (24)
                           x.TimeType == 3 ? (x.TimeMinutes) ://* (24 * 7)
                           (x.TimeMinutes),//(*24 * 7 * 30)

                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                Cost = x.Cost,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                //TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                // TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                //           x.TimeType == 2 ? (x.TimeMinutes ) + " يوم " :
                //           x.TimeType == 3 ? (x.TimeMinutes ) + " أسبوع " :  (x.TimeMinutes) + " ساعة ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                //ExcpectedStartDate = s.ExcpectedStartDate,
                //ExcpectedEndDate = s.ExcpectedEndDate,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                Active = s.Active,
                StopCount = s.StopCount,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                UserName = s.UserName ?? "",
                MainPhaseName = s.DescriptionAr,
                SubPhaseName = s.DescriptionAr,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName ?? "",
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                TimeStr = s.TimeStr,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,

            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<ProjectPhasesTasksVM> GetTaskById(int TaskId, string Lang,int? UserId)
        {
            return _TaamerProContext.ProjectPhasesTasks.Where(s => s.PhaseTaskId == TaskId && s.IsDeleted == false).Select(acc => new ProjectPhasesTasksVM
            {
                PhaseTaskId = acc.PhaseTaskId,
                DescriptionAr = acc.DescriptionAr,
                DescriptionEn = acc.DescriptionEn,
                PhasesTaskName = Lang == "ltr" ? acc.DescriptionEn : acc.DescriptionAr,
                ParentId = acc.ParentId,
                ProjSubTypeId = acc.ProjSubTypeId,
                Type = acc.Type,
                UserId = acc.UserId,NumAdded=acc.NumAdded??0,
                PlusTime = acc.PlusTime,
                 UserName = Lang == "rtl" ? acc.Users!.FullNameAr == null ? acc.Users!.FullName : acc.Users!.FullNameAr : acc.Users!.FullName == null ? acc.Users!.FullNameAr : acc.Users!.FullName ?? "",
                ProjectId = acc.ProjectId,
                TimeMinutes = acc.TimeMinutes,
                TimeType = acc.TimeType,
                Remaining = acc.Remaining,
                IsUrgent = acc.IsUrgent,
                IsTemp = acc.IsTemp,
                TaskType = acc.TaskType,
                Status = acc.Status,
                OldStatus = acc.OldStatus,
                Cost = acc.Cost,
                Active = acc.Active,
                StopCount = acc.StopCount,
                OrderNo = acc.OrderNo,
                StartDate = acc.StartDate,
                EndDate = acc.EndDate,
                ExcpectedStartDate = acc.ExcpectedStartDate,
                ExcpectedEndDate = acc.ExcpectedEndDate,
                EndDateCalc = acc.EndDateCalc != null ? acc.EndDateCalc : acc.EndDate != null ? acc.EndDate : acc.ExcpectedEndDate ?? "",

                ToUserId = acc.ToUserId,
                Notes = acc.Notes ?? "",
                BranchId = acc.BranchId,
                IsConverted = acc.IsConverted,
                PhasePriority = acc.PhasePriority,
                ExecPercentage = acc.ExecPercentage??0,
                ProjectMangerName =  acc.Project!.Users!.FullNameAr==null ? acc.Project!.Users!.FullName : acc.Project!.Users!.FullNameAr,
                ProjectManagerImg = acc.Project!.Users != null ? acc.Project!.Users.ImgUrl != null ? acc.Project!.Users!.ImgUrl : "/distnew/images/userprofile.png" : "/distnew/images/userprofile.png",

                TaskTypeName = acc.TaskTypeModel!.NameAr,//acc.TaskType == 1 ? "رفع ملفات" : acc.TaskType == 2 ? "تحصيل دفعه" : acc.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                //TimeStr = acc.TimeType == 1 ? (acc.TimeMinutes) + " ساعه " :
                //          acc.TimeType == 2 ? (acc.TimeMinutes ) + " يوم " :
                //          acc.TimeType == 3 ? (acc.TimeMinutes ) + " أسبوع " : (acc.TimeMinutes) + " ساعة ",
                TimeStr = Lang == "ltr" ? acc.TimeType == 1 ? (acc.TimeMinutes) + " Hour " :
                          acc.TimeType == 2 ? (acc.TimeMinutes) + " Day " :
                          acc.TimeType == 3 ? (acc.TimeMinutes) + " Week " : (acc.TimeMinutes) + " Month " : acc.TimeType == 1 ? (acc.TimeMinutes) + " ساعه " :
                          acc.TimeType == 2 ? (acc.TimeMinutes) + " يوم " :
                          acc.TimeType == 3 ? (acc.TimeMinutes) + " أسبوع " : (acc.TimeMinutes) + " ساعة ",

                StatusName = acc.Status == 0 ? (Lang == "rtl" ? "غير معلومة" : "Unkown") :
                            acc.Status == 1 ? (Lang == "rtl" ? " لم تبدأ " : "Not started") :
                            acc.Status == 2 ? (Lang == "rtl" ? " قيد التشغيل " : "Running") :
                            acc.Status == 3 ? (Lang == "rtl" ? " متوقفة " : "Stopped") :
                            acc.Status == 4 ? (Lang == "rtl" ? " منتهية " : "Finished") :
                            acc.Status == 5 ? (Lang == "rtl" ? " ملغاة " : "Canceled") :
                            acc.Status == 6 ? (Lang == "rtl" ? " محذوفة " : "Deleted") : (Lang == "rtl" ? " تم تحويلها" : "Transferred"),
                ProjectNumber = acc.Project!.ProjectNo,
                ProTypeName = acc.Project!.projecttype!.NameAr,
                ProjectDescription = acc.Project!.ProjectDescription,
                ClientName_W = Lang == "ltr" ? acc.Project!.customer!.CustomerNameEn : acc.Project!.customer!.CustomerNameAr,
                ClientName = acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? acc.Project!.customer!.CustomerNameAr : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? acc.Project!.customer!.CustomerNameAr : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? acc.Project!.customer!.CustomerNameAr + "(*)" : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? acc.Project!.customer!.CustomerNameAr + "(**)" : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? acc.Project!.customer!.CustomerNameAr + "(***)" : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? acc.Project!.customer!.CustomerNameAr + "(VIP)" : acc.Project!.customer!.CustomerNameAr,
                TaskStart = acc.StartDate != null ? acc.StartDate : acc.ExcpectedStartDate != null ? acc.ExcpectedStartDate : "",
                TaskEnd = acc.EndDate != null ? acc.EndDate : acc.ExcpectedEndDate != null ? acc.ExcpectedEndDate : "",
                StopProjectType = acc.Project!.StopProjectType ?? 0,
                SettingId = acc.SettingId,
                TaskLongDesc =_TaamerProContext.ContactLists.Where(x=>x.TaskId.Value == TaskId && x.UserId != UserId && x.IsDeleted==false).OrderByDescending(x=>x.AddDate).FirstOrDefault().Contacttxt ??"", //acc.TaskLongDesc,
                ProjectGoals = acc.ProjectGoals,
                ProjectTypeId = acc.Project!.projecttype!.TypeId,
                AddedTaskName = acc.AddTaskUserId==null?"": Lang =="rtl"? acc.AddTaskUser!.FullNameAr==null? acc.AddTaskUser!.FullName : acc.AddTaskUser!.FullNameAr : acc.AddTaskUser!.FullName==null? acc.AddTaskUser!.FullNameAr : acc.AddTaskUser!.FullName,
                AddedTaskImg = acc.AddTaskUserId == null ? "/distnew/images/userprofile.png" : acc.AddTaskUser!.ImgUrl == "" ? "/distnew/images/userprofile.png" : acc.AddTaskUser!.ImgUrl ?? "/distnew/images/userprofile.png",
                AddDate2 = acc.AddDate,
               ProjectManagerId=acc.Project!.MangerId,
                NewSetting = acc.Project!.NewSetting??false,

                IsMerig = acc.IsMerig,
               AskDetails=acc.AskDetails ??0,
               IsNew=acc.IsNew ?? 0,
               TaskTimeFrom=acc.TaskTimeFrom??"",
               TaskTimeTo=acc.TaskTimeTo??"",
                SubPhaseName = acc.SubPhase!.DescriptionAr??"",
                //MainPhaseName = acc.MainPhase!.DescriptionAr ??"",
                RetrievedReason=acc.RetrievedReason??"",
                Mobile = acc.Users!=null?acc.Users.Mobile: "",

                SettingName = acc.SettingId==null ?"":_TaamerProContext.ProSettingDetails.Where(x=>x.ProjectSubtypeId==acc.Settings.ProjSubTypeId).FirstOrDefault().ProSettingNote,
               PercentComplete = acc.TimeMinutes != 0 ? acc.TimeMinutes != null ? (((acc.TimeMinutes - acc.Remaining) / acc.TimeMinutes) * 100) <= 100 ? (((acc.TimeMinutes - acc.Remaining) / acc.TimeMinutes) * 100) : 100 : 0 : 0,
            StartDateNew=acc.StartDateNew,
            EndDateNew=acc.EndDateNew,
            Managerapproval=acc.Managerapproval,
            ReasonsIdText=acc.tasksReasons!=null?acc.tasksReasons.NameAr??"":"",
            PlusTimeReason=acc.PlusTimeReason,
            convertReason =acc.convertReason,
            PlusTimeReason_admin =acc.PlusTimeReason_admin,
            convertReason_admin =acc.convertReason_admin,
            TaskNo=acc.TaskNo??null,
            
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                PhasesTaskName = s.PhasesTaskName,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                PlusTime = s.PlusTime,
                UserName = s.UserName ?? "",
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
                StopCount = s.StopCount,
                Cost = s.Cost,
                OrderNo = s.OrderNo,
                TaskStart = s.TaskStart,
                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                IsConverted = s.IsConverted,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                TimeStr = s.TimeStr,

                StatusName = s.StatusName,
                ProjectNumber = s.ProjectNumber,
                ProjectMangerName = s.ProjectMangerName ?? "",
                ProjectManagerImg=s.ProjectManagerImg?? "/distnew/images/userprofile.png",
                ProTypeName = s.ProTypeName,
                ProjectDescription = s.ProjectDescription,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                StopProjectType = s.StopProjectType,
                SettingId = s.SettingId,

                TaskLongDesc = s.TaskLongDesc,
                ProjectGoals = s.ProjectGoals,
                ProjectTypeId = s.ProjectTypeId,
                AddedTaskName = s.AddedTaskName ?? "",
                AddedTaskImg = s.AddedTaskImg ?? "/distnew/images/userprofile.png",
                ProjectManagerId=s.ProjectManagerId,
                NewSetting=s.NewSetting,
                IsMerig =s.IsMerig,
                AskDetails=s.AskDetails,
                IsNew=s.IsNew,
                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",
                //MainPhaseName=s.MainPhaseName ??"",
                SubPhaseName=s.SubPhaseName??"",
                SettingName =s.SettingName??"",
                RetrievedReason = s.RetrievedReason ?? "",
                Mobile=s.Mobile??"",
                StartDateNew = s.StartDateNew,
                EndDateNew = s.EndDateNew,
                Managerapproval = s.Managerapproval,
                ReasonsIdText = s.ReasonsIdText,
                PlusTimeReason =s.PlusTimeReason,
                PlusTimeReason_admin =s.PlusTimeReason_admin,
                convertReason=s.convertReason,
                convertReason_admin =s.convertReason_admin,
                TaskNo = s.TaskNo,
            }).FirstOrDefault()!;

        }
         public async Task<ProjectPhasesTasksVM> GetTaskByUserId(int TaskId, int UserId)
        {
            return _TaamerProContext.ProjectPhasesTasks.Where(s => s.PhaseTaskId == TaskId && s.UserId == UserId && s.IsMerig == -1).Select(acc => new ProjectPhasesTasksVM
            {
                PhaseTaskId = acc.PhaseTaskId,
                DescriptionAr = acc.DescriptionAr,
                DescriptionEn = acc.DescriptionEn,
                ParentId = acc.ParentId,
                ProjSubTypeId = acc.ProjSubTypeId,
                Type = acc.Type,
                UserId = acc.UserId,NumAdded=acc.NumAdded??0,
                UserName = acc.Users!.FullName ?? "",
                ProjectId = acc.ProjectId,
                TimeMinutes = acc.TimeMinutes,
                TimeType = acc.TimeType,
                Remaining = acc.Remaining,
                IsUrgent = acc.IsUrgent,
                IsTemp = acc.IsTemp,
                TaskType = acc.TaskType,
                Status = acc.Status,
                OldStatus = acc.OldStatus,
                Cost = acc.Cost,
                Active = acc.Active,
                StopCount = acc.StopCount,
                OrderNo = acc.OrderNo,
                StartDate = acc.StartDate,
                EndDate = acc.EndDate,
                ExcpectedStartDate = acc.ExcpectedStartDate,
                ExcpectedEndDate = acc.ExcpectedEndDate,
                EndDateCalc = acc.EndDate!=null ? acc.EndDate: acc.EndDateCalc ?? "",

                ToUserId = acc.ToUserId,
                Notes = acc.Notes ?? "",
                BranchId = acc.BranchId,
                PhasePriority = acc.PhasePriority,
                ExecPercentage = acc.ExecPercentage,
                TaskTypeName = acc.TaskTypeModel!.NameAr,//acc.TaskType == 1 ? "رفع ملفات" : acc.TaskType == 2 ? "تحصيل دفعه" : acc.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeStr = acc.TimeType == 1 ? (acc.TimeMinutes) + " ساعه " :
                          acc.TimeType == 2 ? (acc.TimeMinutes) + " يوم " :
                          acc.TimeType == 3 ? (acc.TimeMinutes) + " أسبوع " : (acc.TimeMinutes) + " ساعة ",
                StatusName = acc.Status == 0 ? "غير معلومة" :
                            acc.Status == 1 ? " لم تبدأ " :
                            acc.Status == 2 ? " قيد التشغيل " :
                            acc.Status == 3 ? " متوقفة " :
                            acc.Status == 4 ? " منتهية " :
                            acc.Status == 5 ? " ملغاة " :
                            acc.Status == 6 ? " محذوفة " : " تم تحويلها",
                ProjectNumber = acc.Project!.ProjectNo,
                ProTypeName = acc.Project!.projecttype!.NameAr,
                ProjectDescription = acc.Project!.ProjectDescription,
                ClientName_W = acc.Project!.customer!.CustomerNameAr,
                ClientName = acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? acc.Project!.customer!.CustomerNameAr : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? acc.Project!.customer!.CustomerNameAr : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? acc.Project!.customer!.CustomerNameAr + "(*)" : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? acc.Project!.customer!.CustomerNameAr + "(**)" : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? acc.Project!.customer!.CustomerNameAr + "(***)" : acc.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? acc.Project!.customer!.CustomerNameAr + "(VIP)" : acc.Project!.customer!.CustomerNameAr,
                TaskStart = acc.StartDate != null ? acc.StartDate : acc.ExcpectedStartDate != null ? acc.ExcpectedStartDate : "",
                TaskEnd = acc.EndDate != null ? acc.EndDate : acc.ExcpectedEndDate != null ? acc.ExcpectedEndDate : "",
                StopProjectType = acc.Project!.StopProjectType ?? 0,

                TaskLongDesc = acc.TaskLongDesc,
                ProjectTypeId = acc.Project!.projecttype!.TypeId,
                TaskTimeFrom = acc.TaskTimeFrom ?? "",
                TaskTimeTo = acc.TaskTimeTo ?? "",
                RetrievedReason = acc.RetrievedReason ?? "",
                PercentComplete = acc.TimeMinutes != 0 ? acc.TimeMinutes != null ? (((acc.TimeMinutes - acc.Remaining) / acc.TimeMinutes) * 100) <= 100 ? (((acc.TimeMinutes - acc.Remaining) / acc.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                UserName = s.UserName ?? "",
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
                StopCount = s.StopCount,
                Cost = s.Cost,
                OrderNo = s.OrderNo,
                TaskStart = s.TaskStart,
                TaskEnd = s.TaskEnd,
                ExcpectedStartDate = s.ExcpectedStartDate,
                ExcpectedEndDate = s.ExcpectedEndDate,
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                TimeStr = s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                          s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                          s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                TaskTypeName = s.TaskTypeName,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                ProjectNumber = s.ProjectNumber,
                ProTypeName = s.ProTypeName,
                ProjectDescription = s.ProjectDescription,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                StopProjectType = s.StopProjectType,

                TaskTimeFrom = s.TaskTimeFrom ?? "",
                TaskTimeTo = s.TaskTimeTo ?? "",

                TaskLongDesc = s.TaskLongDesc,
                ProjectGoals = s.ProjectGoals,
                ProjectTypeId = s.ProjectTypeId,
                RetrievedReason = s.RetrievedReason ?? "",
            }).FirstOrDefault()!;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasksbyUserId(int SubPhaseId, int UserId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.ParentId == SubPhaseId && s.UserId == UserId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                            x.TimeType == 2 ? (x.TimeMinutes) :
                            x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                Cost = x.Cost,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                           x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                           x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                Active = s.Active,
                StopCount = s.StopCount,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate,
                ExcpectedEndDate = s.ExcpectedEndDate,
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                UserName = s.UserName ?? "",
                MainPhaseName = s.DescriptionAr,
                SubPhaseName = s.DescriptionAr,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName ?? "",
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                TimeStr = s.TimeStr,
                PlayingTime = s.PlayingTime,
                StopProjectType = s.StopProjectType,

                TaskLongDesc = s.TaskLongDesc,
            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasksbyUserId2(int SubPhaseId, int UserId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.ParentId == SubPhaseId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                           x.TimeType == 2 ? (x.TimeMinutes) :
                           x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                Cost = x.Cost,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                Active = s.Active,
                StopCount = s.StopCount,
                OrderNo = s.OrderNo,
                TaskStart = s.StartDate != null ? s.StartDate : s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate,
                ExcpectedEndDate = s.ExcpectedEndDate,
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                UserName = s.UserName ?? "",
                MainPhaseName = s.DescriptionAr,
                SubPhaseName = s.DescriptionAr,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName ?? "",
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                StopProjectType = s.StopProjectType,

                TaskLongDesc = s.TaskLongDesc,
                TimeStr = s.TimeStr,
                PlayingTime = s.PlayingTime,
            }).ToList();
            return projectPhasesTasks;
        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectId(int ProjectId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.Type == 3 && s.IsMerig == -1 && s.BranchId==BranchId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                          x.TimeType == 2 ? (x.TimeMinutes) :
                          x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdWithoutBranch(int ProjectId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                            x.TimeType == 2 ? (x.TimeMinutes) :
                            x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                           x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                           x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList();
            return projectPhasesTasks;
        }
        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdWithoutBranchNew(int ProjectId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,

            }).ToList();
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId,int? DepartmentId, string DateFrom, string DateTo, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (DepartmentId == null || DepartmentId == 0 || s.DepartmentId == DepartmentId) && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && s.Type == 3 && s.IsMerig == -1 && s.BranchId==BranchId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                          x.TimeType == 2 ? (x.TimeMinutes) :
                          x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!=null? x.ProjectSubTypes!.NameAr??"":"",
                ProjectTypeName = x.ProjectSubTypes != null ? x.ProjectSubTypes.ProjectType != null ? x.ProjectSubTypes!.ProjectType!.NameAr??"":"":"",
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList().Where(m =>(string.IsNullOrEmpty(DateFrom)|| (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
            return projectPhasesTasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId,string? searchtext)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (DepartmentId == null || DepartmentId == 0 || s.DepartmentId == DepartmentId) && (ProjectId == null || ProjectId == 0 || s.ProjectId == ProjectId) && s.Type == 3 && s.IsMerig == -1 && s.BranchId == BranchId
            && (
                s.Project.customer.CustomerNameAr.Contains((searchtext??"")) || s.Project.ProjectDescription.Contains((searchtext??"")) ||
                s.Project.ProjectTypeName.Contains((searchtext??"")) || s.DescriptionAr.Contains((searchtext??"")) || s.Users.FullNameAr.Contains((searchtext??"")) ||
                searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                          x.TimeType == 2 ? (x.TimeMinutes) :
                          x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes != null ? x.ProjectSubTypes!.NameAr ?? "" : "",
                ProjectTypeName = x.ProjectSubTypes != null ? x.ProjectSubTypes.ProjectType != null ? x.ProjectSubTypes!.ProjectType!.NameAr ?? "" : "" : "",
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  "+ x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr,            
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksPhasesByProjectId(int ProjectId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.BranchId == BranchId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                          x.TimeType == 2 ? (x.TimeMinutes) :
                          x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdW2(string DateFrom,string DateTo, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false &&s.BranchId==BranchId && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                         x.TimeType == 2 ? (x.TimeMinutes) :
                         x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,//
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,
                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                TaskLongDesc = x.TaskLongDesc,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                PlayingTime = x.TimeMinutes - x.Remaining,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
            return projectPhasesTasks;
        }



         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdW(int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.IsMerig == -1 && s.BranchId == BranchId).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                         x.TimeType == 2 ? (x.TimeMinutes) :
                         x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes != null ? x.ProjectSubTypes!.NameAr ?? "" : "",
                ProjectTypeName = x.ProjectSubTypes != null ? x.ProjectSubTypes.ProjectType != null ? x.ProjectSubTypes!.ProjectType!.NameAr ?? "" : "" : "",
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                PlayingTime = x.TimeMinutes - x.Remaining,
                TaskTimeFrom = x.TaskTimeFrom ?? "",
                TaskTimeTo = x.TaskTimeTo ?? "",
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList();
            return projectPhasesTasks;
        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllPhasesTasksByProjectId(int ProjectId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                          x.TimeType == 2 ? (x.TimeMinutes) :
                          x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr,
                indentation=x.indentation,
                taskindex = x.taskindex,
                StartDateNew = x.StartDateNew,
                EndDateNew = x.EndDateNew,
                IsConverted=x.IsConverted??0,
                PlusTime= x.PlusTime ?? false,
                IsMerig = x.IsMerig ?? -1,
                Managerapproval = x.Managerapproval,
                ReasonsId = x.ReasonsId,
                

            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdForFinish(int ProjectId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                           x.TimeType == 2 ? (x.TimeMinutes) :
                           x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                            x.Status == 1 ? " لم تبدأ " :
                            x.Status == 2 ? " قيد التشغيل " :
                            x.Status == 3 ? " متوقفة " :
                            x.Status == 4 ? " منتهية " :
                            x.Status == 5 ? " ملغاة " :
                            x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                FullTaskDescription = x.TaskNo + "  " + x.DescriptionAr + "/" + x.Users!.FullName + "/" + x.Project!.ProjectNo + "/" + x.Project!.customer!.CustomerNameAr
            }).ToList();
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectCurrentTasks(int ProjectId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && s.Type == 3 && s.Status == 2 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? (x.TimeMinutes) :
                           x.TimeType == 2 ? (x.TimeMinutes) :
                           x.TimeType == 3 ? (x.TimeMinutes) : (x.TimeMinutes),
                Remaining = x.Remaining,
                Cost = x.Cost,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                TaskTypeName = x.TaskTypeModel==null ? "" : x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr==null? x.Users!.FullName: x.Users!.FullNameAr ?? "",
                UserImg = x.Users == null ? "" : x.Users!.ImgUrl ?? "/distnew/images/userprofile.png",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                //MainPhaseName = x.MainPhase == null ? "" : x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase == null ? "" : x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes == null ? "" : x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType == null ? "" : x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project.customer == null ? "" : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project.Users == null ? "" : x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                         x.Status == 1 ? " لم تبدأ " :
                         x.Status == 2 ? " قيد التشغيل " :
                         x.Status == 3 ? " متوقفة " :
                         x.Status == 4 ? " منتهية " :
                         x.Status == 5 ? " ملغاة " :
                         x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                NodeLocation = x.NodeLocations == null ? "" : x.NodeLocations!.Location ?? x.Settings!.NodeLocations!.Location,
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList();
            return projectPhasesTasks;
        }
        public async Task<int> GetUserTaskCount(int? UserId, int BranchId)
        {
            var UserTaskCount = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1);
            return UserTaskCount.Count();
        }

        //////////////////////////////////
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetInprogressTasksByUserId(int UserId, int BranchId)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.Status == 2 && s.UserId == UserId && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                Cost = x.Cost,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",


                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                Cost = s.Cost,
                IsUrgent = s.IsUrgent,
                UserName = s.UserName ?? "",
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                ProjectDescription = s.ProjectDescription,
                StatusName = s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية " :
                            s.Status == 5 ? " ملغاة " :
                            s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = s.PlayingTime,
                PercentComplete = s.PercentComplete,
                StopProjectType = s.StopProjectType,

                TaskLongDesc = s.TaskLongDesc,
            }); ;
            return projectPhasesTasks;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectTasksByPhaseId(int id)
        {
            var settings = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.ParentId == id && s.Type == 3 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                SettingId=x.SettingId,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                Cost = x.Cost,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project.ProjectDescription,
                StatusName = x.Status == 0 ? "غير معلومة" :
                           x.Status == 1 ? " لم تبدأ " :
                           x.Status == 2 ? " قيد التشغيل " :
                           x.Status == 3 ? " متوقفة " :
                           x.Status == 4 ? " منتهية " :
                           x.Status == 5 ? " ملغاة " :
                           x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                StopProjectType = x.Project.StopProjectType ?? 0,

                TaskLongDesc = x.TaskLongDesc,
            }).ToList();
            return settings;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectsWithProSettingVM()
        {
            var settings = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Project.Status==0 && s.Type==1 && s.DescriptionAr!= "بدون مرحلة رئيسية").GroupBy(s=>s.ProjectId).Select(x => new ProjectPhasesTasksVM
            {
                ProjectId = x.Key,
                
                

            }).ToList();
            return settings;
        }
         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectsWithoutProSettingVM()
        {
            var settings = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Project.Status == 0 && s.Type == 1 && s.DescriptionAr == "بدون مرحلة رئيسية").GroupBy(s => s.ProjectId).Select(x => new ProjectPhasesTasksVM
            {
                ProjectId = x.Key,

            }).ToList();
            return settings;
        }

        public async Task<IEnumerable<rptGetEmpDoneTasksVM>> GetDoneTasksDGV(string FromDate, string ToDate, int UserID, string Con)
        {
            try
            {
                List<rptGetEmpDoneTasksVM> lmd = new List< rptGetEmpDoneTasksVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetEmpDoneTasks";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@to", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (UserID == 0)
                        {
                            UserID = 0;
                            command.Parameters.Add(new SqlParameter("@UserID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@UserID", UserID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetEmpDoneTasksVM
                            {
                                DescriptionAr = (dr[0]).ToString(),
                                Duration = dr[2].ToString() == "1" ? (dr[1].ToString()) + " ساعه " :
                          dr[2].ToString() == "2" ? (dr[1].ToString()) + " يوم " :
                          dr[2].ToString() == "3" ? (dr[1].ToString()) + " أسبوع " : (dr[1].ToString()) + " ساعة ", //dr[2].ToString(),
                                ProjectNo = dr[3].ToString(),
                                ClientName = dr[4].ToString(),
                                EndDate = dr[5].ToString(),
                                Cost = dr[6].ToString(),
                                EmpName = dr[7].ToString(),
                                JobName = dr[8].ToString(),
                                Remaining =Convert.ToInt32(dr[9].ToString()),
                                Status = Convert.ToInt32(dr[10].ToString()),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetEmpDoneTasksVM> lmd = new List< rptGetEmpDoneTasksVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<rptGetEmpUndergoingTasksVM>> GetUndergoingTasksDGV(string FromDate, string ToDate, int UserID, string Con)
        {
            try
            {
                List< rptGetEmpUndergoingTasksVM> lmd = new List< rptGetEmpUndergoingTasksVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetEmpUndergoingTasks";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@to", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (UserID == 0)
                        {
                            UserID = 0;
                            command.Parameters.Add(new SqlParameter("@UserID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@UserID", UserID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetEmpUndergoingTasksVM
                            {
                                DescriptionAr = (dr[0]).ToString(),
                                Duration = dr[2].ToString(),
                                ProjectNo = dr[3].ToString(),
                                ClientName = dr[4].ToString(),
                                StartDate = dr[5].ToString(),
                                Priority = dr[6].ToString(),
                                EmpName = dr[7].ToString(),
                                JobName = dr[8].ToString()
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetEmpUndergoingTasksVM> lmd = new List< rptGetEmpUndergoingTasksVM>();
                return lmd;
            }

        }

        public async Task< IEnumerable< rptGetEmpDelayedTasksVM>> GetEmpDelayedTasksDGV(string FromDate, string ToDate, int UserID, string Con)
        {
            try
            {
                List< rptGetEmpDelayedTasksVM> lmd = new List< rptGetEmpDelayedTasksVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetEmpDelayedTasks";
                        command.Connection = con;
                        string from = null;
                        string to = null;
                        //  command.Parameters.AddWithValue("@SubjectId", b);

                        if (FromDate == "")
                        {
                            from = null;
                            command.Parameters.Add(new SqlParameter("@From", "2000-01-01"));

                        }
                        else
                        {
                            from = FromDate;
                            command.Parameters.Add(new SqlParameter("@From", DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            //command.Parameters.Add(new SqlParameter("@enddate", to));
                        }
                        if (ToDate == "")
                        {
                            to = null;
                            command.Parameters.Add(new SqlParameter("@to", "2100-12-12"));

                        }
                        else
                        {
                            to = ToDate;
                            //command.Parameters.Add(new SqlParameter("@startdate",from));
                            command.Parameters.Add(new SqlParameter("@to", DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }

                        if (UserID == 0)
                        {
                            UserID = 0;
                            command.Parameters.Add(new SqlParameter("@UserID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@UserID", UserID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            //decimal remaining;
                            //string delayTime = "";
                            //var TimeR = 0;
                            //if (decimal.TryParse(dr[9].ToString(), out remaining) && remaining != null)
                            //{
                            //    if ((-(remaining)) > 60 * 24 * 7 * 30)
                            //    {
                            //        delayTime= (-(remaining / (60 * 24 * 7 * 30))).ToString("N0")  + "  ساعة  ";
                            //    }
                            //    else if ((-(remaining)) > 60 * 24 * 7)
                            //    {
                            //        delayTime= (-(remaining / (60 * 24 * 7))).ToString("N0") + "  اسبوع  ";
                            //    }
                            //    else if ((-(remaining)) > 60 * 24)
                            //    {
                            //        delayTime= (-(remaining / (60 * 24))).ToString("N0") + "  يوم  ";
                            //    }
                            //    else if ((-(remaining)) > 60)
                            //    {
                            //        delayTime = (-(remaining / 60)).ToString("N0") + "  ساعة  ";
                            //    }
                            //    else
                            //    {
                            //        delayTime = (-(remaining)).ToString("N0") + " دقيقة  ";
                            //    }
                            //}

                            lmd.Add(new  rptGetEmpDelayedTasksVM
                            {
                                DescriptionAr = (dr[0]).ToString(),
                                Duration = dr[2].ToString(),
                                ProjectNo = dr[3].ToString(),
                                ClientName = dr[4].ToString(),
                                StartDate = dr[5].ToString(),
                                DelayTime = dr[6].ToString(),//delayTime,
                                EmpName = dr[7].ToString(),
                                JobName = dr[8].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetEmpDelayedTasksVM> lmd = new List< rptGetEmpDelayedTasksVM>();
                return lmd;
            }

        }

        public async Task<IEnumerable<rptGetDoneWorkOrdersByExecEmp>> GetEmpDoneWOsDGV(int UserID, string Con)
        {
            try
            {
                List< rptGetDoneWorkOrdersByExecEmp> lmd = new List< rptGetDoneWorkOrdersByExecEmp>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetDoneWorkOrdersByExecEmp";
                        command.Connection = con;

                        if (UserID == 0)
                        {
                            UserID = 0;
                            command.Parameters.Add(new SqlParameter("@UserID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpID", UserID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetDoneWorkOrdersByExecEmp
                            {
                                WorkOrderId = (dr[0]).ToString(),
                                RequiredOrder = dr[1].ToString(),
                                ByUser = dr[2].ToString(),
                                CustomerName = dr[3].ToString(),
                                Duration = dr[4].ToString() +"يوم",
                                EndDate = dr[5].ToString(),
                                EmpName = dr[6].ToString(),
                                JobName = dr[7].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetDoneWorkOrdersByExecEmp> lmd = new List< rptGetDoneWorkOrdersByExecEmp>();
                return lmd;
            }

        }

        public async Task<IEnumerable< rptGetOnGoingWorkOrdersByExecEmp>> GetEmpUnderGoingWOsDGV(int UserID, string Con)
        {
            try
            {
                List< rptGetOnGoingWorkOrdersByExecEmp> lmd = new List< rptGetOnGoingWorkOrdersByExecEmp>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetOnGoingWorkOrdersByExecEmp";
                        command.Connection = con;

                        if (UserID == 0)
                        {
                            UserID = 0;
                            command.Parameters.Add(new SqlParameter("@EmpID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpID", UserID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetOnGoingWorkOrdersByExecEmp
                            {
                                WorkOrderId = (dr[0]).ToString(),
                                RequiredOrder = dr[1].ToString(),
                                ByUser = dr[2].ToString(),
                                CustomerName = dr[3].ToString(),
                                Duration = dr[4].ToString(),
                                LeftDuration = dr[5].ToString(),
                                EmpName = dr[6].ToString(),
                                JobName = dr[7].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetOnGoingWorkOrdersByExecEmp> lmd = new List< rptGetOnGoingWorkOrdersByExecEmp>();
                return lmd;
            }

        }

        public async Task< IEnumerable< rptGetDelayedWorkOrdersByExecEmpVM>> GetEmpDelayedWOsDGV(int UserID, string Con)
        {
            try
            {
                List< rptGetDelayedWorkOrdersByExecEmpVM> lmd = new List< rptGetDelayedWorkOrdersByExecEmpVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetDelayedWorkOrdersByExecEmp";
                        command.Connection = con;

                        if (UserID == 0)
                        {
                            UserID = 0;
                            command.Parameters.Add(new SqlParameter("@EmpID", "0"));

                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@EmpID", UserID));
                        }
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetDelayedWorkOrdersByExecEmpVM
                            {
                                WorkOrderId = (dr[0]).ToString(),
                                RequiredOrder = dr[1].ToString(),
                                ByUser = dr[2].ToString(),
                                CustomerName = dr[3].ToString(),
                                Duration = dr[4].ToString(),
                                DelayDuration = dr[5].ToString(),
                                EmpName = dr[6].ToString(),
                                JobName = dr[7].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetDelayedWorkOrdersByExecEmpVM> lmd = new List< rptGetDelayedWorkOrdersByExecEmpVM>();
                return lmd;
            }

        }

         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksReport(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo)
        {
          
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.BranchId == BranchId && s.IsMerig == -1 && s.IsRetrieved == 1 && (UserId == null || s.UserId == UserId)).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(****)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(****)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project.ProjectDescription,
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,

                    PercentComplete = x.PercentComplete
                    //PercentComplete = (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100 ) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100)  : 100,
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                                    TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                    TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,

                });


                return projectPhasesTasks;

            }



         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAlmostLateTasksByUserId(int userid,string startdate,string enddate,int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false &&(BranchId==0 ||s.BranchId== BranchId) && s.Type == 3 && s.UserId == userid && s.IsMerig == -1 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                DescriptionAr = x.DescriptionAr,
                ProjectNumber = x.Project!.ProjectNo,
                Remaining = x.Remaining,
                EndTime = x.EndTime,
                TaskFullTime = x.TaskFullTime,
                ExecPercentage = x.ExecPercentage,
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                TaskLongDesc = x.TaskLongDesc,
                PercentComplete = x.TimeType == 1 ? (x.TimeMinutes * 60) * 70 / 100 : (x.TimeMinutes * 24 * 60) * 70 / 100,
                PlayingTime = x.TimeType == 1 ? (x.TimeMinutes * 60) * 100 / 100 : (x.TimeMinutes * 24 * 60) * 100 / 100,
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList().Where(m => (string.IsNullOrEmpty(startdate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(enddate) || string.IsNullOrEmpty(m.EndDate) || DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

            return projectPhasesTasks.Where(s=>s.PercentComplete <= s.Remaining && s.Remaining <= s.PlayingTime);
        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdreport( string startdate,string EndDateP, int? UserId, int BranchId, string Lang)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 &&(BranchId==0 || s.BranchId == BranchId) && s.UserId == UserId && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                DescriptionAr = x.DescriptionAr,
                ProjectNumber = x.Project!.ProjectNo,
                Remaining = x.Remaining,
                EndTime = x.EndTime,
                TaskFullTime = x.TaskFullTime,
                ExecPercentage = x.ExecPercentage,
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                TaskLongDesc = x.TaskLongDesc,
                IsRetrieved = x.IsRetrieved,

                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList().Where(m => (string.IsNullOrEmpty(startdate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(EndDateP) || string.IsNullOrEmpty(m.EndDate) || DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
            //if (EndDateP != null && EndDateP != "")
            //{

            //     projectPhasesTasks.Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            //}
   

            return projectPhasesTasks;
        }


         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserIdReport(string startdate,string EndDateP, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.UserId == UserId && s.Type == 3&&(BranchId ==0 ||s.BranchId== BranchId) && s.Status == 1 && s.IsMerig == -1 && (s.Remaining > 0 || s.Remaining == null)).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                PhasesTaskName = Lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                //TimeStr = x.TimeMinutes + " يوم  ",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                        x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                        x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = Lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                StatusName = x.Status == 0 ? "غير معلومة" :
                          x.Status == 1 ? " لم تبدأ " :
                          x.Status == 2 ? " قيد التشغيل " :
                          x.Status == 3 ? " متوقفة " :
                          x.Status == 4 ? " منتهية " :
                          x.Status == 5 ? " ملغاة " :
                          x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",

                TaskLongDesc = x.TaskLongDesc,
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,
                SettingId = x.SettingId,
                PercentComplete = x.PercentComplete,
                IsRetrieved = x.IsRetrieved

            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);
            //if(EndDateP != null && EndDateP != "") {
            // projectPhasesTasks.ToList().Where(s => (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) == DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndTime, "h:mm", CultureInfo.InvariantCulture) < DateTime.ParseExact(DateTime.Now.ToString("h:mm"), "h:mm", CultureInfo.InvariantCulture)) || DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            //}
         return projectPhasesTasks.ToList().Where(m => (string.IsNullOrEmpty(startdate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(EndDateP) || string.IsNullOrEmpty(m.EndDate) || DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
         


        }




         public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string DateFrom, string DateTo, string Lang,int BranchId, List<int> BranchesList)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && BranchesList.Contains(s.BranchId ?? 0) && s.Type == 3 && (UserId == null || s.UserId == UserId) && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project!.ProjectDescription,
                FirstProjectDate = x.Project!.FirstProjectDate,
                TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",
       

                PercentComplete = x.PercentComplete,
                TaskNo=x.TaskNo??null,
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList().Where(s => (string.IsNullOrEmpty(s.EndDate) || (!string.IsNullOrEmpty(s.EndDate) && (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)))) &&
                                   (string.IsNullOrEmpty(s.StartDate) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))));
            
            
            return projectPhasesTasks;
        }

        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string DateFrom, string DateTo, string Lang, int BranchId,string? searchtext)
        {
            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Type == 3 && (UserId == null || s.UserId == UserId) && s.IsMerig == -1 && s.Remaining < 0 && s.Status != 4 &&
            (s.Project.customer.CustomerNameAr.Contains((searchtext??"")) || s.Project.ProjectDescription.Contains((searchtext??"")) ||
            s.DescriptionAr.Contains((searchtext??"")) || s.Project.ProjectNo.Contains((searchtext??"")) || s.Project.ProjectName.Contains((searchtext??"")) ||
            s.Project.projecttype.NameAr.Contains((searchtext??"")) || searchtext == null || searchtext == "")).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
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
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate != null ? x.EndDate : x.EndDateCalc ?? "",

                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                Cost = x.Cost,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = x.Project!.customer!.CustomerNameAr,
                ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ProjectNumber = x.Project!.ProjectNo,
                ProjectDescription = x.Project!.ProjectDescription,
                FirstProjectDate = x.Project!.FirstProjectDate,
                TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "ساعة",
                TimeStr = Lang == "ltr" ? x.TimeType == 1 ? (x.TimeMinutes) + " Hour " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " Day " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " Week " : (x.TimeMinutes) + " Month " : x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                                x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :
                                x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " : (x.TimeMinutes) + " ساعة ",


                PercentComplete = x.PercentComplete,
                TaskNo=x.TaskNo??null,
                //TimeStr = (DateTime.ParseExact(EndDateP, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays + "Day",
            }).ToList().Where(s => (string.IsNullOrEmpty(s.EndDate) || (!string.IsNullOrEmpty(s.EndDate) && (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture)))) &&
                                   (string.IsNullOrEmpty(s.StartDate) || (!string.IsNullOrEmpty(s.StartDate) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))));


            return projectPhasesTasks;
        }


        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksINprogressByUserIdUser(int? UserId, string lang, int Status, int BranchId,string startdate ,string enddate)
        {

            var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId &&(BranchId ==0 || s.BranchId== BranchId) && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
            {
                PhaseTaskId = x.PhaseTaskId,
                PhasesTaskName = lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                UserId = x.UserId,NumAdded=x.NumAdded??0,
                ProjectId = x.ProjectId,
                TimeMinutes = x.TimeMinutes,
                TimeType = x.TimeType,
                Remaining = x.Remaining,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                Cost = x.Cost,
                Status = x.Status,
                OldStatus = x.OldStatus,
                Active = x.Active,
                StopCount = x.StopCount,
                UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                OrderNo = x.OrderNo ?? 1000000,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ExcpectedStartDate = x.ExcpectedStartDate,
                ExcpectedEndDate = x.ExcpectedEndDate,
                EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                ToUserId = x.ToUserId,
                Notes = x.Notes ?? "",
                BranchId = x.BranchId,
                PhasePriority = x.PhasePriority,
                ExecPercentage = x.ExecPercentage,
                //MainPhaseName = x.MainPhase!.DescriptionAr,
                SubPhaseName = x.SubPhase!.DescriptionAr,
                ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                ClientName_W = lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                ProjectMangerName = x.Project!.Users!.FullName,
                ProjectNumber = x.Project!.ProjectNo,
                TaskLongDesc = x.TaskLongDesc,
                StatusName = x.Status == 0 ? "غير معلومة" :
                  x.Status == 1 ? " لم تبدأ " :
                  x.Status == 2 ? " قيد التشغيل " :
                  x.Status == 3 ? " متوقفة " :
                  x.Status == 4 ? " منتهية " :
                  x.Status == 5 ? " ملغاة " :
                  x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                PlayingTime = x.TimeMinutes - x.Remaining,
                StopProjectType = x.Project.StopProjectType ?? 0,

                PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
            }).ToList().Select(s => new ProjectPhasesTasksVM()
            {
                PhaseTaskId = s.PhaseTaskId,
                PhasesTaskName = s.PhasesTaskName,
                DescriptionAr = s.DescriptionAr,
                DescriptionEn = s.DescriptionEn,
                ParentId = s.ParentId,
                ProjSubTypeId = s.ProjSubTypeId,
                Type = s.Type,
                UserId = s.UserId,NumAdded=s.NumAdded??0,
                ProjectId = s.ProjectId,
                TimeMinutes = s.TimeMinutes,
                TimeType = s.TimeType,
                Remaining = s.Remaining,
                IsUrgent = s.IsUrgent,
                UserName = s.UserName ?? "",
                IsTemp = s.IsTemp,
                TaskType = s.TaskType,
                Status = s.Status,
                OldStatus = s.OldStatus,
                Active = s.Active,
                StopCount = s.StopCount,
                Cost = s.Cost,
                TaskTypeName = s.TaskTypeName,
                OrderNo = s.OrderNo ?? 1000000,
                                TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                EndDateCalc = s.EndDateCalc ?? "",

                PercentComplete = s.PercentComplete,
                ToUserId = s.ToUserId,
                Notes = s.Notes,
                BranchId = s.BranchId,
                PhasePriority = s.PhasePriority,
                ExecPercentage = s.ExecPercentage,
                MainPhaseName = s.MainPhaseName,
                SubPhaseName = s.SubPhaseName,
                ProjectSubTypeName = s.ProjectSubTypeName,
                ProjectTypeName = s.ProjectTypeName,
                ClientName = s.ClientName,
                ClientName_W = s.ClientName_W,

                ProjectMangerName = s.ProjectMangerName,
                ProjectNumber = s.ProjectNumber,
                TaskLongDesc = s.TaskLongDesc,
                StatusName = //s.Status == 0 ? "غير معلومة" :
                            s.Status == 1 ? " لم تبدأ " :
                            s.Status == 2 ? " قيد التشغيل " :
                            s.Status == 3 ? " متوقفة " :
                            s.Status == 4 ? " منتهية "
                            //s.Status == 5 ? " ملغاة " :
                            //s.Status == 6 ? " محذوفة " 
                            : " تم تحويلها",
                StopProjectType = s.StopProjectType,

                PlayingTime = s.PlayingTime,
            }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);

            if (Status == 0)
            {
                projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status != 4 && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    PhasesTaskName = lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Cost = x.Cost,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    OrderNo = x.OrderNo ?? 1000000,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                  x.Status == 1 ? " لم تبدأ " :
                  x.Status == 2 ? " قيد التشغيل " :
                  x.Status == 3 ? " متوقفة " :
                  x.Status == 4 ? " منتهية " :
                  x.Status == 5 ? " ملغاة " :
                  x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    StopProjectType = x.Project.StopProjectType ?? 0,

                    PlayingTime = x.TimeMinutes - x.Remaining,
                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                }).ToList().Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    PhasesTaskName = s.PhasesTaskName,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
                    ProjectId = s.ProjectId,
                    TimeMinutes = s.TimeMinutes,
                    TimeType = s.TimeType,
                    Remaining = s.Remaining,
                    IsUrgent = s.IsUrgent,
                    UserName = s.UserName ?? "",
                    IsTemp = s.IsTemp,
                    TaskType = s.TaskType,
                    Status = s.Status,
                    OldStatus = s.OldStatus,
                    Active = s.Active,
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                                    TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                    TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = //s.Status == 0 ? "غير معلومة" :
                                s.Status == 1 ? " لم تبدأ " :
                                s.Status == 2 ? " قيد التشغيل " :
                                s.Status == 3 ? " متوقفة " :
                                s.Status == 4 ? " منتهية "
                                //s.Status == 5 ? " ملغاة " :
                                //s.Status == 6 ? " محذوفة " 
                                : " تم تحويلها",
                    StopProjectType = s.StopProjectType,

                    PlayingTime = s.PlayingTime,
                }).ToList().OrderBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);

                var Late = projectPhasesTasks.Where(x => x.Remaining < 0).ToList().OrderByDescending(x => x.PhasePriority).ThenByDescending(x => x.PhasePriority).ToList();
                var Rest = projectPhasesTasks.Where(x => x.Remaining >= 0 || !x.Remaining.HasValue).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining).ToList();
                var total = Late.Union(Rest);
                return total;
            }
            else
            {
                projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId && s.Status == Status && s.IsMerig == -1).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    PhasesTaskName = lang == "ltr" ? x.DescriptionEn : x.DescriptionAr,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,NumAdded=x.NumAdded??0,
                    ProjectId = x.ProjectId,
                    TimeMinutes = x.TimeMinutes,
                    TimeType = x.TimeType,
                    Remaining = x.Remaining,
                    IsUrgent = x.IsUrgent,
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    Cost = x.Cost,
                    Status = x.Status,
                    OldStatus = x.OldStatus,
                    Active = x.Active,
                    StopCount = x.StopCount,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    OrderNo = x.OrderNo ?? 1000000,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDate!=null ? x.EndDate: x.EndDateCalc ?? "",

                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = lang == "ltr" ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.CustomerNameAr,
                    ClientName = x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,

                    ProjectMangerName = x.Project!.Users!.FullName,
                    ProjectNumber = x.Project!.ProjectNo,
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                x.Status == 1 ? " لم تبدأ " :
                x.Status == 2 ? " قيد التشغيل " :
                x.Status == 3 ? " متوقفة " :
                x.Status == 4 ? " منتهية " :
                x.Status == 5 ? " ملغاة " :
                x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,

                    PercentComplete = x.TimeMinutes != 0 ? x.TimeMinutes != null ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) <= 100 ? (((x.TimeMinutes - x.Remaining) / x.TimeMinutes) * 100) : 100 : 0 : 0,
                }).ToList().Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    PhasesTaskName = s.PhasesTaskName,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,NumAdded=s.NumAdded??0,
                    ProjectId = s.ProjectId,
                    TimeMinutes = s.TimeMinutes,
                    TimeType = s.TimeType,
                    Remaining = s.Remaining,
                    IsUrgent = s.IsUrgent,
                    UserName = s.UserName ?? "",
                    IsTemp = s.IsTemp,
                    TaskType = s.TaskType,
                    Status = s.Status,
                    OldStatus = s.OldStatus,
                    Active = s.Active,
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo ?? 1000000,
                                    TaskStart = s.StartDate!=null?s.StartDate : s.ExcpectedStartDate!=null ? s.ExcpectedStartDate:"",
                                    TaskEnd = s.EndDate != null ? s.EndDate : s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = //s.Status == 0 ? "غير معلومة" :
                                s.Status == 1 ? " لم تبدأ " :
                                s.Status == 2 ? " قيد التشغيل " :
                                s.Status == 3 ? " متوقفة " :
                                s.Status == 4 ? " منتهية "
                                //s.Status == 5 ? " ملغاة " :
                                //s.Status == 6 ? " محذوفة " 
                                : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    StopProjectType = s.StopProjectType,

                }).ToList().OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining);

                if (Status == 2)
                {
                    projectPhasesTasks = projectPhasesTasks.OrderBy(x => x.Remaining).ThenBy(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority);
                }
                //.OrderByDescending(x => x.Status).ThenBy(x => x.OrderNo).ThenByDescending(x => x.PhasePriority).ThenBy(x => x.Remaining); 
            }


            //if (searchtext != "")
            //{
            //    projectPhasesTasks = projectPhasesTasks.Where(s => s.DescriptionAr.Contains((searchtext??"").Trim()) || s.DescriptionEn.Contains((searchtext??"").Trim()) || s.ProjectNumber.Equals(searchtext.Trim())).ToList();
            //}
            return projectPhasesTasks.ToList().Where(m => (string.IsNullOrEmpty(startdate) || (!string.IsNullOrEmpty(m.StartDate) && DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(enddate) || string.IsNullOrEmpty(m.EndDate) || DateTime.ParseExact(m.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture)));

        }

        public IEnumerable<ProjectPhasesTasks> GetAll()
        {
            throw new NotImplementedException();
        }

        public ProjectPhasesTasks GetById(int Id)
        {
            return _TaamerProContext.ProjectPhasesTasks.Where(x => x.PhaseTaskId == Id).FirstOrDefault();
        }

        public IEnumerable<ProjectPhasesTasks> GetMatching(Func<ProjectPhasesTasks, bool> where)
        {
            return _TaamerProContext.ProjectPhasesTasks.Where(where).ToList<ProjectPhasesTasks>();

        }




        public async Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_Costs(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo, List<int> BranchesList)
        {
            try
            {
                var projectPhasesTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3 && BranchesList.Contains(s.BranchId ?? 0) && s.IsMerig == -1 && s.Status == 4 && s.UserId == UserId).Select(x => new ProjectPhasesTasksVM
                {
                    PhaseTaskId = x.PhaseTaskId,
                    DescriptionAr = x.DescriptionAr,
                    DescriptionEn = x.DescriptionEn,
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    UserId = x.UserId,
                    NumAdded = x.NumAdded ?? 0,
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
                    ExcpectedStartDate = x.ExcpectedStartDate,
                    ExcpectedEndDate = x.ExcpectedEndDate,
                    EndDateCalc = x.EndDateCalc != null ? x.EndDateCalc : x.EndDate != null ? x.EndDate : x.ExcpectedEndDate ?? "",
                    ToUserId = x.ToUserId,
                    Notes = x.Notes ?? "",
                    BranchId = x.BranchId,
                    PhasePriority = x.PhasePriority,
                    ExecPercentage = x.ExecPercentage,
                    Cost = x.Cost,
                    //MainPhaseName = x.MainPhase!.DescriptionAr,
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                    SubPhaseName = x.SubPhase!.DescriptionAr,
                    ProjectSubTypeName = x.ProjectSubTypes!.NameAr,
                    ProjectTypeName = x.ProjectSubTypes!.ProjectType!.NameAr,
                    ClientName_W = x.Project!.customer!.CustomerNameAr,
                    ClientName = Lang == "ltr" ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameEn : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameEn + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameEn + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameEn + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameEn + "(VIP)" : x.Project!.customer!.CustomerNameAr
                : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr,
                    TaskStart = x.StartDate != null ? x.StartDate : x.ExcpectedStartDate != null ? x.ExcpectedStartDate : "",
                    TaskEnd = x.EndDate != null ? x.EndDate : x.ExcpectedEndDate != null ? x.ExcpectedEndDate : "",
                    ProjectMangerName = x.Project!.Users!.FullName ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعه" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    ProjectNumber = x.Project!.ProjectNo,
                    ProjectDescription = x.Project.ProjectDescription,
                    FirstProjectDate = x.Project!.FirstProjectDate,
                    TimeStrProject = (x.Project!.NoOfDays) != 0.0 ? (x.Project!.NoOfDays) + " يوم " : "",
                    TaskLongDesc = x.TaskLongDesc,
                    StatusName = x.Status == 0 ? "غير معلومة" :
                             x.Status == 1 ? " لم تبدأ " :
                             x.Status == 2 ? " قيد التشغيل " :
                             x.Status == 3 ? " متوقفة " :
                             x.Status == 4 ? " منتهية " :
                             x.Status == 5 ? " ملغاة " :
                             x.Status == 6 ? " محذوفة " : x.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = x.TimeMinutes - x.Remaining,
                    StopProjectType = x.Project.StopProjectType ?? 0,
                    TaskTimeFrom = x.TaskTimeFrom ?? "",
                    TaskTimeTo = x.TaskTimeTo ?? "",
                    PercentComplete = x.PercentComplete,
                    Totaltaskcost = x.Totaltaskcost ?? (
                                (x.EndDateNew != null && x.StartDateNew != null) ?
                                ((decimal)(x.EndDateNew.Value - x.StartDateNew.Value).TotalHours *
                                 (_TaamerProContext.Employees.Where(e => e.UserId == x.UserId)
                                                             .Select(e => e.EmpHourlyCost)
                                                             .FirstOrDefault() ?? 0)) :
                                ((decimal)(
                                    x.TimeType == 1 ? x.TimeMinutes :      // Hours
                                    x.TimeType == 2 ? x.TimeMinutes * 8 : // Days
                                    x.TimeType == 3 ? x.TimeMinutes * 56 : // Weeks
                                    x.TimeMinutes * 240 // Months (default)
                                ) * (_TaamerProContext.Employees.Where(e => e.UserId == x.UserId)
                                                                 .Select(e => e.EmpHourlyCost)
                                                                 .FirstOrDefault() ?? 0))
                            ),

                                                Totalhourstask = x.Totalhourstask ?? (
                                (x.EndDateNew != null && x.StartDateNew != null) ?
                                (decimal)(x.EndDateNew.Value - x.StartDateNew.Value).TotalHours :
                                (decimal)(
                                    x.TimeType == 1 ? x.TimeMinutes :      // Hours
                                    x.TimeType == 2 ? x.TimeMinutes * 8 : // Days
                                    x.TimeType == 3 ? x.TimeMinutes * 56 : // Weeks
                                    x.TimeMinutes * 240 // Months (default)
                                )
                            )
                }).ToList().Where(m => (string.IsNullOrEmpty(DateFrom) || (!string.IsNullOrEmpty(m.TaskStart) && DateTime.ParseExact(m.TaskStart, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture))) && (string.IsNullOrEmpty(DateTo) || string.IsNullOrEmpty(m.EndDateCalc) || DateTime.ParseExact(m.EndDateCalc, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Select(s => new ProjectPhasesTasksVM()
                {
                    PhaseTaskId = s.PhaseTaskId,
                    DescriptionAr = s.DescriptionAr,
                    DescriptionEn = s.DescriptionEn,
                    ParentId = s.ParentId,
                    ProjSubTypeId = s.ProjSubTypeId,
                    Type = s.Type,
                    UserId = s.UserId,
                    NumAdded = s.NumAdded ?? 0,
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
                    UserName = s.UserName ?? "",
                    StopCount = s.StopCount,
                    Cost = s.Cost,
                    TaskTypeName = s.TaskTypeName,
                    OrderNo = s.OrderNo,
                    TaskStart = s.TaskStart,
                    TaskEnd = s.EndDate,
                    ExcpectedStartDate = s.ExcpectedStartDate != null ? s.ExcpectedStartDate : "",
                    ExcpectedEndDate = s.ExcpectedEndDate != null ? s.ExcpectedEndDate : "",
                    EndDateCalc = s.EndDateCalc ?? "",

                    PercentComplete = s.PercentComplete,
                    ToUserId = s.ToUserId,
                    Notes = s.Notes,
                    BranchId = s.BranchId,
                    PhasePriority = s.PhasePriority,
                    ExecPercentage = s.ExecPercentage,
                    MainPhaseName = s.MainPhaseName,
                    SubPhaseName = s.SubPhaseName,
                    ProjectSubTypeName = s.ProjectSubTypeName,
                    ProjectTypeName = s.ProjectTypeName,
                    ClientName = s.ClientName,
                    ClientName_W = s.ClientName_W,

                    ProjectMangerName = s.ProjectMangerName,
                    ProjectNumber = s.ProjectNumber,
                    ProjectDescription = s.ProjectDescription,
                    FirstProjectDate = s.FirstProjectDate,
                    TimeStrProject = s.TimeStrProject,
                    TaskLongDesc = s.TaskLongDesc,
                    StatusName = s.Status == 0 ? "غير معلومة" :
                                  s.Status == 1 ? " لم تبدأ " :
                                  s.Status == 2 ? " قيد التشغيل " :
                                  s.Status == 3 ? " متوقفة " :
                                  s.Status == 4 ? " منتهية " :
                                  s.Status == 5 ? " ملغاة " :
                                  s.Status == 6 ? " محذوفة " : s.Status == 7 ? " متوقفة لشرط إداري " : " تم تحويلها",
                    PlayingTime = s.PlayingTime,
                    //edit
                    TimeTypeName = s.TimeType == 1 ? "ساعة" : s.TimeType == 2 ? "يوم" : s.TimeType == 3 ? "أسبوع" : "ساعة",
                    TimeStr = Lang == "ltr" ? s.TimeType == 1 ? (s.TimeMinutes) + " Hour " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " Day " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " Week " : (s.TimeMinutes) + " Month " : s.TimeType == 1 ? (s.TimeMinutes) + " ساعه " :
                                s.TimeType == 2 ? (s.TimeMinutes) + " يوم " :
                                s.TimeType == 3 ? (s.TimeMinutes) + " أسبوع " : (s.TimeMinutes) + " ساعة ",
                    StopProjectType = s.StopProjectType,
                    TaskTimeFrom = s.TaskTimeFrom ?? "",
                    TaskTimeTo = s.TaskTimeTo ?? "",
                    Totalhourstask = s.Totalhourstask,
                    Totaltaskcost = s.Totaltaskcost,
                });


                return projectPhasesTasks;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }

        // Helper Function
        private decimal GetHoursFromTime(int timeMinutes, int timeType)
        {
            return timeType switch
            {
                1 => (decimal)timeMinutes,        // Hours
                2 => (decimal)timeMinutes * 24,   // Days -> Convert to Hours
                3 => (decimal)timeMinutes * 168,  // Weeks -> Convert to Hours (7 days * 24 hours)
                _ => (decimal)timeMinutes * 720   // Default: Month -> Convert to Hours (30 days * 24 hours)
            };
        }
        public async Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int PhasesTaskId)
        {
            var projectPhasesTasks = _TaamerProContext.Pro_TaskOperations.Where(s => s.IsDeleted == false && s.PhaseTaskId == PhasesTaskId).Select(x => new Pro_TaskOperationsVM
            {
                TaskOperationId = x.TaskOperationId,
                PhaseTaskId = x.PhaseTaskId,
                WorkOrderId = x.WorkOrderId,
                Type = x.Type,
                OperationName = x.OperationName,
                Date = x.Date,
                UserId = x.UserId,
                BranchId = x.BranchId,
                Note = x.Note,
                TaskNo = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.TaskNo ?? null : null,
                DescriptionAr = x.ProjectPhasesTasks != null ? x.ProjectPhasesTasks.DescriptionAr ?? null : null,
                ExtraNote = x.UserId != null ? x.Users != null ? " تم تحويلها الي :  " + (x.Users.FullNameAr ?? x.Users.FullName) : null : null,
                AddUserName = x.AddUsers != null ? (x.AddUsers.FullNameAr ?? x.AddUsers.FullName) ?? null : null,
            }).ToList();

            return projectPhasesTasks;
        }

        public async Task<int> GenerateNextTaskNumber(int BranchId, string codePrefix, int? ProjectId)
        {
            if (_TaamerProContext.ProjectPhasesTasks != null)
            {
                var lastRow = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/ && s.TaskNoType == 1 && s.TaskNo!.Contains(codePrefix)).OrderByDescending(u => u.TaskNo).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {

                        var TaskNumber = 0;

                        if (codePrefix == "")
                        {
                            TaskNumber = int.Parse(lastRow!.TaskNo!) + 1;
                        }
                        else
                        {
                            TaskNumber = int.Parse(lastRow!.TaskNo!.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return TaskNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }


    }
}
