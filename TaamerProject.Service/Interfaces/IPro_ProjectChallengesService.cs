using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_ProjectChallengesService
    {
        Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallenges();
        Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectid(int projectid, int stepid);
        Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectidOnly(int projectid);

        GeneralMessage SaveProjectChallenge(List<Pro_ProjectChallenges> ProjectChallenges, int UserId, int BranchId);
        GeneralMessage DeleteProjectChallenge(int ProjectChallengeId, int UserId, int BranchId);
    }
}
