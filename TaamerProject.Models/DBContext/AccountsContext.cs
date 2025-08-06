using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DBContext
{
    public partial class AccountsContext :DbContext
    {
        public AccountsContext()
        {
        }

        public AccountsContext(DbContextOptions<AccountsContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Acc_Categories> Acc_Categories { get; set; }
        public virtual DbSet<Acc_CategorType> Acc_CategorType { get; set; }
        public virtual DbSet<Acc_Clauses> Acc_Clauses { get; set; }
        public virtual DbSet<Acc_EmpFinYears> Acc_EmpFinYears { get; set; }
        public virtual DbSet<Acc_Floors> Acc_Floors { get; set; }
        public virtual DbSet<Acc_Packages> Acc_Packages { get; set; }
        public virtual DbSet<Acc_Services_Price> Acc_Services_Price { get; set; }
        public virtual DbSet<Acc_Suppliers> Acc_Suppliers { get; set; }
        public virtual DbSet<Acc_TotalSpacesRange> Acc_TotalSpacesRange { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        //public virtual DbSet<AccTransactionTypes> AccTransactionTypes { get; set; }

        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<VoucherDetails> VoucherDetails { get; set; }
        public virtual DbSet<VoucherSettings> VoucherSettings { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<Checks> Checks { get; set; }
        public virtual DbSet<FiscalYears> FiscalYears { get; set; }
        public virtual DbSet<Guarantees> Guarantees { get; set; }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<Journals> Journals { get; set; }
        public virtual DbSet<OfferService> OfferService { get; set; }
        public virtual DbSet<OffersPrices> OffersPrices { get; set; }
        public virtual DbSet<OfficialDocuments> OfficialDocuments { get; set; }
                public virtual DbSet<OffersConditions> OffersConditions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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

                modelBuilder.Entity<Accounts>().HasOne(s => s.ParentAccount).WithMany().HasForeignKey(e => e.ParentId);
                modelBuilder.Entity<Accounts>().HasMany<Transactions>(s => s.Transactions).WithOne(g => g.Accounts).HasForeignKey(s => s.AccountId);
                modelBuilder.Entity<Accounts>().HasMany<Accounts>(s => s.ChildsAccount).WithOne(g => g.ParentAccount).HasForeignKey(s => s.ParentId);

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
                modelBuilder.Entity<Acc_Services_Price>().HasMany<OfferService>(s => s.OfferService).WithOne(g => g.serviceprice).HasForeignKey(s => s.ServiceId);
                modelBuilder.Entity<Acc_Services_Price>().HasOne(s => s.Package).WithMany().HasForeignKey(e => e.PackageId);
                modelBuilder.Entity<Acc_Services_Price>().HasMany<ContractServices>(s => s.ContractServices).WithOne(g => g.serviceprice).HasForeignKey(s => s.ServiceId);

            });

            //--------------------------------END--------------------------------------------------

            //modelBuilder.Entity<AccTransactionTypes>(entity =>
            //{
            //    entity.HasKey(e => e.TransactionTypeId);
            //    entity.ToTable("Acc_TransactionTypes");
            //    entity.Property(e => e.NameAr).HasColumnName("NameAr");
            //    entity.Property(e => e.NameEn).HasColumnName("NameEn");
             
            //});

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
                modelBuilder.Entity<ContractServices>().HasOne(s => s.Contracts).WithMany().HasForeignKey(e => e.ContractId);
                modelBuilder.Entity<ContractServices>().HasOne(s => s.serviceprice).WithMany().HasForeignKey(e => e.ServiceId);

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
                //entity.Property(t => t.InvoiceHash).HasColumnName("InvoiceHash");
                //entity.Property(t => t.SingedXML).HasColumnName("SingedXML");
                //entity.Property(t => t.EncodedInvoice).HasColumnName("EncodedInvoice");
                //entity.Property(t => t.ZatcaUUID).HasColumnName("ZatcaUUID");
                //entity.Property(t => t.QRCode).HasColumnName("QRCode");
                //entity.Property(t => t.PIH).HasColumnName("PIH");
                //entity.Property(t => t.SingedXMLFileName).HasColumnName("SingedXMLFileName");

                modelBuilder.Entity<Invoices>().HasMany<VoucherDetails>(s => s.VoucherDetails).WithOne(g => g.Invoices).HasForeignKey(s => s.InvoiceId);
                modelBuilder.Entity<Invoices>().HasMany<Transactions>(s => s.TransactionDetails).WithOne(g => g.Invoices).HasForeignKey(s => s.InvoiceId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.AddUsers).WithMany().HasForeignKey(e => e.AddUser);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Suppliers).WithMany().HasForeignKey(e => e.SupplierId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                //modelBuilder.Entity<Invoices>().HasOne(s => s.AccTransactionTypes).WithMany().HasForeignKey(e => e.Type);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Branch).WithMany().HasForeignKey(e => e.BranchId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.ToAccountId);
                modelBuilder.Entity<Invoices>().HasOne(s => s.Invoices_Credit).WithMany().HasForeignKey(e => e.CreditNotiId);
                //modelBuilder.Entity<Invoices>().HasOne(s => s.Invoices_Depit).WithMany().HasForeignKey(e => e.DepitNotiId);

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

                modelBuilder.Entity<OfferService>().HasOne(s => s.OffersPrices).WithMany().HasForeignKey(e => e.OfferId);
                modelBuilder.Entity<OfferService>().HasOne(s => s.serviceprice).WithMany().HasForeignKey(e => e.ServiceId);

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

                //modelBuilder.Entity<OffersPrices>().HasOne(s => s.Project).WithMany().HasForeignKey(e => e.ProjectId);
                modelBuilder.Entity<OffersPrices>().HasMany<CustomerPayments>(s => s.CustomerPayments).WithOne(g => g.OffersPrices).HasForeignKey(s => s.OfferId);
                modelBuilder.Entity<OffersPrices>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<OffersPrices>().HasMany<OffersConditions>(s => s.OffersConditions).WithOne(g => g.OffersPrices).HasForeignKey(s => s.OfferId);
                modelBuilder.Entity<OffersPrices>().HasMany<OfferService>(s => s.OfferService).WithOne(g => g.OffersPrices).HasForeignKey(s => s.OfferId);

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

                modelBuilder.Entity<Transactions>().HasOne(s => s.Invoices).WithMany().HasForeignKey(e => e.InvoiceId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.Accounts).WithMany().HasForeignKey(e => e.AccountId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.CostCenters).WithMany().HasForeignKey(e => e.CostCenterId);
                //modelBuilder.Entity<Transactions>().HasOne(s => s.AccTransactionTypes).WithMany().HasForeignKey(e => e.Type);
                modelBuilder.Entity<Transactions>().HasOne(s => s.Customer).WithMany().HasForeignKey(e => e.CustomerId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.Contracts).WithMany().HasForeignKey(e => e.ContractId);
                modelBuilder.Entity<Transactions>().HasOne(s => s.DiscountReward).WithMany().HasForeignKey(e => e.DiscountRewardId);
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


            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
