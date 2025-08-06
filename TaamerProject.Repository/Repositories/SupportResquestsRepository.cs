using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.Enums;

namespace TaamerProject.Repository.Repositories
{
    public class SupportResquestsRepository : ISupportResquestsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SupportResquestsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<SupportRequestVM> >GetAllSupportResquests(string lang, int BranchId,int UserId)
        {
            var allSupportResquests = _TaamerProContext.SupportResquests.Where(s => s.IsDeleted == false && s.AddUser==UserId ).Select(x => new SupportRequestVM
            {
                RequestId = x.RequestId,
                Type = x.Type,
                Address = x.Address,
                Topic = x.Topic,
                Date = x.Date,
                //DateF=x.Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateF = "",

                UserId = x.UserId,
                OrganizationId = x.OrganizationId,
                AttachmentUrl = x.AttachmentUrl,
                BranchId = x.BranchId,
                Department=x.Department ??"",
                priority = x.priority ?? "",
                LastReplayFrom = x.LastReplayFrom,
                Status = x.Status,
                StatusName =x.Status==1?"تحت الاجراء" :x.Status==2?"مأجلة" :x.Status==3 ?"ملغاه" :x.Status==4?"تم اغلاقها" :x.Status==5?"تم اغلاقها لمضي 24 ساعة دون رد" :"تحت الاجراء",
            }).ToList().Select(s => new SupportRequestVM()
            {
                RequestId = s.RequestId,
                Type = s.Type,
                Address = s.Address,
                Topic = s.Topic,
                Date = s.Date,
                //DateF = s.Date.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                DateF ="",

                UserId = s.UserId,
                OrganizationId = s.OrganizationId,
                AttachmentUrl = s.AttachmentUrl,
                BranchId = s.BranchId,
                Department=s.Department??"",
                priority = s.priority ?? "",
                Repaly=s.Repaly ??"",
                LastReplayDate = s.LastReplayDate??"",
                TicketNo = s.TicketNo ?? "",
                LastReplayFrom = s.LastReplayFrom,
                Status = s.Status,
                
                StatusName = s.Status == 1 ? "تحت الاجراء" : s.Status == 2 ? "مأجلة" : s.Status == 3 ? "ملغاه" : s.Status == 4 ? "تم اغلاقها" : s.Status == 5 ? "تم اغلاقها لمضي 24 ساعة دون رد" : "تحت الاجراء",


            });
            return allSupportResquests;
        }


        public async Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquests(string lang, int BranchId, int UserId)
        {
            var allSupportResquests = _TaamerProContext.SupportResquests.Where(s => s.IsDeleted == false && s.AddUser == UserId && (s.Status==(int)SupportRequestStatus.Opend || s.Status == (int)SupportRequestStatus.Delay)).Select(x => new SupportRequestVM
            {
                RequestId = x.RequestId,
                Type = x.Type,
                Address = x.Address,
                Topic = x.Topic,
                Date = x.Date,
                DateF = "",

                UserId = x.UserId,
                OrganizationId = x.OrganizationId,
                AttachmentUrl = x.AttachmentUrl,
                BranchId = x.BranchId,
                Department = x.Department ?? "",
                priority = x.priority ?? "",
                Status=x.Status,
                CustomerULR = x.CustomerULR,
                Repaly = x.Repaly ?? "",
                LastReplayDate = x.LastReplayDate ?? "",
                TicketNo = x.TicketNo ?? "",
                LastReplayFrom=x.LastReplayFrom??"",
                
                StatusName = x.Status == 1 ? "تحت الاجراء" : x.Status == 2 ? "مأجلة" : x.Status == 3 ? "ملغاه" : x.Status == 4 ? "تم اغلاقها" : x.Status == 5 ? "تم اغلاقها لمضي 24 ساعة دون رد" : "تحت الاجراء",

            }).ToList();
            return allSupportResquests;
        }

            public async Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquestsWithReplay(int UserId)
        {
            var allSupportResquests = _TaamerProContext.SupportResquests.Where(s => s.IsDeleted == false && s.AddUser == UserId && (s.Status==(int)SupportRequestStatus.Opend || s.Status == (int)SupportRequestStatus.Delay) && ! string.IsNullOrEmpty(s.Repaly)).Select(x => new SupportRequestVM
            {
                RequestId = x.RequestId,
                Type = x.Type,
                Address = x.Address,
                Topic = x.Topic,
                Date = x.Date,
                DateF = "",

                UserId = x.UserId,
                OrganizationId = x.OrganizationId,
                AttachmentUrl = x.AttachmentUrl,
                BranchId = x.BranchId,
                Department = x.Department ?? "",
                priority = x.priority ?? "",
                Status=x.Status,
                CustomerULR = x.CustomerULR,
                Repaly = x.Repaly ?? "",
                LastReplayDate = x.LastReplayDate ?? "",
                TicketNo = x.TicketNo ?? "",
                LastReplayFrom=x.LastReplayFrom ??"",
                StatusName = x.Status == 1 ? "تحت الاجراء" : x.Status == 2 ? "مأجلة" : x.Status == 3 ? "ملغاه" : x.Status == 4 ? "تم اغلاقها" : x.Status == 5 ? "تم اغلاقها لمضي 24 ساعة دون رد" : "تحت الاجراء",

            }).ToList();
            return allSupportResquests;
        }

    }
}
