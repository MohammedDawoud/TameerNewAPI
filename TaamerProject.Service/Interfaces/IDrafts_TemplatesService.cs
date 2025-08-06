using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IDrafts_TemplatesService  
    {
        Task<IEnumerable<Drafts_TemplatesVM>> GetAllDrafts_templates();
        Task<Drafts_TemplatesVM> GetDraft_templateByProjectId(int projecttypeid);
        GeneralMessage SaveDraft_Templates(Drafts_Templates drafts, int UserId, int BranchId);

        GeneralMessage ConnectDraft_Templates_WithProject(int DraftId, int ProjectTypeId, int UserId, int BranchId);


    }
}
