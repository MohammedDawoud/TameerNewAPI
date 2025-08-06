using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_Super_PhasesRepository
    {
        Task<IEnumerable<Pro_Super_PhasesVM>> GetAllSuperPhases(string SearchText);
        Task<Pro_Super_PhasesVM> GetSuper_PhasesById(int SuperId);

    }
}
