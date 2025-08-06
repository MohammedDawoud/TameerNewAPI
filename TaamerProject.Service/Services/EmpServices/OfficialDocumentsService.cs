using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class OfficialDocumentsService : IOfficialDocumentsService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOfficialDocumentsRepository _OfficialDocumentsRepository;



        public OfficialDocumentsService(TaamerProjectContext dataContext, ISystemAction systemAction, IOfficialDocumentsRepository officialDocumentsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OfficialDocumentsRepository = officialDocumentsRepository;
        }

        public async Task<IEnumerable<OfficialDocumentsVM>> GetAllOfficialDocuments(string lang)
        {
            var OfficialDocuments =await _OfficialDocumentsRepository.GetAllOfficialDocuments(lang);
            return OfficialDocuments;
        }
        public GeneralMessage SaveOfficialDocuments(OfficialDocuments officialDocuments, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _TaamerProContext.OfficialDocuments.Where(s => s.IsDeleted == false && s.DocumentId != officialDocuments.DocumentId && s.Number == officialDocuments.Number).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = Resources.General_SavedFailed;
                   _SystemAction.SaveAction("SaveOfficialDocuments", "OfficialDocumentsService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                if (officialDocuments.DocumentId == 0)
                {
                    officialDocuments.Notes = "تحذير : لديك وثيقة " + officialDocuments.NameAr + "  برقم " + officialDocuments.Number + " تنتهي في " + officialDocuments.ExpiredDate + " سارع بتجديدها حتي لا يقع عليها غرامة";
                    officialDocuments.AddUser = UserId;
                    officialDocuments.BranchId = BranchId;
                    officialDocuments.IsDeleted = false;
                    officialDocuments.AddDate = DateTime.Now;
                    _TaamerProContext.OfficialDocuments.Add(officialDocuments);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة وثيقة رسمية جديدة";
                    _SystemAction.SaveAction("SaveOfficialDocuments", "OfficialDocumentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var OfficialDocumentsUpdated = _TaamerProContext.OfficialDocuments.Where(x=>x.DocumentId==officialDocuments.DocumentId).FirstOrDefault();
                    if (OfficialDocumentsUpdated != null)
                    {
                        OfficialDocumentsUpdated.Number = officialDocuments.Number;
                        OfficialDocumentsUpdated.NameAr = officialDocuments.NameAr;
                        OfficialDocumentsUpdated.NameEn = officialDocuments.NameEn;
                        OfficialDocumentsUpdated.Date = officialDocuments.Date;
                        OfficialDocumentsUpdated.HijriDate = officialDocuments.HijriDate;
                        OfficialDocumentsUpdated.ExpiredDate = officialDocuments.ExpiredDate;
                        OfficialDocumentsUpdated.ExpiredHijriDate = officialDocuments.ExpiredHijriDate;
                        OfficialDocumentsUpdated.UserId = officialDocuments.UserId;
                        OfficialDocumentsUpdated.Notes = "تحذير : لديك وثيقة " + officialDocuments.NameAr + "  برقم " + officialDocuments.Number + " تنتهي في " + officialDocuments.ExpiredDate + " سارع بتجديدها حتي لا يقع عليها غرامة"; ;
                        if (officialDocuments.AttachmentUrl != null)
                        {
                            OfficialDocumentsUpdated.AttachmentUrl = officialDocuments.AttachmentUrl;
                        }
                        OfficialDocumentsUpdated.DepartmentId = officialDocuments.DepartmentId;
                        OfficialDocumentsUpdated.NotifyCount = officialDocuments.NotifyCount;
                        OfficialDocumentsUpdated.UpdateUser = UserId;
                        OfficialDocumentsUpdated.RepeatAlarm = officialDocuments.RepeatAlarm;
                        OfficialDocumentsUpdated.RecurrenceRateId = officialDocuments.RecurrenceRateId;
                        OfficialDocumentsUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل وثيقة رسمية رقم " + officialDocuments.DocumentId;
                    _SystemAction.SaveAction("SaveOfficialDocuments", "OfficialDocumentsService", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveOfficialDocuments", "OfficialDocumentsService", 1, Resources.FailedToSaveTheDocument, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToSaveTheDocument };
            }
        }
        public GeneralMessage DeleteOfficialDocuments(int DocumentId, int UserId, int BranchId)
        {
            try
            {
                OfficialDocuments Officia = _TaamerProContext.OfficialDocuments.Where(x=>x.DocumentId==DocumentId).FirstOrDefault();
                Officia.IsDeleted = true;
                Officia.DeleteDate = DateTime.Now;
                Officia.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف الوثيقة رقم " + DocumentId;
                _SystemAction.SaveAction("DeleteOfficialDocuments", "OfficialDocumentsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الوثيقة رقم " + DocumentId; ;
                _SystemAction.SaveAction("DeleteOfficialDocuments", "OfficialDocumentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<OfficialDocumentsVM> GetDocumentToNotified(int BranchId, string lang)
        {
            List<OfficialDocumentsVM> offDocsList = new List<OfficialDocumentsVM>();
            var OfficialDocuments = _TaamerProContext.OfficialDocuments.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (DateTime.ParseExact(s.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.Now));
            foreach (var item in OfficialDocuments)
            {
                var notifyCount = _TaamerProContext.OfficialDocuments.Where(x=>x.DocumentId==item.DocumentId).FirstOrDefault().NotifyCount;
                var diff = (DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.Now).Days;
                if (diff <= notifyCount)
                {
                    offDocsList.Add(new OfficialDocumentsVM
                    {
                        OfficialDocumentsName = item.NameAr,
                        Number = item.Number,
                        ExpiredDate = item.ExpiredDate,
                        DocumentId = item.DocumentId,
                        Date = item.Date,
                        UserId = item.UserId,
                        RepeatAlarm = item.RepeatAlarm,
                        RecurrenceRateId = item.RecurrenceRateId


                    });
                }
            }
            var repeatedOfficialDocuments = _TaamerProContext.OfficialDocuments.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.RepeatAlarm == true && (DateTime.ParseExact(s.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now));

            foreach (var item in repeatedOfficialDocuments)
            {
                var notifyCount = _TaamerProContext.OfficialDocuments.Where(x=>x.DocumentId==item.DocumentId).FirstOrDefault().NotifyCount;
                int? diffDays = null; int diffYears = 0; DateTime newComparisonDate = DateTime.Now;
                switch (item.RecurrenceRateId)
                {
                    case 1://1 month
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;

                        if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1);
                        }
                        newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths(1);
                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }
                        else
                        {
                            for (int i = 0; i <= 11; i++)
                            {
                                newComparisonDate = newComparisonDate.AddMonths(1);
                                if (newComparisonDate > DateTime.Now)
                                {
                                    diffDays = (newComparisonDate - DateTime.Now).Days;
                                }
                                if (diffDays != null)
                                {
                                    i = 12;
                                }
                            }
                        }
                        break;
                    case 2://3 months
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;

                        if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1);
                        }
                        newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths(3);
                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }
                        else
                        {
                            for (int i = 0; i <= 3; i++)
                            {
                                newComparisonDate = newComparisonDate.AddMonths(3);
                                if (newComparisonDate > DateTime.Now)
                                {
                                    diffDays = (newComparisonDate - DateTime.Now).Days;
                                }
                                if (diffDays != null)
                                {
                                    i = 4;
                                }
                            }
                        }

                        break;
                    case 3://6 months
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;
                        if (diffYears / 365 == 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddMonths((6));
                        }
                        else if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365) - 1).AddMonths(6);
                        }
                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }
                        else
                        {
                            newComparisonDate = newComparisonDate.AddMonths(6);
                            if (newComparisonDate > DateTime.Now)
                            {
                                diffDays = (newComparisonDate - DateTime.Now).Days;
                            }
                        }

                        break;
                    case 4://year
                        diffYears = (DateTime.Now - DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).Days;
                        if (diffYears / 365 == 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((1));
                        }
                        else if (diffYears / 365 > 0)
                        {
                            newComparisonDate = DateTime.ParseExact(item.ExpiredDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).AddYears((diffYears / 365));
                        }

                        if (newComparisonDate > DateTime.Now)
                        {
                            diffDays = (newComparisonDate - DateTime.Now).Days;
                        }

                        break;
                }
                if (diffDays != null && diffDays <= notifyCount)
                {
                    offDocsList.Add(new OfficialDocumentsVM
                    {
                        OfficialDocumentsName = item.NameAr,
                        Number = item.Number,
                        ExpiredDate = item.ExpiredDate,
                        DocumentId = item.DocumentId,
                        Date = item.Date,
                        UserId = item.UserId,
                        RepeatAlarm = item.RepeatAlarm,
                        RecurrenceRateId = item.RecurrenceRateId


                    });
                }
            }
            return offDocsList;
        }
        public async Task<int> GetMaxOfficialDocumentsNumber(int BranchId)
        {
            var OfficialDocuments =await _OfficialDocumentsRepository.GetMaxOfficialDocumentsNumber(BranchId);
            return OfficialDocuments;
        }
        public async Task<IEnumerable<OfficialDocumentsVM>> SearchOfficialDocuments(OfficialDocumentsVM OfficialDocumentsSearch, int BranchId)
        {
            var OfficialDocuments =await _OfficialDocumentsRepository.SearchOfficialDocuments(OfficialDocumentsSearch, BranchId);
            return OfficialDocuments;
        }
    }
}
