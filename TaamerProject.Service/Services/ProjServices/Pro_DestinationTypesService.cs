using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Pro_DestinationTypesService : IPro_DestinationTypesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IPro_DestinationTypesRepository _Pro_DestinationTypesRepository;
        public Pro_DestinationTypesService(IPro_DestinationTypesRepository pro_DestinationTypesRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _TaamerProContext = dataContext; _SystemAction = systemAction;
            _Pro_DestinationTypesRepository = pro_DestinationTypesRepository;
        }

        public Task<IEnumerable<Pro_DestinationTypesVM>> GetAllDestinationTypes()
        {
            var DestinationTypes = _Pro_DestinationTypesRepository.GetAllDestinationTypes();
            return DestinationTypes;
        }
        public GeneralMessage SaveDestinationType(Pro_DestinationTypes DestinationType, int UserId, int BranchId)
        {
            try
            {

                if (DestinationType.DestinationTypeId == 0)
                {
                    DestinationType.AddUser = UserId;
                    DestinationType.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_DestinationTypes.Add(DestinationType);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة اسم جهة جديد";
                    _SystemAction.SaveAction("SaveDestinationType", "Pro_DestinationTypesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var DestinationTypesUpdated = _TaamerProContext.Pro_DestinationTypes.Where(s => s.DestinationTypeId == DestinationType.DestinationTypeId).FirstOrDefault();

                    if (DestinationTypesUpdated != null)
                    {
                        DestinationTypesUpdated.NameAr = DestinationType.NameAr;
                        DestinationTypesUpdated.NameEn = DestinationType.NameEn;
                        DestinationTypesUpdated.DepartmentName = DestinationType.DepartmentName;
                        DestinationTypesUpdated.UpdateUser = UserId;
                        DestinationTypesUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل اسم جهة رقم " + DestinationType.DestinationTypeId;
                    _SystemAction.SaveAction("SaveDestinationType", "Pro_DestinationTypesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الاسم جهة";
                _SystemAction.SaveAction("SaveDestinationType", "Pro_DestinationTypesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteDestinationType(int DestinationTypeId, int UserId, int BranchId)
        {
            try
            {
                Pro_DestinationTypes? DestinationType = _TaamerProContext.Pro_DestinationTypes.Where(s => s.DestinationTypeId == DestinationTypeId).FirstOrDefault();
                if (DestinationType != null)
                {
                    DestinationType.IsDeleted = true;
                    DestinationType.DeleteDate = DateTime.Now;
                    DestinationType.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف اسم جهة رقم " + DestinationTypeId;
                    _SystemAction.SaveAction("DeleteDestinationType", "Pro_DestinationTypesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف اسم جهة رقم " + DestinationTypeId; ;
                _SystemAction.SaveAction("DeleteDestinationType", "Pro_DestinationTypesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
