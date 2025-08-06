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
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public AttachmentRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<AttachmentVM>> GetAllAttachments(int? EmpId, string SearchText)
        {
            var Attachments = _TaamerProContext.Attachment.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId).Select(x => new AttachmentVM
            {
                AttachmentId = x.AttachmentId,
                AttachmentName = x.AttachmentName,
                Date = x.Date,
                HijriDate = x.HijriDate,
                AttachmentUrl = x.AttachmentUrl,
                Notes = x.Notes,
                EmployeeId = x.EmployeeId
            });
            if (SearchText != "")
            {
                Attachments = Attachments.Where(s => s.AttachmentName.Contains(SearchText.Trim()) || s.Notes.Contains(SearchText.Trim()));
            }
            return Attachments;
        }
    }
}


