using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class Supervisions : Auditable
    {
        public int SupervisionId { get; set; }
        public int? Number { get; set; }
        public string? Phase { get; set; }
        public int? ProjectId { get; set; }
        public string? Location { get; set; }
        public string? Date { get; set; }
        public string? ReceiveNotes { get; set; }
        public string? ManagerNotes { get; set; }
        public int? ReceivedEmpId { get; set; }
        public bool? ReceiveStatus { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public int? PhaseId { get; set; }
        public string? PieceNo { get; set; }
        public string? LicenseNo { get; set; }
        public string? OutlineNo { get; set; }
        public int? WorkerId { get; set; }
        public string? VisitDate { get; set; }
        public int? SuperStatus { get; set; }
        public string? SuperDateConfirm { get; set; }



        public int? MunicipalSelectId { get; set; }
        public int? SubMunicipalitySelectId { get; set; }
        public int? ProBuildingTypeSelectId { get; set; }
        public string? DistrictName { get; set; }
        public string? ProBuildingDisc { get; set; }
        public int? AdwARid { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageUrl2 { get; set; }
        public int? RequiredServiceId { get; set; }

        public string? OutlineChangetxt1 { get; set; }
        public string? OutlineChangetxt2 { get; set; }
        public string? OutlineChangetxt3 { get; set; }
        public string? PointsNotWrittentxt1 { get; set; }
        public string? PointsNotWrittentxt2 { get; set; }
        public string? PointsNotWrittentxt3 { get; set; }


        public virtual Pro_Municipal? Municipal { get; set; }
        public virtual Pro_SubMunicipality? SubMunicipality { get; set; }

        public virtual BuildTypes? BuildTypes { get; set; }
        public virtual Users? Users { get; set; }
        public virtual Project? Project { get; set; }
        public virtual Pro_Super_Phases? Pro_Super_Phases { get; set; }
        public virtual List<Pro_SupervisionDetails>? SupervisionDetails { get; set; }
        //public int? CustomerId { get; set; }
        //public Customer Customer { get; set; }
    }
}
