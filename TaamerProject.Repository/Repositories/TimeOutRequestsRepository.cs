using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class TimeOutRequestsRepository : ITimeOutRequestsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TimeOutRequestsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<TimeOutRequestsVM>> GetTimeOutRequests(int BranchId)
        {
            var TimeOutRequests = _TaamerProContext.TimeOutRequests.Where(s => s.IsDeleted == false && s.BranchId == BranchId ).Select(x => new TimeOutRequestsVM
            {
                RequestId = x.RequestId,
                Address = x.Address,
                Duration=x.Duration,
                Reason = x.Reason,
                AttachmentUrl = x.AttachmentUrl,
                UserId = x.UserId,
                ActionUserId = x.ActionUserId,
                Status = x.Status,
                TaskId = x.TaskId,
                Comment = x.Comment,
                UserFullName = x.Users.FullName,
                UserImgUrl = x.Users.ImgUrl ?? "/distnew/images/userprofile.png",
            }).ToList();
            return TimeOutRequests;
        }
    }
}
