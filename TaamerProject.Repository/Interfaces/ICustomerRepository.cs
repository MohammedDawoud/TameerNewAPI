using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICustomerRepository 
    {
        Customer GetById(int CustomerId);
        Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetAllCustomersW(string lang, int BranchId, bool isPrivate);

        Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId);
        Task<IEnumerable<CustomerVM>> GetAllCustomer();
        Task<IQueryable<CustomerVM>> GetCustomersArchiveProjects(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetCustomersOwnProjects(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetCustomersOwnNotArcheivedProjects(string lang, int BranchId, bool isPrivate);
        Task<List<CustomerVM>> GetCustomersNewTask(string lang, int UserId, int BranchId);
        Task<CustomerVM> GetCustomerInfo(string SearchText);

        Task<IQueryable<CustomerVM>> GetAllCustomersCount(int BranchId);
        Task<IEnumerable<CustomerVM>> CustomerInterval(string FromDate, string ToDate, int BranchId, string lang);
        Task<IEnumerable<CustomerVM>> CustomerIntervalByCustomerType(string FromDate, string ToDate, int customerType, int BranchId, string lang);
        Task<IQueryable<CustomerVM>> GetAllCustomersByCustomerTypeId(int? CustomerTypeId, string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> SearchCustomers(CustomerVM CustomersSearch, string lang, int BranchId);
        Task<IQueryable<CustomerVM>> GetAllPrivateCustomers(string lang, int BranchId);
        Task<int> GetCitizensCount(int BranchId);
        Task<int> GetInvestorCompanyCount(int BranchId);
        Task<int> GetCustomerByEmail(string CustomerSearchEmail, int BranchId);
        Task<int> GetGovernmentSideCount(int BranchId);
        Task<int?> GenerateNextCustomerCodeNumber();
        Task<int> GetCustomersCount(int BranchId);
        Task<IEnumerable<CustomerVM>> GetAllCustomersProj(string lang, int BranchId);
        Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenue(string FromDate, string ToDate, int BranchId, string lang);
        Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenueDGV(string FromDate, string ToDate, int BranchId, string lang, string Con);
        Task<CustomerVM> GetCustomersByCustomerId(int? CustomerId, string lang);
        Task<CustomerVM> GetCustomersByCustomerIdInvoice(int? CustomerId, string lang);

        Task<CustomerVM> GetCustomersByCustomerIdOnly(int? CustomerId, string lang);
        Task<CustomerVM> GetCustomersByAccountId(int? AccountId, string lang);

        Task<IEnumerable<CustomerVM>> GetCustomersByCustId(int? CustomerId);
        Task<CustomerVM> GetCustomersByNationalId(string NationalId, string lang);
        Task<CustomerVM> GetCustomersByCommercialRegister(string CommercialRegister, string lang);
        Task<IEnumerable<CustomerVM>> GetAllCustomers(string SearchText);
        Task<IEnumerable<CustomerVM>> FillAllCustomerSelectWithBranch(string SearchText, int BranchId);

        Task<IEnumerable<CustomerVM>> GetAllCustomersNotHaveProj(string lang, int BranchId);
        Task<IEnumerable<CustomerVM>> FillAllCustomerSelectNotHaveProjWithBranch(string lang, int BranchId);

        Task<IEnumerable<CustomerVM>> GetAllCustomersNotHaveProjWithout(string lang, int BranchId);


        Task<IEnumerable<CustomerVM>> GetAllCustomerForDrop(string lang);
        Task<IEnumerable<CustomerVM>> GetAllCustomerForDropWithBranch(string lang,int BranchId);

        Task<IQueryable<CustomerVM>> GetAllCustomersexist(string lang);

        Task<IQueryable<CustomerVM>> GetAllCustomersSearch(string searchtxt, string lang, int BranchId, bool isPrivate);
    }
}
