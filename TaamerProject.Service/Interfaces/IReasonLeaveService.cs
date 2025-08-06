using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IReasonLeaveService  
    {
        Task<IEnumerable<ReasonLeaveVM>> GetAllreasons(string SearchText);
        GeneralMessage SaveReason(ReasonLeave reson, int UserId, int BranchId);
        GeneralMessage DeleteReason(int ReasonId, int UserId, int BranchId);

        Task<IEnumerable<ReasonLeaveVM>> FillReasonSelect(string SearchText = "");

        Task<ReasonLeaveVM> Getreasonbyid(int ReasonId);


    }
}
