using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class Pro_SupervisionDetailsRepository : IPro_SupervisionDetailsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Pro_SupervisionDetailsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetails(string SearchText)
        {
            if (SearchText == "")
            {
                var SupervisionDetails = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.IsDeleted == false).Select(x => new Pro_SupervisionDetailsVM
                {
                    SuperDetId = x.SuperDetId,
                    SupervisionId = x.SupervisionId,
                    NameAr = x.NameAr??"",
                    NameEn = x.NameEn??"",
                    Note = x.Note == null ? "" : x.Note == "null" ? "" : x.Note == "NULL" ? "" : x.Note ?? "",
                    IsRead = x.IsRead??0,
                    BranchId = x.BranchId,
                    ImageUrl=x.ImageUrl??"",
                    TheNumber=x.TheNumber??"",
                    TheLocation=x.TheLocation??"",
                }).ToList();
                return SupervisionDetails;
            }
            else

            {
                var SupervisionDetails = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.IsDeleted == false && (s.NameAr.Contains(SearchText) || s.NameEn.Contains(SearchText))).Select(x => new Pro_SupervisionDetailsVM
                {
                    SuperDetId = x.SuperDetId,
                    SupervisionId = x.SupervisionId,
                    NameAr = x.NameAr ?? "",
                    NameEn = x.NameEn ?? "",
                    Note = x.Note == null ? "" : x.Note == "null" ? "" : x.Note == "NULL" ? "" : x.Note ?? "",
                    IsRead = x.IsRead ?? 0,
                    BranchId = x.BranchId,
                    ImageUrl = x.ImageUrl ?? "",
                    TheNumber = x.TheNumber ?? "",
                    TheLocation = x.TheLocation ?? "",
                }).ToList();
                return SupervisionDetails;
            }
        }
        public async Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetailsBySuperId(int? SupervisionId)
        {
            var SupervisionDetails = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.IsDeleted == false && s.SupervisionId == SupervisionId).Select(x => new Pro_SupervisionDetailsVM
            {
                SuperDetId = x.SuperDetId,
                SupervisionId = x.SupervisionId,
                NameAr = x.NameAr ?? "",
                NameEn = x.NameEn ?? "",
                Note = x.Note == null ? "" : x.Note == "null" ? "" : x.Note == "NULL" ? "" : x.Note ?? "",
                IsRead = x.IsRead ?? 0,
                BranchId = x.BranchId,
                PhaseId = x.Supervisions.PhaseId,
                PhaseName = x.Supervisions.Pro_Super_Phases != null ? x.Supervisions.Pro_Super_Phases.NameAr ?? "" : "",
                SupervisionNumber = x.Supervisions.Number,
                Date = x.Supervisions.Date,
                ReceiveDate = x.Supervisions.ReceiveDate,
                ReceivedUserName = x.Supervisions.Users != null ? x.Supervisions.Users.SupEngineerName ?? "" : "",
                ReceviedUserStampUrl = x.Supervisions.Users != null ? x.Supervisions.Users.StampUrl ?? "" : "",
                SupEngineerCert = x.Supervisions.Users != null ? x.Supervisions.Users.SupEngineerCert ?? "" : "",
                CustomerName = x.Supervisions.Project != null ? x.Supervisions.Project.customer != null ? x.Supervisions.Project.customer.CustomerNameAr ?? "" : "" : "",
                ProjectNo = x.Supervisions.Project != null ? x.Supervisions.Project.ProjectNo : "",
                PieceNo = x.Supervisions.PieceNo ?? "",
                LicenseNo = x.Supervisions.LicenseNo ?? "",
                OutlineNo = x.Supervisions.OutlineNo ?? "",
                VisitDate = x.Supervisions.VisitDate ?? "",
                OfficeName = x.Supervisions.Project != null ? x.Supervisions.Project.Co_opOfficeName : "",
                OfficeEmail = x.Supervisions.Project != null ? x.Supervisions.Project.Co_opOfficeEmail : "",
                ContractorEmail = x.Supervisions.Project != null ? x.Supervisions.Project.Contractor != null ? x.Supervisions.Project.Contractor.Email : "" : "",
                ContractorName = x.Supervisions.Project != null ? x.Supervisions.Project.Contractor != null ? x.Supervisions.Project.Contractor.NameAr : "" : "",
                ImageUrl = x.ImageUrl ?? "",
                HeadImageUrl = x.Supervisions.ImageUrl ?? "",
                HeadImageUrl2 = x.Supervisions.ImageUrl2 ?? "",

                HeadOutlineChangetxt1 = x.Supervisions.OutlineChangetxt1 ?? "",
                HeadOutlineChangetxt2 = x.Supervisions.OutlineChangetxt2 ?? "",
                HeadOutlineChangetxt3 = x.Supervisions.OutlineChangetxt3 ?? "",
                HeadPointsNotWrittentxt1 = x.Supervisions.PointsNotWrittentxt1 ?? "",
                HeadPointsNotWrittentxt2 = x.Supervisions.PointsNotWrittentxt2 ?? "",
                HeadPointsNotWrittentxt3 = x.Supervisions.PointsNotWrittentxt3 ?? "",



                MunicipalName = x.Supervisions.Municipal != null ? x.Supervisions.Municipal.NameAr : "",
                SubMunicipalityName = x.Supervisions.SubMunicipality != null ? x.Supervisions.SubMunicipality.NameAr : "",
                BuildTypeName = x.Supervisions.BuildTypes != null ? x.Supervisions.BuildTypes.NameAr : "",
                ProjectDiscName = x.Supervisions.Project != null ? x.Supervisions.Project.ProjectDescription : "",
                DistrictName = x.Supervisions.DistrictName ?? "",
                ProBuildingDisc = x.Supervisions.ProBuildingDisc ?? "",
                AdwARid = x.Supervisions.AdwARid ?? 0,
                SuperDateConfirm = x.Supervisions.SuperDateConfirm ?? "",
                SuperStatus = x.Supervisions.SuperStatus ?? 0,
                TheNumber = x.TheNumber ?? "",
                TheLocation = x.TheLocation ?? "",

            }).ToList();
            return SupervisionDetails;
        }
    }
}
