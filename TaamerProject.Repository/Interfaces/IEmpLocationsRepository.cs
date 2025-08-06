using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IEmpLocationsRepository
    {
        Task<IEnumerable<EmpLocationsVM>> GetAllLocationsByEmpId(string Lang, int empId);
        Task<IEnumerable<EmpLocationsVM>> GetLocationByLocationId(string Lang, int locationId);
    }
}
