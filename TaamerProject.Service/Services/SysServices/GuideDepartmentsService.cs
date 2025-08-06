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
using TaamerProject.Service.Generic;
using Bayanatech.TameerPro.Repository;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class GuideDepartmentsService :   IGuideDepartmentsService
    {
        private readonly IGuideDepartmentsRepository _GuideDepartmentsRepository;
        private readonly IGuideDepartmentDetailsRepository _GuideDepartmentDetailsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public GuideDepartmentsService(IGuideDepartmentsRepository GuideDepartmentsRepository, IGuideDepartmentDetailsRepository GuideDepartmentDetailsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _GuideDepartmentsRepository = GuideDepartmentsRepository;
            _GuideDepartmentDetailsRepository = GuideDepartmentDetailsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }


        public Task<IEnumerable<GuideDepartmentsVM>> GetAllDeps(string lang, int? DepId = null)
        {
            var items = _GuideDepartmentsRepository.GetAllDeps(lang, DepId);
            return items;
        }
        public GeneralMessage SaveDepartment(GuideDepartments Dep, int UserId,int BranchId)
        {
            try
            {
                if (Dep.DepId == 0)
                {
                    Dep.AddUser = UserId;
                    Dep.AddDate = DateTime.Now;
                    _TaamerProContext.GuideDepartments.Add(Dep);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة رابط جديد";
                    _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully, ReturnedParm = Dep.DepId, ReturnedStr = Dep.DepId.ToString() };

                }
                else
                {
                    //var ItemUpdated = _GuideDepartmentsRepository.GetById(Dep.DepId);
                    GuideDepartments? ItemUpdated = _TaamerProContext.GuideDepartments.Where(s => s.DepId == Dep.DepId).FirstOrDefault();
                    //if (ItemUpdated.DepId == 1)
                    //{
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    string ActionNote2 = "غير مسموح بالتعديل هذا  النوع";
                    //    _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //    //-----------------------------------------------------------------------------------------------------------------

                    //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNote2 };

                    //}

                    if (ItemUpdated != null)
                    {
                        ItemUpdated.DepNameAr = Dep.DepNameAr;
                        ItemUpdated.DepNameEn = Dep.DepNameEn;
                        ItemUpdated.UpdateDate = Dep.UpdateDate;
                        ItemUpdated.UpdateUser = Dep.UpdateUser;
                    }
                     _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل رابط رقم " + Dep.DepId;
                    _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully, ReturnedParm = Dep.DepId, ReturnedStr = Dep.DepId.ToString() };


                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ رابط";
                _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteDepartment(int DepId, int UserId, int BranchId)
        {
            try
            {
                if (DepId != null)
                {
                    //if (DepId == 1)
                    //{
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    string ActionNote2 = "غير مسموح بحذف هذا  النوع";
                    //    _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //    //-----------------------------------------------------------------------------------------------------------------

                    //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNote2 };

                    //}
                    var dept=_TaamerProContext.GuideDepartments.Where(x=>x.DepId == DepId).FirstOrDefault();
                    dept.IsDeleted=true;
                    dept.DeleteDate = DateTime.Now;
                    dept.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تم الحذف ";
                    _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حذف رابط";
                    _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
                }
                
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف رابط";
                _SystemAction.SaveAction("SaveDepartment", "GuideDepartmentsService", 1, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }



    }
}
