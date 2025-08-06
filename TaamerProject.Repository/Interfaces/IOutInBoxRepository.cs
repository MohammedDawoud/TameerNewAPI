using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IOutInBoxRepository
    {
        Task<IEnumerable<OutInBoxVM>> GetAllOutInbox(int Type, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetAllOutInboxList(int Type, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetAllOutInboxSearch( int BranchId);
        Task<OutInBoxVM> GetOutInboxById(int OutInBoxId);
        Task<OutInBoxVM> GetOutInboxByRelatedToId(int OutInBoxId);
        Task<IEnumerable<OutInBoxVM>> GetOutInboxFiles(int? Type, int? OutInType, int? ArchiveFileId, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetOutInboxProjectFiles(int Type, int? ProjectId, int BranchId);
        Task<IEnumerable<OutInBoxVM>> SearchOutbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<OutInBoxVM>> GetAllOutboxByDateSearch(string DateFrom, string DateTo, int BranchId, int Type);
        Task<IEnumerable<OutInBoxVM>> GetAllOutInboxByDateSearch(string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<OutInBoxVM>> SearchOutInbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId);
        Task<int> GetMaxId();
        Task<int> GetOutboxCount(int BranchId);
        Task<int> GetInboxCount(int BranchId);
       
    }
}
