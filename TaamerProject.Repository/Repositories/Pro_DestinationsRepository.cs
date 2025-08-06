using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class Pro_DestinationsRepository : IPro_DestinationsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;
        public Pro_DestinationsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Pro_DestinationsVM>> GetAllDestinations(int BranchId,List<int> BranchesList)
        {
            var Reasons = _TaamerProContext.Pro_Destinations.Where(s => s.IsDeleted == false && BranchesList.Contains(s.BranchId??0)).Select(x => new Pro_DestinationsVM
            {
                DestinationId = x.DestinationId,
                ProjectId = x.ProjectId,
                TransactionNumber = x.TransactionNumber,
                DestinationTypeId = x.DestinationTypeId,
                DepartmentId = x.DepartmentId,
                FileName = x.FileName,
                UserId = x.UserId,
                UserIdRec = x.UserIdRec,
                Counter = x.Counter??0,
                CounterRec = x.CounterRec ?? 0,
                Status = x.Status ?? 0,
                AddFileDate = x.AddFileDate,
                AddFileDateRec = x.AddFileDateRec,
                Notes = x.Notes,
                NotesRec = x.NotesRec,
                FileReasonId = x.FileReasonId,
                BranchId = x.BranchId,
                UserName = x.User != null ? x.User.FullNameAr!=null?x.User.FullNameAr ?? "" : x.User.UserName : "",
                UserRecName = x.UserRec != null ? x.UserRec.FullNameAr != null ? x.UserRec.FullNameAr ?? "" : x.UserRec.UserName : "",
                ProjectNo = x.Project != null ? x.Project.ProjectNo ?? "" : "",
                CustomerName = x.Project != null ? x.Project.customer!=null?x.Project.customer.CustomerNameAr ?? "" : "" : "",
                DestinationTypeName = x.DestinationType != null ? x.DestinationType.NameAr ?? "" : "",
                StatusName=x.Status==1?"في إنتظار الرد":x.Status==2?"قبول":x.Status==3?"رفض":"غير معلومة",
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate ?? "" : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate ?? "" : "",
                DepartmentName = x.Department != null ? x.Department.NameAr ?? "" : "",

            }).ToList();
            return Reasons;
        }
        public async Task<Pro_DestinationsVM> GetDestinationByProjectId(int projectId)
        {
            var Reasons = _TaamerProContext.Pro_Destinations.Where(s => s.IsDeleted == false && s.ProjectId== projectId).Select(x => new Pro_DestinationsVM
            {
                DestinationId = x.DestinationId,
                ProjectId = x.ProjectId,
                TransactionNumber = x.TransactionNumber,
                DestinationTypeId = x.DestinationTypeId,
                DepartmentId = x.DepartmentId,
                FileName = x.FileName,
                UserId = x.UserId,
                UserIdRec = x.UserIdRec,
                Counter = x.Counter ?? 0,
                CounterRec = x.CounterRec ?? 0,
                Status = x.Status ?? 0,
                AddFileDate = x.AddFileDate,
                AddFileDateRec = x.AddFileDateRec,
                Notes = x.Notes,
                NotesRec = x.NotesRec,
                FileReasonId = x.FileReasonId,
                BranchId = x.BranchId,
                UserName = x.User != null ? x.User.FullNameAr != null ? x.User.FullNameAr ?? "" : x.User.UserName : "",
                UserRecName = x.UserRec != null ? x.UserRec.FullNameAr != null ? x.UserRec.FullNameAr ?? "" : x.UserRec.UserName : "",
                ProjectNo = x.Project != null ? x.Project.ProjectNo ?? "" : "",
                CustomerName = x.Project != null ? x.Project.customer != null ? x.Project.customer.CustomerNameAr ?? "" : "" : "",
                DestinationTypeName = x.DestinationType != null ? x.DestinationType.NameAr ?? "" : "",
                StatusName = x.Status == 1 ? "في إنتظار الرد" : x.Status == 2 ? "قبول" : x.Status == 3 ? "رفض" : "غير معلومة",
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate ?? "" : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate ?? "" : "",
                DepartmentName = x.Department != null ? x.Department.NameAr ?? "" : "",

            }).ToList().FirstOrDefault();
            return Reasons;
        }
        public async Task<Pro_DestinationsVM> GetDestinationByProjectIdToReplay(int projectId)
        {
            var Reasons = _TaamerProContext.Pro_Destinations.Where(s => s.IsDeleted == false && s.ProjectId == projectId && (s.Status == 1 || s.Status == 0 || s.Status == null)).Select(x => new Pro_DestinationsVM
            {
                DestinationId = x.DestinationId,
                ProjectId = x.ProjectId,
                TransactionNumber = x.TransactionNumber,
                DestinationTypeId = x.DestinationTypeId,
                DepartmentId = x.DepartmentId,
                FileName = x.FileName,
                UserId = x.UserId,
                UserIdRec = x.UserIdRec,
                Counter = x.Counter ?? 0,
                CounterRec = x.CounterRec ?? 0,
                Status = x.Status ?? 0,
                AddFileDate = x.AddFileDate,
                AddFileDateRec = x.AddFileDateRec,
                Notes = x.Notes,
                NotesRec = x.NotesRec,
                FileReasonId = x.FileReasonId,
                BranchId = x.BranchId,
                UserName = x.User != null ? x.User.FullNameAr != null ? x.User.FullNameAr ?? "" : x.User.UserName : "",
                UserRecName = x.UserRec != null ? x.UserRec.FullNameAr != null ? x.UserRec.FullNameAr ?? "" : x.UserRec.UserName : "",
                ProjectNo = x.Project != null ? x.Project.ProjectNo ?? "" : "",
                CustomerName = x.Project != null ? x.Project.customer != null ? x.Project.customer.CustomerNameAr ?? "" : "" : "",
                DestinationTypeName = x.DestinationType != null ? x.DestinationType.NameAr ?? "" : "",
                StatusName = x.Status == 1 ? "في إنتظار الرد" : x.Status == 2 ? "قبول" : x.Status == 3 ? "رفض" : "غير معلومة",
                FirstProjectDate = x.Project != null ? x.Project.FirstProjectDate ?? "" : "",
                FirstProjectExpireDate = x.Project != null ? x.Project.FirstProjectExpireDate ?? "" : "",
                DepartmentName = x.Department != null ? x.Department.NameAr ?? "" : "",

            }).ToList().FirstOrDefault();
            return Reasons;
        }

    }
}
