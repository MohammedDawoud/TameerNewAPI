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
    public class ProjectTypeRepository : IProjectTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ProjectTypeVM>> GetAllProjectType(string SearchText)
        {
            if (SearchText == "")
            {
                var projectTypes = _TaamerProContext.ProjectType.Where(s => s.IsDeleted == false).Select(x => new ProjectTypeVM
                {
                    TypeId = x.TypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Typeum=x.Typeum,
                    TypeCode=x.TypeCode??0,
                }).ToList().OrderBy(t=>t.TypeCode);
                return projectTypes;
            }
            else

            {
                var projectTypes = _TaamerProContext.ProjectType.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new ProjectTypeVM
                {
                    TypeId = x.TypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Typeum = x.Typeum,
                    TypeCode = x.TypeCode ?? 0,
                }).ToList().OrderBy(t => t.TypeCode);
                return projectTypes;
            }
        }
    }
}