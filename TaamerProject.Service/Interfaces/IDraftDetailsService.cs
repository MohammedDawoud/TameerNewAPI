using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDraftDetailsService  
    {
        Task<IEnumerable<DraftDetailsVM>> GetAllDraftsDetailsbyProjectId(int? ProjectId);
        Task<IEnumerable<DraftDetailsVM>> GetAllDraftDetailsByDraftId(int? DraftId);
        IEnumerable<DraftDetailsVM> GetAllDraftDetailsByDraftId_Union(int draftId);
        GeneralMessage SaveDraftDetails(DraftDetails draftDetails, int UserId, int BranchId);
        GeneralMessage DeleteDraftDetails(int DraftDetailsId, int UserId, int BranchId);
    }
}
