using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using System.Globalization;

namespace TaamerProject.Repository.Repositories
{
    public class SupervisionsRepository : ISupervisionsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SupervisionsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task< IEnumerable<SupervisionsVM>> GetAllSupervisions(int? ProjectId)
        {
            var supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && (s.ProjectId == ProjectId || ProjectId == null)).Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.Users != null ? x.Users.FullNameAr : "",
                CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                ProjectManager = x.Project != null ? x.Project!.Users!.FullNameAr : "",
                PhaseName = x.Pro_Super_Phases!.NameAr,
                Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                PieceNo = x.PieceNo ?? "",
                LicenseNo = x.LicenseNo ?? "",
                OutlineNo = x.OutlineNo ?? "",
                PhaseId = x.PhaseId,
                OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                OfficePhone = x.Project != null ? x.Project!.Co_opOfficePhone : "",
                ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                ContractorPhone = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.PhoneNo : "" : "",
                SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                AddDate = x.AddDate,
                CustomerEmail = x.Project != null ? x.Project!.customer!.CustomerEmail ?? "" : "",
                CustomerPhone = x.Project != null ? x.Project!.customer!.CustomerMobile ?? "" : "",
                MunicipalSelectId=x.MunicipalSelectId??0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl=x.ImageUrl??"",
                ImageUrl2=x.ImageUrl2??"",
                OutlineChangetxt1=x.OutlineChangetxt1??"",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.Pro_Super_Phases!.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",
            
            }).ToList();
            return supervisions;
        }
        public async Task<IEnumerable<SupervisionsVM>> GetAllBySupervisionId(int SupervisionId)
        {
            var supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && s.SupervisionId == SupervisionId).Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.Users != null ? x.Users.FullName : "",
                CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                PhaseName = x.Pro_Super_Phases!.NameAr,
                Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                PieceNo = x.PieceNo ?? "",
                LicenseNo = x.LicenseNo ?? "",
                OutlineNo = x.OutlineNo ?? "",
                PhaseId = x.PhaseId,
                OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                OfficePhone = x.Project != null ? x.Project!.Co_opOfficePhone : "",
                ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                ContractorPhone = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.PhoneNo : "" : "",
                SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                AddDate=x.AddDate,
                CustomerEmail = x.Project != null ? x.Project!.customer!.CustomerEmail??"" : "",
                CustomerPhone = x.Project != null ? x.Project!.customer!.CustomerMobile ?? "" : "",
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",

                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",

                MunicipalName = x.Municipal != null ? x.Municipal.NameAr : "",
                SubMunicipalityName = x.SubMunicipality != null ? x.SubMunicipality.NameAr : "",
                BuildTypeName = x.BuildTypes != null ? x.BuildTypes.NameAr : "",
                ProjectDiscName = x.Project != null ? x.Project!.ProjectDescription : "",
                ContractorName = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.NameAr : "" : "",
                SuperCode = x.Pro_Super_Phases!.SuperCode,
                TimeStr = (x.Project!.NoOfDays < 30) ? x.Project!.NoOfDays + " يوم " : (x.Project!.NoOfDays == 30) ? x.Project!.NoOfDays / 30 + " شهر " : (x.Project!.NoOfDays > 30) ? ((x.Project!.NoOfDays / 30) + " شهر " + (x.Project!.NoOfDays % 30) + " يوم  ") : "",
                Catego= x.Project!.Catego??"0",
                CityName = x.Project!.city!.NameAr ?? "",
                ProPieceNumber = x.Project!.PieceNo == null ? "0" : x.Project!.projectPieces!.PieceNo ?? "0",
                Cons_components = x.Project!.Cons_components ?? "",
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",

            }).ToList();
            return supervisions;
        }
        public async Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserId(int? UserId)
        {
            var supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && s.ReceivedEmpId == UserId && (s.SuperStatus == 1 || s.SuperStatus == 2 || s.SuperStatus == 3 ) ).Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.Users != null ? x.Users.FullName : "",
                CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                PhaseName = x.Pro_Super_Phases!.NameAr,
                Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                PieceNo = x.PieceNo ?? "",
                LicenseNo = x.LicenseNo ?? "",
                OutlineNo = x.OutlineNo ?? "",
                PhaseId = x.PhaseId,
                OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                OfficePhone = x.Project != null ? x.Project!.Co_opOfficePhone : "",
                ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                ContractorPhone = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.PhoneNo : "" : "",
                SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                AddDate = x.AddDate,
                CustomerEmail = x.Project != null ? x.Project!.customer!.CustomerEmail ?? "" : "",
                CustomerPhone = x.Project != null ? x.Project!.customer!.CustomerMobile ?? "" : "",
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",
                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.Pro_Super_Phases!.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",

            }).ToList();
            return supervisions;
        }


        //edit userid to receivedempid
        public async Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserIdHome(int? UserId)
        {
            
            var supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && s.ReceivedEmpId == UserId && s.IsRead == false).Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.Users != null ? x.Users.FullName : "",
                CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                PhaseName = x.Pro_Super_Phases!.NameAr,
                Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                PieceNo = x.PieceNo ?? "",
                LicenseNo = x.LicenseNo ?? "",
                OutlineNo = x.OutlineNo ?? "",
                PhaseId = x.PhaseId,
                OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                OfficePhone = x.Project != null ? x.Project!.Co_opOfficePhone : "",
                ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                ContractorPhone = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.PhoneNo : "" : "",
                SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                AddDate = x.AddDate,
                CustomerEmail = x.Project != null ? x.Project!.customer!.CustomerEmail ?? "" : "",
                CustomerPhone = x.Project != null ? x.Project!.customer!.CustomerMobile ?? "" : "",
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",
                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.Pro_Super_Phases!.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",

            }).ToList();
            return supervisions;
        }

        public async Task<IEnumerable<SupervisionsVM>> GetAllSupervisions_Search(int? ProjectId, int? UserId, int? EmpId,int? PhaseId, string DateFrom, string Dateto)
        {
            var supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false &&  (s.ProjectId == ProjectId || ProjectId == 0 || ProjectId == null) && (s.Project.CustomerId == UserId || UserId == 0 || UserId == null) 
            && (s.ReceivedEmpId == EmpId || EmpId == 0 || EmpId == null) && (s.PhaseId == PhaseId || PhaseId == 0 || PhaseId == null)).Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.Users != null ? x.Users.FullNameAr : "",
                CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                ProjectManager = x.Project != null ? x.Project!.Users!.FullNameAr : "",

                PhaseName = x.Pro_Super_Phases!.NameAr,
                Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                PieceNo = x.PieceNo ?? "",
                LicenseNo = x.LicenseNo ?? "",
                OutlineNo = x.OutlineNo ?? "",
                PhaseId = x.PhaseId,
                OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",
                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.Pro_Super_Phases!.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",


            }).ToList().Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.ReceivedUserName,
                CustomerName = x.CustomerName ,
                CustomerName_W=x.CustomerName_W,
                ProjectNo = x.ProjectNo,
                ProjectManager=x.ProjectManager,
                PhaseName = x.PhaseName,
                Status = x.Status,
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.CustomerId,
                PieceNo = x.PieceNo ,
                LicenseNo = x.LicenseNo ,
                OutlineNo = x.OutlineNo ,
                PhaseId = x.PhaseId,
                OfficeName = x.OfficeName,
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.OfficeEmail,
                ContractorEmail = x.ContractorEmail,
                SuperStatusName = x.SuperStatusName,
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",
                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName=x.RequiredServiceName??"بدون",



            }).ToList();
            //.Where(s => s.Date >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && s.Date <= DateTime.ParseExact(Dateto, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            if (DateFrom != "" && Dateto != "")
            {
                var supervisions2 = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && (s.ProjectId == ProjectId || ProjectId == null) && (s.Project.CustomerId == UserId || UserId == null) && (s.ReceivedEmpId == EmpId || EmpId == null)).Select(x => new SupervisionsVM
                {
                    SupervisionId = x.SupervisionId,
                    Number = x.Number,
                    Phase = x.Phase,
                    ProjectId = x.ProjectId,
                    Location = x.Location,
                    Date = x.Date ?? "",
                    ReceiveNotes = x.ReceiveNotes,
                    ManagerNotes = x.ManagerNotes,
                    ReceivedEmpId = x.ReceivedEmpId,
                    ReceiveStatus = x.ReceiveStatus,
                    ReceiveDate = x.ReceiveDate,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    ReceivedUserName = x.Users != null ? x.Users.FullName : "",
                    CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                    CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                    PhaseName = x.Pro_Super_Phases!.NameAr,
                    Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                    VisitDate = x.VisitDate ?? "",
                    CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                    PieceNo = x.PieceNo ?? "",
                    LicenseNo = x.LicenseNo ?? "",
                    OutlineNo = x.OutlineNo ?? "",
                    PhaseId = x.PhaseId,
                    OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                    WorkerId = x.WorkerId,
                    SuperDateConfirm = x.SuperDateConfirm ?? "",
                    SuperStatus = x.SuperStatus ?? 0,
                    OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                    ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                    SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                    MunicipalSelectId = x.MunicipalSelectId ?? 0,
                    SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                    ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                    DistrictName = x.DistrictName ?? "",
                    ProBuildingDisc = x.ProBuildingDisc ?? "",
                    AdwARid = x.AdwARid ?? 0,
                    ImageUrl = x.ImageUrl ?? "",
                    ImageUrl2 = x.ImageUrl2 ?? "",
                    OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                    OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                    OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                    PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                    PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                    PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                    SuperCode = x.Pro_Super_Phases!.SuperCode,
                    RequiredServiceId = x.RequiredServiceId,
                    RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",

                }).ToList().Select(x => new SupervisionsVM
                {
                    SupervisionId = x.SupervisionId,
                    Number = x.Number,
                    Phase = x.Phase,
                    ProjectId = x.ProjectId,
                    Location = x.Location,
                    Date = x.Date ?? "",
                    ReceiveNotes = x.ReceiveNotes,
                    ManagerNotes = x.ManagerNotes,
                    ReceivedEmpId = x.ReceivedEmpId,
                    ReceiveStatus = x.ReceiveStatus,
                    ReceiveDate = x.ReceiveDate,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    ReceivedUserName = x.ReceivedUserName,
                    CustomerName = x.CustomerName,
                    CustomerName_W=x.CustomerName_W,
                    ProjectNo = x.ProjectNo,
                    PhaseName = x.PhaseName,
                    Status = x.Status,
                    VisitDate = x.VisitDate ?? "",
                    CustomerId = x.CustomerId,
                    PieceNo = x.PieceNo,
                    LicenseNo = x.LicenseNo,
                    OutlineNo = x.OutlineNo,
                    PhaseId = x.PhaseId,
                    OfficeName = x.OfficeName,
                    WorkerId = x.WorkerId,
                    SuperDateConfirm = x.SuperDateConfirm ?? "",
                    SuperStatus = x.SuperStatus ?? 0,
                    OfficeEmail = x.OfficeEmail,
                    ContractorEmail = x.ContractorEmail,
                    SuperStatusName = x.SuperStatusName,
                    MunicipalSelectId = x.MunicipalSelectId ?? 0,
                    SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                    ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                    DistrictName = x.DistrictName ?? "",
                    ProBuildingDisc = x.ProBuildingDisc ?? "",
                    AdwARid = x.AdwARid ?? 0,
                    ImageUrl = x.ImageUrl ?? "",
                    ImageUrl2 = x.ImageUrl2 ?? "",
                    OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                    OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                    OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                    PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                    PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                    PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                    SuperCode = x.SuperCode,
                    RequiredServiceId = x.RequiredServiceId,
                    RequiredServiceName = x.RequiredServiceName ?? "بدون",

                }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Dateto, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return supervisions2;
            }
            return supervisions;
        }




        public async Task<IEnumerable<SupervisionsVM>> GetAllSupervisions_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto,string? Searchtext)
        {
            var supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && (s.ProjectId == ProjectId || ProjectId == 0 || ProjectId == null) && (s.Project.CustomerId == UserId || UserId == 0 || UserId == null)
            && (s.ReceivedEmpId == EmpId || EmpId == 0 || EmpId == null) && (s.PhaseId == PhaseId || PhaseId == 0 || PhaseId == null) && (s.Project.customer.CustomerNameAr.Contains(Searchtext)||
            s.Project.ProjectNo.Contains(Searchtext) || s.Number.ToString()==Searchtext || s.Users.FullNameAr.Contains(Searchtext) ||
            s.Pro_Super_Phases.NameAr.Contains(Searchtext) || s.Project.Users.FullNameAr.Contains(Searchtext) ||
            Searchtext ==null || Searchtext =="")).Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.Users != null ? x.Users.FullNameAr : "",
                CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                ProjectManager = x.Project != null ? x.Project!.Users!.FullNameAr : "",

                PhaseName = x.Pro_Super_Phases!.NameAr,
                Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                PieceNo = x.PieceNo ?? "",
                LicenseNo = x.LicenseNo ?? "",
                OutlineNo = x.OutlineNo ?? "",
                PhaseId = x.PhaseId,
                OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",
                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.Pro_Super_Phases!.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",


            }).ToList().Select(x => new SupervisionsVM
            {
                SupervisionId = x.SupervisionId,
                Number = x.Number,
                Phase = x.Phase,
                ProjectId = x.ProjectId,
                Location = x.Location,
                Date = x.Date ?? "",
                ReceiveNotes = x.ReceiveNotes,
                ManagerNotes = x.ManagerNotes,
                ReceivedEmpId = x.ReceivedEmpId,
                ReceiveStatus = x.ReceiveStatus,
                ReceiveDate = x.ReceiveDate,
                UserId = x.UserId,
                BranchId = x.BranchId,
                ReceivedUserName = x.ReceivedUserName,
                CustomerName = x.CustomerName,
                CustomerName_W = x.CustomerName_W,
                ProjectNo = x.ProjectNo,
                ProjectManager = x.ProjectManager,
                PhaseName = x.PhaseName,
                Status = x.Status,
                VisitDate = x.VisitDate ?? "",
                CustomerId = x.CustomerId,
                PieceNo = x.PieceNo,
                LicenseNo = x.LicenseNo,
                OutlineNo = x.OutlineNo,
                PhaseId = x.PhaseId,
                OfficeName = x.OfficeName,
                WorkerId = x.WorkerId,
                SuperDateConfirm = x.SuperDateConfirm ?? "",
                SuperStatus = x.SuperStatus ?? 0,
                OfficeEmail = x.OfficeEmail,
                ContractorEmail = x.ContractorEmail,
                SuperStatusName = x.SuperStatusName,
                MunicipalSelectId = x.MunicipalSelectId ?? 0,
                SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                DistrictName = x.DistrictName ?? "",
                ProBuildingDisc = x.ProBuildingDisc ?? "",
                AdwARid = x.AdwARid ?? 0,
                ImageUrl = x.ImageUrl ?? "",
                ImageUrl2 = x.ImageUrl2 ?? "",
                OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                SuperCode = x.SuperCode,
                RequiredServiceId = x.RequiredServiceId,
                RequiredServiceName = x.RequiredServiceName ?? "بدون",



            }).ToList();
            //.Where(s => s.Date >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && s.Date <= DateTime.ParseExact(Dateto, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            if (DateFrom != "" && Dateto != "")
            {
                var supervisions2 = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && (s.ProjectId == ProjectId || ProjectId == null) && (s.Project.CustomerId == UserId || UserId == null) && (s.ReceivedEmpId == EmpId || EmpId == null)).Select(x => new SupervisionsVM
                {
                    SupervisionId = x.SupervisionId,
                    Number = x.Number,
                    Phase = x.Phase,
                    ProjectId = x.ProjectId,
                    Location = x.Location,
                    Date = x.Date ?? "",
                    ReceiveNotes = x.ReceiveNotes,
                    ManagerNotes = x.ManagerNotes,
                    ReceivedEmpId = x.ReceivedEmpId,
                    ReceiveStatus = x.ReceiveStatus,
                    ReceiveDate = x.ReceiveDate,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    ReceivedUserName = x.Users != null ? x.Users.FullName : "",
                    CustomerName_W = x.Project != null ? x.Project!.customer!.CustomerNameAr : "",
                    CustomerName = x.Project != null ? x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.Project!.customer!.CustomerNameAr : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.Project!.customer!.CustomerNameAr + "(*)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.Project!.customer!.CustomerNameAr + "(**)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.Project!.customer!.CustomerNameAr + "(***)" : x.Project!.customer!.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Project!.customer!.CustomerNameAr + "(VIP)" : x.Project!.customer!.CustomerNameAr : "",

                    ProjectNo = x.Project != null ? x.Project!.ProjectNo : "",
                    PhaseName = x.Pro_Super_Phases!.NameAr,
                    Status = x.ReceiveStatus == true ? "تم الاستلام" : "لم يتم الاستلام",
                    VisitDate = x.VisitDate ?? "",
                    CustomerId = x.Project != null ? x.Project!.customer!.CustomerId : 0,
                    PieceNo = x.PieceNo ?? "",
                    LicenseNo = x.LicenseNo ?? "",
                    OutlineNo = x.OutlineNo ?? "",
                    PhaseId = x.PhaseId,
                    OfficeName = x.Project != null ? x.Project!.Co_opOfficeName : "",
                    WorkerId = x.WorkerId,
                    SuperDateConfirm = x.SuperDateConfirm ?? "",
                    SuperStatus = x.SuperStatus ?? 0,
                    OfficeEmail = x.Project != null ? x.Project!.Co_opOfficeEmail : "",
                    ContractorEmail = x.Project != null ? x.Project!.Contractor != null ? x.Project!.Contractor.Email : "" : "",
                    SuperStatusName = x.SuperStatus == 1 ? "قيد التشغيل" : x.SuperStatus == 2 ? "تم الاستلام" : x.SuperStatus == 3 ? "لم يتم الاستلام" : "لم يتم الاتاحة بعد",
                    MunicipalSelectId = x.MunicipalSelectId ?? 0,
                    SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                    ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                    DistrictName = x.DistrictName ?? "",
                    ProBuildingDisc = x.ProBuildingDisc ?? "",
                    AdwARid = x.AdwARid ?? 0,
                    ImageUrl = x.ImageUrl ?? "",
                    ImageUrl2 = x.ImageUrl2 ?? "",
                    OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                    OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                    OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                    PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                    PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                    PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                    SuperCode = x.Pro_Super_Phases!.SuperCode,
                    RequiredServiceId = x.RequiredServiceId,
                    RequiredServiceName = x.RequiredServiceId == 1 ? "زيارات" : x.RequiredServiceId == 2 ? "نصف مقيم" : x.RequiredServiceId == 3 ? "مقيم" : "بدون",

                }).ToList().Select(x => new SupervisionsVM
                {
                    SupervisionId = x.SupervisionId,
                    Number = x.Number,
                    Phase = x.Phase,
                    ProjectId = x.ProjectId,
                    Location = x.Location,
                    Date = x.Date ?? "",
                    ReceiveNotes = x.ReceiveNotes,
                    ManagerNotes = x.ManagerNotes,
                    ReceivedEmpId = x.ReceivedEmpId,
                    ReceiveStatus = x.ReceiveStatus,
                    ReceiveDate = x.ReceiveDate,
                    UserId = x.UserId,
                    BranchId = x.BranchId,
                    ReceivedUserName = x.ReceivedUserName,
                    CustomerName = x.CustomerName,
                    CustomerName_W = x.CustomerName_W,
                    ProjectNo = x.ProjectNo,
                    PhaseName = x.PhaseName,
                    Status = x.Status,
                    VisitDate = x.VisitDate ?? "",
                    CustomerId = x.CustomerId,
                    PieceNo = x.PieceNo,
                    LicenseNo = x.LicenseNo,
                    OutlineNo = x.OutlineNo,
                    PhaseId = x.PhaseId,
                    OfficeName = x.OfficeName,
                    WorkerId = x.WorkerId,
                    SuperDateConfirm = x.SuperDateConfirm ?? "",
                    SuperStatus = x.SuperStatus ?? 0,
                    OfficeEmail = x.OfficeEmail,
                    ContractorEmail = x.ContractorEmail,
                    SuperStatusName = x.SuperStatusName,
                    MunicipalSelectId = x.MunicipalSelectId ?? 0,
                    SubMunicipalitySelectId = x.SubMunicipalitySelectId ?? 0,
                    ProBuildingTypeSelectId = x.ProBuildingTypeSelectId ?? 0,
                    DistrictName = x.DistrictName ?? "",
                    ProBuildingDisc = x.ProBuildingDisc ?? "",
                    AdwARid = x.AdwARid ?? 0,
                    ImageUrl = x.ImageUrl ?? "",
                    ImageUrl2 = x.ImageUrl2 ?? "",
                    OutlineChangetxt1 = x.OutlineChangetxt1 ?? "",
                    OutlineChangetxt2 = x.OutlineChangetxt2 ?? "",
                    OutlineChangetxt3 = x.OutlineChangetxt3 ?? "",
                    PointsNotWrittentxt1 = x.PointsNotWrittentxt1 ?? "",
                    PointsNotWrittentxt2 = x.PointsNotWrittentxt2 ?? "",
                    PointsNotWrittentxt3 = x.PointsNotWrittentxt3 ?? "",
                    SuperCode = x.SuperCode,
                    RequiredServiceId = x.RequiredServiceId,
                    RequiredServiceName = x.RequiredServiceName ?? "بدون",

                }).ToList().Where(s => DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(Dateto, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                return supervisions2;
            }
            return supervisions;
        }


        public async Task<int> GenerateNextSupNumber()
        {
            if (_TaamerProContext.Supervisions != null)
            {
                //var lastRow = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false).OrderByDescending(u => u.Code).Take(1).FirstOrDefault();
                var lastRow = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false).OrderByDescending(u => u.SupervisionId).Take(1).FirstOrDefault();

                if (lastRow != null)
                {
                    return ((lastRow.Number??0) + 1);
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

    }
}


