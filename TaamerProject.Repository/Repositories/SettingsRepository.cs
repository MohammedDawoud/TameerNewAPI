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
    public class SettingsRepository : ISettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<SettingsVM>> GetAllMainPhases(int? ProSubTypeId,int BranchId)
        {
            var settings = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProSubTypeId && s.ParentId == null && s.Type == 1 && s.IsMerig == -1).Select(x => new SettingsVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr,
                DescriptionEn = x.DescriptionEn,
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeMinutes = x.TimeMinutes,
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                Notes = x.Notes ?? "",
                RequirmentId = x.RequirmentId,
            });
            return settings;
        }
        public async Task<IEnumerable<SettingsVM>> GetAllSubPhases(int? ParentId, int BranchId)
        {
            var settings = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ParentId == ParentId && s.Type == 2 && s.IsMerig == -1).Select(x => new SettingsVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr ?? "",
                DescriptionEn = x.DescriptionEn ?? "",
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? x.TimeMinutes:
                              x.TimeType == 2 ? (x.TimeMinutes):// * 24 ) :
                              x.TimeType == 3 ? (x.TimeMinutes):// * (24 * 7)) :
                              (x.TimeMinutes),// * (24 * 7 * 30)),
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                UserName = x.Users!.FullNameAr ==null? x.Users.FullName : x.Users.FullNameAr ??"",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                          x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                          x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " :// * (24 * 7)
                          (x.TimeMinutes ) + " شهر ",//* (24 * 7 * 30)
                Notes = x.Notes ?? "",
                Cost = x.Cost,
                RequirmentId = x.RequirmentId
            }).ToList();
            return settings;
        }

        public async Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectID(int ProSubTypeId)
        {
            var settings = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProSubTypeId && s.IsMerig == -1).Select(x => new SettingsVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr ?? "",
                DescriptionEn = x.DescriptionEn ?? "",
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? x.TimeMinutes :
                             x.TimeType == 2 ? (x.TimeMinutes) :// * 24 ) :
                             x.TimeType == 3 ? (x.TimeMinutes) :// * (24 * 7)) :
                             (x.TimeMinutes),// * (24 * 7 * 30)),
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                UserName = x.Users!.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " :// * (24 * 7)
                         (x.TimeMinutes) + " شهر ",//* (24 * 7 * 30)
                Notes = x.Notes ?? "",
                NodeLocation = x.NodeLocations!.Location,
                TaskFullDescription = x.DescriptionAr + "/" + x.Users.FullName + "/" + x.TimeMinutes,
                Cost = x.Cost,
                RequirmentId = x.RequirmentId,

            }).ToList();
            return settings;
        }
        public async Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDNew(int ProSubTypeId)
        {
            var settings = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProSubTypeId && s.IsMerig == -1).Select(x => new SettingsNewVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr ?? "",
                DescriptionEn = x.DescriptionEn ?? "",
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? x.TimeMinutes :
                             x.TimeType == 2 ? (x.TimeMinutes) :// * 24 ) :
                             x.TimeType == 3 ? (x.TimeMinutes) :// * (24 * 7)) :
                             (x.TimeMinutes),// * (24 * 7 * 30)),
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                UserName = x.Users==null?"": x.Users!.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " :// * (24 * 7)
                         (x.TimeMinutes) + " شهر ",//* (24 * 7 * 30)
                Notes = x.Notes ?? "",
                NodeLocation = x.NodeLocations!.Location,
                TaskFullDescription =x.Users==null?x.DescriptionAr: x.DescriptionAr + "/" + x.Users.FullName + "/" + x.TimeMinutes,
                Cost = x.Cost,
                RequirmentId = x.RequirmentId,
                indentation = x.indentation,
                taskindex = x.taskindex,
                Managerapproval = x.Managerapproval,
                ReasonsId = x.ReasonsId,

            }).ToList();
            return settings;
        }

        public async Task<IEnumerable<SettingsVM>> GetAllSettingsByProjectIDAll()
        {
            var settings = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.IsMerig == -1).Select(x => new SettingsVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr ?? "",
                DescriptionEn = x.DescriptionEn ?? "",
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? x.TimeMinutes :
                             x.TimeType == 2 ? (x.TimeMinutes) :// * 24 ) :
                             x.TimeType == 3 ? (x.TimeMinutes) :// * (24 * 7)) :
                             (x.TimeMinutes),// * (24 * 7 * 30)),
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                UserName = x.Users!.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " :// * (24 * 7)
                         (x.TimeMinutes) + " شهر ",//* (24 * 7 * 30)
                Notes = x.Notes ?? "",
                NodeLocation = x.NodeLocations!.Location,
                TaskFullDescription = x.DescriptionAr + "/" + x.Users.FullName + "/" + x.TimeMinutes,
                Cost = x.Cost,
                RequirmentId = x.RequirmentId,

            }).ToList();
            return settings;
        }
        public async Task<IEnumerable<SettingsNewVM>> GetAllSettingsByProjectIDAllNew()
        {
            var settings = _TaamerProContext.SettingsNew.Where(s => s.IsDeleted == false && s.IsMerig == -1).Select(x => new SettingsNewVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr ?? "",
                DescriptionEn = x.DescriptionEn ?? "",
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? x.TimeMinutes :
                             x.TimeType == 2 ? (x.TimeMinutes) :// * 24 ) :
                             x.TimeType == 3 ? (x.TimeMinutes) :// * (24 * 7)) :
                             (x.TimeMinutes),// * (24 * 7 * 30)),
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                UserName = x.Users!.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " :// * (24 * 7)
                         (x.TimeMinutes) + " شهر ",//* (24 * 7 * 30)
                Notes = x.Notes ?? "",
                NodeLocation = x.NodeLocations!.Location,
                TaskFullDescription = x.DescriptionAr + "/" + x.Users.FullName + "/" + x.TimeMinutes,
                Cost = x.Cost,
                RequirmentId = x.RequirmentId,
                Managerapproval = x.Managerapproval,
                ReasonsId = x.ReasonsId,

            }).ToList();
            return settings;
        }

        public async Task<IEnumerable<SettingsVM>> GetAllTasks(int? ProSubTypeId, int BranchId)
        {
            var settings = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ProjSubTypeId == ProSubTypeId  && s.Type == 3 && s.IsMerig == -1).Select(x => new SettingsVM
            {
                SettingId = x.SettingId,
                DescriptionAr = x.DescriptionAr ?? "",
                DescriptionEn = x.DescriptionEn ?? "",
                ParentId = x.ParentId,
                ProjSubTypeId = x.ProjSubTypeId,
                Type = x.Type,
                TimeType = x.TimeType,
                TimeMinutes = x.TimeType == 1 ? x.TimeMinutes :
                                x.TimeType == 2 ? (x.TimeMinutes) :// * 24) :
                                x.TimeType == 3 ? (x.TimeMinutes) :// * (24 * 7)) :
                                (x.TimeMinutes),// * (24 * 7 * 30)),
                IsUrgent = x.IsUrgent,
                IsTemp = x.IsTemp,
                TaskType = x.TaskType,
                OrderNo = x.OrderNo,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentComplete = x.PercentComplete,
                BranchId = x.BranchId,
                UserId = x.UserId,
                UserName = x.Users!.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr ?? "",
                TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                            x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                            x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع" ://* (24 * 7)
                            (x.TimeMinutes) + " شهر ",//* (24 * 7 * 30)
                NodeLocation = x.NodeLocations!.Location,
                Notes = x.Notes ?? "",
                TaskFullDescription = x.DescriptionAr + "/" + x.Users.FullName + "/" + x.TimeMinutes,
                Cost = x.Cost,
                RequirmentId=x.RequirmentId
            });
            return settings;
        }
        public async Task<IEnumerable<SettingsVM>> GetAllTasksByPhaseId(int PhaseId)
        {
                var settings = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.ParentId == PhaseId && s.Type == 3 && s.IsMerig == -1).Select(x => new SettingsVM
                {
                    SettingId = x.SettingId,
                    DescriptionAr = x.DescriptionAr ?? "",
                    DescriptionEn = x.DescriptionEn ?? "",
                    ParentId = x.ParentId,
                    ProjSubTypeId = x.ProjSubTypeId,
                    Type = x.Type,
                    TimeType = x.TimeType,
                    TimeMinutes = x.TimeType == 1 ? x.TimeMinutes :
                             x.TimeType == 2 ? (x.TimeMinutes) :// * 24 ) :
                             x.TimeType == 3 ? (x.TimeMinutes) :// * (24 * 7)) :
                             (x.TimeMinutes),// * (24 * 7 * 30)),
                    IsUrgent = x.IsUrgent,
                    IsTemp = x.IsTemp,
                    TaskType = x.TaskType,
                    OrderNo = x.OrderNo,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    PercentComplete = x.PercentComplete,
                    BranchId = x.BranchId,
                    UserId = x.UserId,
                    UserName = x.Users!.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr ?? "",
                    TaskTypeName = x.TaskTypeModel!.NameAr,//x.TaskType == 1 ? "رفع ملفات" : x.TaskType == 2 ? "تحصيل دفعة" : x.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                    TimeTypeName = x.TimeType == 1 ? "ساعة" : x.TimeType == 2 ? "يوم" : x.TimeType == 3 ? "أسبوع" : "شهر",
                    TimeStr = x.TimeType == 1 ? (x.TimeMinutes) + " ساعه " :
                         x.TimeType == 2 ? (x.TimeMinutes) + " يوم " :// * 24
                         x.TimeType == 3 ? (x.TimeMinutes) + " أسبوع " :// * (24 * 7)
                         (x.TimeMinutes) + " شهر ",//* (24 * 7 * 30)
                    Notes = x.Notes ?? "",
                    Cost = x.Cost,
                    IsUserDeleted=x.Users.IsDeleted,
                    VacationCount=0,
                    RequirmentId = x.RequirmentId

                });
                return settings;
            }

    }
}
