using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<CityVM>> GetAllCities(string SearchText);
        GeneralMessage SaveCity(City city, int UserId, int BranchId);
        GeneralMessage DeleteCity(int CityId, int UserId, int BranchId);
        IEnumerable<object> FillCitySelect(string SearchText);
        City GetCityById(int CityId);
    }
}
