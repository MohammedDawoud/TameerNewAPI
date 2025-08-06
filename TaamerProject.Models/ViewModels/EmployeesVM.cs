using System;

namespace TaamerProject.Models
{
    public class EmployeesVM : Auditable
    {
        public int EmployeeId { get; set; }
        public string? EmployeeNo { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeNameAr { get; set; }
        public string? EmployeeNameEn { get; set; }
        public int? JobId { get; set; }
        public int? ReligionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? NationalityId { get; set; }
        public int? BankId { get; set; }
        public int? Gender { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? NationalId { get; set; }
        public string? PassportNo { get; set; }
        public string? PhotoUrl { get; set; }
        public string? NationalIdDate { get; set; }
        public string? NationalIdHijriDate { get; set; }
        public string? PassportNoDate { get; set; }
        public string? PassportNoHijriDate { get; set; }
        public decimal? Salary { get; set; }
        public decimal? ThisMonthSalary { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Rewards { get; set; }
        public decimal? Allowances { get; set; }
        public decimal? Loan { get; set; }
        public string? BeginWorkDate { get; set; }
        public string? BeginWorkHijriDate { get; set; }
        public string? EndWorkDate { get; set; }
        public string? EndWorkHijriDate { get; set; }
        public string? BankCardNo { get; set; }
        public string? BankDate { get; set; }
        public string? BankHijriDate { get; set; }
        public bool? HaveLicence { get; set; }
        public string? LicenceNo { get; set; }
        public string? LicenceStartDate { get; set; }
        public string? LicenceStartHijriDate { get; set; }
        public string? LicenceEndDate { get; set; }
        public string? LicenceEndHijriDate { get; set; }
        public string? LicenceSource { get; set; }
        public int? LiscenseSourceId { get; set; }
        public int? AccountId { get; set; }
        public string? EducationalQualification { get; set; }
        public string? BirthDate { get; set; }
        public string? BirthHijriDate { get; set; }
        public string? BirthPlace { get; set; }
        public string? Telephone { get; set; }
        public string? Mailbox { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public int? ChildrenNo { get; set; }
        public int? MaritalStatus { get; set; }
        public bool? Active { get; set; }
        public int? UsrId { get; set; }
        public int? NationalIdSource { get; set; }
        public string? NationalIdEndDate { get; set; }
        public string? NationalIdEndHijriDate { get; set; }
        public string? PassportSource { get; set; }
        public string? PassportEndDate { get; set; }
        public string? PassportEndHijriDate { get; set; }
        public string? ContractNo { get; set; }
        public string? ContractStartDate { get; set; }
        public string? ContractStartHijriDate { get; set; }
        public string? ContractSource { get; set; }
        public string? ContractEndDate { get; set; }
        public string? ContractEndHijriDate { get; set; }
        public string? WorkNo { get; set; }
        public string? WorkStartDate { get; set; }
        public string? WorkStartHijriDate { get; set; }
        //public string? WorkEndDate { get; set; }
        //public string? WorkEndHijriDate { get; set; }
        public string? WorkSource { get; set; }
        public string? MedicalNo { get; set; }
        public string? MedicalSource { get; set; }
        public string? MedicalStartDate { get; set; }
        public string? MedicalStartHijriDate { get; set; }
        public string? MedicalEndDate { get; set; }
        public string? MedicalEndHijriDate { get; set; }
        public string? MedNo { get; set; }
        public string? MedDNo { get; set; }
        public string? MedSource { get; set; }
        public string? MedDSource { get; set; }
        public string? MedDate { get; set; }
        public string? MedHijriDate { get; set; }
        public string? MedDDate { get; set; }
        public string? MedDHijriDate { get; set; }
        public string? MedEndDate { get; set; }
        public string? MedEndHijriDate { get; set; }
        public string? MedDEndDate { get; set; }
        public string? MedDEndHijriDate { get; set; }
        public int? DawamId { get; set; }
        public int? TimeDurationLate { get; set; }
        public int? LogoutDuration { get; set; }
        public int? AfterLogoutTime { get; set; }
        public int? DeppID { get; set; }
        public int? VacationsCount { get; set; }
        public int? CitySourceId { get; set; }
        public int? NationalIdEndCount { get; set; }
        public int? PassportEndCount { get; set; }
        public int? ContractEndCount { get; set; }
        public int? LicenceCarEndCount { get; set; }
        public int? MedicalEndCount { get; set; }
        public int? VacationEndCount { get; set; }
        public int? LoanCount { get; set; }
        public string? NodeLocation { get; set; }
        public string? DepartmentName { get; set; }
        public string? JobName { get; set; }
        public string? BankName { get; set; }
        public string? AcountName { get; set; }
        public string? AccountCode { get; set; }
        public string? AttendaceTimeName { get; set; }
        public string? GenderName { get; set; }
        public string? MaritalStatusName { get; set; }
        public string? ReligionStatusName { get; set; }
        public string? NationalityName{ get; set; }

        public decimal? CommunicationAllawance { get; set; }
        public decimal? ProfessionAllawance { get; set; }
        public decimal? TransportationAllawance { get; set; }
        public decimal? HousingAllowance { get; set; }
        public decimal? MonthlyAllowances { get; set; }
        public decimal? ExtraAllowances { get; set; }
        public decimal? TotalLoans { get; set; }
        public decimal? TotalDiscounts { get; set; }
        public decimal? TotalRewards { get; set; }
        public decimal? TotalySalaries { get; set; }
        public decimal? AddAllowances { get; set; }
        public decimal? TotalViolations { get; set; }
        public decimal? TotalPaidVacations { get; set; }
        public decimal? TotalLateDiscount { get; set; }
        public decimal? TotalAbsenceDiscount { get; set; }
        public bool IsSearch { get; set; }

        public int TotalDayAbs { get; set; }
        public bool? IsAllBranch { get; set; }
        public string? PostalCode { get; set; }
        public int? AccountIDs { get; set; }

        public int? AccountIDs_Discount { get; set; }
        public int? AccountIDs_Bouns { get; set; }
        public int? AccountIDs_Salary { get; set; }
        public int? AccountIDs_Custody { get; set; }

        public int? MonthNo { get; set; }
        public string? Taamen { get; set; }
        public string? BranchName { get; set; }
        public string? MonthName { get; set; }
        public string? AddUserName { get; set; }

        public int? EarlyLogin { get; set; }

        public string? ResonLeave { get; set; }
        public string? EmpServiceDuration { get; set; }

        public string? AddEmployeeImg { get; set; }
        public string? UserName { get; set; }
        public int? DirectManager { get; set; }

        public string? QuaContract { get; set; }

        public decimal? OtherAllownces { get; set; }
        public string? Age { get; set; }

        public int? AttendenceLocationId { get; set; }
        public string? AttendenceLocationName { get; set; }
        public bool? allowoutsidesite { get; set; }
        public bool? allowallsite { get; set; }

        public decimal? EmpHourlyCost { get; set; }
        public decimal? DailyWorkinghours { get; set; }
    }
}
