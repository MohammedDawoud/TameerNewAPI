using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class StatisticsVM
    {
        public int? ReceiptVoucherCount { get; set; }
        public int? ExchangeVoucherCount { get; set; }
        public int? DailyVoucherCount { get; set; }
        public int? OpeningVoucherCount { get; set; }
        public int? OpeningBalanceCount { get; set; }
        public int? CitizensCount { get; set; }
        public int? InvestorCompanyCount { get; set; }
        public int? GovernmentSideCount { get; set; }
        public int? ProjectAreaCount { get; set; }
        public int? ProjectWorkOrdersCount { get; set; }
        public int? ProjectGovernmentCount { get; set; }
        public int? ProjectDesignCount { get; set; }
        public int? ProjectPlanningCount { get; set; }
        public int? ProjectSupervisionCount { get; set; }
        public int? ExpensesCount { get; set; }
        public int? RevenuesCount { get; set; }
        public decimal? ExpensesAmount { get; set; }
        public decimal? RevenuesAmount { get; set; }
        public int? NotificationCount { get; set; }
        public int? AlertCount { get; set; }
        public int? UnderCollectionCount { get; set; }
        public int? ExportsCount { get; set; }
        public int? InvoicesAndServicesCount { get; set; }
        public int? CustomersCount { get; set; }
        public int? AllProjectCount { get; set; }
        public int? AllUsersCount { get; set; }
        public int? AllVouchersCount { get; set; }
        public int? OutboxCount { get; set; }
        public int? InboxCount { get; set; }
        public int? OfficialDocumentsCount { get; set; }
        public int? VacationsCount { get; set; }
        public int? NationalityIdCount { get; set; }
        public int? PassportNoCount { get; set; }
        public decimal? NotStartedTasksCount { get; set; }
        public decimal? InProgressTasksCount { get; set; }
        public decimal? FinishedTasksCount { get; set; }
        public int? LateTasksCount { get; set; }

        public decimal? NotStartedTasksCountPrecnt { get; set; }
        public decimal? InProgressTasksCountPrecnt { get; set; }
        public decimal? FinishedTasksCountPrecnt { get; set; }
        public decimal? LateTasksCountPrecnt { get; set; }

        public decimal? NotStartedWorkOrdersCount { get; set; }
        public decimal? InProgressWorkOrdersCount { get; set; }
        public decimal? FinishedWorkOrdersCount { get; set; }
        public decimal? LateWorkOrdersCount { get; set; }

        public decimal? TotalLateCount { get; set; }
        public decimal? TotalInProressCount { get; set; }

        public int? UserNewTaskCount { get; set; }
        public int? UserLateWorkOrderCount { get; set; }

        public int? NewTasksCountByUserId { get; set; }
        public int? LateTasksCountByUserId { get; set; }
        public int? NewWorkOrdersCountByUserId { get; set; }
        public decimal? TasksPercentByUserIdAndProjectId { get; set; }
        public decimal? WorkOrdersPercentByUserIdAndProjectId { get; set; }
        public decimal? ProjectsPercentByUserIdAndProjectId { get; set; }
        public int? UserProjectsCount { get; set; }
        public int? UserMailsCount { get; set; }
        public int? UserTasksCount { get; set; }
        public int? UserWorkOrdersCount { get; set; }
        public int? UserTotalCount { get; set; }
        public decimal? ProjectCountByStatus { get; set; }
        public List<ProjectVM>? Projects { get; set; }

        public string? Custodies { get; set; }
    }
}
