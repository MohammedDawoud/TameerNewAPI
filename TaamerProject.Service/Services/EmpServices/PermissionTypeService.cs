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
using TaamerP.Service.LocalResources;
using TaamerProject.Models.ViewModels;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services.EmpServices
{
    public class PermissionTypeService : IPermissionTypeService
    {
        private readonly IPermissionTypeRepository _permissionTypeRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public PermissionTypeService(IPermissionTypeRepository permissionTypeRepository, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _permissionTypeRepository = permissionTypeRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<PermissionTypeVM>> GetAllPermissionTypes(string SearchText)
        {
            return _permissionTypeRepository.GetAllPermissionTypes(SearchText);
            
        }
        public GeneralMessage SavePermissionType(PermissionType permissionType, int UserId, int BranchId)
        {
            try
            {
                if (permissionType.TypeId == 0)
                {
                    permissionType.AddUser = UserId;
                    permissionType.AddDate = DateTime.Now;
                    permissionType.Code = (_TaamerProContext.PermissionTypes.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.PermissionTypes.Add(permissionType);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع اذن جديد";
                    _SystemAction.SaveAction("SavepermissionType", "permissionTypeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                else
                {
                    PermissionType? PermissionTypeUpdated = _TaamerProContext.PermissionTypes.Where(s => s.TypeId == permissionType.TypeId).FirstOrDefault();
                    if (PermissionTypeUpdated != null)
                    {
                        PermissionTypeUpdated.Code = permissionType.Code;
                        PermissionTypeUpdated.NameAr = permissionType.NameAr;
                        PermissionTypeUpdated.NameEn = permissionType.NameEn;
                        PermissionTypeUpdated.Notes = permissionType.Notes;
                        PermissionTypeUpdated.UpdateUser = UserId;
                        PermissionTypeUpdated.UpdateDate = DateTime.Now;
                    }

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع اذن رقم " + PermissionTypeUpdated.TypeId;
                    _SystemAction.SaveAction("SavepermissionType", "permissionTypeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع اذن";
                _SystemAction.SaveAction("SavepermissionType", "permissionTypeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeletePermissionType(int TypeId, int UserId, int BranchId)
        {
            try
            {
                PermissionType? permisiontyp = _TaamerProContext.PermissionTypes.Where(s => s.TypeId == TypeId).FirstOrDefault();
                if (permisiontyp != null)
                {
                    permisiontyp.IsDeleted = true;
                    permisiontyp.DeleteDate = DateTime.Now;
                    permisiontyp.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع اذن رقم " + TypeId;
                    _SystemAction.SaveAction("DeletePermissionType", "PermissionTypeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في نوع اذن رقم " + TypeId; ;
                _SystemAction.SaveAction("DeletePermissionType", "PermissionTypeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public Task<IEnumerable<PermissionTypeVM>> FillPermissionTypeSelect(string SearchText = "")
        {
            var permissionTypes = _permissionTypeRepository.GetAllPermissionTypes(SearchText);
            return permissionTypes;
        }

    }
}
