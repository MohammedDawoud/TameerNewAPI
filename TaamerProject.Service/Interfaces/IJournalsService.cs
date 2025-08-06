using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IJournalsService
    {
        IEnumerable<JournalsVM> GetAllJournals();
        Task<IEnumerable<JournalsVM>> GetJournalsbyParam(int InvoiceId, int Year, int Branch, int Type);
        GeneralMessage SaveJournals(Journals journals, int UserId, int BranchId);
        GeneralMessage DeleteJournals(int JournalId, int UserId, int BranchId);
    }
}
