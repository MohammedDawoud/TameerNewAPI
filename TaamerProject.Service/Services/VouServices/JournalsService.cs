using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class JournalsService : IJournalsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IJournalsRepository _JournalsRepository;



        public JournalsService(TaamerProjectContext dataContext, ISystemAction systemAction, IJournalsRepository journalsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _JournalsRepository = journalsRepository;
        }
        public IEnumerable<JournalsVM> GetAllJournals()
        {
            //var Journals = _JournalsRepository.GetAllJournals();
            return null;
        }

        public async Task<IEnumerable<JournalsVM>> GetJournalsbyParam(int InvoiceId, int Year, int Branch, int Type)
        {
            return await _JournalsRepository.GetJournalsbyParam(InvoiceId, Year, Branch, Type);


        }
        public GeneralMessage SaveJournals(Journals journals, int UserId, int BranchId)
        {
            try
            {

                if (journals.JournalId == 0)
                {
                    journals.AddUser = UserId;
                    journals.AddDate = DateTime.Now;
                    _TaamerProContext.Journals.Add(journals);

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة قيد جديد";
                   _SystemAction.SaveAction("SaveJournals", "JournalsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var JournalsUpdated = _TaamerProContext.Journals.Where(x=>x.JournalId==journals.JournalId).FirstOrDefault();
                    if (JournalsUpdated != null)
                    {
                        JournalsUpdated.JournalNo = journals.JournalNo;
                        JournalsUpdated.VoucherId = journals.VoucherId;
                        JournalsUpdated.VoucherType = journals.VoucherType;
                        JournalsUpdated.BranchId = journals.BranchId;
                        JournalsUpdated.UserId = journals.UserId;

                        JournalsUpdated.UpdateUser = UserId;
                        JournalsUpdated.UpdateDate = DateTime.Now;


                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل قيد رقم " + journals.JournalId;
                    _SystemAction.SaveAction("SaveJournals", "JournalsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ القيد";
                _SystemAction.SaveAction("SaveClause", "JournalsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteJournals(int JournalId, int UserId, int BranchId)
        {
            try
            {
                Journals journals = _TaamerProContext.Journals.Where(x => x.JournalId == JournalId).FirstOrDefault();
                journals.IsDeleted = true;
                journals.DeleteDate = DateTime.Now;
                journals.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف قيد رقم " + JournalId;
                _SystemAction.SaveAction("DeleteJournals", "JournalsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف قيد رقم " + JournalId; ;
                _SystemAction.SaveAction("DeleteJournals", "JournalsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
