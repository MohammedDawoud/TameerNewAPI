using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IJournalsRepository
    {
        Task<IEnumerable<JournalsVM>> GetAllJournals(int FromJournal, int ToJournal, string FromDate, string ToDate,int BranchId);
        Task<IEnumerable<JournalsVM>> GetJournalsbyParam(int InvoiceId, int Year, int Branch, int Type);

        Task<int> GenerateNextJournalNumber(int year,int BranchId);
    }
}
