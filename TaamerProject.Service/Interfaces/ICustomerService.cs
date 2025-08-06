using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetAllCustomersW(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetCustomersArchiveProjects(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetCustomersOwnProjects(string lang, int BranchId, bool isPrivate);
        Task<IQueryable<CustomerVM>> GetCustomersOwnNotArcheivedProjects(string lang, int BranchId, bool isPrivate);
        Task<List<CustomerVM>> GetCustomersNewTask(string lang, int BranchId, int UserId);
        Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId);
        Task<IQueryable<CustomerVM>> GetAllCustomersCount(int BranchId);
        Task<IEnumerable<CustomerVM>> CustomerInterval(string FromDate, string ToDate, int BranchId, string lang);
        Task<IEnumerable<CustomerVM>> GetAllCustomersByCustomerTypeId(string FromDate, string ToDate, int CustomerTypeId, int BranchId, string lang);
        Task<IEnumerable<CustomerVM>> GetAllCustomer();
        Task<CustomerVM> GetCustomerInfo(string SearchText);

        Task<IEnumerable<CustomerVM>> GetAllCustomerForDrop(string lang);
        Task<IEnumerable<CustomerVM>> GetAllCustomerForDropWithBranch(string lang, int BranchId);

        Task<IEnumerable<CustomerVM>> GetAllCustomersByCustomerTypeId(int? CustomerTypeId, string lang, int BranchId, bool isPrivate);
        Task<IEnumerable<object>> GetCustFinancialDetails(string FromDate, string ToDate, int CustomerId, int? yearid);
        Task<IEnumerable<TransactionsVM>> GetAllCustomerTrans(string FromDate, string ToDate, int BranchId, int? yearid);
        // GeneralMessage SaveCustomer(Customer customer, int UserId, int BranchId);
        GeneralMessage SaveCustomer(Customer customer, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage DeleteCustomer(int customerId, int UserId, int BranchId);
        Task<IEnumerable<CustomerVM>> SearchCustomers(CustomerVM CustomersSearch, string lang, int BranchId);
        Task<IEnumerable<CustomerVM>> GetAllPrivateCustomers(string lang, int BranchId);
        Task<int?> GenerateNextCustomerCodeNumber();
        Task<IEnumerable<CustomerVM>> GetAllCustomersProj(string lang, int BranchId);
        Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenue(string FromDate, string ToDate, int BranchId, string lang);
        Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenueDGV(string FromDate, string ToDate, int BranchId, string lang, string Con);
        Task<IEnumerable<ContractsVM>> GetAllCustHaveRemainingMoney(CustomerVM CustomersSearch, string lang, int BranchId);
        Task<CustomerVM> GetCustomersByCustomerId(int? CustomerId, string lang);
        Task<CustomerVM> GetCustomersByProjectId(int? ProjectId, string lang);

        Task<CustomerVM> GetCustomersByCustomerIdInvoice(int? CustomerId, string lang);

        Task<CustomerVM> GetCustomersByAccountId(int? AccountId, string lang);

        Task<IEnumerable<CustomerVM>> GetCustomersByCustId(int? CustomerId);
        Task<CustomerVM> GetCustomersByNationalId(string NationalId, string lang);
        Task<CustomerVM> GetCustomersByCommercialRegister(string CommercialRegister, string lang);
        string GetAccountByCustomerId(int CustomerId, string lang);

        IEnumerable<object> FillAllCustomerSelect(string SearchText = "");
        IEnumerable<object> FillAllCustomerSelectWithBranch(string SearchText, int BranchId);

        IEnumerable<object> FillAllCustomerSelectNotHaveProj(string lang, int BranchId);
        IEnumerable<object> FillAllCustomerSelectNotHaveProjWithBranch(string lang, int BranchId);

        IEnumerable<object> FillAllCustomerSelectNotHaveProjWithout(string lang, int BranchId);
        Task<IQueryable<CustomerVM>> GetAllCustomerExist(string lang);
        Task<IQueryable<CustomerVM>> GetAllCustomersSearch(string searchtext, string lang, int BranchId, bool isPrivate);
    }
}
