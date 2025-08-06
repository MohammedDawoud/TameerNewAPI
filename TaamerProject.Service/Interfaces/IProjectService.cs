using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.ViewModels;
using TaamerProject.Models.Enums;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectVM>> GetAllProject(string Lang, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllProjectCustomerBranch(string Lang, int BranchId);

       Task<IEnumerable<ProjectVM>>GetAllProjectsStatusTasks(string Lang, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllProjectWithout(string Lang, int BranchId);

       Task<IEnumerable<ProjectVM>>GetAllProjectAllBranches();
       Task<IEnumerable<ProjectVM>>GetAllProjectAllBranches2();
        GeneralMessage BarcodePDF(int FileID, int UserId);

       Task<IEnumerable<ProjectVM>>GetAllProjectNumber(string SearchText, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllArchiveProject(int BranchId);
        //GeneralMessage SaveProject(Project project,int UserId, int BranchId);

        GeneralMessage SaveProject(Project project, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage PostProjectsCheckBox(List<string> ProjectIds, string MangerId, int BranchId, int? yearid);
        GeneralMessage UpdateProject(Project project, int UserId, int BranchId);

        GeneralMessage DeleteProject(int projectId, int UserId, int BranchId);
        GeneralMessage SendCustomerEmail_SMS(int CustomerId, int ProjectId, int TypeId, int UserId, int BranchId);

        GeneralMessage DeleteProjectNEW(int projectId, int UserId, int BranchId);
        GeneralMessage DeleteAllProject_NEW(int projectId, int UserId, int BranchId);
        GeneralMessage DeleteAllProject_NEWWithVouchers(int projectId,string password, int UserId, int BranchId);


        //GeneralMessage StopProject(int projectId, int UserId, int BranchId);
        GeneralMessage DestinationsUploadProject(int projectId, int status, int UserId, int BranchId);
        GeneralMessage StopProject(int projectId, int UserId, int BranchId, string Url, string ImgUrl, int TypeId, int whichClickDesc);
        //GeneralMessage PlayProject(int projectId, int UserId, int BranchId);
        GeneralMessage PlayProject(int projectId, int UserId, int BranchId, string Url, string ImgUrl, int TypeId, int whichClickDesc);
        Task<ProjectVM> GetProjectById(string Lang, int ProjectId);
       Task<ProjectVM>GetProjectByIdSome(string Lang, int ProjectId);

       Task<ProjectVM>GetCostCenterByProId(string Lang, int ProjectId);
        Project GetProjectByOfferId(string OfferId);

       Task<ProjectVM>GetProjectDataOffice(string Lang, int ProjectId);

       Task<ProjectVM>GetProjectByIdStopType(string Lang, int ProjectId);

       Task<ProjectVM>GetProjectAddUser(int ProjectId);
        int GetTypeOfProjct(int projectId);
        GeneralMessage SaveProjectDetails(ProjectVM project, int UserId, int BranchId);
       Task<IEnumerable<ProjectVM>>GetProjectsByCustomerId(int? CustomerId, int? Status);
       Task<IEnumerable<ProjectVM>>GetSomeDataByProjId(int? ProjectId);

       Task<IEnumerable<ProjectVM>>GetUserProjects(int UserId, int BranchId, string DateNow);
       Task<IEnumerable<ProjectVM>>GetUserProjects2(int UserId, int BranchId, string DateNow);
       Task<IEnumerable<ProjectVM>>GetUserProjectsReport(int UserId, int BranchId, string DateNow);
       Task<IEnumerable<ProjectVM>>GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, string? Searchtext);
       Task<IEnumerable<ProjectVM>>GetUserProjectsReportW(int BranchId, string DateNow);
       Task<IEnumerable<ProjectVM>>GetProjectsSearch(ProjectVM ProjectsSearch, int BranchId, string Con, string lang);
       Task<IEnumerable<ProjectVM>>GetProjectsStatusTasksSearch(ProjectVM ProjectsSearch, int BranchId, string Con, string lang);
        Task<IEnumerable<object>> FillAllUsersByProject(int BranchId, string lang);
       Task<IEnumerable<ProjectVM>>GetAllProjectsByDateSearch(string DateFrom, string DateTo, int BranchId);
       Task<IEnumerable<ProjectVM>>GetArchiveProjectsSearch(ProjectVM ProjectsSearch, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllArchiveProjectsByDateSearch(string DateFrom, string DateTo, int BranchId);

        // GeneralMessage FinishProject(int projectId, string reason,  int Reasontype, string Reasontext, string Date, int UserId,string Con,int BranchId);
        GeneralMessage FinishProject(int projectId, int ReasonsId, string reason, int Reasontype, string Reasontext, string Date, int UserId, string Con, int BranchId, string Url, string ImgUrl, int TypeId, int whichClickDesc);
        Task<string> GenerateNextProjectNumber(int BranchId);
        Task<string> GetProjectCode_S(int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllHirearchialProject(int BranchId, int UserId);
        //GeneralMessage UpdateStatusProject(int projectId, int UserId, int BranchId);
        GeneralMessage UpdateStatusProject(int projectId, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage ConvertManagerProjectsSome(int projectId, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage ConvertMoreManagerProjects(List<int> projectIds, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl);

        IEnumerable<object> GetAllWOStatuses(string Con);
       Task<IEnumerable<ProjectVM>>GetAllProjByCustId(string lang, int BranchId, int? custID);
       Task<IEnumerable<ProjectVM>>GetAllProjByCustIdWithout(string lang, int BranchId, int? custID);

       Task<IEnumerable<ProjectVM>>GetAllProjByBranch(string lang, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllProjByBranchWithout(string lang, int BranchId);

       Task<IEnumerable<ProjectVM>>GetAllProjByCustomerId(int customerId);
       Task<IEnumerable<ProjectVM>>GetAllProjByCustomerIdWithout(int customerId);

       Task<IEnumerable<ProjectVM>>GetAllProjByCustomerIdHaveTasks(int customerId);
        Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdandbranchHaveTasks(int customerId, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllProjByFawater(string lang, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllProjByMrdod(string lang, int BranchId);
       Task<IEnumerable<ProjectVM>>GetAllProjByNoti(string lang, int BranchId, int? yearid);

       Task<IEnumerable<ProjectVM>>GetAllProjByMrdod_Pur(string lang, int BranchId);

       Task<IEnumerable<ProjectVM>>GetAllProjByMrdod_C(string lang, int BranchId, int? CustomerId);
       Task<IEnumerable<ProjectVM>>GetAllProjByNoti_C(string lang, int BranchId, int? CustomerId, int? yearid);

       Task<IEnumerable<ProjectVM>>GetAllProjByMrdod_C_Pur(string lang, int BranchId, int? SupplierId);


        IEnumerable<CustomerVM> GetAllCustByFawater(string lang, int BranchId);
        IEnumerable<CustomerVM> GetAllCustByMrdod(string lang, int BranchId);
        IEnumerable<CustomerVM> GetAllCustByNoti(string lang, int BranchId, int? yearid);

        IEnumerable<CustomerVM> GetAllCustByReVoucher(string lang, int BranchId);
        Task<List<ProjectVMNewCounting>> GetProjectVMNew(string Lang, string Con, int BranchId);
        Task<List<ProjectVMNewStat>> GetProjectVMStatNew(int ProjectId,string Lang, string Con, int BranchId);

        Task<List<ProjectVMPhasesDetails>> GetPhasesDetails(string Lang, string Con, int ProjectId);

        Task<IEnumerable<ProjectVM>>GetProjectsStoppedVM();
        Task<IEnumerable<ProjectVM>> GetdestinationsUploadVM();

        Task<IEnumerable<ProjectVM>>GetProjectsWithoutContractVM();
       Task<IEnumerable<ProjectVM>>GetProjectsLateVM();
       Task<IEnumerable<ProjectVM>>GetMaxCosEManagerName();
        IEnumerable<object> GetMaxCosEManagerName_StatmentTOP1(string Con, string SelectStetment);
        IEnumerable<object> GetMaxCosECustomerName_StatmentTOP1(string Con, string SelectStetment);

        IEnumerable<object> GetMaxCosEManagerName_Statment(string Con, string SelectStetment);
        IEnumerable<object> GetViewDetailsGrid(string Con, string SelectStetment);
        GeneralMessage checkprojectpercentage(int branchid, string date, int phasetakid);
       Task<IEnumerable<ProjectVM>>GetUserProjectsReportW2(int BranchId, string DateFrom, string DateTo);

       Task<IEnumerable<ProjectVM>>GetAlmostFinish(int userid, int? branchid);
        //GeneralMessage UpdateImportant(int projectId, int important, int UserId, int BranchId);

       Task<IEnumerable<ProjectVM>>GetAllProjects2(string Lang, int BranchId, int UserId);
       Task<IEnumerable<ProjectVM>>GetAllProjects3(string con, string Lang, int BranchId, int UserId);

       Task<IEnumerable<ProjectVM>>GetAllProjectsmartfollow(string Lang, int BranchId, int UserId);

        GeneralMessage UpdateProjectEnddate(int projectid, string Enddate, int UserId, int BranchId);

        GeneralMessage Updateskiptime(int projectId, int UserId, int BranchId);
        GeneralMessage UpdateProjectnoSpaces( int UserId, int BranchId);

        Task<IEnumerable<ProjectVM>>GetAllProjectsmartfollowforadmin(string Lang, int BranchId);
        Task<List<ProjectVM>> GetAllProjectsNew(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int FilterType, int? BranchId);
        Task<List<ProjectVM>> GetAllProjectsNew_DashBoard(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int? BranchId);
        Task<IEnumerable<ProjectVM>> GetprojectNewTasks(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectVM>> GetprojectLateTasks(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang);
        Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang, string EndDate);
        rptProjectStatus GetTaskData(int projectId, string con);
        GeneralMessage SaveProjectLocation(ProjectLocationVM locationVM);
        ProjectLocationVM GetProjectLocation(int ProjectId);
        GeneralMessage deleteProjectLocation(int projectId);

        rptProjectStatus_phases GetTaskData_phases(int projectId, string con);
        (List<int> Users, string Description) GetNotificationRecipients(NotificationCode code, int projectId);
    }
}
