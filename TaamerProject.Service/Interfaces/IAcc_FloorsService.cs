using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_FloorsService
    {
        Task<IEnumerable<Acc_FloorsVM>> GetAllFloors(string SearchText);
        GeneralMessage SaveFloor(Acc_Floors Floor, int UserId, int BranchId);
        GeneralMessage DeleteFloor(int FloorId, int UserId, int BranchId);
    }
}
