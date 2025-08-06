using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_ProjectChallengesRepository
    {
        Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallenges();
        Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectid(int projectid, int stepid);
        Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectidOnly(int projectid);


    }
}
