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
using TaamerProject.Models.Enums;

namespace TaamerProject.Repository.Repositories
{
    public class EmployeesRepository :  IEmployeesRepository
    {
     
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly IAttendenceRepository _attendenceRepository;
        private readonly IVacationRepository _vacationRepository;
        private readonly IAllowanceRepository _allowanceRepository;
        private readonly IDiscountRewardRepository _discountRewardRepository;
        private readonly ICarMovementsRepository _carMovementsRepository;

        public EmployeesRepository(TaamerProjectContext dataContext,  IAttendenceRepository attendence, IVacationRepository vacation, IAllowanceRepository allowanceRepository, IDiscountRewardRepository discountRewardRepository,
           ICarMovementsRepository carMovementsRepository)
        {
            _TaamerProContext = dataContext;
            _attendenceRepository = attendence;
            _vacationRepository = vacation;
            _allowanceRepository = allowanceRepository;
            _discountRewardRepository = discountRewardRepository;
            _carMovementsRepository = carMovementsRepository;
        }

        public async Task<IEnumerable<EmployeesVM>> FillAllEmployees(string lang, int BranchId)
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.WorkStartDate) && string.IsNullOrEmpty(s.EndWorkDate) && s.BranchId == BranchId).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
            }).ToList();
            return employees;
        }
        public async Task<IEnumerable<EmployeesVM>> FillSelectEmployeeWorkers(string lang, int BranchId)
        {
            //var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.WorkStartDate)  && string.IsNullOrEmpty(s.EndWorkDate) && s.BranchId == BranchId && s.UserId==null).Select(x => new EmployeesVM
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && (s.UserId == null || s.UserId==0) && !string.IsNullOrEmpty(s.WorkStartDate)).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                WorkStartDate =x.WorkStartDate,
                EndWorkDate=x.EndWorkDate,
                ContractStartDate=x.ContractStartDate,
                ContractEndDate=x.ContractEndDate,
            }).ToList();

            employees = employees.Where(s => (int.Parse(s.WorkStartDate.Split('-')[0]) <= DateTime.Now.Year) &&
           (string.IsNullOrEmpty(s.EndWorkDate) || (!string.IsNullOrEmpty(s.EndWorkDate) && ((int.Parse(s.EndWorkDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.EndWorkDate.Split('-')[1]) >= DateTime.Now.Month) && (int.Parse(s.EndWorkDate.Split('-')[0]) == DateTime.Now.Year)))))
         ).ToList();

            return employees;
        }


 
        public async Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int BranchId) // شئون الموظفين
        {
            try
            {
                var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate) && s.BranchId == BranchId).Select(x => new EmployeesVM
                {
                    EmployeeId = x.EmployeeId,
                    EmployeeNo = x.EmployeeNo,
                    EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                    EmployeeNameAr = x.EmployeeNameAr,
                    EmployeeNameEn = x.EmployeeNameEn,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    Address = x.Address,
                    NationalId = x.NationalId,
                    AccountId = x.AccountId,
                    EducationalQualification = x.EducationalQualification,
                    BirthDate = x.BirthDate,
                    BirthHijriDate = x.BirthHijriDate,
                    BirthPlace = x.BirthPlace,
                    MaritalStatus = x.MaritalStatus,
                    ChildrenNo = x.ChildrenNo,
                    Gender = x.Gender,
                    NationalityId = x.NationalityId,
                    ReligionId = x.ReligionId,
                    UserId = x.UserId,
                    JobId = x.JobId,
                    DepartmentId = x.DepartmentId,
                    BranchId = x.BranchId,
                    Telephone = x.Telephone,
                    Mailbox = x.Mailbox,
                    NationalIdSource = x.NationalIdSource,
                    NationalIdDate = x.NationalIdDate,
                    NationalIdHijriDate = x.NationalIdHijriDate,
                    NationalIdEndDate = x.NationalIdEndDate,
                    NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                    PassportNo = x.PassportNo,
                    PassportSource = x.PassportSource,
                    PassportNoDate = x.PassportNoDate,
                    PassportNoHijriDate = x.PassportNoHijriDate,
                    PassportEndDate = x.PassportEndDate,
                    PassportEndHijriDate = x.PassportEndHijriDate,
                    ContractNo = x.ContractNo,
                    ContractSource = x.ContractSource,
                    ContractStartDate = x.ContractStartDate,
                    ContractStartHijriDate = x.ContractStartHijriDate,
                    ContractEndDate = x.ContractEndDate,
                    ContractEndHijriDate = x.ContractEndHijriDate,
                    MedicalNo = x.MedicalNo,
                    MedicalSource = x.MedicalSource,
                    MedicalStartDate = x.MedicalStartDate,
                    MedicalStartHijriDate = x.MedicalStartHijriDate,
                    MedicalEndDate = x.MedicalEndDate,
                    MedicalEndHijriDate = x.MedicalEndHijriDate,
                    LicenceNo = x.LicenceNo,
                    LiscenseSourceId = x.LiscenseSourceId,
                    LicenceStartDate = x.LicenceStartDate,
                    LicenceStartHijriDate = x.LicenceStartHijriDate,
                    LicenceEndDate = x.LicenceEndDate,
                    LicenceEndHijriDate = x.LicenceEndHijriDate,
                    DawamId = x.DawamId,
                    TimeDurationLate = x.TimeDurationLate,
                    LogoutDuration = x.LogoutDuration,
                    AfterLogoutTime = x.AfterLogoutTime,
                    Salary = x.Salary ?? 0,
                    Bonus = x.Bonus ?? 0,
                    AddAllowances =x.Allowances?? 0,
                    TotalViolations = 0,
                    VacationEndCount = x.VacationEndCount??0,
                    WorkStartDate = x.WorkStartDate,
                    EndWorkDate = x.EndWorkDate,
                    WorkStartHijriDate = x.WorkStartHijriDate,
                    //WorkEndDate = x.WorkEndDate,
                    //WorkEndHijriDate = x.WorkEndHijriDate,
                    NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                    PhotoUrl = x.PhotoUrl,
                    DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                    JobName = x.Job!=null? x.Job.JobNameAr:"",
                    BranchName = x.Branch.NameAr,
                    BankName = x.Bank!=null? x.Bank.NameAr:"",
                    AcountName = x.Account!=null? x.Account.NameAr:"",
                    AccountCode = x.Account!=null? x.Account.Code:"",
                    AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                    GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                    MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                    ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                    NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                    DeppID = x.DeppID,
                    PostalCode = x.PostalCode,
                    AddDate = x.AddDate,
                    AddUser = x.AddUser,

                    AccountIDs = x.AccountIDs,
                    AccountIDs_Bouns = x.AccountIDs_Bouns,
                    AccountIDs_Custody = x.AccountIDs_Custody,
                    AccountIDs_Discount = x.AccountIDs_Discount,
                    AccountIDs_Salary = x.AccountIDs_Salary,
                    BankCardNo = x.BankCardNo,
                    BankId = x.BankId,
                    Taamen = x.Taamen,
                    EarlyLogin = x.EarlyLogin,
                    UserName = x.users.UserName ?? "",
                    DirectManager = x.DirectManager,

                    MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == true).Sum(s => s.AllowanceAmount) ?? 0,
                    ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && !s.IsFixed && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                    TotalLoans = x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0,
                    TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                    TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0,
                    TotalySalaries = (x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0)
                                    + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
                    TotalDayAbs = x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault(),
                    QuaContract=x.QuaContract??"",
                    VacationsCount = x.VacationsCount,
                    Allowances=x.Allowances==null?
                    x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == true && s.AllowanceTypeId==4).Sum(s => s.AllowanceAmount)??0: x.Allowances,
                    OtherAllownces=x.OtherAllownces??0,
                    HousingAllowance = ((x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true && s.AllowanceType.NameEn == "Housing allowance").FirstOrDefault()).AllowanceAmount ?? x.Allowances??0) + (x.OtherAllownces ?? 0),
                    Age=x.Age,
                    //AttendenceLocationId =x.AttendenceLocationId,
                    allowoutsidesite=x.allowoutsidesite??false,
                    allowallsite = x.allowallsite ?? false,
                    EmpHourlyCost =x.EmpHourlyCost ??0,
                    DailyWorkinghours =x.DailyWorkinghours??0,
                }).ToList();
                return employees;
            }catch(Exception ex)
            {
                throw new Exception();

            }

        }
        public async Task<IEnumerable<EmployeesVM>> GetAllEmployeesByLocationId(string lang, int LocationId) // شئون الموظفين
        {
            try
            {
                var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate) && s.AttendenceLocationId == LocationId).Select(x => new EmployeesVM
                {
                    EmployeeId = x.EmployeeId,
                    EmployeeNo = x.EmployeeNo,
                    EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                    EmployeeNameAr = x.EmployeeNameAr,
                    EmployeeNameEn = x.EmployeeNameEn,
                    Email = x.Email,
                    BranchId = x.BranchId,
                    DepartmentName = x.Department != null ? x.Department.DepartmentNameAr : "",
                    JobName = x.Job != null ? x.Job.JobNameAr : "",
                    BranchName = x.Branch.NameAr,
                    UserName = x.users.UserName ?? "",
                    //AttendenceLocationId = x.AttendenceLocationId,
                    //AttendenceLocationName = x.AttendenceLocation != null ? x.AttendenceLocation.Name : "",
                    allowoutsidesite = x.allowoutsidesite ?? false,
                    allowallsite = x.allowallsite ?? false,
                    EmpHourlyCost = x.EmpHourlyCost ?? 0,
                    DailyWorkinghours = x.DailyWorkinghours ?? 0,
                }).ToList();
                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception();

            }

        }


        public async Task<IEnumerable<EmployeesVM>> GetAllArchivesEmployees(string lang, int BranchId) // شئون الموظفين
        {
            var employees = _TaamerProContext.Employees.Where(s => !string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                MaritalStatus = x.MaritalStatus,
                ChildrenNo = x.ChildrenNo,
                Gender = x.Gender,
                NationalityId = x.NationalityId,
                ReligionId = x.ReligionId,
                UserId = x.UserId,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                BranchId = x.BranchId,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                NationalIdSource = x.NationalIdSource,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportNo = x.PassportNo,
                PassportSource = x.PassportSource,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractSource = x.ContractSource,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                LicenceNo = x.LicenceNo,
                LiscenseSourceId = x.LiscenseSourceId,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                Salary = x.Salary ?? 0,
                Bonus = x.Bonus ?? 0,
                AddAllowances = 0,
                TotalViolations = 0,
                VacationEndCount = x.VacationEndCount,
                WorkStartDate = x.WorkStartDate,
                EndWorkDate = x.EndWorkDate,
                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                PhotoUrl = x.PhotoUrl,
                DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName = x.Job!=null? x.Job.JobNameAr:"",
                BranchName = x.Branch.NameAr,
                BankName = x.Bank!=null? x.Bank.NameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                AddDate = x.AddDate,
                AddUser = x.AddUser,

                AccountIDs = x.AccountIDs,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                BankCardNo = x.BankCardNo,
                BankId = x.BankId,
                Taamen = x.Taamen,
                EarlyLogin = x.EarlyLogin,
            EmpServiceDuration=x.EmpServiceDuration,
             ResonLeave=x.ResonLeave,
                DirectManager = x.DirectManager,
                Allowances=x.Allowances??0,
                WorkSource=x.WorkSource,
                

                MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == true).Sum(s => s.AllowanceAmount) ?? 0,
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && !s.IsFixed && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                TotalLoans = x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0,
                TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0,
                TotalySalaries = (x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0)
                                + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
                TotalDayAbs = x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault(),
                //VacationsCount = x.VacationsCount - x.VacationEndCount,
                EmpHourlyCost = x.EmpHourlyCost ?? 0,
                DailyWorkinghours = x.DailyWorkinghours ?? 0,
            }).ToList();
            return employees;

        }
        public async Task<IEnumerable<EmployeesVM>> GetAllBranchEmployees(string lang)
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.WorkStartDate) && string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                MaritalStatus = x.MaritalStatus,
                ChildrenNo = x.ChildrenNo,
                Gender = x.Gender,
                NationalityId = x.NationalityId,
                ReligionId = x.ReligionId,
                UserId = x.UserId,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                BranchId = x.BranchId,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                NationalIdSource = x.NationalIdSource,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportNo = x.PassportNo,
                PassportSource = x.PassportSource,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractSource = x.ContractSource,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                LicenceNo = x.LicenceNo,
                LiscenseSourceId = x.LiscenseSourceId,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                Salary = x.Salary ?? 0,
                Bonus = x.Bonus ?? 0,
                AddAllowances = 0,
                TotalViolations = 0,
                VacationsCount = x.VacationsCount,
                VacationEndCount = x.VacationEndCount,
                WorkStartDate = x.WorkStartDate,
                EndWorkDate = x.EndWorkDate,
                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                NodeLocation =x.NodeLocations!=null? x.NodeLocations!=null? x.NodeLocations.Location:"":"",
                PhotoUrl = x.PhotoUrl,
                DepartmentName =x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName =x.Job!=null? x.Job!=null? x.Job.JobNameAr:"":"",
                BankName =x.Bank!=null? x.Bank.NameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                AttendaceTimeName = x.AttendaceTime!=null?  x.AttendaceTime.NameAr:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName =  x.Nationality!=null? x.Nationality.NameAr:"",
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                AddDate = x.AddDate,
                AddUser = x.AddUser,

                AccountIDs = x.AccountIDs,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                Taamen = x.Taamen,
                EarlyLogin=x.EarlyLogin,
                DirectManager = x.DirectManager,

                MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == true).Sum(s => s.AllowanceAmount) ?? 0,
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == false && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                
                TotalLoans = x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0,
                TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0,
                TotalySalaries = ((x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0))- (((x.Salary ?? 0) / 30) * (x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault()))
                                + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
                TotalDayAbs = x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault(),
            }).ToList();
            return employees;

        } 
        public async Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int SearchAll, int Branch)
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.WorkStartDate) && string.IsNullOrEmpty(s.EndWorkDate) && s.BranchId == Branch).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                MaritalStatus = x.MaritalStatus,
                ChildrenNo = x.ChildrenNo,
                Gender = x.Gender,
                NationalityId = x.NationalityId,
                ReligionId = x.ReligionId,
                UserId = x.UserId,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                BranchId = x.BranchId,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                NationalIdSource = x.NationalIdSource,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportNo = x.PassportNo,
                PassportSource = x.PassportSource,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractSource = x.ContractSource,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                LicenceNo = x.LicenceNo,
                LiscenseSourceId = x.LiscenseSourceId,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                Salary = x.Salary ?? 0,
                Bonus = x.Bonus ?? 0,
                AddAllowances = 0,
                TotalViolations = 0,
                VacationsCount = x.VacationsCount,
                VacationEndCount = x.VacationEndCount,
                WorkStartDate = x.WorkStartDate,
                EndWorkDate = x.EndWorkDate,

                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                PhotoUrl = x.PhotoUrl,
                DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName = x.Job!=null? x.Job.JobNameAr:"",
                BankName = x.Bank!=null? x.Bank.NameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                AddDate = x.AddDate,
                AddUser = x.AddUser,
                EarlyLogin=x.EarlyLogin,
                DirectManager = x.DirectManager,

                AccountIDs = x.AccountIDs,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                Taamen = x.Taamen,
                MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == true).Sum(s => s.AllowanceAmount) ?? 0,
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == false && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,

                TotalLoans = x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0,
                TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0,
                TotalySalaries = (x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0)
                                + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
            }).ToList();
            return employees;

        }
        public async Task<IEnumerable<EmployeesVM>>  GetAllEmployeesSearch(string lang, int BranchId)
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.WorkStartDate) && !string.IsNullOrEmpty(s.EndWorkDate) && s.BranchId == BranchId).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                MaritalStatus = x.MaritalStatus,
                ChildrenNo = x.ChildrenNo,
                Gender = x.Gender,
                NationalityId = x.NationalityId,
                ReligionId = x.ReligionId,
                UserId = x.UserId,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                BranchId = x.BranchId,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                NationalIdSource = x.NationalIdSource,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportNo = x.PassportNo,
                PassportSource = x.PassportSource,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractSource = x.ContractSource,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                LicenceNo = x.LicenceNo,
                LiscenseSourceId = x.LiscenseSourceId,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                Salary = x.Salary ?? 0,
                ThisMonthSalary = x.Salary ?? 0,
                Bonus = x.Bonus ?? 0,
                AddAllowances = 0,
                TotalViolations = 0,
                VacationsCount = x.VacationsCount,
                VacationEndCount = x.VacationEndCount,
                WorkStartDate = x.WorkStartDate,
                EndWorkDate = x.EndWorkDate,

                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                PhotoUrl = x.PhotoUrl,
                DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName = x.Job!=null? x.Job.JobNameAr:"",
                BankName = x.Bank!=null? x.Bank.NameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                AddDate = x.AddDate,
                AddUser = x.AddUser,

                AccountIDs = x.AccountIDs,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                Taamen = x.Taamen,
                EarlyLogin=x.EarlyLogin,
                DirectManager = x.DirectManager,
                
                //CommunicationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                        s.AllowanceType.NameEn == "Communications").FirstOrDefault()).AllowanceAmount ?? 0,

                //ProfessionAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                      s.AllowanceType.NameEn == "Profession").FirstOrDefault()).AllowanceAmount ?? 0,

                //TransportationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                      s.AllowanceType.NameEn == "Transportation").FirstOrDefault()).AllowanceAmount ?? 0,

                HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true && s.AllowanceType.NameEn == "Housing allowance").FirstOrDefault()).AllowanceAmount ?? 0,


                MonthlyAllowances = (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ??0)+ (x.OtherAllownces ?? 0),
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == false && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                
                TotalLoans = x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0,
                TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0,
                TotalySalaries = ((x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0))- (((x.Salary ?? 0) / 30) * (x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault()))
                                + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
                 TotalDayAbs=x.AttAbsentDays.Where(w=>w.EmpId==x.EmployeeId && w.Year==DateTime.Now.Year && w.Month== DateTime.Now.Month).Select(s=>s.AbsDays).FirstOrDefault(),
                EmpHourlyCost = x.EmpHourlyCost ?? 0,
                DailyWorkinghours = x.DailyWorkinghours ?? 0,
            }).ToList();
            return employees;

        }
        public async Task<IEnumerable<EmployeesVM>>  GetEmployeeByUserid(int UserId)
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.UserId == UserId).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                MaritalStatus = x.MaritalStatus,
                ChildrenNo = x.ChildrenNo,
                Gender = x.Gender,
                NationalityId = x.NationalityId,
                ReligionId = x.ReligionId,
                UserId = x.UserId,
                JobId = x.JobId,
                DepartmentId = x.DepartmentId,
                BranchId = x.BranchId,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                NationalIdSource = x.NationalIdSource,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportNo = x.PassportNo,
                PassportSource = x.PassportSource,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractSource = x.ContractSource,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                LicenceNo = x.LicenceNo,
                LiscenseSourceId = x.LiscenseSourceId,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                Salary = x.Salary ?? 0,
                Bonus = x.Bonus ?? 0,
                AddAllowances = 0,
                TotalViolations = 0,
                VacationsCount = x.VacationsCount,
                VacationEndCount = x.VacationEndCount,
                WorkStartDate = x.WorkStartDate,
                EndWorkDate = x.EndWorkDate,

                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                PhotoUrl = x.PhotoUrl,
                DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName = x.Job!=null? x.Job.JobNameAr:"",
                BankName = x.Bank!=null? x.Bank.NameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                AddDate = x.AddDate,
                AddUser = x.AddUser,

                AccountIDs = x.AccountIDs,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                Taamen = x.Taamen,
                EarlyLogin=x.EarlyLogin,
                DirectManager = x.DirectManager,


                MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == true).Sum(s => s.AllowanceAmount) ?? 0,
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false &&  s.IsFixed == false && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,

                TotalLoans = x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0,
                TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0,
                TotalySalaries = (x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0)
                                + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
            }).ToList();
            return employees;
        }

        public async Task<IEnumerable<EmployeesVM>>  GetAllEmployeesBySearchObject(EmployeesVM SalarySearch, string lang, int BranchId, string Con="")
        {
            try
            {

                if (SalarySearch.MonthNo > 0)
                {
                    int MonthNo = DateTime.Now.Month;

                    var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false &&
                                           !string.IsNullOrEmpty(s.WorkStartDate) && s.ContractStartDate != null).Select(x => new EmployeesVM
                                           {
                                               EmployeeId = x.EmployeeId,
                                               EmployeeNo = x.EmployeeNo,
                                               EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                               EmployeeNameAr = x.EmployeeNameAr,
                                               EmployeeNameEn = x.EmployeeNameEn,
                                               Email = x.Email,
                                               Mobile = x.Mobile,
                                               Address = x.Address,
                                               NationalId = x.NationalId,
                                               AccountId = x.AccountId,
                                               EducationalQualification = x.EducationalQualification,
                                               BirthDate = x.BirthDate,
                                               BirthHijriDate = x.BirthHijriDate,
                                               BirthPlace = x.BirthPlace,
                                               MaritalStatus = x.MaritalStatus,
                                               ChildrenNo = x.ChildrenNo,
                                               Gender = x.Gender,
                                               NationalityId = x.NationalityId,
                                               ReligionId = x.ReligionId,
                                               UserId = x.UserId,
                                               JobId = x.JobId,
                                               DepartmentId = x.DepartmentId,
                                               BranchId = x.BranchId,
                                               Telephone = x.Telephone,
                                               Mailbox = x.Mailbox,
                                               NationalIdSource = x.NationalIdSource,
                                               NationalIdDate = x.NationalIdDate,
                                               NationalIdHijriDate = x.NationalIdHijriDate,
                                               NationalIdEndDate = x.NationalIdEndDate,
                                               NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                               PassportNo = x.PassportNo,
                                               PassportSource = x.PassportSource,
                                               PassportNoDate = x.PassportNoDate,
                                               PassportNoHijriDate = x.PassportNoHijriDate,
                                               PassportEndDate = x.PassportEndDate,
                                               PassportEndHijriDate = x.PassportEndHijriDate,
                                               ContractNo = x.ContractNo,
                                               ContractSource = x.ContractSource,
                                               ContractStartDate = x.ContractStartDate,
                                               ContractStartHijriDate = x.ContractStartHijriDate,
                                               ContractEndDate = x.ContractEndDate,
                                               ContractEndHijriDate = x.ContractEndHijriDate,
                                               MedicalNo = x.MedicalNo,
                                               MedicalSource = x.MedicalSource,
                                               MedicalStartDate = x.MedicalStartDate,
                                               MedicalStartHijriDate = x.MedicalStartHijriDate,
                                               MedicalEndDate = x.MedicalEndDate,
                                               MedicalEndHijriDate = x.MedicalEndHijriDate,
                                               LicenceNo = x.LicenceNo,
                                               LiscenseSourceId = x.LiscenseSourceId,
                                               LicenceStartDate = x.LicenceStartDate,
                                               LicenceStartHijriDate = x.LicenceStartHijriDate,
                                               LicenceEndDate = x.LicenceEndDate,
                                               LicenceEndHijriDate = x.LicenceEndHijriDate,
                                               DawamId = x.DawamId,
                                               TimeDurationLate = x.TimeDurationLate,
                                               LogoutDuration = x.LogoutDuration,
                                               AfterLogoutTime = x.AfterLogoutTime,
                                               Salary = x.Salary ?? 0,
                                               Bonus = x.Bonus ?? 0,
                                               AddAllowances = x.Allowances ?? 0,
                                               TotalViolations = 0,
                                               VacationsCount = x.VacationsCount,
                                               VacationEndCount = x.VacationEndCount,
                                               WorkStartDate = x.WorkStartDate,
                                               EndWorkDate = x.EndWorkDate,

                                               WorkStartHijriDate = x.WorkStartHijriDate,
                                               NodeLocation = x.NodeLocations != null ? x.NodeLocations.Location : "",
                                               PhotoUrl = x.PhotoUrl,
                                               DepartmentName = x.Department != null ? x.Department.DepartmentNameAr : "",
                                               JobName = x.Job != null ? x.Job.JobNameAr : "",
                                               BankName = x.Bank != null ? x.Bank.NameAr : "",
                                               AcountName = x.Account != null ? x.Account.NameAr : "",
                                               AccountCode = x.Account != null ? x.Account.Code : "",
                                               AttendaceTimeName = x.AttendaceTime != null ? x.AttendaceTime.NameAr : "",
                                               GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                                               MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                                               ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                                               NationalityName = x.Nationality != null ? x.Nationality.NameAr : "",
                                               DeppID = x.DeppID,
                                               PostalCode = x.PostalCode,
                                               AddDate = x.AddDate,
                                               AddUser = x.AddUser,

                                               AccountIDs = x.AccountIDs,
                                               AccountIDs_Bouns = x.AccountIDs_Bouns,
                                               AccountIDs_Custody = x.AccountIDs_Custody,
                                               AccountIDs_Discount = x.AccountIDs_Discount,
                                               AccountIDs_Salary = x.AccountIDs_Salary,
                                               Taamen = x.Taamen,
                                               EarlyLogin = x.EarlyLogin,
                                               DirectManager = x.DirectManager,
                                               Allowances = x.Allowances ?? 0,
                                               OtherAllownces = x.OtherAllownces ?? 0,

                                               HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                                                  s.AllowanceType.NameEn == "Housing allowance" && !(s.AddDate.Value > DateTime.Now)).FirstOrDefault()).AllowanceAmount ?? 0,

                                               TotalLoans = (x.Loans.Where(s => s.IsDeleted == false && s.Status == 2).FirstOrDefault()
                                               .LoanDetails.Where(y => y.Date.Value.Month == SalarySearch.MonthNo)).Sum(s => s.Amount) ?? 0,

                                           }).ToList();


                    //استبعاد الموظفين المنهي خدماتهم
                    //!(int.Parse(s.WorkStartDate.Split('-')[1]) > MonthNo) && 
                    employees = employees.Where(s => (int.Parse(s.WorkStartDate.Split('-')[0]) <= DateTime.Now.Year) &&
                        (string.IsNullOrEmpty(s.EndWorkDate) || (!string.IsNullOrEmpty(s.EndWorkDate) && ((int.Parse(s.EndWorkDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.EndWorkDate.Split('-')[1]) >= SalarySearch.MonthNo) && (int.Parse(s.EndWorkDate.Split('-')[0]) == DateTime.Now.Year))))) &&
                        (string.IsNullOrEmpty(s.ContractEndDate) || (!string.IsNullOrEmpty(s.ContractEndDate) && ((int.Parse(s.ContractEndDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.ContractEndDate.Split('-')[1]) >= SalarySearch.MonthNo) && (int.Parse(s.ContractEndDate.Split('-')[0]) == DateTime.Now.Year)))))).ToList();

                    var EmpInVacactions = this.GetEmpsInVacations(SalarySearch.MonthNo);
                    // employees = employees.Where(x => !EmpInVacactions.Contains(x.EmployeeId)).ToList();

                    List<VacationVM> EmpVactions;
                    List<VacationVM> paidVactions;

                    DateTime StartDateOfMonth = new DateTime(DateTime.Now.Year, SalarySearch.MonthNo.Value, 1);
                    string startDate = Utilities.ConvertDateCalendar(StartDateOfMonth, "Gregorian", "en-US");

                    DateTime EndDateOfMonth = new DateTime(DateTime.Now.Year, SalarySearch.MonthNo.Value, DateTime.DaysInMonth(DateTime.Now.Year, SalarySearch.MonthNo.Value));
                    //EndDateOfMonth = DateTime.Now;
                    string endDate = Utilities.ConvertDateCalendar((EndDateOfMonth), "Gregorian", "en-US");

                    int day = DateTime.Now.Day;
                    foreach (var Emp in employees)
                    {
                        //If Employee started work at 20th of the month 
                        if (!string.IsNullOrEmpty(Emp.EndWorkDate) && int.Parse(Emp.EndWorkDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.EndWorkDate.Split('-')[1]) == SalarySearch.MonthNo)
                        {
                            EndDateOfMonth = DateTime.ParseExact(Emp.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        }
                        //else
                        //    EndDateOfMonth = DateTime.Now;


                        if (int.Parse(Emp.WorkStartDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.WorkStartDate.Split('-')[1]) == SalarySearch.MonthNo)
                        {
                            StartDateOfMonth = DateTime.ParseExact(Emp.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            //int DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, MonthNo);

                            int DaysInMonth = EndDateOfMonth.Subtract(StartDateOfMonth).Days + 1;
                            int RestDays = 30;//DaysInMonth; //- int.Parse(Emp.WorkStartDate.Split('-')[2]);
                            if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                            {
                                Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value + Emp.HousingAllowance.Value) / 30) * RestDays), 2);

                            }
                            else
                            {
                                Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value + Emp.Allowances.Value) / 30) * RestDays), 2);

                            }
                        }
                        else
                        {
                            if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                            {
                                Emp.ThisMonthSalary = Emp.Salary.Value + Emp.HousingAllowance.Value;

                            }
                            else
                            {
                                Emp.ThisMonthSalary = Emp.Salary.Value + Emp.Allowances.Value;

                            }

                        }

                        var monthallownc = (decimal)await _allowanceRepository.GetAllownacesSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth) + (decimal)await _allowanceRepository.GetAllownacesSumForPayroll2(Emp.EmployeeId, SalarySearch.MonthNo.Value);
                        Emp.MonthlyAllowances = monthallownc + Emp.OtherAllownces;
                        var totalrewrd = await _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 2) + await _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, SalarySearch.MonthNo.Value, 2);
                        Emp.TotalRewards = totalrewrd;
                        var toaoldiscount = await _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 1) + await _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, SalarySearch.MonthNo.Value, 1);

                        Emp.TotalDiscounts = toaoldiscount;
                        //Accepted Vactions
                        EmpVactions = _vacationRepository.GetAllVacationsBySearchObject(new VacationVM() { StartDate = startDate, EndDate = endDate, VacationStatus = 2, EmployeeId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0).Result.ToList();
                        paidVactions = EmpVactions.Where(x => x.IsDiscount.Value).ToList();
                        Emp.TotalPaidVacations = paidVactions.Sum(x => x.DiscountAmount.HasValue ? x.DiscountAmount.Value : 0);

                        var allloans = _TaamerProContext.Loan.Where(x => x.IsDeleted == false && x.Status == 2 && x.EmployeeId == Emp.EmployeeId).ToList();
                        decimal loanval = 0;
                        foreach (var lon in allloans)
                        {
                            var loandetail = _TaamerProContext.LoanDetails.Where(x => x.IsDeleted == false && x.LoanId == lon.LoanId).ToList();
                            if (loandetail != null && loandetail.Count() > 0)
                            {
                                if (lon.LoanDetails != null)
                                {
                                    loanval += loandetail.Where(y => y.Date.Value.Month == SalarySearch.MonthNo).Sum(x => x.Amount) ?? 0;
                                }
                            }
                        }
                        Emp.TotalLoans = loanval;
                        decimal converttamen = Convert.ToDecimal(Emp.Taamen);
                        decimal tmen = 0;

                        if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                        {
                            tmen = (converttamen * (Emp.Salary.Value + Emp.HousingAllowance.Value)) / 100;

                        }
                        else
                        {

                            tmen = (converttamen * (Emp.Salary.Value + Emp.Allowances.Value)) / 100;

                        }

                        decimal tamn = Convert.ToDecimal(tmen);
                        Emp.Taamen = tamn.ToString();

                        decimal totcaramount = 0;
                        var crmov = await _carMovementsRepository.GetAllCarMovementsSearchObject(new CarMovementsVM { EmpId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0);
                        if (crmov.Count() > 0)
                        {
                            foreach (var emcar in crmov)
                            {
                                if (int.Parse(emcar.Date.Split('-')[0]) == DateTime.Now.Year && int.Parse(emcar.Date.Split('-')[1]) == SalarySearch.MonthNo)
                                {
                                    totcaramount = totcaramount + emcar.EmpAmount ?? 0;
                                }

                            }
                        }
                        //Absent days excluded vaction days
                        var totlabs = await _attendenceRepository.GetAbsenceData(startDate, endDate, Emp.EmployeeId, DateTime.Now.Year, Emp.BranchId ?? 0, lang, Con);

                        Emp.TotalDayAbs = totlabs.Count();

                        if (SalarySearch.MonthNo < MonthNo)
                        {
                            Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary / 30) * 30) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                              (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                               ), 2);
                        }
                        else
                        {
                            Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary / 30) * day) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                          (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                           ), 2);
                        }

                    }

                    if (SalarySearch.IsAllBranch == true)
                    {
                        employees = employees.ToList();
                    }
                    else
                    {

                        employees = employees.Where(w => w.BranchId == SalarySearch.BranchId).ToList();
                    }

                    return employees;
                }
                else
                {
                    var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && !string.IsNullOrEmpty(s.WorkStartDate) && string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmployeesVM
                    {
                        EmployeeId = x.EmployeeId,
                        EmployeeNo = x.EmployeeNo,
                        EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                        EmployeeNameAr = x.EmployeeNameAr,
                        EmployeeNameEn = x.EmployeeNameEn,
                        Email = x.Email,
                        Mobile = x.Mobile,
                        Address = x.Address,
                        NationalId = x.NationalId,
                        AccountId = x.AccountId,
                        EducationalQualification = x.EducationalQualification,
                        BirthDate = x.BirthDate,
                        BirthHijriDate = x.BirthHijriDate,
                        BirthPlace = x.BirthPlace,
                        MaritalStatus = x.MaritalStatus,
                        ChildrenNo = x.ChildrenNo,
                        Gender = x.Gender,
                        NationalityId = x.NationalityId,
                        ReligionId = x.ReligionId,
                        UserId = x.UserId,
                        JobId = x.JobId,
                        DepartmentId = x.DepartmentId,
                        BranchId = x.BranchId,
                        Telephone = x.Telephone,
                        Mailbox = x.Mailbox,
                        NationalIdSource = x.NationalIdSource,
                        NationalIdDate = x.NationalIdDate,
                        NationalIdHijriDate = x.NationalIdHijriDate,
                        NationalIdEndDate = x.NationalIdEndDate,
                        NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                        PassportNo = x.PassportNo,
                        PassportSource = x.PassportSource,
                        PassportNoDate = x.PassportNoDate,
                        PassportNoHijriDate = x.PassportNoHijriDate,
                        PassportEndDate = x.PassportEndDate,
                        PassportEndHijriDate = x.PassportEndHijriDate,
                        ContractNo = x.ContractNo,
                        ContractSource = x.ContractSource,
                        ContractStartDate = x.ContractStartDate,
                        ContractStartHijriDate = x.ContractStartHijriDate,
                        ContractEndDate = x.ContractEndDate,
                        ContractEndHijriDate = x.ContractEndHijriDate,
                        MedicalNo = x.MedicalNo,
                        MedicalSource = x.MedicalSource,
                        MedicalStartDate = x.MedicalStartDate,
                        MedicalStartHijriDate = x.MedicalStartHijriDate,
                        MedicalEndDate = x.MedicalEndDate,
                        MedicalEndHijriDate = x.MedicalEndHijriDate,
                        LicenceNo = x.LicenceNo,
                        LiscenseSourceId = x.LiscenseSourceId,
                        LicenceStartDate = x.LicenceStartDate,
                        LicenceStartHijriDate = x.LicenceStartHijriDate,
                        LicenceEndDate = x.LicenceEndDate,
                        LicenceEndHijriDate = x.LicenceEndHijriDate,
                        DawamId = x.DawamId,
                        TimeDurationLate = x.TimeDurationLate,
                        LogoutDuration = x.LogoutDuration,
                        AfterLogoutTime = x.AfterLogoutTime,
                        Salary = x.Salary ?? 0,
                        Bonus = x.Bonus ?? 0,
                        AddAllowances = 0,
                        TotalViolations = 0,
                        VacationsCount = x.VacationsCount,
                        VacationEndCount = x.VacationEndCount,
                        WorkStartDate = x.WorkStartDate,
                        EndWorkDate = x.EndWorkDate,
                        WorkStartHijriDate = x.WorkStartHijriDate,
                        //WorkEndDate = x.WorkEndDate,
                        //WorkEndHijriDate = x.WorkEndHijriDate,
                        NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                        PhotoUrl = x.PhotoUrl,
                        DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                        JobName = x.Job!=null? x.Job.JobNameAr:"",
                        BankName = x.Bank!=null? x.Bank.NameAr:"",
                        AcountName = x.Account!=null? x.Account.NameAr:"",
                        AccountCode = x.Account!=null? x.Account.Code:"",
                        AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                        GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                        MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                        ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                        NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                        DeppID = x.DeppID,
                        PostalCode = x.PostalCode,
                        AddDate = x.AddDate,
                        AddUser = x.AddUser,
                        Allowances=x.Allowances??0,

                        AccountIDs = x.AccountIDs,
                        AccountIDs_Bouns = x.AccountIDs_Bouns,
                        AccountIDs_Custody = x.AccountIDs_Custody,
                        AccountIDs_Discount = x.AccountIDs_Discount,
                        AccountIDs_Salary = x.AccountIDs_Salary,
                        Taamen = x.Taamen,
                        EarlyLogin=x.EarlyLogin,
                        DirectManager = x.DirectManager,
                        OtherAllownces=x.OtherAllownces??0,

                        //CommunicationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                        //                      s.AllowanceType.NameEn == "Communications").FirstOrDefault()).AllowanceAmount ?? 0,

                        //ProfessionAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                        //                      s.AllowanceType.NameEn == "Profession").FirstOrDefault()).AllowanceAmount ?? 0,

                        //TransportationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                        //                      s.AllowanceType.NameEn == "Transportation").FirstOrDefault()).AllowanceAmount ?? 0,

                        HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                              s.AllowanceType.NameEn == "Housing allowance").FirstOrDefault()).AllowanceAmount ==null? x.Allowances??0 :
                                              (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                              s.AllowanceType.NameEn == "Housing allowance").FirstOrDefault()).AllowanceAmount??0,

                        MonthlyAllowances = (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0) + (x.OtherAllownces ??0),
                        ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && !s.IsFixed && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,

                        TotalLoans =(x.Loans.Where(s => s.IsDeleted == false && s.Status == 2).FirstOrDefault()
                                    .LoanDetails.Where(y => y.Date.Value.Year == DateTime.Now.Year)).Sum(s => s.Amount) ?? 0,

                        TotalDiscounts = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 1).Sum(s => s.Amount) ?? 0,
                        TotalRewards = x.DiscountRewards.Where(s => s.IsDeleted == false && s.Type == 2).Sum(s => s.Amount) ?? 0

                        //TotalySalaries = ((x.Salary ?? 0) + (x.Bonus ?? 0) + (x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true).Sum(s => s.AllowanceAmount) ?? 0) + (x.Loans.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0)) - (((x.Salary ?? 0) / 30) * (x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault()))
                        //   + (x.DiscountRewards.Where(s => s.IsDeleted == false).Sum(s => s.Amount) ?? 0),
                        //TotalDayAbs = x.AttAbsentDays.Where(w => w.EmpId == x.EmployeeId && w.Year == DateTime.Now.Year && w.Month == DateTime.Now.Month).Select(s => s.AbsDays).FirstOrDefault(),
                    }).ToList();
                    foreach (var Emp in employees)
                    {

                       var totalday = await _attendenceRepository.GetAbsenceData(DateTime.Now.Year +"-01-01", DateTime.Now.Year +"-12-31", Emp.EmployeeId, DateTime.Now.Year, Emp.BranchId ?? 0, lang, Con);
                        Emp.TotalDayAbs = totalday.Count();
                        //Emp.TotalySalaries = (decimal)Math.Round((Emp.Salary + Emp.Bonus + Emp.CommunicationAllawance + Emp.TransportationAllawance + Emp.ProfessionAllawance + Emp.HousingAllowance
                        //        + Emp.MonthlyAllowances ?? 0 + Emp.ExtraAllowances ?? 0) -
                        //        (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans ?? 0), 2);
                        decimal converttamen = Convert.ToDecimal(Emp.Taamen);
                        decimal tmen = 0;

                        if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                        {
                            tmen = (converttamen * (Emp.Salary.Value + Emp.HousingAllowance.Value)) / 100;

                        }
                        else
                        {

                            tmen = (converttamen * (Emp.Salary.Value + Emp.Allowances.Value)) / 100;

                        }
                        Emp.Taamen = tmen.ToString();

                        Emp.TotalySalaries = 0;
                    }
                    if (SalarySearch.IsAllBranch == true)
                    {
                        employees = employees.ToList();
                    }
                    else
                    {

                        employees = employees.Where(w => w.BranchId == SalarySearch.BranchId).ToList();
                    }

                    return employees;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<EmployeesVM>>  GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, string Con = "")
        {
            int MonthNo = DateTime.Now.Month;

            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false  &&
                                   !string.IsNullOrEmpty(s.WorkStartDate) && s.ContractStartDate != null).Select(x => new EmployeesVM
                                   {
                                       EmployeeId = x.EmployeeId,
                                       EmployeeNo = x.EmployeeNo,
                                       EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                       EmployeeNameAr = x.EmployeeNameAr,
                                       EmployeeNameEn = x.EmployeeNameEn,
                                       Email = x.Email,
                                       Mobile = x.Mobile,
                                       Address = x.Address,
                                       NationalId = x.NationalId,
                                       AccountId = x.AccountId,
                                       EducationalQualification = x.EducationalQualification,
                                       BirthDate = x.BirthDate,
                                       BirthHijriDate = x.BirthHijriDate,
                                       BirthPlace = x.BirthPlace,
                                       MaritalStatus = x.MaritalStatus,
                                       ChildrenNo = x.ChildrenNo,
                                       Gender = x.Gender,
                                       NationalityId = x.NationalityId,
                                       ReligionId = x.ReligionId,
                                       UserId = x.UserId,
                                       JobId = x.JobId,
                                       DepartmentId = x.DepartmentId,
                                       BranchId = x.BranchId,
                                       Telephone = x.Telephone,
                                       Mailbox = x.Mailbox,
                                       NationalIdSource = x.NationalIdSource,
                                       NationalIdDate = x.NationalIdDate,
                                       NationalIdHijriDate = x.NationalIdHijriDate,
                                       NationalIdEndDate = x.NationalIdEndDate,
                                       NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                       PassportNo = x.PassportNo,
                                       PassportSource = x.PassportSource,
                                       PassportNoDate = x.PassportNoDate,
                                       PassportNoHijriDate = x.PassportNoHijriDate,
                                       PassportEndDate = x.PassportEndDate,
                                       PassportEndHijriDate = x.PassportEndHijriDate,
                                       ContractNo = x.ContractNo,
                                       ContractSource = x.ContractSource,
                                       ContractStartDate = x.ContractStartDate,
                                       ContractStartHijriDate = x.ContractStartHijriDate,
                                       ContractEndDate = x.ContractEndDate,
                                       ContractEndHijriDate = x.ContractEndHijriDate,
                                       MedicalNo = x.MedicalNo,
                                       MedicalSource = x.MedicalSource,
                                       MedicalStartDate = x.MedicalStartDate,
                                       MedicalStartHijriDate = x.MedicalStartHijriDate,
                                       MedicalEndDate = x.MedicalEndDate,
                                       MedicalEndHijriDate = x.MedicalEndHijriDate,
                                       LicenceNo = x.LicenceNo,
                                       LiscenseSourceId = x.LiscenseSourceId,
                                       LicenceStartDate = x.LicenceStartDate,
                                       LicenceStartHijriDate = x.LicenceStartHijriDate,
                                       LicenceEndDate = x.LicenceEndDate,
                                       LicenceEndHijriDate = x.LicenceEndHijriDate,
                                       DawamId = x.DawamId,
                                       TimeDurationLate = x.TimeDurationLate,
                                       LogoutDuration = x.LogoutDuration,
                                       AfterLogoutTime = x.AfterLogoutTime,
                                       Salary = x.Salary ?? 0,
                                       Bonus = x.Bonus ?? 0,
                                       AddAllowances = x.Allowances??0,
                                       TotalViolations = 0,
                                       VacationsCount = x.VacationsCount,
                                       VacationEndCount = x.VacationEndCount,
                                       WorkStartDate = x.WorkStartDate,
                                       EndWorkDate = x.EndWorkDate,

                                       WorkStartHijriDate = x.WorkStartHijriDate,
                                    NodeLocation = x.NodeLocations!=null? x.NodeLocations.Location:"",
                                       PhotoUrl = x.PhotoUrl,
                                       DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                                       JobName = x.Job!=null? x.Job.JobNameAr:"",
                                       BankName = x.Bank!=null? x.Bank.NameAr:"",
                                       AcountName = x.Account!=null? x.Account.NameAr:"",
                                       AccountCode = x.Account!=null? x.Account.Code:"",
                                       AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                                       GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                                       MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                                       ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                                       NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                                       DeppID = x.DeppID,
                                       PostalCode = x.PostalCode,
                                       AddDate = x.AddDate,
                                       AddUser = x.AddUser,

                                       AccountIDs = x.AccountIDs,
                                       AccountIDs_Bouns = x.AccountIDs_Bouns,
                                       AccountIDs_Custody = x.AccountIDs_Custody,
                                       AccountIDs_Discount = x.AccountIDs_Discount,
                                       AccountIDs_Salary = x.AccountIDs_Salary,
                                       Taamen = x.Taamen,
                                       EarlyLogin=x.EarlyLogin,
                                       DirectManager = x.DirectManager,
                                       Allowances=x.Allowances??0,
                                       OtherAllownces=x.OtherAllownces??0,

                                       HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                                          s.AllowanceType.NameEn == "Housing allowance" && !(s.AddDate.Value > DateTime.Now)).FirstOrDefault()).AllowanceAmount ?? 0,

                                       TotalLoans = (x.Loans.Where(s => s.IsDeleted == false && s.Status == 2).FirstOrDefault()
                                       .LoanDetails.Where(y => y.Date.Value.Month == MonthNo)).Sum(s => s.Amount) ?? 0,

                                   }).ToList();


            //استبعاد الموظفين المنهي خدماتهم
            //!(int.Parse(s.WorkStartDate.Split('-')[1]) > MonthNo) && 
            employees = employees.Where(s => (int.Parse(s.WorkStartDate.Split('-')[0]) <= DateTime.Now.Year) &&
                (string.IsNullOrEmpty(s.EndWorkDate) || (!string.IsNullOrEmpty(s.EndWorkDate) && ( (int.Parse(s.EndWorkDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.EndWorkDate.Split('-')[1]) >= MonthNo) && (int.Parse(s.EndWorkDate.Split('-')[0]) == DateTime.Now.Year)))))&&
                (string.IsNullOrEmpty(s.ContractEndDate) || (!string.IsNullOrEmpty(s.ContractEndDate) &&( (int.Parse(s.ContractEndDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.ContractEndDate.Split('-')[1]) >= MonthNo) && (int.Parse(s.ContractEndDate.Split('-')[0]) == DateTime.Now.Year)))))).ToList();

            var EmpInVacactions = this.GetEmpsInVacations(MonthNo);
           // employees = employees.Where(x => !EmpInVacactions.Contains(x.EmployeeId)).ToList();

            List<VacationVM> EmpVactions;
            List<VacationVM> paidVactions;

            DateTime StartDateOfMonth = new DateTime(DateTime.Now.Year, MonthNo, 1);
            string startDate = Utilities.ConvertDateCalendar(StartDateOfMonth, "Gregorian", "en-US");

            DateTime EndDateOfMonth = new DateTime(DateTime.Now.Year, MonthNo, DateTime.DaysInMonth(DateTime.Now.Year, MonthNo));
            //EndDateOfMonth = DateTime.Now;
            string endDate = Utilities.ConvertDateCalendar((EndDateOfMonth), "Gregorian", "en-US");

            int day = DateTime.Now.Day;
            foreach (var Emp in employees)
            {
                //If Employee started work at 20th of the month 
                if (!string.IsNullOrEmpty(Emp.EndWorkDate) && int.Parse(Emp.EndWorkDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.EndWorkDate.Split('-')[1]) == MonthNo)
                {
                    EndDateOfMonth = DateTime.ParseExact(Emp.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                //else
                //    EndDateOfMonth = DateTime.Now;


                if (int.Parse(Emp.WorkStartDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.WorkStartDate.Split('-')[1]) == MonthNo)
                {
                    StartDateOfMonth = DateTime.ParseExact(Emp.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    //int DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, MonthNo);

                    int DaysInMonth = EndDateOfMonth.Subtract(StartDateOfMonth).Days + 1;
                    int RestDays = 30;//DaysInMonth; //- int.Parse(Emp.WorkStartDate.Split('-')[2]);
                    if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                    {
                        Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value + Emp.HousingAllowance.Value) / 30) * RestDays), 2);

                    }
                    else
                    {
                        Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value + Emp.Allowances.Value) / 30) * RestDays), 2);

                    }
                }
                else
                {
                    if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value >0)
                    {
                        Emp.ThisMonthSalary = Emp.Salary.Value + Emp.HousingAllowance.Value ;

                    }
                    else
                    {
                        Emp.ThisMonthSalary = Emp.Salary.Value + Emp.Allowances.Value;

                    }

                }

                var monthallownc =(decimal) await _allowanceRepository.GetAllownacesSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth) + (decimal)await _allowanceRepository.GetAllownacesSumForPayroll2(Emp.EmployeeId, MonthNo);
                Emp.MonthlyAllowances = monthallownc + Emp.OtherAllownces ;
                var totalrewrd= await _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 2) +  await _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, MonthNo, 2);
                Emp.TotalRewards = totalrewrd;
                var toaoldiscount= await _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 1) + await _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, MonthNo, 1);

                Emp.TotalDiscounts = toaoldiscount;
                //Accepted Vactions
                EmpVactions = _vacationRepository.GetAllVacationsBySearchObject(new VacationVM() { StartDate = startDate, EndDate = endDate, VacationStatus = 2, EmployeeId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0).Result.ToList();
                paidVactions = EmpVactions.Where(x => x.IsDiscount.Value).ToList();
                Emp.TotalPaidVacations = paidVactions.Sum(x => x.DiscountAmount.HasValue ? x.DiscountAmount.Value : 0);

                var allloans = _TaamerProContext.Loan.Where(x => x.IsDeleted == false && x.Status == 2 && x.EmployeeId == Emp.EmployeeId).ToList();
                decimal loanval =0;
                foreach (var lon in allloans)
                {
                    var loandetail = _TaamerProContext.LoanDetails.Where(x => x.IsDeleted == false && x.LoanId == lon.LoanId).ToList();
                    if (loandetail != null && loandetail.Count() > 0)
                    {
                        if (lon.LoanDetails != null)
                        {
                            loanval += loandetail.Where(y => y.Date.Value.Month == MonthNo).Sum(x => x.Amount) ?? 0;
                        }
                    }
                }
                Emp.TotalLoans = loanval;
                decimal converttamen = Convert.ToDecimal(Emp.Taamen);
                decimal tmen = 0;

                if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                {
                     tmen = (converttamen * (Emp.Salary.Value + Emp.HousingAllowance.Value)) / 100;

                }
                else
                {
                 
                    tmen = (converttamen * (Emp.Salary.Value + Emp.Allowances.Value)) / 100;

                }

                decimal tamn = Convert.ToDecimal(tmen);
                Emp.Taamen = tamn.ToString();

                decimal totcaramount = 0;
                var crmov = await _carMovementsRepository.GetAllCarMovementsSearchObject(new CarMovementsVM { EmpId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0);
                if (crmov.Count() > 0)
                {
                    foreach (var emcar in crmov)
                    {
                        if (int.Parse(emcar.Date.Split('-')[0]) == DateTime.Now.Year && int.Parse(emcar.Date.Split('-')[1]) == MonthNo)
                        {
                            totcaramount = totcaramount + emcar.EmpAmount ?? 0;
                        }

                    }
                }
                //Absent days excluded vaction days
                var totlabs= await _attendenceRepository.GetAbsenceData(startDate, endDate, Emp.EmployeeId, DateTime.Now.Year, Emp.BranchId ?? 0, lang, Con);

                Emp.TotalDayAbs = totlabs.Count();
                
                Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary /30) * day) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                        (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                         ), 2);
            }
            if (IsAllBranch == true)
            {
                employees = employees.ToList();
            }
            else
            {

                employees = employees.Where(w => w.BranchId == BranchId).ToList();
            }
            return employees;
        }

        //overloading on salary if past month
        public async Task<IEnumerable<EmployeesVM>> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId,int Monthno, string Con = "")
        {
            int MonthNo = DateTime.Now.Month;

            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false &&
                                   !string.IsNullOrEmpty(s.WorkStartDate) && s.ContractStartDate != null).Select(x => new EmployeesVM
                                   {
                                       EmployeeId = x.EmployeeId,
                                       EmployeeNo = x.EmployeeNo,
                                       EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                       EmployeeNameAr = x.EmployeeNameAr,
                                       EmployeeNameEn = x.EmployeeNameEn,
                                       Email = x.Email,
                                       Mobile = x.Mobile,
                                       Address = x.Address,
                                       NationalId = x.NationalId,
                                       AccountId = x.AccountId,
                                       EducationalQualification = x.EducationalQualification,
                                       BirthDate = x.BirthDate,
                                       BirthHijriDate = x.BirthHijriDate,
                                       BirthPlace = x.BirthPlace,
                                       MaritalStatus = x.MaritalStatus,
                                       ChildrenNo = x.ChildrenNo,
                                       Gender = x.Gender,
                                       NationalityId = x.NationalityId,
                                       ReligionId = x.ReligionId,
                                       UserId = x.UserId,
                                       JobId = x.JobId,
                                       DepartmentId = x.DepartmentId,
                                       BranchId = x.BranchId,
                                       Telephone = x.Telephone,
                                       Mailbox = x.Mailbox,
                                       NationalIdSource = x.NationalIdSource,
                                       NationalIdDate = x.NationalIdDate,
                                       NationalIdHijriDate = x.NationalIdHijriDate,
                                       NationalIdEndDate = x.NationalIdEndDate,
                                       NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                       PassportNo = x.PassportNo,
                                       PassportSource = x.PassportSource,
                                       PassportNoDate = x.PassportNoDate,
                                       PassportNoHijriDate = x.PassportNoHijriDate,
                                       PassportEndDate = x.PassportEndDate,
                                       PassportEndHijriDate = x.PassportEndHijriDate,
                                       ContractNo = x.ContractNo,
                                       ContractSource = x.ContractSource,
                                       ContractStartDate = x.ContractStartDate,
                                       ContractStartHijriDate = x.ContractStartHijriDate,
                                       ContractEndDate = x.ContractEndDate,
                                       ContractEndHijriDate = x.ContractEndHijriDate,
                                       MedicalNo = x.MedicalNo,
                                       MedicalSource = x.MedicalSource,
                                       MedicalStartDate = x.MedicalStartDate,
                                       MedicalStartHijriDate = x.MedicalStartHijriDate,
                                       MedicalEndDate = x.MedicalEndDate,
                                       MedicalEndHijriDate = x.MedicalEndHijriDate,
                                       LicenceNo = x.LicenceNo,
                                       LiscenseSourceId = x.LiscenseSourceId,
                                       LicenceStartDate = x.LicenceStartDate,
                                       LicenceStartHijriDate = x.LicenceStartHijriDate,
                                       LicenceEndDate = x.LicenceEndDate,
                                       LicenceEndHijriDate = x.LicenceEndHijriDate,
                                       DawamId = x.DawamId,
                                       TimeDurationLate = x.TimeDurationLate,
                                       LogoutDuration = x.LogoutDuration,
                                       AfterLogoutTime = x.AfterLogoutTime,
                                       Salary = x.Salary ?? 0,
                                       Bonus = x.Bonus ?? 0,
                                       AddAllowances = x.Allowances ?? 0,
                                       TotalViolations = 0,
                                       VacationsCount = x.VacationsCount,
                                       VacationEndCount = x.VacationEndCount,
                                       WorkStartDate = x.WorkStartDate,
                                       EndWorkDate = x.EndWorkDate,

                                       WorkStartHijriDate = x.WorkStartHijriDate,
                                       NodeLocation = x.NodeLocations != null ? x.NodeLocations.Location : "",
                                       PhotoUrl = x.PhotoUrl,
                                       DepartmentName = x.Department != null ? x.Department.DepartmentNameAr : "",
                                       JobName = x.Job != null ? x.Job.JobNameAr : "",
                                       BankName = x.Bank != null ? x.Bank.NameAr : "",
                                       AcountName = x.Account != null ? x.Account.NameAr : "",
                                       AccountCode = x.Account != null ? x.Account.Code : "",
                                       AttendaceTimeName = x.AttendaceTime != null ? x.AttendaceTime.NameAr : "",
                                       GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                                       MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                                       ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                                       NationalityName = x.Nationality != null ? x.Nationality.NameAr : "",
                                       DeppID = x.DeppID,
                                       PostalCode = x.PostalCode,
                                       AddDate = x.AddDate,
                                       AddUser = x.AddUser,

                                       AccountIDs = x.AccountIDs,
                                       AccountIDs_Bouns = x.AccountIDs_Bouns,
                                       AccountIDs_Custody = x.AccountIDs_Custody,
                                       AccountIDs_Discount = x.AccountIDs_Discount,
                                       AccountIDs_Salary = x.AccountIDs_Salary,
                                       Taamen = x.Taamen,
                                       EarlyLogin = x.EarlyLogin,
                                       DirectManager = x.DirectManager,
                                       Allowances = x.Allowances ?? 0,
                                       OtherAllownces = x.OtherAllownces ?? 0,

                                       HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                                          s.AllowanceType.NameEn == "Housing allowance" && !(s.AddDate.Value > DateTime.Now)).FirstOrDefault()).AllowanceAmount ?? 0,

                                       TotalLoans = (x.Loans.Where(s => s.IsDeleted == false && s.Status == 2).FirstOrDefault()
                                       .LoanDetails.Where(y => y.Date.Value.Month == Monthno)).Sum(s => s.Amount) ?? 0,

                                   }).ToList();


            //استبعاد الموظفين المنهي خدماتهم
            //!(int.Parse(s.WorkStartDate.Split('-')[1]) > MonthNo) && 
            employees = employees.Where(s => (int.Parse(s.WorkStartDate.Split('-')[0]) <= DateTime.Now.Year) &&
                (string.IsNullOrEmpty(s.EndWorkDate) || (!string.IsNullOrEmpty(s.EndWorkDate) && ((int.Parse(s.EndWorkDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.EndWorkDate.Split('-')[1]) >= Monthno) && (int.Parse(s.EndWorkDate.Split('-')[0]) == DateTime.Now.Year))))) &&
                (string.IsNullOrEmpty(s.ContractEndDate) || (!string.IsNullOrEmpty(s.ContractEndDate) && ((int.Parse(s.ContractEndDate.Split('-')[0]) > DateTime.Now.Year) || ((int.Parse(s.ContractEndDate.Split('-')[1]) >= Monthno) && (int.Parse(s.ContractEndDate.Split('-')[0]) == DateTime.Now.Year)))))).ToList();

            var EmpInVacactions = this.GetEmpsInVacations(Monthno);
            // employees = employees.Where(x => !EmpInVacactions.Contains(x.EmployeeId)).ToList();

            List<VacationVM> EmpVactions;
            List<VacationVM> paidVactions;

            DateTime StartDateOfMonth = new DateTime(DateTime.Now.Year, Monthno, 1);
            string startDate = Utilities.ConvertDateCalendar(StartDateOfMonth, "Gregorian", "en-US");

            DateTime EndDateOfMonth = new DateTime(DateTime.Now.Year, Monthno, DateTime.DaysInMonth(DateTime.Now.Year, Monthno));
            //EndDateOfMonth = DateTime.Now;
            string endDate = Utilities.ConvertDateCalendar((EndDateOfMonth), "Gregorian", "en-US");

            int day = DateTime.Now.Day;
            foreach (var Emp in employees)
            {
                //If Employee started work at 20th of the month 
                if (!string.IsNullOrEmpty(Emp.EndWorkDate) && int.Parse(Emp.EndWorkDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.EndWorkDate.Split('-')[1]) == Monthno)
                {
                    EndDateOfMonth = DateTime.ParseExact(Emp.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                //else
                //    EndDateOfMonth = DateTime.Now;


                if (int.Parse(Emp.WorkStartDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.WorkStartDate.Split('-')[1]) == Monthno)
                {
                    StartDateOfMonth = DateTime.ParseExact(Emp.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    //int DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, MonthNo);

                    int DaysInMonth = EndDateOfMonth.Subtract(StartDateOfMonth).Days + 1;
                    int RestDays = 30;//DaysInMonth; //- int.Parse(Emp.WorkStartDate.Split('-')[2]);
                    if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                    {
                        Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value) / 30) * RestDays), 2);
                        Emp.HousingAllowance = Emp.HousingAllowance.Value;
                    }
                    else
                    {
                        Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value) / 30) * RestDays), 2);
                        Emp.HousingAllowance = Emp.Allowances.Value;

                    }
                }
                else
                {
                    if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                    {
                        Emp.ThisMonthSalary = Emp.Salary.Value ;
                        Emp.HousingAllowance = Emp.HousingAllowance.Value;
                    }
                    else
                    {
                        Emp.ThisMonthSalary = Emp.Salary.Value;
                        Emp.HousingAllowance = Emp.Allowances.Value;
                    }

                }

                var monthallownc = (decimal)await _allowanceRepository.GetAllownacesSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth) + (decimal)await _allowanceRepository.GetAllownacesSumForPayroll2(Emp.EmployeeId, Monthno);
                Emp.MonthlyAllowances = monthallownc + Emp.OtherAllownces;
                var totalrewrd = await _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 2) + await _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, Monthno, 2);
                Emp.TotalRewards = totalrewrd;
                var toaoldiscount = await _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 1) + await _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, Monthno, 1);

                Emp.TotalDiscounts = toaoldiscount;
                //Accepted Vactions
                EmpVactions = _vacationRepository.GetAllVacationsBySearchObject(new VacationVM() { StartDate = startDate, EndDate = endDate, VacationStatus = 2, EmployeeId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0).Result.ToList();
                paidVactions = EmpVactions.Where(x => x.IsDiscount.Value).ToList();
                Emp.TotalPaidVacations = paidVactions.Sum(x => x.DiscountAmount.HasValue ? x.DiscountAmount.Value : 0);

                var allloans = _TaamerProContext.Loan.Where(x => x.IsDeleted == false && x.Status == 2 && x.EmployeeId == Emp.EmployeeId).ToList();
                decimal loanval = 0;
                foreach (var lon in allloans)
                {
                    var loandetail = _TaamerProContext.LoanDetails.Where(x => x.IsDeleted == false && x.LoanId == lon.LoanId).ToList();
                    if (loandetail != null && loandetail.Count() > 0)
                    {
                        if (lon.LoanDetails != null)
                        {
                            loanval += loandetail.Where(y => y.Date.Value.Month == Monthno).Sum(x => x.Amount) ?? 0;
                        }
                    }
                }
                Emp.TotalLoans = loanval;
                decimal converttamen = Convert.ToDecimal(Emp.Taamen);
                decimal tmen = 0;

                if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                {
                    tmen = (converttamen * (Emp.Salary.Value + Emp.HousingAllowance.Value)) / 100;

                }
                else
                {

                    tmen = (converttamen * (Emp.Salary.Value + Emp.Allowances.Value)) / 100;

                }

                decimal tamn = Convert.ToDecimal(tmen);
                Emp.Taamen = tamn.ToString();

                decimal totcaramount = 0;
                var crmov = await _carMovementsRepository.GetAllCarMovementsSearchObject(new CarMovementsVM { EmpId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0);
                if (crmov.Count() > 0)
                {
                    foreach (var emcar in crmov)
                    {
                        if (int.Parse(emcar.Date.Split('-')[0]) == DateTime.Now.Year && int.Parse(emcar.Date.Split('-')[1]) == Monthno)
                        {
                            totcaramount = totcaramount + emcar.EmpAmount ?? 0;
                        }

                    }
                }
                //Absent days excluded vaction days
                var totlabs = await _attendenceRepository.GetAbsenceData(startDate, endDate, Emp.EmployeeId, DateTime.Now.Year, Emp.BranchId ?? 0, lang, Con);

                Emp.TotalDayAbs = totlabs.Count();

                if(!string.IsNullOrEmpty(Emp.EndWorkDate) && int.Parse(Emp.EndWorkDate.Split('-')[0]) == DateTime.Now.Year && int.Parse(Emp.EndWorkDate.Split('-')[1]) == Monthno)
                {
                    int daynum = int.Parse(Emp.EndWorkDate.Split('-')[2]);

                    if (Monthno < MonthNo)
                    {
                        Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary / 30) * 30) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                          (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                           ), 2);
                    }
                    else
                    {
                        Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary / 30) * daynum) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                      (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                       ), 2);
                    }
                }
                else
                {

                    if (Monthno < MonthNo)
                    {
                        Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary / 30) * 30) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                          (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                           ), 2);
                    }
                    else
                    {
                        Emp.TotalySalaries = Math.Round((decimal)((((Emp.ThisMonthSalary / 30) * day) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                      (((Emp.Salary / 30) * Emp.TotalDayAbs) + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations)
                       ), 2);
                    }
                }


              
            }
            if (IsAllBranch == true)
            {
                employees = employees.ToList();
            }
            else
            {

                employees = employees.Where(w => w.BranchId == BranchId).ToList();
            }
            return employees;
        }



        public IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno,int YearId, string Con = "")
        {
            int MonthNo = DateTime.Now.Month;

            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false &&
                                   !string.IsNullOrEmpty(s.WorkStartDate) && s.ContractStartDate != null).Select(x => new EmployeesVM
                                   {
                                       EmployeeId = x.EmployeeId,
                                       EmployeeNo = x.EmployeeNo,
                                       EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                       EmployeeNameAr = x.EmployeeNameAr,
                                       EmployeeNameEn = x.EmployeeNameEn,
                                       Email = x.Email,
                                       Mobile = x.Mobile,
                                       Address = x.Address,
                                       NationalId = x.NationalId,
                                       AccountId = x.AccountId,
                                       EducationalQualification = x.EducationalQualification,
                                       BirthDate = x.BirthDate,
                                       BirthHijriDate = x.BirthHijriDate,
                                       BirthPlace = x.BirthPlace,
                                       MaritalStatus = x.MaritalStatus,
                                       ChildrenNo = x.ChildrenNo,
                                       Gender = x.Gender,
                                       NationalityId = x.NationalityId,
                                       ReligionId = x.ReligionId,
                                       UserId = x.UserId,
                                       JobId = x.JobId,
                                       DepartmentId = x.DepartmentId,
                                       BranchId = x.BranchId,
                                       Telephone = x.Telephone,
                                       Mailbox = x.Mailbox,
                                       NationalIdSource = x.NationalIdSource,
                                       NationalIdDate = x.NationalIdDate,
                                       NationalIdHijriDate = x.NationalIdHijriDate,
                                       NationalIdEndDate = x.NationalIdEndDate,
                                       NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                       PassportNo = x.PassportNo,
                                       PassportSource = x.PassportSource,
                                       PassportNoDate = x.PassportNoDate,
                                       PassportNoHijriDate = x.PassportNoHijriDate,
                                       PassportEndDate = x.PassportEndDate,
                                       PassportEndHijriDate = x.PassportEndHijriDate,
                                       ContractNo = x.ContractNo,
                                       ContractSource = x.ContractSource,
                                       ContractStartDate = x.ContractStartDate,
                                       ContractStartHijriDate = x.ContractStartHijriDate,
                                       ContractEndDate = x.ContractEndDate,
                                       ContractEndHijriDate = x.ContractEndHijriDate,
                                       MedicalNo = x.MedicalNo,
                                       MedicalSource = x.MedicalSource,
                                       MedicalStartDate = x.MedicalStartDate,
                                       MedicalStartHijriDate = x.MedicalStartHijriDate,
                                       MedicalEndDate = x.MedicalEndDate,
                                       MedicalEndHijriDate = x.MedicalEndHijriDate,
                                       LicenceNo = x.LicenceNo,
                                       LiscenseSourceId = x.LiscenseSourceId,
                                       LicenceStartDate = x.LicenceStartDate,
                                       LicenceStartHijriDate = x.LicenceStartHijriDate,
                                       LicenceEndDate = x.LicenceEndDate,
                                       LicenceEndHijriDate = x.LicenceEndHijriDate,
                                       DawamId = x.DawamId,
                                       TimeDurationLate = x.TimeDurationLate,
                                       LogoutDuration = x.LogoutDuration,
                                       AfterLogoutTime = x.AfterLogoutTime,
                                       Salary = x.Salary ?? 0,
                                       Bonus = x.Bonus ?? 0,
                                       AddAllowances = x.Allowances ?? 0,
                                       TotalViolations = 0,
                                       VacationsCount = x.VacationsCount,
                                       VacationEndCount = x.VacationEndCount,
                                       WorkStartDate = x.WorkStartDate,
                                       EndWorkDate = x.EndWorkDate,

                                       WorkStartHijriDate = x.WorkStartHijriDate,
                                       NodeLocation = x.NodeLocations != null ? x.NodeLocations.Location : "",
                                       PhotoUrl = x.PhotoUrl,
                                       DepartmentName = x.Department != null ? x.Department.DepartmentNameAr : "",
                                       JobName = x.Job != null ? x.Job.JobNameAr : "",
                                       BankName = x.Bank != null ? x.Bank.NameAr : "",
                                       AcountName = x.Account != null ? x.Account.NameAr : "",
                                       AccountCode = x.Account != null ? x.Account.Code : "",
                                       AttendaceTimeName = x.AttendaceTime != null ? x.AttendaceTime.NameAr : "",
                                       GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                                       MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                                       ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                                       NationalityName = x.Nationality != null ? x.Nationality.NameAr : "",
                                       DeppID = x.DeppID,
                                       PostalCode = x.PostalCode,
                                       AddDate = x.AddDate,
                                       AddUser = x.AddUser,

                                       AccountIDs = x.AccountIDs,
                                       AccountIDs_Bouns = x.AccountIDs_Bouns,
                                       AccountIDs_Custody = x.AccountIDs_Custody,
                                       AccountIDs_Discount = x.AccountIDs_Discount,
                                       AccountIDs_Salary = x.AccountIDs_Salary,
                                       Taamen = x.Taamen,
                                       EarlyLogin = x.EarlyLogin,
                                       DirectManager = x.DirectManager,
                                       Allowances = x.Allowances ?? 0,
                                       OtherAllownces = x.OtherAllownces ?? 0,

                                       HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                                          s.AllowanceType.NameEn == "Housing allowance" && !(s.AddDate.Value > DateTime.Now)).FirstOrDefault()).AllowanceAmount ?? 0,

                                       TotalLoans = (x.Loans.Where(s => s.IsDeleted == false && s.Status == 2).FirstOrDefault()
                                       .LoanDetails.Where(y => y.Date.Value.Month == Monthno)).Sum(s => s.Amount) ?? 0,

                                   }).ToList();


            //استبعاد الموظفين المنهي خدماتهم
            //!(int.Parse(s.WorkStartDate.Split('-')[1]) > MonthNo) && 
            employees = employees.Where(s => (int.Parse(s.WorkStartDate.Split('-')[0]) <= YearId) &&
            (!string.IsNullOrEmpty(s.WorkStartDate) && ((int.Parse(s.WorkStartDate.Split('-')[0]) < YearId) || ((int.Parse(s.WorkStartDate.Split('-')[1]) <= Monthno) && (int.Parse(s.WorkStartDate.Split('-')[0]) == YearId)))) &&
                        (!string.IsNullOrEmpty(s.ContractStartDate) && ((int.Parse(s.ContractStartDate.Split('-')[0]) < YearId) || ((int.Parse(s.ContractStartDate.Split('-')[1]) <= Monthno) && (int.Parse(s.ContractStartDate.Split('-')[0]) == YearId)))) &&

                (string.IsNullOrEmpty(s.EndWorkDate) || (!string.IsNullOrEmpty(s.EndWorkDate) && ((int.Parse(s.EndWorkDate.Split('-')[0]) > YearId) || ((int.Parse(s.EndWorkDate.Split('-')[1]) >= Monthno) && (int.Parse(s.EndWorkDate.Split('-')[0]) == YearId))))) 
                &&(string.IsNullOrEmpty(s.ContractEndDate) || (!string.IsNullOrEmpty(s.ContractEndDate) && ((int.Parse(s.ContractEndDate.Split('-')[0]) > YearId) || ((int.Parse(s.ContractEndDate.Split('-')[1]) >= Monthno) && (int.Parse(s.ContractEndDate.Split('-')[0]) == YearId)))))
                ).ToList();

            var EmpInVacactions = this.GetEmpsInVacations(Monthno);
            // employees = employees.Where(x => !EmpInVacactions.Contains(x.EmployeeId)).ToList();

            List<VacationVM> EmpVactions;
            List<VacationVM> paidVactions;

            DateTime StartDateOfMonth = new DateTime(YearId, Monthno, 1);
            string startDate = Utilities.ConvertDateCalendar(StartDateOfMonth, "Gregorian", "en-US");

            DateTime EndDateOfMonth = new DateTime(YearId, Monthno, DateTime.DaysInMonth(YearId, Monthno));
            //EndDateOfMonth = DateTime.Now;
            string endDate = Utilities.ConvertDateCalendar((EndDateOfMonth), "Gregorian", "en-US");

            int day = DateTime.Now.Day;
            foreach (var Emp in employees)
            {
                //If Employee started work at 20th of the month 
                if (!string.IsNullOrEmpty(Emp.EndWorkDate) && int.Parse(Emp.EndWorkDate.Split('-')[0]) == YearId && int.Parse(Emp.EndWorkDate.Split('-')[1]) == Monthno)
                {
                    EndDateOfMonth = DateTime.ParseExact(Emp.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                //else
                //    EndDateOfMonth = DateTime.Now;


                if (int.Parse(Emp.WorkStartDate.Split('-')[0]) == YearId && int.Parse(Emp.WorkStartDate.Split('-')[1]) == Monthno)
                {
                    StartDateOfMonth = DateTime.ParseExact(Emp.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    //int DaysInMonth = DateTime.DaysInMonth(YearId, MonthNo);

                    int DaysInMonth = EndDateOfMonth.Subtract(StartDateOfMonth).Days + 1;
                    int RestDays = 30;//DaysInMonth; //- int.Parse(Emp.WorkStartDate.Split('-')[2]);
                    if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                    {
                        Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value ) / 30) * RestDays), 2);
                        Emp.HousingAllowance = Emp.HousingAllowance.Value;

                    }
                    else
                    {
                        Emp.ThisMonthSalary = Math.Round((decimal)(((Emp.Salary.Value) / 30) * RestDays), 2);
                        Emp.HousingAllowance = Emp.Allowances.Value;
                    }
                }
                else
                {
                    if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                    {
                        Emp.ThisMonthSalary = Emp.Salary.Value;
                        Emp.HousingAllowance = Emp.HousingAllowance.Value;
                    }
                    else
                    {
                        Emp.ThisMonthSalary = Emp.Salary.Value;
                        Emp.HousingAllowance = Emp.Allowances.Value;

                    }

                }

                var monthallownc = (decimal) _allowanceRepository.GetAllownacesSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth).Result + (decimal) _allowanceRepository.GetAllownacesSumForPayroll2(Emp.EmployeeId, Monthno).Result;
                Emp.MonthlyAllowances = monthallownc + Emp.OtherAllownces;
                var totalrewrd =  _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 2).Result +  _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, Monthno, 2).Result;
                Emp.TotalRewards = totalrewrd;
                var toaoldiscount =  _discountRewardRepository.GetDiscountRewordSumForPayroll(Emp.EmployeeId, StartDateOfMonth, EndDateOfMonth, 1).Result +  _discountRewardRepository.GetDiscountRewordSumForPayroll2(Emp.EmployeeId, Monthno, 1).Result;

                Emp.TotalDiscounts = toaoldiscount;
                //Accepted Vactions
                EmpVactions = _vacationRepository.GetAllVacationsBySearchObject(new VacationVM() { StartDate = startDate, EndDate = endDate, VacationStatus = 2, EmployeeId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0).Result.ToList();
                paidVactions = EmpVactions.Where(x => x.IsDiscount.Value).ToList();
                Emp.TotalPaidVacations = paidVactions.Sum(x => x.DiscountAmount.HasValue ? x.DiscountAmount.Value : 0);

                var allloans = _TaamerProContext.Loan.Where(x => x.IsDeleted == false && x.Status == 2 && x.EmployeeId == Emp.EmployeeId).ToList();
                decimal loanval = 0;
                foreach (var lon in allloans)
                {
                    var loandetail = _TaamerProContext.LoanDetails.Where(x => x.IsDeleted == false && x.LoanId == lon.LoanId).ToList();
                    if (loandetail != null && loandetail.Count() > 0)
                    {
                        if (lon.LoanDetails != null)
                        {
                            loanval += loandetail.Where(y => y.Date.Value.Month == Monthno).Sum(x => x.Amount) ?? 0;
                        }
                    }
                }
                Emp.TotalLoans = loanval;
                decimal converttamen = Convert.ToDecimal(Emp.Taamen);
                decimal tmen = 0;

                if (Emp.HousingAllowance.Value != null && Emp.HousingAllowance.Value > 0)
                {
                    tmen = (converttamen * (Emp.Salary.Value + Emp.HousingAllowance.Value)) / 100;

                }
                else
                {

                    tmen = (converttamen * (Emp.Salary.Value + Emp.Allowances.Value)) / 100;

                }

                decimal tamn = Convert.ToDecimal(tmen);
                Emp.Taamen = tamn.ToString();

                decimal totcaramount = 0;
                var crmov =  _carMovementsRepository.GetAllCarMovementsSearchObject(new CarMovementsVM { EmpId = Emp.EmployeeId }, Emp.BranchId.HasValue ? Emp.BranchId.Value : 0).Result;
                if (crmov.Count() > 0)
                {
                    foreach (var emcar in crmov)
                    {
                        if (int.Parse(emcar.Date.Split('-')[0]) == YearId && int.Parse(emcar.Date.Split('-')[1]) == Monthno)
                        {
                            totcaramount = totcaramount + emcar.EmpAmount ?? 0;
                        }

                    }
                }
                //Absent days excluded vaction days
                var totlabs =  _attendenceRepository.GetAbsenceData_withWeekEnd(startDate, endDate, Emp.EmployeeId, YearId, 0, lang, Con).Result;
                var dawam = _TaamerProContext.AttTimeDetails.Where(x => x.AttTimeId == Emp.DawamId).ToList().Select(x => x.Day);

                var totalabsencediscount = CalculateAbsenceDiscounts(totlabs.ToList(), Emp.Salary.Value /30 ,Emp.DawamId);

                Emp.TotalDayAbs = totlabs.Count(a => dawam.Contains(int.Parse(a.DayNOfWeek))); ;// totlabs.Count();
                Emp.TotalAbsenceDiscount = totalabsencediscount;
                var latedata = _attendenceRepository.GetLateData(startDate, endDate, Emp.EmployeeId, YearId,0, 0, lang, Con);
                Emp.TotalLateDiscount = latedata.Result.Sum(x => x.Discount1) + latedata.Result.Sum(x => x.Discount2);
                if (!string.IsNullOrEmpty(Emp.EndWorkDate) && int.Parse(Emp.EndWorkDate.Split('-')[0]) == YearId && int.Parse(Emp.EndWorkDate.Split('-')[1]) == Monthno)
                {
                    int daynum = int.Parse(Emp.EndWorkDate.Split('-')[2]);

                    if (Monthno < MonthNo)
                    {
                        Emp.TotalySalaries = Math.Round((decimal)(((((Emp.ThisMonthSalary+ Emp.HousingAllowance) / 30) * 30) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                          (totalabsencediscount + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations + latedata.Result.Sum(x=>x.Discount1) + latedata.Result.Sum(x=>x.Discount2))
                           ), 2);
                    }
                    else
                    {
                        Emp.TotalySalaries = Math.Round((decimal)(((((Emp.ThisMonthSalary + Emp.HousingAllowance )/ 30) * daynum) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                      (totalabsencediscount + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations + latedata.Result.Sum(x => x.Discount1) + latedata.Result.Sum(x => x.Discount2))
                       ), 2);
                    }
                }
                else
                {

                    if (Monthno < MonthNo)
                    {
                        Emp.TotalySalaries = Math.Round((decimal)(((((Emp.ThisMonthSalary + Emp.HousingAllowance )/ 30) * 30) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                          (totalabsencediscount + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations + latedata.Result.Sum(x => x.Discount1) + latedata.Result.Sum(x => x.Discount2))
                           ), 2);
                    }
                    else
                    {
                        Emp.TotalySalaries = Math.Round((decimal)(((((Emp.ThisMonthSalary + Emp.HousingAllowance )/ 30) * day) + Emp.MonthlyAllowances.Value + Emp.Bonus + Emp.TotalRewards.Value) -
                      (totalabsencediscount + Emp.TotalLoans.Value + tamn + totcaramount + Emp.TotalDiscounts.Value + Emp.TotalPaidVacations + latedata.Result.Sum(x => x.Discount1) + latedata.Result.Sum(x => x.Discount2))
                       ), 2);
                    }
                }



            }
            if (IsAllBranch == true)
            {
                employees = employees.ToList();
            }
            else
            {

                employees = employees.Where(w => w.BranchId == BranchId).ToList();
            }
            return employees;
        }
        public async Task<IEnumerable<EmployeesVM>>  GetAllUsersEmployees()
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate)&& !string.IsNullOrEmpty(s.WorkStartDate)).Select(x => new EmployeesVM
            {
                UserId = x.UserId,
            }).ToList();
            return employees;
        }

        private decimal CalculateAbsenceDiscounts(List<AbsenceVM> absences ,decimal salary,int? dawamId)
        {
            if (dawamId == null || dawamId == 0)
            {
                return 0;
            }
            else { 
                decimal total = 0;


                //discount variables 
                int Between1And3 = 0;
                int Between4And6 = 0;
                int Between7And10 = 0;
                int GreaterThan10 = 0;
                var groupedAbsences = absences.GroupBy(a => a.EmpNo)
                                                .Select(g => new
                                                {
                                                    EmpNo = g.Key,
                                                    Absences = g.OrderBy(a => a.Mdate).ToList()
                                                }).ToList();
                var dawam = _TaamerProContext.AttTimeDetails.Where(x => x.AttTimeId == dawamId).ToList().Select(x => x.Day);

                //foreach (var group in groupedAbsences)
                foreach (var group in groupedAbsences)
                {
                    int consecutiveDays = 0;
                    DateTime? previousDate = null;

                    foreach (var absence in group.Absences)
                    {
                        var currentDate = DateTime.ParseExact(absence.Mdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                        Console.WriteLine($"Current Date: {currentDate}, Previous Date: {previousDate}");

                        if (previousDate.HasValue && (currentDate - previousDate.Value).Days == 1)
                        {
                            if (!dawam.Contains(Convert.ToInt32(absence.DayNOfWeek)))
                            {
                                previousDate = currentDate;
                                continue;
                            }

                            consecutiveDays++;
                        }
                        else if (!dawam.Contains(Convert.ToInt32(absence.DayNOfWeek)))
                        {
                            continue;
                        }
                        else
                        {
                            if (consecutiveDays > 0)
                            {
                                total += CalculateDiscount(consecutiveDays, salary, ref Between1And3, ref Between4And6, ref Between7And10, ref GreaterThan10);
                            }

                            consecutiveDays = 1;
                        }

                        previousDate = currentDate;
                    }

                    if (consecutiveDays > 0)
                    {
                        total += CalculateDiscount(consecutiveDays, salary, ref Between1And3, ref Between4And6, ref Between7And10, ref GreaterThan10);
                    }
                }

                //    int consecutiveDays = 0;
                //    DateTime? previousDate = null;


                //    foreach (var absence in group.Absences)
                //    {
                //        var currentDate = DateTime.ParseExact(absence.Mdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);// Convert.ToDateTime(absence.Mdate);

                //        if (previousDate.HasValue && (currentDate - previousDate.Value).Days == 1)
                //        {
                //            if (previousDate.HasValue && !dawam.Contains(Convert.ToInt32(absence.DayNOfWeek)))
                //            {
                //                previousDate = currentDate;
                //            }
                //            else
                //            {
                //                consecutiveDays++;
                //               // total += CalculateDiscount(consecutiveDays,salary,ref Between1And3,ref Between4And6,ref Between7And10,ref GreaterThan10);
                //                previousDate = currentDate;
                //            }
                //        }
                //        //else if (previousDate.HasValue && !dawam.Contains(dayofweek))
                //        //{
                //        //    previousDate = currentDate;
                //        //}
                //        else if(!previousDate.HasValue && !dawam.Contains(Convert.ToInt32(absence.DayNOfWeek)))
                //        {
                //            continue;
                //        }
                //        else
                //        {
                //            if(consecutiveDays > 0)
                //            {
                //                total += CalculateDiscount(consecutiveDays, salary, ref Between1And3, ref Between4And6, ref Between7And10, ref GreaterThan10);
                //            }

                //            consecutiveDays = 1;
                //           // total += CalculateDiscount(consecutiveDays,salary,ref Between1And3,ref Between4And6,ref Between7And10,ref GreaterThan10);
                //            previousDate = currentDate;
                //        }


                //    }

                //    if (consecutiveDays > 0)
                //    {
                //        total += CalculateDiscount(consecutiveDays, salary, ref Between1And3, ref Between4And6, ref Between7And10, ref GreaterThan10);
                //    }
                //}
                return total;
            }
        }


        public decimal CalculateDiscount(int absent, decimal salary, ref int Between1And3, ref int Between4And6, ref int Between7And10, ref int GreaterThan10)
        {
            
            if (absent >=1  && absent <=3)
            {
                return CalculateAbsenceDiscount(salary,absent, (int)AbsencePeriod.Between1And3, ref Between1And3);
            }
            else if (absent >= 4 && absent < 7)
            {
                return CalculateAbsenceDiscount(salary, absent, (int)AbsencePeriod.Between4And6, ref Between4And6);
            }
            else if (absent >= 7 && absent <= 10)
            {
                return CalculateAbsenceDiscount(salary, absent, (int)AbsencePeriod.Between7And10, ref Between7And10);
            }
            else if (absent > 10)
            {
                return CalculateAbsenceDiscount(salary, absent, (int)AbsencePeriod.GreaterThan10, ref GreaterThan10);
            }

            return 0m;
        }

        private decimal CalculateAbsenceDiscount(decimal salary,int daynum ,int type,ref int count)
        {


            //count++;
            //var id = (count > 1 && count <=3) ? 1 : (count >= 4 && count <= 6) ? 2 : (count >= 7 && count <= 10) ? 3 : (count >10) ? 4 :1;
            //var absencerules = _TaamerProContext.AbsenceLists.FirstOrDefault(x => x.ID == id);
            //return id switch
            //{


            count++;
            var id = count > 4 ? 4 : count;
            var absencerules = _TaamerProContext.AbsenceLists.FirstOrDefault(x => x.ID == type);
            return id switch
            {
                (int)AbsencePeriod.Between1And3 => (daynum * salary ) + (absencerules.First.Value   * salary),
                (int)AbsencePeriod.Between4And6 => (daynum * salary) + (absencerules.Second.Value   * salary),
                (int)AbsencePeriod.Between7And10 => (daynum * salary) + (absencerules.Third.Value  * salary),
                (int)AbsencePeriod.GreaterThan10 => (daynum * salary) + (absencerules.Fourth.Value   * salary),
                _ => 0m
            };
        }

        //public decimal calculateabsencediscount(decimal salary, int consecutivedays)
        //{
        //    var id = (consecutivedays > 1 && consecutivedays <= 3) ? 1 : (consecutivedays >= 4 && consecutivedays <= 6) ? 2 : (consecutivedays >= 7 && consecutivedays <= 10) ? 3 : (consecutivedays > 10) ? 4 : 1;
        //    var absencerules = _TaamerProContext.AbsenceLists.FirstOrDefault(x => x.ID == id);
        //    if (consecutivedays >= 1 && consecutivedays <= 3)
        //    {
        //        return consecutivedays switch
        //        {
        //            (int)AbsencePeriod.Between1And3 => (absencerules.First.Value / 100) * salary,
        //            (int)AbsencePeriod.Between4And6 => (absencerules.Second.Value / 100) * salary,
        //            (int)AbsencePeriod.Between7And10 => (absencerules.Third.Value / 100) * salary,
        //            (int)AbsencePeriod.GreaterThan10 => (absencerules.Fourth.Value / 100) * salary,
        //            _ => 0m
        //        };
        //    }
        //    else if (consecutivedays >= 4 && consecutivedays <= 6)
        //    {
        //        return consecutivedays switch
        //        {
        //            (int)AbsencePeriod.Between1And3 => (absencerules.First.Value / 100) * salary,
        //            (int)AbsencePeriod.Between4And6 => (absencerules.Second.Value / 100) * salary,
        //            (int)AbsencePeriod.Between7And10 => (absencerules.Third.Value / 100) * salary,
        //            (int)AbsencePeriod.GreaterThan10 => (absencerules.Fourth.Value / 100) * salary,
        //            _ => 0m
        //        };
        //    }
        //    else if (consecutivedays >= 7 && consecutivedays <= 10)
        //    {
        //        return consecutivedays switch
        //        {
        //            (int)AbsencePeriod.Between1And3 => (absencerules.First.Value / 100) * salary,
        //            (int)AbsencePeriod.Between4And6 => (absencerules.Second.Value / 100) * salary,
        //            (int)AbsencePeriod.Between7And10 => (absencerules.Third.Value / 100) * salary,
        //            (int)AbsencePeriod.GreaterThan10 => (absencerules.Fourth.Value / 100) * salary,
        //            _ => 0m
        //        };
        //    }
        //    else if (consecutivedays > 10)
        //    {
        //        return consecutivedays switch
        //        {
        //            (int)AbsencePeriod.Between1And3 => (absencerules.First.Value / 100) * salary,
        //            (int)AbsencePeriod.Between4And6 => (absencerules.Second.Value / 100) * salary,
        //            (int)AbsencePeriod.Between7And10 => (absencerules.Third.Value / 100) * salary,
        //            (int)AbsencePeriod.GreaterThan10 => (absencerules.Fourth.Value / 100) * salary,
        //            _ => 0m
        //        };
        //    }

        //    return 0m;
        //}

        public async Task<EmployeesVM> GetEmployeeById(int EmployeeId, string lang)
        {
            var emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == EmployeeId).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                JobId = x.JobId,
                ReligionId = x.ReligionId,
                DepartmentId = x.DepartmentId,
                NationalityId = x.NationalityId,
                BankId = x.BankId,
                Gender = x.Gender,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                PassportNo = x.PassportNo,
                PhotoUrl = x.PhotoUrl,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                Salary = x.Salary,
                Discount = x.Discount,
                Bonus = x.Bonus,
                Rewards = x.Rewards,
                Allowances = x.Allowances,
                Loan = x.Loan,
                BeginWorkDate = x.BeginWorkDate,
                BeginWorkHijriDate = x.BeginWorkHijriDate,
                EndWorkHijriDate = x.EndWorkHijriDate,
                BankCardNo = x.BankCardNo,
                BankDate = x.BankDate,
                BankHijriDate = x.BankHijriDate,
                HaveLicence = x.HaveLicence,
                LicenceNo = x.LicenceNo,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                LicenceSource = x.LicenceSource,
                LiscenseSourceId = x.LiscenseSourceId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ChildrenNo = x.ChildrenNo,
                MaritalStatus = x.MaritalStatus,
                Active = x.Active,
                UsrId = x.UsrId,
                NationalIdSource = x.NationalIdSource,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportSource = x.PassportSource,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractSource = x.ContractSource,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                WorkNo = x.WorkNo,
                WorkStartDate = x.WorkStartDate,
                VacationEndCount = x.VacationEndCount,
                EndWorkDate = x.EndWorkDate,
                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                WorkSource = x.WorkSource,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                MedNo = x.MedNo,
                MedDNo = x.MedDNo,
                MedSource = x.MedSource,
                MedDSource = x.MedDSource,
                MedDate = x.MedDate,
                MedHijriDate = x.MedHijriDate,
                MedDDate = x.MedDDate,
                MedDHijriDate = x.MedDHijriDate,
                MedEndDate = x.MedEndDate,
                MedEndHijriDate = x.MedEndHijriDate,
                MedDEndDate = x.MedDEndDate,
                MedDEndHijriDate = x.MedDEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                VacationsCount = x.VacationsCount,
                CitySourceId = x.CitySourceId,
                NationalIdEndCount = x.NationalIdEndCount,
                PassportEndCount = x.PassportEndCount,
                ContractEndCount = x.ContractEndCount,
                LicenceCarEndCount = x.LicenceCarEndCount,
                MedicalEndCount = x.MedicalEndCount,
                LoanCount = x.LoanCount,
                DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName = x.Job!=null? x.Job.JobNameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                AccountIDs=x.AccountIDs,
                AddDate = x.AddDate,
                AddUser = x.AddUser,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                Taamen = x.Taamen,
                EarlyLogin=x.EarlyLogin,
                DirectManager = x.DirectManager,
                Age=x.Age,
                OtherAllownces=x.OtherAllownces,
                AttendenceLocationId = x.AttendenceLocationId,
                allowoutsidesite = x.allowoutsidesite ?? false,
                allowallsite = x.allowallsite ?? false,
                EmpHourlyCost = x.EmpHourlyCost ?? 0,
                DailyWorkinghours = x.DailyWorkinghours ?? 0,
                //CommunicationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                       s.AllowanceType.NameEn == "Communications").FirstOrDefault()).AllowanceAmount ?? 0,

                //ProfessionAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                      s.AllowanceType.NameEn == "Profession").FirstOrDefault()).AllowanceAmount ?? 0,

                //TransportationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                      s.AllowanceType.NameEn == "Transportation").FirstOrDefault()).AllowanceAmount ?? 0,

                HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                      s.AllowanceType.NameEn == "Housing allowance").FirstOrDefault()).AllowanceAmount ?? 0,

                MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false  && !s.IsFixed && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0

            }).First();
            return emp;
        }

        public async Task< EmployeesVM> GetEmployeeById_d(int EmployeeId, string lang)
        {
            var emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == EmployeeId && s.IsDeleted==false).Select(x => new EmployeesVM
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                EmployeeNameAr = x.EmployeeNameAr,
                EmployeeNameEn = x.EmployeeNameEn,
                JobId = x.JobId,
                ReligionId = x.ReligionId,
                DepartmentId = x.DepartmentId,
                NationalityId = x.NationalityId,
                BankId = x.BankId,
                Gender = x.Gender,
                Email = x.Email,
                Mobile = x.Mobile,
                Address = x.Address,
                NationalId = x.NationalId,
                PassportNo = x.PassportNo,
                PhotoUrl = x.PhotoUrl,
                NationalIdDate = x.NationalIdDate,
                NationalIdHijriDate = x.NationalIdHijriDate,
                PassportNoDate = x.PassportNoDate,
                PassportNoHijriDate = x.PassportNoHijriDate,
                Salary = x.Salary,
                Discount = x.Discount,
                Bonus = x.Bonus,
                Rewards = x.Rewards,
                Allowances = x.Allowances,
                Loan = x.Loan,
                BeginWorkDate = x.BeginWorkDate,
                BeginWorkHijriDate = x.BeginWorkHijriDate,
                EndWorkHijriDate = x.EndWorkHijriDate,
                BankCardNo = x.BankCardNo,
                BankDate = x.BankDate,
                BankHijriDate = x.BankHijriDate,
                HaveLicence = x.HaveLicence,
                LicenceNo = x.LicenceNo,
                LicenceStartDate = x.LicenceStartDate,
                LicenceStartHijriDate = x.LicenceStartHijriDate,
                LicenceEndDate = x.LicenceEndDate,
                LicenceEndHijriDate = x.LicenceEndHijriDate,
                LicenceSource = x.LicenceSource,
                LiscenseSourceId = x.LiscenseSourceId,
                AccountId = x.AccountId,
                EducationalQualification = x.EducationalQualification,
                BirthDate = x.BirthDate,
                BirthHijriDate = x.BirthHijriDate,
                BirthPlace = x.BirthPlace,
                Telephone = x.Telephone,
                Mailbox = x.Mailbox,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ChildrenNo = x.ChildrenNo,
                MaritalStatus = x.MaritalStatus,
                Active = x.Active,
                UsrId = x.UsrId,
                NationalIdSource = x.NationalIdSource,
                NationalIdEndDate = x.NationalIdEndDate,
                NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                PassportSource = x.PassportSource,
                PassportEndDate = x.PassportEndDate,
                PassportEndHijriDate = x.PassportEndHijriDate,
                ContractNo = x.ContractNo,
                ContractStartDate = x.ContractStartDate,
                ContractStartHijriDate = x.ContractStartHijriDate,
                ContractSource = x.ContractSource,
                ContractEndDate = x.ContractEndDate,
                ContractEndHijriDate = x.ContractEndHijriDate,
                WorkNo = x.WorkNo,
                WorkStartDate = x.WorkStartDate,
                VacationEndCount = x.VacationEndCount,
                EndWorkDate = x.EndWorkDate,
                WorkStartHijriDate = x.WorkStartHijriDate,
                //WorkEndDate = x.WorkEndDate,
                //WorkEndHijriDate = x.WorkEndHijriDate,
                WorkSource = x.WorkSource,
                MedicalNo = x.MedicalNo,
                MedicalSource = x.MedicalSource,
                MedicalStartDate = x.MedicalStartDate,
                MedicalStartHijriDate = x.MedicalStartHijriDate,
                MedicalEndDate = x.MedicalEndDate,
                MedicalEndHijriDate = x.MedicalEndHijriDate,
                MedNo = x.MedNo,
                MedDNo = x.MedDNo,
                MedSource = x.MedSource,
                MedDSource = x.MedDSource,
                MedDate = x.MedDate,
                MedHijriDate = x.MedHijriDate,
                MedDDate = x.MedDDate,
                MedDHijriDate = x.MedDHijriDate,
                MedEndDate = x.MedEndDate,
                MedEndHijriDate = x.MedEndHijriDate,
                MedDEndDate = x.MedDEndDate,
                MedDEndHijriDate = x.MedDEndHijriDate,
                DawamId = x.DawamId,
                TimeDurationLate = x.TimeDurationLate,
                LogoutDuration = x.LogoutDuration,
                AfterLogoutTime = x.AfterLogoutTime,
                DeppID = x.DeppID,
                PostalCode = x.PostalCode,
                VacationsCount = x.VacationsCount,
                CitySourceId = x.CitySourceId,
                NationalIdEndCount = x.NationalIdEndCount,
                PassportEndCount = x.PassportEndCount,
                ContractEndCount = x.ContractEndCount,
                LicenceCarEndCount = x.LicenceCarEndCount,
                MedicalEndCount = x.MedicalEndCount,
                LoanCount = x.LoanCount,
                DepartmentName = x.Department!=null? x.Department.DepartmentNameAr:"",
                JobName = x.Job!=null? x.Job.JobNameAr:"",
                AcountName = x.Account!=null? x.Account.NameAr:"",
                AccountCode = x.Account!=null? x.Account.Code:"",
                GenderName = x.Gender == 1 ? "ذكر" : "انثي",
                MaritalStatusName = x.MaritalStatus == 1 ? "اعزب" : "متزوج",
                ReligionStatusName = x.ReligionId == 1 ? "مسلم" : "مسيحي",
                NationalityName = x.Nationality!=null? x.Nationality.NameAr:"",
                AttendaceTimeName = x.AttendaceTime!=null? x.AttendaceTime.NameAr:"",
                AccountIDs = x.AccountIDs,
                AddDate = x.AddDate,
                AddUser = x.AddUser,
                AccountIDs_Bouns = x.AccountIDs_Bouns,
                AccountIDs_Custody = x.AccountIDs_Custody,
                AccountIDs_Discount = x.AccountIDs_Discount,
                AccountIDs_Salary = x.AccountIDs_Salary,
                Taamen = x.Taamen,
                EarlyLogin = x.EarlyLogin,
                DirectManager = x.DirectManager,

                //CommunicationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                       s.AllowanceType.NameEn == "Communications").FirstOrDefault()).AllowanceAmount ?? 0,

                //ProfessionAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                      s.AllowanceType.NameEn == "Profession").FirstOrDefault()).AllowanceAmount ?? 0,

                //TransportationAllawance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                //                      s.AllowanceType.NameEn == "Transportation").FirstOrDefault()).AllowanceAmount ?? 0,

                HousingAllowance = (x.Allowance.Where(s => s.IsDeleted == false && s.AllowanceType.IsSalaryPart.Value == true &&
                                      s.AllowanceType.NameEn == "Housing allowance").FirstOrDefault()).AllowanceAmount ?? 0,

                MonthlyAllowances = x.Allowance.Where(s => s.IsDeleted == false && s.IsFixed == true && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                ExtraAllowances = x.Allowance.Where(s => s.IsDeleted == false && !s.IsFixed && s.AllowanceType.IsSalaryPart == false).Sum(s => s.AllowanceAmount) ?? 0,
                      EmpHourlyCost = x.EmpHourlyCost ?? 0,
                DailyWorkinghours = x.DailyWorkinghours ?? 0,
            }).FirstOrDefault();
            return emp;
        }
        public async Task<IEnumerable<EmployeesVM>>  SearchEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate) && !string.IsNullOrEmpty(s.WorkStartDate) && s.BranchId == BranchId &&
             (s.EmployeeNameAr == EmployeesSearch.EmployeeNameAr || s.EmployeeNameAr.Contains(EmployeesSearch.EmployeeNameAr) || EmployeesSearch.EmployeeNameAr == null) &&
                                                 (s.NationalId == EmployeesSearch.NationalId || s.NationalId.Contains(EmployeesSearch.NationalId) || EmployeesSearch.NationalId == null) &&
                                                     (s.Mobile == EmployeesSearch.Mobile || s.Mobile.Contains(EmployeesSearch.Mobile) || EmployeesSearch.Mobile == null) &&
                                                     (s.PassportNo == EmployeesSearch.PassportNo || s.PassportNo.Contains(EmployeesSearch.PassportNo) || EmployeesSearch.PassportNo == null) &&
                                                     (s.Email == EmployeesSearch.Email || s.Email.Contains(EmployeesSearch.Email) || EmployeesSearch.Email == null) &&
                                                     (s.EmployeeNo == EmployeesSearch.EmployeeNo || s.EmployeeNo.Contains(EmployeesSearch.EmployeeNo) || EmployeesSearch.EmployeeNo == null) )
                                                .Select(x => new EmployeesVM
                                                {
                                                    EmployeeId = x.EmployeeId,
                                                    EmployeeNo = x.EmployeeNo,
                                                    EmployeeNameAr = x.EmployeeNameAr,
                                                    EmployeeNameEn = x.EmployeeNameEn,
                                                    EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                                    Email = x.Email,
                                                    Mobile = x.Mobile,
                                                    Address = x.Address,
                                                    NationalId = x.NationalId,
                                                    AccountId = x.AccountId,
                                                    EducationalQualification = x.EducationalQualification,
                                                    BirthDate = x.BirthDate,
                                                    BirthHijriDate = x.BirthHijriDate,
                                                    BirthPlace = x.BirthPlace,
                                                    MaritalStatus = x.MaritalStatus,
                                                    ChildrenNo = x.ChildrenNo,
                                                    Gender = x.Gender,
                                                    NationalityId = x.NationalityId,
                                                    ReligionId = x.ReligionId,
                                                    UserId = x.UserId,
                                                    JobId = x.JobId,
                                                    DepartmentId = x.DepartmentId,
                                                    BranchId = x.BranchId,
                                                    Telephone = x.Telephone,
                                                    Mailbox = x.Mailbox,
                                                    NationalIdSource = x.NationalIdSource,
                                                    NationalIdDate = x.NationalIdDate,
                                                    NationalIdHijriDate = x.NationalIdHijriDate,
                                                    NationalIdEndDate = x.NationalIdEndDate,
                                                    NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                                    PassportNo = x.PassportNo,
                                                    PassportSource = x.PassportSource,
                                                    PassportNoDate = x.PassportNoDate,
                                                    PassportNoHijriDate = x.PassportNoHijriDate,
                                                    PassportEndDate = x.PassportEndDate,
                                                    PassportEndHijriDate = x.PassportEndHijriDate,
                                                    ContractNo = x.ContractNo,
                                                    ContractSource = x.ContractSource,
                                                    ContractStartDate = x.ContractStartDate,
                                                    ContractStartHijriDate = x.ContractStartHijriDate,
                                                    ContractEndDate = x.ContractEndDate,
                                                    ContractEndHijriDate = x.ContractEndHijriDate,
                                                    MedicalNo = x.MedicalNo,
                                                    MedicalSource = x.MedicalSource,
                                                    MedicalStartDate = x.MedicalStartDate,
                                                    MedicalStartHijriDate = x.MedicalStartHijriDate,
                                                    MedicalEndDate = x.MedicalEndDate,
                                                    MedicalEndHijriDate = x.MedicalEndHijriDate,
                                                    LicenceNo = x.LicenceNo,
                                                    LiscenseSourceId = x.LiscenseSourceId,
                                                    LicenceStartDate = x.LicenceStartDate,
                                                    LicenceStartHijriDate = x.LicenceStartHijriDate,
                                                    LicenceEndDate = x.LicenceEndDate,
                                                    LicenceEndHijriDate = x.LicenceEndHijriDate,
                                                    DawamId = x.DawamId,
                                                    TimeDurationLate = x.TimeDurationLate,
                                                    LogoutDuration = x.LogoutDuration,
                                                    AfterLogoutTime = x.AfterLogoutTime,
                                                    Salary = x.Salary,
                                                    Bonus = x.Bonus,
                                                    VacationsCount = x.VacationsCount,
                                                    VacationEndCount = x.VacationEndCount,
                                                    WorkStartDate = x.WorkStartDate,
                                                    EndWorkDate = x.EndWorkDate,
                                                    WorkStartHijriDate = x.WorkStartHijriDate,
                                                    //WorkEndDate = x.WorkEndDate,
                                                    //WorkEndHijriDate = x.WorkEndHijriDate,
                                                    DeppID = x.DeppID,
                                                    BranchName = x.Branch.NameAr,
                                                    PostalCode = x.PostalCode,
                                                    AccountIDs = x.AccountIDs,
                                                    AccountIDs_Bouns = x.AccountIDs_Bouns,
                                                    AccountIDs_Custody = x.AccountIDs_Custody,
                                                    AccountIDs_Discount = x.AccountIDs_Discount,
                                                    AccountIDs_Salary = x.AccountIDs_Salary,
                                                    Taamen = x.Taamen,
                                                    EarlyLogin=x.EarlyLogin,
                                                    JobName=x.Job!=null? x.Job.JobNameAr:"",
                                                    NationalityName=x.Nationality!=null? x.Nationality.NameAr:"",
                                                }).ToList();
            return Employees;
        }


        public async Task<IEnumerable<EmployeesVM>>  SearchArchiveEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId)
        {
            var Employees = _TaamerProContext.Employees.Where(s => (!string.IsNullOrEmpty(s.EndWorkDate) ) && s.BranchId == BranchId &&
             (s.EmployeeNameAr == EmployeesSearch.EmployeeNameAr || s.EmployeeNameAr.Contains(EmployeesSearch.EmployeeNameAr) || EmployeesSearch.EmployeeNameAr == null) &&
                                                 (s.NationalId == EmployeesSearch.NationalId || s.NationalId.Contains(EmployeesSearch.NationalId) || EmployeesSearch.NationalId == null) &&
                                                     (s.Mobile == EmployeesSearch.Mobile || s.Mobile.Contains(EmployeesSearch.Mobile) || EmployeesSearch.Mobile == null) &&
                                                     (s.PassportNo == EmployeesSearch.PassportNo || s.PassportNo.Contains(EmployeesSearch.PassportNo) || EmployeesSearch.PassportNo == null) &&
                                                     (s.Email == EmployeesSearch.Email || s.Email.Contains(EmployeesSearch.Email) || EmployeesSearch.Email == null) &&
                                                     (s.EmployeeNo == EmployeesSearch.EmployeeNo || s.EmployeeNo.Contains(EmployeesSearch.EmployeeNo) || EmployeesSearch.EmployeeNo == null))
                                                .Select(x => new EmployeesVM
                                                {
                                                    EmployeeId = x.EmployeeId,
                                                    EmployeeNo = x.EmployeeNo,
                                                    EmployeeNameAr = x.EmployeeNameAr,
                                                    EmployeeNameEn = x.EmployeeNameEn,
                                                    EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                                    Email = x.Email,
                                                    Mobile = x.Mobile,
                                                    Address = x.Address,
                                                    NationalId = x.NationalId,
                                                    AccountId = x.AccountId,
                                                    EducationalQualification = x.EducationalQualification,
                                                    BirthDate = x.BirthDate,
                                                    BirthHijriDate = x.BirthHijriDate,
                                                    BirthPlace = x.BirthPlace,
                                                    MaritalStatus = x.MaritalStatus,
                                                    ChildrenNo = x.ChildrenNo,
                                                    Gender = x.Gender,
                                                    NationalityId = x.NationalityId,
                                                    ReligionId = x.ReligionId,
                                                    UserId = x.UserId,
                                                    JobId = x.JobId,
                                                    DepartmentId = x.DepartmentId,
                                                    BranchId = x.BranchId,
                                                    Telephone = x.Telephone,
                                                    Mailbox = x.Mailbox,
                                                    NationalIdSource = x.NationalIdSource,
                                                    NationalIdDate = x.NationalIdDate,
                                                    NationalIdHijriDate = x.NationalIdHijriDate,
                                                    NationalIdEndDate = x.NationalIdEndDate,
                                                    NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                                    PassportNo = x.PassportNo,
                                                    PassportSource = x.PassportSource,
                                                    PassportNoDate = x.PassportNoDate,
                                                    PassportNoHijriDate = x.PassportNoHijriDate,
                                                    PassportEndDate = x.PassportEndDate,
                                                    PassportEndHijriDate = x.PassportEndHijriDate,
                                                    ContractNo = x.ContractNo,
                                                    ContractSource = x.ContractSource,
                                                    ContractStartDate = x.ContractStartDate,
                                                    ContractStartHijriDate = x.ContractStartHijriDate,
                                                    ContractEndDate = x.ContractEndDate,
                                                    ContractEndHijriDate = x.ContractEndHijriDate,
                                                    MedicalNo = x.MedicalNo,
                                                    MedicalSource = x.MedicalSource,
                                                    MedicalStartDate = x.MedicalStartDate,
                                                    MedicalStartHijriDate = x.MedicalStartHijriDate,
                                                    MedicalEndDate = x.MedicalEndDate,
                                                    MedicalEndHijriDate = x.MedicalEndHijriDate,
                                                    LicenceNo = x.LicenceNo,
                                                    LiscenseSourceId = x.LiscenseSourceId,
                                                    LicenceStartDate = x.LicenceStartDate,
                                                    LicenceStartHijriDate = x.LicenceStartHijriDate,
                                                    LicenceEndDate = x.LicenceEndDate,
                                                    LicenceEndHijriDate = x.LicenceEndHijriDate,
                                                    DawamId = x.DawamId,
                                                    TimeDurationLate = x.TimeDurationLate,
                                                    LogoutDuration = x.LogoutDuration,
                                                    AfterLogoutTime = x.AfterLogoutTime,
                                                    Salary = x.Salary,
                                                    Bonus = x.Bonus,
                                                    VacationsCount = x.VacationsCount,
                                                    VacationEndCount = x.VacationEndCount,
                                                    WorkStartDate = x.WorkStartDate,
                                                    EndWorkDate = x.EndWorkDate,
                                                    WorkStartHijriDate = x.WorkStartHijriDate,
                                                    //WorkEndDate = x.WorkEndDate,
                                                    //WorkEndHijriDate = x.WorkEndHijriDate,
                                                    DeppID = x.DeppID,
                                                    BranchName = x.Branch.NameAr,
                                                    PostalCode = x.PostalCode,
                                                    AccountIDs = x.AccountIDs,
                                                    AccountIDs_Bouns = x.AccountIDs_Bouns,
                                                    AccountIDs_Custody = x.AccountIDs_Custody,
                                                    AccountIDs_Discount = x.AccountIDs_Discount,
                                                    AccountIDs_Salary = x.AccountIDs_Salary,
                                                    Taamen = x.Taamen,
                                                    EarlyLogin = x.EarlyLogin,
                                                    EmpServiceDuration = x.EmpServiceDuration,
                                                    ResonLeave = x.ResonLeave,
                                                    DirectManager = x.DirectManager,

                                                }).ToList();
            return Employees;
        }
        public async Task<EmployeesVM> GetEmployeeInfo(int EmployeeId, string lang, int BranchId)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.EmployeeId == EmployeeId).Select(x => new EmployeesVM        
            {
                EmployeeId = x.EmployeeId,
                EmployeeNo = x.EmployeeNo,
                EmployeeNameAr = x.EmployeeNameAr,
                Salary = x.Salary??0,
                Bonus = x.Bonus,

            }).FirstOrDefault();
            return Employees;
        }

        public async Task<int> GenerateNextEmpNumber(int BranchId)
        {
            if (_TaamerProContext.Employees != null)
            {
                var lastRow = _TaamerProContext.Employees.Where(s => s.IsDeleted == false).OrderByDescending(u => u.EmployeeId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.EmployeeNo) + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        public async Task<int> GetVacationsCount(int BranchId)
        {
            return _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Count();

        }
        public async Task<int> GetNationalityIdCount(int BranchId)
        {
            return _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Count();

        }
        public async Task<int> GetPassportNoCount(int BranchId)
        {
            return _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Count();

        }

        public async Task<int> SearchEmployeesOfPass(string EmployeesSearchpass, string lang, int BranchId, int UserId)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.PassportNo == EmployeesSearchpass && s.EmployeeId != UserId).Count();
            return Employees;
        }
        public async Task<int> SearchEmployeesOfNational(string EmployeesSearchNationalId, string lang, int BranchId, int UserId)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.NationalId == EmployeesSearchNationalId && s.EmployeeId != UserId).Count();
            return Employees;
        }
        public async Task<int> SearchEmployeesOfEmail(string EmployeesSearchEmail, int BranchId)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.Email == EmployeesSearchEmail).Count();
            return Employees;
        }
        public async Task<List<int?>> GetEmployeeByDepartment(int departmentId)
        {
            var employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && s.DepartmentId == departmentId).Select(x => x.UserId).ToList();
            return employees;
        }

        public async Task<List<int>> GetEmpsInVacations(int? month = 0)
        {
            DateTime Start = DateTime.Now;
            if (month.HasValue && month != 0)
                Start = new DateTime(DateTime.Now.Year, month.Value, DateTime.Now.Day).Date; 

            var CurrentVactions_EmpS = _TaamerProContext.Vacation.Where(s => !s.IsDeleted && s.VacationStatus == 2 && string.IsNullOrEmpty(s.BackToWorkDate)).ToList().
                Where(x => !string.IsNullOrEmpty(x.StartDate) && DateTime.ParseExact(x.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date <= Start).Select(x => x.EmployeeId);
            List<int> EmpsInVac = _TaamerProContext.Employees.Where(x => CurrentVactions_EmpS.Contains(x.EmployeeId) && !x.IsDeleted && !string.IsNullOrEmpty(x.WorkStartDate) && string.IsNullOrEmpty(x.EndWorkDate)).
                Select(x => x.EmployeeId).ToList();
            return EmpsInVac;
        }
        public async Task<List<string>> GetEmpNosInVacations(int? month = 0)
        {
            DateTime Start = DateTime.Now;
            if (month.HasValue && month != 0)
                Start = new DateTime(DateTime.Now.Year, month.Value, DateTime.Now.Day).Date;

            var CurrentVactions_EmpS = _TaamerProContext.Vacation.Where(s => !s.IsDeleted && string.IsNullOrEmpty(s.BackToWorkDate)).ToList().
                Where(x => !string.IsNullOrEmpty(x.StartDate) && DateTime.ParseExact(x.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date <= DateTime.Now.Date).Select(x => x.EmployeeId);
            List<string> EmpsInVac = _TaamerProContext.Employees.Where(x => CurrentVactions_EmpS.Contains(x.EmployeeId) && !x.IsDeleted && !string.IsNullOrEmpty(x.WorkStartDate) && string.IsNullOrEmpty(x.EndWorkDate)).
                Select(x => x.EmployeeNo).ToList();
            return EmpsInVac;
        }

        public async Task<List<int>> GetAllActiveEmpsByDate(string StartDate, string EndDate)
        {
            try {
                var emp = _TaamerProContext.Employees.Where(x => x.IsDeleted==false).Select(x => new EmployeesVM
                {
                    EmployeeId =x.EmployeeId,
                    WorkStartDate=x.WorkStartDate,
                    EndWorkDate=x.EndWorkDate,
                    
                }).ToList();
                var emp2= emp.Where( x =>
                           !string.IsNullOrEmpty(x.WorkStartDate) && DateTime.ParseExact(x.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                           &&
                           (string.IsNullOrEmpty(x.EndWorkDate) ||
                             (!string.IsNullOrEmpty(x.EndWorkDate) &&
                                (DateTime.ParseExact(x.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)))))
                       .ToList();
                return emp2.Select(x=>x.EmployeeId).ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Object> GetEmployeeStatistics() {
            //بدون عقد عمل , بدون تأمين طبي, بدون عمل مباشرة, لديهم عهد, لديهم سلف , المجازين

            var statis = new
            {
                WithoutContracts = _TaamerProContext.Employees.Where(x => !x.IsDeleted && string.IsNullOrEmpty(x.ContractNo)).Count(),
                WithoutMedicalInsurance = _TaamerProContext.Employees.Where(x => !x.IsDeleted && string.IsNullOrEmpty(x.MedicalNo)).Count(),
                DidnotStartWork = _TaamerProContext.Employees.Where(x => !x.IsDeleted && string.IsNullOrEmpty(x.WorkStartDate)).Count(),
                HaveCustodies = _TaamerProContext.Employees.Where(x => !x.IsDeleted && (x.Custodies.Where(s => s.IsDeleted == false && s.Status == false).Count() > 0)).Count(),

                HaveLoans = _TaamerProContext.Employees.Where(x => !x.IsDeleted && x.Loans.Where(s => s.IsDeleted == false && s.Status == 2).FirstOrDefault()
                                    .LoanDetails.Where(y => y.Date.Value >= DateTime.Now).Count() > 0).Count(),

                HaveDicounts = _TaamerProContext.Employees.Where(x => !x.IsDeleted && x.DiscountRewards.Where(y => !y.IsDeleted && y.Type == 1 &&
                                 y.AddDate.Value.Month >= DateTime.Today.Month).Count() > 0).Count()
            };
            return statis;
        }
        public async Task<IEnumerable<rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con,int? DepartmentID)
        {
            try
            {
                List<rptGetResdencesAboutToExpireVM> lmd = new List<rptGetResdencesAboutToExpireVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                        command.CommandText = "rptGetResdencesAboutToExpire";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new rptGetResdencesAboutToExpireVM
                            {
                                NationalId = (dr[0]).ToString(),
                                NameAr = dr[1].ToString(),
                                Nationality = dr[2].ToString(),
                                NationalIdEndDate = dr[3].ToString(),
                                NotifiDate = dr[4].ToString(),
                                Department = dr[5].ToString(),
                                Branch = dr[6].ToString(),
                                ContractEndDate= dr[7].ToString(),
                                JobName= dr[8].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<rptGetResdencesAboutToExpireVM> lmd = new List<rptGetResdencesAboutToExpireVM>();
                return lmd;
            }


        }
        public async Task< IEnumerable<rptGetResdencesExpiredVM>> GetResDencesExpired(string Con,int? DepartmentID)
        {
            try
            {
                List<rptGetResdencesExpiredVM> lmd = new List<rptGetResdencesExpiredVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));

                        command.CommandText = "rptGetResdencesExpired";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new rptGetResdencesExpiredVM
                            {
                                NationalId = (dr[0]).ToString(),
                                NameAr = dr[1].ToString(),
                                Nationality = dr[2].ToString(),
                                NationalIdEndDate = dr[3].ToString(),
                                Department = dr[4].ToString(),
                                Branch = dr[5].ToString(),
                                ContractEndDate = dr[6].ToString(),
                                JobName = dr[7].ToString(),
                                DirectManager = dr[8].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<rptGetResdencesExpiredVM> lmd = new List<rptGetResdencesExpiredVM>();
                return lmd;
            }

        }
        public async Task< IEnumerable<rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con,int? DepartmentID)
        {
            try
            {
                List<rptGetOfficialDocsAboutToExpire> lmd = new List<rptGetOfficialDocsAboutToExpire>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                        command.CommandText = "rptGetOfficialDocsAboutToExpire";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new rptGetOfficialDocsAboutToExpire
                            {
                                NameAr = (dr[0]).ToString(),
                                Number = dr[1].ToString(),
                                DocSource = dr[2].ToString(),
                                ExpiredDate = dr[3].ToString(),
                                NotifiDate = dr[4].ToString(),
                                Branch = dr[5].ToString(),
                                Notes = dr[6].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetOfficialDocsAboutToExpire> lmd = new List< rptGetOfficialDocsAboutToExpire>();
                return lmd;
            }

        }
        public async Task< IEnumerable< rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con, int? DepartmentID)
        {
            try
            {
                List< rptGetOfficialDocsExpiredVM> lmd = new List< rptGetOfficialDocsExpiredVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));

                        command.CommandText = "rptGetOfficialDocsExpired";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetOfficialDocsExpiredVM
                            {
                                NameAr = (dr[0]).ToString(),
                                Number = dr[1].ToString(),
                                DocSource = dr[2].ToString(),
                                ExpiredDate = dr[3].ToString(),
                                Branch = dr[4].ToString(),
                                Notes = dr[5].ToString(),
                                AttachmentUrl= dr[6].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetOfficialDocsExpiredVM> lmd = new List< rptGetOfficialDocsExpiredVM>();
                return lmd;
            }

        }
        public async Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmentId,string lang)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted==false &&(s.DepartmentId==DepartmentId || DepartmentId==null || DepartmentId==0)
            &&(s.ContractNo ==null  && s.QuaContract ==null))
                                                .Select(x => new EmployeesVM
                                                {
                                                    EmployeeId = x.EmployeeId,
                                                    EmployeeNo = x.EmployeeNo,
                                                    EmployeeNameAr = x.EmployeeNameAr,
                                                    EmployeeNameEn = x.EmployeeNameEn,
                                                    EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                                    Email = x.Email,
                                                    Mobile = x.Mobile,
                                                    Address = x.Address,
                                                    NationalId = x.NationalId,
                                                    AccountId = x.AccountId,
                                                    EducationalQualification = x.EducationalQualification,
                                                    BirthDate = x.BirthDate,
                                                    BirthHijriDate = x.BirthHijriDate,
                                                    BirthPlace = x.BirthPlace,
                                                    MaritalStatus = x.MaritalStatus,
                                                    ChildrenNo = x.ChildrenNo,
                                                    Gender = x.Gender,
                                                    NationalityId = x.NationalityId,
                                                    ReligionId = x.ReligionId,
                                                    UserId = x.UserId,
                                                    JobId = x.JobId,
                                                    DepartmentId = x.DepartmentId,
                                                    BranchId = x.BranchId,
                                                    Telephone = x.Telephone,
                                                    Mailbox = x.Mailbox,
                                                    NationalIdSource = x.NationalIdSource,
                                                    NationalIdDate = x.NationalIdDate,
                                                    NationalIdHijriDate = x.NationalIdHijriDate,
                                                    NationalIdEndDate = x.NationalIdEndDate,
                                                    NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                                    PassportNo = x.PassportNo,
                                                    PassportSource = x.PassportSource,
                                                    PassportNoDate = x.PassportNoDate,
                                                    PassportNoHijriDate = x.PassportNoHijriDate,
                                                    PassportEndDate = x.PassportEndDate,
                                                    PassportEndHijriDate = x.PassportEndHijriDate,
                                                    ContractNo = x.ContractNo,
                                                    ContractSource = x.ContractSource,
                                                    ContractStartDate = x.ContractStartDate,
                                                    ContractStartHijriDate = x.ContractStartHijriDate,
                                                    ContractEndDate = x.ContractEndDate,
                                                    ContractEndHijriDate = x.ContractEndHijriDate,
                                                    MedicalNo = x.MedicalNo,
                                                    MedicalSource = x.MedicalSource,
                                                    MedicalStartDate = x.MedicalStartDate,
                                                    MedicalStartHijriDate = x.MedicalStartHijriDate,
                                                    MedicalEndDate = x.MedicalEndDate,
                                                    MedicalEndHijriDate = x.MedicalEndHijriDate,
                                                    LicenceNo = x.LicenceNo,
                                                    LiscenseSourceId = x.LiscenseSourceId,
                                                    LicenceStartDate = x.LicenceStartDate,
                                                    LicenceStartHijriDate = x.LicenceStartHijriDate,
                                                    LicenceEndDate = x.LicenceEndDate,
                                                    LicenceEndHijriDate = x.LicenceEndHijriDate,
                                                    DawamId = x.DawamId,
                                                    TimeDurationLate = x.TimeDurationLate,
                                                    LogoutDuration = x.LogoutDuration,
                                                    AfterLogoutTime = x.AfterLogoutTime,
                                                    Salary = x.Salary,
                                                    Bonus = x.Bonus,
                                                    VacationsCount = x.VacationsCount,
                                                    VacationEndCount = x.VacationEndCount,
                                                    WorkStartDate = x.WorkStartDate,
                                                    EndWorkDate = x.EndWorkDate,
                                                    WorkStartHijriDate = x.WorkStartHijriDate,
                                                    //WorkEndDate = x.WorkEndDate,
                                                    //WorkEndHijriDate = x.WorkEndHijriDate,
                                                    DeppID = x.DeppID,
                                                    BranchName = x.Branch.NameAr,
                                                    PostalCode = x.PostalCode,
                                                    AccountIDs = x.AccountIDs,
                                                    AccountIDs_Bouns = x.AccountIDs_Bouns,
                                                    AccountIDs_Custody = x.AccountIDs_Custody,
                                                    AccountIDs_Discount = x.AccountIDs_Discount,
                                                    AccountIDs_Salary = x.AccountIDs_Salary,
                                                    Taamen = x.Taamen,
                                                    EarlyLogin = x.EarlyLogin,
                                                    EmpServiceDuration = x.EmpServiceDuration,
                                                    ResonLeave = x.ResonLeave,
                                                    DirectManager = x.DirectManager,
                                                    JobName=x.Job!=null? x.Job.JobNameAr:"",
                                                    NationalityName=x.Nationality!=null? x.Nationality.NameAr:"",
                                                    DepartmentName=x.Department!=null? x.Department.DepartmentNameAr:"",

                                                }).ToList();
            return Employees;
        }


        public async Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmentId, string lang,string? Searchtext)
        {
            var Employees = _TaamerProContext.Employees.Where(s => s.IsDeleted == false && (s.DepartmentId == DepartmentId || DepartmentId == null || DepartmentId == 0)
            && (s.ContractNo == null && s.QuaContract == null) && (s.EmployeeNameAr.Contains(Searchtext) || s.EmployeeNameEn.Contains(Searchtext) || s.Nationality.NameAr.Contains(Searchtext) ||
            s.Job.JobNameAr.Contains(Searchtext) || s.Department.DepartmentNameAr.Contains(Searchtext) || s.Branch.NameAr.Contains(Searchtext) ||
            s.Mobile.Contains(Searchtext) || s.Email.Contains(Searchtext) || s.NationalId.Contains(Searchtext) || Searchtext == null || Searchtext ==""))
                                                .Select(x => new EmployeesVM
                                                {
                                                    EmployeeId = x.EmployeeId,
                                                    EmployeeNo = x.EmployeeNo,
                                                    EmployeeNameAr = x.EmployeeNameAr,
                                                    EmployeeNameEn = x.EmployeeNameEn,
                                                    EmployeeName = lang == "ltr" ? x.EmployeeNameEn : x.EmployeeNameAr,
                                                    Email = x.Email,
                                                    Mobile = x.Mobile,
                                                    Address = x.Address,
                                                    NationalId = x.NationalId,
                                                    AccountId = x.AccountId,
                                                    EducationalQualification = x.EducationalQualification,
                                                    BirthDate = x.BirthDate,
                                                    BirthHijriDate = x.BirthHijriDate,
                                                    BirthPlace = x.BirthPlace,
                                                    MaritalStatus = x.MaritalStatus,
                                                    ChildrenNo = x.ChildrenNo,
                                                    Gender = x.Gender,
                                                    NationalityId = x.NationalityId,
                                                    ReligionId = x.ReligionId,
                                                    UserId = x.UserId,
                                                    JobId = x.JobId,
                                                    DepartmentId = x.DepartmentId,
                                                    BranchId = x.BranchId,
                                                    Telephone = x.Telephone,
                                                    Mailbox = x.Mailbox,
                                                    NationalIdSource = x.NationalIdSource,
                                                    NationalIdDate = x.NationalIdDate,
                                                    NationalIdHijriDate = x.NationalIdHijriDate,
                                                    NationalIdEndDate = x.NationalIdEndDate,
                                                    NationalIdEndHijriDate = x.NationalIdEndHijriDate,
                                                    PassportNo = x.PassportNo,
                                                    PassportSource = x.PassportSource,
                                                    PassportNoDate = x.PassportNoDate,
                                                    PassportNoHijriDate = x.PassportNoHijriDate,
                                                    PassportEndDate = x.PassportEndDate,
                                                    PassportEndHijriDate = x.PassportEndHijriDate,
                                                    ContractNo = x.ContractNo,
                                                    ContractSource = x.ContractSource,
                                                    ContractStartDate = x.ContractStartDate,
                                                    ContractStartHijriDate = x.ContractStartHijriDate,
                                                    ContractEndDate = x.ContractEndDate,
                                                    ContractEndHijriDate = x.ContractEndHijriDate,
                                                    MedicalNo = x.MedicalNo,
                                                    MedicalSource = x.MedicalSource,
                                                    MedicalStartDate = x.MedicalStartDate,
                                                    MedicalStartHijriDate = x.MedicalStartHijriDate,
                                                    MedicalEndDate = x.MedicalEndDate,
                                                    MedicalEndHijriDate = x.MedicalEndHijriDate,
                                                    LicenceNo = x.LicenceNo,
                                                    LiscenseSourceId = x.LiscenseSourceId,
                                                    LicenceStartDate = x.LicenceStartDate,
                                                    LicenceStartHijriDate = x.LicenceStartHijriDate,
                                                    LicenceEndDate = x.LicenceEndDate,
                                                    LicenceEndHijriDate = x.LicenceEndHijriDate,
                                                    DawamId = x.DawamId,
                                                    TimeDurationLate = x.TimeDurationLate,
                                                    LogoutDuration = x.LogoutDuration,
                                                    AfterLogoutTime = x.AfterLogoutTime,
                                                    Salary = x.Salary,
                                                    Bonus = x.Bonus,
                                                    VacationsCount = x.VacationsCount,
                                                    VacationEndCount = x.VacationEndCount,
                                                    WorkStartDate = x.WorkStartDate,
                                                    EndWorkDate = x.EndWorkDate,
                                                    WorkStartHijriDate = x.WorkStartHijriDate,
                                                    //WorkEndDate = x.WorkEndDate,
                                                    //WorkEndHijriDate = x.WorkEndHijriDate,
                                                    DeppID = x.DeppID,
                                                    BranchName = x.Branch.NameAr,
                                                    PostalCode = x.PostalCode,
                                                    AccountIDs = x.AccountIDs,
                                                    AccountIDs_Bouns = x.AccountIDs_Bouns,
                                                    AccountIDs_Custody = x.AccountIDs_Custody,
                                                    AccountIDs_Discount = x.AccountIDs_Discount,
                                                    AccountIDs_Salary = x.AccountIDs_Salary,
                                                    Taamen = x.Taamen,
                                                    EarlyLogin = x.EarlyLogin,
                                                    EmpServiceDuration = x.EmpServiceDuration,
                                                    ResonLeave = x.ResonLeave,
                                                    DirectManager = x.DirectManager,
                                                    JobName = x.Job != null ? x.Job.JobNameAr : "",
                                                    NationalityName = x.Nationality != null ? x.Nationality.NameAr : "",
                                                    DepartmentName = x.Department != null ? x.Department.DepartmentNameAr : "",

                                                }).ToList();
            return Employees;
        }

        public Employees Add(Employees entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employees> AddRange(IEnumerable<Employees> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employees> GetAll()
        {
            throw new NotImplementedException();
        }

        public Employees GetById(int Id)
        {
            return _TaamerProContext.Employees.Where(x => x.EmployeeId == Id).FirstOrDefault();
        }

        public IEnumerable<Employees> GetMatching(Func<Employees, bool> where)
        {
            return _TaamerProContext.Employees.Where(where).ToList<Employees>();
        }

        public void Remove(Employees entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void RemoveMatching(Func<Employees, bool> where)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Employees> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Employees entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employees> Queryable()
        {
            throw new NotImplementedException();
        }
    }
}