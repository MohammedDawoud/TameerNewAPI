using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IServicesPriceService 
    {
        GeneralMessage SaveService(Acc_Services_Price service, int UserId, int BranchId, List<Acc_Services_Price>? Details);
        GeneralMessage DeleteService(int? ServicesId, int UserId, int BranchId);
        Task<IEnumerable<AccServicesPricesVM>> GetAllServicesPrice();
        Task<AccServicesPricesVM> GetServicesPriceByServiceId(int ServiceId);
        Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId(int? ProjectId);
        Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId2(int? ProjectId, int? ProjectId2);
        Task<IEnumerable<AccServicesPricesVM>> GetServicePriceByProject_Search(int? Project1, int? Project2, string ServiceName, string ServiceDetailName, decimal? Amount);
        Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByParentId(int? ParentId);
        Task<decimal?> GetServicesPriceAmountByServicesId(int? ServicesId);
    }
}
