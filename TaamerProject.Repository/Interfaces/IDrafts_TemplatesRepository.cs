using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IDrafts_TemplatesRepository
    {
        Task<IEnumerable<Drafts_TemplatesVM>> GetAllDrafts_templates();
        Task<Drafts_TemplatesVM> GetDraft_templateByProjectId(int projecttypeid);


    }
}
