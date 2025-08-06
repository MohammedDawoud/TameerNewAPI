using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaamerProject.Models
{
    public class OutInBox : Auditable
    {
        public int OutInBoxId { get; set; }
        public string? Number { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public int? TypeId { get; set; }
        public int? SideFromId { get; set; }
        public int? SideToId { get; set; }
        public int? InnerId { get; set; }
        public string? Topic { get; set; }
        public int? ArchiveFileId { get; set; }
        public int? RelatedToId { get; set; }
        public int? Type { get; set; }
        public int? OutInType { get; set; }
        public int? NumberType { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? ProjectId { get; set; }
        public int? BranchId { get; set; }
        public int? Priority { get; set; }
        public int? Code { get; set; }
        public Project? Project { get; set; }
        public OutInBoxType? OutInBoxType { get; set; }
        public OutInBoxSerial? OutInBoxSerial { get; set; }
        public OutInBox? InnerToOutIn { get; set; }
        public OutInBox? RelatedToOutIn { get; set; }
        public ArchiveFiles? ArchiveFiles { get; set; }
        public Department? FromDepartment { get; set; }
        public Department? ToDepartment { get; set; }
        public List<ContacFiles>? ContacFiles { get; set; }

        [NotMapped]
        public List<int>? OutInImagesIds { get; set; }
    }
}
