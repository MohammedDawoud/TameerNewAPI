using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DBContext
{
    public partial class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
        }

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Allowance> Allowance { get; set; }
        public virtual DbSet<AllowanceType> AllowanceType { get; set; }
        public virtual DbSet<Appraisal> Appraisal { get; set; }
        public virtual DbSet<ArchiveFiles> ArchiveFiles { get; set; }
        public virtual DbSet<AttAbsentDay> AttAbsentDay { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<AttDeviceSitting> AttDeviceSitting { get; set; }
        public virtual DbSet<AttendaceTime> AttendaceTime { get; set; }
        public virtual DbSet<Attendees> Attendees { get; set; }
        public virtual DbSet<Attendence> Attendence { get; set; }
        public virtual DbSet<AttendenceDevice> AttendenceDevice { get; set; }
        public virtual DbSet<AttTimeDetails> AttTimeDetails { get; set; }
        public virtual DbSet<CarMovements> CarMovements { get; set; }
        public virtual DbSet<CarMovementsType> CarMovementsType { get; set; }
        public virtual DbSet<Custody> Custody { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DiscountReward> DiscountReward { get; set; }
        public virtual DbSet<EmpContract> EmpContract { get; set; }
        public virtual DbSet<EmpContractDetail> EmpContractDetail { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<EmpStructure> EmpStructure { get; set; }
        public virtual DbSet<ExpensesGovernment> ExpensesGovernment { get; set; }
        public virtual DbSet<ExpensesGovernmentType> ExpensesGovernmentType { get; set; }
        public virtual DbSet<ExpRevenuExpenses> ExpRevenuExpenses { get; set; }
        public virtual DbSet<ExternalEmployees> ExternalEmployees { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<LoanDetails> LoanDetails { get; set; }
        public virtual DbSet<Nationality> Nationality { get; set; }
        public virtual DbSet<OfficalHoliday> OfficalHoliday { get; set; }
        public virtual DbSet<PayrollMarches> PayrollMarches { get; set; }
        public virtual DbSet<ReasonLeave> ReasonLeave { get; set; }
        public virtual DbSet<Vacation> Vacation { get; set; }
        public virtual DbSet<VacationType> VacationType { get; set; }
        public virtual DbSet<DeviceAtt> DeviceAtt { get; set; }

        //public virtual DbSet<Emp_SalaryParts> Emp_SalaryParts { get; set; }
        public virtual DbSet<Emp_VacationsStat> Emp_VacationsStat { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Allowance>(entity =>
            {
                entity.HasKey(e => e.AllowanceId);
                entity.ToTable("Emp_Allowances");
                entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
                entity.Property(t => t.AllowanceTypeId).HasColumnName("TypeId");
                entity.Property(t => t.AllowanceAmount).HasColumnName("Amount");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.Month).HasColumnName("Month");
                entity.Property(t => t.IsFixed).HasColumnName("IsFixed");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.AllowanceMonthNo).HasColumnName("AllowanceMonthNo");

                modelBuilder.Entity<Allowance>().HasOne(s => s.AllowanceType).WithMany().HasForeignKey(e => e.AllowanceTypeId);
                modelBuilder.Entity<Allowance>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmployeeId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<AllowanceType>(entity =>
            {
                entity.HasKey(e => e.AllowanceTypeId);
                entity.ToTable("Emp_AllowancesTypes");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.IsSalaryPart).HasColumnName("IsSalaryPart");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Appraisal>(entity =>
            {
                entity.HasKey(e => e.AppraisalId);
                entity.ToTable("Emp_Appraisal");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.Degree).HasColumnName("Degree");
                entity.Property(t => t.ManagerId).HasColumnName("ManagerId");
                entity.Property(t => t.MonthDate).HasColumnName("MonthDate");
                entity.Property(t => t.Month).HasColumnName("Month");
                entity.Property(t => t.Year).HasColumnName("Year");
                modelBuilder.Entity<Appraisal>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);

            });

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<AttAbsentDay>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Emp_AbsentDay");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.Year).HasColumnName("Year");
                entity.Property(t => t.Month).HasColumnName("Month");
                entity.Property(t => t.AbsDays).HasColumnName("AbsDays");
                entity.Property(t => t.SDate).HasColumnName("SDate");
                entity.Property(t => t.EDate).HasColumnName("EDate");
                modelBuilder.Entity<AttAbsentDay>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);
                entity.ToTable("Emp_Attachments");
                entity.Property(t => t.AttachmentName).HasColumnName("Name");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<AttendaceTime>(entity =>
            {
                entity.HasKey(e => e.TimeId);
                entity.ToTable("Emp_AttendaceTime");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<AttendaceTime>().HasMany<AttTimeDetails>(s => s.AttTimeDetails).WithOne(g => g.AttendaceTime).HasForeignKey(s => s.AttTimeId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Attendees>(entity =>
            {
                entity.HasKey(e => e.AttendeesId);
                entity.ToTable("Emp_Attendees");
                entity.Property(t => t.EarlyCheckOutMin).HasColumnName("EarlyCheckOutMin");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.Discount).HasColumnName("Discount");
                entity.Property(t => t.Day).HasColumnName("Day");
                entity.Property(t => t.IsRealVacancy).HasColumnName("IsRealVacancy");
                entity.Property(t => t.IsVacancy).HasColumnName("IsVacancy");
                entity.Property(t => t.IsVacancyEmp).HasColumnName("IsVacancyEmp");
                entity.Property(t => t.LateCheckInMinutes).HasColumnName("LateCheckInMinutes");
                entity.Property(t => t.LateMinutes).HasColumnName("LateMinutes");
                entity.Property(t => t.WorkMinutes).HasColumnName("WorkMinutes");
                entity.Property(t => t.IsEarlyCheckOut).HasColumnName("IsEarlyCheckOut");
                entity.Property(t => t.AttTimeId).HasColumnName("AttTimeId");
                entity.Property(t => t.IsDone).HasColumnName("IsDone");
                entity.Property(t => t.IsEntry).HasColumnName("IsEntry");
                entity.Property(t => t.IsLate).HasColumnName("IsLate");
                entity.Property(t => t.IsOverTime).HasColumnName("IsOverTime");
                entity.Property(t => t.ActualWorkMinutes).HasColumnName("ActualWorkMinutes");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.DayOfWeek).HasColumnName("DayOfWeek");
                entity.Property(t => t.OverTimeMinutes).HasColumnName("OverTimeMinutes");
                entity.Property(t => t.Bonus).HasColumnName("Bonus");
                entity.Property(t => t.IsLateCheckIn).HasColumnName("IsLateCheckIn");
                entity.Property(t => t.IsOut).HasColumnName("IsOut");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<AttAbsentDay>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<AttendenceDevice>(entity =>
            {
                entity.HasKey(e => e.AttendenceDeviceId);
                entity.ToTable("Emp_AttendenceDevice");
                entity.Property(t => t.DeviceIP).HasColumnName("DeviceIP");
                entity.Property(t => t.Port).HasColumnName("Port");
                entity.Property(t => t.MachineNumber).HasColumnName("MachineNumber");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.LastUpdate).HasColumnName("LastUpdate");
                modelBuilder.Entity<AttendenceDevice>().HasOne(s => s.BranchName).WithMany().HasForeignKey(e => e.BranchId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Attendence>(entity =>
            {
                entity.HasKey(e => e.AttendenceId);
                entity.ToTable("Emp_Attendence");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.RealEmpId).HasColumnName("RealEmpId");
                entity.Property(t => t.AttendenceDate).HasColumnName("AttendenceDate");
                entity.Property(t => t.AttendenceHijriDate).HasColumnName("AttendenceHijriDate");
                entity.Property(t => t.Day).HasColumnName("Day");
                entity.Property(t => t.CheckIn).HasColumnName("CheckIn");
                entity.Property(t => t.CheckTime).HasColumnName("CheckTime");
                entity.Property(t => t.CheckOut).HasColumnName("CheckOut");
                entity.Property(t => t.IsLate).HasColumnName("IsLate");
                entity.Property(t => t.LateDuration).HasColumnName("LateDuration");
                entity.Property(t => t.IsOverTime).HasColumnName("IsOverTime");
                entity.Property(t => t.SameDate).HasColumnName("SameDate");
                entity.Property(t => t.IsDone).HasColumnName("IsDone");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.MoveTime).HasColumnName("MoveTime");
                entity.Property(t => t.Location).HasColumnName("Location");
                entity.Property(t => t.Latitude).HasColumnName("Latitude");
                entity.Property(t => t.Longitude).HasColumnName("Longitude");
                entity.Property(t => t.FromApplication).HasColumnName("FromApplication");
                entity.Property(t => t.Comment).HasColumnName("Comment");
                modelBuilder.Entity<Attendence>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<Attendence>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<AttTimeDetails>(entity =>
            {
                entity.HasKey(e => e.TimeDetailsId);
                entity.ToTable("Emp_AttTimeDetails");
                entity.Property(t => t.AttTimeId).HasColumnName("AttTimeId");
                entity.Property(t => t.Day).HasColumnName("Day");
                entity.Property(t => t.DayDate).HasColumnName("DayDate");
                entity.Property(t => t._1StFromHour).HasColumnName("1StFromHour");
                entity.Property(t => t._1StToHour).HasColumnName("1StToHour");
                entity.Property(t => t._2ndFromHour).HasColumnName("2ndFromHour");
                entity.Property(t => t._2ndToHour).HasColumnName("2ndToHour");
                entity.Property(t => t.IsWeekDay).HasColumnName("IsWeekDay");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<AttTimeDetails>().HasOne(s => s.AttendaceTime).WithMany().HasForeignKey(e => e.AttTimeId);

            });

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<CarMovements>(entity =>
            {
                entity.HasKey(e => e.MovementId);
                entity.ToTable("Emp_CarMovements");
                entity.Property(t => t.ItemId).HasColumnName("ItemId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.EmpAmount).HasColumnName("EmpAmount");
                entity.Property(t => t.OwnerAmount).HasColumnName("OwnerAmount");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                modelBuilder.Entity<CarMovements>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<CarMovements>().HasOne(s => s.Types).WithMany().HasForeignKey(e => e.Type);
                modelBuilder.Entity<CarMovements>().HasOne(s => s.Item).WithMany().HasForeignKey(e => e.ItemId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CarMovementsType>(entity =>
            {
                entity.HasKey(e => e.TypeId);
                entity.ToTable("Emp_CarMovementsType");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");

            });

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<Custody>(entity =>
            {
                entity.HasKey(e => e.CustodyId);
                entity.ToTable("Emp_EmpCustody");
                entity.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
                entity.Property(t => t.ItemId).HasColumnName("ItemId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Quantity).HasColumnName("Quantity");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.CustodyValue).HasColumnName("CustodyValue");
                entity.Property(t => t.ConvertStatus).HasColumnName("ConvertStatus");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                modelBuilder.Entity<Custody>().HasOne(s => s.Item).WithMany().HasForeignKey(e => e.ItemId);
                modelBuilder.Entity<Custody>().HasOne(s => s.Employee).WithMany().HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<Custody>().HasOne(s => s.Invoices).WithMany().HasForeignKey(e => e.InvoiceId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);
                entity.ToTable("Emp_Departments");
                entity.Property(t => t.DepartmentNameAr).HasColumnName("NameAr");
                entity.Property(t => t.DepartmentNameEn).HasColumnName("NameEn");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<DiscountReward>(entity =>
            {
                entity.HasKey(e => e.DiscountRewardId);
                entity.ToTable("Emp_DiscountRewards");
                entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.MonthNo).HasColumnName("MonthNo");

                modelBuilder.Entity<DiscountReward>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<DiscountReward>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.DiscountReward).HasForeignKey(s => s.DiscountRewardId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.ToTable("Emp_Employees");
                entity.Property(t => t.EmployeeNo).HasColumnName("EmpNo");
                entity.Property(t => t.EmployeeNameAr).HasColumnName("NameAr");
                entity.Property(t => t.EmployeeNameEn).HasColumnName("NameEn");
                entity.Property(t => t.JobId).HasColumnName("JobId");
                entity.Property(t => t.ReligionId).HasColumnName("ReligionId");
                entity.Property(t => t.DepartmentId).HasColumnName("DeptId");
                entity.Property(t => t.NationalityId).HasColumnName("NationalityId");
                entity.Property(t => t.BankId).HasColumnName("BankId");
                entity.Property(t => t.Gender).HasColumnName("Gender");
                entity.Property(t => t.Email).HasColumnName("Email");
                entity.Property(t => t.Mobile).HasColumnName("Mobile");
                entity.Property(t => t.Address).HasColumnName("Address");
                entity.Property(t => t.NationalId).HasColumnName("NationalId");
                entity.Property(t => t.PassportNo).HasColumnName("PassportNo");
                entity.Property(t => t.PhotoUrl).HasColumnName("PhotoUrl");
                entity.Property(t => t.NationalIdDate).HasColumnName("NationalIdDate");
                entity.Property(t => t.NationalIdHijriDate).HasColumnName("NationalIdHijriDate");
                entity.Property(t => t.PassportNoDate).HasColumnName("PassportNoDate");
                entity.Property(t => t.PassportNoHijriDate).HasColumnName("PassportNoHijriDate");
                entity.Property(t => t.Salary).HasColumnName("Salary");
                entity.Property(t => t.Discount).HasColumnName("Discount");
                entity.Property(t => t.Bonus).HasColumnName("Bonus");
                entity.Property(t => t.Rewards).HasColumnName("Rewards");
                entity.Property(t => t.Allowances).HasColumnName("Allowances");
                entity.Property(t => t.Loan).HasColumnName("Loan");
                entity.Property(t => t.WorkStartDate).HasColumnName("WorkStartDate");
                entity.Property(t => t.BeginWorkHijriDate).HasColumnName("BeginWorkHijriDate");
                entity.Property(t => t.EndWorkDate).HasColumnName("EndWorkDate");
                entity.Property(t => t.EndWorkHijriDate).HasColumnName("EndWorkHijriDate");
                entity.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
                entity.Property(t => t.BankDate).HasColumnName("BankDate");
                entity.Property(t => t.BankHijriDate).HasColumnName("BankHijriDate");
                entity.Property(t => t.HaveLicence).HasColumnName("HaveLicence");
                entity.Property(t => t.LicenceNo).HasColumnName("LicenceNo");
                entity.Property(t => t.LicenceStartDate).HasColumnName("LicenceStartDate");
                entity.Property(t => t.LicenceStartHijriDate).HasColumnName("LicenceStartHijriDate");
                entity.Property(t => t.LicenceEndDate).HasColumnName("LicenceEndDate");
                entity.Property(t => t.LicenceEndHijriDate).HasColumnName("LicenceEndHijriDate");
                entity.Property(t => t.LicenceSource).HasColumnName("LicenceSource");
                entity.Property(t => t.LiscenseSourceId).HasColumnName("LiscenseSourceId");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.EducationalQualification).HasColumnName("EducationalQualification");
                entity.Property(t => t.BirthDate).HasColumnName("BirthDate");
                entity.Property(t => t.BirthHijriDate).HasColumnName("BirthHijriDate");
                entity.Property(t => t.BirthPlace).HasColumnName("BirthPlace");
                entity.Property(t => t.Telephone).HasColumnName("Tele");
                entity.Property(t => t.Mailbox).HasColumnName("Mailbox");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ChildrenNo).HasColumnName("ChildrenNo");
                entity.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
                entity.Property(t => t.Active).HasColumnName("Active");
                entity.Property(t => t.UsrId).HasColumnName("UsrId");
                entity.Property(t => t.NationalIdSource).HasColumnName("NationalIdSource");
                entity.Property(t => t.NationalIdEndDate).HasColumnName("NationalIdEndDate");
                entity.Property(t => t.NationalIdEndHijriDate).HasColumnName("NationalIdEndHijriDate");
                entity.Property(t => t.PassportSource).HasColumnName("PassportSource");
                entity.Property(t => t.PassportEndDate).HasColumnName("PassportEndDate");
                entity.Property(t => t.PassportEndHijriDate).HasColumnName("PassportEndHijriDate");
                entity.Property(t => t.ContractNo).HasColumnName("ContractNo");
                entity.Property(t => t.ContractStartDate).HasColumnName("ContractStartDate");
                entity.Property(t => t.ContractStartHijriDate).HasColumnName("ContractStartHijriDate");
                entity.Property(t => t.ContractSource).HasColumnName("ContractSource");
                entity.Property(t => t.ContractEndDate).HasColumnName("ContractEndDate");
                entity.Property(t => t.ContractEndHijriDate).HasColumnName("ContractEndHijriDate");
                entity.Property(t => t.WorkNo).HasColumnName("WorkNo");
                entity.Property(t => t.WorkStartDate).HasColumnName("WorkStartDate");
                entity.Property(t => t.WorkStartHijriDate).HasColumnName("WorkStartHijriDate");
                entity.Property(t => t.WorkSource).HasColumnName("WorkSource");
                entity.Property(t => t.MedicalNo).HasColumnName("MedicalNo");
                entity.Property(t => t.MedicalSource).HasColumnName("MedicalSource");
                entity.Property(t => t.MedicalStartDate).HasColumnName("MedicalStartDate");
                entity.Property(t => t.MedicalStartHijriDate).HasColumnName("MedicalStartHijriDate");
                entity.Property(t => t.MedicalEndDate).HasColumnName("MedicalEndDate");
                entity.Property(t => t.MedicalEndHijriDate).HasColumnName("MedicalEndHijriDate");
                entity.Property(t => t.MedNo).HasColumnName("MedNo");
                entity.Property(t => t.MedDNo).HasColumnName("MedDNo");
                entity.Property(t => t.MedSource).HasColumnName("MedSource");
                entity.Property(t => t.MedDSource).HasColumnName("MedDSource");
                entity.Property(t => t.MedDate).HasColumnName("MedDate");
                entity.Property(t => t.MedHijriDate).HasColumnName("MedHijriDate");
                entity.Property(t => t.MedDDate).HasColumnName("MedDDate");
                entity.Property(t => t.MedDHijriDate).HasColumnName("MedDHijriDate");
                entity.Property(t => t.MedEndDate).HasColumnName("MedEndDate");
                entity.Property(t => t.MedEndHijriDate).HasColumnName("MedEndHijriDate");
                entity.Property(t => t.MedDEndDate).HasColumnName("MedDEndDate");
                entity.Property(t => t.MedDEndHijriDate).HasColumnName("MedDEndHijriDate");
                entity.Property(t => t.DawamId).HasColumnName("DawamId");
                entity.Property(t => t.TimeDurationLate).HasColumnName("TimeDurationLate");
                entity.Property(t => t.LogoutDuration).HasColumnName("LogoutDuration");
                entity.Property(t => t.AfterLogoutTime).HasColumnName("AfterLogoutTime");
                entity.Property(t => t.DeppID).HasColumnName("DeppID");
                entity.Property(t => t.VacationsCount).HasColumnName("VacationsCount");
                entity.Property(t => t.CitySourceId).HasColumnName("CitySourceId");
                entity.Property(t => t.NationalIdEndCount).HasColumnName("NationalIdEndCount");
                entity.Property(t => t.PassportEndCount).HasColumnName("PassportEndCount");
                entity.Property(t => t.ContractEndCount).HasColumnName("ContractEndCount");
                entity.Property(t => t.LicenceCarEndCount).HasColumnName("LicenceCarEndCount");
                entity.Property(t => t.MedicalEndCount).HasColumnName("MedicalEndCount");
                entity.Property(t => t.VacationEndCount).HasColumnName("VacationEndCount");
                entity.Property(t => t.LoanCount).HasColumnName("LoanCount");
                entity.Property(t => t.LocationId).HasColumnName("LocationId");
                entity.Property(t => t.PostalCode).HasColumnName("PostalCode");
                entity.Property(t => t.AccountIDs).HasColumnName("AccountIDs");
                entity.Property(t => t.AccountIDs_Discount).HasColumnName("AccountIDs_Discount");
                entity.Property(t => t.AccountIDs_Bouns).HasColumnName("AccountIDs_Bouns");
                entity.Property(t => t.AccountIDs_Salary).HasColumnName("AccountIDs_Salary");
                entity.Property(t => t.AccountIDs_Custody).HasColumnName("AccountIDs_Custody");
                entity.Property(t => t.Taamen).HasColumnName("Taamen");
                entity.Property(t => t.EarlyLogin).HasColumnName("EarlyLogin");
                entity.Property(t => t.ResonLeave).HasColumnName("ResonLeave");
                entity.Property(t => t.EmpServiceDuration).HasColumnName("EmpServiceDuration");

                modelBuilder.Entity<Employees>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<Employees>().HasMany<Allowance>(s => s.Allowance).WithOne(g => g.Employees).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<Loan>(s => s.Loans).WithOne(g => g.Employees).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<Custody>(s => s.Custodies).WithOne(g => g.Employee).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<DiscountReward>(s => s.DiscountRewards).WithOne(g => g.Employees).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<AttAbsentDay>(s => s.AttAbsentDays).WithOne(g => g.Employees).HasForeignKey(s => s.EmpId);
                modelBuilder.Entity<Employees>().HasMany<Vacation>(s => s.Vacations).WithOne(g => g.EmployeeName).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<PayrollMarches>(s => s.PayrollMarches).WithOne(g => g.Employee).HasForeignKey(s => s.EmpId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Job).WithMany().HasForeignKey(e => e.JobId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Bank).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Account).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Nationality).WithMany().HasForeignKey(e => e.NationalityId);
                modelBuilder.Entity<Employees>().HasOne(s => s.AttendaceTime).WithMany().HasForeignKey(e => e.DawamId);
                modelBuilder.Entity<Employees>().HasOne(s => s.users).WithMany().HasForeignKey(e => e.UserId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Emp_VacationsStat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Emp_VacationsStat");
                entity.Property(t => t.EmpID).HasColumnName("EmpID");
                entity.Property(t => t.Year).HasColumnName("Year");
                entity.Property(t => t.Balance).HasColumnName("Balance");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Consumed).HasColumnName("Consumed");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<EmpContractDetail>(entity =>
            {
                entity.HasKey(e => e.ContractDetailId);
                entity.ToTable("Emp_ContractDetails");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.SerialId).HasColumnName("SerialId");
                entity.Property(t => t.Clause).HasColumnName("Clause");
                modelBuilder.Entity<EmpContractDetail>().HasOne(s => s.EmpContracts).WithMany().HasForeignKey(e => e.ContractId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<EmpContract>(entity =>
            {
                entity.HasKey(e => e.ContractId);
                entity.ToTable("Emp_Contract");
                entity.Property(t => t.ContractCode).HasColumnName("ContractCode");
                entity.Property(t => t.OrgId).HasColumnName("OrgId");
                entity.Property(t => t.CompanyRepresentativeId).HasColumnName("CompanyRepresentativeId");
                entity.Property(t => t.PerSe).HasColumnName("PerSe");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.ContTypeId).HasColumnName("ContTypeId");
                entity.Property(t => t.ContDuration).HasColumnName("ContDuration");
                entity.Property(t => t.StartDatetxt).HasColumnName("StartDatetxt");
                entity.Property(t => t.StartWorkDate).HasColumnName("StartWorkDate");
                entity.Property(t => t.EndWorkDate).HasColumnName("EndWorkDate");
                entity.Property(t => t.EndDatetxt).HasColumnName("EndDatetxt");
                entity.Property(t => t.ProbationDuration).HasColumnName("ProbationDuration");
                entity.Property(t => t.Workingdaysperweek).HasColumnName("Workingdaysperweek");
                entity.Property(t => t.Dailyworkinghours).HasColumnName("Dailyworkinghours");
                entity.Property(t => t.Workinghoursperweek).HasColumnName("Workinghoursperweek");
                entity.Property(t => t.Durationofannualleave).HasColumnName("Durationofannualleave");
                entity.Property(t => t.FreelanceAmount).HasColumnName("FreelanceAmount");
                entity.Property(t => t.Paycase).HasColumnName("Paycase");
                entity.Property(t => t.ProbationTypeId).HasColumnName("ProbationTypeId");
                entity.Property(t => t.Restrictedmode).HasColumnName("Restrictedmode");
                entity.Property(t => t.NotTodivulgeSecrets).HasColumnName("NotTodivulgeSecrets");
                entity.Property(t => t.RestrictionDuration).HasColumnName("RestrictionDuration");
                entity.Property(t => t.Identifyplaces).HasColumnName("Identifyplaces");
                entity.Property(t => t.Withregardtowork).HasColumnName("Withregardtowork");
                entity.Property(t => t.NotTodivulgeSecretsDuration).HasColumnName("NotTodivulgeSecretsDuration");
                entity.Property(t => t.SecretsIdentifyplaces).HasColumnName("SecretsIdentifyplaces");
                entity.Property(t => t.SecretsWithregardtowork).HasColumnName("SecretsWithregardtowork");
                entity.Property(t => t.ContractTerminationNotice).HasColumnName("ContractTerminationNotice");
                entity.Property(t => t.Compensation).HasColumnName("Compensation");
                entity.Property(t => t.CompensationBothParty).HasColumnName("CompensationBothParty");
                entity.Property(t => t.Firstpartycompensation).HasColumnName("Firstpartycompensation");
                entity.Property(t => t.Secondpartycompensation).HasColumnName("Secondpartycompensation");
                entity.Property(t => t.Year).HasColumnName("Year");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.NationalityId).HasColumnName("NationalityId");
                entity.Property(t => t.DailyEmpCost).HasColumnName("DailyEmpCost");


                modelBuilder.Entity<EmpContract>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<EmpContract>().HasOne(s => s.BranchName).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<EmpContract>().HasOne(s => s.NatName).WithMany().HasForeignKey(e => e.NationalityId);
                modelBuilder.Entity<EmpContract>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<EmpContract>().HasOne(s => s.UpdateUsers).WithMany().HasForeignKey(e => e.UpdateUser);
                modelBuilder.Entity<EmpContract>().HasMany<EmpContractDetail>(s => s.EmpContractDetails).WithOne(g => g.EmpContracts).HasForeignKey(s => s.ContractId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<EmpStructure>(entity =>
            {
                entity.HasKey(e => e.StructureId);
                entity.ToTable("Emp_Structure");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.ManagerId).HasColumnName("ManagerId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<EmpStructure>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<EmpStructure>().HasOne(s => s.Managers).WithMany().HasForeignKey(e => e.ManagerId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<EmpStructure>(entity =>
            {
                entity.HasKey(e => e.StructureId);
                entity.ToTable("Emp_Structure");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.ManagerId).HasColumnName("ManagerId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<EmpStructure>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<EmpStructure>().HasOne(s => s.Managers).WithMany().HasForeignKey(e => e.ManagerId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ExpensesGovernment>(entity =>
            {
                entity.HasKey(e => e.ExpensesId);
                entity.ToTable("Emp_ExpensesGov");
                entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.StartHijriDate).HasColumnName("StartHijriDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.EndHijriDate).HasColumnName("EndHijriDate");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Year).HasColumnName("Year");
                entity.Property(t => t.HijriYear).HasColumnName("HijriYear");

                modelBuilder.Entity<ExpensesGovernment>().HasOne(s => s.ExpGovType).WithMany().HasForeignKey(e => e.TypeId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ExpensesGovernmentType>(entity =>
            {
                entity.HasKey(e => e.ExpensesGovernmentTypeId);
                entity.ToTable("Emp_ExpensesTypes");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.ToTable("Emp_Items");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.Quantity).HasColumnName("Quantity");
                entity.Property(t => t.Price).HasColumnName("Price");
                entity.Property(t => t.SachetNo).HasColumnName("SachetNo");
                entity.Property(t => t.FormNo).HasColumnName("FormNo");
                entity.Property(t => t.Color).HasColumnName("Color");
                entity.Property(t => t.IssuancePlace).HasColumnName("IssuancePlace");
                entity.Property(t => t.IssuanceDate).HasColumnName("IssuanceDate");
                entity.Property(t => t.IssuanceHijriDate).HasColumnName("IssuanceHijriDate");
                entity.Property(t => t.IssuanceEndDate).HasColumnName("IssuanceEndDate");
                entity.Property(t => t.IssuanceEndHijriDate).HasColumnName("IssuanceEndHijriDate");
                entity.Property(t => t.SupplyDate).HasColumnName("SupplyDate");
                entity.Property(t => t.SupplyHijriDate).HasColumnName("SupplyHijriDate");
                entity.Property(t => t.PlateNo).HasColumnName("PlateNo");
                entity.Property(t => t.InsuranceNo).HasColumnName("InsuranceNo");
                entity.Property(t => t.InsuranceEndDate).HasColumnName("InsuranceEndDate");
                entity.Property(t => t.InsuranceEndHijriDate).HasColumnName("InsuranceEndHijriDate");
                entity.Property(t => t.LiscenceFileUrl).HasColumnName("LiscenceFileUrl");
                entity.Property(t => t.InsuranceFileUrl).HasColumnName("InsuranceFileUrl");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.Ramainder).HasColumnName("Ramainder");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);
                entity.ToTable("Emp_ItemTypes");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.UserId).HasColumnName("UserId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(e => e.JobId);
                entity.ToTable("Emp_Jobs");
                entity.Property(t => t.JobCode).HasColumnName("Code");
                entity.Property(t => t.JobNameAr).HasColumnName("NameAr");
                entity.Property(t => t.JobNameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<LoanDetails>(entity =>
            {
                entity.HasKey(e => e.LoanDetailsId);
                entity.ToTable("Emp_LoanDetails");
                entity.Property(t => t.LoanId).HasColumnName("LoanId");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.Finished).HasColumnName("Finished");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.SanadId).HasColumnName("SanadId");

                modelBuilder.Entity<LoanDetails>().HasOne(s => s.Loan).WithMany().HasForeignKey(e => e.LoanId);
            });

            //--------------------------------END--------------------------------------------------
            //modelBuilder.Entity<Loan>(entity =>
            //{
            //    entity.HasKey(e => e.LoanId);
            //    entity.ToTable("Emp_Loans");
            //    entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
            //    entity.Property(t => t.Date).HasColumnName("Date");
            //    entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
            //    entity.Property(t => t.Amount).HasColumnName("Amount");
            //    entity.Property(t => t.MonthNo).HasColumnName("MonthNo");
            //    entity.Property(t => t.Money).HasColumnName("Money");
            //    entity.Property(t => t.Note).HasColumnName("Note");
            //    entity.Property(t => t.UserId).HasColumnName("UserId");
            //    entity.Property(t => t.Status).HasColumnName("Status");
            //    entity.Property(t => t.BranchId).HasColumnName("BranchId");
            //    entity.Property(t => t.StartDate).HasColumnName("StartDate");
            //    entity.Property(t => t.StartMonth).HasColumnName("StartMonth");
            //    entity.Property(t => t.AcceptedDate).HasColumnName("AcceptedDate");
            //    entity.Property(t => t.DecisionType).HasColumnName("DecisionType");
            //    entity.Property(t => t.AcceptedUser).HasColumnName("AcceptedUser");
            //    entity.Property(t => t.Isconverted).HasColumnName("Isconverted");

            //    modelBuilder.Entity<Loan>().HasMany<LoanDetails>(s => s.LoanDetails).WithOne(g => g.Loan).HasForeignKey(s => s.LoanId);
            //    modelBuilder.Entity<Loan>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
            //    modelBuilder.Entity<Loan>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmployeeId);
            //    modelBuilder.Entity<Loan>().HasOne(s => s.UserAcccept).WithMany().HasForeignKey(e => e.AcceptedUser);


            //});

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.HasKey(e => e.NationalityId);
                entity.ToTable("Emp_Nationality");
                entity.Property(t => t.NationalityCode).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OfficalHoliday>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Emp_Holidays_Public");
                entity.Property(t => t.FromDate).HasColumnName("FromDate");
                entity.Property(t => t.ToDate).HasColumnName("ToDate");
                entity.Property(t => t.Description).HasColumnName("Description");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<PayrollMarches>(entity =>
            {
                entity.HasKey(e => e.PayrollId);
                entity.ToTable("Emp_PayrollMarches");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.MonthNo).HasColumnName("MonthNo");
                entity.Property(t => t.PostDate).HasColumnName("PostDate");
                entity.Property(t => t.MainSalary).HasColumnName("MainSalary");
                entity.Property(t => t.SalaryOfThisMonth).HasColumnName("SalaryOfThisMonth");
                entity.Property(t => t.Bonus).HasColumnName("Bonus");
                entity.Property(t => t.CommunicationAllawance).HasColumnName("CommunicationAllawance");
                entity.Property(t => t.ProfessionAllawance).HasColumnName("ProfessionAllawance");
                entity.Property(t => t.TransportationAllawance).HasColumnName("TransportationAllawance");
                entity.Property(t => t.HousingAllowance).HasColumnName("HousingAllowance");
                entity.Property(t => t.MonthlyAllowances).HasColumnName("MonthlyAllowances");
                entity.Property(t => t.ExtraAllowances).HasColumnName("ExtraAllowances");
                entity.Property(t => t.TotalRewards).HasColumnName("TotalRewards");
                entity.Property(t => t.TotalDiscounts).HasColumnName("TotalDiscounts");
                entity.Property(t => t.TotalLoans).HasColumnName("TotalLoans");
                entity.Property(t => t.TotalSalaryOfThisMonth).HasColumnName("TotalSalaryOfThisMonth");
                entity.Property(t => t.TotalAbsDays).HasColumnName("TotalAbsDays");
                entity.Property(t => t.TotalVacations).HasColumnName("TotalVacations");
                entity.Property(t => t.IsPostVoucher).HasColumnName("IsPostVoucher");
                entity.Property(t => t.Taamen).HasColumnName("Taamen");
                entity.Property(t => t.IsPostPayVoucher).HasColumnName("IsPostPayVoucher");
                modelBuilder.Entity<PayrollMarches>().HasOne(s => s.Employee).WithMany().HasForeignKey(e => e.EmpId);


            });

            //--------------------------------END--------------------------------------------------


            modelBuilder.Entity<ReasonLeave>(entity =>
            {
                entity.HasKey(e => e.ReasonId);
                entity.ToTable("Emp_ReasonLeave");
                entity.Property(t => t.ReasonTxt).HasColumnName("ReasonTxt");
                entity.Property(t => t.DesecionTxt).HasColumnName("DesecionTxt");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Vacation>(entity =>
            {
                entity.HasKey(e => e.VacationId);
                entity.ToTable("Emp_Vacations");
                entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
                entity.Property(t => t.VacationTypeId).HasColumnName("TypeId");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.StartHijriDate).HasColumnName("StartHijriDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.EndHijriDate).HasColumnName("EndHijriDate");
                entity.Property(t => t.VacationReason).HasColumnName("Reason");
                entity.Property(t => t.VacationStatus).HasColumnName("Status");
                entity.Property(t => t.IsDiscount).HasColumnName("IsDiscount");
                entity.Property(t => t.DiscountAmount).HasColumnName("DiscountAmount");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.AcceptedDate).HasColumnName("AcceptedDate");
                entity.Property(t => t.DaysOfVacation).HasColumnName("DaysOfVacation");
                entity.Property(t => t.DecisionType).HasColumnName("DecisionType");
                entity.Property(t => t.AcceptedUser).HasColumnName("AcceptedUser");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.FileName).HasColumnName("FileName");
                entity.Property(t => t.BackToWorkDate).HasColumnName("BackToWorkDate");

                modelBuilder.Entity<Vacation>().HasOne(s => s.VacationTypeName).WithMany().HasForeignKey(e => e.VacationTypeId);
                modelBuilder.Entity<Vacation>().HasOne(s => s.EmployeeName).WithMany().HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<Vacation>().HasOne(s => s.UserAcccept).WithMany().HasForeignKey(e => e.AcceptedUser);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<VacationType>(entity =>
            {
                entity.HasKey(e => e.VacationTypeId);
                entity.ToTable("Emp_VacationType");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
            });

            //--------------------------------END--------------------------------------------------


            modelBuilder.Entity<DeviceAtt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Sys_DeviceAtt");
                entity.Property(t => t.DeviceId).HasColumnName("DeviceId");
                entity.Property(t => t.LastUpdate).HasColumnName("LastUpdate");
            });

            //--------------------------------END--------------------------------------------------


            modelBuilder.Entity<AttDeviceSitting>(entity =>
            {
                entity.HasKey(e => e.AttDeviceSittingId);
                entity.ToTable("Sys_AttDeviceSitting");
                entity.Property(t => t.ArgCompanyCode).HasColumnName("ArgCompanyCode");
                entity.Property(t => t.ArgEmpUsername).HasColumnName("ArgEmpUsername");
                entity.Property(t => t.ArgEmpPassowrd).HasColumnName("ArgEmpPassowrd");
                entity.Property(t => t.ArgDeviceName).HasColumnName("ArgDeviceName");
            });

            //--------------------------------END--------------------------------------------------

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



    }
}
