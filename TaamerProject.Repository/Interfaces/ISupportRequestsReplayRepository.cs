using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISupportRequestsReplayRepository
    {
        Task<IEnumerable<SupportRequestsReplayVM>> GetAllReplyByServiceId(int RequestId);
        Task<IEnumerable<SupportRequestsReplayVM>> GetAllUnReadedReplyByServiceId(int UserID);
    }
}
