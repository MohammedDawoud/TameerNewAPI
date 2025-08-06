using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class ChattingLogService : IChattingLogService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IChattingLogRepository _ChattingLogRepository;
        


        public ChattingLogService(TaamerProjectContext dataContext, ISystemAction systemAction, IChattingLogRepository chattingLogRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _ChattingLogRepository= chattingLogRepository;
        }
        public async Task<IEnumerable<ChattingLogVM>> GetAllChattingLog()
        {
            var ChattingLog = await _ChattingLogRepository.GetAllChattingLog();
            return ChattingLog.ToList();
        }
    }
}
