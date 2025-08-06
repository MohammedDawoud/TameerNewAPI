using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICarMovementsService
    {
        Task<IEnumerable<CarMovementsVM>> GetAllCarMovements();
        Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsSearch(CarMovementsVM Search, int BranchId);
        GeneralMessage SaveCarMovement(CarMovements carMovement, int UserId, int BranchId, int? YearId);
        GeneralMessage DeleteCarMovement(int MovementId, int UserId, int BranchId);
        //IEnumerable<object> FillCarMovementSelect();
        Task<IEnumerable<CarMovementsVM>> SearchCarMovements(CarMovementsVM CarMovementsSearch, int BranchId);
        Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsByDateSearch(string FromDate, string ToDate);
        //heba
        Task<DataTable> GetAllCarMovementsByDateSearch(string CarType, string CarId, string StartDate, string EndDate, string EmpId, string Con);
        Task<DataTable> GetAllCarMovementsByDate(string fromDate, string toDate, string con);
    }
}
