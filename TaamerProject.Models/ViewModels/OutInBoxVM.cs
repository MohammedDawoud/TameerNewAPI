using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class OutInBoxVM
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
        public int? FileCount { get; set; }
        public string? AttachmentUrl { get; set; }
        public int? ProjectId { get; set; }
        public int? BranchId { get; set; }
        public int? Priority { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerName_W { get; set; }

        public string? OutInTypeName { get; set; }
        public string? OutInBoxTypeName { get; set; }
        public string? OutInBoxSerialCode { get; set; }
        public string? SideFromName { get; set; }
        public string? SideToName { get; set; }
        public string? ArchiveFilesName { get; set; }
        public string? PriorityName { get; set; }
        public string? InnerOutInDate { get; set; }
        public string? RelatedToSideTo { get; set; }
        public string? RelatedToTopic { get; set; }
        public string? RelatedToDate { get; set; }
        public int? NumberType { get; set; }
        public string? RelatedToName { get; set; }
        public string? ProjectNumber { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public int? Code { get; set; }

    }
}
