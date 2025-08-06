
namespace TaamerProject.Models
{
    public class ProjectArchivesRe : Auditable
    {
        public int ProArchReID { get; set; }
        public int? ProjectId { get; set; }
        public string? ReDate { get; set; }
        public int? Re_TypeID { get; set; }
        public string? Re_TypeName { get; set; }
        public int? Re_PhasesTaskId { get; set; }


        public virtual Project? Project { get; set; }
        public virtual ProjectPhasesTasks? Phases { get; set; }



    }
}
