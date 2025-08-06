using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IUsersLocationsService  
    {
        bool SaveUsersLocations(string ipAddress, int UserId, int BranchId);
    
    }
}
