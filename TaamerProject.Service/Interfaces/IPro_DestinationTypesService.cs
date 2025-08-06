using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IPro_DestinationTypesService
    {
        Task<IEnumerable<Pro_DestinationTypesVM>> GetAllDestinationTypes();
        GeneralMessage SaveDestinationType(Pro_DestinationTypes DestinationType, int UserId, int BranchId);
        GeneralMessage DeleteDestinationType(int DestinationTypeId, int UserId, int BranchId);
    }
}
