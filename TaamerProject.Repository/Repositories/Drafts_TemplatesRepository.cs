using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class Drafts_TemplatesRepository :IDrafts_TemplatesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Drafts_TemplatesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }


        public async Task<IEnumerable<Drafts_TemplatesVM>> GetAllDrafts_templates()
        {
            var drafts = _TaamerProContext.Drafts_Templates.Where(s => s.IsDeleted == false).Select(x => new Drafts_TemplatesVM
            {
                DraftTempleteId = x.DraftTempleteId,
                Name = x.Name,
                DraftUrl = x.DraftUrl,
                ProjectTypeId = x.ProjectTypeId,
                //ProjectTypeName = x.ProjectType.NameAr
            }).ToList();
            return drafts;
        }


        public async Task<Drafts_TemplatesVM> GetDraft_templateByProjectId(int projecttypeid)
        {
            var drafts = _TaamerProContext.Drafts_Templates.Where(s => s.IsDeleted == false &&s.ProjectTypeId== projecttypeid).Select(x => new Drafts_TemplatesVM
            {
                DraftTempleteId = x.DraftTempleteId,
                Name = x.Name,
                DraftUrl = x.DraftUrl,
                ProjectTypeId = x.ProjectTypeId,
                ProjectTypeName = x.ProjectType.NameAr
            }).FirstOrDefault();
            return drafts;
        }

    }
}
