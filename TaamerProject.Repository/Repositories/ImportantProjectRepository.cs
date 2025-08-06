using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ImportantProjectRepository : IImportantProjectRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;


        public ImportantProjectRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ImportantProjectVM>> GetImportantProjects(int projectId,int UserId)
        {

            var important = _TaamerProContext.ImportantProject.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.UserId == UserId).Select(x => new ImportantProjectVM
            {
                    ImportantProId=x.ImportantProId,
                    UserId=x.UserId,
                    ProjectId=x.ProjectId,
                    BranchId=x.BranchId,
                    Flag=x.Flag,
                    IsImportant=x.IsImportant,


            }).ToList();
            return important;


        }


    }
}
