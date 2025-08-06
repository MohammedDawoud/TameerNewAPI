using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICarMovementsRepository
    {
        Task<IEnumerable<CarMovementsVM>> GetAllCarMovements();
        Task<IEnumerable<CarMovementsVM>> SearchCarMovements(CarMovementsVM CarMovementsSearch, int BranchId);
        Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsByDateSearch(string FromDate, string ToDate);
        Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsSearchObject(CarMovementsVM VacationSearch, int BranchId);
        //heba
        Task<DataTable> GetAllCarMovementsByDateSearch(string carType, string carId, string FromDate, string ToDate, string EmpId, string Con);
        Task<DataTable> GetAllCarMovementsByDate(string fromDate, string toDate, string con);

    }
}
