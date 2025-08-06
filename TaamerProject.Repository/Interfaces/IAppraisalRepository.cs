using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IAppraisalRepository 
    {
        Task<IEnumerable<AppraisalVM>> GetAllAppraisal();
        Task<IEnumerable<AppraisalVM>> SearchAppraisal(AppraisalVM AppraisalySearch, string lang, int BranchId);
    }
}
