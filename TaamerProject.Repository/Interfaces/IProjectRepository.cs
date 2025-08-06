using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectRepository :IRepository<Project>
    {
        Task<IEnumerable<ProjectVM>> GetAllProject(string Lang, int BranchId);
        Task<IEnumerable<ProjectVM>> GetAllProjectCustomerBranch(string Lang, int BranchId);

        Task<IEnumerable<ProjectVM>> GetAllProjectWithout(string Lang, int BranchId);

        Task<IEnumerable<ProjectVM>> GetAllProjectAllBranches();
        Task<IEnumerable<ProjectVM>> GetAllProjectAllBranches2();
        Task<IEnumerable<ProjectVM>> GetAllProjectNumber(string SearchText, int BranchId);
        Task<IEnumerable<ProjectVM>> GetAllArchiveProject(int BranchId);
        Task<ProjectVM> GetProjectById(string Lang,int ProjectId);
        Task<ProjectVM> GetProjectByIdSome(string Lang, int ProjectId);

        Task<ProjectVM> GetCostCenterByProId(string Lang, int ProjectId);

        Task<ProjectVM> GetProjectDataOffice(string Lang, int ProjectId);


        Task<ProjectVM> GetProjectByIdStopType(string Lang, int ProjectId);

        Task<ProjectVM> GetProjectAddUser(int ProjectId);
        Task<IEnumerable<ProjectVM>> GetProjectsByCustomerId(int? CustomerId, int? Status);
        Task<IEnumerable<ProjectVM>> GetSomeDataByProjId(int? ProjectId);

        Task<IEnumerable<ProjectVM>> GetUserProjects(int UserId,int BranchId, string DateNow);
        Task<IEnumerable<ProjectVM>> GetUserProjects2(int UserId, int BranchId, string DateNow);
        Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int UserId, int BranchId, string DateNow);
        Task<IEnumerable<ProjectVM>> GetUserProjectsReportW( int BranchId, string DateNow);

        Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, string? SearchText);

        Task<decimal?> GetProjectCountByStatus(int? UserId, int BranchId);
        Task<int> GetMaxId();
        Task<IEnumerable<ProjectVM>> GetProjectsSearch(ProjectVM ProjectsSearch,int BranchId,string Con, string lang);
        Task<IEnumerable<object>> FillAllUsersByProject(int BranchId, string lang);
        Task<IEnumerable<ProjectVM>> GetAllProjectsByDateSearch(string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<ProjectVM>> GetArchiveProjectsSearch(ProjectVM ProjectsSearch, int BranchId);
        Task<IEnumerable<ProjectVM>> GetAllArchiveProjectsByDateSearch(string DateFrom, string DateTo, int BranchId);
        Task<int> GetProjectAreaCount(int BranchId);
        Task<int> GetProjectWorkOrdersCount(int BranchId);
        Task<int> GetProjectGovernmentCount(int BranchId);
        Task<int> GetProjectDesignCount(int BranchId);
        Task<int> GetProjectPlanningCount(int BranchId);
        Task<int> GetProjectSupervisionCount(int BranchId);
        Task<int> GetAllProjectCount(int BranchId);
        Task<decimal> GetUserProjectsCount(int UserId, int BranchId);
        Task<int> GenerateNextProjectNumber(int BranchId,string codePrefix);
        Task<IEnumerable<ProjectVM>> GetAllHirearchialProject(int BranchId, int UserId);
        Task<decimal> GetProjectsPercentByUserIdAndProjectId(int? UserId, int BranchId);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustId(string lang, int BranchId, int? CustId);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustIdWithout(string lang, int BranchId, int? CustId);

        Task<IEnumerable<ProjectVM>> GetAllProjByBranch(string lang, int BranchId);
        Task<IEnumerable<ProjectVM>> GetAllProjByBranchWithout(string lang, int BranchId);

        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerId(int customerId);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdWithout(int customerId);

        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdHaveTasks(int customerId);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdandbranchHaveTasks(int customerId, int branchid);
        Task<IEnumerable<ProjectVM>> GetAllProjByFawater(string lang, int BranchId);
        Task<IEnumerable<ProjectVM>> GetAllProjByMrdod(string lang, int BranchId);

        Task<IEnumerable<ProjectVM>> GetProjectsStoppedVM();
        Task<List<ProjectVMNewCounting>> GetProjectVMNew(string Lang, string Con, int BranchId);
        Task<List<ProjectVMNewStat>> GetProjectVMStatNew(int ProjectId,string Lang, string Con, int BranchId);

        Task<List<ProjectVMPhasesDetails>> GetPhasesDetails(string Lang, string Con, int ProjectId);

        Task<IEnumerable<ProjectVM>> GetdestinationsUploadVM();

        Task<IEnumerable<ProjectVM>> GetProjectsWithoutContractVM();
        Task<IEnumerable<ProjectVM>> GetProjectsLateVM(string Today);
        Task<IEnumerable<ProjectVM>> GetMaxCosEManagerName();

        Task<ProjectVM> GetProjectByNUmber(string Lang, string projectnumber);
        Task<IEnumerable<ProjectVM>> GetUserProjectsReportW2(int BranchId, string DateFrom, string DateTo);
        Task<IEnumerable<ProjectVM>> GetProjectsLatereport(int userid, int branchid, string startdate, string enddate);
        Task<IEnumerable<ProjectVM>> GetProjectsStopped(int userid, int? branchid);
        Task<IEnumerable<ProjectVM>> GetProjectsInprogress(int userid, int? branchid);
        Task<IEnumerable<ProjectVM>> GetProjectsWithoutaction(int userid, int? branchid);
        Task<IEnumerable<ProjectVM>> Getprojectalmostfinish(int userid, int? branchid);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerId2(int customerId);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerId2Pro(int customerId, int ProjectId);

        Task<IEnumerable<ProjectVM>> GetAllProjects2(string Lang, int BranchId, int UserId);
        Task<IEnumerable<ProjectVM>> GetAllProjects3(string Con, string Lang, int BranchId, int UserId);

        Task<IEnumerable<ProjectVM>> GetAllProjectsmartfollow(string Lang, int BranchId, int UserId);
        Task<IEnumerable<ProjectVM>> GetAllProjectsmartfollowforadmin(string Lang, int BranchId);
        Task<List<ProjectVM>> GetAllProjectsNew(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int FilterType, int? BranchId);
        Task<List<ProjectVM>> GetAllProjectsNew_DashBoard(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int? BranchId);
        Task<IEnumerable<ProjectVM>> GetprojectNewTasks(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectVM>> GetprojectLateTasks(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang, string EndDateP);
        rptProjectStatus GetTaskData(int projectId, string con);
        ProjectLocationVM GetProjectLocation(int ProjectId);
        rptProjectStatus_phases GetTaskData_phases(int projectId, string con);
    }
}
