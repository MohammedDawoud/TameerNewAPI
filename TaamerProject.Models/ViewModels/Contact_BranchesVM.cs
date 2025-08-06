using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{

    public class Contact_BranchesVM
    {

        public int ContactId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchAddress { get; set; }
        public string? BranchPhone { get; set; }
        public string? BranchCS { get; set; }
        public string? BranchEmail { get; set; }
    }
}
