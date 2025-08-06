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
    public class InstrumentsService : IInstrumentsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IInstrumentsRepository _InstrumentsRepository;



        public InstrumentsService(TaamerProjectContext dataContext, ISystemAction systemAction, IInstrumentsRepository instrumentsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _InstrumentsRepository = instrumentsRepository;
        }
        public async Task<IEnumerable<InstrumentsVM>> GetAllInstruments(int ProjectId)
        {
            var instruments =await _InstrumentsRepository.GetAllInstruments(ProjectId);
            return instruments;
        }
        public GeneralMessage SaveInstruments(List<Instruments> instruments, int UserId, int BranchId)
        {
            if (instruments != null)
            {
                try
                {
                    var existringinstrument = _TaamerProContext.Instruments.Where(s => s.ProjectId == instruments[0].ProjectId);
                    if (existringinstrument != null && existringinstrument.Count() > 0)
                    {
                        _TaamerProContext.Instruments.RemoveRange(existringinstrument);
                        List<InstrumentsVM> Instrument = new List<InstrumentsVM>();

                    }
                    foreach (var item in instruments)
                    {
                        _TaamerProContext.Instruments.Add(new Instruments { InstrumentNo = item.InstrumentNo, ProjectId = item.ProjectId, Date = item.Date, SourceId = item.SourceId });
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صك جديد";
                   _SystemAction.SaveAction("SaveInstruments", "InstrumentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                catch (Exception)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الصك";
                    _SystemAction.SaveAction("SaveInstruments", "InstrumentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
            else
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل في الصك  ";
                _SystemAction.SaveAction("SaveInstruments", "InstrumentsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
        }

        public GeneralMessage SaveInstrument(Instruments instruments, int UserId, int BranchId)
        {
            if (instruments != null)
            {
                try
                {
                    if (instruments.InstrumentId == 0)
                    {
                        instruments.AddUser = UserId;
                        instruments.AddDate = DateTime.Now;
                        _TaamerProContext.Instruments.Add(instruments);

                    }
                    else
                    {
                        var updatedinstrumente = _TaamerProContext.Instruments.Where(x => x.InstrumentId == instruments.InstrumentId).FirstOrDefault();
                        updatedinstrumente.InstrumentNo = instruments.InstrumentNo;
                        updatedinstrumente.Date = instruments.Date;
                        updatedinstrumente.HijriDate = instruments.HijriDate;
                        updatedinstrumente.ProjectId = instruments.ProjectId;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صك جديد";
                    _SystemAction.SaveAction("SaveInstruments", "InstrumentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                catch (Exception)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الصك";
                    _SystemAction.SaveAction("SaveInstruments", "InstrumentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
            else
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل في الصك  ";
                _SystemAction.SaveAction("SaveInstruments", "InstrumentsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
        }
        public GeneralMessage DeleteInstruments(int instrumentId, int UserId, int BranchId)
        {
            try
            {
                Instruments instruments = _TaamerProContext.Instruments.Where(x=>x.InstrumentId==instrumentId).FirstOrDefault();
                instruments.IsDeleted = true;
                instruments.DeleteDate = DateTime.Now;
                instruments.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف صك رقم " + instrumentId;
                _SystemAction.SaveAction("DeleteInstruments", "InstrumentsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف صك رقم " + instrumentId; ;
                _SystemAction.SaveAction("DeleteInstruments", "InstrumentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
