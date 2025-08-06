using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_Super_PhasesService  
    {
        Task<IEnumerable<Pro_Super_PhasesVM>> GetAllSuper_Phases(string SearchText);
        Task<Pro_Super_PhasesVM> GetSuper_PhasesById(int SuperId);

        Task<IEnumerable<Pro_Super_PhaseDetVM>> GetAllSuper_PhaseDet(int? PhaseId);

        GeneralMessage SaveSuper_Phases(Pro_Super_Phases Phases, int UserId, int BranchId);
        GeneralMessage DeleteSuper_Phases(int PhaseId, int UserId, int BranchId);
        GeneralMessage SaveSuperPhaseDet(List<Pro_Super_PhaseDet> PhaseDets, int UserId, int BranchId);


        GeneralMessage UpdateStatus_SuperPhaseDet(int PhaseDetailesId, bool? ISRead, int UserId, int BranchId);
    }
}
