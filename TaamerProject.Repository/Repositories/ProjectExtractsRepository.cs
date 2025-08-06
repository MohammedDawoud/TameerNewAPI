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
    public class ProjectExtractsRepository : IProjectExtractsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectExtractsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<ProjectExtractsVM>> GetAllProjectExtracts(int? ProjectId)
        {
            var ProjectExtracts = _TaamerProContext.ProjectExtracts.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new ProjectExtractsVM
            {
                ExtractId = x.ExtractId,
                ExtractNo = x.ExtractNo,
                Type = x.Type,
                Value = x.Value,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                Status = x.Status,
                ProjectId = x.ProjectId,
                ValueText = x.ValueText,
                IsDoneBefore = x.IsDoneBefore,
                IsDoneAfter = x.IsDoneAfter,
                AttachmentUrl = x.AttachmentUrl,
                SignatureUrl = x.SignatureUrl,
                StatusName = x.Status == 1 ? "صرف" : x.Status == 2 ? "رفض" : x.Status == 3 ? "جاري العمل عليه" : "تحت الصرف"
            }).ToList();
            return ProjectExtracts;
        }
    }
}


