using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICarMovementsTypeRepository
    {
        Task<IEnumerable<CarMovementsTypeVM>> GetAllCarMovmentsTypes(string SearchText);
    }
}
