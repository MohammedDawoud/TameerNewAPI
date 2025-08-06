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
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TemplatesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<TemplatesVM>> GetAllTemplates(int BranchId)
        {
            var Templates = _TaamerProContext.Templates.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new TemplatesVM
            {
                TemplateId = x.TemplateId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Notes = x.Notes,
                BranchId = x.BranchId,
               
            });
               
            return Templates;
        }
       
    }
}
