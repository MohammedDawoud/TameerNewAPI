using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_Suppliers : Auditable
    {
        public int SupplierId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? TaxNo { get; set; }
        public string? PhoneNo { get; set; }
        public int? AccountId { get; set; }
        public string? CompAddress { get; set; }
        public string? PostalCodeFinal { get; set; }
        public string? ExternalPhone { get; set; }
        public string? Country { get; set; }
        public string? Neighborhood { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingNumber { get; set; }
        public int? CityId { get; set; }
        public virtual City? city { get; set; }
    }
}
