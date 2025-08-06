using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class Exceptions
    {
        public int ExceptionId { get; set; }
        public string? Date { get; set; }
        public string? PageName { get; set; }
        public string? MethodName { get; set; }
        public string? Exception { get; set; }
        public DateTime? DateandTime { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
