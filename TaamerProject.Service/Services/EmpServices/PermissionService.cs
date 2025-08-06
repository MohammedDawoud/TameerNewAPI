using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerP.Service.LocalResources;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Models.Enums;

namespace TaamerProject.Service.Services.EmpServices
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly ISystemAction _SystemAction;
        public PermissionService(IPermissionsRepository permissionsRepository,TaamerProjectContext taamerProjectContext,
            IEmployeesRepository employeesRepository, ISystemAction sys_SystemActionsService)
        {
            _permissionsRepository = permissionsRepository;
            _TaamerProContext = taamerProjectContext;
            _EmployeesRepository = employeesRepository;
            _SystemAction = sys_SystemActionsService;
        }

        public async Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId, int? Type, int? Status, string? FromDate, string? ToDate, string SearchText)
        {
            return await _permissionsRepository.GetAllPermissions(EmpId, Type,Status,FromDate,ToDate, SearchText);
        }
        public async Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId)
        {
            return await _permissionsRepository.GetAllPermissions(EmpId);
        }


        public GeneralMessage SavePermission(Permissions permission, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {

            var ExistEmp = _EmployeesRepository.GetEmployeeByUserid(UserId).Result.FirstOrDefault();
            
            if (ExistEmp == null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الإجازة";
                _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.UserNotAssociatedWithEmployee, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.UserNotAssociatedWithEmployee };
            }
            else if (string.IsNullOrEmpty(ExistEmp.WorkStartDate) || !string.IsNullOrEmpty(ExistEmp.EndWorkDate))
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                string msg = string.IsNullOrEmpty(ExistEmp.WorkStartDate) ? "لا يمكنك عمل طلب اذن وذلك لعدم مباشرة الموظف للعمل" :
                    "لا يمكنك عمل طلب اذن وذلك لإنتهاء خدمات الموظف";
                _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = msg };
            }
           

            else
            {
                try
                {


                    //  var emp = _EmployeesRepository.GetById(ExistEmp.EmployeeId);
                    Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == ExistEmp.EmployeeId).FirstOrDefault();

                    if (emp != null && permission.PermissionId == 0)
                    {

                      


                        permission.AddUser = UserId;
                        permission.EmpId = ExistEmp.EmployeeId;
                        permission.UserId = UserId;

                        permission.DecisionType = 0;

                        permission.BranchId = BranchId;
                        permission.UserId = emp.UserId;
                        permission.AddDate = DateTime.Now;
                        _TaamerProContext.Permissions.Add(permission);

                        //VacationType? vactionType = _TaamerProContext.VacationType.Where(s => s.VacationTypeId == vacation.VacationTypeId).FirstOrDefault();

                        _TaamerProContext.SaveChanges();
                        //NewVacation_Notification(Lang, vacation, emp, Lang == "rtl" ? vactionType.NameAr : vactionType.NameEn, UserId, BranchId, Url, ImgUrl);


                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة اذن جديدة";
                        _SystemAction.SaveAction("Savepermission", "permissionService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = permission.PermissionId };
                    }
                    else
                    {
                        //if (emp != null)
                        //{
                        //    if (emp.UserId != null)
                        //    {
                        //        var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                        //        if (userTasks > 0)
                        //        {
                        //            var massage2 = "";
                        //            if (Lang == "rtl")
                        //            {
                        //                massage2 = userTasks + Resources.ExistingWorkTasksTransferredAnotherUser;
                        //            }
                        //            else
                        //            {
                        //                massage2 = userTasks + Resources.ExistingWorkTasksTransferredAnotherUser;
                        //            }


                        //            //-----------------------------------------------------------------------------------------------------------------
                        //            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        //            string ActionNote2 = "فشل في حفظ الاذن";
                        //            _SystemAction.SaveAction("SaveVacation", "VacationService", 1, massage2, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //            //-----------------------------------------------------------------------------------------------------------------

                        //            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage2 };
                        //        }

                        //    }
                        //    var userLoan = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);

                        //    var userLoanDatails = 0;
                        //    if (userLoan != null)
                        //    {
                        //        foreach (var item in userLoan)
                        //        {
                        //            userLoanDatails = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();

                        //        }
                        //    }
                        //    if (userLoanDatails > 0)
                        //    {
                        //        var massage1 = "";
                        //        if (Lang == "rtl")
                        //        {
                        //            massage1 = Resources.employeeEdvancesBeSettledFirst;
                        //        }
                        //        else
                        //        {
                        //            massage1 = Resources.employeeEdvancesBeSettledFirst;
                        //        }

                        //        //-----------------------------------------------------------------------------------------------------------------
                        //        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        //        string ActionNote3 = "فشل في حفظ الإجازة";
                        //        _SystemAction.SaveAction("SaveVacation", "VacationService", 1, massage1, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                        //        //-----------------------------------------------------------------------------------------------------------------

                        //        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage1 };
                        //    }
                       
                        //}


                        Permissions? PermissionUpdated = _TaamerProContext.Permissions.Where(s => s.PermissionId == permission.PermissionId).FirstOrDefault();

                        if (PermissionUpdated != null)
                        {
                            PermissionUpdated.EmpId = permission.EmpId;
                            PermissionUpdated.TypeId = permission.TypeId;
                            PermissionUpdated.Date = permission.Date;
                            PermissionUpdated.Reason = permission.Reason;
                            PermissionUpdated.Status = permission.Status;
                            PermissionUpdated.UserId = emp.UserId;
                            PermissionUpdated.UpdateUser = UserId;
                            PermissionUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل اذن رقم " + permission.PermissionId;
                        _SystemAction.SaveAction("SaveVacation", "VacationService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الاذن";
                    _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }

            }

        }

        public GeneralMessage Updatepermission(int PermissionId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl, string? reason)
        {
            try
            {

                var permission = _TaamerProContext.Permissions.Where(x => x.PermissionId == PermissionId).FirstOrDefault();
                if(permission != null)
                {
                    
                    if (Type != (int)PermissionStatus.AtManagement)
                    {
                        permission.AcceptedDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        permission.AcceptedUser = UserId;
                    }
                    permission.Status = Type;
                    permission.UpdateUser = UserId;


                }


                _TaamerProContext.SaveChanges();
                var massage = Type == (int)PermissionStatus.Accepted ? "الموافقة علي اذن" : "رفض الاذن";

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل اذن برقم" + PermissionId;
                _SystemAction.SaveAction("permissionVacation", "permissionService", 2, massage, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = massage };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل الاذن";
                _SystemAction.SaveAction("Updatepermission", "permissionService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeletePermissions(int PermissionId, int UserId, int BranchId)
        {
            try
            {
                Permissions? permission = _TaamerProContext.Permissions.Where(s => s.PermissionId == PermissionId).FirstOrDefault();
                if (permission != null)
                {
                    permission.IsDeleted = true;
                    permission.DeleteDate = DateTime.Now;
                    permission.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حذف الاذن ";
                    _SystemAction.SaveAction("Deletepermission", "permissionService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف الاذن ";
                _SystemAction.SaveAction("Deletepermission", "permissionService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }



    }
}
