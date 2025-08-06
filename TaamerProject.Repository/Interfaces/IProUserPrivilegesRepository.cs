using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProUserPrivilegesRepository
    {
        Task<IEnumerable<ProUserPrivilegesVM>> GetAllPriv(string SearchText, string Projectno, int BranchId);
        Task<IEnumerable<ProUserPrivilegesVM>> GetAllPrivUser(int UserId, int projectId);

    }
}
