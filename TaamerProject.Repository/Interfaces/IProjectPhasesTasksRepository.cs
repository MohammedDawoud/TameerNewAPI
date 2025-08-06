using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IProjectPhasesTasksRepository :IRepository<ProjectPhasesTasks>
    {
        Task<ProjectPhasesTasksVM> GetProjectPhasesTasksbygoalandproject(int projectid, int projectgoal, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectPhasesTasksbygoalandproject2(int projectid, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks(string SearchText, int BranchId,string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesAndTasks(int projectId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_WithB(string SearchText, int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksUPage(int UserId, int BranchId, string Lang);

        //IEnumerable<ProjectPhasesTasksVM>  GetAllProjectPhasesTasksS(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksW(int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksPhasesByProjectId(int ProjectId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasks(string EndDateP, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksWithoutUser(int? DepartmentId, int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasksByUserId(string EndDateP, int BranchId, int USERID);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasks(string EndDateP, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId(string EndDateP, int BranchId,int UserId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksByUserId(string SearchText, int? UserId, int BranchId);

        
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks2(string SearchText, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectPhasesTasks2(int ProjectId, int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks(string SearchText, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome(string SearchText, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome_Search(ProjectPhasesTasksVM Search, string Lang);
        Task<decimal?> GetProjectPhasesTasksCountByStatus(int? UserId, int Status, int BranchId);
        Task<decimal?> GetProjectPhasesTasksCountByStatusPercent(int? UserId, int Status, int BranchId);
        Task<decimal?> GetLateTasksByUserIdCount(string EndDateP, int? UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserId(int? UserId, int Status, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdHome(int? UserId, int Status, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdUser(int? UserId, string lang, int Status, int BranchId);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserId(int? UserId, int Status, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHome(string EndDateP, int? UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHome_Search(ProjectPhasesTasksVM Search, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetDayWeekMonth_Tasks(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByDate(string StartDate, string EndDate, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByDateByUserId(string StartDate, string EndDate, int? UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId(string EndDateP, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserIdSearchProj(string EndDateP, int? ProjectId, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null);

        Task<int> GetNewTasksCountByUserId(string EndDateP, int? UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserId(string EndDateP, int? UserId, int BranchId);
        Task<int> GetLateTasksCountByUserId(string EndDateP, int? UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdAndProjectId(string EndDate, int? UserId, int BranchId);
        Task<decimal> GetTasksPercentByUserIdAndProjectId(int? UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByStatus(int? StatusId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByProjectNo(string ProjectNo, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksSearch(ProjectPhasesTasksVM TasksSearch, int BranchId);
        Task<ProjectPhasesTasksVM> GetProjectPhasesTaskByPerdecessorId(int? PerdecessorId, int BranchId);
        Task<ProjectPhasesTasksVM> GetProjectPhasesTaskByParentId(int? ParentId, int BranchId);

       // ProjectPhasesTasksVM GetProjectPhasesTaskByProjectId(int ProjectId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectMainPhasesByProjectId(int ProjectId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectSubPhasesByProjectId(int MainPhaseId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasks(int SubPhaseId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasksbyUserId(int SubPhaseId, int UserId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllSubPhasesTasksbyUserId2(int SubPhaseId, int UserId);

        Task<ProjectPhasesTasksVM> GetTaskById(int TaskId, string Lang,int? UserId);
        Task<ProjectPhasesTasksVM> GetTaskByUserId(int TaskId, int UserId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectId(int ProjectId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdWithoutBranch(int ProjectId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdWithoutBranchNew(int ProjectId, int BranchId);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdW(int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdW2(string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllPhasesTasksByProjectId(int ProjectId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdForFinish(int ProjectId);
        Task<int> GetUserTaskCount(int? UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInprogressTasksByUserId(int UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectCurrentTasks(int ProjectId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectTasksByPhaseId(int id);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectsWithProSettingVM();
        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectsWithoutProSettingVM();


        Task<IEnumerable<rptGetEmpDoneTasksVM>> GetDoneTasksDGV(string FromDate, string ToDate, int UserID, string Con);
        Task<IEnumerable<rptGetEmpUndergoingTasksVM>> GetUndergoingTasksDGV(string FromDate, string ToDate, int UserID, string Con);
        Task<IEnumerable<rptGetEmpDelayedTasksVM>> GetEmpDelayedTasksDGV(string FromDate, string ToDate, int UserID, string Con);
        Task<IEnumerable<rptGetDoneWorkOrdersByExecEmp>> GetEmpDoneWOsDGV( int UserID, string Con);

        Task<IEnumerable<rptGetOnGoingWorkOrdersByExecEmp>> GetEmpUnderGoingWOsDGV(int UserID, string Con);

        Task<IEnumerable<rptGetDelayedWorkOrdersByExecEmpVM>> GetEmpDelayedWOsDGV(int UserID, string Con);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU2(int UserId, int BranchId, string Lang, string DateFrom, string DateTo);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksReport(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAlmostLateTasksByUserId(int userid, string startdate, string enddate,int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdreport(string startdate, string EndDateP, int? UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserIdReport(string startdate, string EndDateP, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string DateFrom, string DateTo, string Lang, int BranchId, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string DateFrom, string DateTo, string Lang, int BranchId, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksINprogressByUserIdUser(int? UserId, string lang, int Status, int BranchId, string startdate, string enddate);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasksbyprojectid(string EndDateP, int BranchId, int projectid);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTaskswithtime(string EndDateP, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTaskswithtime(string EndDateP, int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksByUserIdCustomerIdProjectIdwithtime(int? UserId, int? CustomerId, int? ProjectId, int? BrancId,string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdCustomerIdProjectIdwithtime(int? UserId, int? CustomerId, int? ProjectId, int? BrancId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang, int? ProjectId, int? CustomerId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang, int? ProjectId, int? CustomerId, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang, int? ProjectId, int? CustomerId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang, int? ProjectId, int? CustomerId, string? Searchtext);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branchesfilterd(int BranchId, string Lang, int? CustomerId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branchesfilterd(int BranchId, string Lang, int? CustomerId, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_Costs(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo, List<int> BranchesList);
        Task<int> GenerateNextTaskNumber(int BranchId, string codePrefix,int? ProjectId);
        Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int PhasesTaskId);


    }
}
