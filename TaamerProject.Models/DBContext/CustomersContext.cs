using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DBContext
{
    public partial class CustomersContext : DbContext
    {
        public CustomersContext()
        {
        }

        public CustomersContext(DbContextOptions<CustomersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OutInBox> OutInBox { get; set; }
        public virtual DbSet<OutInBoxSerial> OutInBoxSerial { get; set; }
        public virtual DbSet<OutInBoxType> OutInBoxType { get; set; }
        public virtual DbSet<OutInImagesTo> OutInImagesTo { get; set; }
        public virtual DbSet<ServicesPricingForm> ServicesPricingForm { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DBConnection");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<OutInBox>(entity =>
            {
                entity.HasKey(e => e.OutInBoxId);
                entity.ToTable("Contac_OutInBox");
                entity.Property(t => t.TypeId).HasColumnName("TypeId");
                entity.Property(t => t.SideFromId).HasColumnName("SideFromId");
                entity.Property(t => t.SideToId).HasColumnName("SideToId");
                entity.Property(t => t.InnerId).HasColumnName("InnerId");
                entity.Property(t => t.Topic).HasColumnName("Topic");
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

            });

            //--------------------------------END--------------------------------------------------

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
