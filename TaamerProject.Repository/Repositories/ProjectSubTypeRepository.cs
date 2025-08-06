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
    public class ProjectSubTypeRepository : IProjectSubTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectSubTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string SearchText , int BranchId)
        {
            if (SearchText == "")
            {
                var Tas = _TaamerProContext.ProjectSubTypes.Where(s => s.IsDeleted == false && s.ProjectTypeId == ProjectTypeId).Select(x => new ProjectSubTypeVM
                {
                    SubTypeId = x.SubTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    ProjectTypeId = x.ProjectTypeId,
                    TimePeriod = x.TimePeriod ?? "",
                    TimePeriodStr = x.TimePeriod != null ? x.TimePeriod + " يوم " : ""
                }).ToList();
                return Tas;
            }
            else
            {
                var Tas = _TaamerProContext.ProjectSubTypes.Where(s => s.IsDeleted == false  && s.ProjectTypeId == ProjectTypeId && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new ProjectSubTypeVM
                {
                    SubTypeId = x.SubTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    ProjectTypeId = x.ProjectTypeId,
                    TimePeriod = x.TimePeriod ?? "",
                    TimePeriodStr = x.TimePeriod != null ? x.TimePeriod + " يوم " : ""
                }).ToList();
                return Tas;
            }
        }
        public async Task<IEnumerable<ProjectSubTypeVM>> GetTimePeriordBySubTypeId(int SubTypeId)
        {
            var Tas = _TaamerProContext.ProjectSubTypes.Where(s => s.IsDeleted == false && s.SubTypeId == SubTypeId).Select(x => new ProjectSubTypeVM
            {
                SubTypeId = x.SubTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                ProjectTypeId = x.ProjectTypeId,
                TimePeriod = x.TimePeriod ?? "",
                TimePeriodStr = x.TimePeriod != null ? x.TimePeriod + " يوم " : ""
            }).ToList();
            return Tas;
        }
    

        public async Task<IEnumerable<ProjectSubTypeVM>> GetAllProjectSubType(int BranchId)
        {
            var projectsubTypes = _TaamerProContext.ProjectSubTypes.Where(s => s.IsDeleted == false).Select(x => new ProjectSubTypeVM
            {
                SubTypeId = x.SubTypeId,
                ProjectTypeId = x.ProjectTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                TimePeriod = x.TimePeriod??"",
                TimePeriodStr = x.TimePeriod != null ? x.TimePeriod + " يوم " : ""
            }).ToList();
            return projectsubTypes;
        }
    }
}
