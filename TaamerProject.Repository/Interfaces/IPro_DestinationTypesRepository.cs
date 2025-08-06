using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_DestinationTypesRepository
    {
        Task<IEnumerable<Pro_DestinationTypesVM>> GetAllDestinationTypes();
    }
}
