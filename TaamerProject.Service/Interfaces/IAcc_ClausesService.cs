using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_ClausesService
    {
        Task<IEnumerable<Acc_ClausesVM>> GetAllClauses(string SearchText);
        GeneralMessage SaveClause(Acc_Clauses Clause, int UserId, int BranchId);
        GeneralMessage DeleteClause(int ClauseId, int UserId, int BranchId);
    }
}
