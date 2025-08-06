using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IServicesPricingFormService  
    {
        Task<IEnumerable<ServicesPricingFormVM>> GetAllServicesPricingForms(string SearchText, int BranchId);
        Task<ServicesPricingFormVM> GetServicesPricingFormById(int FormId, int BranchId);

        GeneralMessage SaveServicesPricingForm(ServicesPricingForm Form, int UserId, int BranchId);
        GeneralMessage UpdateURL(ServicesPricingForm Form, int UserId);

        ServicesPricingForm SaveServicesPricingForm_Ret_DATA(ServicesPricingForm Form, int UserId, int BranchId);

        GeneralMessage DeleteServicesPricingForm(int FormId, int UserId, int BranchId);

        Task<IEnumerable<ServicesPricingFormVM>> GetAllPublicanddesignServicesPricingForms(int Type, int BranchId);

        int GetAllPublicanddesignServicesPricingFormstocount(int Type, int BranchId);

        Task<IEnumerable<ServicesPricingFormVM>> FilterPublicanddesignServicesPricingForms(int Type, int BranchId, string date);

        GeneralMessage UpdateStatusServicesPricingForm(int FormId, bool status, int UserId);
    }
}
