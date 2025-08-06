
using System;

namespace TaamerProject.Models
{
    public interface IAuditable
    {
         int? AddUser { get; set; }
         int? UpdateUser { get; set; }
         int? DeleteUser { get; set; }
         DateTime? AddDate { get; set; }
         DateTime? UpdateDate { get; set; }
         DateTime? DeleteDate { get; set; }
         bool IsDeleted { get; set; }

    }
}
