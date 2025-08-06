using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class MachineInfo
    {
        public int MachineNumber { get; set; }
        public int IndRegID { get; set; }
        public string? DateTimeRecord { get; set; }

        public DateTime DateOnlyRecord
        {
            get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("yyyy-MM-dd")); }
        }
        public DateTime TimeOnlyRecord
        {
            get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("hh:mm:ss tt")); }
        }

    }
}
