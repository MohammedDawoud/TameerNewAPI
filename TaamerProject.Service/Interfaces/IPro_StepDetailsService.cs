using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_StepDetailsService
    {
        Task<IEnumerable<Pro_StepDetailsVM>> GetAllStepDetails();
        GeneralMessage SaveStepDetail(Pro_StepDetails StepDetail, int UserId, int BranchId);
        GeneralMessage DeleteStepDetail(int StepDetailId, int UserId, int BranchId);
    }
}
