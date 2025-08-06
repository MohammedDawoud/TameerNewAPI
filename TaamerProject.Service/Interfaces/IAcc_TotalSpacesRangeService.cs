using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_TotalSpacesRangeService
    {
        Task<IEnumerable<Acc_TotalSpacesRangeVM>> GetAllTotalSpacesRange(string SearchText);
        GeneralMessage SaveTotalSpacesRange(Acc_TotalSpacesRange TotalSpacesRange, int UserId, int BranchId);
        GeneralMessage DeleteTotalSpacesRange(int TotalSpacesRangeId, int UserId, int BranchId);
    }
}
