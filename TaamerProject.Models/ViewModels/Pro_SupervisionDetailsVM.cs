using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Pro_SupervisionDetailsVM
    {
        public int SuperDetId { get; set; }
        public int? SupervisionId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Note { get; set; }
        public int? IsRead { get; set; }
        public int? BranchId { get; set; }
        public string? PhaseName { get; set; }

        public string? ImageUrl { get; set; }
        public string? HeadImageUrl { get; set; }
        public string? HeadImageUrl2 { get; set; }
        public string? HeadOutlineChangetxt1 { get; set; }
        public string? HeadOutlineChangetxt2 { get; set; }
        public string? HeadOutlineChangetxt3 { get; set; }
        public string? HeadPointsNotWrittentxt1 { get; set; }
        public string? HeadPointsNotWrittentxt2 { get; set; }
        public string? HeadPointsNotWrittentxt3 { get; set; }

        
        public int? SupervisionNumber { get; set; }
        public string? Date { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? ReceivedUserName { get; set; }
        public string? ReceviedUserStampUrl { get; set; }
        public string? SupEngineerCert { get; set; }
        public string? ProjectName { get; set; }
        public string? CustomerName { get; set; }
        public string? ProjectNo { get; set; }
        public string? PieceNo { get; set; }
        public string? LicenseNo { get; set; }
        public string? OutlineNo { get; set; }
        public string? VisitDate { get; set; }
        public string? OfficeName { get; set; }
        public string? OfficeEmail { get; set; }
        public string? ContractorEmail { get; set; }
        public string? ContractorName { get; set; }
        public string? MunicipalName { get; set; }
        public string? SubMunicipalityName { get; set; }
        public string? BuildTypeName { get; set; }
        public string? ProjectDiscName { get; set; }
        public string? DistrictName { get; set; }
        public string? ProBuildingDisc { get; set; }
        public int? AdwARid { get; set; }
        public int? PhaseId { get; set; }
        public int? SuperStatus { get; set; }
        public string? SuperDateConfirm { get; set; }
        public string? TheNumber { get; set; }
        public string? TheLocation { get; set; }

    }
}
