using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
   public  class ReasonLeave : Auditable
    {

        public int ReasonId { get; set; }
        public string? ReasonTxt { get; set; }
        public string? DesecionTxt { get; set; }
    }
}
