using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public PermissionsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId, int? Type, int? Status, string? FromDate, string? ToDate, string SearchText)
        {
            try
            {
                var query = _TaamerProContext.Permissions.Include(x=>x.Employee).Include(x=>x.UserAcccept).Include(x=>x.PermissionType).Include(x=>x.Employee.Job).Include(x=>x.Employee.Branch)
                    .Where(s => s.IsDeleted == false &&
                                (s.EmpId == EmpId || EmpId == null) &&
                                !s.Employee.IsDeleted &&
                                string.IsNullOrEmpty(s.Employee.EndWorkDate) &&
                                !string.IsNullOrEmpty(s.Employee.WorkStartDate) &&
                                (s.TypeId == Type || Type == null) &&
                                (s.Status == Status || Status == null));

                var permissionList = query.ToList();

                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    DateTime from = DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    permissionList = permissionList
                        .Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= from &&
                                    DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= to)
                        .ToList();
                }

                var Permissions = permissionList.Select(x => new PermissionsVM
                {
                    Date = x.Date ?? "",
                    AcceptedDate = x.AcceptedDate ?? "",
                    DecisionType = x.DecisionType,
                    EmpId = x.EmpId,
                    TypeId = x.TypeId,
                    AcceptedUser = x.AcceptedUser,
                    BranchId = x.BranchId,
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    PermissionHours = x.PermissionHours,
                    PermissionId = x.PermissionId,
                    Reason = x.Reason,
                    Status = x.Status,
                    UserId = x.UserId,
                    PermissionTypeName = x.PermissionType?.NameAr ?? "",        
                    EmployeName = x.Employee?.EmployeeNameAr ?? "",            
                    StatusName = x.Status == (int)PermissionStatus.New ? "طلب جديد" :
                      x.Status == (int)PermissionStatus.AtManagement ? "تحت الاجراء" :
                      x.Status == (int)PermissionStatus.Accepted ? "تمت الموافقة" : "تم الرفض",
                    AcceptUser = x.UserAcccept?.FullNameAr ?? "",
                    EmployeeJob = x.Employee.Job.JobNameAr,
                    EmployeeNo = x.Employee.EmployeeNo,
                    IdentityNo = x.Employee.NationalId.ToString(),
                    BranchName=x.Employee.Branch.NameAr,
                }).ToList();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    Permissions = Permissions
                        .Where(s => s.PermissionTypeName.Contains(SearchText.Trim()))
                        .ToList();
                }

                return Permissions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<IEnumerable<PermissionsVM>> GetAllPermissions(int? EmpId)
        {
            try
            {
                var query = _TaamerProContext.Permissions.Where(x=>x.EmpId==EmpId).Select(x => new PermissionsVM
                {
                    Date = x.Date ?? "",
                    AcceptedDate = x.AcceptedDate ?? "",
                    DecisionType = x.DecisionType,
                    EmpId = x.EmpId,
                    TypeId = x.TypeId,
                    AcceptedUser = x.AcceptedUser,
                    BranchId = x.BranchId,
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    PermissionHours = x.PermissionHours,
                    PermissionId = x.PermissionId,
                    Reason = x.Reason,
                    Status = x.Status,
                    UserId = x.UserId,
                    PermissionTypeName = x.PermissionType.NameAr ?? "",        // <-- Safe null check
                    EmployeName = x.Employee.EmployeeNameAr ?? "",             // <-- Safe null check
                    StatusName = x.Status == (int)PermissionStatus.New ? "طلب جديد" :
                      x.Status == (int)PermissionStatus.AtManagement ? "تحت الاجراء" :
                      x.Status == (int)PermissionStatus.Accepted ? "تمت الموافقة" : "تم الرفض",
                    AcceptUser = x.UserAcccept.FullNameAr ?? "",               // <-- Safe null check
                }).ToList();


                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



    }
}
