using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISys_SystemActionsRepository
    {
        Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActionsAll(string DateFrom, string DateTo);
        Task<IEnumerable<Sys_SystemActionsVM>> GetAllSystemActions(string SearchText, string DateFrom, string DateTo, int BranchId,int UserId,int ActionType);


    }
}
