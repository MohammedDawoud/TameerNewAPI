using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_projectsReasonsService
    {
        Task<IEnumerable<Pro_projectsReasonsVM>> GetAllprojectsReasons();
        GeneralMessage SaveReason(Pro_projectsReasons Reasons, int UserId, int BranchId);
        GeneralMessage DeleteReason(int ReasonsId, int UserId, int BranchId);
    }
}
