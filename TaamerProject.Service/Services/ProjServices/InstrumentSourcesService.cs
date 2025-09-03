
using System.Globalization;
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
    public class InstrumentSourcesService : IInstrumentSourcesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IInstrumentSourcesRepository _InstrumentSourcesRepository;



        public InstrumentSourcesService(TaamerProjectContext dataContext, ISystemAction systemAction, IInstrumentSourcesRepository instrumentSourcesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _InstrumentSourcesRepository = instrumentSourcesRepository;
        }
        public async Task<IEnumerable<InstrumentSourcesVM>> GetAllInstrumentSources(string SearchText)
        {
            var instrumentSources =await _InstrumentSourcesRepository.GetAllInstrumentSources(SearchText);
            return instrumentSources;
        }
        public GeneralMessage SaveInstrumentSources(InstrumentSources instrumentSources, int UserId, int BranchId)
        {
            try
            {

                if (instrumentSources.SourceId == 0)
                {
                    instrumentSources.AddUser = UserId;
                    instrumentSources.AddDate = DateTime.Now;
                    _TaamerProContext.InstrumentSources.Add(instrumentSources);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع جهة قضائية للصك جديد";
                   _SystemAction.SaveAction("SaveInstrumentSources", "InstrumentSourcesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var InstrumentSourcesUpdated = _TaamerProContext.InstrumentSources.Where(x => x.SourceId == instrumentSources.SourceId).FirstOrDefault();
                    if (InstrumentSourcesUpdated != null)
                    {
                        InstrumentSourcesUpdated.NameAr = instrumentSources.NameAr;
                        InstrumentSourcesUpdated.NameEn = instrumentSources.NameEn;
                        InstrumentSourcesUpdated.UpdateUser = UserId;
                        InstrumentSourcesUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع الجهة القضائية رقم " + instrumentSources.SourceId;
                    _SystemAction.SaveAction("SaveInstrumentSources", "InstrumentSourcesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع الجهة القضائية";
                _SystemAction.SaveAction("SaveInstrumentSources", "InstrumentSourcesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteInstrumentSources(int instrumentSourcesId, int UserId, int BranchId)
        {
            try
            {
                InstrumentSources instrumentSources = _TaamerProContext.InstrumentSources.Where(x=>x.SourceId==instrumentSourcesId).FirstOrDefault();
                instrumentSources.IsDeleted = true;
                instrumentSources.DeleteDate = DateTime.Now;
                instrumentSources.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف جهة قضائية رقم " + instrumentSourcesId;
                _SystemAction.SaveAction("DeleteInstrumentSources", "InstrumentSourcesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف جهة قضائية رقم " + instrumentSourcesId; ;
                _SystemAction.SaveAction("DeleteInstrumentSources", "InstrumentSourcesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
