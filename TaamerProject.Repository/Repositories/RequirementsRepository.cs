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
    public class RequirementsRepository : IRequirementsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public RequirementsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<RequirementsVM>> GetAllRequirements(int BranchId)
        {
            var requirements = _TaamerProContext.Requirements.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new RequirementsVM
            {
                RequirementId = x.RequirementId,
                NameAr = x.NameAr ?? "",
                NameEn = x.NameEn ?? "",
                BranchId = x.BranchId,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                ConfirmStatus = x.ConfirmStatus??false,
                ConfirmStatusDate = x.ConfirmStatusDate,
                Cost = x.Cost??0,
                ConfirmStatustxt = x.ConfirmStatus== true?"تمت":"لم تتم",
                AddUserName = x.UpdateUsers != null ? x.UpdateUsers.FullNameAr != null ? x.UpdateUsers.FullNameAr : x.UpdateUsers.FullName : "",

            }).ToList();
            return requirements;
        }
        public async Task<IEnumerable<RequirementsVM>> GetAllRequirementsByProjectId(int ProjectId, int BranchId)
        {
            var requirements = _TaamerProContext.Requirements.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new RequirementsVM
            {
                RequirementId = x.RequirementId,
                NameAr = x.NameAr ?? "",
                NameEn = x.NameEn ?? "",
                BranchId = x.BranchId,
                AttachmentUrl = x.AttachmentUrl,
                ProjectId = x.ProjectId,
                ConfirmStatus = x.ConfirmStatus ?? false,
                ConfirmStatusDate = x.ConfirmStatusDate,
                Cost = x.Cost ?? 0,
                ConfirmStatustxt = x.ConfirmStatus == true ? "تمت" : "لم تتم",
                AddUserName = x.UpdateUsers != null ? x.UpdateUsers.FullNameAr != null ? x.UpdateUsers.FullNameAr : x.UpdateUsers.FullName : "",

            }).ToList();
            return requirements;
        }
    }
}


