using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.Enums;

namespace TaamerProject.Service.Interfaces
{
    public interface IEmployeesService
    {
        Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int BranchId);
        Task<IEnumerable<EmpLocationsVM>> GetAllEmployeesByLocationId(string lang, int LocationId);

        string GetEmployeeJobName(int EmpId, string lang, int BranchId);
        Job GetEmployeeJob(int EmpId, string lang, int BranchId);

        Task<IEnumerable<EmployeesVM>> GetAllBranchEmployees(string lang);
        Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int SearchAll, int BranchId);
        Task<IEnumerable<EmployeesVM>> GetAllEmployeesSearch(EmployeesVM SalarySearch, string lang, int UserId, int BranchId, string Con = "");
        IEnumerable<EmployeesVM> GetAllEmployeesSearch(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, int YearId, string Con);
        IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, string Con = "");
        IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, string Con);
        IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, int YearId, string Con);
        IEnumerable<EmployeesVM> GetEmployeesForPayrollPrint(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, int YearId, string Con);
        Task<IEnumerable<EmployeesVM>> GetEmployeeByUserid(int UserId);
        GeneralMessage SaveEmployee(Employees emp, int UserId, int BranchId);
        GeneralMessage SaveEmpBouns(int EmpId, int Bouns, int User, string Lang, int BranchId);
        GeneralMessage DeleteEmplocation(int EmpId, int LocationId, int User, string Lang, int BranchId);
        GeneralMessage AllowEmployeesites(int EmpId, bool Check, int Type, int User, string Lang, int BranchId);
        GeneralMessage AllowEmployeesitesList(List<int> EmpId, bool Check, int Type, int User, string Lang, int BranchId);


        GeneralMessage ConvertEmplocation(int EmpId, int oldLocationId, int newLocationId, int User, string Lang, int BranchId);
        GeneralMessage SaveEmplocation(int EmpId, int LocationId, int User, string Lang, int BranchId);
        GeneralMessage SaveEmplocationList(List<int> EmpList, int LocationId, int User, string Lang, int BranchId);


        GeneralMessage CheckifCodeIsExist(string empCode, int UserId, int BranchId);
        GeneralMessage SaveOfficialDocuments(Employees OffDoc, int UserId, int BranchId, string lang);
        GeneralMessage DeleteEmployee(int EmpId, int UserId, int BranchId);
        IEnumerable<NodeVM> FillEmployeeSelect(string lang, int BranchId, bool IsNewContract, int? EmpId = null);
        IEnumerable<object> FillSelectEmployee(string lang, int BranchId);
        IEnumerable<object> FillSelectEmployeeWorkers(string lang, int BranchId);

        Task<EmployeesVM> GetEmployeeById(int EmpId, string lang);
        Task<EmployeesVM> GetEmployeeById_d(int EmpId, string lang);
        Task<IEnumerable<EmployeesVM>> SearchEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId);
        Task<EmployeesVM> GetEmployeeInfo(int EmployeeId, string lang, int BranchId);

        Task<int> GenerateNextEmpNumber(int BranchId);
        IEnumerable<object> FillEmpAppraisSelect(string lang, int BranchId, int UserId);
        Object GetEmployeeStatistics();
        Task<IEnumerable<rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con);
        Task<IEnumerable<rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con, int? DepartmectId);
        Task<IEnumerable<rptGetResdencesExpiredVM>> GetResDencesExpired(string Con);
        Task<IEnumerable<rptGetResdencesExpiredVM>> GetResDencesExpired(string Con, int? DepartmectId);
        Task<IEnumerable< rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con);
        Task<IEnumerable<rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con, int? DepartmectId);
        Task<IEnumerable<rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con);
        Task<IEnumerable<rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con, int? DepartmectId);
        IEnumerable<rptGetEmpContractsAboutToExpireVM> GetEmpContractsAboutToExpire(string Con, int? DepartmentID);
        IEnumerable<rptGetEmpContractsAboutToExpireVM> GetEmpContractsExpired(string Con, int? DepartmentID);
        IEnumerable< rptGetEmpLoans> GetEmpLoans(string Con);
        IEnumerable< rptGetEmpContractsAboutToExpireVM> GetEmpContractsAboutToExpire(string Con);
        Task<IEnumerable<EmployeesVM>> GetAllArchivesEmployees(string lang, int BranchId);
        Task<IEnumerable<EmployeesVM>> SearchArchiveEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId);
        // GeneralMessage Changecompanyresponsive(int EmpId, int User, string Lang, int BranchId);
        GeneralMessage Savequacontract(int EmpId, string quacontract, int UserId, int BranchId, int yearid, string lang);
        Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmectId, string lang);
        Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmectId, string lang, string? Searchtext);
        GeneralMessage DeleteQuacontractDetails(int EmployeeId);
        GeneralMessage RemoveEmployee(int EmpId, int UserId, int BranchId);
        (List<int> Users, string Description, string mail) GetNotificationRecipients(NotificationCode code, int? EmpId);
    }
}
