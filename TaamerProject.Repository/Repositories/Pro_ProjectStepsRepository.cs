using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using System.Security;

namespace TaamerProject.Repository.Repositories
{
    public class Pro_ProjectStepsRepository: IPro_ProjectStepsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_ProjectStepsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_ProjectStepsVM>> GetAllProjectSteps()
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectSteps.Where(s => s.IsDeleted == false).Select(x => new Pro_ProjectStepsVM
                {
                    ProjectStepId = x.ProjectStepId,
                    ProjectId = x.ProjectId,
                    StepId = x.StepId,
                    StepDetailId = x.StepDetailId,
                    Status = x.Status??false,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    Date = x.Date ?? "",
                    Notes = x.Notes ?? "",
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_ProjectStepsVM> ListR = new List<Pro_ProjectStepsVM>();
                return ListR;
            }
        }
        public async Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectid(int projectid,int stepid)
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectSteps.Where(s => s.IsDeleted == false && s.ProjectId== projectid && s.StepId== stepid).Select(x => new Pro_ProjectStepsVM
                {
                    ProjectStepId = x.ProjectStepId,
                    ProjectId = x.ProjectId,
                    StepId = x.StepId,
                    StepName = x.StepDetails != null ? x.StepDetails.StepName ?? "" : "",
                    StepDetailId = x.StepDetailId,
                    StepDetailNameAr=x.StepDetails!=null?x.StepDetails.NameAr??"":"",
                    StepDetailNameEn = x.StepDetails != null ? x.StepDetails.NameEn ?? "" : "",
                    Status = x.Status ?? false,
                    StatusName = x.Status == true?"تم": "لم يتم",
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    Date = x.Date ?? "",
                    Notes = x.Notes ?? "",
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_ProjectStepsVM> ListR = new List<Pro_ProjectStepsVM>();
                return ListR;
            }
        }
        public async Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectidOnly(int projectid)
        {
            try
            {
                var RetList = _TaamerProContext.Pro_ProjectSteps.Where(s => s.IsDeleted == false && s.ProjectId == projectid).Select(x => new Pro_ProjectStepsVM
                {
                    ProjectStepId = x.ProjectStepId,
                    ProjectId = x.ProjectId,
                    StepId = x.StepId,
                    StepName = x.StepDetails != null ? x.StepDetails.StepName ?? "" : "",
                    StepDetailId = x.StepDetailId,
                    StepDetailNameAr = x.StepDetails != null ? x.StepDetails.NameAr ?? "" : "",
                    StepDetailNameEn = x.StepDetails != null ? x.StepDetails.NameEn ?? "" : "",
                    Status = x.Status ?? false,
                    StatusName = x.Status == true ? "تم" : "لم يتم",
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    Date = x.Date ?? "",
                    Notes = x.Notes ?? "",
                    UserName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                }).ToList();
                return RetList;
            }
            catch (Exception ex)
            {
                IEnumerable<Pro_ProjectStepsVM> ListR = new List<Pro_ProjectStepsVM>();
                return ListR;
            }
        }


    }
}
