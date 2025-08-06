using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOfficialDocumentsService
    {
        Task<IEnumerable<OfficialDocumentsVM>> GetAllOfficialDocuments(string lang);
        GeneralMessage SaveOfficialDocuments(OfficialDocuments officialDocuments, int UserId, int BranchId);
        GeneralMessage DeleteOfficialDocuments(int DocumentId, int UserId, int BranchId);
        IEnumerable<OfficialDocumentsVM> GetDocumentToNotified(int BranchId, string lang);
        Task<IEnumerable<OfficialDocumentsVM>> SearchOfficialDocuments(OfficialDocumentsVM OfficialDocumentsSearch, int BranchId);
        Task<int> GetMaxOfficialDocumentsNumber(int BranchId);
    }
}
