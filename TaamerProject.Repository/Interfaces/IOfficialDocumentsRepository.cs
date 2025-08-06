using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IOfficialDocumentsRepository
    {
        Task<IEnumerable<OfficialDocumentsVM>> GetAllOfficialDocuments(string lang);
        Task<int> GetOfficialDocumentsCount();
        Task<int> GetMaxOfficialDocumentsNumber(int BranchId);
        Task<IEnumerable<OfficialDocumentsVM>> SearchOfficialDocuments(OfficialDocumentsVM OfficialDocumentsSearch, int BranchId);
    }
}
