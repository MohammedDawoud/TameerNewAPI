using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IEmployeesRepository : IRepository<Employees>
    {
        Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang,int BranchId);
        Task<IEnumerable<EmployeesVM>> GetAllEmployeesByLocationId(string lang, int LocationId);

        Task<IEnumerable<EmployeesVM>> FillAllEmployees(string lang, int BranchId);
        Task<IEnumerable<EmployeesVM>> FillSelectEmployeeWorkers(string lang, int BranchId);

        Task<IEnumerable<EmployeesVM>> GetAllBranchEmployees(string lang);
        Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int SearchAll, int BranchId);
        Task<IEnumerable<EmployeesVM>> GetAllEmployeesSearch(string lang, int BranchId);
        Task<IEnumerable<EmployeesVM>> GetEmployeeByUserid(int UserId);
        Task<IEnumerable<EmployeesVM>> GetAllEmployeesBySearchObject(EmployeesVM SalarySearch, string lang, int BranchId, string Con = "");
        Task<IEnumerable<EmployeesVM>> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, string Con = "");
        Task<IEnumerable<EmployeesVM>> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, string Con = "");
        //Task<IEnumerable<EmployeesVM>> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, int YearId, string Con = "");
        Task<IEnumerable<EmployeesVM>> GetAllUsersEmployees();
        Task<List<int>> GetEmpsInVacations(int? month = 0);
        Task<List<string>> GetEmpNosInVacations(int? month = 0);
        Task<EmployeesVM> GetEmployeeById(int EmpId,string lang);
        Task<EmployeesVM> GetEmployeeById_d(int EmployeeId, string lang);
        Task<IEnumerable<EmployeesVM>> SearchEmployees(EmployeesVM EmployeesSearch,string lang, int BranchId);
        Task<EmployeesVM> GetEmployeeInfo(int EmployeeId, string lang, int BranchId);

        Task<List<int>> GetAllActiveEmpsByDate(string StartDate, string EndDate);
        Task<Object> GetEmployeeStatistics();
        Task<int> SearchEmployeesOfPass(string EmployeesSearchpass, string lang, int BranchId, int UserId);
        Task<int> SearchEmployeesOfEmail(string EmployeesSearchEmail, int BranchId);
        Task<int> SearchEmployeesOfNational(string EmployeesSearchNationalId, string lang, int BranchId, int UserId);
        Task<int> GenerateNextEmpNumber(int BranchId);
        Task<int> GetVacationsCount(int BranchId);
        Task<int> GetNationalityIdCount(int BranchId);
        Task<int> GetPassportNoCount(int BranchId);
        Task<List<int?>> GetEmployeeByDepartment(int departmentId);
        //Task<IEnumerable<rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con);
        Task<IEnumerable<rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con, int? DepartmentID);
       // Task<IEnumerable<rptGetResdencesExpiredVM>> GetResDencesExpired(string Con);
        Task<IEnumerable<rptGetResdencesExpiredVM>> GetResDencesExpired(string Con, int? DepartmentID);
        //Task<IEnumerable<rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con);
        Task<IEnumerable<rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con, int? DepartmentID);
        // Task<IEnumerable<rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con);
        Task<IEnumerable<rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con, int? DepartmentID);
        Task<IEnumerable<EmployeesVM>> GetAllArchivesEmployees(string lang, int BranchId);
        Task<IEnumerable<EmployeesVM>> SearchArchiveEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId);
        Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmentId, string lang);
        Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmentId, string lang, string? Searchtext);


        IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, int YearId, string Con = "");
    }
}
