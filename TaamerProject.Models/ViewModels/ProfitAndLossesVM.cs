using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class ProfitAndLossesVM
    {
        public List<AccountVM>? Trading { get; set; }
        public List<AccountVM>? InComeState { get; set; }
        public List<AccountVM>? Expenses { get; set; }
    }
}
