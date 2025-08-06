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
    public class ProjectTrailingRepository : IProjectTrailingRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectTrailingRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInOfficeArea(int BranchId)
        {
            var ProjectTrailing = _TaamerProContext.ProjectTrailing.Where(s => s.IsDeleted == false && s.Active == true && s.Status == 1 && s.BranchId == BranchId).Select(x => new ProjectTrailingVM
            {
                TrailingId = x.TrailingId,
                ProjectId = x.ProjectId,
                DepartmentId = x.DepartmentId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Active = x.Active,
                Status = x.UserId,
                TypeId = x.TypeId,
                UserId = x.UserId,
                ReceiveDate = x.ReceiveDate,
                ReceiveHijriDate = x.ReceiveHijriDate,
                ReceiveUserId = x.ReceiveUserId,
                Notes = x.Notes,
                TaskId = x.TaskId,
                BranchId = x.BranchId,
                ProjectNumber = x.Project.ProjectNo,
                SideName = x.Department.DepartmentNameAr,
                CustomerName = x.Project.customer.CustomerNameAr,
                ProjectTypeName = x.Project.projecttype.NameAr,
                SketchNumber = x.Project.SketchNo,
                SiteName = x.Project.SiteName,
                MobileNo = x.Project.customer.CustomerMobile,
            }).ToList();
            return ProjectTrailing;
        }
        public async Task<IEnumerable<ProjectTrailingVM>> GetProjectTrailingInExternalSide(int BranchId)
        {//s.Status == 2 &&
            var ProjectTrailing = _TaamerProContext.ProjectTrailing.Where(s => s.IsDeleted == false && s.Active == true &&  s.BranchId == BranchId).Select(x => new ProjectTrailingVM
            {
                TrailingId = x.TrailingId,
                ProjectId = x.ProjectId,
                DepartmentId = x.DepartmentId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Active = x.Active,
                Status = x.UserId,
                TypeId = x.TypeId,
                UserId = x.UserId,
                ReceiveDate = x.ReceiveDate,
                ReceiveHijriDate = x.ReceiveHijriDate,
                ReceiveUserId = x.ReceiveUserId,
                Notes = x.Notes,
                TaskId = x.TaskId,
                BranchId = x.BranchId,
                ProjectNumber = x.Project.ProjectNo,
                SideName = x.Department.DepartmentNameAr,
                CustomerName = x.Project.customer.CustomerNameAr,
                ProjectTypeName = x.Project.projecttype.NameAr,
                SketchNumber = x.Project.SketchNo,
                SiteName = x.Project.SiteName,
                MobileNo = x.Project.customer.CustomerMobile,
            }).ToList();
            return ProjectTrailing;
        }
        public async Task<int> GetMaxId()
        {
            return (_TaamerProContext.ProjectTrailing.Count() > 0) ? _TaamerProContext.ProjectTrailing.Max(s => s.TrailingId) : 0;
        }
    }
}


