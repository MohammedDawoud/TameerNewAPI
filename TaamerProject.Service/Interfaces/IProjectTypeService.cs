using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectTypeService 
    {
        IEnumerable<ProjectTypeVM> GetAllProjectType(string SearchText);
        GeneralMessage SaveProjectType(ProjectType projectType, int UserId, int BranchId);
        GeneralMessage DeleteProjectType(int projectTypeId, int UserId, int BranchId);

    }
}
