
namespace TaamerProject.Models
{
    public class ImportantProject : Auditable
    {


        public int ImportantProId { get; set; }


        public int? ProjectId { get; set; }

        public int? UserId { get; set; }
        public int? Flag { get; set; }
        public int? BranchId { get; set; }

        public int? IsImportant { get; set; }
        public virtual Project? project { get; set; }




    }
}
