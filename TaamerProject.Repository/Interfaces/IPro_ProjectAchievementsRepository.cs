using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_ProjectAchievementsRepository
    {
        Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievements();
        Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectid(int projectid, int stepid);
        Task<IEnumerable<Pro_ProjectAchievementsVM>> GetAllProjectAchievementsbyprojectidOnly(int projectid);


    }
}
