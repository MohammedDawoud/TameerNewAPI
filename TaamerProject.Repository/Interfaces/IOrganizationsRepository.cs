using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IOrganizationsRepository :IRepository<Organizations>
    {
        Task<OrganizationsVM> GetBranchOrganization(int? BranchId);
        Task<OrganizationsVM> GetBranchOrganization();
        Task<OrganizationsVM> CheckEmailOrganization(int? OrganizationId);
        Task<OrganizationsVM> GetBranchOrganizationData(int orgId);
        Task<OrganizationsVM> GetComDomainLink_Org(int orgId);

        Task<OrganizationsVM> GetBranchOrganizationDataInvoice(int orgId);

        Task<string> GetDefaultLogoOfOrganization();

        Task<OrganizationsVM> GetOrganizationDataLogin(string Lang);
        Task<OrganizationsVM> GetApplicationVersion_Org(int OrgId);
        Task<List<FillSelectVM>> FillSelect_Proc(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId);
        Task<List<FillSelectVM>> FillSelect_Cust(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId);

    }
}
