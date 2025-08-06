using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ModelVM
    {
        public int ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Date { get; set; }
        public string? HijriDate { get; set; }
        public string? Notes { get; set; }
        public int? UserId { get; set; }
        public string? FileUrl { get; set; }
        public int? TypeId { get; set; }
        public string? Extension { get; set; }
        public string?  UserName { get; set; }
        public string? FileTypeName { get; set; }
    }
}
