using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class SupportRequestsReplayRepository : ISupportRequestsReplayRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SupportRequestsReplayRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<IEnumerable<SupportRequestsReplayVM>> GetAllReplyByServiceId(int RequestId)
        {
            var drafts = _TaamerProContext.RequestsReplays.Where(s => s.IsDeleted == false && s.ServiceRequestId== RequestId).Select(x => new SupportRequestsReplayVM
            {
                ServiceRequestId = x.ServiceRequestId,
                ContactDate = x.ContactDate,
                Contacttxt = x.Contacttxt,
                CusomerId = x.CusomerId,
                SenderPhoto="",//x.SenderPhoto,
                SupportRequestsReplayId = x.SupportRequestsReplayId,
                UserId=x.UserId,
                SenderName=x.SenderName,
               ReplayFrom=x.ReplayFrom,
               AttachmentUrl=x.AttachmentUrl,

            }).ToList();
            return drafts;
        }

        public async Task<IEnumerable<SupportRequestsReplayVM>> GetAllUnReadedReplyByServiceId(int UserID)
        {
            try
            {
                var drafts = _TaamerProContext.RequestsReplays.Where(s => s.IsDeleted == false && s.SupportResquests.AddUser == UserID && (s.SupportResquests.Status == (int)SupportRequestStatus.Opend || s.SupportResquests.Status == (int)SupportRequestStatus.Delay) && s.IsRead != true && s.ReplayFrom=="2").Select(x => new SupportRequestsReplayVM
                {
                    ServiceRequestId = x.ServiceRequestId,
                    ContactDate = x.ContactDate,
                    Contacttxt = x.Contacttxt,
                    CusomerId = x.CusomerId,
                    SenderPhoto = x.SenderPhoto,
                    SupportRequestsReplayId = x.SupportRequestsReplayId,
                    UserId = x.UserId,
                    SenderName = x.SenderName,


                }).ToList();
                return drafts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
