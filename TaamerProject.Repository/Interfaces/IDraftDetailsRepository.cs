using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDraftDetailsRepository
    {
        Task<IEnumerable<DraftDetailsVM>> GetAllDraftDetailsByDraftId(int? DraftId);
        Task<IEnumerable<DraftDetailsVM>> GetAllDraftsDetailsbyProjectId(int? ProjectId);
    }
}
