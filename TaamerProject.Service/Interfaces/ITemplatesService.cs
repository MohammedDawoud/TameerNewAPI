using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ITemplatesService  
    {
        Task<IEnumerable<TemplatesVM>> GetAllTemplates(int BranchId);
        GeneralMessage SaveTemplates(Templates templates, int UserId, int BranchId);
        GeneralMessage DeleteTemplates(int TemplateId, int UserId, int BranchId);
       
    }
}
