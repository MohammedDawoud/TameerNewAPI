using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IWorkOrdersService 
    {
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrders(int BranchId, int UserId);
        //IEnumerable<WorkOrdersVM> GetAllProjectSubsByProjectTypeId(int ProjectTypeId, string SearchText, int BranchId);
        GeneralMessage SaveWorkOrders(WorkOrders WorkOrders, int UserId, int BranchId);
        GeneralMessage EditWorkOrdersFile(WorkOrders WorkOrders, int UserId, int BranchId);

        GeneralMessage DeleteWorkOrders(int WorkOrderId, int UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> SearchWorkOrders(WorkOrdersVM WorkOrdersSearch, string lang, int BranchId);
        Task<int> GetMaxOrderNumber(int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByDateSearch(string DateFrom, string DateTo, int BranchId, int UserId);
        Task<WorkOrdersVM> GetWorkOrderById(int WorkOrderId, string lang);
        Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId);
        Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId, string? Searchtext);
        Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId);
        Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? customerid, int? ProjectId, string? Searchtext);
        GeneralMessage FinishOrder(WorkOrders workOrders, int UserId, int BranchId);

        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserId( int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask(string task,int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask2(string task, int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetDayWeekMonth_Orders(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersyProjectId(int ProjectId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int Status, int BranchId, string Lang, string StartDate, string EndDate, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int Status, int BranchId, string Lang, string StartDate, string EndDate, string? Searchtext);
        Task<List<ProjectPhasesTasksVM>> GetWorkOrderReport_print(int? UserId, int Status, int BranchId, string Lang, string StartDate, string EndDate);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetALlWorkOrderReport(string Lang, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(int? projectid, string Lang, string StartDate, string EndDate, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(int? projectid, string Lang, string StartDate, string EndDate, int BranchId, string? SeachText);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId, int UserId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersFilterd(int BranchId, int UserId, int? CustomerId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersFilterd(int BranchId, int UserId, int? CustomerId, string? SearchText);

        GeneralMessage ConvertUserTasksSome(int WorkOrderId, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage RequestConvertTask(WorkOrders WorkOrders, int UserId, int BranchId, string URL, string ImgUrl);
        GeneralMessage PlayPauseTask(WorkOrders WorkOrders, int UserId, string Lang, int BranchId);
        GeneralMessage ChangeTaskTime(WorkOrders WorkOrders, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage ConvertTask(WorkOrders WorkOrders, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage SaveTaskLongDesc(int WorkOrderId, string taskLongDesc, int UserId, int BranchId);

        GeneralMessage PlustimeTask(WorkOrders WorkOrders, int UserId, int BranchId, string Url, string ImgUrl);
        GeneralMessage RefusePlustimeTask(WorkOrders WorkOrders, int UserId, int BranchId);

        Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int WorkOrderId);
        GeneralMessage SaveTaskOperations(Pro_TaskOperations TaskOperations, int UserId, int BranchId);
        Task<string> GenerateNextOrderNumber(int BranchId, int? ProjectId);
        Task<string> GetOrderCode_S(int BranchId, int? ProjectId);


    }
}
