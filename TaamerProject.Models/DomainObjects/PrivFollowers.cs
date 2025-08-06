using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class PrivFollowers : Auditable
    {
        public int? PrivFollowerID { get; set; }
        public int? UserID { get; set; }
        public int? TaskID { get; set; }
        public int? Flag { get; set; }
        public Users? Users { get; set; }
        public ProjectPhasesTasks? ProjectPhasesTasks { get; set; }
    }
}
