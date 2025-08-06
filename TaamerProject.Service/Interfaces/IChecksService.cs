using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IChecksService
    {
        Task<IEnumerable<ChecksVM>> GetAllChecks(int Type, int BranchId);
        GeneralMessage SaveCheck(Checks check, int UserId, int BranchId);
        GeneralMessage DeleteCheck(int CheckId, int UserId, int BranchId);
        Task<IEnumerable<ChecksVM>> GetAllCheckSearch(ChecksVM checkSearch, int BranchId);
    }
}
