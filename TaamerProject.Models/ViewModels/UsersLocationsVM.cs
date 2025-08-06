using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class UsersLocationsVM
    {
        public int LocationId { get; set; }
        public int? UserId { get; set; }
        public string? Ip { get; set; }
        public string? Type { get; set; }
        public string? Continent_Code { get; set; }
        public string? Continent_Name { get; set; }
        public string? Country_Code { get; set; }
        public string? Country_Name { get; set; }
        public string? Region_Code { get; set; }
        public string? Region_Name { get; set; }
        public string? City { get; set; }
        public string? Zip_Code { get; set; }
        public string? TimeZone { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

    }
}
