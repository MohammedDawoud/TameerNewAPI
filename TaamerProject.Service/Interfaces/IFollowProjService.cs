using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IFollowProjService
    {
        Task<IEnumerable<FollowProjVM>> GetAllFollowProj();
        Task<IEnumerable<FollowProjVM>> GetAllFollowProjById(int FollowProjId);
        Task<IEnumerable<FollowProjVM>> GetAllFollowProjByProId(int ProjectId);
        GeneralMessage SaveFollowProj(List<FollowProj> followProj, int UserId, int BranchId);
        GeneralMessage DeleteFollowProj(int FollowProjId, int UserId, int BranchId);
        GeneralMessage ConfirmRate(int FollowProjId, bool Status, int UserId, int BranchId);
    }
}
