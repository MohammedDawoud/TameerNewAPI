using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Pro_ProjectAchievementsRepository: IPro_ProjectAchievementsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_ProjectAchievementsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievements()
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectAchievements.Where(s => s.IsDeleted == false).Select(x => new Pro_ProjectAchievementsVM
                {
                    ProjectAchievementId = x.ProjectAchievementId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    ProjectId = x.ProjectId,
                    StepId = x.StepId,
                    LineNumber = x.LineNumber,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_ProjectAchievementsVM> ListR = new List<Pro_ProjectAchievementsVM>();
                return ListR;
            }
        }
        public async Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectid(int projectid, int stepid)
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectAchievements.Where(s => s.IsDeleted == false && s.ProjectId== projectid && s.StepId== stepid).Select(x => new Pro_ProjectAchievementsVM
                {
                    ProjectAchievementId = x.ProjectAchievementId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    ProjectId = x.ProjectId,
                    StepId = x.StepId,
                    LineNumber = x.LineNumber,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_ProjectAchievementsVM> ListR = new List<Pro_ProjectAchievementsVM>();
                return ListR;
            }
        }
        public async Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectidOnly(int projectid)
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectAchievements.Where(s => s.IsDeleted == false && s.ProjectId == projectid).Select(x => new Pro_ProjectAchievementsVM
                {
                    ProjectAchievementId = x.ProjectAchievementId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    ProjectId = x.ProjectId,
                    StepId = x.StepId,
                    LineNumber = x.LineNumber,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_ProjectAchievementsVM> ListR = new List<Pro_ProjectAchievementsVM>();
                return ListR;
            }
        }

    }
}
