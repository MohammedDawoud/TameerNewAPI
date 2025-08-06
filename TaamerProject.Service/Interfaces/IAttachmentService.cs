using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IAttachmentService
    {
        Task<IEnumerable<AttachmentVM>> GetAllAttachments(int? EmpId, string SearchText);
        GeneralMessage SaveAttachment(Attachment attachment, int UserId, int BranchId);
        GeneralMessage DeleteAttachment(int AttachmentId, int UserId, int BranchId);
    }
}
