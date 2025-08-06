using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class VacationRepository :  IVacationRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public VacationRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<VacationVM>> GetAllVacations(int? EmpId, string SearchText)
        {
            var Vacations = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId && !s.EmployeeName.IsDeleted && string.IsNullOrEmpty(s.EmployeeName.EndWorkDate)
             && !string.IsNullOrEmpty(s.EmployeeName.WorkStartDate)).Select(x => new VacationVM
            {
                VacationId = x.VacationId,
                EmployeeId = x.EmployeeId,
                VacationTypeId = x.VacationTypeId,
                
                StartDate = x.StartDate,
                StartHijriDate = x.StartHijriDate,
                BackToWorkDate = x.BackToWorkDate,
                EndDate = x.EndDate,
                EndHijriDate = x.EndHijriDate,
                VacationReason = x.VacationReason,
                VacationStatus = x.VacationStatus,
                
                IsDiscount = x.IsDiscount,
                DiscountAmount = x.DiscountAmount,
                UserId = x.UserId,
                VacationTypeName = x.VacationTypeName.NameAr,
                Date = x.Date==null ? "": x.Date,
                AcceptedDate = x.AcceptedDate == null ? "" : x.AcceptedDate,
                DecisionType = x.DecisionType,
                //DaysOfVacation = x.DaysOfVacation == null ? "" : x.DaysOfVacation,
                TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",
                 EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,

                 VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة" : x.VacationStatus == 5 ? "تم تأجيلها" : ""

            }).ToList();
            if (SearchText != "")
            {
                Vacations = Vacations.Where(s => s.VacationTypeName.Contains(SearchText.Trim()) || s.VacationStatusName.Contains(SearchText.Trim()) || s.DiscountAmount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Vacations;
        }

        public async Task<IEnumerable<VacationVM>> GetAllVacationsArchived(int? EmpId, string SearchText)
        {
            var Vacations = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.EmployeeId == EmpId && !s.EmployeeName.IsDeleted ).Select(x => new VacationVM
             {
                 VacationId = x.VacationId,
                 EmployeeId = x.EmployeeId,
                 VacationTypeId = x.VacationTypeId,

                 StartDate = x.StartDate,
                 StartHijriDate = x.StartHijriDate,
                 BackToWorkDate = x.BackToWorkDate,
                 EndDate = x.EndDate,
                 EndHijriDate = x.EndHijriDate,
                 VacationReason = x.VacationReason,
                 VacationStatus = x.VacationStatus,

                 IsDiscount = x.IsDiscount,
                 DiscountAmount = x.DiscountAmount,
                 UserId = x.UserId,
                 VacationTypeName = x.VacationTypeName.NameAr,
                 Date = x.Date == null ? "" : x.Date,
                 AcceptedDate = x.AcceptedDate == null ? "" : x.AcceptedDate,
                 DecisionType = x.DecisionType,
                 //DaysOfVacation = x.DaysOfVacation == null ? "" : x.DaysOfVacation,
                 TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",
                 EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,

                 VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة" : x.VacationStatus == 5 ? "تم تأجيلها" : ""

             }).ToList();
            if (SearchText != "")
            {
                Vacations = Vacations.Where(s => s.VacationTypeName.Contains(SearchText.Trim()) || s.VacationStatusName.Contains(SearchText.Trim()) || s.DiscountAmount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Vacations;
        }

        public async Task< IEnumerable<VacationVM>> GetAllVacations2(int? UserId, string SearchText)
        {
            var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.UserId == UserId && !s.EmployeeName.IsDeleted && string.IsNullOrEmpty(s.EmployeeName.EndWorkDate)
             && !string.IsNullOrEmpty(s.EmployeeName.WorkStartDate)).Select(x => new VacationVM
            {
                VacationId = x.VacationId,
                EmployeeId = x.EmployeeId,
                VacationTypeId = x.VacationTypeId,

                StartDate = x.StartDate,
                StartHijriDate = x.StartHijriDate,
                BackToWorkDate = x.BackToWorkDate,
                EndDate = x.EndDate,
                EndHijriDate = x.EndHijriDate,
                VacationReason = x.VacationReason,
                VacationStatus = x.VacationStatus,

                IsDiscount = x.IsDiscount,
                DiscountAmount = x.DiscountAmount,
                UserId = x.UserId,
                VacationTypeName = x.VacationTypeName.NameAr,
                Date = x.Date == null ? "" : x.Date,
                AcceptedDate = x.AcceptedDate == null ? "" : x.AcceptedDate,
                DecisionType = x.DecisionType,
                //DaysOfVacation = x.DaysOfVacation == null ? "" : x.DaysOfVacation,
                TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",
                 EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,

                 VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                x.VacationStatus == 3 ? "تم رفض الإجازة " : "تحت الطلب",
                 BranchName = x.EmployeeName.Branch.NameAr,
                 EmployeeNo = x.EmployeeName.EmployeeNo,
                 IdentityNo = x.EmployeeName.NationalId.ToString(),
                 EmployeeJob = x.EmployeeName.Job.JobNameAr,

             }).ToList();
            if (SearchText != "")
            {
                Vacations = Vacations.Where(s => s.VacationTypeName.Contains(SearchText.Trim()) || s.VacationStatusName.Contains(SearchText.Trim()) || s.DiscountAmount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Vacations;
        }
        public async Task< IEnumerable<VacationVM>> GetAllVacationsw(string SearchText)
        {
            var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.DecisionType == 1 && !s.EmployeeName.IsDeleted && string.IsNullOrEmpty(s.EmployeeName.EndWorkDate)
             && !string.IsNullOrEmpty(s.EmployeeName.WorkStartDate)).Select(x => new VacationVM
            {
                VacationId = x.VacationId,
                EmployeeId = x.EmployeeId,
                VacationTypeId = x.VacationTypeId,

                StartDate = x.StartDate,
                StartHijriDate = x.StartHijriDate,
                BackToWorkDate = x.BackToWorkDate,
                EndDate = x.EndDate,
                EndHijriDate = x.EndHijriDate,
                VacationReason = x.VacationReason,
                VacationStatus = x.VacationStatus,
                EmployeeName= x.EmployeeName.EmployeeNameAr==null?"": x.EmployeeName.EmployeeNameAr,
                //BranchName = x.Branches.NameAr == null ? "" : x.Branches.NameAr,
                IsDiscount = x.IsDiscount,
                DiscountAmount = x.DiscountAmount,
                UserId = x.UserId,
                VacationTypeName = x.VacationTypeName.NameAr,
                Date = x.Date == null ? "" : x.Date,
                AcceptedDate = x.AcceptedDate == null ? "" : x.AcceptedDate,
                DecisionType = x.DecisionType,
                //DaysOfVacation = x.DaysOfVacation == null ? "" : x.DaysOfVacation,
                TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

                VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة " :  "تم التأجيل"

            }).ToList();
            if (SearchText != "")
            {
                Vacations = Vacations.Where(s => s.VacationTypeName.Contains(SearchText.Trim()) || s.VacationStatusName.Contains(SearchText.Trim()) || s.DiscountAmount.ToString().Contains(SearchText.Trim())).ToList();
            }
            return Vacations;
        }

        public async Task< IEnumerable<VacationVM>> GetAllVacationsw2(string SearchText,int status)
        {
            var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.DecisionType == 1 && !s.EmployeeName.IsDeleted && string.IsNullOrEmpty(s.EmployeeName.EndWorkDate)
             && !string.IsNullOrEmpty(s.EmployeeName.WorkStartDate)).Select(x => new VacationVM
             {
                 VacationId = x.VacationId,
                 EmployeeId = x.EmployeeId,
                 VacationTypeId = x.VacationTypeId,

                 StartDate = x.StartDate,
                 StartHijriDate = x.StartHijriDate,
                 BackToWorkDate = x.BackToWorkDate,
                 EndDate = x.EndDate,
                 EndHijriDate = x.EndHijriDate,
                 VacationReason = x.VacationReason,
                 VacationStatus = x.VacationStatus,
                 EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,
                 //BranchName = x.Branches.NameAr == null ? "" : x.Branches.NameAr,
                 IsDiscount = x.IsDiscount,
                 DiscountAmount = x.DiscountAmount,
                 UserId = x.UserId,
                 VacationTypeName = x.VacationTypeName.NameAr,
                 Date = x.Date == null ? "" : x.Date,
                 AcceptedDate = x.AcceptedDate == null ? "" : x.AcceptedDate,
                 DecisionType = x.DecisionType,
                 //DaysOfVacation = x.DaysOfVacation == null ? "" : x.DaysOfVacation,
                 TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

                 VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة " : "تم التأجيل",
                 FileName =x.FileName??"",
                 FileUrl=x.FileUrl??"",
                 BranchName = x.EmployeeName.Branch.NameAr,
                 EmployeeNo = x.EmployeeName.EmployeeNo,
                 IdentityNo = x.EmployeeName.NationalId.ToString(),
                 EmployeeJob = x.EmployeeName.Job.JobNameAr,

             }).ToList();
            if (SearchText != "")
            {
                Vacations = Vacations.Where(s => s.VacationTypeName.Contains(SearchText.Trim()) || s.VacationStatusName.Contains(SearchText.Trim()) || s.DiscountAmount.ToString().Contains(SearchText.Trim())).ToList();
            }
            if (status > 0 && status != null)
            {
                Vacations = Vacations.Where(s => s.VacationStatus == status).ToList();

            }
            return Vacations;
        }

        public async Task< IEnumerable<VacationVM>>GetAllVacationsSearch(int BranchId)
        {
            var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.EmployeeName.IsDeleted==false && string.IsNullOrEmpty(s.EmployeeName.EndWorkDate)
             && !string.IsNullOrEmpty(s.EmployeeName.WorkStartDate) && string.IsNullOrEmpty(s.BackToWorkDate)).Select(x => new VacationVM
            {
                VacationId = x.VacationId,
                EmployeeId = x.EmployeeId,
                VacationTypeId = x.VacationTypeId,
                StartDate = x.StartDate,
                StartHijriDate = x.StartHijriDate,
                BackToWorkDate = x.BackToWorkDate,
                EndDate = x.EndDate,
                EndHijriDate = x.EndHijriDate,
                VacationReason = x.VacationReason,
                VacationStatus = x.VacationStatus,
                IsDiscount = x.IsDiscount,
                DiscountAmount = x.DiscountAmount,
                UserId = x.UserId,
                VacationTypeName = x.VacationTypeName.NameAr,
                Date = x.Date,
                AcceptedDate = x.AcceptedDate,
                DecisionType=x.DecisionType,
                //DaysOfVacation = x.DaysOfVacation,
                TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",
                 EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,

                 VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة" : x.VacationStatus == 5? "تم تأجيلها":"",
                 AcceptUser = x.UserAcccept.FullNameAr ?? "",
                 FileUrl=x.FileUrl??"",
                 BranchName = x.EmployeeName.Branch.NameAr,
                 EmployeeNo = x.EmployeeName.EmployeeNo,
                 IdentityNo = x.EmployeeName.NationalId.ToString(),
                 EmployeeJob = x.EmployeeName.Job.JobNameAr,
             }).ToList();
            return Vacations;
        }
        //public IEnumerable<VacationVM> GetAllVacationsBySearchObject2(VacationVM VacationSearch, int BranchId)
        //{
        //    try
        //    {

        //        if (VacationSearch.EndDate == null || VacationSearch.StartDate == null)
        //        {
        //            var Vacations1 =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmployeeId == VacationSearch.EmployeeId || VacationSearch.EmployeeId == null) &&
        //          (s.VacationTypeId == VacationSearch.VacationTypeId || VacationSearch.VacationTypeId == null) && (s.VacationStatus == VacationSearch.VacationStatus || VacationSearch.VacationStatus == null) &&
        //          (s.IsDiscount == VacationSearch.IsDiscount || VacationSearch.IsDiscount == null)).Select(x => new VacationVM
        //          {
        //              VacationId = x.VacationId,
        //              EmployeeId = x.EmployeeId,
        //              VacationTypeId = x.VacationTypeId,
        //              StartDate = x.StartDate,
        //              StartHijriDate = x.StartHijriDate,
        //              EndDate = x.EndDate,
        //              EndHijriDate = x.EndHijriDate,
        //              VacationReason = x.VacationReason,
        //              VacationStatus = x.VacationStatus,
        //              IsDiscount = x.IsDiscount,
        //              DiscountAmount = x.DiscountAmount,
        //              UserId = x.UserId,
        //              VacationTypeName = x.VacationTypeName.NameAr,
        //              Date = x.Date,
        //              AcceptedDate = x.AcceptedDate,
        //              DecisionType = x.DecisionType,
        //              //DaysOfVacation = x.DaysOfVacation,
        //              TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

        //              VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
        //              x.VacationStatus == 3 ? "تم رفض الإجازة " : "تم الانتهاء",
        //              EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,
        //          }).Select(s => new VacationVM
        //          {
        //              VacationId = s.VacationId,
        //              EmployeeId = s.EmployeeId,
        //              VacationTypeId = s.VacationTypeId,
        //              StartDate = s.StartDate,
        //              StartHijriDate = s.StartHijriDate,
        //              EndDate = s.EndDate,
        //              EndHijriDate = s.EndHijriDate,
        //              VacationReason = s.VacationReason,
        //              VacationStatus = s.VacationStatus,
        //              IsDiscount = s.IsDiscount,
        //              DiscountAmount = s.DiscountAmount,
        //              UserId = s.UserId,
        //              VacationTypeName = s.VacationTypeName,
        //              VacationStatusName = s.VacationStatusName,
        //              Date = s.Date,
        //              AcceptedDate = s.AcceptedDate,
        //              DecisionType = s.DecisionType,
        //              //DaysOfVacation = s.DaysOfVacation,
        //              TimeStr =s.TimeStr,
        //              EmployeeName = s.EmployeeName,
        //          }).ToList();
        //            return Vacations1;
        //        }
        //        else
        //        {
        //            var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmployeeId == VacationSearch.EmployeeId || VacationSearch.EmployeeId == null) &&
        //            (s.VacationTypeId == VacationSearch.VacationTypeId || VacationSearch.VacationTypeId == null) && (s.VacationStatus == VacationSearch.VacationStatus || VacationSearch.VacationStatus == null) &&
        //            (s.IsDiscount == VacationSearch.IsDiscount || VacationSearch.IsDiscount == null)).Select(x => new VacationVM
        //            {
        //                VacationId = x.VacationId,
        //                EmployeeId = x.EmployeeId,
        //                VacationTypeId = x.VacationTypeId,
        //                StartDate = x.StartDate,
        //                StartHijriDate = x.StartHijriDate,
        //                EndDate = x.EndDate,
        //                EndHijriDate = x.EndHijriDate,
        //                VacationReason = x.VacationReason,
        //                VacationStatus = x.VacationStatus,
        //                IsDiscount = x.IsDiscount,
        //                DiscountAmount = x.DiscountAmount,
        //                UserId = x.UserId,
        //                VacationTypeName = x.VacationTypeName.NameAr,
        //                Date = x.Date,
        //                AcceptedDate = x.AcceptedDate,
        //                DecisionType = x.DecisionType,
        //                ////DaysOfVacation = x.DaysOfVacation,
        //                TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

        //                //VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
        //                //x.VacationStatus == 3 ? "تم رفض الإجازة " : "تم الانتهاء",
        //                //EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,
        //            }).Select(s => new VacationVM
        //            {
        //                VacationId = s.VacationId,
        //                EmployeeId = s.EmployeeId,
        //                VacationTypeId = s.VacationTypeId,
        //                StartDate = s.StartDate,
        //                StartHijriDate = s.StartHijriDate,
        //                EndDate = s.EndDate,
        //                EndHijriDate = s.EndHijriDate,
        //                VacationReason = s.VacationReason,
        //                VacationStatus = s.VacationStatus,
        //                IsDiscount = s.IsDiscount,
        //                DiscountAmount = s.DiscountAmount,
        //                UserId = s.UserId,
        //                VacationTypeName = s.VacationTypeName,
        //                //VacationStatusName = s.VacationStatusName,
        //                //EmployeeName = s.EmployeeName,
        //                Date = s.Date,
        //                AcceptedDate = s.AcceptedDate,
        //                DecisionType = s.DecisionType,
        //                ////DaysOfVacation = s.DaysOfVacation,
        //                TimeStr =s.TimeStr,
        //            }).ToList().Where(s => DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
        //            //.Where(x => (DateTime.ParseExact(x.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
        //            //(DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
        //            return Vacations;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmployeeId == VacationSearch.EmployeeId ||
        //        s.VacationTypeId == VacationSearch.VacationTypeId ||
        //        s.VacationStatus == VacationSearch.VacationStatus ||
        //        s.IsDiscount == VacationSearch.IsDiscount)).Select(x => new VacationVM
        //        {
        //            VacationId = x.VacationId,
        //            EmployeeId = x.EmployeeId,
        //            VacationTypeId = x.VacationTypeId,
        //            StartDate = x.StartDate,
        //            StartHijriDate = x.StartHijriDate,
        //            EndDate = x.EndDate,
        //            EndHijriDate = x.EndHijriDate,
        //            VacationReason = x.VacationReason,
        //            VacationStatus = x.VacationStatus,
        //            IsDiscount = x.IsDiscount,
        //            DiscountAmount = x.DiscountAmount,
        //            UserId = x.UserId,
        //            VacationTypeName = x.VacationTypeName.NameAr,
        //            Date = x.Date,
        //            AcceptedDate = x.AcceptedDate,
        //            DecisionType = x.DecisionType,
        //            //DaysOfVacation = x.DaysOfVacation,
        //            TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

        //            VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
        //            x.VacationStatus == 3 ? "تم رفض الإجازة " : "تم الانتهاء",
        //            EmployeeName = x.EmployeeName.EmployeeNameAr,
        //        });
        //        return Vacations;
        //    }

        //}

        public async Task< IEnumerable<VacationVM>> GetAllVacationsBySearchObject(VacationVM VacationSearch, int BranchId)
        {
            try
            {

                if (VacationSearch.EndDate == null || VacationSearch.StartDate == null)
                {
                  var Vacations1 =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && (s.EmployeeId == VacationSearch.EmployeeId || VacationSearch.EmployeeId == null) &&
                  (s.VacationTypeId == VacationSearch.VacationTypeId || VacationSearch.VacationTypeId == null) && (s.VacationStatus == VacationSearch.VacationStatus || VacationSearch.VacationStatus == null) &&
                  (s.IsDiscount == VacationSearch.IsDiscount || VacationSearch.IsDiscount == null) /*&& string.IsNullOrEmpty(s.BackToWorkDate)*/).Select(x => new VacationVM
                  {
                      VacationId = x.VacationId,
                      EmployeeId = x.EmployeeId,
                      VacationTypeId = x.VacationTypeId,
                      StartDate = x.StartDate,
                      StartHijriDate = x.StartHijriDate,
                      BackToWorkDate = x.BackToWorkDate,
                      EndDate = x.EndDate,
                      EndHijriDate = x.EndHijriDate,
                      VacationReason = x.VacationReason,
                      VacationStatus = x.VacationStatus,
                      IsDiscount = x.IsDiscount,
                      DiscountAmount = x.DiscountAmount,
                      UserId = x.UserId,
                      VacationTypeName = x.VacationTypeName.NameAr,
                      Date = x.Date,
                      AcceptedDate = x.AcceptedDate,
                      DecisionType = x.DecisionType,
                      DaysOfVacation = x.DaysOfVacation??0,
                      //DaysOfVacation = x.DaysOfVacation,
                      TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

                      VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                      x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة" : x.VacationStatus == 5 ? "تم تأجيلها" : "",
                      EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,
                      AcceptUser = x.UserAcccept.FullNameAr ?? x.UserAcccept.FullName,
                      FileUrl = x.FileUrl??"",
                      BranchName=x.EmployeeName.Branch.NameAr,
                      EmployeeNo = x.EmployeeName.EmployeeNo,
                       IdentityNo=x.EmployeeName.NationalId.ToString(),
                       EmployeeJob=x.EmployeeName.Job.JobNameAr,

                  }).ToList();
                    return Vacations1;
                }
                else
                {
                    //var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.EmployeeId == VacationSearch.EmployeeId || VacationSearch.EmployeeId == null) &&
                    //(s.VacationTypeId == VacationSearch.VacationTypeId || VacationSearch.VacationTypeId == null) && (s.VacationStatus == VacationSearch.VacationStatus || VacationSearch.VacationStatus == null) &&
                    //(s.IsDiscount == VacationSearch.IsDiscount || VacationSearch.IsDiscount == null)).Select(x => new VacationVM
                    //{
                    //    VacationId = x.VacationId,
                    //    EmployeeId = x.EmployeeId,
                    //    VacationTypeId = x.VacationTypeId,
                    //    StartDate = x.StartDate,
                    //    StartHijriDate = x.StartHijriDate,
                    //    EndDate = x.EndDate,
                    //    EndHijriDate = x.EndHijriDate,
                    //    VacationReason = x.VacationReason,
                    //    VacationStatus = x.VacationStatus,
                    //    IsDiscount = x.IsDiscount,
                    //    DiscountAmount = x.DiscountAmount,
                    //    UserId = x.UserId,
                    //    VacationTypeName = x.VacationTypeName.NameAr,
                    //    Date = x.Date,
                    //    AcceptedDate = x.AcceptedDate,
                    //    DecisionType = x.DecisionType,
                    //    ////DaysOfVacation = x.DaysOfVacation,
                    //    TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

                    //    VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                    //    x.VacationStatus == 3 ? "تم رفض الإجازة " : "تم الانتهاء",
                    //    EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr
                    //}).Select(s => new VacationVM
                    //{
                    //    VacationId = s.VacationId,
                    //    EmployeeId = s.EmployeeId,
                    //    VacationTypeId = s.VacationTypeId,
                    //    StartDate = s.StartDate,
                    //    StartHijriDate = s.StartHijriDate,
                    //    EndDate = s.EndDate,
                    //    EndHijriDate = s.EndHijriDate,
                    //    VacationReason = s.VacationReason,
                    //    VacationStatus = s.VacationStatus,
                    //    IsDiscount = s.IsDiscount,
                    //    DiscountAmount = s.DiscountAmount,
                    //    UserId = s.UserId,
                    //    VacationTypeName = s.VacationTypeName,
                    //    VacationStatusName = s.VacationStatusName,
                    //    EmployeeName = s.EmployeeName,
                    //    Date = s.Date,
                    //    AcceptedDate = s.AcceptedDate,
                    //    DecisionType = s.DecisionType,
                    //    ////DaysOfVacation = s.DaysOfVacation,
                    //    TimeStr = s.TimeStr,
                    //}).ToList().Where(s => DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    ////.Where(x => (DateTime.ParseExact(x.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    ////(DateTime.ParseExact(x.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                    //return Vacations;

                    var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false  && (s.EmployeeId == VacationSearch.EmployeeId || VacationSearch.EmployeeId == null) &&
                    (s.VacationTypeId == VacationSearch.VacationTypeId || VacationSearch.VacationTypeId == null) && (s.VacationStatus == VacationSearch.VacationStatus || VacationSearch.VacationStatus == null) &&
                    (s.IsDiscount == VacationSearch.IsDiscount || VacationSearch.IsDiscount == null) /*&& string.IsNullOrEmpty(s.BackToWorkDate)*/).Select(x => new VacationVM
                    {
                        VacationId = x.VacationId,
                        EmployeeId = x.EmployeeId,
                        VacationTypeId = x.VacationTypeId,
                        StartDate = x.StartDate,
                        StartHijriDate = x.StartHijriDate,
                        BackToWorkDate = x.BackToWorkDate,
                        EndDate = x.EndDate,
                        EndHijriDate = x.EndHijriDate,
                        VacationReason = x.VacationReason,
                        VacationStatus = x.VacationStatus,
                        IsDiscount = x.IsDiscount,
                        DiscountAmount = x.DiscountAmount,
                        UserId = x.UserId,
                        VacationTypeName = x.VacationTypeName.NameAr,
                        Date = x.Date,
                        AcceptedDate = x.AcceptedDate,
                        DecisionType = x.DecisionType,
                        DaysOfVacation = x.DaysOfVacation??0,
                        ////DaysOfVacation = x.DaysOfVacation,
                        TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

                        VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                        x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة" : x.VacationStatus == 5 ? "تم تأجيلها" : "",
                        EmployeeName = x.EmployeeName.EmployeeNameAr == null ? "" : x.EmployeeName.EmployeeNameAr,
                        AcceptUser = x.UserAcccept.FullName ?? ""
                    }).ToList().Where(s => DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationSearch.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) &&
                                           DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationSearch.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                          );
                    return Vacations;
                }
            }
            catch (Exception ex)
            {
                var Vacations =  _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && (s.EmployeeId == VacationSearch.EmployeeId ||
                s.VacationTypeId == VacationSearch.VacationTypeId ||
                s.VacationStatus == VacationSearch.VacationStatus ||
                s.IsDiscount == VacationSearch.IsDiscount) && string.IsNullOrEmpty(s.BackToWorkDate)).Select(x => new VacationVM
                {
                    VacationId = x.VacationId,
                    EmployeeId = x.EmployeeId,
                    VacationTypeId = x.VacationTypeId,
                    StartDate = x.StartDate,
                    StartHijriDate = x.StartHijriDate,
                    BackToWorkDate = x.BackToWorkDate,
                    EndDate = x.EndDate,
                    EndHijriDate = x.EndHijriDate,
                    VacationReason = x.VacationReason,
                    VacationStatus = x.VacationStatus,
                    IsDiscount = x.IsDiscount,
                    DiscountAmount = x.DiscountAmount,
                    UserId = x.UserId,
                    VacationTypeName = x.VacationTypeName.NameAr,
                    Date = x.Date,
                    AcceptedDate = x.AcceptedDate,
                    DecisionType = x.DecisionType,
                    DaysOfVacation = x.DaysOfVacation ?? 0,
                    //DaysOfVacation = x.DaysOfVacation,
                    TimeStr = (x.DaysOfVacation < 30) ? x.DaysOfVacation + " يوم " : (x.DaysOfVacation == 30) ? x.DaysOfVacation / 30 + " شهر " : (x.DaysOfVacation > 30) ? ((x.DaysOfVacation / 30) + " شهر " + (x.DaysOfVacation % 30) + " يوم  ") : "",

                    VacationStatusName = x.VacationStatus == 1 ? "تقديم طلب" : x.VacationStatus == 2 ? "تم الموافقة على الإجازة" :
                    x.VacationStatus == 3 ? "تم رفض الإجازة " : x.VacationStatus == 4 ? "تحت المراجعة" : x.VacationStatus == 5 ? "تم تأجيلها" : "",
                    EmployeeName = x.EmployeeName.EmployeeNameAr,
                    AcceptUser = x.UserAcccept.FullName ?? ""
                });
                return Vacations;
            }

        }

        public async Task< List<string>> GetVacationDays(int EmpId, string Con)
        {
            List<string> dates = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"select* from dbo.fn_GetVacationDays("+ EmpId + ")";
                        command.Connection = con;
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        dates = (from DataRow dr in dt.Rows
                                 select ((DateTime)dr["Mdate"]).ToString("yyyy-MM-dd",CultureInfo.InvariantCulture)).ToList();
                    }
                }
                return dates;
            }
            catch (Exception ex)
            {
                return dates;
            }
        }


        public async Task<List<string>> GetVacationApprovedDays(int EmpId, string Con)
        {
            List<string> dates = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"select* from dbo.fn_GetVacationApprovedDayes(" + EmpId + ")";
                        command.Connection = con;
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        dates = (from DataRow dr in dt.Rows
                                 select ((DateTime)dr["Mdate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
                    }
                }
                return dates;
            }
            catch (Exception ex)
            {
                return dates;
            }
        }


        public async Task<List<string>> GetVacationDays_WithoutHolidays(string StartDate, string EndDate, int DawamId, string Con ,int vacationttypeiid)
        {
            List<string> dates = new List<string>();
            try
            {
                if (vacationttypeiid == 1)
                {
                    using (SqlConnection con = new SqlConnection(Con))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = @"select* from dbo.fn_GetVacationDays_WithHolidays('" + StartDate + "','" + EndDate + "'," + DawamId + ")";
                            command.Connection = con;
                            con.Open();

                            SqlDataAdapter a = new SqlDataAdapter(command);
                            DataSet ds = new DataSet();
                            a.Fill(ds);
                            DataTable dt = new DataTable();
                            dt = ds.Tables[0];

                            dates = (from DataRow dr in dt.Rows
                                     select ((DateTime)dr["Mdate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
                        }
                    }
                }
                else
                {

               
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = @"select* from dbo.fn_GetVacationDays_WithoutHolidays('" + StartDate + "','" + EndDate + "'," + DawamId +")";
                        command.Connection = con;
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        dates = (from DataRow dr in dt.Rows
                                 select ((DateTime)dr["Mdate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
                    }
                }
                }

                return dates;
            }
            catch (Exception ex)
            {
                return dates;
            }
        }
    }
}


