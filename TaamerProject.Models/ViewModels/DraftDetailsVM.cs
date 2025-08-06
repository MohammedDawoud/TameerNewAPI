using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class DraftDetailsVM
    {
        public int DraftDetailId { get; set; }
        public int? DraftId { get; set; }
        public string? DraftName { get; set; }
        public int? ProjectId { get; set; }
        public string? ProjectNo { get; set; }
    }
}
