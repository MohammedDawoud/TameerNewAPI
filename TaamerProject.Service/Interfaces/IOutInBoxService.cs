using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOutInBoxService
    {
        Task<IEnumerable<OutInBoxVM>> GetAllOutInbox(int Type, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetAllOutInboxSearch(int BranchId);
        Task<OutInBoxVM> GetOutInboxById(int OutInBoxId);
        GeneralMessage SaveOutInbox(OutInBox Outbox, int UserId, int BranchId);
        //GeneralMessage UpdateOutbox(OutInBox Outbox, int UserId, int BranchId);
        //GeneralMessage UpdateInbox(OutInBox Outbox, int UserId, int BranchId);
        GeneralMessage DeleteOutInBox(int OutInBoxId, int UserId, int BranchId);
        IEnumerable<object> FillOutboxTypeSelect(int? param, int BranchId);
        IEnumerable<object> FillInboxTypeSelect(int? param, int BranchId);
        Task<IEnumerable<OutInBoxVM>> SearchOutInbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetOutInboxFiles(int? Type, int? OutInType, int? ArchiveFileId, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetOutInboxProjectFiles(int Type, int? ProjectId, int BranchId);
        Task<IEnumerable<OutInBoxVM>> SearchOutbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetAllOutboxByDateSearch(string DateFrom, string DateTo, int BranchId, int Type);
        Task<IEnumerable<OutInBoxVM>> GetAllOutInboxByDateSearch(string DateFrom, string DateTo, int BranchId);
        List<string> GetAllDeptsByOutInBoxId(int OutInboxId, int Type);
        GeneralMessage SaveOutInboxattach(int OutInboxid, string attachurl, string con);
    }
}
