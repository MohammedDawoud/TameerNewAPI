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
    public class Pro_ProjectChallengesRepository: IPro_ProjectChallengesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_ProjectChallengesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallenges()
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectChallenges.Where(s => s.IsDeleted == false).Select(x => new Pro_ProjectChallengesVM
                {
                    ProjectChallengeId = x.ProjectChallengeId,
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
                IEnumerable<Pro_ProjectChallengesVM> ListR = new List<Pro_ProjectChallengesVM>();
                return ListR;
            }
        }
        public async Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectid(int projectid, int stepid)
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectChallenges.Where(s => s.IsDeleted == false && s.ProjectId== projectid && s.StepId== stepid).Select(x => new Pro_ProjectChallengesVM
                {
                    ProjectChallengeId = x.ProjectChallengeId,
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
                IEnumerable<Pro_ProjectChallengesVM> ListR = new List<Pro_ProjectChallengesVM>();
                return ListR;
            }
        }
        public async Task<IEnumerable<Pro_ProjectChallengesVM>> GetAllProjectChallengesbyprojectidOnly(int projectid)
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectChallenges.Where(s => s.IsDeleted == false && s.ProjectId == projectid).Select(x => new Pro_ProjectChallengesVM
                {
                    ProjectChallengeId = x.ProjectChallengeId,
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
                IEnumerable<Pro_ProjectChallengesVM> ListR = new List<Pro_ProjectChallengesVM>();
                return ListR;
            }
        }

    }
}
