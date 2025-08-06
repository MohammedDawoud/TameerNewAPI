using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IServicesPriceServiceRepository
    {
        Task<IEnumerable<AccServicesPricesVM>> GetAllServicesPrice();
        Task<AccServicesPricesVM> GetServicesPriceByServiceId(int ServiceId);
        Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId(int? projectId);
        Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByProjectId2(int? projectId, int? projectId2);
        Task<IEnumerable<AccServicesPricesVM>> GetServicePriceByProject_Search(int? Project1, int? Project2, string ServiceName, string ServiceDetailName, decimal? Amount);
        Task<IEnumerable<AccServicesPricesVM>> GetServicesPriceByParentId(int? ParentId);
        Task<decimal?> GetServicesPriceAmountByServicesId(int? ServicesId);
    }
}
