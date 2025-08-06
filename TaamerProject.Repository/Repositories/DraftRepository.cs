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
    public class DraftRepository : IDraftRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DraftRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<DraftVM>> GetAllDrafts()
        {
            var drafts = _TaamerProContext.Draft.Where(s => s.IsDeleted == false).Select(x => new DraftVM
            {
                DraftId = x.DraftId,
                DraftName = x.Name,
                DraftUrl = x.DraftUrl,
                ProjectTypeId = x.ProjectTypeId,
                ProjectTypeName = x.ProjectType.NameAr
            }).ToList();
            return drafts;
        }

        public async Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType(int? TypeId)
        {
            var drafts = _TaamerProContext.Draft.Where(s => s.IsDeleted == false && s.ProjectTypeId == TypeId).Select(x => new DraftVM
            {
                DraftId = x.DraftId,
                DraftName = x.Name,
                DraftUrl = x.DraftUrl,
                ProjectTypeId = x.ProjectTypeId,
                ProjectTypeName = x.ProjectType.NameAr
            }).ToList();
            return drafts;
        }


        public async Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType_2(int? TypeId)
        {
            var drafts = _TaamerProContext.Draft.Where(s => s.IsDeleted == false && s.ProjectTypeId == TypeId).Select(x => new DraftVM
            {
                DraftId = x.DraftId,
                DraftName = x.Name +" مرتبطة بمشروع"+ " : "+ x.ProjectType.NameAr,
                DraftUrl = x.DraftUrl,
                ProjectTypeId = x.ProjectTypeId,
                ProjectTypeName = x.ProjectType.NameAr
            }).ToList();
            return drafts;
        }


        public async Task<DraftVM> GetDraftById(int DraftId)
        {
            var drafts = _TaamerProContext.Draft.Where(s => s.IsDeleted == false && s.DraftId== DraftId).Select(x => new DraftVM
            {
                DraftId = x.DraftId,
                DraftName = x.Name,
                DraftUrl = x.DraftUrl,
                ProjectTypeId = x.ProjectTypeId,
                ProjectTypeName = x.ProjectType.NameAr
            }).FirstOrDefault();
            return drafts;
        }

    }
}
