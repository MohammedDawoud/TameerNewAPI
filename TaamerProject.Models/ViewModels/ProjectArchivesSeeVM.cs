using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ProjectArchivesSeeVM : Auditable
    {
        public int ProArchSeeID { get; set; }
        public int? ProArchReID { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public int? See_TypeID { get; set; }


    }
}
