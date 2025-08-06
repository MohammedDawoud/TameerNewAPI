using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class ChattingLogRepository :  IChattingLogRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ChattingLogRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ChattingLogVM>> GetAllChattingLog()
        {
            var ChattingLog = _TaamerProContext.ChattingLog.Where(s => s.IsDeleted == false).Select(x => new ChattingLogVM
            {
                LogId = x.LogId,
                Body = x.Body,
                Date = x.Date,
                HijriDate = x.HijriDate,
                UserId = x.UserId,
                Status = x.Status,
                ReceivedUserId = x.ReceivedUserId,
                SenderUserName = x.SenderUser.FullName,
                ReceiveUserName = x.ReceiveUsers.FullName,

            });
            return ChattingLog;
        }
     
    }
}
