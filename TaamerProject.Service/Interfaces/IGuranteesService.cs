using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IGuranteesService  
    {
        Task<IEnumerable<GuaranteesVM>> GetAllGurantees(int BranchId);
        GeneralMessage SaveGurantee(Guarantees guarantees, int UserId, int BranchId, int? yearid);
        GeneralMessage DeleteGurantee(int GuaranteeId, int UserId,int BranchId);
    }
}
