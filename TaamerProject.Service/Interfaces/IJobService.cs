using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IJobService
    {
       Task<IEnumerable<JobVM>> GetAllJobs(string SearchText);
        GeneralMessage SaveJob(Job job, int UserId, int BranchId);
        GeneralMessage DeleteJob(int JobId, int UserId, int BranchId);
        IEnumerable<object> FillJobSelect(string SearchText = "");
    }
}
