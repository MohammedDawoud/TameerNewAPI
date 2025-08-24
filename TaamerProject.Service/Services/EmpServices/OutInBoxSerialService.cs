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
    public class OutInBoxSerialService : IOutInBoxSerialService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOutInBoxSerialRepository _OutInBoxSerialRepository;



        public OutInBoxSerialService(TaamerProjectContext dataContext, ISystemAction systemAction, IOutInBoxSerialRepository outInBoxSerialRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OutInBoxSerialRepository = outInBoxSerialRepository;
        }


        public async Task< IEnumerable<OutInBoxSerialVM>> GetAllOutInBoxSerial(int Type, int BranchId)
        {
            var OutInBoxSerial = await _OutInBoxSerialRepository.GetAllOutInBoxSerial(Type, BranchId);
            return OutInBoxSerial;
        }
        public GeneralMessage SaveOutInBoxSerial(OutInBoxSerial OutInBoxSerial, int UserId, int BranchId)
        {
            try
            {
                if (OutInBoxSerial.OutInSerialId == 0)
                {
                    OutInBoxSerial.BranchId = BranchId;
                    OutInBoxSerial.AddUser = UserId;
                    OutInBoxSerial.AddDate = DateTime.Now;
                    _TaamerProContext.OutInBoxSerial.Add(OutInBoxSerial);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ صادر ووارد ";
                   _SystemAction.SaveAction("SavepartialOrganizations", "OutInBoxSerialService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var OutInBoxSerialUpdated = _TaamerProContext.OutInBoxSerial.Where(x=>x.OutInSerialId==OutInBoxSerial.OutInSerialId).FirstOrDefault();
                    if (OutInBoxSerialUpdated != null)
                    {
                        OutInBoxSerialUpdated.Name = OutInBoxSerial.Name;
                        OutInBoxSerialUpdated.Code = OutInBoxSerial.Code;
                        OutInBoxSerialUpdated.UpdateUser = UserId;
                        OutInBoxSerialUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل الصادر/وارد رقم " + OutInBoxSerial;
                    _SystemAction.SaveAction("SavepartialOrganizations", "OutInBoxSerialService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم التعديل" };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الصادر/وارد";
                _SystemAction.SaveAction("SavepartialOrganizations", "OutInBoxSerialService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public int? GenerateOutInBoxNumber(int outInSerialId)
        {
            return _TaamerProContext.OutInBoxSerial.Where(x => x.OutInSerialId == outInSerialId).FirstOrDefault().LastNumber + 1;
        }
        public GeneralMessage DeleteOutInBoxSerial(int OutInSerialId, int UserId, int BranchId)
        {
            try
            {
                OutInBoxSerial OutInBoxSerial = _TaamerProContext.OutInBoxSerial.Where(x => x.OutInSerialId == OutInSerialId).FirstOrDefault() ;
                OutInBoxSerial.IsDeleted = true;
                OutInBoxSerial.DeleteDate = DateTime.Now;
                OutInBoxSerial.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف الصادر/وارد رقم " + OutInSerialId;
                _SystemAction.SaveAction("DeleteOutInBoxSerial", "OutInBoxSerialService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الصادر/وارد رقم " + OutInSerialId; ;
                _SystemAction.SaveAction("DeleteOutInBoxSerial", "OutInBoxSerialService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
