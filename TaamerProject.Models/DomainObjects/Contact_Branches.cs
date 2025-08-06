using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class Contact_Branches : Auditable
    {


        public int ContactId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchAddress { get; set; }
        public string? BranchPhone { get; set; }
        public string? BranchCS { get; set; }
        public string? BranchEmail { get; set; }
    
    }
}
