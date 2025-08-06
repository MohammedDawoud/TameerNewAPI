using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Models.DBContext
{
    public partial class TaamerProjectContext : DbContext
    {
        public TaamerProjectContext()
        {
        }

        public TaamerProjectContext(DbContextOptions<TaamerProjectContext> options)
            : base(options)
        {
            
        }
        //DbSet Region
        //----------------------------------------------------------------------------
        #region
        //public virtual DbSet<UsersModel> Users { get; set; }

        public virtual DbSet<Acc_Categories> Acc_Categories { get; set; }
        public virtual DbSet<Acc_CategorType> Acc_CategorType { get; set; }
        public virtual DbSet<Acc_Clauses> Acc_Clauses { get; set; }
        public virtual DbSet<Acc_EmpFinYears> Acc_EmpFinYears { get; set; }
        public virtual DbSet<Acc_InvoicesRequests> Acc_InvoicesRequests { get; set; }
        public virtual DbSet<Acc_Floors> Acc_Floors { get; set; }
        public virtual DbSet<Acc_Packages> Acc_Packages { get; set; }
        public virtual DbSet<Acc_Services_Price> Acc_Services_Price { get; set; }
        public virtual DbSet<Acc_Services_PriceOffer> Acc_Services_PriceOffer { get; set; }

        public virtual DbSet<Acc_Suppliers> Acc_Suppliers { get; set; }
        public virtual DbSet<Acc_TotalSpacesRange> Acc_TotalSpacesRange { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AccTransactionTypes> AccTransactionTypes { get; set; }
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
        public virtual DbSet<BackupAlert> BackupAlert { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<BuildTypes> BuildTypes { get; set; }
        public virtual DbSet<CarMovements> CarMovements { get; set; }
        public virtual DbSet<CarMovementsType> CarMovementsType { get; set; }
        public virtual DbSet<ChattingLog> ChattingLog { get; set; }
        public virtual DbSet<Checks> Checks { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CityPass> CityPass { get; set; }
        public virtual DbSet<ContacFiles> ContacFiles { get; set; }
        public virtual DbSet<Contact_Branches> Contact_Branches { get; set; }
        //public virtual DbSet<ContactPerson> ContactPerson { get; set; }
        public virtual DbSet<ContractDetails> ContractDetails { get; set; }
        public virtual DbSet<Contracts> Contracts { get; set; }
        public virtual DbSet<ContractServices> ContractServices { get; set; }
        public virtual DbSet<ContractStage> ContractStage { get; set; }
        public virtual DbSet<CostCenters> CostCenters { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Custody> Custody { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Customer_Branches> Customer_Branches { get; set; }
        public virtual DbSet<CustomerFiles> CustomerFiles { get; set; }
        public virtual DbSet<CustomerMail> CustomerMail { get; set; }
        public virtual DbSet<CustomerPayments> CustomerPayments { get; set; }
        public virtual DbSet<CustomerSMS> CustomerSMS { get; set; }
        public virtual DbSet<DatabaseBackup> DatabaseBackup { get; set; }
        //public virtual DbSet<Dawamyattend> Dawamyattend { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DependencySettings> DependencySettings { get; set; }
        public virtual DbSet<DependencySettingsNew> DependencySettingsNew { get; set; }

        public virtual DbSet<DeviceAtt> DeviceAtt { get; set; }
        public virtual DbSet<DiscountReward> DiscountReward { get; set; }
        public virtual DbSet<Draft> Draft { get; set; }
        public virtual DbSet<DraftDetails> DraftDetails { get; set; }
        public virtual DbSet<Drafts_Templates> Drafts_Templates { get; set; }
        public virtual DbSet<EmailSetting> EmailSetting { get; set; }
        //public virtual DbSet<Emp_SalaryParts> Emp_SalaryParts { get; set; }
        public virtual DbSet<Emp_VacationsStat> Emp_VacationsStat { get; set; }
        public virtual DbSet<EmpContract> EmpContract { get; set; }
        public virtual DbSet<EmpContractDetail> EmpContractDetail { get; set; }
        public virtual DbSet<EmpLocations> EmpLocations { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<EmpStructure> EmpStructure { get; set; }
        public virtual DbSet<ExpensesGovernment> ExpensesGovernment { get; set; }
        public virtual DbSet<ExpensesGovernmentType> ExpensesGovernmentType { get; set; }
        public virtual DbSet<ExpRevenuExpenses> ExpRevenuExpenses { get; set; }
        public virtual DbSet<ExternalEmployees> ExternalEmployees { get; set; }
        public virtual DbSet<FilesAuth> FilesAuth { get; set; }
        public virtual DbSet<FileType> FileType { get; set; }
        public virtual DbSet<FiscalYears> FiscalYears { get; set; }
        public virtual DbSet<FollowProj> FollowProj { get; set; }
        public virtual DbSet<FullProjectsReport> FullProjectsReport { get; set; }
        public virtual DbSet<GroupPrivileges> GroupPrivileges { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Guarantees> Guarantees { get; set; }
        public virtual DbSet<GuideDepartmentDetails> GuideDepartmentDetails { get; set; }
        public virtual DbSet<GuideDepartments> GuideDepartments { get; set; }
        public virtual DbSet<ImportantProject> ImportantProject { get; set; }
        public virtual DbSet<Instruments> Instruments { get; set; }
        public virtual DbSet<InstrumentSources> InstrumentSources { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Journals> Journals { get; set; }
        public virtual DbSet<Licences> Licences { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<LoanDetails> LoanDetails { get; set; }
        //public virtual DbSet<MachineInfo> MachineInfo { get; set; }
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<ModelRequirements> ModelRequirements { get; set; }
        public virtual DbSet<ModelType> ModelType { get; set; }
        public virtual DbSet<Nationality> Nationality { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NodeLocations> NodeLocations { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationSettings> NotificationSettings { get; set; }
        public virtual DbSet<OffersConditions> OffersConditions { get; set; }
        public virtual DbSet<OfferService> OfferService { get; set; }
        public virtual DbSet<OffersPrices> OffersPrices { get; set; }
        public virtual DbSet<OfficalHoliday> OfficalHoliday { get; set; }
        public virtual DbSet<OfficialDocuments> OfficialDocuments { get; set; }
        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<OutInBox> OutInBox { get; set; }
        public virtual DbSet<OutInBoxSerial> OutInBoxSerial { get; set; }
        public virtual DbSet<OutInBoxType> OutInBoxType { get; set; }
        public virtual DbSet<OutInImagesTo> OutInImagesTo { get; set; }
        public virtual DbSet<OutMovements> OutMovements { get; set; }
        public virtual DbSet<PayrollMarches> PayrollMarches { get; set; }
        public virtual DbSet<PrivFollowers> PrivFollowers { get; set; }
        public virtual DbSet<Pro_Municipal> Pro_Municipal { get; set; }
        public virtual DbSet<Pro_SubMunicipality> Pro_SubMunicipality { get; set; }
        public virtual DbSet<Pro_Super_PhaseDet> Pro_Super_PhaseDet { get; set; }
        public virtual DbSet<Pro_Super_Phases> Pro_Super_Phases { get; set; }
        public virtual DbSet<Pro_SuperContractor> Pro_SuperContractor { get; set; }
        public virtual DbSet<Pro_SupervisionDetails> Pro_SupervisionDetails { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectArchivesRe> ProjectArchivesRe { get; set; }
        public virtual DbSet<ProjectArchivesSee> ProjectArchivesSee { get; set; }
        public virtual DbSet<ProjectComments> ProjectComments { get; set; }
        public virtual DbSet<ProjectExtracts> ProjectExtracts { get; set; }
        public virtual DbSet<ProjectFiles> ProjectFiles { get; set; }
        public virtual DbSet<ProjectPhasesTasks> ProjectPhasesTasks { get; set; }     
        public virtual DbSet<Pro_projectsReasons> Pro_projectsReasons { get; set; }
        public virtual DbSet<Pro_TaskOperations> Pro_TaskOperations { get; set; }
        public virtual DbSet<Pro_tasksReasons> Pro_tasksReasons { get; set; }
        public virtual DbSet<Pro_DestinationDepartments> Pro_DestinationDepartments { get; set; }
        public virtual DbSet<Pro_DestinationTypes> Pro_DestinationTypes { get; set; }
        public virtual DbSet<Pro_filesReasons> Pro_filesReasons { get; set; }
        public virtual DbSet<Pro_Destinations> Pro_Destinations { get; set; }

        public virtual DbSet<ProjectPieces> ProjectPieces { get; set; }
        public virtual DbSet<ProjectRequirements> ProjectRequirements { get; set; }
        public virtual DbSet<ProjectRequirementsGoals> ProjectRequirementsGoals { get; set; }
        public virtual DbSet<ProjectSubTypes> ProjectSubTypes { get; set; }
        public virtual DbSet<ProjectTrailing> ProjectTrailing { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<ProjectWorkers> ProjectWorkers { get; set; }
        public virtual DbSet<ProSettingDetails> ProSettingDetails { get; set; }
        public virtual DbSet<ProSettingDetailsNew> ProSettingDetailsNew { get; set; }

        public virtual DbSet<ProUserPrivileges> ProUserPrivileges { get        ; set; }
public virtual DbSet<Pro_ProjectSteps> Pro_ProjectSteps { get; set; }
        public virtual DbSet<Pro_StepDetails> Pro_StepDetails { get; set; }
        public virtual DbSet<Pro_ProjectAchievements> Pro_ProjectAchievements { get; set; }
        public virtual DbSet<Pro_ProjectChallenges> Pro_ProjectChallenges { get; set; }

        public virtual DbSet<ReasonLeave> ReasonLeave { get; set; }
        public virtual DbSet<RegionTypes> RegionTypes { get; set; }
        public virtual DbSet<Requirements> Requirements { get; set; }
        public virtual DbSet<RequirementsandGoals> RequirementsandGoals { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServicesPricingForm> ServicesPricingForm { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<SettingsNew> SettingsNew { get; set; }

        public virtual DbSet<SMSSettings> SMSSettings { get; set; }
        public virtual DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public virtual DbSet<Supervisions> Supervisions { get; set; }
        public virtual DbSet<SupportResquests> SupportResquests { get; set; }
        public virtual DbSet<Sys_SystemActions> Sys_SystemActions { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
        public virtual DbSet<TasksDependency> TasksDependency { get; set; }
        public virtual DbSet<TaskType> TaskType { get; set; }
        public virtual DbSet<Templates> Templates { get; set; }
        public virtual DbSet<TemplatesTasks> TemplatesTasks { get; set; }
        public virtual DbSet<TimeOutRequests> TimeOutRequests { get; set; }
        public virtual DbSet<TrailingFiles> TrailingFiles { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<TransactionTypes> TransactionTypes { get; set; }
        public virtual DbSet<UserBranches> UserBranches { get; set; }
        //public virtual DbSet<UserIDInfo> UserIDInfo { get; set; }
        public virtual DbSet<UserNotificationPrivileges> UserNotificationPrivileges { get; set; }
        public virtual DbSet<UserPrivileges> UserPrivileges { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersLocations> UsersLocations { get; set; }
        public virtual DbSet<UserMails> UserMails { get; set; }
        public virtual DbSet<Vacation> Vacation { get; set; }
        public virtual DbSet<VacationType> VacationType { get; set; }
        public virtual DbSet<Versions> Versions { get; set; }
        public virtual DbSet<VoucherDetails> VoucherDetails { get; set; }
        public virtual DbSet<VoucherSettings> VoucherSettings { get; set; }
        public virtual DbSet<WorkOrders> WorkOrders { get; set; }
        public virtual DbSet<WhatsAppSettings> WhatsAppSettings { get; set; }

        public virtual DbSet<Guide_QuestionsAnswers> Guide_QuestionsAnswers { get; set; }
        public virtual DbSet<SupportRequestsReplay> RequestsReplays { get; set; }

        public virtual DbSet<Emp_AbsenceList> AbsenceLists { get; set; }
        public virtual DbSet<Emp_LateList> LateLists { get; set; }
        public virtual DbSet<ContactList> ContactLists { get; set; }
        public virtual DbSet<AttendenceLocationSettings> AttendenceLocations { get; set; }
        public virtual DbSet<Exceptions> Exceptions { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<PermissionType> PermissionTypes { get; set; }
        public virtual DbSet<CommercialActivity> CommercialActivities { get; set; }
        public virtual DbSet<NotificationConfiguration> NotificationConfigurations { get; set; }
        public virtual DbSet<NotificationConfigurationsAssines> NotificationConfigurationsAssines { get; set; }

        public string GetDatabaseName()
        {
            return Database.GetDbConnection().Database;
        }
        //----------------------------------------------------------------------------

        #endregion



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection",
                options => options.EnableRetryOnFailure(
                     maxRetryCount: 5,
                     maxRetryDelay: System.TimeSpan.FromSeconds(30),
                     errorNumbersToAdd: null));
                //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder Entity Region
            //-------------------------------------------------------------------------------------
            #region
            //modelBuilder.Entity<UsersModel>(entity =>
            //{
            //    entity.HasKey(e => e.UserId);
            //    entity.ToTable("Sys_Users");
            //    entity.Property(e => e.AddDate).HasColumnType("datetime");
            //    entity.Property(e => e.DeleteDate).HasColumnType("datetime");
            //    entity.Property(e => e.Email).HasMaxLength(100);
            //    entity.Property(e => e.Mobile).HasMaxLength(50);
            //    entity.Property(e => e.Password).HasMaxLength(150);
            //    entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            //    entity.Property(e => e.UserName).HasMaxLength(150);
            //    entity.Property(e => e.DeviceTokenId).HasColumnName("DeviceTokenId");
            //    entity.Property(e => e.DeviceType).HasColumnName("DeviceType");
            //   // modelBuilder.Entity<Users>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
            //});


            //--------------------------------END--------------------------------------------------
            //modelBuilder.Entity<Auditable>(entity =>
            //{
            //    entity.Property(e => e.AddUser).HasColumnName("AddUser");
            //    entity.Property(e => e.UpdateUser).HasColumnName("UpdateUser");
            //    entity.Property(e => e.DeleteUser).HasColumnName("DeleteUser");
            //    entity.Property(e => e.AddDate).HasColumnName("AddDate");
            //    entity.Property(e => e.UpdateDate).HasColumnName("UpdateDate");
            //    entity.Property(e => e.DeleteDate).HasColumnName("DeleteDate");
            //    entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            //});

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.ToTable("Acc_Categories");
                entity.Property(e => e.NAmeAr).HasColumnName("NAmeAr");
                entity.Property(e => e.NAmeEn).HasColumnName("NAmeEn");
                entity.Property(e => e.Price).HasColumnName("Price");
                entity.Property(e => e.Code).HasColumnName("Code");
                entity.Property(e => e.Note).HasColumnName("Note");
                entity.Property(e => e.CategorTypeId).HasColumnName("CategorTypeId");
                entity.Property(e => e.AccountId).HasColumnName("AccountId");

                modelBuilder.Entity<Acc_Categories>().HasOne(s => s.CategorType).WithMany().HasForeignKey(e => e.CategorTypeId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_CategorType>(entity =>
            {
                entity.HasKey(e => e.CategorTypeId);
                entity.ToTable("Acc_CategorType");
                entity.Property(e => e.NAmeAr).HasColumnName("NAmeAr");
                entity.Property(e => e.NAmeEn).HasColumnName("NAmeEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Clauses>(entity =>
            {
                entity.HasKey(e => e.ClauseId);
                entity.ToTable("Acc_Clauses");
                entity.Property(e => e.NameAr).HasColumnName("NameAr");
                entity.Property(e => e.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_EmpFinYears>(entity =>
            {
                entity.HasKey(e => e.Acc_EmpFinYearID);
                entity.ToTable("Acc_EmpFinYears");
                entity.Property(e => e.EmpID).HasColumnName("EmpID");
                entity.Property(e => e.BranchID).HasColumnName("BranchID");
                entity.Property(e => e.YearID).HasColumnName("YearID");
                modelBuilder.Entity<Acc_EmpFinYears>().HasOne(s => s.branch).WithMany().HasForeignKey(e => e.BranchID);
                modelBuilder.Entity<Acc_EmpFinYears>().HasOne(s => s.fiscalYears).WithMany().HasForeignKey(e => e.YearID);
                modelBuilder.Entity<Acc_EmpFinYears>().HasOne(s => s.user).WithMany().HasForeignKey(e => e.EmpID);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Floors>(entity =>
            {
                entity.HasKey(e => e.FloorId);
                entity.ToTable("Acc_Floors");
                entity.Property(e => e.FloorName).HasColumnName("FloorName");
                entity.Property(e => e.FloorRatio).HasColumnName("FloorRatio");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Packages>(entity =>
            {
                entity.HasKey(e => e.PackageId);
                entity.ToTable("Acc_Packages");
                entity.Property(e => e.PackageName).HasColumnName("PackageName");
                entity.Property(e => e.MeterPrice1).HasColumnName("MeterPrice1");
                entity.Property(e => e.MeterPrice2).HasColumnName("MeterPrice2");
                entity.Property(e => e.MeterPrice3).HasColumnName("MeterPrice3");
                entity.Property(e => e.PackageRatio1).HasColumnName("PackageRatio1");
                entity.Property(e => e.PackageRatio2).HasColumnName("PackageRatio2");
                entity.Property(e => e.PackageRatio3).HasColumnName("PackageRatio3");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId);
                entity.ToTable("Acc_Suppliers");
                entity.Property(e => e.NameAr).HasColumnName("NameAr");
                entity.Property(e => e.NameEn).HasColumnName("NameEn");
                entity.Property(e => e.TaxNo).HasColumnName("TaxNo");
                entity.Property(e => e.PhoneNo).HasColumnName("PhoneNo");
                entity.Property(e => e.PhoneNo).HasColumnName("PhoneNo");
                entity.Property(e => e.AccountId).HasColumnName("AccountId");
                entity.Property(e => e.CompAddress).HasColumnName("CompAddress");
                entity.Property(e => e.PostalCodeFinal).HasColumnName("PostalCodeFinal");
                entity.Property(e => e.ExternalPhone).HasColumnName("ExternalPhone");
                entity.Property(e => e.Country).HasColumnName("Country");
                entity.Property(e => e.Neighborhood).HasColumnName("Neighborhood");
                entity.Property(e => e.StreetName).HasColumnName("StreetName");
                entity.Property(e => e.BuildingNumber).HasColumnName("BuildingNumber");
                entity.Property(e => e.CityId).HasColumnName("CityId");
                modelBuilder.Entity<Acc_Suppliers>().HasOne(s => s.city).WithMany().HasForeignKey(e => e.CityId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_TotalSpacesRange>(entity =>
            {
                entity.HasKey(e => e.TotalSpacesRangeId);
                entity.ToTable("Acc_TotalSpacesRange");
                entity.Property(e => e.TotalSpacesRengeName).HasColumnName("TotalSpacesRengeName");
                entity.Property(e => e.RangeValue).HasColumnName("RangeValue");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.HasKey(e => e.AccountId);
                entity.ToTable("Acc_Accounts");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Nature).HasColumnName("Nature");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.Level).HasColumnName("AccountLevel");
                entity.Property(t => t.Classification).HasColumnName("Classification");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Code).HasColumnName("AccountCode");
                entity.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
                entity.Property(t => t.TransferedAccId).HasColumnName("TransferedAccId");
                entity.Property(t => t.IsMainCustAcc).HasColumnName("IsMainCustAcc");
                entity.Property(t => t.Active).HasColumnName("Active");
                entity.Property(t => t.Balance).HasColumnName("Balance");
                entity.Property(t => t.OpeningBalance).HasColumnName("OpeningBalance");
                entity.Property(t => t.ExpensesAccId).HasColumnName("ExpensesAccId");
                entity.Property(t => t.AccountIdAhlak).HasColumnName("AccountIdAhlak");
                entity.Property(t => t.OpenAccCredit).HasColumnName("OpenAccCredit");
                entity.Property(t => t.OpenAccDepit).HasColumnName("OpenAccDepit");
                entity.Property(t => t.AccountCodeNew).HasColumnName("AccountCodeNew");
                entity.Property(t => t.PublicRev).HasColumnName("PublicRev");
                entity.Property(t => t.OtherRev).HasColumnName("OtherRev");
                entity.Property(t => t.OpenAccCreditDate).HasColumnName("OpenAccCreditDate");
                entity.Property(t => t.OpenAccDepitDate).HasColumnName("OpenAccDepitDate");

                modelBuilder.Entity<Accounts>().HasOne(s => s.ParentAccount).WithMany().HasForeignKey(e => e.ParentId).IsRequired(false);
                modelBuilder.Entity<Accounts>().HasMany<Transactions>(s => s.Transactions).WithOne(g => g.Accounts).HasForeignKey(s => s.AccountId).IsRequired(false); 
                modelBuilder.Entity<Accounts>().HasMany<Accounts>(s => s.ChildsAccount).WithOne(g => g.ParentAccount).HasForeignKey(s => s.ParentId).IsRequired(false); 

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Services_Price>(entity =>
            {
                entity.HasKey(e => e.ServicesId);
                entity.ToTable("Acc_Services_Price");
                entity.Property(t => t.ServicesName).HasColumnName("ServicesName");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.AccountName).HasColumnName("AccountName");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.ProjectSubTypeID).HasColumnName("ProjectSubTypeID");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.CostCenterId).HasColumnName("CostCenterId");
                entity.Property(t => t.PackageId).HasColumnName("PackageId");
                entity.Property(t => t.ServiceName_EN).HasColumnName("ServiceName_EN");
                entity.Property(t => t.ServiceType).HasColumnName("ServiceType");

                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.AccountParentId).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.ProjectParentId).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjectSubTypeID);
                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.Parent).WithMany().HasForeignKey(e => e.ParentId);
                modelBuilder.Entity<Acc_Services_Price>().HasMany<OfferService>(s => s.OfferService).WithOne(g => g.serviceprice).HasForeignKey(s => s.ServiceId);
                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.Package).WithMany().HasForeignKey(e => e.PackageId);
                modelBuilder.Entity<Acc_Services_Price>().HasMany<ContractServices>(s => s.ContractServices).WithOne(g => g.serviceprice).HasForeignKey(s => s.ServiceId);
                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.servicepriceoffer).WithMany().HasForeignKey(e => e.ParentId);

                //modelBuilder.Entity<Acc_Services_Price>().HasOne(t => t.servicepriceoffer).WithOne(t => t.ServiceDetails).HasForeignKey<Acc_Services_Price>(t => t.ServicesId).IsRequired();

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_Services_PriceOffer>(entity =>
            {
                entity.HasKey(e => e.ServicesIdVou);
                entity.ToTable("Acc_Services_PriceOffer");
                entity.Property(t => t.OfferId).HasColumnName("OfferId");
                entity.Property(t => t.ServicesId).HasColumnName("ServicesId");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.SureService).HasColumnName("SureService");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");

                modelBuilder.Entity<Acc_Services_PriceOffer>().HasOne(s => s.ServiceDetails).WithMany().HasForeignKey(e => e.ServicesId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<AccTransactionTypes>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId);
                entity.ToTable("Acc_TransactionTypes");
                entity.Property(e => e.NameAr).HasColumnName("NameAr");
                entity.Property(e => e.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
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
                modelBuilder.Entity<Allowance>().HasOne(s => s.Employees).WithMany(s=>s.Allowance).HasForeignKey(e => e.EmployeeId);

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
            modelBuilder.Entity<ArchiveFiles>(entity =>
            {
                entity.HasKey(e => e.ArchiveFileId);
                entity.ToTable("Contac_ArchiveFiles");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
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
                modelBuilder.Entity<AttAbsentDay>().HasOne(s => s.Employees).WithMany(s=>s.AttAbsentDays).HasForeignKey(e => e.EmpId);

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
                modelBuilder.Entity<AttTimeDetails>().HasOne(s => s.AttendaceTime).WithMany(s=>s.AttTimeDetails).HasForeignKey(e => e.AttTimeId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<BackupAlert>(entity =>
            {
                entity.HasKey(e => e.AlertId);
                entity.ToTable("Sys_BackupAlert");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.AlertSms).HasColumnName("AlertSms");
                entity.Property(t => t.AlertTimeType).HasColumnName("AlertTimeType");
                entity.Property(t => t.AlertTime).HasColumnName("AlertTime");
                entity.Property(t => t.AlertNextTime).HasColumnName("AlertNextTime");
                entity.Property(t => t.Alert_IsSent).HasColumnName("Alert_IsSent");
                modelBuilder.Entity<BackupAlert>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Banks>(entity =>
            {
                entity.HasKey(e => e.BankId);
                entity.ToTable("Acc_Banks");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BanckLogo).HasColumnName("BanckLogo");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.BranchId);
                entity.ToTable("Sys_Branches");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchManager).HasColumnName("BranchManager");
                entity.Property(t => t.Phone).HasColumnName("Phone");
                entity.Property(t => t.Mobile).HasColumnName("Mobile");
                entity.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
                entity.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
                entity.Property(t => t.BoxAccId).HasColumnName("BoxAccId");
                entity.Property(t => t.StockAccd).HasColumnName("StockAccd");
                entity.Property(t => t.SaleCostAccId).HasColumnName("SaleCostAccId");
                entity.Property(t => t.SaleCashAccId).HasColumnName("SaleCashAccId");
                entity.Property(t => t.SaleDelayAccId).HasColumnName("SaleDelayAccId");
                entity.Property(t => t.SaleDiscountAccId).HasColumnName("SaleDiscountAccId");
                entity.Property(t => t.SaleReturnCashAccId).HasColumnName("SaleReturnCashAccId");
                entity.Property(t => t.SaleReturnDelayAccId).HasColumnName("SaleReturnDelayAccId");
                entity.Property(t => t.SaleReturnDiscountAccId).HasColumnName("SaleReturnDiscountAccId");
                entity.Property(t => t.PurchaseCashAccId).HasColumnName("PurchaseCashAccId");
                entity.Property(t => t.PurchaseDelayAccId).HasColumnName("PurchaseDelayAccId");
                entity.Property(t => t.PurchaseApprovalAccId).HasColumnName("PurchaseApprovalAccId");
                entity.Property(t => t.PurchaseOutCashAccId).HasColumnName("PurchaseOutCashAccId");
                entity.Property(t => t.PurchaseOutDelayAccId).HasColumnName("PurchaseOutDelayAccId");
                entity.Property(t => t.PurchaseDiscAccId).HasColumnName("PurchaseDiscAccId");
                entity.Property(t => t.PurchaseReturnCashAccId).HasColumnName("PurchaseReturnCashAccId");
                entity.Property(t => t.PurchaseReturnDelayAccId).HasColumnName("PurchaseReturnDelayAccId");
                entity.Property(t => t.PurchaseApprovalAccId).HasColumnName("PurchaseApprovalAccId");
                entity.Property(t => t.PurchaseReturnDiscAccId).HasColumnName("PurchaseReturnDiscAccId");
                entity.Property(t => t.RevenuesAccountId).HasColumnName("RevenuesAccountId");
                entity.Property(t => t.SuspendedFundAccId).HasColumnName("SuspendedFundAccId");
                entity.Property(t => t.CashInvoicesAccId).HasColumnName("CashInvoicesAccId");
                entity.Property(t => t.DelayInvoicesAccId).HasColumnName("DelayInvoicesAccId");
                entity.Property(t => t.DiscountInvoicesAccId).HasColumnName("DiscountInvoicesAccId");
                entity.Property(t => t.CashReturnInvoicesAccId).HasColumnName("CashReturnInvoicesAccId");
                entity.Property(t => t.DelayReturnInvoicesAccId).HasColumnName("DelayReturnInvoicesAccId");
                entity.Property(t => t.DiscountReturnInvoiceAccId).HasColumnName("DiscountReturnInvoiceAccId");
                entity.Property(t => t.CheckInvoicesAccId).HasColumnName("CheckInvoicesAccId");
                entity.Property(t => t.VisaInvoicesAccId).HasColumnName("VisaInvoicesAccId");
                entity.Property(t => t.TeleInvoicesAccId).HasColumnName("TeleInvoicesAccId");
                entity.Property(t => t.AmericanAccId).HasColumnName("AmericanAccId");
                entity.Property(t => t.CustomersAccId).HasColumnName("CustomersAccId");
                entity.Property(t => t.SuppliersAccId).HasColumnName("SuppliersAccId");
                entity.Property(t => t.EmployeesAccId).HasColumnName("EmployeesAccId");
                entity.Property(t => t.GuaranteeAccId).HasColumnName("GuaranteeAccId");
                entity.Property(t => t.ContractsAccId).HasColumnName("ContractsAccId");
                entity.Property(t => t.TaxsAccId).HasColumnName("TaxsAccId");
                entity.Property(t => t.EngineeringLicense).HasColumnName("EngineeringLicense");
                entity.Property(t => t.LabLicense).HasColumnName("LabLicense");
                entity.Property(t => t.Mailbox).HasColumnName("Mailbox");
                entity.Property(t => t.CityId).HasColumnName("CityId");
                entity.Property(t => t.LastExport).HasColumnName("LastExport");
                entity.Property(t => t.LastExportInner).HasColumnName("LastExportInner");
                entity.Property(t => t.IsActive).HasColumnName("IsActive");
                entity.Property(t => t.LoanAccId).HasColumnName("LoanAccId");
                entity.Property(t => t.BoxAccId2).HasColumnName("BoxAccId2");
                entity.Property(t => t.ProjectStartCode).HasColumnName("ProjectStartCode");
                entity.Property(t => t.OfferStartCode).HasColumnName("OfferStartCode");
                entity.Property(t => t.TaskStartCode).HasColumnName("TaskStartCode");
                entity.Property(t => t.OrderStartCode).HasColumnName("OrderStartCode");
                entity.Property(t => t.InvoiceStartCode).HasColumnName("InvoiceStartCode");
                entity.Property(t => t.InvoiceBranchSeparated).HasColumnName("InvoiceBranchSeparated");
                entity.Property(t => t.Engineering_License).HasColumnName("Engineering_License");
                entity.Property(t => t.Engineering_LicenseDate).HasColumnName("Engineering_LicenseDate");
                entity.Property(t => t.IsPrintInvoice).HasColumnName("IsPrintInvoice");
                entity.Property(t => t.BranchLogoUrl).HasColumnName("BranchLogoUrl");
                entity.Property(t => t.HeaderLogoUrl).HasColumnName("HeaderLogoUrl");
                entity.Property(t => t.FooterLogoUrl).HasColumnName("FooterLogoUrl");
                entity.Property(t => t.headerPrintInvoice).HasColumnName("headerPrintInvoice");
                entity.Property(t => t.headerPrintrevoucher).HasColumnName("headerPrintrevoucher");
                entity.Property(t => t.headerprintdarvoucher).HasColumnName("headerprintdarvoucher");
                entity.Property(t => t.headerPrintpayvoucher).HasColumnName("headerPrintpayvoucher");
                entity.Property(t => t.headerPrintcontract).HasColumnName("headerPrintcontract");
                entity.Property(t => t.BublicRevenue).HasColumnName("BublicRevenue");
                entity.Property(t => t.OtherRevenue).HasColumnName("OtherRevenue");
                entity.Property(t => t.AccountBank).HasColumnName("AccountBank");
                entity.Property(t => t.PostalCodeFinal).HasColumnName("PostalCodeFinal");
                entity.Property(t => t.ExternalPhone).HasColumnName("ExternalPhone");
                entity.Property(t => t.Country).HasColumnName("Country");
                entity.Property(t => t.Neighborhood).HasColumnName("Neighborhood");
                entity.Property(t => t.StreetName).HasColumnName("StreetName");
                entity.Property(t => t.BuildingNumber).HasColumnName("BuildingNumber");
                entity.Property(t => t.AccountBank2).HasColumnName("AccountBank2");
                entity.Property(t => t.Address).HasColumnName("Address");
                entity.Property(t => t.TaxCode).HasColumnName("TaxCode");
                entity.Property(t => t.PostalCode).HasColumnName("PostalCode");
                entity.Property(t => t.BankId).HasColumnName("BankId");
                entity.Property(t => t.BankId2).HasColumnName("BankId2");
                entity.Property(t => t.CSR).HasColumnName("CSR");
                entity.Property(t => t.PrivateKey).HasColumnName("PrivateKey");
                entity.Property(t => t.PublicKey).HasColumnName("PublicKey");
                entity.Property(t => t.SecreteKey).HasColumnName("SecreteKey");

                modelBuilder.Entity<Branch>().HasOne(s => s.City).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Currency).WithMany().HasForeignKey(e => e.CurrencyId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Organizations).WithMany().HasForeignKey(e => e.OrganizationId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Bank).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Bank2).WithMany().HasForeignKey(e => e.BankId2);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<BuildTypes>(entity =>
            {
                entity.HasKey(e => e.BuildTypeId);
                entity.ToTable("Pro_BuildTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Description).HasColumnName("Description");
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
            modelBuilder.Entity<ChattingLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.ToTable("Sys_ChattingLog");
                entity.Property(t => t.ReceivedUserId).HasColumnName("ReceivedUserId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Body).HasColumnName("Body");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                modelBuilder.Entity<ChattingLog>().HasOne(s => s.SenderUser).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ChattingLog>().HasOne(s => s.ReceiveUsers).WithMany().HasForeignKey(e => e.ReceivedUserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Checks>(entity =>
            {
                entity.HasKey(e => e.CheckId);
                entity.ToTable("Acc_Checks");
                entity.Property(t => t.CheckNumber).HasColumnName("CheckNumber");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.ActionDate).HasColumnName("ActionDate");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.BankId).HasColumnName("BankId");
                entity.Property(t => t.BeneficiaryName).HasColumnName("BeneficiaryName");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.ReceivedName).HasColumnName("ReceivedName");
                entity.Property(t => t.TotalAmont).HasColumnName("TotalAmont");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.IsFinished).HasColumnName("IsFinished");
                modelBuilder.Entity<Checks>().HasOne(s => s.Banks).WithMany().HasForeignKey(e => e.BankId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.ToTable("Pro_Cities");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CityPass>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.ToTable("Pro_CitiesPass");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ContacFiles>(entity =>
            {
                entity.HasKey(e => e.FileId);
                entity.ToTable("Contac_Files");
                entity.Property(t => t.FileName).HasColumnName("FileName");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.Extension).HasColumnName("Extension");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.IsCertified).HasColumnName("IsCertified");
                entity.Property(t => t.OutInBoxId).HasColumnName("OutInBoxId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.DeleteUrl).HasColumnName("DeleteUrl");
                entity.Property(t => t.ThumbnailUrl).HasColumnName("ThumbnailUrl");
                entity.Property(t => t.DeleteType).HasColumnName("DeleteType");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UploadDate).HasColumnName("UploadDate");
                modelBuilder.Entity<ContacFiles>().HasOne(s => s.OutInBox).WithMany().HasForeignKey(e => e.OutInBoxId);
                modelBuilder.Entity<ContacFiles>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Contact_Branches>(entity =>
            {
                entity.HasKey(e => e.ContactId);
                entity.ToTable("Sys_Contact_Branches");
                entity.Property(t => t.BranchName).HasColumnName("BranchName");
                entity.Property(t => t.BranchAddress).HasColumnName("BranchAddress");
                entity.Property(t => t.BranchPhone).HasColumnName("BranchPhone");
                entity.Property(t => t.BranchCS).HasColumnName("BranchCS");
                entity.Property(t => t.BranchEmail).HasColumnName("BranchEmail");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ContractDetails>(entity =>
            {
                entity.HasKey(e => e.ContractDetailId);
                entity.ToTable("Pro_ContractDetails");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.SerialId).HasColumnName("SerialId");
                entity.Property(t => t.Clause).HasColumnName("Clause");
                modelBuilder.Entity<ContractDetails>().HasOne(s => s.Contracts).WithMany(g => g.ContractDetails).HasForeignKey(e => e.ContractId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Contracts>(entity =>
            {
                entity.HasKey(e => e.ContractId);
                entity.ToTable("Pro_Contracts");
                entity.Property(t => t.ContractNo).HasColumnName("ContractNo");
                entity.Property(t => t.Value).HasColumnName("Value");
                entity.Property(t => t.InstrumentNo).HasColumnName("InstrumentNo");
                entity.Property(t => t.InstrumentDate).HasColumnName("InstrumentDate");
                entity.Property(t => t.TaxType).HasColumnName("TaxType");
                entity.Property(t => t.TaxesValue).HasColumnName("TaxesValue");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.CityId).HasColumnName("CityId");
                entity.Property(t => t.District).HasColumnName("District");
                entity.Property(t => t.ValueText).HasColumnName("ValueText");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.TotalValue).HasColumnName("TotalValue");
                entity.Property(t => t.TotalValuetxt).HasColumnName("TotalValuetxt");
                entity.Property(t => t.OrgId).HasColumnName("OrgId");
                entity.Property(t => t.OrgEmpId).HasColumnName("OrgEmpId");
                entity.Property(t => t.OrgEmpJobId).HasColumnName("OrgEmpJobId");
                entity.Property(t => t.ServiceId).HasColumnName("ServiceId");
                entity.Property(t => t.AttachmentUrlExtra).HasColumnName("AttachmentUrlExtra");
                entity.Property(t => t.Engineering_License).HasColumnName("Engineering_License");
                entity.Property(t => t.Engineering_LicenseDate).HasColumnName("Engineering_LicenseDate");
                entity.Property(t => t.Appr_LetterDate_Des).HasColumnName("Appr_LetterDate_Des");
                entity.Property(t => t.EngServ_OfferDate_Des).HasColumnName("EngServ_OfferDate_Des");
                entity.Property(t => t.MaxPay_Des).HasColumnName("MaxPay_Des");
                entity.Property(t => t.ContractDurCommit_Des).HasColumnName("ContractDurCommit_Des");
                entity.Property(t => t.ContPeriod_Des).HasColumnName("ContPeriod_Des");
                entity.Property(t => t.TeamWork_Num1_Des).HasColumnName("TeamWork_Num1_Des");
                entity.Property(t => t.TeamWork_Note1_Des).HasColumnName("TeamWork_Note1_Des");
                entity.Property(t => t.TeamWork_Num2_Des).HasColumnName("TeamWork_Num2_Des");
                entity.Property(t => t.TeamWork_Note2_Des).HasColumnName("TeamWork_Note2_Des");
                entity.Property(t => t.TeamWork_Num3_Des).HasColumnName("TeamWork_Num3_Des");
                entity.Property(t => t.TeamWork_Note3_Des).HasColumnName("TeamWork_Note3_Des");
                entity.Property(t => t.TeamWork_Num4_Des).HasColumnName("TeamWork_Num4_Des");
                entity.Property(t => t.TeamWork_Note4_Des).HasColumnName("TeamWork_Note4_Des");
                entity.Property(t => t.TeamWork_Num5_Des).HasColumnName("TeamWork_Num5_Des");
                entity.Property(t => t.TeamWork_Note5_Des).HasColumnName("TeamWork_Note5_Des");
                entity.Property(t => t.TeamWork_Num6_Des).HasColumnName("TeamWork_Num6_Des");
                entity.Property(t => t.TeamWork_Note6_Des).HasColumnName("TeamWork_Note6_Des");
                entity.Property(t => t.TeamWork_Num7_Des).HasColumnName("TeamWork_Num7_Des");
                entity.Property(t => t.TeamWork_Note7_Des).HasColumnName("TeamWork_Note7_Des");
                entity.Property(t => t.TeamWork_Num8_Des).HasColumnName("TeamWork_Num8_Des");
                entity.Property(t => t.TeamWork_Note8_Des).HasColumnName("TeamWork_Note8_Des");
                entity.Property(t => t.TeamWork_Num9_Des).HasColumnName("TeamWork_Num9_Des");
                entity.Property(t => t.TeamWork_Note9_Des).HasColumnName("TeamWork_Note9_Des");
                entity.Property(t => t.TeamWork_Num10_Des).HasColumnName("TeamWork_Num10_Des");
                entity.Property(t => t.TeamWork_Note10_Des).HasColumnName("TeamWork_Note10_Des");
                entity.Property(t => t.Cons_TotalFees_Des).HasColumnName("Cons_TotalFees_Des");
                entity.Property(t => t.ContractorName_Des).HasColumnName("ContractorName_Des");
                entity.Property(t => t.ContDate_Des).HasColumnName("ContDate_Des");
                entity.Property(t => t.ProjBriefDesc_Des).HasColumnName("ProjBriefDesc_Des");
                entity.Property(t => t.Oper_expeValue).HasColumnName("Oper_expeValue");

                modelBuilder.Entity<Contracts>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId).OnDelete(DeleteBehavior.SetNull);
                modelBuilder.Entity<Contracts>().HasMany<CustomerPayments>(s => s.CustomerPayments).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId).OnDelete(DeleteBehavior.SetNull);

                //modelBuilder.Entity<Contracts>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                //modelBuilder.Entity<Contracts>().HasMany<CustomerPayments>(s => s.CustomerPayments).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Contracts>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Contracts>().HasOne(s => s.Service).WithMany(g => g.Contracts).HasForeignKey(e => e.ServiceId);
                modelBuilder.Entity<Contracts>().HasMany<ContractDetails>(s => s.ContractDetails).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasMany<ContractStage>(s => s.ContractStage).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasMany<ContractServices>(s => s.ContractServices).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ContractServices>(entity =>
            {
                entity.HasKey(e => e.ContractServicesId);
                entity.ToTable("Acc_ContractServices");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.ServiceId).HasColumnName("ServiceId");
                entity.Property(t => t.ServiceQty).HasColumnName("ServiceQty");
                entity.Property(t => t.serviceoffertxt).HasColumnName("serviceoffertxt");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.TaxType).HasColumnName("TaxType");
                entity.Property(t => t.Serviceamountval).HasColumnName("Serviceamountval");
                modelBuilder.Entity<ContractServices>().HasOne(s => s.Contracts).WithMany(g=>g.ContractServices).HasForeignKey(e => e.ContractId);
                modelBuilder.Entity<ContractServices>().HasOne(s => s.serviceprice).WithMany(g => g.ContractServices).HasForeignKey(e => e.ServiceId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ContractStage>(entity =>
            {
                entity.HasKey(e => e.ContractStageId);
                entity.ToTable("Pro_ContractStage");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.Stage).HasColumnName("Stage");
                entity.Property(t => t.StageDescreption).HasColumnName("StageDescreption");
                entity.Property(t => t.Stagestartdate).HasColumnName("Stagestartdate");
                entity.Property(t => t.Stageenddate).HasColumnName("Stageenddate");
                modelBuilder.Entity<ContractStage>().HasOne(s => s.Contracts).WithMany(g=>g.ContractStage).HasForeignKey(e => e.ContractId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CostCenters>(entity =>
            {
                entity.HasKey(e => e.CostCenterId);
                entity.ToTable("Acc_CostCenters");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.Level).HasColumnName("CostCenterLevel");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ProjId).HasColumnName("ProjId");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                modelBuilder.Entity<CostCenters>().HasOne(s => s.ParentCostCenter).WithMany().HasForeignKey(e => e.ParentId);
                modelBuilder.Entity<CostCenters>().HasMany<Transactions>(s => s.Transactions).WithOne(g => g.CostCenters).HasForeignKey(s => s.CostCenterId);
                modelBuilder.Entity<CostCenters>().HasMany<CostCenters>(s => s.ChildsCostCenter).WithOne(g => g.ParentCostCenter).HasForeignKey(s => s.ParentId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.CurrencyId);
                entity.ToTable("Acc_Currency");
                entity.Property(t => t.CurrencyCode).HasColumnName("Code");
                entity.Property(t => t.CurrencyNameAr).HasColumnName("NameAr");
                entity.Property(t => t.CurrencyNameEn).HasColumnName("NameEn");
                entity.Property(t => t.PartCount).HasColumnName("PartCount");
                entity.Property(t => t.PartNameAr).HasColumnName("PartNameAr");
                entity.Property(t => t.PartNameEn).HasColumnName("PartNameEn");
                entity.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Custody>(entity =>
            {
                entity.HasKey(e => e.CustodyId);
                //entity.ToTable("Emp_EmpCustody");
                entity.ToTable("Emp_EmpCustody", t => t.HasTrigger("Del_Remainder"));
                entity.ToTable("Emp_EmpCustody", t => t.HasTrigger("Update_Remainder"));
                entity.ToTable("Emp_EmpCustody", t => t.HasTrigger("UpdateRemainder"));

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
                modelBuilder.Entity<Custody>().HasOne(s => s.Employee).WithMany(s=>s.Custodies).HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<Custody>().HasOne(s => s.Invoices).WithMany().HasForeignKey(e => e.InvoiceId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<EmpLocations>(entity =>
            {
                entity.HasKey(e => e.EmpLocationId);
                entity.ToTable("Emp_EmpLocations");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.LocationId).HasColumnName("LocationId");
                modelBuilder.Entity<EmpLocations>().HasOne(s => s.Employee).WithMany(s => s.EmployeeLocations).HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<EmpLocations>().HasOne(s => s.AttendenceLocation).WithMany(s => s.EmpLocations).HasForeignKey(e => e.LocationId);

                //modelBuilder.Entity<EmpLocations>().HasOne(s => s.location).WithMany().HasForeignKey(e => e.LocationId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CustomerFiles>(entity =>
            {
                entity.HasKey(e => e.FileId);
                entity.ToTable("Pro_CustomerFiles");
                entity.Property(t => t.FileName).HasColumnName("FileName");
                entity.Property(t => t.Description).HasColumnName("Description");
                entity.Property(t => t.Extenstion).HasColumnName("Extenstion");
                entity.Property(t => t.OriginalFileName).HasColumnName("OriginalFileName");
                entity.Property(t => t.UploadDate).HasColumnName("UploadDate");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                modelBuilder.Entity<CustomerFiles>().HasOne(s => s.FileType).WithMany().HasForeignKey(e => e.TypeId);
                modelBuilder.Entity<CustomerFiles>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CustomerMail>(entity =>
            {
                entity.HasKey(e => e.MailId);
                entity.ToTable("Pro_CustomerMails");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.SenderUser).HasColumnName("SenderUser");
                entity.Property(t => t.MailText).HasColumnName("MailText");
                entity.Property(t => t.MailSubject).HasColumnName("MailSubject");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.AllCustomers).HasColumnName("AllCustomers");
                entity.Property(t => t.AllCustomers).HasColumnName("AllCustomers");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<CustomerMail>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<CustomerMail>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.SenderUser);
            });

            //--------------------------------END--------------------------------------------------

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Customer_Branches>(entity =>
            {
                entity.HasKey(e => e.Customer_BranchesId);
                entity.ToTable("Pro_Customer_Branches");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<Customer_Branches>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Customer_Branches>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.ToTable("Pro_Customers");
                entity.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
                entity.Property(t => t.CustomerNameAr).HasColumnName("NameAr");
                entity.Property(t => t.CustomerNameEn).HasColumnName("NameEn");
                entity.Property(t => t.CustomerNationalId).HasColumnName("NationalId");
                entity.Property(t => t.NationalIdSource).HasColumnName("NationalIdSource");
                entity.Property(t => t.CustomerAddress).HasColumnName("Address");
                entity.Property(t => t.CustomerEmail).HasColumnName("Email");
                entity.Property(t => t.CustomerPhone).HasColumnName("PhoneNo");
                entity.Property(t => t.CustomerMobile).HasColumnName("MobileNo");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.CustomerTypeId).HasColumnName("CustomerType");
                entity.Property(t => t.LogoUrl).HasColumnName("LogoUrl");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.CommercialActivity).HasColumnName("CommercialActivity");
                entity.Property(t => t.CommercialRegister).HasColumnName("CommercialRegister");
                entity.Property(t => t.CommercialRegDate).HasColumnName("CommercialRegDate");
                entity.Property(t => t.CommercialRegHijriDate).HasColumnName("CommercialRegHijriDate");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.GeneralManager).HasColumnName("GeneralManager");
                entity.Property(t => t.AgentName).HasColumnName("AgentName");
                entity.Property(t => t.AgentType).HasColumnName("AgentType");
                entity.Property(t => t.AgentNumber).HasColumnName("AgentNumber");
                entity.Property(t => t.AgentAttachmentUrl).HasColumnName("AgentAttachmentUrl");
                entity.Property(t => t.ResponsiblePerson).HasColumnName("ResponsiblePerson");
                entity.Property(t => t.IsPrivate).HasColumnName("IsPrivate");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.CompAddress).HasColumnName("CompAddress");
                entity.Property(t => t.PostalCodeFinal).HasColumnName("PostalCodeFinal");
                entity.Property(t => t.ExternalPhone).HasColumnName("ExternalPhone");
                entity.Property(t => t.Country).HasColumnName("Country");
                entity.Property(t => t.Neighborhood).HasColumnName("Neighborhood");
                entity.Property(t => t.StreetName).HasColumnName("StreetName");
                entity.Property(t => t.BuildingNumber).HasColumnName("BuildingNumber");
                entity.Property(t => t.CityId).HasColumnName("CityId");
                modelBuilder.Entity<Customer>().HasOne(s => s.commercialActivities).WithMany().HasForeignKey(e => e.CommercialActivity);
                modelBuilder.Entity<Customer>().HasOne(s => s.BranchActivity).WithMany().HasForeignKey(e => e.GeneralManager);

                modelBuilder.Entity<Customer>().HasOne(s => s.city).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Customer>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<Customer>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<Customer>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Customer>().HasMany<Project>(s => s.Projects).WithOne(g => g.customer).HasForeignKey(s => s.CustomerId);
                modelBuilder.Entity<Customer>().HasMany<Invoices>(s => s.Invoicess).WithOne(g => g.Customer).HasForeignKey(s => s.CustomerId);
                modelBuilder.Entity<Customer>().HasMany<Transactions>(s => s.Transactions).WithOne(g => g.Customer).HasForeignKey(s => s.AccountId);
                modelBuilder.Entity<Customer>().HasMany<Customer_Branches>(s => s.Customer_Branches).WithOne(g => g.Customer).HasForeignKey(s => s.CustomerId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CustomerPayments>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.ToTable("Pro_CustomerPayments");
                entity.Property(t => t.PaymentNo).HasColumnName("PaymentNo");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.PaymentDate).HasColumnName("PaymentDate");
                entity.Property(t => t.PaymentDateHijri).HasColumnName("PaymentDateHijri");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
                entity.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
                entity.Property(t => t.TransactionId).HasColumnName("TransactionId");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.ToAccountId).HasColumnName("ToAccountId");
                entity.Property(t => t.IsPaid).HasColumnName("IsPaid");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.AmountValueText).HasColumnName("AmountValueText");
                entity.Property(t => t.OfferId).HasColumnName("OfferId");
                entity.Property(t => t.Isconst).HasColumnName("Isconst");
                entity.Property(t => t.ServiceId).HasColumnName("ServiceId");
                entity.Property(t => t.AmountValueText_EN).HasColumnName("AmountValueText_EN");
                entity.Property(t => t.AmountPercentage).HasColumnName("AmountPercentage");
                entity.Property(t => t.IsCanceled).HasColumnName("IsCanceled");

                modelBuilder.Entity<CustomerPayments>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.ToAccountId);
                modelBuilder.Entity<CustomerPayments>().HasOne(s => s.Invoices).WithMany().HasForeignKey(e => e.InvoiceId);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CustomerSMS>(entity =>
            {
                entity.HasKey(e => e.SMSId);
                entity.ToTable("Pro_CustomerSMS");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.SenderUser).HasColumnName("SenderUser");
                entity.Property(t => t.SMSText).HasColumnName("SMSText");
                entity.Property(t => t.SMSSubject).HasColumnName("SMSSubject");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.AllCustomers).HasColumnName("AllCustomers");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<CustomerSMS>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<CustomerSMS>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.SenderUser);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<DatabaseBackup>(entity =>
            {
                entity.HasKey(e => e.BackupId);
                entity.ToTable("Sys_DBackup");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.SavedName).HasColumnName("SavedName");
                entity.Property(t => t.LocalSavedPath).HasColumnName("LocalSavedPath");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.FileSize).HasColumnName("FileSize");
                entity.Property(t => t.TotalProject).HasColumnName("TotalProject");
                entity.Property(t => t.TotalClient).HasColumnName("TotalClient");
                entity.Property(t => t.TotalExp).HasColumnName("TotalExp");
                entity.Property(t => t.TotalReve).HasColumnName("TotalReve");
                entity.Property(t => t.TotalBranches).HasColumnName("TotalBranches");
                entity.Property(t => t.TotalUsers).HasColumnName("TotalUsers");
                entity.Property(t => t.LastPro).HasColumnName("LastPro");
                entity.Property(t => t.Lastinvoice).HasColumnName("Lastinvoice");
                entity.Property(t => t.LastVoucherRet).HasColumnName("LastVoucherRet");
                entity.Property(t => t.LastreVoucher).HasColumnName("LastreVoucher");
                entity.Property(t => t.LastpayVoucher).HasColumnName("LastpayVoucher");
                entity.Property(t => t.LastEntyvoucher).HasColumnName("LastEntyvoucher");
                entity.Property(t => t.LasEmpContract).HasColumnName("LasEmpContract");
                entity.Property(t => t.LasCustomer).HasColumnName("LasCustomer");
                entity.Property(t => t.TotalarchiveProject).HasColumnName("TotalarchiveProject");
                modelBuilder.Entity<DatabaseBackup>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);
                entity.ToTable("Emp_Departments");
                entity.Property(t => t.DepartmentNameAr).HasColumnName("NameAr");
                entity.Property(t => t.DepartmentNameEn) .HasColumnName("NameEn");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<DependencySettings>(entity =>
            {
                entity.HasKey(e => e.DependencyId);
                entity.ToTable("Pro_DependencySettings");
                entity.Property(t => t.PredecessorId).HasColumnName("PredecessorId");
                entity.Property(t => t.SuccessorId).HasColumnName("SuccessorId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<DependencySettingsNew>(entity =>
            {
                entity.HasKey(e => e.DependencyId);
                entity.ToTable("Pro_DependencySettingsNew");
                entity.Property(t => t.PredecessorId).HasColumnName("PredecessorId");
                entity.Property(t => t.SuccessorId).HasColumnName("SuccessorId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<DependencySettingsNew>().HasOne(s => s.SettingsPredecessor).WithMany().HasForeignKey(e => e.PredecessorId);
                modelBuilder.Entity<DependencySettingsNew>().HasOne(s => s.SettingsSuccessor).WithMany().HasForeignKey(e => e.SuccessorId);

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

                modelBuilder.Entity<DiscountReward>().HasOne(s => s.Employees).WithMany(s=>s.DiscountRewards).HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<DiscountReward>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.DiscountReward).HasForeignKey(s => s.DiscountRewardId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<DraftDetails>(entity =>
            {
                entity.HasKey(e => e.DraftDetailId);
                entity.ToTable("Pro_DraftDetails");
                entity.Property(t => t.DraftId).HasColumnName("DraftId");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");

                modelBuilder.Entity<DraftDetails>().HasOne(s => s.Draft).WithMany().HasForeignKey(e => e.DraftId);
                modelBuilder.Entity<DraftDetails>().HasOne(s => s.Project).WithMany(s=>s.DraftDetails).HasForeignKey(e => e.ProjectId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Draft>(entity =>
            {
                entity.HasKey(e => e.DraftId);
                entity.ToTable("Pro_Drafts");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.Name).HasColumnName("Name");
                entity.Property(t => t.DraftUrl).HasColumnName("DraftUrl");

                modelBuilder.Entity<Draft>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<Draft>().HasMany<DraftDetails>(s => s.DraftDetails).WithOne(g => g.Draft).HasForeignKey(s => s.DraftId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Drafts_Templates>(entity =>
            {
                entity.HasKey(e => e.DraftTempleteId);
                entity.ToTable("Pro_Drafts_Templates");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.Name).HasColumnName("Name");
                entity.Property(t => t.DraftUrl).HasColumnName("DraftUrl");

                modelBuilder.Entity<Drafts_Templates>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<EmailSetting>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Sys_EmailSetting");
                entity.Property(t => t.SenderEmail).HasColumnName("SenderEmail");
                entity.Property(t => t.Password).HasColumnName("Password");
                entity.Property(t => t.SenderName).HasColumnName("SenderName");
                entity.Property(t => t.Host).HasColumnName("Host");
                entity.Property(t => t.Port).HasColumnName("Port");
                entity.Property(t => t.SSL).HasColumnName("SSL");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.DisplayName).HasColumnName("DisplayName");
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
                entity.Property(t => t.DirectManager).HasColumnName("DirectManager");
                entity.Property(t => t.QuaContract).HasColumnName("QuaContract");
                entity.Property(t => t.OtherAllownces).HasColumnName("OtherAllownces");

                entity.Property(t => t.IsRememberContract).HasColumnName("IsRememberContract");
                entity.Property(t => t.RememberDateContract).HasColumnName("RememberDateContract");


                entity.Property(t => t.IsRememberResident).HasColumnName("IsRememberResident");
                entity.Property(t => t.RememberDateResident).HasColumnName("RememberDateResident");
                entity.Property(t => t.EmpHourlyCost).HasColumnName("EmpHourlyCost");
                entity.Property(t => t.DailyWorkinghours).HasColumnName("DailyWorkinghours");

                modelBuilder.Entity<Employees>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<Employees>().HasMany<Allowance>(s => s.Allowance).WithOne(g => g.Employees).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<Loan>(s => s.Loans).WithOne(g => g.Employees).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<Custody>(s => s.Custodies).WithOne(g => g.Employee).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<DiscountReward>(s => s.DiscountRewards).WithOne(g => g.Employees).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<AttAbsentDay>(s => s.AttAbsentDays).WithOne(g => g.Employees).HasForeignKey(s => s.EmpId);
                modelBuilder.Entity<Employees>().HasMany<Vacation>(s => s.Vacations).WithOne(g => g.EmployeeName).HasForeignKey(s => s.EmployeeId);
                modelBuilder.Entity<Employees>().HasMany<PayrollMarches>(s => s.PayrollMarches).WithOne(g => g.Employee).HasForeignKey(s => s.EmpId);
                modelBuilder.Entity<Employees>().HasMany<EmpLocations>(s => s.EmployeeLocations).WithOne(g => g.Employee).HasForeignKey(s => s.EmpId);

                modelBuilder.Entity<Employees>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Job).WithMany().HasForeignKey(e => e.JobId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Bank).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Account).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<Employees>().HasOne(s => s.Nationality).WithMany().HasForeignKey(e => e.NationalityId);
                modelBuilder.Entity<Employees>().HasOne(s => s.AttendaceTime).WithMany().HasForeignKey(e => e.DawamId);
                modelBuilder.Entity<Employees>().HasOne(s => s.users).WithMany().HasForeignKey(e => e.UserId);
                //modelBuilder.Entity<Employees>().HasOne(s => s.AttendenceLocation).WithMany().HasForeignKey(e => e.AttendenceLocationId);

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
            modelBuilder.Entity<ExpRevenuExpenses>(entity =>
            {
                entity.HasKey(e => e.ExpecteId);
                entity.ToTable("Acc_ExpRevenuExpenses");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.ToAccountId).HasColumnName("ToAccountId");
                entity.Property(t => t.CostCenterId).HasColumnName("CostCenterId");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.CollectionDate).HasColumnName("CollectionDate");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.IsDone).HasColumnName("IsDone");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<ExpRevenuExpenses>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<ExpRevenuExpenses>().HasOne(s => s.CostCenters).WithMany().HasForeignKey(e => e.CostCenterId);
                modelBuilder.Entity<ExpRevenuExpenses>().HasOne(s => s.ToAccounts).WithMany().HasForeignKey(e => e.ToAccountId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ExternalEmployees>(entity =>
            {
                entity.HasKey(e => e.EmpId);
                entity.ToTable("Pro_ExternalEmployees");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.Description).HasColumnName("Description");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<ExternalEmployees>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectFiles>(entity =>
            {
                entity.HasKey(e => e.FileId);
                entity.ToTable("Pro_Files");
                entity.Property(t => t.FileName).HasColumnName("FileName");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.Extension).HasColumnName("Extension");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.IsCertified).HasColumnName("IsCertified");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.TaskId).HasColumnName("TaskId");
                entity.Property(t => t.Brand).HasColumnName("Brand");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.DeleteUrl).HasColumnName("DeleteUrl");
                entity.Property(t => t.ThumbnailUrl).HasColumnName("ThumbnailUrl");
                entity.Property(t => t.DeleteType).HasColumnName("DeleteType");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UploadDate).HasColumnName("UploadDate");
                entity.Property(t => t.BarcodeFileNum).HasColumnName("BarcodeFileNum");
                entity.Property(t => t.CompanyTaxNo).HasColumnName("CompanyTaxNo");
                entity.Property(t => t.IsShare).HasColumnName("IsShare");
                entity.Property(t => t.ViewShare).HasColumnName("ViewShare");
                entity.Property(t => t.DonwloadShare).HasColumnName("DonwloadShare");
                entity.Property(t => t.TimeShare).HasColumnName("TimeShare");
                entity.Property(t => t.TimeTypeShare).HasColumnName("TimeTypeShare");
                entity.Property(t => t.TimeShareDate).HasColumnName("TimeShareDate");
                entity.Property(t => t.FileUrlW).HasColumnName("FileUrlW");
                entity.Property(t => t.CustomeComment).HasColumnName("CustomeComment");
                entity.Property(t => t.PageInsert).HasColumnName("PageInsert");
                entity.Property(t => t.UploadType).HasColumnName("UploadType");
                entity.Property(t => t.UploadName).HasColumnName("UploadName");
                entity.Property(t => t.UploadFileId).HasColumnName("UploadFileId");
                entity.Property(t => t.UploadFileIdB).HasColumnName("UploadFileIdB");

                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.FileType).WithMany().HasForeignKey(e => e.TypeId);
                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.Project).WithMany(s=>s.ProjectFiles).HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.ProjectPhasesTasks).WithMany().HasForeignKey(e => e.TaskId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<FilesAuth>(entity =>
            {
                entity.HasKey(e => e.FilesAuthId);
                entity.ToTable("Sys_FilesAuth");
                entity.Property(t => t.AppKey).HasColumnName("AppKey");
                entity.Property(t => t.AppSecret).HasColumnName("AppSecret");
                entity.Property(t => t.RedirectUri).HasColumnName("RedirectUri");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.AccessToken).HasColumnName("AccessToken");
                entity.Property(t => t.RefreshToken).HasColumnName("RefreshToken");
                entity.Property(t => t.FolderName).HasColumnName("FolderName");
                entity.Property(t => t.ExpiresIn).HasColumnName("ExpiresIn");
                entity.Property(t => t.CreationDate).HasColumnName("CreationDate");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Sendactive).HasColumnName("Sendactive");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<FileType>(entity =>
            {
                entity.HasKey(e => e.FileTypeId);
                entity.ToTable("Pro_FileTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<FiscalYears>(entity =>
            {
                entity.HasKey(e => e.FiscalId);
                entity.ToTable("Acc_FiscalYears");
                entity.Property(t => t.YearId).HasColumnName("YearId");
                entity.Property(t => t.YearName).HasColumnName("YearName");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.IsActive).HasColumnName("IsActive");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<FollowProj>(entity =>
            {
                entity.HasKey(e => e.FollowProjId);
                entity.ToTable("Pro_FollowProj");
                entity.Property(t => t.FollowProjId).HasColumnName("FollowProjId");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.TimeNo).HasColumnName("TimeNo");
                entity.Property(t => t.TimeType).HasColumnName("TimeType");
                entity.Property(t => t.EmpRate).HasColumnName("EmpRate");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.ExpectedCost).HasColumnName("ExpectedCost");
                entity.Property(t => t.ConfirmRate).HasColumnName("ConfirmRate");

                modelBuilder.Entity<FollowProj>().HasOne(s => s.project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<FollowProj>().HasOne(s => s.employees).WithMany().HasForeignKey(e => e.EmpId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<FullProjectsReport>(entity =>
            {
                entity.HasKey(e => e.ReportId);
                entity.ToTable("Sys_FullProjectsReport");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.PhaseTaskId).HasColumnName("PhaseTaskId");
                entity.Property(t => t.Revenue).HasColumnName("Revenue");
                entity.Property(t => t.Expenses).HasColumnName("Expenses");
                entity.Property(t => t.Projectpercentage).HasColumnName("Projectpercentage");
                entity.Property(t => t.Taskpercentage).HasColumnName("Taskpercentage");
                entity.Property(t => t.date).HasColumnName("date");
                entity.Property(t => t.Time).HasColumnName("Time");

                modelBuilder.Entity<FullProjectsReport>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<FullProjectsReport>().HasOne(s => s.ProjectPhasesTasks).WithMany().HasForeignKey(e => e.PhaseTaskId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<GroupPrivileges>(entity =>
            {
                entity.HasKey(e => e.GroupPrivId);
                entity.ToTable("Sys_GroupPrivileges");
                entity.Property(t => t.PrivilegeId).HasColumnName("PrivilegeId");
                entity.Property(t => t.GroupId).HasColumnName("GroupId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GroupId);
                entity.ToTable("Sys_Groups");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<GuideDepartments>(entity =>
            {
                entity.HasKey(e => e.DepId);
                entity.ToTable("Guide_Departments");
                entity.Property(t => t.DepNameAr).HasColumnName("DepNameAr");
                entity.Property(t => t.DepNameEn).HasColumnName("DepNameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<GuideDepartmentDetails>(entity =>
            {
                entity.HasKey(e => e.DepDetailsId);
                entity.ToTable("Guide_DepartmentDetails");
                entity.Property(t => t.DepId).HasColumnName("DepId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.Header).HasColumnName("Header");
                entity.Property(t => t.Link).HasColumnName("Link");
                entity.Property(t => t.Text).HasColumnName("Text");
                entity.Property(t => t.NameAR).HasColumnName("NameAR");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                modelBuilder.Entity<GuideDepartmentDetails>().HasOne(s => s.GuideDepartments).WithMany().HasForeignKey(e => e.DepId);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Guarantees>(entity =>
            {
                entity.HasKey(e => e.GuaranteeId);
                entity.ToTable("Acc_Guarantees");
                entity.Property(t => t.Number).HasColumnName("Number");
                entity.Property(t => t.Percentage).HasColumnName("Percentage");
                entity.Property(t => t.Period).HasColumnName("Period");
                entity.Property(t => t.ProjectName).HasColumnName("ProjectName");
                entity.Property(t => t.ReturnReason).HasColumnName("ReturnReason");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.Value).HasColumnName("Value");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.GuarantorAccId).HasColumnName("GuarantorAccId");
                entity.Property(t => t.IsReturned).HasColumnName("IsReturned");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.CustomerName).HasColumnName("CustomerName");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<Guarantees>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.GuarantorAccId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ImportantProject>(entity =>
            {
                entity.HasKey(e => e.ImportantProId);
                entity.ToTable("Pro_ImportantProject");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Flag).HasColumnName("Flag");
                entity.Property(t => t.IsImportant).HasColumnName("IsImportant");
                modelBuilder.Entity<ImportantProject>().HasOne(s => s.project).WithMany(s=>s.ImportantProjects).HasForeignKey(e => e.ProjectId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Instruments>(entity =>
            {
                entity.HasKey(e => e.InstrumentId);
                entity.ToTable("Pro_Instruments");
                entity.Property(t => t.InstrumentNo).HasColumnName("InstrumentNo");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.SourceId).HasColumnName("SourceId");
                modelBuilder.Entity<Instruments>().HasOne(s => s.project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Instruments>().HasOne(s => s.instrumentSources).WithMany().HasForeignKey(e => e.SourceId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<InstrumentSources>(entity =>
            {
                entity.HasKey(e => e.SourceId);
                entity.ToTable("Pro_InstrumentSources");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.HasKey(e => e.InvoiceId);
                entity.ToTable("Acc_Invoices");

                entity.Property(t => t.InvoiceNumber).HasColumnName("InvoiceNumber");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.OrderId).HasColumnName("OrderId");
                entity.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
                entity.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
                entity.Property(t => t.Rad).HasColumnName("Rad");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.DelegateId).HasColumnName("DelegateId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.StoreId).HasColumnName("StoreId");
                entity.Property(t => t.JournalNumber).HasColumnName("JournalNumber");
                entity.Property(t => t.IsPurchaseReturn).HasColumnName("IsPurchaseReturn");
                entity.Property(t => t.InvoiceValue).HasColumnName("InvoiceValue");
                entity.Property(t => t.DiscountPercentage).HasColumnName("DiscountPercentage");
                entity.Property(t => t.DiscountValue).HasColumnName("DiscountValue");
                entity.Property(t => t.TotalValue).HasColumnName("TotalValue");
                entity.Property(t => t.Paid).HasColumnName("Paid");
                entity.Property(t => t.PaidRequired).HasColumnName("PayRequired");
                entity.Property(t => t.TotalExpenses).HasColumnName("TotalExpenses");
                entity.Property(t => t.IsPost).HasColumnName("IsPost");
                entity.Property(t => t.PostDate).HasColumnName("PostDate");
                entity.Property(t => t.PostHijriDate).HasColumnName("PostHijriDate");
                entity.Property(t => t.YearId).HasColumnName("YearId");
                entity.Property(t => t.CurrentOp).HasColumnName("CurrentOp");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.ReceiverName).HasColumnName("ReceiverName");
                entity.Property(t => t.InvoiceReference).HasColumnName("InvoiceReference");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.ToAccountId).HasColumnName("ToAccountId");
                entity.Property(t => t.VoucherType).HasColumnName("VoucherType");
                entity.Property(t => t.ToInvoiceId).HasColumnName("ToInvoiceId");
                entity.Property(t => t.IsTax).HasColumnName("IsTax");
                entity.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
                entity.Property(t => t.NumberOfDepit).HasColumnName("NumberOfDepit");
                entity.Property(t => t.NumberOfCredit).HasColumnName("NumberOfCredit");
                entity.Property(t => t.InvoiceValueText).HasColumnName("InvoiceValueText");
                entity.Property(t => t.printBankAccount).HasColumnName("printBankAccount");
                entity.Property(t => t.SupplierInvoiceNo).HasColumnName("SupplierInvoiceNo");
                entity.Property(t => t.RecevierTxt).HasColumnName("RecevierTxt");
                entity.Property(t => t.ClauseId).HasColumnName("ClauseId");
                entity.Property(t => t.SupplierId).HasColumnName("SupplierId");
                entity.Property(t => t.CostCenterId).HasColumnName("CostCenterId");
                entity.Property(t => t.PageInsert).HasColumnName("PageInsert");
                entity.Property(t => t.QRCodeNum).HasColumnName("QRCodeNum");
                entity.Property(t => t.InvoiceNotes).HasColumnName("InvoiceNotes");
                entity.Property(t => t.RecycleYearTo).HasColumnName("RecycleYearTo");
                entity.Property(t => t.RecycleStatus).HasColumnName("RecycleStatus");
                entity.Property(t => t.InvoiceRetId).HasColumnName("InvoiceRetId");
                entity.Property(t => t.DunCalc).HasColumnName("DunCalc");
                entity.Property(t => t.VoucherAlarmDate).HasColumnName("VoucherAlarmDate");
                entity.Property(t => t.VoucherAlarmCheck).HasColumnName("VoucherAlarmCheck");
                entity.Property(t => t.IsSendAlarm).HasColumnName("IsSendAlarm");
                entity.Property(t => t.MovementId).HasColumnName("MovementId");
                entity.Property(t => t.CreditNotiId).HasColumnName("CreditNotiId");
                entity.Property(t => t.DepitNotiId).HasColumnName("DepitNotiId");
                entity.Property(t => t.InvUUID).HasColumnName("InvUUID");
                entity.Property(t => t.VoucherAdjustment).HasColumnName("VoucherAdjustment");
                entity.Property(t => t.PurchaseOrderNo).HasColumnName("PurchaseOrderNo");

                modelBuilder.Entity<Invoices>().HasMany<Acc_InvoicesRequests>(s => s.InvoicesRequests).WithOne(g => g.Invoice).HasForeignKey(s => s.InvoiceId);
                modelBuilder.Entity<Invoices>().HasMany<VoucherDetails>(s => s.VoucherDetails).WithOne(g => g.Invoices).HasForeignKey(s => s.InvoiceId);
                modelBuilder.Entity<Invoices>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.Invoices).HasForeignKey(s => s.InvoiceId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Customer).WithMany(x=>x.Invoicess).HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Suppliers).WithMany().HasForeignKey(e => e.SupplierId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Project).WithMany(x => x.Invoices).HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.AccTransactionTypes).WithMany().HasForeignKey(e => e.Type);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.ToAccountId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Invoices_Credit).WithMany().HasForeignKey(e => e.CreditNotiId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Invoices_Depit).WithMany().HasForeignKey(e => e.DepitNotiId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Delegate).WithMany().HasForeignKey(e => e.DelegateId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Acc_InvoicesRequests>(entity =>
            {
                entity.HasKey(e => e.InvoiceReqId);
                entity.ToTable("Acc_InvoicesRequests");

                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.InvoiceNoRequest).HasColumnName("InvoiceNoRequest");
                entity.Property(t => t.IsSent).HasColumnName("IsSent");
                entity.Property(t => t.StatusCode).HasColumnName("StatusCode");
                entity.Property(t => t.SendingStatus).HasColumnName("SendingStatus");
                entity.Property(t => t.warningmessage).HasColumnName("warningmessage");
                entity.Property(t => t.ClearedInvoice).HasColumnName("ClearedInvoice");
                entity.Property(t => t.errormessage).HasColumnName("errormessage");
                entity.Property(t => t.InvoiceHash).HasColumnName("InvoiceHash");
                entity.Property(t => t.SingedXML).HasColumnName("SingedXML");
                entity.Property(t => t.EncodedInvoice).HasColumnName("EncodedInvoice");
                entity.Property(t => t.ZatcaUUID).HasColumnName("ZatcaUUID");
                entity.Property(t => t.QRCode).HasColumnName("QRCode");
                entity.Property(t => t.PIH).HasColumnName("PIH");
                entity.Property(t => t.SingedXMLFileName).HasColumnName("SingedXMLFileName");
                //modelBuilder.Entity<Acc_InvoicesRequests>().HasOne(s => s.Invoice).WithMany().HasForeignKey(e => e.InvoiceId);
            });

            //--------------------------------END--------------------------------------------------

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.ToTable("Emp_Items", tb => tb.HasTrigger("UpdateItem_Remainder"));

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
            modelBuilder.Entity<Journals>(entity =>
            {
                entity.HasKey(e => e.JournalId);
                entity.ToTable("Acc_Journals");
                entity.Property(t => t.JournalNo).HasColumnName("JournalNo");
                entity.Property(t => t.VoucherId).HasColumnName("VoucherId");
                entity.Property(t => t.VoucherType).HasColumnName("VoucherType");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                modelBuilder.Entity<Journals>().HasOne(s => s.Invoice).WithMany().HasForeignKey(e => e.VoucherId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Licences>(entity =>
            {
                entity.HasKey(e => e.LicenceId);
                entity.ToTable("Sys_Licences");
                entity.Property(t => t.LicenceId).HasColumnName("LicenceId");
                entity.Property(t => t.LicenceContractNo).HasColumnName("LicenceContractNo");
                entity.Property(t => t.NoOfUsers).HasColumnName("NoOfUsers");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.Support_Expiry_Date).HasColumnName("Support_Expiry_Date");
                entity.Property(t => t.Email).HasColumnName("Email");
                entity.Property(t => t.Mobile).HasColumnName("Mobile");
                entity.Property(t => t.Hosting_Expiry_Date).HasColumnName("Hosting_Expiry_Date");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.Lic_Hosting_Info).HasColumnName("Lic_Hosting_Info");
                entity.Property(t => t.Lic_HostingCheck_Info).HasColumnName("Lic_HostingCheck_Info");
                entity.Property(t => t.Lic_HostingTimeType_Info).HasColumnName("Lic_HostingTimeType_Info");
                entity.Property(t => t.Lic_Hosting_NextTime_Info).HasColumnName("Lic_Hosting_NextTime_Info");
                entity.Property(t => t.Lic_Hosting_IsSend_Info).HasColumnName("Lic_Hosting_IsSend_Info");
                entity.Property(t => t.Lic_TechSuppCon_Info).HasColumnName("Lic_TechSuppCon_Info");
                entity.Property(t => t.Lic_TechSuppConCheck_Info).HasColumnName("Lic_TechSuppConCheck_Info");
                entity.Property(t => t.Lic_TechSuppConTimeType_Info).HasColumnName("Lic_TechSuppConTimeType_Info");
                entity.Property(t => t.Lic_TechSuppCon_NextTime_Info).HasColumnName("Lic_TechSuppCon_NextTime_Info");
                entity.Property(t => t.Lic_TechSuppCon_IsSend_Info).HasColumnName("Lic_TechSuppCon_IsSend_Info");
                entity.Property(t => t.Lic_Domain_Info).HasColumnName("Lic_Domain_Info");
                entity.Property(t => t.Lic_DomainCheck_Info).HasColumnName("Lic_DomainCheck_Info");
                entity.Property(t => t.Lic_DomainTimeType_Info).HasColumnName("Lic_DomainTimeType_Info");
                entity.Property(t => t.Lic_Domain_NextTime_Info).HasColumnName("Lic_Domain_NextTime_Info");
                entity.Property(t => t.Lic_Domain_IsSend_Info).HasColumnName("Lic_Domain_IsSend_Info");
                entity.Property(t => t.Lic_Other_Info).HasColumnName("Lic_Other_Info");
                entity.Property(t => t.Lic_OtherCheck_Info).HasColumnName("Lic_OtherCheck_Info");
                entity.Property(t => t.Lic_OtherTimeType_Info).HasColumnName("Lic_OtherTimeType_Info");
                entity.Property(t => t.Lic_Other_NextTime_Info).HasColumnName("Lic_Other_NextTime_Info");
                entity.Property(t => t.Lic_Other_IsSend_Info).HasColumnName("Lic_Other_IsSend_Info");
                entity.Property(t => t.Lic_Hosting_Cust).HasColumnName("Lic_Hosting_Cust");
                entity.Property(t => t.Lic_HostingCheck_Cust).HasColumnName("Lic_HostingCheck_Cust");
                entity.Property(t => t.Lic_Hosting_TimeType_Cust).HasColumnName("Lic_Hosting_TimeType_Cust");
                entity.Property(t => t.Lic_Hosting_NextTime_Cust).HasColumnName("Lic_Hosting_NextTime_Cust");
                entity.Property(t => t.Lic_Hosting_IsSend_Cust).HasColumnName("Lic_Hosting_IsSend_Cust");
                entity.Property(t => t.Lic_TechSuppCon_Cust).HasColumnName("Lic_TechSuppCon_Cust");
                entity.Property(t => t.Lic_TechSuppConCheck_Cust).HasColumnName("Lic_TechSuppConCheck_Cust");
                entity.Property(t => t.Lic_TechSuppConTimeType_Cust).HasColumnName("Lic_TechSuppConTimeType_Cust");
                entity.Property(t => t.Lic_TechSuppCon_NextTime_Cust).HasColumnName("Lic_TechSuppCon_NextTime_Cust");
                entity.Property(t => t.Lic_TechSuppCon_IsSend_Cust).HasColumnName("Lic_TechSuppCon_IsSend_Cust");
                entity.Property(t => t.Lic_Domain_Cust).HasColumnName("Lic_Domain_Cust");
                entity.Property(t => t.Lic_DomainCheck_Cust).HasColumnName("Lic_DomainCheck_Cust");
                entity.Property(t => t.Lic_DomainTimeType_Cust).HasColumnName("Lic_DomainTimeType_Cust");
                entity.Property(t => t.Lic_Domain_NextTime_Cust).HasColumnName("Lic_Domain_NextTime_Cust");
                entity.Property(t => t.Lic_Domain_IsSend_Cust).HasColumnName("Lic_Domain_IsSend_Cust");
                entity.Property(t => t.Lic_Other_Cust).HasColumnName("Lic_Other_Cust");
                entity.Property(t => t.Lic_OtherCheck_Cust).HasColumnName("Lic_OtherCheck_Cust");
                entity.Property(t => t.Lic_OtherTimeType_Cust).HasColumnName("Lic_OtherTimeType_Cust");
                entity.Property(t => t.Lic_Other_NextTime_Cust).HasColumnName("Lic_Other_NextTime_Cust");
                entity.Property(t => t.Lic_Other_IsSend_Cust).HasColumnName("Lic_Other_IsSend_Cust");
                entity.Property(t => t.Lic_Note_Info).HasColumnName("Lic_Note_Info");
                entity.Property(t => t.Lic_Note_Cust).HasColumnName("Lic_Note_Cust");
                entity.Property(t => t.Email2).HasColumnName("Email2");
                entity.Property(t => t.Email3).HasColumnName("Email3");
                entity.Property(t => t.Subscrip_Domain).HasColumnName("Subscrip_Domain");
                entity.Property(t => t.Subscrip_Hosting).HasColumnName("Subscrip_Hosting");

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
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.LoanId);
                //entity.ToTable("Emp_Loans");
                entity.ToTable("Emp_Loans", t => t.HasTrigger("LoanSendMail"));

                entity.Property(t => t.EmployeeId).HasColumnName("EmpId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.MonthNo).HasColumnName("MonthNo");
                entity.Property(t => t.Money).HasColumnName("Money");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.StartMonth).HasColumnName("StartMonth");
                entity.Property(t => t.AcceptedDate).HasColumnName("AcceptedDate");
                entity.Property(t => t.DecisionType).HasColumnName("DecisionType");
                entity.Property(t => t.AcceptedUser).HasColumnName("AcceptedUser");
                entity.Property(t => t.Isconverted).HasColumnName("Isconverted");


                modelBuilder.Entity<Loan>().HasMany<LoanDetails>(s => s.LoanDetails).WithOne(g => g.Loan).HasForeignKey(s => s.LoanId);
                modelBuilder.Entity<Loan>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<Loan>().HasOne(s => s.Employees).WithMany(s=>s.Loans).HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<Loan>().HasOne(s => s.UserAcccept).WithMany().HasForeignKey(e => e.AcceptedUser);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasKey(e => e.ModelId);
                entity.ToTable("Pro_Models");
                entity.Property(t => t.ModelName).HasColumnName("Name");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.Extension).HasColumnName("Extension");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<Model>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<Model>().HasOne(s => s.FileType).WithMany().HasForeignKey(e => e.TypeId);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ModelRequirements>(entity =>
            {
                entity.HasKey(e => e.ModelReqId);
                entity.ToTable("Pro_ModelRequirements");
                entity.Property(t => t.RequirementId).HasColumnName("RequirementId");
                entity.Property(t => t.ModelId).HasColumnName("ModelId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<ModelRequirements>().HasOne(s => s.Requirements).WithMany().HasForeignKey(e => e.RequirementId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ModelType>(entity =>
            {
                entity.HasKey(e => e.ModelTypeId);
                entity.ToTable("Pro_ModelTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

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
            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(e => e.NewsId);
                entity.ToTable("Sys_News");
                entity.Property(t => t.NewsTitleAr).HasColumnName("NewsTitleAr");
                entity.Property(t => t.NewsBodyAr).HasColumnName("NewsBodyAr");
                entity.Property(t => t.NewsTitleEn).HasColumnName("NewsTitleEn");
                entity.Property(t => t.NewsBodyEn).HasColumnName("NewsBodyEn");
                entity.Property(t => t.NewsImg).HasColumnName("NewsImg");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<NodeLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId);
                entity.ToTable("Pro_NodeLocations");
                entity.Property(t => t.SettingId).HasColumnName("SettingId");
                entity.Property(t => t.TaskId).HasColumnName("TaskId");
                entity.Property(t => t.Location).HasColumnName("Location");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);
                entity.ToTable("Pro_Notifications");
                entity.Property(t => t.Name).HasColumnName("Name");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.SendUserId).HasColumnName("SendUserId");
                entity.Property(t => t.ReceiveUserId).HasColumnName("ReceiveUserId");
                entity.Property(t => t.Description).HasColumnName("Description");
                entity.Property(t => t.Done).HasColumnName("Done");
                entity.Property(t => t.AllUsers).HasColumnName("AllUsers");
                entity.Property(t => t.ActionUser).HasColumnName("ActionUser");
                entity.Property(t => t.ActionDate).HasColumnName("ActionDate");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");
                entity.Property(t => t.SendDate).HasColumnName("SendDate");
                entity.Property(t => t.ReadingDate).HasColumnName("ReadingDate");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.TaskId).HasColumnName("TaskId");
                entity.Property(t => t.Title).HasColumnName("Title");
                entity.Property(t => t.NextTime).HasColumnName("NextTime");

                modelBuilder.Entity<Notification>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.ReceiveUserId);
                modelBuilder.Entity<Notification>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.SendUserId);
                modelBuilder.Entity<Notification>().HasOne(s => s.ReceiveUsers).WithMany().HasForeignKey(e => e.ReceiveUserId);
                modelBuilder.Entity<Notification>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<NotificationSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Sys_NotificationSettings");
                entity.Property(t => t.IDEndCount).HasColumnName("IDEndCount");
                entity.Property(t => t.PassportCount).HasColumnName("PassportCount");
                entity.Property(t => t.LicesnseCount).HasColumnName("LicesnseCount");
                entity.Property(t => t.ContractCount).HasColumnName("ContractCount");
                entity.Property(t => t.MedicalCount).HasColumnName("MedicalCount");
                entity.Property(t => t.VacancyCount).HasColumnName("VacancyCount");
                entity.Property(t => t.LoanCount).HasColumnName("LoanCount");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OffersConditions>(entity =>
            {
                entity.HasKey(e => e.OffersConditionsId);
                entity.ToTable("Acc_OffersConditions");
                entity.Property(t => t.OfferConditiontxt).HasColumnName("OfferConditiontxt");
                entity.Property(t => t.OfferId).HasColumnName("OfferId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Isconst).HasColumnName("Isconst");
                entity.Property(t => t.OfferConditiontxt_EN).HasColumnName("OfferConditiontxt_EN");
                modelBuilder.Entity<OffersConditions>().HasOne(s => s.OffersPrices).WithMany().HasForeignKey(e => e.OfferId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OfferService>(entity =>
            {
                entity.HasKey(e => e.OffersServicesId);
                entity.ToTable("Acc_OffersServicrs");
                entity.Property(t => t.OfferId).HasColumnName("OfferId");
                entity.Property(t => t.ServiceId).HasColumnName("ServiceId");
                entity.Property(t => t.ServiceQty).HasColumnName("ServiceQty");
                entity.Property(t => t.serviceoffertxt).HasColumnName("serviceoffertxt");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.TaxType).HasColumnName("TaxType");
                entity.Property(t => t.Serviceamountval).HasColumnName("Serviceamountval");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");

                modelBuilder.Entity<OfferService>().HasOne(s => s.OffersPrices).WithMany().HasForeignKey(e => e.OfferId);
                modelBuilder.Entity<OfferService>().HasOne(s => s.serviceprice).WithMany(s=>s.OfferService).HasForeignKey(e => e.ServiceId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OffersPrices>(entity =>
            {
                entity.HasKey(e => e.OffersPricesId);
                entity.ToTable("Acc_OffersPrices");
                entity.Property(t => t.OfferNo).HasColumnName("OfferNo");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.OfferDate).HasColumnName("OfferDate");
                entity.Property(t => t.OfferHijriDate).HasColumnName("OfferHijriDate");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.CustomerName).HasColumnName("CustomerName");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Department).HasColumnName("Department");
                entity.Property(t => t.OfferValue).HasColumnName("OfferValue");
                entity.Property(t => t.OfferValueTxt).HasColumnName("OfferValueTxt");
                entity.Property(t => t.OfferStatus).HasColumnName("OfferStatus");
                entity.Property(t => t.CustomerStatus).HasColumnName("CustomerStatus");
                entity.Property(t => t.ServiceId).HasColumnName("ServiceId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.RememberDate).HasColumnName("RememberDate");
                entity.Property(t => t.ISsent).HasColumnName("ISsent");
                entity.Property(t => t.OfferAlarmCheck).HasColumnName("OfferAlarmCheck");
                entity.Property(t => t.ServQty).HasColumnName("ServQty");
                entity.Property(t => t.OfferNoType).HasColumnName("OfferNoType");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.IsContainLogo).HasColumnName("IsContainLogo");
                entity.Property(t => t.IsContainSign).HasColumnName("IsContainSign");
                entity.Property(t => t.printBankAccount).HasColumnName("printBankAccount");
                entity.Property(t => t.CustomerEmail).HasColumnName("CustomerEmail");
                entity.Property(t => t.Customerphone).HasColumnName("Customerphone");
                entity.Property(t => t.CUstomerName_EN).HasColumnName("CUstomerName_EN");
                entity.Property(t => t.IsEnglish).HasColumnName("IsEnglish");
                entity.Property(t => t.IsEnglish).HasColumnName("IsEnglish");
                entity.Property(t => t.setIntroduction).HasColumnName("setIntroduction");
                entity.Property(t => t.NickName).HasColumnName("NickName");
                entity.Property(t => t.Description).HasColumnName("Description");
                entity.Property(t => t.Introduction).HasColumnName("Introduction");
                entity.Property(t => t.NotDisCustPrint).HasColumnName("NotDisCustPrint");
                entity.Property(t => t.CustomerMailCode).HasColumnName("CustomerMailCode");
                entity.Property(t => t.IsCertified).HasColumnName("IsCertified");
                entity.Property(t => t.CertifiedCode).HasColumnName("CertifiedCode");
                entity.Property(t => t.ProjectName).HasColumnName("ProjectName");
                entity.Property(t => t.ImplementationDuration).HasColumnName("ImplementationDuration");
                entity.Property(t => t.OfferValidity).HasColumnName("OfferValidity");



                modelBuilder.Entity<OffersPrices>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<OffersPrices>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<OffersPrices>().HasOne(s => s.Service).WithMany().HasForeignKey(e => e.ServiceId);

                modelBuilder.Entity<OffersPrices>().HasMany<CustomerPayments>(s => s.CustomerPayments).WithOne(g => g.OffersPrices).HasForeignKey(s => s.OfferId);
                modelBuilder.Entity<OffersPrices>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<OffersPrices>().HasMany<OffersConditions>(s => s.OffersConditions).WithOne(g => g.OffersPrices).HasForeignKey(s => s.OfferId);
                modelBuilder.Entity<OffersPrices>().HasMany<OfferService>(s => s.OfferService).WithOne(g => g.OffersPrices).HasForeignKey(s => s.OfferId);

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
            modelBuilder.Entity<OfficialDocuments>(entity =>
            {
                entity.HasKey(e => e.DocumentId);
                entity.ToTable("Acc_OfficialDocuments");
                entity.Property(t => t.Number).HasColumnName("Number");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.ExpiredDate).HasColumnName("ExpiredDate");
                entity.Property(t => t.ExpiredHijriDate).HasColumnName("ExpiredHijriDate");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.NotifyCount).HasColumnName("NotifyCount");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.RepeatAlarm).HasColumnName("RepeatAlarm");
                entity.Property(t => t.RecurrenceRateId).HasColumnName("RecurrenceRateId");
                modelBuilder.Entity<OfficialDocuments>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Organizations>(entity =>
            {
                entity.HasKey(e => e.OrganizationId);
                entity.ToTable("Sys_Organizations");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Mobile).HasColumnName("Mobile");
                entity.Property(t => t.Email).HasColumnName("Email");
                entity.Property(t => t.LogoUrl).HasColumnName("LogoUrl");
                entity.Property(t => t.ServerName).HasColumnName("ServerName");
                entity.Property(t => t.Address).HasColumnName("Address");
                entity.Property(t => t.WebSite).HasColumnName("WebSite");
                entity.Property(t => t.Fax).HasColumnName("Fax");
                entity.Property(t => t.CityId).HasColumnName("CityId");
                entity.Property(t => t.ReportType).HasColumnName("ReportType");
                entity.Property(t => t.TaxCode).HasColumnName("TaxCode");
                entity.Property(t => t.VAT).HasColumnName("VAT");
                entity.Property(t => t.VATSetting).HasColumnName("VATSetting");
                entity.Property(t => t.PostalCode).HasColumnName("PostalCode");
                entity.Property(t => t.Password).HasColumnName("Password");
                entity.Property(t => t.SenderName).HasColumnName("SenderName");
                entity.Property(t => t.Host).HasColumnName("Host");
                entity.Property(t => t.Port).HasColumnName("Port");
                entity.Property(t => t.SSL).HasColumnName("SSL");
                entity.Property(t => t.AccountBank).HasColumnName("AccountBank");
                entity.Property(t => t.IsFooter).HasColumnName("IsFooter");
                entity.Property(t => t.RepresentorEmpId).HasColumnName("RepresentorEmpId");
                entity.Property(t => t.PostalCodeFinal).HasColumnName("PostalCodeFinal");
                entity.Property(t => t.ExternalPhone).HasColumnName("ExternalPhone");
                entity.Property(t => t.Country).HasColumnName("Country");
                entity.Property(t => t.Neighborhood).HasColumnName("Neighborhood");
                entity.Property(t => t.StreetName).HasColumnName("StreetName");
                entity.Property(t => t.BuildingNumber).HasColumnName("BuildingNumber");
                entity.Property(t => t.AccountBank2).HasColumnName("AccountBank2");
                entity.Property(t => t.NotificationsMail).HasColumnName("NotificationsMail");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Engineering_License).HasColumnName("Engineering_License");
                entity.Property(t => t.Engineering_LicenseDate).HasColumnName("Engineering_LicenseDate");
                entity.Property(t => t.ComDomainLink).HasColumnName("ComDomainLink");
                entity.Property(t => t.ComDomainAddress).HasColumnName("ComDomainAddress");
                entity.Property(t => t.BankId).HasColumnName("BankId");
                entity.Property(t => t.BankId2).HasColumnName("BankId2");
                entity.Property(t => t.CSR).HasColumnName("CSR");
                entity.Property(t => t.PrivateKey).HasColumnName("PrivateKey");
                entity.Property(t => t.PublicKey).HasColumnName("PublicKey");
                entity.Property(t => t.SecreteKey).HasColumnName("SecreteKey");
                entity.Property(t => t.ApiBaseUri).HasColumnName("ApiBaseUri");
                entity.Property(t => t.LastvesionAndroid).HasColumnName("LastvesionAndroid");
                entity.Property(t => t.LastversionIOS).HasColumnName("LastversionIOS");
                entity.Property(t => t.MessageUpdateAr).HasColumnName("MessageUpdateAr");
                entity.Property(t => t.MessageUpdateEn).HasColumnName("MessageUpdateEn");
                entity.Property(t => t.SupportMessageAr).HasColumnName("SupportMessageAr");
                entity.Property(t => t.SupportMessageEn).HasColumnName("SupportMessageEn");
                entity.Property(t => t.SendCustomerMail).HasColumnName("SendCustomerMail");
                entity.Property(t => t.ModeType).HasColumnName("ModeType");


                modelBuilder.Entity<Organizations>().HasOne(s => s.City).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Organizations>().HasOne(s => s.UpdateUserT).WithMany().HasForeignKey(e => e.UpdateUser);
                modelBuilder.Entity<Organizations>().HasOne(s => s.RepEmployee).WithMany().HasForeignKey(e => e.RepresentorEmpId);
                modelBuilder.Entity<Organizations>().HasOne(s => s.Bank).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Organizations>().HasOne(s => s.Bank2).WithMany().HasForeignKey(e => e.BankId2);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OutInBox>(entity =>
            {
                entity.HasKey(e => e.OutInBoxId);
                entity.ToTable("Contac_OutInBox");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.SideFromId).HasColumnName("SideFromId");
                entity.Property(t => t.SideToId).HasColumnName("SideToId");
                entity.Property(t => t.InnerId).HasColumnName("InnerId");
                entity.Property(t => t.Topic) .HasColumnName("Topic");
                entity.Property(t => t.ArchiveFileId).HasColumnName("ArchiveFileId");
                entity.Property(t => t.RelatedToId).HasColumnName("RelatedToId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.OutInType).HasColumnName("OutInType");
                entity.Property(t => t.NumberType).HasColumnName("NumberType");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Priority).HasColumnName("Priority");

                modelBuilder.Entity<OutInBox>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.OutInBoxType).WithMany().HasForeignKey(e => e.TypeId);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.OutInBoxSerial).WithMany().HasForeignKey(e => e.NumberType);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.FromDepartment).WithMany().HasForeignKey(e => e.SideFromId);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.ToDepartment).WithMany().HasForeignKey(e => e.SideToId);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.InnerToOutIn).WithMany().HasForeignKey(e => e.InnerId);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.ArchiveFiles).WithMany().HasForeignKey(e => e.ArchiveFileId);
                modelBuilder.Entity<OutInBox>().HasMany<ContacFiles>(s => s.ContacFiles).WithOne(g => g.OutInBox).HasForeignKey(s => s.OutInBoxId);
                modelBuilder.Entity<OutInBox>().HasOne(s => s.RelatedToOutIn).WithMany().HasForeignKey(e => e.RelatedToId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OutInBoxSerial>(entity =>
            {
                entity.HasKey(e => e.OutInSerialId);
                entity.ToTable("Contac_OutInBoxSerial");
                entity.Property(t => t.Name).HasColumnName("Name");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.LastNumber).HasColumnName("LastNumber");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OutInBoxType>(entity =>
            {
                entity.HasKey(e => e.TypeId);
                entity.ToTable("Contac_OutInBoxType");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");           
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OutInImagesTo>(entity =>
            {
                entity.HasKey(e => e.ImageToId);
                entity.ToTable("Contac_OutInImagesTo");
                entity.Property(t => t.OutInboxId).HasColumnName("OutInboxId");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                modelBuilder.Entity<OutInImagesTo>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<OutMovements>(entity =>
            {
                entity.HasKey(e => e.MoveId);
                entity.ToTable("Pro_OutMovements");
                entity.Property(t => t.ConstraintNo).HasColumnName("ConstraintNo");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.RequiredWork).HasColumnName("RequiredWork");
                entity.Property(t => t.FinishedWork).HasColumnName("FinishedWork");
                entity.Property(t => t.ExpeditorId).HasColumnName("ExpeditorId");
                entity.Property(t => t.TrailingId).HasColumnName("TrailingId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                modelBuilder.Entity<OutMovements>().HasOne(s => s.ProjectTrailing).WithMany().HasForeignKey(e => e.TrailingId);
                modelBuilder.Entity<OutMovements>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.ExpeditorId);
                modelBuilder.Entity<OutMovements>().HasOne(s => s.ExternalEmployees).WithMany().HasForeignKey(e => e.EmpId);

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
                modelBuilder.Entity<PayrollMarches>().HasOne(s => s.Employee).WithMany(s => s.PayrollMarches).HasForeignKey(e => e.EmpId);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<PrivFollowers>(entity =>
            {
                entity.HasKey(e => e.PrivFollowerID);
                entity.ToTable("Pro_PrivFollowers");
                entity.Property(t => t.UserID).HasColumnName("UserID");
                entity.Property(t => t.TaskID).HasColumnName("TaskID");
                entity.Property(t => t.Flag).HasColumnName("Flag");

                modelBuilder.Entity<PrivFollowers>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserID);
                modelBuilder.Entity<PrivFollowers>().HasOne(s => s.ProjectPhasesTasks).WithMany().HasForeignKey(e => e.TaskID);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_Municipal>(entity =>
            {
                entity.HasKey(e => e.MunicipalId);
                entity.ToTable("Pro_Municipal");
                entity.Property(t => t.MunicipalId).HasColumnName("MunicipalId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_SubMunicipality>(entity =>
            {
                entity.HasKey(e => e.SubMunicipalityId);
                entity.ToTable("Pro_SubMunicipality");
                entity.Property(t => t.SubMunicipalityId).HasColumnName("SubMunicipalityId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_Super_PhaseDet>(entity =>
            {
                entity.HasKey(e => e.PhaseDetailesId);
                entity.ToTable("Pro_Super_PhaseDet");
                entity.Property(t => t.PhaseId).HasColumnName("PhaseId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_Super_Phases>(entity =>
            {
                entity.HasKey(e => e.PhaseId);
                entity.ToTable("Pro_Super_Phases");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.SuperCode).HasColumnName("SuperCode");
                modelBuilder.Entity<Pro_Super_Phases>().HasMany<Pro_Super_PhaseDet>(s => s.SuperPhaseDet).WithOne(g => g.Pro_Super_Phases).HasForeignKey(s => s.PhaseId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_SuperContractor>(entity =>
            {
                entity.HasKey(e => e.ContractorId);
                entity.ToTable("Pro_SuperContractor");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Email).HasColumnName("Email");
                entity.Property(t => t.CommercialRegister).HasColumnName("CommercialRegister");
                entity.Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_SupervisionDetails>(entity =>
            {
                entity.HasKey(e => e.SuperDetId);
                entity.ToTable("Pro_SupervisionDetails");
                entity.Property(t => t.SupervisionId).HasColumnName("SupervisionId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
                entity.Property(t => t.TheNumber).HasColumnName("TheNumber");
                entity.Property(t => t.TheLocation).HasColumnName("TheLocation");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectArchivesRe>(entity =>
            {
                entity.HasKey(e => e.ProArchReID);
                entity.ToTable("Pro_ProjectArchivesRe");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.ReDate).HasColumnName("ReDate");
                entity.Property(t => t.Re_TypeID).HasColumnName("Re_TypeID");
                entity.Property(t => t.Re_TypeName).HasColumnName("Re_TypeName");
                entity.Property(t => t.Re_PhasesTaskId).HasColumnName("Re_PhasesTaskId");
                modelBuilder.Entity<ProjectArchivesRe>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectArchivesRe>().HasOne(s => s.Phases).WithMany().HasForeignKey(e => e.Re_PhasesTaskId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectArchivesSee>(entity =>
            {
                entity.HasKey(e => e.ProArchSeeID);
                entity.ToTable("Pro_ProjectArchivesSee");
                entity.Property(t => t.ProArchReID).HasColumnName("ProArchReID");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.See_TypeID).HasColumnName("See_TypeID");
                modelBuilder.Entity<ProjectArchivesSee>().HasOne(s => s.ProjectArchivesRe).WithMany().HasForeignKey(e => e.ProArchReID);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectComments>(entity =>
            {
                entity.HasKey(e => e.CommentId);
                entity.ToTable("Pro_ProjectComments");
                entity.Property(t => t.Comment).HasColumnName("Comment");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                modelBuilder.Entity<ProjectComments>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectExtracts>(entity =>
            {
                entity.HasKey(e => e.ExtractId);
                entity.ToTable("Pro_ProjectExtracts");
                entity.Property(t => t.ExtractNo).HasColumnName("ExtractNo");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.Value).HasColumnName("Value");
                entity.Property(t => t.DateFrom).HasColumnName("DateFrom");
                entity.Property(t => t.DateTo).HasColumnName("DateTo");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.ValueText).HasColumnName("ValueText");
                entity.Property(t => t.IsDoneBefore).HasColumnName("IsDoneBefore");
                entity.Property(t => t.IsDoneAfter).HasColumnName("IsDoneAfter");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.SignatureUrl).HasColumnName("SignatureUrl");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectPhasesTasks>(entity =>
            {
                entity.HasKey(e => e.PhaseTaskId);
                //entity.ToTable("Pro_PhasesTasks");
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("PhaseStartSendNoti"));
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("PhaseStartSendNotiMail"));
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("TaskSendMail"));
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("TaskSendMail_Priority"));
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("TaskSendMail2"));
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("TaskSendNoti"));
                entity.ToTable("Pro_PhasesTasks", tb => tb.HasTrigger("TaskSendNotiMailStart"));

                entity.Property(t => t.DescriptionAr).HasColumnName("DescriptionAr");
                entity.Property(t => t.DescriptionEn).HasColumnName("DescriptionEn");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.TimeMinutes).HasColumnName("TimeMinutes");
                entity.Property(t => t.TimeType).HasColumnName("TimeType");
                entity.Property(t => t.Remaining).HasColumnName("Remaining");
                entity.Property(t => t.IsUrgent).HasColumnName("IsUrgent");
                entity.Property(t => t.IsTemp).HasColumnName("IsTemp");
                entity.Property(t => t.TaskType).HasColumnName("TaskType");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.OldStatus).HasColumnName("OldStatus");
                entity.Property(t => t.Active).HasColumnName("Active");
                entity.Property(t => t.StopCount).HasColumnName("StopCount");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.PercentComplete).HasColumnName("PercentComplete");
                entity.Property(t => t.Cost).HasColumnName("Cost");
                entity.Property(t => t.ToUserId).HasColumnName("ToUserId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.SettingId).HasColumnName("SettingId");
                entity.Property(t => t.LocationId).HasColumnName("LocationId");
                entity.Property(t => t.ParentSettingId).HasColumnName("ParentSettingId");
                entity.Property(t => t.PhasePriority).HasColumnName("PhasePriority");
                entity.Property(t => t.ExecPercentage).HasColumnName("ExecPercentage");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");
                entity.Property(t => t.PlusTime).HasColumnName("PlusTime");
                entity.Property(t => t.IsConverted).HasColumnName("IsConverted");
                entity.Property(t => t.IsMerig).HasColumnName("IsMerig");
                entity.Property(t => t.EndTime).HasColumnName("EndTime");
                entity.Property(t => t.TaskFullTime).HasColumnName("TaskFullTime");
                entity.Property(t => t.ExcpectedStartDate).HasColumnName("ExcpectedStartDate");
                entity.Property(t => t.ExcpectedEndDate).HasColumnName("ExcpectedEndDate");
                entity.Property(t => t.TaskLongDesc).HasColumnName("TaskLongDesc");
                entity.Property(t => t.IsRetrieved).HasColumnName("IsRetrieved");
                entity.Property(t => t.ProjectGoals).HasColumnName("ProjectGoals");
                entity.Property(t => t.AddTaskUserId).HasColumnName("AddTaskUserId");
                entity.Property(t => t.AskDetails).HasColumnName("AskDetails");
                entity.Property(t => t.IsNew).HasColumnName("IsNew");
                entity.Property(t => t.TaskTimeFrom).HasColumnName("TaskTimeFrom");
                entity.Property(t => t.TaskTimeTo).HasColumnName("TaskTimeTo");
                entity.Property(t => t.EndDateCalc).HasColumnName("EndDateCalc");
                entity.Property(t => t.RetrievedReason).HasColumnName("RetrievedReason");
                entity.Property(t => t.NumAdded).HasColumnName("NumAdded");
                entity.Property(t => t.NotVacCalc).HasColumnName("NotVacCalc");

                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.tasksReasons).WithMany().HasForeignKey(e => e.ReasonsId);

                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.AddTaskUser).WithMany().HasForeignKey(e => e.AddTaskUserId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.ProjectRequirementsGoals).WithMany().HasForeignKey(e => e.ProjectGoals);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.Project).WithMany(s=>s.ProjectPhasesTasks).HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjSubTypeId);

                //modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.MainPhase).WithMany().HasForeignKey(e => e.ProjSubTypeId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.SubPhase).WithMany().HasForeignKey(e => e.ParentId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.Settings).WithMany().HasForeignKey(e => e.SettingId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.TaskTypeModel).WithMany().HasForeignKey(e => e.TaskType);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.department).WithMany().HasForeignKey(e => e.DepartmentId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasMany<ContactList>(s => s.ContactLists).WithOne(g => g.ProjectPhasesTasks).HasForeignKey(s => s.TaskId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasMany<Pro_TaskOperations>(s => s.TaskOperationsList).WithOne(g => g.ProjectPhasesTasks).HasForeignKey(s => s.PhaseTaskId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProUserPrivileges>(entity =>
            {
                entity.HasKey(e => e.UserPrivId);
                //entity.ToTable("Pro_UserPrivileges");
                entity.ToTable("Pro_UserPrivileges", tb => tb.HasTrigger("PrivSendMail"));

                entity.Property(t => t.PrivilegeId).HasColumnName("PrivilegeId");
                entity.Property(t => t.ProjectID).HasColumnName("ProjectID");
                entity.Property(t => t.Projectno).HasColumnName("Projectno");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Select).HasColumnName("Select");
                entity.Property(t => t.Insert).HasColumnName("Insert");
                entity.Property(t => t.Update).HasColumnName("Update");
                entity.Property(t => t.Delete).HasColumnName("Delete");
                entity.Property(t => t.CustomerID).HasColumnName("CustomerID");

                modelBuilder.Entity<ProUserPrivileges>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ProUserPrivileges>().HasOne(s => s.Projects).WithMany(s=>s.ProUserPrivileges).HasForeignKey(e => e.ProjectID);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_ProjectSteps>(entity =>
            {
                entity.HasKey(e => e.ProjectStepId);
                entity.ToTable("Pro_ProjectSteps");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.StepId).HasColumnName("StepId");
                entity.Property(t => t.StepDetailId).HasColumnName("StepDetailId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                modelBuilder.Entity<Pro_ProjectSteps>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<Pro_ProjectSteps>().HasOne(s => s.StepDetails).WithMany().HasForeignKey(e => e.StepDetailId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_StepDetails>(entity =>
            {
                entity.HasKey(e => e.StepDetailId);
                entity.ToTable("Pro_StepDetails");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.StepId).HasColumnName("StepId");
                entity.Property(t => t.StepName).HasColumnName("StepName");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_ProjectAchievements>(entity =>
            {
                entity.HasKey(e => e.ProjectAchievementId);
                entity.ToTable("Pro_ProjectAchievements");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.StepId).HasColumnName("StepId");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_ProjectChallenges>(entity =>
            {
                entity.HasKey(e => e.ProjectChallengeId);
                entity.ToTable("Pro_ProjectChallenges");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.StepId).HasColumnName("StepId");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------


            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId);
                entity.ToTable("Pro_Projects", tb => tb.HasTrigger("ProSendMail"));

                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.ParentProjectId).HasColumnName("ParentProjectId");
                entity.Property(t => t.TransactionTypeId).HasColumnName("TransactionTypeId");
                entity.Property(t => t.MangerId).HasColumnName("MangerId");
                entity.Property(t => t.ProjectNoType).HasColumnName("ProjectNoType");
                entity.Property(t => t.ProjectDate).HasColumnName("ProjectDate");
                entity.Property(t => t.ProjectHijriDate).HasColumnName("ProjectHijriDate");
                entity.Property(t => t.ProjectExpireDate).HasColumnName("ProjectExpireDate");
                entity.Property(t => t.ProjectExpireHijriDate).HasColumnName("ProjectExpireHijriDate");
                entity.Property(t => t.SiteName).HasColumnName("SiteName");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.SubProjectTypeId).HasColumnName("SubProjectTypeId");
                entity.Property(t => t.OrderType).HasColumnName("OrderType");
                entity.Property(t => t.SketchName).HasColumnName("SketchName");
                entity.Property(t => t.SketchNo).HasColumnName("SketchNo");
                entity.Property(t => t.PieceNo).HasColumnName("PieceNo");
                entity.Property(t => t.AdwAR).HasColumnName("AdwAR");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.OutBoxNo).HasColumnName("OutBoxNo");
                entity.Property(t => t.OutBoxDate).HasColumnName("OutBoxDate");
                entity.Property(t => t.OutBoxHijriDate).HasColumnName("OutBoxHijriDate");
                entity.Property(t => t.Reason1).HasColumnName("Reason1");
                entity.Property(t => t.Notes1).HasColumnName("Notes1");
                entity.Property(t => t.Subject).HasColumnName("Subject");
                entity.Property(t => t.XPoint).HasColumnName("XPoint");
                entity.Property(t => t.YPoint).HasColumnName("YPoint");
                entity.Property(t => t.Technical).HasColumnName("Technical");
                entity.Property(t => t.Prosedor).HasColumnName("Prosedor");
                entity.Property(t => t.ReasonRevers).HasColumnName("ReasonRevers");
                entity.Property(t => t.EngNotes).HasColumnName("EngNotes");
                entity.Property(t => t.ReverseDate).HasColumnName("ReverseDate");
                entity.Property(t => t.ReverseHijriDate).HasColumnName("ReverseHijriDate");
                entity.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Receipt).HasColumnName("Receipt");
                entity.Property(t => t.PayStatus).HasColumnName("PayStatus");
                entity.Property(t => t.RegionName).HasColumnName("RegionName");
                entity.Property(t => t.DistrictName).HasColumnName("DistrictName");
                entity.Property(t => t.SiteType).HasColumnName("SiteType");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.ContractDate).HasColumnName("ContractDate");
                entity.Property(t => t.ContractHijriDate).HasColumnName("ContractHijriDate");
                entity.Property(t => t.ContractSource).HasColumnName("ContractSource");
                entity.Property(t => t.SiteNo).HasColumnName("SiteNo");
                entity.Property(t => t.PayanNo).HasColumnName("PayanNo");
                entity.Property(t => t.JehaId).HasColumnName("JehaId");
                entity.Property(t => t.ZaraaSak).HasColumnName("ZaraaSak");
                entity.Property(t => t.ZaraaNatural).HasColumnName("ZaraaNatural");
                entity.Property(t => t.BordersSak).HasColumnName("BordersSak");
                entity.Property(t => t.BordersNatural).HasColumnName("BordersNatural");
                entity.Property(t => t.Ertedad).HasColumnName("Ertedad");
                entity.Property(t => t.Brooz).HasColumnName("Brooz");
                entity.Property(t => t.AreaSak).HasColumnName("AreaSak");
                entity.Property(t => t.AreaNatural).HasColumnName("AreaNatural");
                entity.Property(t => t.AreaArrange).HasColumnName("AreaArrange");
                entity.Property(t => t.BuildingType).HasColumnName("BuildingType");
                entity.Property(t => t.BuildingPercent).HasColumnName("BuildingPercent");
                entity.Property(t => t.SpaceName).HasColumnName("SpaceName");
                entity.Property(t => t.Office).HasColumnName("Office");
                entity.Property(t => t.Usage).HasColumnName("Usage");
                entity.Property(t => t.Docpath).HasColumnName("Docpath");
                entity.Property(t => t.RegionTypeId).HasColumnName("RegionTypeId");
                entity.Property(t => t.elevators).HasColumnName("elevators");
                entity.Property(t => t.typ1).HasColumnName("typ1");
                entity.Property(t => t.brozat).HasColumnName("brozat");
                entity.Property(t => t.entries).HasColumnName("entries");
                entity.Property(t => t.Basement).HasColumnName("Basement");
                entity.Property(t => t.GroundFloor).HasColumnName("GroundFloor");
                entity.Property(t => t.FirstFloor).HasColumnName("FirstFloor");
                entity.Property(t => t.Motkrr).HasColumnName("Motkrr");
                entity.Property(t => t.FirstExtension).HasColumnName("FirstExtension");
                entity.Property(t => t.ExtensionName).HasColumnName("ExtensionName");
                entity.Property(t => t.GeneralLocation).HasColumnName("GeneralLocation");
                entity.Property(t => t.LicenseNo).HasColumnName("LicenseNo");
                entity.Property(t => t.Licensedate).HasColumnName("Licensedate");
                entity.Property(t => t.LicenseHijridate).HasColumnName("LicenseHijridate");
                entity.Property(t => t.DesiningOffice).HasColumnName("DesiningOffice");
                entity.Property(t => t.estsharyformoslhat).HasColumnName("estsharyformoslhat");
                entity.Property(t => t.Consultantfinishing).HasColumnName("Consultantfinishing");
                entity.Property(t => t.Period).HasColumnName("Period");
                entity.Property(t => t.punshmentamount).HasColumnName("punshmentamount");
                entity.Property(t => t.FirstPay).HasColumnName("FirstPay");
                entity.Property(t => t.LicenseContent).HasColumnName("LicenseContent");
                entity.Property(t => t.OtherStatus).HasColumnName("OtherStatus");
                entity.Property(t => t.AreaSpace).HasColumnName("AreaSpace");
                entity.Property(t => t.ContractorName).HasColumnName("ContractorName");
                entity.Property(t => t.ContractorMobile).HasColumnName("ContractorMobile");
                entity.Property(t => t.SupervisionSatartDate).HasColumnName("SupervisionSatartDate");
                entity.Property(t => t.SupervisionSatartHijriDate).HasColumnName("SupervisionSatartHijriDate");
                entity.Property(t => t.SupervisionEndDate).HasColumnName("SupervisionEndDate");
                entity.Property(t => t.SupervisionEndHijriDate).HasColumnName("SupervisionEndHijriDate");
                entity.Property(t => t.SupervisionNo).HasColumnName("SupervisionNo");
                entity.Property(t => t.SupervisionNotes).HasColumnName("SupervisionNotes");
                entity.Property(t => t.qaboqwaedmostlm).HasColumnName("qaboqwaedmostlm");
                entity.Property(t => t.qaboreqabmostlm).HasColumnName("qaboreqabmostlm");
                entity.Property(t => t.qabosaqfmostlm).HasColumnName("qabosaqfmostlm");
                entity.Property(t => t.molhqalwisaqffash).HasColumnName("molhqalwisaqffash");
                entity.Property(t => t.molhqalwisaqfdate).HasColumnName("molhqalwisaqfdate");
                entity.Property(t => t.molhqalwisaqfHijridate).HasColumnName("molhqalwisaqfHijridate");
                entity.Property(t => t.molhqalwisaqfmostlm).HasColumnName("molhqalwisaqfmostlm");
                entity.Property(t => t.molhqardisaqffash).HasColumnName("molhqardisaqffash");
                entity.Property(t => t.molhqardisaqfdate).HasColumnName("molhqardisaqfdate");
                entity.Property(t => t.molhqardisaqfHijridate).HasColumnName("molhqardisaqfHijridate");
                entity.Property(t => t.molhqardisaqfmostlm).HasColumnName("molhqardisaqfmostlm");
                entity.Property(t => t.FinalOrder).HasColumnName("FinalOrder");
                entity.Property(t => t.SpaceBuild).HasColumnName("SpaceBuild");
                entity.Property(t => t.FloorEstablishing).HasColumnName("FloorEstablishing");
                entity.Property(t => t.Roof).HasColumnName("Roof");
                entity.Property(t => t.Electric).HasColumnName("Electric");
                entity.Property(t => t.Takeef).HasColumnName("Takeef");
                entity.Property(t => t.ProjectNo).HasColumnName("ProjectNo");
                entity.Property(t => t.LimitDate).HasColumnName("LimitDate");
                entity.Property(t => t.LimitHijriDate).HasColumnName("LimitHijriDate");
                entity.Property(t => t.LimitDays).HasColumnName("LimitDays");
                entity.Property(t => t.NoteDate).HasColumnName("NoteDate");
                entity.Property(t => t.NoteHijriDate).HasColumnName("NoteHijriDate");
                entity.Property(t => t.ResponseEng).HasColumnName("ResponseEng");
                entity.Property(t => t.ReseveStatus).HasColumnName("ReseveStatus");
                entity.Property(t => t.kaeedno).HasColumnName("kaeedno");
                entity.Property(t => t.TechnicalDemands).HasColumnName("TechnicalDemands");
                entity.Property(t => t.Todoaction).HasColumnName("Todoaction");
                entity.Property(t => t.Responsible).HasColumnName("Responsible");
                entity.Property(t => t.ExternalEmpId).HasColumnName("ExternalEmpId");
                entity.Property(t => t.FinishDate).HasColumnName("FinishDate");
                entity.Property(t => t.FinishHijriDate).HasColumnName("FinishHijriDate");
                entity.Property(t => t.ContractPeriod).HasColumnName("ContractPeriod");
                entity.Property(t => t.SpaceNotes).HasColumnName("SpaceNotes");
                entity.Property(t => t.ContractNotes).HasColumnName("ContractNotes");
                entity.Property(t => t.SpaceId).HasColumnName("SpaceId");
                entity.Property(t => t.CityId).HasColumnName("CityId");
                entity.Property(t => t.ProjectDescription).HasColumnName("ProjectDescription");
                entity.Property(t => t.Paied).HasColumnName("Paied");
                entity.Property(t => t.Discount).HasColumnName("Discount");
                entity.Property(t => t.Fees).HasColumnName("Fees");
                entity.Property(t => t.ProjectTypeName).HasColumnName("ProjectTypeName");
                entity.Property(t => t.ProjectRegionName).HasColumnName("ProjectRegionName");
                entity.Property(t => t.Catego).HasColumnName("Catego");
                entity.Property(t => t.ContractPeriodType).HasColumnName("ContractPeriodType");
                entity.Property(t => t.ContractPeriodMinites).HasColumnName("ContractPeriodMinites");
                entity.Property(t => t.ProjectName).HasColumnName("ProjectName");
                entity.Property(t => t.ProjectValue).HasColumnName("ProjectValue");
                entity.Property(t => t.ProjectContractTawk).HasColumnName("ProjectContractTawk");
                entity.Property(t => t.ProjectRecieveLoaction).HasColumnName("ProjectRecieveLoaction");
                entity.Property(t => t.ProjectObserveName).HasColumnName("ProjectObserve");
                entity.Property(t => t.ProjectObserveMobile).HasColumnName("ProjectObserveMobile");
                entity.Property(t => t.ProjectObserveMail).HasColumnName("ProjectObserveMail");
                entity.Property(t => t.ProjectTaslemFirst).HasColumnName("ProjectTaslemFirst");
                entity.Property(t => t.FDamanID).HasColumnName("FDamanID");
                entity.Property(t => t.LDamanID).HasColumnName("LDamanID");
                entity.Property(t => t.NesbaEngaz).HasColumnName("NesbaEngaz");
                entity.Property(t => t.Takeem).HasColumnName("Takeem");
                entity.Property(t => t.ProjectContractTawkCh).HasColumnName("ProjectContractTawkCh");
                entity.Property(t => t.ProjectRecieveLoactionCh).HasColumnName("ProjectRecieveLoactionCh");
                entity.Property(t => t.ProjectTaslemFirstCh).HasColumnName("ProjectTaslemFirstCh");
                entity.Property(t => t.ContractCh).HasColumnName("ContractCh");
                entity.Property(t => t.PeriodProject).HasColumnName("PeriodProject");
                entity.Property(t => t.AgentDate).HasColumnName("AgentDate");
                entity.Property(t => t.AgentHijriDate).HasColumnName("AgentHijriDate");
                entity.Property(t => t.StreetName).HasColumnName("StreetName");
                entity.Property(t => t.MainText).HasColumnName("MainText");
                entity.Property(t => t.BranchText).HasColumnName("BranchText");
                entity.Property(t => t.TaskText).HasColumnName("TaskText");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ActiveMainPhaseId).HasColumnName("ActiveMainPhaseId");
                entity.Property(t => t.ActiveSubPhaseId).HasColumnName("ActiveSubPhaseId");
                entity.Property(t => t.NoOfDays).HasColumnName("NoOfDays");
                entity.Property(t => t.ReasonID).HasColumnName("ReasonID");
                entity.Property(t => t.ReasonText).HasColumnName("ReasonText");
                entity.Property(t => t.DateOfFinish).HasColumnName("DateOfFinish");
                entity.Property(t => t.FirstProjectDate).HasColumnName("FirstProjectDate");
                entity.Property(t => t.FirstProjectExpireDate).HasColumnName("FirstProjectExpireDate");
                entity.Property(t => t.Co_opOfficeName).HasColumnName("Co_opOfficeName");
                entity.Property(t => t.Co_opOfficeEmail).HasColumnName("Co_opOfficeEmail");
                entity.Property(t => t.Co_opOfficePhone).HasColumnName("Co_opOfficePhone");
                entity.Property(t => t.ContractorSelectId).HasColumnName("ContractorSelectId");
                entity.Property(t => t.CostCenterId).HasColumnName("CostCenterId");
                entity.Property(t => t.MunicipalId).HasColumnName("MunicipalId");
                entity.Property(t => t.SubMunicipalityId).HasColumnName("SubMunicipalityId");
                entity.Property(t => t.ProBuildingDisc).HasColumnName("ProBuildingDisc");
                entity.Property(t => t.StopProjectType).HasColumnName("StopProjectType");
                entity.Property(t => t.IsNotSent).HasColumnName("IsNotSent");
                entity.Property(t => t.OffersPricesId).HasColumnName("OffersPricesId");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.MotionProject).HasColumnName("MotionProject");
                entity.Property(t => t.MotionProjectDate).HasColumnName("MotionProjectDate");
                entity.Property(t => t.MotionProjectNote).HasColumnName("MotionProjectNote");
                entity.Property(t => t.Cons_components).HasColumnName("Cons_components");
                entity.Property(t => t.Plustimecount).HasColumnName("Plustimecount");
                entity.Property(t => t.SkipCount).HasColumnName("SkipCount");
                entity.Property(t => t.StopProjectDate).HasColumnName("StopProjectDate");
                entity.Property(t => t.ReasonsId).HasColumnName("ReasonsId");
                entity.Property(t => t.DestinationsUpload).HasColumnName("DestinationsUpload");


                modelBuilder.Entity<Project>().HasOne(s => s.city).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Project>().HasOne(s => s.Contractor).WithMany().HasForeignKey(e => e.ContractorSelectId);
                modelBuilder.Entity<Project>().HasOne(s => s.projecttype).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.projectsubtype).WithMany().HasForeignKey(e => e.SubProjectTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.Users).WithMany(x => x.Project).HasForeignKey(e => e.MangerId);
                modelBuilder.Entity<Project>().HasOne(s => s.AddUsers).WithMany(x => x.ProjectAdd).HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Project>().HasOne(s => s.UpdateUsers).WithMany(x => x.ProjectUpdate).HasForeignKey(e => e.UpdateUser);
                modelBuilder.Entity<Project>().HasOne(s => s.customer).WithMany(x=>x.Projects).HasForeignKey(e => e.CustomerId);

                modelBuilder.Entity<Project>().HasOne(s => s.transactionTypes).WithMany().HasForeignKey(e => e.TransactionTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.regionTypes).WithMany().HasForeignKey(e => e.RegionTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.projectPieces).WithMany().HasForeignKey(e => e.PieceNo);
                modelBuilder.Entity<Project>().HasOne(s => s.Contracts).WithMany().HasForeignKey(e => e.ContractId);
                modelBuilder.Entity<Project>().HasOne(s => s.OffersPrices).WithMany().HasForeignKey(e => e.OffersPricesId);
                modelBuilder.Entity<Project>().HasOne(s => s.Municipal).WithMany().HasForeignKey(e => e.MunicipalId);
                modelBuilder.Entity<Project>().HasOne(s => s.SubMunicipality).WithMany().HasForeignKey(e => e.SubMunicipalityId);
                modelBuilder.Entity<Project>().HasOne(s => s.projectsReasons).WithMany().HasForeignKey(e => e.ReasonsId);

                //modelBuilder.Entity<Project>().HasOne(s => s.ProUserPrivileges).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProjectPhasesTasks>(s => s.ProjectPhasesTasks).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<WorkOrders>(s => s.WorkOrders).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<Invoices>(s => s.Invoices).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProjectFiles>(s => s.ProjectFiles).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasOne(s => s.ActiveMainPhase).WithMany().HasForeignKey(e => e.ActiveMainPhaseId);
                modelBuilder.Entity<Project>().HasOne(s => s.ActiveSubPhase).WithMany().HasForeignKey(e => e.ActiveSubPhaseId);
                modelBuilder.Entity<Project>().HasMany<ProjectWorkers>(s => s.ProjectWorkers).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProUserPrivileges>(s => s.ProUserPrivileges).WithOne(g => g.Projects).HasForeignKey(s => s.ProjectID);
                modelBuilder.Entity<Project>().HasMany<DraftDetails>(s => s.DraftDetails).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProjectRequirementsGoals>(s => s.ProjectRequirementsGoals).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ImportantProject>(s => s.ImportantProjects).WithOne(g => g.project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasOne(s => s.Costcenter).WithMany().HasForeignKey(e => e.CostCenterId);
            });


            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_projectsReasons>(entity =>
            {
                entity.HasKey(e => e.ReasonsId);
                entity.ToTable("Pro_projectsReasons");
                entity.Property(e => e.NameAr).HasColumnName("NameAr");
                entity.Property(e => e.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_Destinations>(entity =>
            {
                entity.HasKey(e => e.DestinationId);
                //entity.ToTable("Pro_Destinations");
                 entity.ToTable("Pro_Destinations", t => t.HasTrigger("PhaseSendMailNoti"));
                 entity.ToTable("Pro_Destinations", t => t.HasTrigger("ProSendMailNoti"));

                modelBuilder.Entity<Pro_Destinations>().HasOne(s => s.User).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<Pro_Destinations>().HasOne(s => s.UserRec).WithMany().HasForeignKey(e => e.UserIdRec);
                modelBuilder.Entity<Pro_Destinations>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Pro_Destinations>().HasOne(s => s.DestinationType).WithMany().HasForeignKey(e => e.DestinationTypeId);
                modelBuilder.Entity<Pro_Destinations>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_filesReasons>(entity =>
            {
                entity.HasKey(e => e.ReasonsId);
                entity.ToTable("Pro_filesReasons");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_DestinationDepartments>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);
                entity.ToTable("Pro_DestinationDepartments");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_DestinationTypes>(entity =>
            {
                entity.HasKey(e => e.DestinationTypeId);
                entity.ToTable("Pro_DestinationTypes");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_TaskOperations>(entity =>
            {
                entity.HasKey(e => e.TaskOperationId);
                entity.ToTable("Pro_TaskOperations");
                modelBuilder.Entity<Pro_TaskOperations>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Pro_TaskOperations>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Pro_tasksReasons>(entity =>
            {
                entity.HasKey(e => e.ReasonsId);
                entity.ToTable("Pro_tasksReasons");
                entity.Property(e => e.NameAr).HasColumnName("NameAr");
                entity.Property(e => e.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectPieces>(entity =>
            {
                entity.HasKey(e => e.PieceId);
                entity.ToTable("Pro_ProjectPieces");
                entity.Property(t => t.PieceId).HasColumnName("PieceId");
                entity.Property(t => t.PieceNo).HasColumnName("PieceNo");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectRequirementsGoals>(entity =>
            {
                entity.HasKey(e => e.RequirementGoalId);
                entity.ToTable("Pro_ProjectRequirementsGoals");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.RequirementId).HasColumnName("RequirementId");

                modelBuilder.Entity<ProjectRequirementsGoals>().HasOne(s => s.Project).WithMany(s=>s.ProjectRequirementsGoals).HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectRequirementsGoals>().HasOne(s => s.RequirementsandGoals).WithMany().HasForeignKey(e => e.RequirementId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectRequirements>(entity =>
            {
                entity.HasKey(e => e.RequirementId);
                entity.ToTable("Pro_ProjectRequirements");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.ProjectSubTypeId).HasColumnName("ProjectSubTypeId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Cost).HasColumnName("Cost");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.PhasesTaskID).HasColumnName("PhasesTaskID");
                entity.Property(t => t.OrderId).HasColumnName("OrderId");
                entity.Property(t => t.NotifactionId).HasColumnName("NotifactionId");
                entity.Property(t => t.PageInsert).HasColumnName("PageInsert");


                modelBuilder.Entity<ProjectRequirements>().HasOne(s => s.projecttype).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<ProjectRequirements>().HasOne(s => s.projectsubtype).WithMany().HasForeignKey(e => e.ProjectSubTypeId);
                modelBuilder.Entity<ProjectRequirements>().HasOne(s => s.ProjectPhasesTasks).WithMany().HasForeignKey(e => e.PhasesTaskID);
                modelBuilder.Entity<ProjectRequirements>().HasOne(s => s.workOrders).WithMany().HasForeignKey(e => e.OrderId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectSubTypes>(entity =>
            {
                entity.HasKey(e => e.SubTypeId);
                entity.ToTable("Pro_ProjectSubTypes");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.TimePeriod).HasColumnName("TimePeriod");
          
                modelBuilder.Entity<ProjectSubTypes>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectTrailing>(entity =>
            {
                entity.HasKey(e => e.TrailingId);
                entity.ToTable("Pro_Trailing");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.Active).HasColumnName("Active");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.ReceiveDate).HasColumnName("ReceiveDate");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.ReceiveHijriDate).HasColumnName("ReceiveHijriDate");
                entity.Property(t => t.ReceiveUserId).HasColumnName("ReceiveUserId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.TaskId).HasColumnName("TaskId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<ProjectTrailing>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectTrailing>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.HasKey(e => e.TypeId);
                entity.ToTable("Pro_ProjectTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Typeum).HasColumnName("Typeum");
                entity.Property(t => t.TypeCode).HasColumnName("TypeCode");
                modelBuilder.Entity<ProjectType>().HasMany<RequirementsandGoals>(s => s.RequirementsandGoals).WithOne(g => g.ProjectType).HasForeignKey(s => s.ProjectTypeId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProjectWorkers>(entity =>
            {
                entity.HasKey(e => e.WorkerId);
                entity.ToTable("Pro_ProjectWorkers");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.WorkerType).HasColumnName("WorkerType");

                modelBuilder.Entity<ProjectWorkers>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ProjectWorkers>().HasOne(s => s.Project).WithMany(s => s.ProjectWorkers).HasForeignKey(e => e.ProjectId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProSettingDetails>(entity =>
            {
                entity.HasKey(e => e.ProSettingId);
                entity.ToTable("Pro_ProSettingDetails");
                entity.Property(t => t.ProSettingNo).HasColumnName("ProSettingNo");
                entity.Property(t => t.ProSettingNote).HasColumnName("ProSettingNote");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.ProjectSubtypeId).HasColumnName("ProjectSubtypeId");

                modelBuilder.Entity<ProSettingDetails>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<ProSettingDetails>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjectSubtypeId);
                modelBuilder.Entity<ProSettingDetails>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.AddUser);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProSettingDetailsNew>(entity =>
            {
                entity.HasKey(e => e.ProSettingId);
                entity.ToTable("Pro_ProSettingDetailsNew");
                entity.Property(t => t.ProSettingNo).HasColumnName("ProSettingNo");
                entity.Property(t => t.ProSettingNote).HasColumnName("ProSettingNote");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.ProjectSubtypeId).HasColumnName("ProjectSubtypeId");

                modelBuilder.Entity<ProSettingDetailsNew>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<ProSettingDetailsNew>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjectSubtypeId);
                modelBuilder.Entity<ProSettingDetailsNew>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.AddUser);

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
            modelBuilder.Entity<RegionTypes>(entity =>
            {
                entity.HasKey(e => e.RegionTypeId);
                entity.ToTable("Pro_RegionTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<RequirementsandGoals>(entity =>
            {
                entity.HasKey(e => e.RequirementId);
                entity.ToTable("Pro_RequirementsandGoals");
                entity.Property(t => t.RequirmentName).HasColumnName("RequirmentName");
                entity.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.ProjectTypeId).HasColumnName("ProjectTypeId");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");
                entity.Property(t => t.TimeNo).HasColumnName("TimeNo");
                entity.Property(t => t.TimeType).HasColumnName("TimeType");
                modelBuilder.Entity<RequirementsandGoals>().HasOne(s => s.ProjectType).WithMany(s=>s.RequirementsandGoals).HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<RequirementsandGoals>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<RequirementsandGoals>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.EmployeeId);
                modelBuilder.Entity<RequirementsandGoals>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Requirements>(entity =>
            {
                entity.HasKey(e => e.RequirementId);
                entity.ToTable("Pro_Requirements");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.ConfirmStatus).HasColumnName("ConfirmStatus");
                entity.Property(t => t.ConfirmStatusDate).HasColumnName("ConfirmStatusDate");
                entity.Property(t => t.Cost).HasColumnName("Cost");


                modelBuilder.Entity<Requirements>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Requirements>().HasOne(s => s.UpdateUsers).WithMany().HasForeignKey(e => e.UpdateUser);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.ServiceId);
                entity.ToTable("Acc_Services");
                entity.Property(t => t.Number).HasColumnName("Number");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.ExpireDate).HasColumnName("ExpireDate");
                entity.Property(t => t.ExpireHijriDate).HasColumnName("ExpireHijriDate");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.NotifyCount).HasColumnName("NotifyCount");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.BankId).HasColumnName("BankId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.RepeatAlarm).HasColumnName("RepeatAlarm");
                entity.Property(t => t.RecurrenceRateId).HasColumnName("RecurrenceRateId");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");

                modelBuilder.Entity<Service>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
                modelBuilder.Entity<Service>().HasOne(s => s.Banks).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Service>().HasOne(s => s.Account).WithMany().HasForeignKey(e => e.AccountId);

                modelBuilder.Entity<Service>().HasMany<Contracts>(s => s.Contracts).WithOne(g => g.Service).HasForeignKey(s => s.ServiceId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ServicesPricingForm>(entity =>
            {
                entity.HasKey(e => e.FormId);
                entity.ToTable("Sys_ServicesPricingForm");
                entity.Property(t => t.FormId).HasColumnName("FormId");
                entity.Property(t => t.Name).HasColumnName("Name");
                entity.Property(t => t.Email).HasColumnName("Email");
                entity.Property(t => t.Mobile).HasColumnName("Mobile");
                entity.Property(t => t.LandArea).HasColumnName("LandArea");
                entity.Property(t => t.NumofStreetsAdjacent).HasColumnName("NumofStreetsAdjacent");
                entity.Property(t => t.Height).HasColumnName("Height");
                entity.Property(t => t.Width).HasColumnName("Width");
                entity.Property(t => t.FormType).HasColumnName("FormType");
                entity.Property(t => t.ServiceNotes).HasColumnName("ServiceNotes");
                entity.Property(t => t.Bas_swimmingPool).HasColumnName("Bas_swimmingPool");
                entity.Property(t => t.Bas_board).HasColumnName("Bas_board");
                entity.Property(t => t.Bas_garden).HasColumnName("Bas_garden");
                entity.Property(t => t.Bas_LaundryRoom).HasColumnName("Bas_LaundryRoom");
                entity.Property(t => t.Bas_storehouse).HasColumnName("Bas_storehouse");
                entity.Property(t => t.Bas_carparking).HasColumnName("Bas_carparking");
                entity.Property(t => t.Bas_Desk).HasColumnName("Bas_Desk");
                entity.Property(t => t.Bas_openkitchen).HasColumnName("Bas_openkitchen");
                entity.Property(t => t.Bas_closedkitchen).HasColumnName("Bas_closedkitchen");
                entity.Property(t => t.Bas_bedroomonly).HasColumnName("Bas_bedroomonly");
                entity.Property(t => t.Bas_Bedandbathroom).HasColumnName("Bas_Bedandbathroom");
                entity.Property(t => t.Bas_Healthclub).HasColumnName("Bas_Healthclub");
                entity.Property(t => t.Bas_Multipurposehall).HasColumnName("Bas_Multipurposehall");
                entity.Property(t => t.Bas_Kidsplayroom).HasColumnName("Bas_Kidsplayroom");
                entity.Property(t => t.Bas_Homecinemahall).HasColumnName("Bas_Homecinemahall");
                entity.Property(t => t.Bas_Notes).HasColumnName("Bas_Notes");
                entity.Property(t => t.Gro_familyliving).HasColumnName("Gro_familyliving");
                entity.Property(t => t.Gro_guestdiningroom).HasColumnName("Gro_guestdiningroom");
                entity.Property(t => t.Gro_guestcouncil).HasColumnName("Gro_guestcouncil");
                entity.Property(t => t.Gro_closedkitchen).HasColumnName("Gro_closedkitchen");
                entity.Property(t => t.Gro_Desk).HasColumnName("Gro_Desk");
                entity.Property(t => t.Gro_familydiningroom).HasColumnName("Gro_familydiningroom");
                entity.Property(t => t.Gro_elevator).HasColumnName("Gro_elevator");
                entity.Property(t => t.Gro_Store).HasColumnName("Gro_Store");
                entity.Property(t => t.Gro_openkitchen).HasColumnName("Gro_openkitchen");
                entity.Property(t => t.Gro_SubEntrance).HasColumnName("Gro_SubEntrance");
                entity.Property(t => t.Gro_MainEntrance).HasColumnName("Gro_MainEntrance");
                entity.Property(t => t.Gro_Toilets).HasColumnName("Gro_Toilets");
                entity.Property(t => t.Gro_Extrabedandbathroom).HasColumnName("Gro_Extrabedandbathroom");
                entity.Property(t => t.Gro_servicedrawer).HasColumnName("Gro_servicedrawer");
                entity.Property(t => t.Gro_maindrawer).HasColumnName("Gro_maindrawer");
                entity.Property(t => t.Gro_Laundryandironingroom).HasColumnName("Gro_Laundryandironingroom");
                entity.Property(t => t.Gro_Maidsroomandbathroom).HasColumnName("Gro_Maidsroomandbathroom");
                entity.Property(t => t.Gro_Extrabedroomonly).HasColumnName("Gro_Extrabedroomonly");
                entity.Property(t => t.Gro_Numberofguestboards).HasColumnName("Gro_Numberofguestboards");
                entity.Property(t => t.Gro_Numberoftoilets).HasColumnName("Gro_Numberoftoilets");
                entity.Property(t => t.Gro_Notes).HasColumnName("Gro_Notes");
                entity.Property(t => t.Firrou_bedroomonly).HasColumnName("Firrou_bedroomonly");
                entity.Property(t => t.Firrou_bedandbathroom).HasColumnName("Firrou_bedandbathroom");
                entity.Property(t => t.Firrou_Masterbedroomsuite).HasColumnName("Firrou_Masterbedroomsuite");
                entity.Property(t => t.Firrou_Smallandopenkitchen).HasColumnName("Firrou_Smallandopenkitchen");
                entity.Property(t => t.Firrou_familysitting).HasColumnName("Firrou_familysitting");
                entity.Property(t => t.Firrou_WC).HasColumnName("Firrou_WC");
                entity.Property(t => t.Firrou_Gym).HasColumnName("Firrou_Gym");
                entity.Property(t => t.Firrou_serviceroom).HasColumnName("Firrou_serviceroom");
                entity.Property(t => t.Firrou_Desk).HasColumnName("Firrou_Desk");
                entity.Property(t => t.Firrou_Numofbednadbathroom).HasColumnName("Firrou_Numofbednadbathroom");
                entity.Property(t => t.Firrou_Numofbedroomonly).HasColumnName("Firrou_Numofbedroomonly");
                entity.Property(t => t.Firrou_Notes).HasColumnName("Firrou_Notes");
                entity.Property(t => t.Sur_storehouse).HasColumnName("Sur_storehouse");
                entity.Property(t => t.Sur_WC).HasColumnName("Sur_WC");
                entity.Property(t => t.Sur_surface).HasColumnName("Sur_surface");
                entity.Property(t => t.Sur_Maidsbedroom).HasColumnName("Sur_Maidsbedroom");
                entity.Property(t => t.Sur_Laundryandironingroom).HasColumnName("Sur_Laundryandironingroom");
                entity.Property(t => t.Sur_multiusehall).HasColumnName("Sur_multiusehall");
                entity.Property(t => t.Sur_Notes).HasColumnName("Sur_Notes");
                entity.Property(t => t.Gar_waterbodyonly).HasColumnName("Gar_waterbodyonly");
                entity.Property(t => t.Gar_Externalstaircase).HasColumnName("Gar_Externalstaircase");
                entity.Property(t => t.Gar_Childrensplayarea).HasColumnName("Gar_Childrensplayarea");
                entity.Property(t => t.Gar_outdoorsitting).HasColumnName("Gar_outdoorsitting");
                entity.Property(t => t.Gar_swimmingpool).HasColumnName("Gar_swimmingpool");
                entity.Property(t => t.Ext_Council).HasColumnName("Ext_Council");
                entity.Property(t => t.Ext_Multipurposehall).HasColumnName("Ext_Multipurposehall");
                entity.Property(t => t.Ext_WC).HasColumnName("Ext_WC");
                entity.Property(t => t.Par_OneCar).HasColumnName("Par_OneCar");
                entity.Property(t => t.Par_TwoCar).HasColumnName("Par_TwoCar");
                entity.Property(t => t.Par_MoreCars).HasColumnName("Par_MoreCars");
                entity.Property(t => t.Par_NoofCars).HasColumnName("Par_NoofCars");
                entity.Property(t => t.Dri_OneDriversroomtoilet).HasColumnName("Dri_OneDriversroomtoilet");
                entity.Property(t => t.Dri_TwoDriversroomwithtoilets).HasColumnName("Dri_TwoDriversroomwithtoilets");
                entity.Property(t => t.Dri_MoreDriversroomwithtoilets).HasColumnName("Dri_MoreDriversroomwithtoilets");
                entity.Property(t => t.Dri_NoofDriversroomwithtoilets).HasColumnName("Dri_NoofDriversroomwithtoilets");
                entity.Property(t => t.AnotherNotes).HasColumnName("AnotherNotes");
                entity.Property(t => t.FormStatus).HasColumnName("FormStatus");
                entity.Property(t => t.ServiceDate).HasColumnName("ServiceDate");
                entity.Property(t => t.URLFile).HasColumnName("URLFile");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Settings>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Pro_Settings");

                entity.Property(t => t.DescriptionAr).HasColumnName("DescriptionAr");
                entity.Property(t => t.DescriptionEn).HasColumnName("DescriptionEn");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.TimeMinutes).HasColumnName("TimeMinutes");
                entity.Property(t => t.IsUrgent).HasColumnName("IsUrgent");
                entity.Property(t => t.IsTemp).HasColumnName("IsTemp");
                entity.Property(t => t.TaskType).HasColumnName("TaskType");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.PercentComplete).HasColumnName("PercentComplete");
                entity.Property(t => t.Cost).HasColumnName("Cost");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.TimeType).HasColumnName("TimeType");
                entity.Property(t => t.LocationId).HasColumnName("LocationId");
                entity.Property(t => t.CopySettingId).HasColumnName("CopySettingId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.Priority).HasColumnName("Priority");
                entity.Property(t => t.ExecPercentage).HasColumnName("ExecPercentage");
                entity.Property(t => t.IsMerig).HasColumnName("IsMerig");
                entity.Property(t => t.EndTime).HasColumnName("EndTime");
                entity.Property(t => t.RequirmentId).HasColumnName("RequirmentId");

                modelBuilder.Entity<Settings>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<Settings>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<Settings>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<Settings>().HasOne(s => s.TaskTypeModel).WithMany().HasForeignKey(e => e.TaskType);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<SettingsNew>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Pro_SettingsNew");

                entity.Property(t => t.DescriptionAr).HasColumnName("DescriptionAr");
                entity.Property(t => t.DescriptionEn).HasColumnName("DescriptionEn");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.TimeMinutes).HasColumnName("TimeMinutes");
                entity.Property(t => t.IsUrgent).HasColumnName("IsUrgent");
                entity.Property(t => t.IsTemp).HasColumnName("IsTemp");
                entity.Property(t => t.TaskType).HasColumnName("TaskType");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.PercentComplete).HasColumnName("PercentComplete");
                entity.Property(t => t.Cost).HasColumnName("Cost");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.TimeType).HasColumnName("TimeType");
                entity.Property(t => t.LocationId).HasColumnName("LocationId");
                entity.Property(t => t.CopySettingId).HasColumnName("CopySettingId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.Priority).HasColumnName("Priority");
                entity.Property(t => t.ExecPercentage).HasColumnName("ExecPercentage");
                entity.Property(t => t.IsMerig).HasColumnName("IsMerig");
                entity.Property(t => t.EndTime).HasColumnName("EndTime");
                entity.Property(t => t.RequirmentId).HasColumnName("RequirmentId");

                modelBuilder.Entity<SettingsNew>().HasOne(s => s.Employees).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<SettingsNew>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<SettingsNew>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<SettingsNew>().HasOne(s => s.TaskTypeModel).WithMany().HasForeignKey(e => e.TaskType);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<SMSSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Sys_SMSSettings");
                entity.Property(t => t.MobileNo).HasColumnName("MobileNo");
                entity.Property(t => t.Password).HasColumnName("Password");
                entity.Property(t => t.SenderName).HasColumnName("SenderName");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ApiUrl).HasColumnName("ApiUrl");
                entity.Property(t => t.UserName).HasColumnName("UserName");
                entity.Property(t => t.SendCustomerSMS).HasColumnName("SendCustomerSMS");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<SocialMediaLinks>(entity =>
            {
                entity.HasKey(e => e.LinksId);
                entity.ToTable("Sys_SocialMediaLinks");
                entity.Property(t => t.FaceBookLink).HasColumnName("FaceBookLink");
                entity.Property(t => t.TwitterLink).HasColumnName("TwitterLink");
                entity.Property(t => t.GooglePlusLink).HasColumnName("GooglePlusLink");
                entity.Property(t => t.InstagramLink).HasColumnName("InstagramLink");
                entity.Property(t => t.LinkedInLink).HasColumnName("LinkedInLink");
                entity.Property(t => t.SnapchatLink).HasColumnName("SnapchatLink");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Supervisions>(entity =>
            {
                entity.HasKey(e => e.SupervisionId);
                entity.ToTable("Pro_Supervisions");
                entity.Property(t => t.Number).HasColumnName("Number");
                entity.Property(t => t.Phase).HasColumnName("Phase");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.Location).HasColumnName("Location");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.ReceiveNotes).HasColumnName("ReceiveNotes");
                entity.Property(t => t.ManagerNotes).HasColumnName("ManagerNotes");
                entity.Property(t => t.ReceivedEmpId).HasColumnName("ReceivedEmpId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.ReceiveStatus).HasColumnName("ReceiveStatus");
                entity.Property(t => t.ReceiveDate).HasColumnName("ReceiveDate");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");
                entity.Property(t => t.PhaseId).HasColumnName("PhaseId");
                entity.Property(t => t.PieceNo).HasColumnName("PieceNo");
                entity.Property(t => t.LicenseNo).HasColumnName("LicenseNo");
                entity.Property(t => t.OutlineNo).HasColumnName("OutlineNo");
                entity.Property(t => t.WorkerId).HasColumnName("WorkerId");
                entity.Property(t => t.VisitDate).HasColumnName("VisitDate");
                entity.Property(t => t.SuperStatus).HasColumnName("SuperStatus");
                entity.Property(t => t.SuperDateConfirm).HasColumnName("SuperDateConfirm");
                entity.Property(t => t.MunicipalSelectId).HasColumnName("MunicipalSelectId");
                entity.Property(t => t.SubMunicipalitySelectId).HasColumnName("SubMunicipalitySelectId");
                entity.Property(t => t.ProBuildingTypeSelectId).HasColumnName("ProBuildingTypeSelectId");
                entity.Property(t => t.DistrictName).HasColumnName("DistrictName");
                entity.Property(t => t.ProBuildingDisc).HasColumnName("ProBuildingDisc");
                entity.Property(t => t.AdwARid).HasColumnName("AdwARid");
                entity.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
                entity.Property(t => t.ImageUrl2).HasColumnName("ImageUrl2");
                entity.Property(t => t.OutlineChangetxt1).HasColumnName("OutlineChangetxt1");
                entity.Property(t => t.OutlineChangetxt2).HasColumnName("OutlineChangetxt2");
                entity.Property(t => t.OutlineChangetxt3).HasColumnName("OutlineChangetxt3");
                entity.Property(t => t.PointsNotWrittentxt1).HasColumnName("PointsNotWrittentxt1");
                entity.Property(t => t.PointsNotWrittentxt2).HasColumnName("PointsNotWrittentxt2");
                entity.Property(t => t.PointsNotWrittentxt3).HasColumnName("PointsNotWrittentxt3");
                entity.Property(t => t.RequiredServiceId).HasColumnName("RequiredServiceId");

                modelBuilder.Entity<Supervisions>().HasOne(s => s.Municipal).WithMany().HasForeignKey(e => e.MunicipalSelectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.SubMunicipality).WithMany().HasForeignKey(e => e.SubMunicipalitySelectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.BuildTypes).WithMany().HasForeignKey(e => e.ProBuildingTypeSelectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.ReceivedEmpId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.Pro_Super_Phases).WithMany().HasForeignKey(e => e.PhaseId);
                modelBuilder.Entity<Supervisions>().HasMany<Pro_SupervisionDetails>(s => s.SupervisionDetails).WithOne(g => g.Supervisions).HasForeignKey(s => s.SupervisionId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<SupportResquests>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.ToTable("Sys_SupportResquests");
                entity.Property(t => t.Address).HasColumnName("Address");
                entity.Property(t => t.Topic).HasColumnName("Topic");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.OrganizationId).HasColumnName("OrganizationId");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.Department).HasColumnName("Department");
                entity.Property(t => t.priority).HasColumnName("Spriority");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Sys_SystemActions>(entity =>
            {
                entity.HasKey(e => e.SysID);
                entity.ToTable("Sys_SystemActions");
                entity.Property(t => t.FunctionName).HasColumnName("FunctionName");
                entity.Property(t => t.ServiceName).HasColumnName("ServiceName");
                entity.Property(t => t.ActionType).HasColumnName("ActionType");
                entity.Property(t => t.MessageName).HasColumnName("MessageName");
                entity.Property(t => t.ModuleName).HasColumnName("ModuleName");
                entity.Property(t => t.PageName).HasColumnName("PageName");
                entity.Property(t => t.ActionDate).HasColumnName("ActionDate");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.Success).HasColumnName("Success");
                modelBuilder.Entity<Sys_SystemActions>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<Sys_SystemActions>().HasOne(s => s.Branches).WithMany().HasForeignKey(e => e.BranchId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<SystemSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Sys_SystemSettings");
                entity.Property(t => t.FiscalYear).HasColumnName("FiscalYear");
                entity.Property(t => t.ContractGenerateCode).HasColumnName("ContractGenerateCode");
                entity.Property(t => t.CurrencyId).HasColumnName("CurrencyId");
                entity.Property(t => t.CustGenerateCode).HasColumnName("CustGenerateCode");
                entity.Property(t => t.AttendenceId).HasColumnName("AttendenceId");
                entity.Property(t => t.BranchGenerateCode).HasColumnName("BranchGenerateCode");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.VoucherGenerateCode).HasColumnName("VoucherGenerateCode");
                entity.Property(t => t.NoReplyMail).HasColumnName("NoReplyMail");
                entity.Property(t => t.ProjGenerateCode).HasColumnName("ProjGenerateCode");
                entity.Property(t => t.OfferGenerateCode).HasColumnName("OfferGenerateCode");
                entity.Property(t => t.ActiveCodeInterval).HasColumnName("ActiveCodeInterval");
                entity.Property(t => t.ActiveUserNumber).HasColumnName("ActiveUserNumber");
                entity.Property(t => t.DecimalPoints).HasColumnName("DecimalPoints");
                entity.Property(t => t.EmpGenerateCode).HasColumnName("EmpGenerateCode");
                entity.Property(t => t.LogErrors).HasColumnName("LogErrors");
                entity.Property(t => t.EnableNotification).HasColumnName("EnableNotification");
                entity.Property(t => t.EnableSMS).HasColumnName("EnableSMS");
                entity.Property(t => t.SMTPPort).HasColumnName("SMTPPort");
                entity.Property(t => t.DefaultUserSession).HasColumnName("DefaultUserSession");
                entity.Property(t => t.MobileNoDigits).HasColumnName("MobileNoDigits");
                entity.Property(t => t.PhoneNoDigits).HasColumnName("PhoneNoDigits");
                entity.Property(t => t.NationalIDDigits).HasColumnName("NationalIDDigits");
                entity.Property(t => t.ContractGenerateCode2).HasColumnName("ContractGenerateCode2");
                entity.Property(t => t.CustomerMailIsRequired).HasColumnName("CustomerMailIsRequired");
                entity.Property(t => t.CustomerNationalIdIsRequired).HasColumnName("CustomerNationalIdIsRequired");
                entity.Property(t => t.OrgDataIsRequired).HasColumnName("OrgDataIsRequired");
                entity.Property(t => t.CustomerphoneIsRequired).HasColumnName("CustomerphoneIsRequired");
                entity.Property(t => t.Contract_Con_Code).HasColumnName("Contract_Con_Code");
                entity.Property(t => t.Contract_Sup_Code).HasColumnName("Contract_Sup_Code");
                entity.Property(t => t.Contract_Des_Code).HasColumnName("Contract_Des_Code");
                entity.Property(t => t.UploadInvZatca).HasColumnName("UploadInvZatca");
                entity.Property(t => t.ZatcaCheckCode).HasColumnName("ZatcaCheckCode");
                entity.Property(t => t.DestinationCheckCode).HasColumnName("DestinationCheckCode");
                entity.Property(t => t.ContractEndNote).HasColumnName("ContractEndNote");
                entity.Property(t => t.ResedentEndNote).HasColumnName("ResedentEndNote");
                entity.Property(t => t.ValueAddedSeparated).HasColumnName("ValueAddedSeparated");


                modelBuilder.Entity<SystemSettings>().HasOne(s => s.UpdateUserT).WithMany().HasForeignKey(e => e.UpdateUser);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<TasksDependency>(entity =>
            {
                entity.HasKey(e => e.DependencyId);
                entity.ToTable("Pro_Dependency");
                entity.Property(t => t.PredecessorId).HasColumnName("PredecessorId");
                entity.Property(t => t.SuccessorId).HasColumnName("SuccessorId");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                modelBuilder.Entity<TasksDependency>().HasOne(s => s.SettingsPredecessor).WithMany().HasForeignKey(e => e.PredecessorId);
                modelBuilder.Entity<TasksDependency>().HasOne(s => s.SettingsSuccessor).WithMany().HasForeignKey(e => e.SuccessorId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<TaskType>(entity =>
            {
                entity.HasKey(e => e.TaskTypeId);
                entity.ToTable("Pro_TaskType");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Templates>(entity =>
            {
                entity.HasKey(e => e.TemplateId);
                entity.ToTable("Pro_Templates");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<TemplatesTasks>(entity =>
            {
                entity.HasKey(e => e.TemplateTaskId);
                entity.ToTable("Pro_TemplatesTasks");
                entity.Property(t => t.DescriptionAr).HasColumnName("DescriptionAr");
                entity.Property(t => t.DescriptionEn).HasColumnName("DescriptionEn");
                entity.Property(t => t.ParentId).HasColumnName("ParentId");
                entity.Property(t => t.TemplateId).HasColumnName("TemplateId");
                entity.Property(t => t.ProjSubTypeId).HasColumnName("ProjSubTypeId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.TimeMinutes).HasColumnName("TimeMinutes");
                entity.Property(t => t.TimeType).HasColumnName("TimeType");
                entity.Property(t => t.Remaining).HasColumnName("Remaining");
                entity.Property(t => t.IsUrgent).HasColumnName("IsUrgent");
                entity.Property(t => t.IsTemp).HasColumnName("IsTemp");
                entity.Property(t => t.TaskType).HasColumnName("TaskType");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.OldStatus).HasColumnName("OldStatus");
                entity.Property(t => t.Active).HasColumnName("Active");
                entity.Property(t => t.StopCount).HasColumnName("StopCount");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.StartDate).HasColumnName("StartDate");
                entity.Property(t => t.EndDate).HasColumnName("EndDate");
                entity.Property(t => t.PercentComplete).HasColumnName("PercentComplete");
                entity.Property(t => t.Cost).HasColumnName("Cost");
                entity.Property(t => t.ToUserId).HasColumnName("ToUserId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.SettingId).HasColumnName("SettingId");
                entity.Property(t => t.LocationId).HasColumnName("LocationId");
                entity.Property(t => t.ParentSettingId).HasColumnName("ParentSettingId");

                modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjSubTypeId);
                //modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.MainPhase).WithMany().HasForeignKey(e => e.ProjSubTypeId);
                modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.SubPhase).WithMany().HasForeignKey(e => e.ParentId);
                modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<TemplatesTasks>().HasOne(s => s.Settings).WithMany().HasForeignKey(e => e.SettingId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<TimeOutRequests>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.ToTable("Pro_TimeOutRequests");
                entity.Property(t => t.Address).HasColumnName("Address");
                entity.Property(t => t.Duration).HasColumnName("Duration");
                entity.Property(t => t.Reason).HasColumnName("Reason");
                entity.Property(t => t.AttachmentUrl).HasColumnName("AttachmentUrl");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.ActionUserId).HasColumnName("ActionUserId");
                entity.Property(t => t.TaskId).HasColumnName("TaskId");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.Comment).HasColumnName("Comment");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<TimeOutRequests>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<TrailingFiles>(entity =>
            {
                entity.HasKey(e => e.FileId);
                entity.ToTable("Pro_TrailingFiles");
                entity.Property(t => t.FileName).HasColumnName("FileName");
                entity.Property(t => t.FileUrl).HasColumnName("FileUrl");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.TrailingId).HasColumnName("TrailingId");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.TransactionId);
                entity.ToTable("Acc_Transactions");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");
                entity.Property(t => t.TransactionDate).HasColumnName("TransactionDate");
                entity.Property(t => t.TransactionHijriDate).HasColumnName("TransactionHijriDate");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.Credit).HasColumnName("Credit");
                entity.Property(t => t.Depit).HasColumnName("Depit");
                entity.Property(t => t.CostCenterId).HasColumnName("CostCenterId");
                entity.Property(t => t.CurrentBalance).HasColumnName("CurrentBalance");
                entity.Property(t => t.Details).HasColumnName("Details");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.IsPost).HasColumnName("IsPost");
                entity.Property(t => t.YearId).HasColumnName("YearId");
                entity.Property(t => t.InvoiceReference).HasColumnName("InvoiceReference");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.PaymentId).HasColumnName("PaymentId");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.AccountType).HasColumnName("AccountType");
                entity.Property(t => t.JournalNo).HasColumnName("JournalNo");
                entity.Property(t => t.RecycleYearTo).HasColumnName("RecycleYearTo");
                entity.Property(t => t.RecycleStatus).HasColumnName("RecycleStatus");
                entity.Property(t => t.AccCalcExpen).HasColumnName("AccCalcExpen");
                entity.Property(t => t.AccCalcIncome).HasColumnName("AccCalcIncome");
                entity.Property(t => t.AccTransactionDate).HasColumnName("AccTransactionDate");
                entity.Property(t => t.AccTransactionHijriDate).HasColumnName("AccTransactionHijriDate");
                entity.Property(t => t.VoucherDetailsId).HasColumnName("VoucherDetailsId");

                modelBuilder.Entity<Transactions>().HasOne(s => s.Invoices).WithMany(s => s.TransactionDetails).HasForeignKey(e => e.InvoiceId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.Accounts).WithMany(s=>s.Transactions).HasForeignKey(e => e.AccountId).IsRequired(false);
                modelBuilder.Entity<Transactions>().HasOne(s => s.CostCenters).WithMany(s=>s.Transactions).HasForeignKey(e => e.CostCenterId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.AccTransactionTypes).WithMany().HasForeignKey(e => e.Type);
                modelBuilder.Entity<Transactions>().HasOne(s => s.Customer).WithMany(s => s.Transactions).HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.Contracts).WithMany(s => s.TransactionDetails).HasForeignKey(e => e.ContractId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.DiscountReward).WithMany(s => s.TransactionDetails).HasForeignKey(e => e.DiscountRewardId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<TransactionTypes>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId);
                entity.ToTable("Pro_TransactionTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<UserBranches>(entity =>
            {
                entity.HasKey(e => e.UserBranchId);
                entity.ToTable("Sys_UserBranches");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");

                modelBuilder.Entity<UserBranches>().HasOne(s => s.Branches).WithMany().HasForeignKey(e => e.BranchId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<UserMails>(entity =>
            {
                entity.HasKey(e => e.MailId);
                entity.ToTable("Sys_UserMails");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.SenderUserId).HasColumnName("SenderUserId");
                entity.Property(t => t.MailText).HasColumnName("MailText");
                entity.Property(t => t.MailSubject).HasColumnName("MailSubject");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
                entity.Property(t => t.AllUsers).HasColumnName("AllUsers");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.IsRead).HasColumnName("IsRead");

                modelBuilder.Entity<UserMails>().HasOne(s => s.SendUsers).WithMany().HasForeignKey(e => e.SenderUserId);
                modelBuilder.Entity<UserMails>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<UserNotificationPrivileges>(entity =>
            {
                entity.HasKey(e => e.UserPrivId);
                entity.ToTable("Sys_UserNotificationPrivileges");
                entity.Property(t => t.PrivilegeId).HasColumnName("PrivilegeId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Select).HasColumnName("Select");
                entity.Property(t => t.Insert).HasColumnName("Insert");
                entity.Property(t => t.Update).HasColumnName("Update");
                entity.Property(t => t.Delete).HasColumnName("Delete");

                modelBuilder.Entity<UserNotificationPrivileges>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<UserPrivileges>(entity =>
            {
                entity.HasKey(e => e.UserPrivId);
                entity.ToTable("Sys_UserPrivileges");
                entity.Property(t => t.PrivilegeId).HasColumnName("PrivilegeId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Select).HasColumnName("Select");
                entity.Property(t => t.Insert).HasColumnName("Insert");
                entity.Property(t => t.Update).HasColumnName("Update");
                entity.Property(t => t.Delete).HasColumnName("Delete");

                modelBuilder.Entity<UserNotificationPrivileges>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<UsersLocations>(entity =>
            {
                entity.HasKey(e => e.LocationId);
                entity.ToTable("Sys_UsersLocations");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.Ip).HasColumnName("Ip");
                entity.Property(t => t.IPType).HasColumnName("IPType");
                entity.Property(t => t.City).HasColumnName("City");
                entity.Property(t => t.ContinentCode).HasColumnName("ContinentCode");
                entity.Property(t => t.ContinentName).HasColumnName("ContinentName");
                entity.Property(t => t.CountryCode).HasColumnName("CountryCode");
                entity.Property(t => t.CountryName).HasColumnName("CountryName");
                entity.Property(t => t.RegionCode).HasColumnName("RegionCode");
                entity.Property(t => t.RegionName).HasColumnName("RegionName");
                entity.Property(t => t.ZipCode).HasColumnName("ZipCode");
                entity.Property(t => t.Latitude).HasColumnName("Latitude");
                entity.Property(t => t.Longitude).HasColumnName("Longitude");
                entity.Property(t => t.TimeZone).HasColumnName("TimeZone");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("Sys_Users");
                entity.Property(t => t.FullName).HasColumnName("FullName");
                entity.Property(t => t.JobId).HasColumnName("JobId");
                entity.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
                entity.Property(t => t.Email).HasColumnName("Email");
                entity.Property(t => t.Mobile).HasColumnName("Mobile");
                entity.Property(t => t.GroupId).HasColumnName("GroupId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ImgUrl).HasColumnName("ImgUrl");
                entity.Property(t => t.EmpId).HasColumnName("EmpId");
                entity.Property(t => t.UserName).HasColumnName("UserName");
                entity.Property(t => t.Password).HasColumnName("Password");
                entity.Property(t => t.Status).HasColumnName("Status");
                entity.Property(t => t.Notes).HasColumnName("Notes");
                entity.Property(t => t.IsOnline).HasColumnName("IsOnline");
                entity.Property(t => t.LastSeenDate).HasColumnName("LastSeenDate");
                entity.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
                entity.Property(t => t.LastLogOutDate).HasColumnName("LastLogOutDate");
                entity.Property(t => t.ActivationCode).HasColumnName("ActivationCode");
                entity.Property(t => t.VisualCode).HasColumnName("VisualCode");
                entity.Property(t => t.Session).HasColumnName("Session");
                entity.Property(t => t.IsAdmin).HasColumnName("IsAdmin");
                entity.Property(t => t.ExpireDate).HasColumnName("ExpireDate");
                entity.Property(t => t.ISOnlineNew).HasColumnName("ISOnlineNew");
                entity.Property(t => t.ActiveTime).HasColumnName("ActiveTime");
                entity.Property(t => t.StampUrl).HasColumnName("StampUrl");
                entity.Property(t => t.TimeId).HasColumnName("TimeId");
                entity.Property(t => t.AccStatusConfirm).HasColumnName("AccStatusConfirm");
                entity.Property(t => t.FullNameAr).HasColumnName("FullNameAr");
                entity.Property(t => t.EncryptedCode).HasColumnName("EncryptedCode");
                entity.Property(t => t.IsActivated).HasColumnName("IsActivated");
                entity.Property(t => t.SupEngineerName).HasColumnName("SupEngineerName");
                entity.Property(t => t.SupEngineerNationalId).HasColumnName("SupEngineerNationalId");
                entity.Property(t => t.SupEngineerCert).HasColumnName("SupEngineerCert");
                entity.Property(t => t.AppearWelcome).HasColumnName("AppearWelcome");
                entity.Property(t => t.QrCodeUrl).HasColumnName("QrCodeUrl");
                entity.Property(t => t.DeviceType).HasColumnName("DeviceType");
                entity.Property(t => t.DeviceTokenId).HasColumnName("DeviceTokenId");
                entity.Property(t => t.AppearInInvoicePrint).HasColumnName("AppearInInvoicePrint");
                entity.Property(t => t.DeviceId).HasColumnName("DeviceId");

                //modelBuilder.Entity<Users>().HasMany<int>(s => s.BranchesList).WithOne(g => g.Users).HasForeignKey(s => s.UserId);
                modelBuilder.Entity<Users>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
                modelBuilder.Entity<Users>().HasOne(s => s.Groups).WithMany().HasForeignKey(e => e.GroupId);
                modelBuilder.Entity<Users>().HasOne(s => s.Jobs).WithMany().HasForeignKey(e => e.JobId);
                modelBuilder.Entity<Users>().HasOne(s => s.Branches).WithMany().HasForeignKey(e => e.BranchId);

                modelBuilder.Entity<Users>().HasMany<UserPrivileges>(s => s.UserPrivileges).WithOne(g => g.Users).HasForeignKey(s => s.UserId);
                modelBuilder.Entity<Users>().HasMany<ProjectPhasesTasks>(s => s.ProjectPhasesTasks).WithOne(g => g.Users).HasForeignKey(s => s.UserId);
                modelBuilder.Entity<Users>().HasMany<Project>(s => s.Project).WithOne(g => g.Users).HasForeignKey(s => s.MangerId);
                modelBuilder.Entity<Users>().HasMany<Project>(s => s.ProjectAdd).WithOne(g => g.AddUsers).HasForeignKey(s => s.AddUser);
                modelBuilder.Entity<Users>().HasMany<Project>(s => s.ProjectUpdate).WithOne(g => g.UpdateUsers).HasForeignKey(s => s.UpdateUser);

                modelBuilder.Entity<Users>().HasMany<WorkOrders>(s => s.WorkOrders).WithOne(g => g.User).HasForeignKey(s => s.ResponsibleEng);
                modelBuilder.Entity<Users>().HasMany<WorkOrders>(s => s.WorkOrdersRe).WithOne(g => g.ResponsibleEngineer).HasForeignKey(s => s.ResponsibleEng);
                modelBuilder.Entity<Users>().HasMany<WorkOrders>(s => s.WorkOrdersEx).WithOne(g => g.ExecutiveEngineer).HasForeignKey(s => s.ResponsibleEng);


            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Vacation>(entity =>
            {
                entity.HasKey(e => e.VacationId);
               // entity.ToTable("Emp_Vacations");
                entity.ToTable("Emp_Vacations", t => t.HasTrigger("VacationSendMail"));

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
                modelBuilder.Entity<Vacation>().HasOne(s => s.EmployeeName).WithMany(s => s.Vacations).HasForeignKey(e => e.EmployeeId);
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
            modelBuilder.Entity<Versions>(entity =>
            {
                entity.HasKey(e => e.VersionId);
                entity.ToTable("Sys_Version");
                entity.Property(t => t.VersionCode).HasColumnName("VersionCode");
                entity.Property(t => t.Date).HasColumnName("Date");
                entity.Property(t => t.HijriDate).HasColumnName("HijriDate");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<VoucherDetails>(entity =>
            {
                entity.HasKey(e => e.VoucherDetailsId);
                entity.ToTable("Acc_VoucherDetails");
                entity.Property(t => t.LineNumber).HasColumnName("LineNumber");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.Amount).HasColumnName("Amount");
                entity.Property(t => t.CostCenterId).HasColumnName("CostCenterId");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.PayType).HasColumnName("PayType");
                entity.Property(t => t.ReferenceNumber).HasColumnName("ReferenceNumber");
                entity.Property(t => t.TaxAmount).HasColumnName("TaxAmount");
                entity.Property(t => t.ToAccountId).HasColumnName("ToAccountId");
                entity.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
                entity.Property(t => t.TaxType).HasColumnName("TaxType");
                entity.Property(t => t.InvoiceId).HasColumnName("InvoiceId");
                entity.Property(t => t.Description).HasColumnName("Description");
                entity.Property(t => t.CheckNo).HasColumnName("CheckNo");
                entity.Property(t => t.CheckDate).HasColumnName("CheckDate");
                entity.Property(t => t.MoneyOrderNo).HasColumnName("MoneyOrderNo");
                entity.Property(t => t.MoneyOrderDate).HasColumnName("MoneyOrderDate");
                entity.Property(t => t.BankId).HasColumnName("BankId");
                entity.Property(t => t.TFK).HasColumnName("TFK");
                entity.Property(t => t.Qty).HasColumnName("Qty");
                entity.Property(t => t.ServicesPriceId).HasColumnName("ServicesPriceId");
                entity.Property(t => t.CategoryId).HasColumnName("CategoryId");
                entity.Property(t => t.IsRetrieve).HasColumnName("IsRetrieve");
                entity.Property(t => t.DiscountPercentage_Det).HasColumnName("DiscountPercentage_Det");
                entity.Property(t => t.DiscountValue_Det).HasColumnName("DiscountValue_Det");
                modelBuilder.Entity<VoucherDetails>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<VoucherDetails>().HasOne(s => s.CostCenters).WithMany().HasForeignKey(e => e.CostCenterId);
                //modelBuilder.Entity<VoucherDetails>().HasOne(s => s.Invoices).WithMany().HasForeignKey(e => e.InvoiceId);
                modelBuilder.Entity<VoucherDetails>().HasOne(s => s.ToAccounts).WithMany().HasForeignKey(e => e.ToAccountId);
                modelBuilder.Entity<VoucherDetails>().HasOne(s => s.Banks).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<VoucherDetails>().HasOne(s => s.ServicesPrice).WithMany().HasForeignKey(e => e.ServicesPriceId);
                modelBuilder.Entity<VoucherDetails>().HasOne(s => s.Categories).WithMany().HasForeignKey(e => e.CategoryId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<VoucherSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Acc_VoucherSettings");
                entity.Property(t => t.AccountId).HasColumnName("AccountId");
                entity.Property(t => t.Type).HasColumnName("Type");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<WorkOrders>(entity =>
            {
                entity.HasKey(e => e.WorkOrderId);
                entity.ToTable("Pro_WorkOrders");
                entity.Property(t => t.OrderNo).HasColumnName("OrderNo");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.OrderDate).HasColumnName("OrderDate");
                entity.Property(t => t.OrderHijriDate).HasColumnName("OrderHijriDate");
                entity.Property(t => t.ResponsibleEng).HasColumnName("ResponsibleEng");
                entity.Property(t => t.ExecutiveEng).HasColumnName("ExecutiveEng");
                entity.Property(t => t.CustomerId).HasColumnName("CustomerId");
                entity.Property(t => t.Mediator).HasColumnName("Mediator");
                entity.Property(t => t.Required).HasColumnName("Required");
                entity.Property(t => t.Note).HasColumnName("Note");
                entity.Property(t => t.OrderValue).HasColumnName("OrderValue");
                entity.Property(t => t.OrderPaid).HasColumnName("OrderPaid");
                entity.Property(t => t.OrderRemaining).HasColumnName("OrderRemaining");
                entity.Property(t => t.OrderDiscount).HasColumnName("OrderDiscount");
                entity.Property(t => t.OrderTax).HasColumnName("OrderTax");
                entity.Property(t => t.OrderValueAfterTax).HasColumnName("OrderValueAfterTax");
                entity.Property(t => t.DiscountReason).HasColumnName("DiscountReason");
                entity.Property(t => t.Sketch).HasColumnName("Sketch");
                entity.Property(t => t.District).HasColumnName("District");
                entity.Property(t => t.Location).HasColumnName("Location");
                entity.Property(t => t.PieceNo).HasColumnName("PieceNo");
                entity.Property(t => t.InstrumentNo).HasColumnName("InstrumentNo");
                entity.Property(t => t.ExecutiveType).HasColumnName("ExecutiveType");
                entity.Property(t => t.ContractNo).HasColumnName("ContractNo");
                entity.Property(t => t.AgentId).HasColumnName("AgentId");
                entity.Property(t => t.AgentMobile).HasColumnName("AgentMobile");
                entity.Property(t => t.Social).HasColumnName("Social");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.NoOfDays).HasColumnName("NoOfDays");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.AttatchmentUrl).HasColumnName("AttatchmentUrl");
                entity.Property(t => t.WOStatus).HasColumnName("WOStatus");
                entity.Property(t => t.IsConverted).HasColumnName("IsConverted");
                entity.Property(t => t.PlusTime).HasColumnName("PlusTime");
                entity.Property(t => t.PhasePriority).HasColumnName("PhasePriority");

                modelBuilder.Entity<WorkOrders>().HasOne(s => s.Project).WithMany(x => x.WorkOrders).HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.User).WithMany(x=>x.WorkOrders).HasForeignKey(e => e.UserId);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.ResponsibleEngineer).WithMany(x => x.WorkOrdersRe).HasForeignKey(e => e.ResponsibleEng);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.ExecutiveEngineer).WithMany(x => x.WorkOrdersEx).HasForeignKey(e => e.ExecutiveEng);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<WorkOrders>().HasMany<ContactList>(s => s.ContactLists).WithOne(g => g.WorkOrders).HasForeignKey(s => s.OrderId);
                modelBuilder.Entity<WorkOrders>().HasMany<Pro_TaskOperations>(s => s.TaskOperationsList).WithOne(g => g.WorkOrders).HasForeignKey(s => s.WorkOrderId);


            });
            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<WhatsAppSettings>(entity =>
            {
                entity.HasKey(e => e.SettingId);
                entity.ToTable("Sys_WhatsAppSettings");
                entity.Property(t => t.MobileNo).HasColumnName("MobileNo");
                entity.Property(t => t.Password).HasColumnName("Password");
                entity.Property(t => t.SenderName).HasColumnName("SenderName");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.ApiUrl).HasColumnName("ApiUrl");
                entity.Property(t => t.UserName).HasColumnName("UserName");
                entity.Property(t => t.Sendactivation).HasColumnName("InstanceId");
                entity.Property(t => t.Sendactivation).HasColumnName("Token");
                entity.Property(t => t.Sendactivation).HasColumnName("Sendactivation");
                entity.Property(t => t.SendactivationOffer).HasColumnName("SendactivationOffer");
                entity.Property(t => t.SendactivationProject).HasColumnName("SendactivationProject");
                entity.Property(t => t.SendactivationSupervision).HasColumnName("SendactivationSupervision");

            });
            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Guide_QuestionsAnswers>(entity =>
            {
                entity.HasKey(e => e.Guide_QuestionsAnswersId);
                entity.ToTable("Guide_QuestionsAnswers");
                entity.Property(t => t.QuestionAr).HasColumnName("QuestionAr");
                entity.Property(t => t.QuestionEn).HasColumnName("QuestionEn");
                entity.Property(t => t.AnswersAr).HasColumnName("AnswersAr");
                entity.Property(t => t.AnswersEn).HasColumnName("AnswersEn");
            });



            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<SupportRequestsReplay>(entity =>
            {
                entity.HasKey(e => e.SupportRequestsReplayId);
                entity.ToTable("SupportRequestsReplay");
                modelBuilder.Entity<SupportRequestsReplay>().HasOne(s => s.User).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<SupportRequestsReplay>().HasOne(s => s.SupportResquests).WithMany().HasForeignKey(e => e.ServiceRequestId);

            });



            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Emp_LateList>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.ToTable("Emp_LateList");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Emp_AbsenceList>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.ToTable("Emp_AbsenceList");
            });


            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ContactList>(entity =>
            {
                entity.HasKey(e => e.ContactListId);
                entity.ToTable("ContactList");
                modelBuilder.Entity<ContactList>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<AttendenceLocationSettings>(entity =>
            {
                entity.HasKey(e => e.AttendenceLocationSettingsId);
                entity.ToTable("Sys_AttendenceLocationSettings");
                //modelBuilder.Entity<AttendenceLocationSettings>().HasMany<Employees>(s => s.Employees).WithOne(g => g.AttendenceLocation).HasForeignKey(s => s.AttendenceLocationId);
                modelBuilder.Entity<AttendenceLocationSettings>().HasMany<EmpLocations>(s => s.EmpLocations).WithOne(g => g.AttendenceLocation).HasForeignKey(s => s.LocationId);

            });
            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Exceptions>(entity =>
            {
                entity.HasKey(e => e.ExceptionId);
                entity.ToTable("App_Exceptions");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.HasKey(e => e.PermissionId);
                entity.ToTable("Emp_Permissions");
                modelBuilder.Entity<Permissions>().HasOne(s => s.PermissionType).WithMany().HasForeignKey(e => e.TypeId);
                modelBuilder.Entity<Permissions>().HasOne(s => s.Employee).WithMany(s => s.Permissions).HasForeignKey(e => e.EmpId);
                modelBuilder.Entity<Permissions>().HasOne(s => s.UserAcccept).WithMany().HasForeignKey(e => e.AcceptedUser);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<PermissionType>(entity =>
            {
                entity.HasKey(e => e.TypeId);
                entity.ToTable("Emp_PermissionType");
                entity.Property(t => t.Code).HasColumnName("Code");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Notes).HasColumnName("Notes");
            });

            //--------------------------------END--------------------------------------------------

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<CommercialActivity>(entity =>
            {
                entity.HasKey(e => e.CommercialActivityId);
                entity.ToTable("Acc_CommercialActivity");
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<NotificationConfiguration>(entity =>
            {
                entity.HasKey(e => e.ConfigurationId);
                entity.ToTable("Sys_NotificationConfigurations");
                modelBuilder.Entity<NotificationConfiguration>().HasMany<NotificationConfigurationsAssines>(s => s.NotificationConfigurationsAssines).WithOne(g => g.NotificationConfiguration).HasForeignKey(s => s.ConfigurationId);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<NotificationConfigurationsAssines>(entity =>
            {
                entity.HasKey(e => e.ConfigurationAssinesId);
                entity.ToTable("Sys_NotificationConfigurationsAssines");

            });

            //--------------------------------END--------------------------------------------------
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }

}
