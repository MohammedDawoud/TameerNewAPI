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
    public class ProjectWorkersRepository : IProjectWorkersRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectWorkersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProjectWorkersVM>> GetAllProjectWorkers(int? ProjectId, string SearchText)
        {
            var ProjectWorkers = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new ProjectWorkersVM
            {
                WorkerId = x.WorkerId,
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                BranchId = x.BranchId,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",
                WorkerType = x.WorkerType,
                WorkerTypeName = x.WorkerType == 1 ? "مدير مشروع" : x.WorkerType == 2 ? " مشارك " : "غير محدد",
                UserImg = x.Users == null ? "" : x.Users.ImgUrl ?? "/distnew/images/userprofile.png",

            }).ToList();
            if (SearchText != "")
            {
                ProjectWorkers = ProjectWorkers.Where(s => s.UserFullName.ToLower().Contains(SearchText.Trim().ToLower())).ToList();
            }
            return ProjectWorkers;
        }
        public async Task<IEnumerable<ProjectWorkersVM>> GetUserProjectRpt(int? UserId)
        {
            var ProjectWorkers = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && (s.UserId == UserId || UserId == null)).Select(x => new ProjectWorkersVM
            {
                ProjectNo = x.Project.ProjectNo,
                ProjectTypeName = x.Project.ProjectTypeName,
                ProjectDescription = x.Project. ProjectDescription,
                CustomerName = x.Project.customer.CustomerNameAr,
                ProjectTypesName = x.Project.projecttype.NameAr,
                ProjectSubTypeName = x.Project.projectsubtype.NameAr,
                ProjectMangerName = x.Project.Users.FullName,
                ExpectedTime = x.Project.ProjectPhasesTasks.Sum(s => s.TimeMinutes),
                Cost = x.Project.ProjectPhasesTasks.Where(s => s.IsDeleted == false && s.Type == 3).Sum(s => s.Cost) ?? 0,
                UserFullName = x.Users == null ? "" : x.Users!.FullNameAr == null ? x.Users!.FullName : x.Users!.FullNameAr ?? "",

            }).ToList();
            return ProjectWorkers;
        }
        public async Task<int> GetUserProjectWorkerCount(int? UserId, int BranchId)
        {
            var UserTaskCount = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && s.UserId == UserId && s.BranchId == BranchId && s.Project.ProjectPhasesTasks.Where(x => x.IsDeleted == false && x.ProjectId == s.ProjectId).Count() > 0);
            return UserTaskCount.Count();
        }
    }
}
