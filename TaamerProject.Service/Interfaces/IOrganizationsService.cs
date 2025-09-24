using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IOrganizationsService
    {
        Task<OrganizationsVM> GetBranchOrganization(int? BranchId);
        Task<OrganizationsVM> GetBranchOrganization();
        GeneralMessage SaveOrganizations(Organizations organizations, int UserId, int BranchId);
        GeneralMessage SaveComDomainLink(Organizations organizations, int UserId, int BranchId);


        GeneralMessage SaveOrganizationSettings(Organizations organizations, int UserId, int BranchId);
        GeneralMessage SavepartialOrganizations(Organizations organizations, int UserId, int BranchId, decimal VAT, int VATSet);
        GeneralMessage SaveRetentionBackup(Organizations organizations, int UserId, int BranchId, decimal VAT, int VATSet);
        GeneralMessage SaveCSIDOrganizations(int OrganizationId,string CSR,string PrivateKey,string CSID,string SecretKey, int UserId, int BranchId);
        GeneralMessage SaveErrorMessageCSIDOrganizations(int OrganizationId, string ErrorMessage, int UserId, int BranchId);

        GeneralMessage DeleteOrganizations(int OrganizationId, int UserId, int BranchId);
        Task<OrganizationsVM> CheckEmailOrganization(int? OrganizationId);
        Task<OrganizationsVM> GetBranchOrganizationData(int orgId);
        Task<OrganizationsVM> GetComDomainLink_Org(int orgId);
        Task<OrganizationsVM> GetApplicationVersion_Org(int orgId);

        Task<OrganizationsVM> GetBranchOrganizationDataInvoice(int orgId);

        Task<OrganizationsVM> GetOrganizationData(int branchId);
        Task<string> GetDefaultLogoOfOrganization();
        Task<OrganizationsVM> GetOrganizationDataLogin(string Lang);
        GeneralMessage SaveAppVersion(Organizations organizations, int UserId, int BranchId);
        GeneralMessage SaveSupport(Organizations organizations, int UserId, int BranchId);

        Task<List<FillSelectVM>> FillSelect_Proc(string Con,int? UserId,int Type,int? Param, int? Status, int? BranchId);
        Task<List<FillSelectVM>> FillSelect_Cust(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId);

        GeneralMessage SendMail_test(int BranchId, string ReceivedUser, string Subject, string textBody, bool IsBodyHtml = false);

    }
}
