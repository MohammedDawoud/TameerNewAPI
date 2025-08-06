using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DBContext
{
    public partial class OrganizationandSettingContext : DbContext
    {
        public OrganizationandSettingContext()
        {
        }

        public OrganizationandSettingContext(DbContextOptions<OrganizationandSettingContext> options)
            : base(options)
        {
        }
        public virtual DbSet<SMSSettings> SMSSettings { get; set; }
        public virtual DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
        public virtual DbSet<SupportResquests> SupportResquests { get; set; }
        public virtual DbSet<Sys_SystemActions> Sys_SystemActions { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
        public virtual DbSet<Versions> Versions { get; set; }
        public virtual DbSet<EmailSetting> EmailSetting { get; set; }
        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<BackupAlert> BackupAlert { get; set; }
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<ChattingLog> ChattingLog { get; set; }
        public virtual DbSet<ContacFiles> ContacFiles { get; set; }
        public virtual DbSet<Contact_Branches> Contact_Branches { get; set; }
        public virtual DbSet<DatabaseBackup> DatabaseBackup { get; set; }
        public virtual DbSet<Licences> Licences { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NotificationSettings> NotificationSettings { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


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
                entity.Property(t => t.ContractEndNote).HasColumnName("ContractEndNote");
                entity.Property(t => t.ResedentEndNote).HasColumnName("ResedentEndNote");

                modelBuilder.Entity<SystemSettings>().HasOne(s => s.UpdateUserT).WithMany().HasForeignKey(e => e.UpdateUser);

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

                modelBuilder.Entity<Organizations>().HasOne(s => s.City).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Organizations>().HasOne(s => s.UpdateUserT).WithMany().HasForeignKey(e => e.UpdateUser);
                modelBuilder.Entity<Organizations>().HasOne(s => s.RepEmployee).WithMany().HasForeignKey(e => e.RepresentorEmpId);
                modelBuilder.Entity<Organizations>().HasOne(s => s.Bank).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Organizations>().HasOne(s => s.Bank2).WithMany().HasForeignKey(e => e.BankId2);

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
                modelBuilder.Entity<Branch>().HasOne(s => s.City).WithMany().HasForeignKey(e => e.CityId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Currency).WithMany().HasForeignKey(e => e.CurrencyId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Organizations).WithMany().HasForeignKey(e => e.OrganizationId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Bank).WithMany().HasForeignKey(e => e.BankId);
                modelBuilder.Entity<Branch>().HasOne(s => s.Bank2).WithMany().HasForeignKey(e => e.BankId2);

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

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
