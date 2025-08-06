using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDraftRepository 
    {
        Task<IEnumerable<DraftVM>> GetAllDrafts();
        Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType(int? TypeId);
        Task<IEnumerable<DraftVM>> GetAllDraftsbyProjectsType_2(int? TypeId);
        Task<DraftVM> GetDraftById(int DraftId);
    }
}
