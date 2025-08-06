using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DBContext
{
    public partial class ProjectsContext : DbContext
    {
        public ProjectsContext()
        {
        }

        public ProjectsContext(DbContextOptions<ProjectsContext> options)
            : base(options)
        {
        }





        public virtual DbSet<Pro_Municipal> Pro_Municipal { get; set; }
        public virtual DbSet<Pro_SubMunicipality> Pro_SubMunicipality { get; set; }
        public virtual DbSet<Pro_Super_PhaseDet> Pro_Super_PhaseDet { get; set; }
        public virtual DbSet<Pro_Super_Phases> Pro_Super_Phases { get; set; }
        public virtual DbSet<Pro_SuperContractor> Pro_SuperContractor { get; set; }
        public virtual DbSet<Pro_SupervisionDetails> Pro_SupervisionDetails { get; set; }
        public virtual DbSet<ProUserPrivileges> ProUserPrivileges { get; set; }

        public virtual DbSet<FullProjectsReport> FullProjectsReport { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CityPass> CityPass { get; set; }

        //public virtual DbSet<ContactPerson> ContactPerson { get; set; }
        public virtual DbSet<ContractDetails> ContractDetails { get; set; }
        public virtual DbSet<Contracts> Contracts { get; set; }
        public virtual DbSet<ContractServices> ContractServices { get; set; }
        public virtual DbSet<ContractStage> ContractStage { get; set; }
        public virtual DbSet<CostCenters> CostCenters { get; set; }

        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerFiles> CustomerFiles { get; set; }
        public virtual DbSet<CustomerMail> CustomerMail { get; set; }
        public virtual DbSet<CustomerPayments> CustomerPayments { get; set; }
        public virtual DbSet<CustomerSMS> CustomerSMS { get; set; }
        public virtual DbSet<DatabaseBackup> DatabaseBackup { get; set; }
        //public virtual DbSet<Dawamyattend> Dawamyattend { get; set; }
        public virtual DbSet<DependencySettings> DependencySettings { get; set; }
        public virtual DbSet<Draft> Draft { get; set; }
        public virtual DbSet<DraftDetails> DraftDetails { get; set; }
        public virtual DbSet<Drafts_Templates> Drafts_Templates { get; set; }

        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectArchivesRe> ProjectArchivesRe { get; set; }
        public virtual DbSet<ProjectArchivesSee> ProjectArchivesSee { get; set; }
        public virtual DbSet<ProjectComments> ProjectComments { get; set; }
        public virtual DbSet<ProjectExtracts> ProjectExtracts { get; set; }
        public virtual DbSet<ProjectFiles> ProjectFiles { get; set; }
        public virtual DbSet<ProjectPhasesTasks> ProjectPhasesTasks { get; set; }
        public virtual DbSet<ProjectPieces> ProjectPieces { get; set; }
        public virtual DbSet<ProjectRequirements> ProjectRequirements { get; set; }
        public virtual DbSet<ProjectRequirementsGoals> ProjectRequirementsGoals { get; set; }
        public virtual DbSet<ProjectSubTypes> ProjectSubTypes { get; set; }
        public virtual DbSet<ProjectTrailing> ProjectTrailing { get; set; }
        public virtual DbSet<ProjectType> ProjectType { get; set; }
        public virtual DbSet<ProjectWorkers> ProjectWorkers { get; set; }
        public virtual DbSet<ProSettingDetails> ProSettingDetails { get; set; }
        public virtual DbSet<RegionTypes> RegionTypes { get; set; }
        public virtual DbSet<Requirements> Requirements { get; set; }
        public virtual DbSet<RequirementsandGoals> RequirementsandGoals { get; set; }
        public virtual DbSet<Service> Service { get; set; }

        public virtual DbSet<Supervisions> Supervisions { get; set; }
        public virtual DbSet<TasksDependency> TasksDependency { get; set; }
        public virtual DbSet<TaskType> TaskType { get; set; }
        public virtual DbSet<Templates> Templates { get; set; }
        public virtual DbSet<TemplatesTasks> TemplatesTasks { get; set; }
        public virtual DbSet<TimeOutRequests> TimeOutRequests { get; set; }
        public virtual DbSet<TrailingFiles> TrailingFiles { get; set; }
        public virtual DbSet<TransactionTypes> TransactionTypes { get; set; }

        public virtual DbSet<WorkOrders> WorkOrders { get; set; }
        public virtual DbSet<BuildTypes> BuildTypes { get; set; }

        public virtual DbSet<FileType> FileType { get; set; }

        public virtual DbSet<FollowProj> FollowProj { get; set; }


        public virtual DbSet<ImportantProject> ImportantProject { get; set; }
        public virtual DbSet<Instruments> Instruments { get; set; }
        public virtual DbSet<InstrumentSources> InstrumentSources { get; set; }
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<ModelRequirements> ModelRequirements { get; set; }
        public virtual DbSet<ModelType> ModelType { get; set; }

        public virtual DbSet<NodeLocations> NodeLocations { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
   

        public virtual DbSet<OutMovements> OutMovements { get; set; }
        public virtual DbSet<PrivFollowers> PrivFollowers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           modelBuilder.Entity<BuildTypes>(entity =>
            {
                entity.HasKey(e => e.BuildTypeId);
                entity.ToTable("Pro_BuildTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
                entity.Property(t => t.Description).HasColumnName("Description");
            });

            //--------------------------------END--------------------------------------------------

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
            modelBuilder.Entity<ContractDetails>(entity =>
            {
                entity.HasKey(e => e.ContractDetailId);
                entity.ToTable("Pro_ContractDetails");
                entity.Property(t => t.ContractId).HasColumnName("ContractId");
                entity.Property(t => t.SerialId).HasColumnName("SerialId");
                entity.Property(t => t.Clause).HasColumnName("Clause");
                modelBuilder.Entity<ContractDetails>().HasOne(s => s.Contracts).WithMany().HasForeignKey(e => e.ContractId);
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

                modelBuilder.Entity<Contracts>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasMany<CustomerPayments>(s => s.CustomerPayments).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Contracts>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Contracts>().HasOne(s => s.Service).WithMany().HasForeignKey(e => e.ServiceId);
                modelBuilder.Entity<Contracts>().HasMany<ContractDetails>(s => s.ContractDetails).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasMany<ContractStage>(s => s.ContractStage).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);
                modelBuilder.Entity<Contracts>().HasMany<ContractServices>(s => s.ContractServices).WithOne(g => g.Contracts).HasForeignKey(s => s.ContractId);


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
                modelBuilder.Entity<ContractStage>().HasOne(s => s.Contracts).WithMany().HasForeignKey(e => e.ContractId);
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

                modelBuilder.Entity<Customer>().HasOne(s => s.city).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Customer>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<Customer>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Customer>().HasMany<Project>(s => s.Projects).WithOne(g => g.customer).HasForeignKey(s => s.CustomerId);
                modelBuilder.Entity<Customer>().HasMany<Invoices>(s => s.Invoicess).WithOne(g => g.Customer).HasForeignKey(s => s.CustomerId);
                modelBuilder.Entity<Customer>().HasMany<Transactions>(s => s.Transactions).WithOne(g => g.Customer).HasForeignKey(s => s.AccountId);

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

                modelBuilder.Entity<CustomerPayments>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.ToAccountId);

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

            modelBuilder.Entity<DraftDetails>(entity =>
            {
                entity.HasKey(e => e.DraftDetailId);
                entity.ToTable("Pro_DraftDetails");
                entity.Property(t => t.DraftId).HasColumnName("DraftId");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");

                modelBuilder.Entity<DraftDetails>().HasOne(s => s.Draft).WithMany().HasForeignKey(e => e.DraftId);
                modelBuilder.Entity<DraftDetails>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
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

                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.FileType).WithMany().HasForeignKey(e => e.TypeId);
                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ProjectFiles>().HasOne(s => s.ProjectPhasesTasks).WithMany().HasForeignKey(e => e.TaskId);

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

            modelBuilder.Entity<ImportantProject>(entity =>
            {
                entity.HasKey(e => e.ImportantProId);
                entity.ToTable("Pro_ImportantProject");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.UserId).HasColumnName("UserId");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.Flag).HasColumnName("Flag");
                entity.Property(t => t.IsImportant).HasColumnName("IsImportant");
                modelBuilder.Entity<ImportantProject>().HasOne(s => s.project).WithMany().HasForeignKey(e => e.ProjectId);
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
                entity.ToTable("Pro_PhasesTasks");
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
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.AddTaskUser).WithMany().HasForeignKey(e => e.AddTaskUserId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.ProjectRequirementsGoals).WithMany().HasForeignKey(e => e.ProjectGoals);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjSubTypeId);

                //modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.MainPhase).WithMany().HasForeignKey(e => e.ProjSubTypeId);
                //modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.SubPhase).WithMany().HasForeignKey(e => e.ParentId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.NodeLocations).WithMany().HasForeignKey(e => e.LocationId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.Settings).WithMany().HasForeignKey(e => e.SettingId);
                modelBuilder.Entity<ProjectPhasesTasks>().HasOne(s => s.TaskTypeModel).WithMany().HasForeignKey(e => e.TaskType);

            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<ProUserPrivileges>(entity =>
            {
                entity.HasKey(e => e.UserPrivId);
                entity.ToTable("Pro_UserPrivileges");
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
                modelBuilder.Entity<ProUserPrivileges>().HasOne(s => s.Projects).WithMany().HasForeignKey(e => e.ProjectID);
            });

            //--------------------------------END--------------------------------------------------
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId);
                entity.ToTable("Pro_Projects");
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


                modelBuilder.Entity<Project>().HasOne(s => s.city).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Project>().HasOne(s => s.Contractor).WithMany().HasForeignKey(e => e.ContractorSelectId);
                modelBuilder.Entity<Project>().HasOne(s => s.projecttype).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.projectsubtype).WithMany().HasForeignKey(e => e.SubProjectTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.MangerId);
                //modelBuilder.Entity<Project>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Project>().HasOne(s => s.UpdateUsers).WithMany().HasForeignKey(e => e.UpdateUser);
                modelBuilder.Entity<Project>().HasOne(s => s.customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Project>().HasOne(s => s.transactionTypes).WithMany().HasForeignKey(e => e.TransactionTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.regionTypes).WithMany().HasForeignKey(e => e.RegionTypeId);
                modelBuilder.Entity<Project>().HasOne(s => s.projectPieces).WithMany().HasForeignKey(e => e.PieceNo);
                modelBuilder.Entity<Project>().HasOne(s => s.Contracts).WithMany().HasForeignKey(e => e.ContractId);
                modelBuilder.Entity<Project>().HasOne(s => s.OffersPrices).WithMany().HasForeignKey(e => e.OffersPricesId);
                modelBuilder.Entity<Project>().HasOne(s => s.Municipal).WithMany().HasForeignKey(e => e.MunicipalId);
                modelBuilder.Entity<Project>().HasOne(s => s.SubMunicipality).WithMany().HasForeignKey(e => e.SubMunicipalityId);
                //modelBuilder.Entity<Project>().HasOne(s => s.ProUserPrivileges).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProjectPhasesTasks>(s => s.ProjectPhasesTasks).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<WorkOrders>(s => s.WorkOrders).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<Invoices>(s => s.Invoices).WithOne(g => g.Project).HasForeignKey(s => s.InvoiceId);
                modelBuilder.Entity<Project>().HasMany<ProjectFiles>(s => s.ProjectFiles).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                //modelBuilder.Entity<Project>().HasOne(s => s.ActiveMainPhase).WithMany().HasForeignKey(e => e.ActiveMainPhaseId);
                //modelBuilder.Entity<Project>().HasOne(s => s.ActiveSubPhase).WithMany().HasForeignKey(e => e.ActiveSubPhaseId);
                modelBuilder.Entity<Project>().HasMany<ProjectWorkers>(s => s.ProjectWorkers).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProUserPrivileges>(s => s.ProUserPrivileges).WithOne(g => g.Projects).HasForeignKey(s => s.ProjectID);
                modelBuilder.Entity<Project>().HasMany<DraftDetails>(s => s.DraftDetails).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ProjectRequirementsGoals>(s => s.ProjectRequirementsGoals).WithOne(g => g.Project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasMany<ImportantProject>(s => s.ImportantProjects).WithOne(g => g.project).HasForeignKey(s => s.ProjectId);
                modelBuilder.Entity<Project>().HasOne(s => s.Costcenter).WithMany().HasForeignKey(e => e.CostCenterId);
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

                modelBuilder.Entity<ProjectRequirementsGoals>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
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
                modelBuilder.Entity<ProjectWorkers>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
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
                entity.Property(t => t.AddUser).HasColumnName("AddUser");

                modelBuilder.Entity<ProSettingDetails>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);
                modelBuilder.Entity<ProSettingDetails>().HasOne(s => s.ProjectSubTypes).WithMany().HasForeignKey(e => e.ProjectSubtypeId);
                modelBuilder.Entity<ProSettingDetails>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.AddUser);

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
                modelBuilder.Entity<RequirementsandGoals>().HasOne(s => s.ProjectType).WithMany().HasForeignKey(e => e.ProjectTypeId);
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

                modelBuilder.Entity<Supervisions>().HasOne(s => s.Municipal).WithMany().HasForeignKey(e => e.MunicipalSelectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.SubMunicipality).WithMany().HasForeignKey(e => e.SubMunicipalitySelectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.BuildTypes).WithMany().HasForeignKey(e => e.ProBuildingTypeSelectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.Users).WithMany().HasForeignKey(e => e.ReceivedEmpId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<Supervisions>().HasOne(s => s.Pro_Super_Phases).WithMany().HasForeignKey(e => e.PhaseId);
                modelBuilder.Entity<Supervisions>().HasMany<Pro_SupervisionDetails>(s => s.SupervisionDetails).WithOne(g => g.Supervisions).HasForeignKey(s => s.SupervisionId);
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

            modelBuilder.Entity<TransactionTypes>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId);
                entity.ToTable("Pro_TransactionTypes");
                entity.Property(t => t.NameAr).HasColumnName("NameAr");
                entity.Property(t => t.NameEn).HasColumnName("NameEn");
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
                entity.Property(t => t.PercentComplete).HasColumnName("PercentComplete");
                entity.Property(t => t.Social).HasColumnName("Social");
                entity.Property(t => t.BranchId).HasColumnName("BranchId");
                entity.Property(t => t.NoOfDays).HasColumnName("NoOfDays");
                entity.Property(t => t.ProjectId).HasColumnName("ProjectId");
                entity.Property(t => t.AttatchmentUrl).HasColumnName("AttatchmentUrl");
                entity.Property(t => t.WOStatus).HasColumnName("WOStatus");
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.User).WithMany().HasForeignKey(e => e.UserId);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.ResponsibleEngineer).WithMany().HasForeignKey(e => e.ResponsibleEng);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.ExecutiveEngineer).WithMany().HasForeignKey(e => e.ExecutiveEng);
                modelBuilder.Entity<WorkOrders>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);

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
            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
