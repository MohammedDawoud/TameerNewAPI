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
    public class Pro_DestinationDepartmentsService : IPro_DestinationDepartmentsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IPro_DestinationDepartmentsRepository _Pro_DestinationDepartmentsRepository;
        public Pro_DestinationDepartmentsService(IPro_DestinationDepartmentsRepository pro_DestinationDepartmentsRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _TaamerProContext = dataContext; _SystemAction = systemAction;
            _Pro_DestinationDepartmentsRepository = pro_DestinationDepartmentsRepository;
        }

        public Task<IEnumerable<Pro_DestinationDepartmentsVM>> GetAllDestinationDepartments()
        {
            var DestinationDepartments = _Pro_DestinationDepartmentsRepository.GetAllDestinationDepartments();
            return DestinationDepartments;
        }
        public Task<IEnumerable<Pro_DestinationDepartmentsVM>> GetAllDestinationDepartmentsByTypeId(int TypeId)
        {
            var DestinationDepartments = _Pro_DestinationDepartmentsRepository.GetAllDestinationDepartmentsByTypeId(TypeId);
            return DestinationDepartments;
        }
        public GeneralMessage SaveDestinationDepartment(Pro_DestinationDepartments DestinationDepartment, int UserId, int BranchId)
        {
            try
            {

                if (DestinationDepartment.DepartmentId == 0)
                {
                    DestinationDepartment.AddUser = UserId;
                    DestinationDepartment.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_DestinationDepartments.Add(DestinationDepartment);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة اسم جهة جديد";
                    _SystemAction.SaveAction("SaveDestinationDepartment", "Pro_DestinationDepartmentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var DestinationDepartmentsUpdated = _TaamerProContext.Pro_DestinationDepartments.Where(s => s.DepartmentId == DestinationDepartment.DepartmentId).FirstOrDefault();

                    if (DestinationDepartmentsUpdated != null)
                    {
                        DestinationDepartmentsUpdated.NameAr = DestinationDepartment.NameAr;
                        DestinationDepartmentsUpdated.NameEn = DestinationDepartment.NameEn;
                        DestinationDepartmentsUpdated.UpdateUser = UserId;
                        DestinationDepartmentsUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل اسم جهة رقم " + DestinationDepartment.DepartmentId;
                    _SystemAction.SaveAction("SaveDestinationDepartment", "Pro_DestinationDepartmentsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الاسم جهة";
                _SystemAction.SaveAction("SaveDestinationDepartment", "Pro_DestinationDepartmentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteDestinationDepartment(int DepartmentId, int UserId, int BranchId)
        {
            try
            {
                Pro_DestinationDepartments? DestinationDepartment = _TaamerProContext.Pro_DestinationDepartments.Where(s => s.DepartmentId == DepartmentId).FirstOrDefault();
                if (DestinationDepartment != null)
                {
                    DestinationDepartment.IsDeleted = true;
                    DestinationDepartment.DeleteDate = DateTime.Now;
                    DestinationDepartment.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف اسم جهة رقم " + DepartmentId;
                    _SystemAction.SaveAction("DeleteDestinationDepartment", "Pro_DestinationDepartmentsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف اسم جهة رقم " + DepartmentId; ;
                _SystemAction.SaveAction("DeleteDestinationDepartment", "Pro_DestinationDepartmentsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
