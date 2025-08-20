
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class OutInBoxTypeService : IOutInBoxTypeService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOutInBoxTypeRepository _OutInBoxTypeRepository;



        public OutInBoxTypeService(TaamerProjectContext dataContext, ISystemAction systemAction, IOutInBoxTypeRepository outInBoxType)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OutInBoxTypeRepository = outInBoxType;
        }

        public async Task< IEnumerable<OutInBoxTypeVM>> GetAllOutInBoxTypes(string SearchText, int BranchId)
        {
            var OutInBoxTypes =await _OutInBoxTypeRepository.GetAllOutInBoxTypes(SearchText, BranchId);
            return OutInBoxTypes;
        }
        public GeneralMessage SaveOutInBoxType(OutInBoxType OutInBoxType, int userId, int BranchId)
        {
            try
            {
                if (OutInBoxType.TypeId == 0)
                {
                    OutInBoxType.BranchId = BranchId;
                    OutInBoxType.AddUser = userId;
                    OutInBoxType.AddDate = DateTime.Now;
                    _TaamerProContext.OutInBoxType.Add(OutInBoxType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صادر/وارد ";
                   _SystemAction.SaveAction("SaveOutInBoxType", "OutInBoxTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, userId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var OutInBoxTypeUpdated = _TaamerProContext.OutInBoxType.Where(x => x.TypeId == OutInBoxType.TypeId).FirstOrDefault();
                    if (OutInBoxTypeUpdated != null)
                    {
                        OutInBoxTypeUpdated.NameAr = OutInBoxType.NameAr;
                        OutInBoxTypeUpdated.NameEn = OutInBoxType.NameEn;
                        OutInBoxTypeUpdated.UpdateUser = userId;
                        OutInBoxTypeUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل نوع صادر رقم " + OutInBoxType;
                    _SystemAction.SaveAction("SaveOutInBoxType", "OutInBoxTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, userId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع الصادر";
                _SystemAction.SaveAction("SaveOutInBoxType", "OutInBoxTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, userId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteOutInBoxType(int TypeId, int userId, int BranchId)
        {
            try
            {
                OutInBoxType OutInBoxType = _TaamerProContext.OutInBoxType.Where(x=>x.TypeId==TypeId).FirstOrDefault();
                OutInBoxType.IsDeleted = true;
                OutInBoxType.DeleteDate = DateTime.Now;
                OutInBoxType.DeleteUser = userId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف النوع رقم " + TypeId;
                _SystemAction.SaveAction("DeleteOutInBoxType", "OutInBoxTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, userId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف النوع رقم " + TypeId; ;
                _SystemAction.SaveAction("DeleteOutInBoxType", "OutInBoxTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, userId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillOutInBoxTypeSelect(int BranchId)
        {
            return _OutInBoxTypeRepository.GetAllOutInBoxTypes("", BranchId).Result.Select(s => new
            {
                Id = s.TypeId,
                Name = s.NameAr
            });
        }
    }
}
