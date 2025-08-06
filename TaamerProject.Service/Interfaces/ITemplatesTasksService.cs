using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ITemplatesTasksService  
    {
        Task<IEnumerable<TemplatesTasksVM>> GetAllTemplatesTasks(int BranchId);
        GeneralMessage SaveTemplatesTasks(TemplatesTasks templatesTasks, int UserId, int BranchId);
        GeneralMessage DeleteTemplatestasks(int TemplateTaskId, int UserId, int BranchId);
        Task<IEnumerable<TemplatesTasksVM>> GetAllTemplatesTasksByTemplateId(int TemplateId, int BranchId);

    }
}
