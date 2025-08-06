using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DBContext
{
    public partial class UsersContext : DbContext
    {

        public UsersContext()
        {
        }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersLocations> UsersLocations { get; set; }
        public virtual DbSet<UserMails> UserMails { get; set; }
        public virtual DbSet<UserBranches> UserBranches { get; set; }
        //public virtual DbSet<UserIDInfo> UserIDInfo { get; set; }
        public virtual DbSet<UserNotificationPrivileges> UserNotificationPrivileges { get; set; }
        public virtual DbSet<UserPrivileges> UserPrivileges { get; set; }

        public virtual DbSet<GroupPrivileges> GroupPrivileges { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<GuideDepartmentDetails> GuideDepartmentDetails { get; set; }
        public virtual DbSet<GuideDepartments> GuideDepartments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");
            }

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


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
                //modelBuilder.Entity<Users>().HasOne(s => s.Department).WithMany().HasForeignKey(e => e.DepartmentId);
                //modelBuilder.Entity<Users>().HasOne(s => s.Groups).WithMany().HasForeignKey(e => e.GroupId);
                //modelBuilder.Entity<Users>().HasOne(s => s.Jobs).WithMany().HasForeignKey(e => e.JobId);
                //modelBuilder.Entity<Users>().HasOne(s => s.Branches).WithMany().HasForeignKey(e => e.BranchId);

                modelBuilder.Entity<Users>().HasMany<UserPrivileges>(s => s.UserPrivileges).WithOne(g => g.Users).HasForeignKey(s => s.UserId);
                modelBuilder.Entity<Users>().HasMany<ProjectPhasesTasks>(s => s.ProjectPhasesTasks).WithOne(g => g.Users).HasForeignKey(s => s.UserId);
                modelBuilder.Entity<Users>().HasMany<Project>(s => s.Project).WithOne(g => g.Users).HasForeignKey(s => s.MangerId);
                modelBuilder.Entity<Users>().HasMany<WorkOrders>(s => s.WorkOrders).WithOne(g => g.ResponsibleEngineer).HasForeignKey(s => s.ResponsibleEng);

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
            });

            //--------------------------------END--------------------------------------------------

            OnModelCreatingPartial(modelBuilder);

        }

          partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
