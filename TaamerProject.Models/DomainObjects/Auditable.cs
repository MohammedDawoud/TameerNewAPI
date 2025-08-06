using System;

namespace TaamerProject.Models
{
    public abstract class Auditable :  IAuditable
    {
        public Auditable()
        {
            this.IsDeleted = false;
        }

        public int? AddUser { get; set; }
        public int? UpdateUser { get; set; }
        public int? DeleteUser { get; set; }
        public DateTime? AddDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
