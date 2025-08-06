using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_ProjectStepsService
    {
        Task<IEnumerable<Pro_ProjectStepsVM>> GetAllProjectSteps();
        Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectid(int projectid, int stepid);
        Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectidOnly(int projectid);

        GeneralMessage UpdateProjectStepStatus(Pro_ProjectSteps ProjectStep, int UserId, int BranchId);
        GeneralMessage ReturnProjectStepStatus(Pro_ProjectSteps ProjectStep, int UserId, int BranchId);

        GeneralMessage SaveProjectStep(int projectId, int UserId, int BranchId);
        GeneralMessage DeleteProjectStep(int ProjectStepid, int UserId, int BranchId);
    }
}
