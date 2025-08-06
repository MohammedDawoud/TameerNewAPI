using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAppraisalService
    {
        Task<IEnumerable<AppraisalVM>> GetAllAppraisal();
        GeneralMessage SaveAppraisal(Appraisal appraisal, int UserId, int BranchId);
        GeneralMessage DeleteAppraisal(int AppraisalId, int UserId, int BranchId);
        Task<IEnumerable<AppraisalVM>> SearchAppraisal(AppraisalVM AppraisalySearch, string lang, int BranchId);
    }
}
