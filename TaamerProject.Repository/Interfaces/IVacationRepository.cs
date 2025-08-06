using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IVacationRepository
    {
        Task<IEnumerable<VacationVM>> GetAllVacations(int? EmpId, string SearchText);
        Task<IEnumerable<VacationVM>> GetAllVacationsArchived(int? EmpId, string SearchText);
        Task<IEnumerable<VacationVM>> GetAllVacations2(int? UserId, string SearchText);

        Task<IEnumerable<VacationVM>> GetAllVacationsw( string SearchText);
            
        Task<IEnumerable<VacationVM>> GetAllVacationsSearch(int BranchId);
        Task<IEnumerable<VacationVM>> GetAllVacationsBySearchObject(VacationVM VacationSearch, int BranchId);
        Task<List<string>> GetVacationDays(int EmpId, string Con);
        Task<List<string>> GetVacationApprovedDays(int EmpId, string Con);
        Task<List<string>> GetVacationDays_WithoutHolidays(string StartDate, string EndDate, int DawamId, string Con, int vacationttypeiid);
        Task<IEnumerable<VacationVM>> GetAllVacationsw2(string SearchText, int status);
    }
}
