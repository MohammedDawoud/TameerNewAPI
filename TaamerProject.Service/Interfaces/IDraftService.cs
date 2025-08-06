using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDraftService  
    {
        Task<IEnumerable<DraftVM>> GetAllDrafts();
        Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType(int? projectTypeId);
        Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType_2(int? projectTypeId);
        GeneralMessage SaveDraft(Draft draft, int UserId, int BranchId);
        GeneralMessage DeleteDraft(int DraftId, int UserId, int BranchId);
        IEnumerable<DraftVM> GetAllDraftsByDraftName_Union(string DraftName);

        Task<DraftVM> GetDraftById(int DraftId);
    }
}
