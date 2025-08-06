using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    public class DepartmentService :  IDepartmentService
    {
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public DepartmentService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IDepartmentRepository DepartmentRepository)
        {
            _DepartmentRepository = DepartmentRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<DepartmentVM>> GetAllDepartment(string SearchText,int BranchId)
        {
            var departments = _DepartmentRepository.GetAllDepartment(SearchText, BranchId);
            return departments;
        }

        public Task<IEnumerable<DepartmentVM>> GetExternalDepartment()
        {
            var departments = _DepartmentRepository.GetExternalDepartment();
            return departments;
        }
        public Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyType(int? Type, int BranchId,string SearchText)
        {
            var departments = _DepartmentRepository.GetAllDepartmentbyType(Type, BranchId, SearchText);
            return departments;
        }

 

        public Task<IEnumerable<DepartmentVM>> GetAllDepartmentbyTypeUser(int? Type, int BranchId, string SearchText)
        {
            var departments = _DepartmentRepository.GetAllDepartmentbyTypeUser(Type, BranchId, SearchText);
            return departments;
        }



        public Task<IEnumerable<DepartmentVM>> GetDepartmentbyid(int DeptId)
        {
            var departments = _DepartmentRepository.GetDepartmentbyid(DeptId);
            return departments;
        }


        public GeneralMessage SaveDepartment(Department department, int UserId, int BranchId)
        {
            try
            {
                if (department.DepartmentId == 0)
                {
                    department.BranchId = BranchId;
                    department.AddUser = UserId;
                    department.AddDate = DateTime.Now;
                    _TaamerProContext.Department.Add(department);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة قسم جديد" +department.DepartmentNameAr;
                     _SystemAction.SaveAction("SaveDepartment", "DepartmentService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var DepartmentUpdated = _DepartmentRepository.GetById(department.DepartmentId);
                    Department? DepartmentUpdated = _TaamerProContext.Department.Where(s => s.DepartmentId == department.DepartmentId).FirstOrDefault();
                    if (DepartmentUpdated != null)
                    {
                        DepartmentUpdated.DepartmentNameAr = department.DepartmentNameAr;
                        DepartmentUpdated.DepartmentNameEn = department.DepartmentNameEn;
                        DepartmentUpdated.UpdateUser = UserId;
                        DepartmentUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل قسم رقم " + department.DepartmentNameAr;
                     _SystemAction.SaveAction("SaveDepartment", "DepartmentService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_EditedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في قسم البند"+department.DepartmentNameEn;
                 _SystemAction.SaveAction("SaveDepartment", "DepartmentService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteDepartment(int DepartmentId, int UserId,int BranchId)
        {
            try
            {
                //Department department = _DepartmentRepository.GetById(DepartmentId);
                Department? department = _TaamerProContext.Department.Where(s => s.DepartmentId == DepartmentId).FirstOrDefault();
                if (department != null)
                {
                    department.IsDeleted = true;
                    department.DeleteDate = DateTime.Now;
                    department.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف قسم  " + department.DepartmentNameAr;
                    _SystemAction.SaveAction("DeleteDepartment", "DepartmentService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف قسم رقم " + DepartmentId; ;
                 _SystemAction.SaveAction("DeleteDepartment", "DepartmentService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        //public IEnumerable<DepartmentVM> GetAllDeptsByBranchId(int branchId)
        //{
        //    var departments = _DepartmentRepository.GetAllDeptsByBranchId(branchId).ToList();
        //    return departments;
        //}


    }
}
