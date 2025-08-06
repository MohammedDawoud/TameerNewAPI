using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_SupervisionDetailsRepository
    {
        Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetails(string SearchText);
        Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetailsBySuperId(int? SupervisionId);

    }
}
