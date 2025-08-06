using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class AttendenceLocationSettings : Auditable
    {
        public int AttendenceLocationSettingsId { get; set; }
        public string? Name { get; set; }
        public decimal? Distance { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? xmax { get; set; }
        public string? xmin { get; set; }
        public string? ymax { get; set; }
        public string? ymin { get; set; }
        //public virtual List<Employees>? Employees { get; set; }
        public virtual List<EmpLocations>? EmpLocations { get; set; }


    }
}
