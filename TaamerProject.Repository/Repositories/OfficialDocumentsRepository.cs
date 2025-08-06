using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{

    public class OfficialDocumentsRepository : IOfficialDocumentsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public OfficialDocumentsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task< IEnumerable<OfficialDocumentsVM>> GetAllOfficialDocuments(string lang)
        {
            var OfficialDocuments = _TaamerProContext.OfficialDocuments.Where(s => s.IsDeleted == false).Select(x => new OfficialDocumentsVM
            {
                DocumentId = x.DocumentId,
                Number = x.Number,
                OfficialDocumentsName = lang == "ltr" ? x.NameEn : x.NameAr,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Date = x.Date,
                HijriDate = x.HijriDate,
                ExpiredDate = x.ExpiredDate,
                ExpiredHijriDate = x.ExpiredHijriDate,
                UserId = x.UserId,
                Notes = x.Notes,
                AttachmentUrl = x.AttachmentUrl,
                DepartmentId = x.DepartmentId,
                NotifyCount = x.NotifyCount,
                BranchId = x.BranchId,                
                RepeatAlarm = x.RepeatAlarm,
                RecurrenceRateId = x.RecurrenceRateId,
                DepartmentName = x.Department.DepartmentNameAr,

            }).ToList();
            return OfficialDocuments;
        }
        public async Task<int> GetOfficialDocumentsCount()
        {
            return _TaamerProContext.OfficialDocuments.Where(s => s.IsDeleted == false).Count();

        }
        public async Task<IEnumerable<OfficialDocumentsVM>> SearchOfficialDocuments(OfficialDocumentsVM OfficialDocumentsSearch, int BranchId)
        {

            var OfficialDocuments = _TaamerProContext.OfficialDocuments.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.NameAr == OfficialDocumentsSearch.NameAr || s.NameAr.Contains(OfficialDocumentsSearch.NameAr) || OfficialDocumentsSearch.NameAr == null) &&
                (s.Date == OfficialDocumentsSearch.Date || OfficialDocumentsSearch.Date == null) && (s.ExpiredDate == OfficialDocumentsSearch.ExpiredDate || OfficialDocumentsSearch.ExpiredDate == null)
                                       ).Select(x => new OfficialDocumentsVM
                                       {
                                           DocumentId = x.DocumentId,
                                           Number = x.Number,
                                           NameAr = x.NameAr,
                                           NameEn = x.NameEn,
                                           Date = x.Date,
                                           HijriDate = x.HijriDate,
                                           ExpiredDate = x.ExpiredDate,
                                           ExpiredHijriDate = x.ExpiredHijriDate,
                                           UserId = x.UserId,
                                           Notes = x.Notes,
                                           AttachmentUrl = x.AttachmentUrl,
                                           DepartmentId = x.DepartmentId,
                                           NotifyCount = x.NotifyCount,
                                           BranchId = x.BranchId,                                           
                                           RepeatAlarm = x.RepeatAlarm,
                                           RecurrenceRateId = x.RecurrenceRateId,
                                           DepartmentName = x.Department.DepartmentNameAr,
                                       });
            return OfficialDocuments;
        }
        public async Task<int> GetMaxOfficialDocumentsNumber(int BranchId)
        {
            if (_TaamerProContext.OfficialDocuments != null)
            {
                var lastRow = _TaamerProContext.OfficialDocuments.Where(s => s.BranchId == BranchId).OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {
                        return (lastRow.DocumentId) + 1;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
