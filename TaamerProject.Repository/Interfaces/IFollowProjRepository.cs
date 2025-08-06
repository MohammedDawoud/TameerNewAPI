using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IFollowProjRepository : IRepository<FollowProj>
    {
        Task<IEnumerable<FollowProjVM>> GetAllFollowProj();
        Task<IEnumerable<FollowProjVM>> GetAllFollowProjById(int FollowProjId);
        Task<IEnumerable<FollowProjVM>> GetAllFollowProjByProId(int ProjectId);


    }
}
