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
    public class ReasonLeaveRepository : IReasonLeaveRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ReasonLeaveRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ReasonLeaveVM>> GetAllreasons(string SearchText)
        {
            if (SearchText == "")
            {
                var reason = _TaamerProContext.ReasonLeave.Where(s => s.IsDeleted == false).Select(x => new ReasonLeaveVM
                {
              ReasonId=x.ReasonId,
              DesecionTxt=x.DesecionTxt,
              ReasonTxt=x.ReasonTxt,
                }).ToList();
                return reason;
            }
            else
            {
                var reason = _TaamerProContext.ReasonLeave.Where(s => s.IsDeleted == false && (s.ReasonTxt.Contains(SearchText) || s.DesecionTxt.Contains(SearchText))).Select(x => new ReasonLeaveVM
                {
                    ReasonId = x.ReasonId,
                    DesecionTxt = x.DesecionTxt,
                    ReasonTxt = x.ReasonTxt,
                }).ToList();
                return reason;
            }
        }


        public async Task<ReasonLeaveVM> Getreasonbyid(int ReasonId)
        {
            var reason = _TaamerProContext.ReasonLeave.Where(s => s.IsDeleted == false && s.ReasonId==ReasonId).Select(x => new ReasonLeaveVM
            {
                ReasonId = x.ReasonId,
                DesecionTxt = x.DesecionTxt,
                ReasonTxt = x.ReasonTxt,
            }).FirstOrDefault();
            return reason;
        }

    }
}
