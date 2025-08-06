using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class SupervisionsVM
    {
        public int SupervisionId { get; set; }
        public int? Number { get; set; }
        public string? Phase { get; set; }
        public int? ProjectId { get; set; }
        public string? Location { get; set; }
        public string? Date { get; set; }
        public string? stringDate { get; set; }
        public string? ReceiveNotes { get; set; }
        public string? ManagerNotes { get; set; }
        public int? ReceivedEmpId { get; set; }
        public bool? ReceiveStatus { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public string? ReceivedUserName { get; set; }
        public string? ProjectName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerName_W { get; set; }

        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public bool IsRead { get; set; }
        public string? ProjectNo { get; set; }
        public string? ProjectManager { get; set; }


        public int? PhaseId { get; set; }
        public string? PieceNo { get; set; }
        public string? LicenseNo { get; set; }
        public string? OutlineNo { get; set; }
        public int? WorkerId { get; set; }
        public string? VisitDate { get; set; }
        public string? PhaseName { get; set; }
        public string? Status { get; set; }
        public string? OfficeName { get; set; }
        public string? OfficeEmail { get; set; }
        public string? OfficePhone { get; set; }

        public string? ContractorEmail { get; set; }
        public string? ContractorPhone { get; set; }
        public int? SuperStatus { get; set; }
        public string? SuperStatusName { get; set; }
        public string? SuperDateConfirm { get; set; }
        public DateTime? AddDate { get; set; }

        public int? MunicipalSelectId { get; set; }
        public int? SubMunicipalitySelectId { get; set; }
        public int? ProBuildingTypeSelectId { get; set; }
        public string? DistrictName { get; set; }
        public string? ProBuildingDisc { get; set; }
        public int? AdwARid { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageUrl2 { get; set; }

        public string? MunicipalName { get; set; }
        public string? SubMunicipalityName { get; set; }
        public string? BuildTypeName { get; set; }
        public string? ContractorName { get; set; }
        public string? ProjectDiscName { get; set; }

        public string? OutlineChangetxt1 { get; set; }
        public string? OutlineChangetxt2 { get; set; }
        public string? OutlineChangetxt3 { get; set; }
        public string? PointsNotWrittentxt1 { get; set; }
        public string? PointsNotWrittentxt2 { get; set; }
        public string? PointsNotWrittentxt3 { get; set; }
        public string? SuperCode { get; set; }
        public string? TimeStr { get; set; }
        public string Catego { get; set; }
        public string? CityName { get; set; }
        public string? ProPieceNumber { get; set; }
        public string? Cons_components { get; set; }
        public int? RequiredServiceId { get; set; }
        public string? RequiredServiceName { get; set; }



    }
}
