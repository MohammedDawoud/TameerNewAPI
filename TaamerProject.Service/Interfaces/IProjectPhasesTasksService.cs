using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectPhasesTasksService 
    {
        Task<ProjectPhasesTasksVM> GetProjectPhasesTasksbygoalandproject(int projectid, int progectgoal, int BranchId, string lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesAndTasks(int projectId, string lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks(string SearchText, int BranchId, string lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_WithB(string SearchText, int BranchId, string lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksU(int UserId, int BranchId, string lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksUPage(int UserId, int BranchId, string lang);

        // IEnumerable<ProjectPhasesTasksVM> GetAllProjectPhasesTasksS(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo);


        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksS(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, string? Searchtext);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksW(int BranchId, string lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksPhasesByProjectId(int ProjectId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasks(string EndDateP, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetTasksWithoutUser(int? DepartmentId,int BranchId, string Lang);

        IEnumerable<TasksLoadVM> GetAllNewProjectPhasesTasksd(string EndDateP, int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllNewProjectPhasesTasksByUserId(string EndDateP, int BranchId, int USERID);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasks(string EndDateP, int BranchId);
        IEnumerable<TasksLoadVM> GetAllLateProjectPhasesTasksd(string EndDateP, int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId(string EndDateP, int BranchId, int UserId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksByUserId(string SearchText, int? UserId, int BranchId);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks2(string SearchText, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetProjectPhasesTasks2(int ProjectId, int BranchId, string Lang);
        Task<IEnumerable<TasksLoadVM>> GetProjectPhasesTasks2Tree(int ProjectId,string SearchText, int BranchId, string Lang);

        
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks(string SearchText, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome(string SearchText, int BranchId, string Lang);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasksHome_Search(ProjectPhasesTasksVM Search, string Lang);
        Task<decimal?> GetProjectPhasesTasksCountByStatus(int? UserId, int Status, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByUserId(int? UserId, int Status, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByUserIdHome(int? UserId, int Status, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByUserIdUser(int? UserId, string lang, int Status, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetLateTasksByUserId(int? UserId, int Status, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetLateTasksByUserIdHome(string EndDateP, int? UserId, int BranchId, string Lang);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetLateTasksByUserIdCustomerIdProjectId(int? UserId, int? CustomerId, int? ProjectId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetLateTasksByUserIdHome_Search(ProjectPhasesTasksVM Search, string Lang);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetDayWeekMonth_Tasks(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByDate(string StartDate, string EndDate, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByDateByUserId(string StartDate, string EndDate, int? UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetNewTasksByUserId(string EndDateP, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetNewTasksByUserIdSearchProj(string EndDateP, int? ProjectId, int? UserId, int BranchId, string Lang, bool? AllStatusExptEnd = null);

      Task<IEnumerable<ProjectPhasesTasksVM>>GetLateTasksByUserId(string EndDateP, int? UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByUserIdAndProjectId(string EndDate, int? UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByStatus(int? StatusId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetTasksByProjectNo(string ProjectNo, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllTasksSearch(ProjectPhasesTasksVM TasksSearch, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetProjectMainPhasesByProjectId(int ProjectId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetProjectSubPhasesByProjectId(int MainPhaseId);
        //GeneralMessage SaveProjectPhasesTasks(Project Project, int UserId, int BranchId);
        GeneralMessage SaveProjectPhasesTasks(Project Project, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveProjectPhasesTasksPart1(Project Project, int UserId, int BranchId, string Url, string ImgUrl); 
        GeneralMessage SaveProjectPhasesTasksPart2(Project Project, int UserId, int BranchId, string Url, string ImgUrl); 
        GeneralMessage SaveProjectPhasesTasksPart3(Project Project, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveProjectPhasesTasksNew(Project Project, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveProjectPhasesTasksNewPart1(Project Project, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveProjectPhasesTasksNewPart2(Project Project, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveProjectPhasesTasksNewPart3(Project Project, int UserId, int BranchId, string Url, string ImgUrl);

        GeneralMessage UpdateProjectPhasesTasks(Project Project, int UserId, int BranchId);
        GeneralMessage UpdateProjectPhasesTasksNew(Project Project, int UserId, int BranchId);

        GeneralMessage SaveNewProjectPhasesTasks(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage SaveMainPhases_P(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage SavefourMainPhases_P(int projectid, int UserId, int BranchId);

        GeneralMessage SaveSubPhases_P(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, int ParentId);
        int SaveNewProjectPhasesTasks2(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        //GeneralMessage SaveNewProjectPhasesTasks_E(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage SaveNewProjectPhasesTasks_E(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl);

        GeneralMessage SaveTaskSetting(List<ProjectPhasesTasks> ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage DeleteProjectPhasesTasks(int PhaseTaskId, int UserId, int BranchId);
        GeneralMessage DeleteProjectPhasesTasksNEW(int PhaseTaskId, int UserId, int BranchId);

        GeneralMessage VoucherTaskStop(int PhaseTaskId, int UserId, int BranchId);
        GeneralMessage ShowmanagerapprovalTask(int PhaseTaskId, int UserId, int BranchId);

        GeneralMessage AcceptmanagerapprovalTask(int PhaseTaskId, int UserId, int BranchId);
        GeneralMessage VoucherTaskStopR(int PhaseTaskId, int UserId, int BranchId);

        GeneralMessage ConditionTaskStopR(int PhaseTaskId, int UserId, int BranchId);
        GeneralMessage TransferTask(int PhaseTaskId, int MainSelectId, int SubSelectId, int UserId, int BranchId);


        // GeneralMessage ConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage ConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveTaskLongDesc(int ProjectPhaseTasksId, string taskLongDesc, int UserId, int BranchId);
        GeneralMessage ConvertUserTasks(int FromUserId, int ToUserId, int UserId, int BranchId);
        GeneralMessage ConvertUserTasksSome(int PhasesTaskId, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage ConvertMoreUserTasks(List<int> PhasesTaskIds, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl);

        //GeneralMessage RequestConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage RequestConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl);
        GeneralMessage SetUserTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, string Lang, int BranchId);
        GeneralMessage PlayPauseTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, string Lang, int BranchId);
        //GeneralMessage ChangeTaskTime(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage ChangeTaskTime(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl);
        //GeneralMessage FinishTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage FinishTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl);
        GeneralMessage FinishTaskManager(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl);

        GeneralMessage FinishTaskCheck(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string URL, string ImgUrl);

        GeneralMessage ChangePriorityTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);

        //GeneralMessage PlustimeTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId);
        GeneralMessage PlustimeTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage RefusePlustimeTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage RefuseConvertTask(ProjectPhasesTasks ProjectPhasesTasks, int UserId, int BranchId, string Url, string ImgUrl);

        Task<ProjectPhasesTasksVM> GetTaskById(int TaskId, string Lang,int? UserId);
        int? GetProjectNoById(int? TaskId, string Lang);

        Task<ProjectPhasesTasksVM> GetTaskByUserId(int TaskId, int UserId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllSubPhasesTasks(int SubPhaseId, string Lang);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllSubPhasesTasksbyUserId(int SubPhaseId, int UserId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllSubPhasesTasksbyUserId2(int SubPhaseId, int UserId);

        IEnumerable<object> FillProjectMainPhases(int ProjectId);
        IEnumerable<object> FillUsersTasksVacationSelect(int UserId);
        IEnumerable<object> FillUsershaveRunningTasks();
        IEnumerable<object> FillUsershaveRunningTasks(int BranchId);
        IEnumerable<object> FillUsershaveRunningTasksWithBranch(int BranchId);

        IEnumerable<object> FillProjectSubPhases(int MainPhaseId);
       Task<int> GetUserTaskCount(int? UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetInprogressTasksByUserId(int UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllProjectCurrentTasks(int ProjectId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllTasksByProjectId(int ProjectId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllTasksByProjectIdS(int? ProjectId, int? DepartmentId, string DateFrom, string DateTo, int BranchId, string? SearchText);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllTasksByProjectIdW2(string DateFrom, string DateTo, int BranchId);

      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllTasksByProjectIdW(int BranchId);
        Task<IEnumerable<rptGetEmpDoneTasksVM>> GetDoneTasksDGV(string FromDate, string ToDate, int UserID, string Con);
        Task<IEnumerable<rptGetEmpUndergoingTasksVM>> GetUndergoingTasksDGV(string FromDate, string ToDate, int UserID, string Con);
        Task<IEnumerable<rptGetEmpDelayedTasksVM>> GetEmpDelayedTasksDGV(string FromDate, string ToDate, int UserID, string Con);

        Task<IEnumerable<rptGetDoneWorkOrdersByExecEmp>> GetEmpDoneWOsDGV(int UserID, string Con);

        Task<IEnumerable<rptGetOnGoingWorkOrdersByExecEmp>> GetEmpUnderGoingWOsDGV(int UserID, string Con);

        Task<IEnumerable<rptGetDelayedWorkOrdersByExecEmpVM>> GetEmpDelayedWOsDGV(int UserID, string Con);

        int GetAllTasksBySubPhase(int SubPhases);

        GeneralMessage MerigTasks(int[] TasksIdArray, string Description, string Note, int UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllProjectTasksByPhaseId(int id);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetProjectsWithProSettingVM();
      Task<IEnumerable<ProjectPhasesTasksVM>>GetProjectsWithoutProSettingVM();
        GeneralMessage RestartTask(int id, string RestartTaskReason, int UserId, int BranchId);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllProjectPhasesTasksU2(int UserId, int BranchId, string lang, string DateFrom, string DateTo);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasksbystatus(int? UserId, int BranchId, int? status, string Lang, string DateFrom, string DateTo, string? Searchtext);
        RptAllEmpPerformance getempdata(ProjectPhasesTasksVM Search, string Lang, string Con);
        List<RptAllEmpPerformance> getempdataNew(ProjectPhasesTasksVM Search, string Lang, string Con, int BranchId);
        Task<List<RptAllEmpPerformance>> getempdataNew_Proc(PerformanceReportVM Search, string Lang, string Con, int BranchId);


        // List<RptAllEmpPerformance> getempdata(ProjectPhasesTasksVM Search, string Lang, string Con);
        Task<IEnumerable<ProjectPhasesTasksVM>>GetLateTasksByUserIdrptsearch(int? UserId, int? status, string Lang, string DateFrom, string DateTo, int BranchId, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdrptsearch(int? UserId, int? status, string Lang, string DateFrom, string DateTo, int BranchId, string? SearchText);
      Task<IEnumerable<ProjectPhasesTasksVM>>GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllLateProjectPhasesTasksbyUserId2(string EndDateP, int BranchId, int? UserId, string Lang, string? SearchText);
        GeneralMessage UpdateprojectphaseRequirment(int projectphaseid, int status, int UserId, int BranchId);

        IEnumerable<AccountTreeVM> GetAllNewProjectPhasesTasksTree(string EndDateP, int BranchId);

        IEnumerable<TasksLoadVM> GetTasksByUserIdCustomerIdProjectIdTree(int? UserId, int? CustomerId, int? ProjectId,int? BrancId, string Lang);
        IEnumerable<TasksLoadVM> GetLateTasksByUserIdCustomerIdProjectIdTree(int? UserId, int? CustomerId, int? ProjectId,int? BrancId, string Lang);
        GeneralMessage AskForMoreDetails(int ProjectPhasesTaskid, int askdetai, int UserId, int BranchId);
        GeneralMessage UpdateIsNew(int TaskId, int UserId, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang, int? ProjectId, int? CustomerId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetNewTasksByUserId2(int? UserId, string Lang, int? ProjectId, int? CustomerId, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang, int? ProjectId, int? CustomerId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetLateTasksByUserIdHomefilterd(int? UserId, string Lang, int? ProjectId, int? CustomerId, string? Searchtext);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang, int? CustomerId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetInProgressProjectPhasesTasks_Branches(int BranchId, string Lang, int? CustomerId, string? SearchText);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetAllProjectPhasesTasks_Costs(int? UserId, int BranchId, string Lang, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int PhasesTaskId);
        GeneralMessage SaveTaskOperations(Pro_TaskOperations TaskOperations, int UserId, int BranchId);
        Task<string> GenerateNextTaskNumber(int BranchId,int? ProjectId);
        Task<string> GetTaskCode_S(int BranchId, int? ProjectId);
    }
}
