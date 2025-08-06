using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class BuildTypesRepository : IBuildTypesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public BuildTypesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<BuildTypesVM>> GetAllBuildTypes(string SearchText)
        {
            if (SearchText == "")
            { 
            var types = _TaamerProContext.BuildTypes.Where(s => s.IsDeleted == false).Select(x => new BuildTypesVM
            {
                BuildTypeId = x.BuildTypeId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Description = x.Description,
            });
                return types;
            }
            else
            {
                var types = _TaamerProContext.BuildTypes.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText)) ).Select(x => new BuildTypesVM
                {
                    BuildTypeId = x.BuildTypeId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                    Description = x.Description,
                });
                return types;
            }
        }
    }
}


