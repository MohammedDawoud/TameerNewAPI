using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Service.Interfaces
{
    public interface ISupportRequestsService  
    {
        GeneralMessage SaveSupportResquests(SupportResquests supportResquests, int UserId, int BranchId,string VersionCode, string AttachmentFile);
        Task<IEnumerable<SupportRequestVM>> GetAllSupportResquests(string lang, int BranchId,int UserId);
        GeneralMessage UpdateSupportResquests(int Serviceid, string replay, int status, string SenderName, string SenderPhoto, int UserId, int BranchId,
            string AttachmentUrl = null);
        Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquests(string lang, int BranchId, int UserId);
        GeneralMessage UpdateSupportResquestsNo(int Serviceid, string TicketNo, int UserId, int BranchId);
        Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquestsWithReplay(int UserId);
        GeneralMessage SaveRequestReplay(SupportRequestsReplay supportResquests, int UserId, int BranchId);
        GeneralMessage SaveRequestReplayFromTameer(int Serviceid, string replay, int UserId, int BranchId, string? AttachmentUrl = null);
        Task<IEnumerable<SupportRequestsReplayVM>> GetAllReplyByServiceId(int RequestId);
        Task<IEnumerable<SupportRequestsReplayVM>> GetAllOpenSupportResquestsreplayesDashboard(int UserId);
        GeneralMessage ReadReplay(int SupportReplayId, int UserId, int BranchId);
        bool SendMail(SupportResquests supportResquests, int BranchId, int UserId, string VersionCode, string OrgName, string AttachmentFile, string RequesterMail);
        bool AutomationMail(SupportResquests supportResquests, int BranchId);
        string DecryptValue(string value);
        GeneralMessage Deleterequest(int Serviceid, int UserId, int BranchId);
    }
}
