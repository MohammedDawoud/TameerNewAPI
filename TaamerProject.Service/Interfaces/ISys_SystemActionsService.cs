using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;


namespace TaamerProject.Service.Interfaces
{
    public interface ISys_SystemActionsService  
    {
        Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActionsAll();
        Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActions(string SearchText, string DateFrom, string DateTo, int BranchId,int UserId,int ActionType);

        GeneralMessage SaveSystemActions(Sys_SystemActions SystemActions, int UserId, int BranchId);
    }
}
