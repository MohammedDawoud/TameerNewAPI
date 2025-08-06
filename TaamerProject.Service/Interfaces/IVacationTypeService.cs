using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IVacationTypeService  
    {
        Task<IEnumerable<VacationTypeVM>> GetAllVacationsTypes(string SearchText);
        GeneralMessage SaveVacationType(VacationType vacationType,int UserId, int BranchId);
        GeneralMessage DeleteVacationType(int VacationTypeId,int UserId, int BranchId);
        Task<IEnumerable<VacationTypeVM>> FillVacationTypeSelect(string SearchText = "");
    }
}
