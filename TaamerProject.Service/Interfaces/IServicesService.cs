using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IServiceService  
    {
        Task<IEnumerable<ServicesVM>> GetAllServices(int BranchId);
        GeneralMessage SaveService(TaamerProject.Models.Service service, int UserId, int BranchId);
        GeneralMessage DeleteService( int ServiceId ,int UserId, int BranchId);
        Task<IEnumerable<ServicesVM>> GetServicesSearch(ServicesVM ServiceSearch, int BranchId);

        IEnumerable<ServicesVM> GetServicesToNotified(int BranchId, string lang);
        Task<int?> GenerateServicesNumber(int BranchId);
        IEnumerable<rptGetDeservedServicesVM> GetDeservedServices(string Con);


    }
}
