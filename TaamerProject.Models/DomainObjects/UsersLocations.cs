using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class UsersLocations : Auditable
    {
        public int LocationId { get; set; }
        public int? UserId { get; set; }
        public string? Ip { get; set; }
        public string? IPType { get; set; }
        public string? ContinentCode { get; set; }
        public string? ContinentName { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? RegionCode { get; set; }
        public string? RegionName { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? TimeZone { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
