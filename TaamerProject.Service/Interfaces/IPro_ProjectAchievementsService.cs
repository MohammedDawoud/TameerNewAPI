using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_ProjectAchievementsService
    {
        Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievements();
        Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectid(int projectid, int stepid);
        Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectidOnly(int projectid);

        GeneralMessage SaveProjectAchievement(List<Pro_ProjectAchievements> ProjectAchievement, int UserId, int BranchId);
        GeneralMessage DeleteProjectAchievement(int ProjectAchievementid, int UserId, int BranchId);
    }
}
