using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IWorkOrdersRepository
    {
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersForAdmin(int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrders(int BranchId);
        Task<IEnumerable<WorkOrdersVM>> SearchWorkOrders(WorkOrdersVM WorkOrdersSearch, string lang, int BranchId);
        Task<int> GetMaxOrderNumber();
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByDateSearch(string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByDateSearchForAdmin(string DateFrom, string DateTo, int BranchId);
        Task<decimal?> GetWorkOrderCountByStatus(int? UserId, int Status, int BranchId);
        Task<decimal> GetLateWorkOrdersCount(string EndDateP, int? UserId, int BranchId);
        Task<WorkOrdersVM> GetWorkOrderById(int WorkOrderId, string lang);
        Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserId(string EndDateP, int? UserId, int BranchId);

        Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId);
        Task<IEnumerable<WorkOrdersVM>> GetNewWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId, string? SearchText);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserId(int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask(string task,int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersByUserIdandtask2(string task, int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetDayWeekMonth_Orders(int? UserId, int Status, int BranchId, int Flag, string StartDate, string EndDate);
        Task<int> GetLateWorkOrdersCountByUserId(string EndDateP, int? UserId, int BranchId);
        Task<decimal> GetWorkOrdersPercentByUserIdAndProjectId(int? UserId, int BranchId);
        Task<int?> GetUserWorkOrderCount(int? UserId, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersyProjectId(int ProjectId);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int BranchId, string lang, int status, string DateFrom, string DateTo, List<int> BranchesList);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport(int? UserId, int BranchId, string lang, int status, string DateFrom, string DateTo, string? SearchText);
        Task<List<ProjectPhasesTasksVM>> GetWorkOrderReport_print(int? UserId, int BranchId, string lang, int status, string DateFrom, string DateTo);

        Task<IEnumerable<ProjectPhasesTasksVM>> GetALlWorkOrderReport(string lang, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(string lang, int? ProjectId, string DateFrom, string DateTo, int BranchId);
        Task<IEnumerable<ProjectPhasesTasksVM>> GetWorkOrderReport_ptoject(string lang, int? ProjectId, string DateFrom, string DateTo, int BranchId, string? SearchText);

        Task<List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_NotStarted(ProjectPhasesTasksVM Search, string lang);
        Task<List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_Inptogress(ProjectPhasesTasksVM Search, string lang);
        Task<List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_Finished(ProjectPhasesTasksVM Search, string lang);
        Task<List<ProjectPhasesTasksVM>> GetALlWorkOrderReport_Late(ProjectPhasesTasksVM Search, string Today);

        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId);
        Task<IEnumerable<WorkOrdersVM>> GetWorkOrdersBytask(string task, int BranchId, int UserId);

        Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId);
        Task<IEnumerable<WorkOrdersVM>> GetLateWorkOrdersByUserIdFilterd(string EndDateP, int? UserId, int BranchId, int? CustomerId, int? ProjectId, string? SearchText);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersForAdminByCustomer(int BranchId, int? CustomerId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersForAdminByCustomer(int BranchId, int? CustomerId, string? SearchText);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByCustomer(int BranchId, int? CustomerId);
        Task<IEnumerable<WorkOrdersVM>> GetAllWorkOrdersByCustomer(int BranchId, int? CustomerId, string? SearchText);
        Task<int> GenerateNextOrderNumber(int BranchId, string codePrefix, int? ProjectId);
        Task<IEnumerable<Pro_TaskOperationsVM>> GetTaskOperationsByTaskId(int PhasesTaskId);
    }
}
