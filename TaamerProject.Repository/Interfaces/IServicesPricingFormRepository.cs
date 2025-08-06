using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IServicesPricingFormRepository
    {
        Task<IEnumerable<ServicesPricingFormVM>> GetAllServicesPricingForms(string SearchText, int BranchId);
        Task<ServicesPricingFormVM> GetServicesPricingFormById(int FormId, int BranchId);


        Task<IEnumerable<ServicesPricingFormVM>> GetAllPublicanddesignServicesPricingForms(int Type, int BranchId);

        Task<IEnumerable<ServicesPricingFormVM>> GetAllPublicanddesignServicesPricingFormstocount(int Type, int BranchId);
        Task<IEnumerable<ServicesPricingFormVM>> FilterPublicanddesignServicesPricingForms(int Type, int BranchId, string date);

    }
}
