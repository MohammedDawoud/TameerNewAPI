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
    public class DraftDetailsRepository :  IDraftDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DraftDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<DraftDetailsVM>> GetAllDraftDetailsByDraftId(int? DraftId)
        {
            var draftdetails = _TaamerProContext.DraftDetails.Where(s => s.IsDeleted == false && s.DraftId == DraftId  ).Select(x => new DraftDetailsVM
            {
                DraftId = x.DraftId,
                DraftDetailId = x.DraftDetailId,
                DraftName = x.Draft.Name,
                ProjectId = x.ProjectId,
                ProjectNo= x.Project.ProjectNo
            }).ToList();
            return draftdetails;
        }

        public async Task<IEnumerable<DraftDetailsVM>> GetAllDraftsDetailsbyProjectId(int? ProjectId)
        {
            var draftdetails = _TaamerProContext.DraftDetails.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new DraftDetailsVM
            {
                DraftId = x.DraftId,
                DraftDetailId = x.DraftDetailId,
                DraftName = x.Draft.Name,
                ProjectId = x.ProjectId,
                ProjectNo = x.Project.ProjectNo
            }).ToList();
            return draftdetails;
        }

    }
}
