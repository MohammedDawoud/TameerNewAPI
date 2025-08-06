using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_Super_PhaseDetRepository
    {
        Task<IEnumerable<Pro_Super_PhaseDetVM>> GetAllSuper_PhaseDet(int? PhaseId);

    }
}
