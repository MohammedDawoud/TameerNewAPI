using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class HomeService : IHomeService
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly IExpRevenuExpensesRepository _revenuExpensesRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IOfficialDocumentsRepository _OfficialDocumentsRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly IWorkOrdersRepository _WorkOrdersRepository;
        private readonly IUserMailsRepository _UserMailsRepository;
        private readonly IUserBranchesRepository _userbranchesRepository;
        private readonly ICustodyService _custodyService;
        private readonly IEmployeesRepository _employeesRepository;

        public HomeService(IProjectRepository projectRepository, IExpRevenuExpensesRepository expRevenuExpensesRepository, INotificationRepository notificationRepository,
            IUsersRepository usersRepository, IOfficialDocumentsRepository officialDocumentsRepository, IProjectPhasesTasksRepository projectPhasesTasksRepository,
            IWorkOrdersRepository workOrdersRepository, IUserMailsRepository userMailsRepository, IUserBranchesRepository userBranchesRepository, ICustodyService custodyService
            , IEmployeesRepository employeesRepository)
        {
            _ProjectRepository = projectRepository;
            _revenuExpensesRepository = expRevenuExpensesRepository;
            _NotificationRepository = notificationRepository;
            _UsersRepository = usersRepository;
            _ProjectPhasesTasksRepository = projectPhasesTasksRepository;
            _WorkOrdersRepository = workOrdersRepository;
            _UserMailsRepository = userMailsRepository;
            _userbranchesRepository = userBranchesRepository;
            _custodyService = custodyService;
            _employeesRepository = employeesRepository;

        }
        public StatisticsVM GetAllStatistics(int BranchId, int? yearid)
        {
            var StatisticsVM = new StatisticsVM();
            //var year = _fiscalyearsRepository.GetCurrentYear();
            //if (yearid != null)
            //{
            //    //StatisticsVM.ReceiptVoucherCount = _InvoicesRepository.GetReceiptVoucherCount(yearid ?? default(int), BranchId);
            //StatisticsVM.ExchangeVoucherCount = _InvoicesRepository.GetExchangeVoucherCount(yearid ?? default(int), BranchId);
            //StatisticsVM.DailyVoucherCount = _InvoicesRepository.GetDailyVoucherCount(yearid ?? default(int), BranchId);
            //StatisticsVM.OpeningVoucherCount = _InvoicesRepository.GetOpeningVoucherCount(yearid ?? default(int), BranchId);
            //StatisticsVM.OpeningBalanceCount = _InvoicesRepository.GetOpeningBalanceCount(yearid ?? default(int), BranchId);
            //StatisticsVM.AllVouchersCount = _InvoicesRepository.GetAllVouchersCount(yearid ?? default(int), BranchId);
            //}
            //else
            //{
            //StatisticsVM.AllVouchersCount = StatisticsVM.ReceiptVoucherCount = StatisticsVM.ExchangeVoucherCount = StatisticsVM.DailyVoucherCount = StatisticsVM.OpeningVoucherCount = StatisticsVM.OpeningBalanceCount = 0;
            //}
            //StatisticsVM.CitizensCount = _CustomerRepository.GetCitizensCount(BranchId);
            //StatisticsVM.InvestorCompanyCount = _CustomerRepository.GetInvestorCompanyCount(BranchId);
            //StatisticsVM.GovernmentSideCount = _CustomerRepository.GetGovernmentSideCount(BranchId);
            //StatisticsVM.ProjectAreaCount = _ProjectRepository.GetProjectAreaCount(BranchId);
            //StatisticsVM.ProjectWorkOrdersCount = _ProjectRepository.GetProjectWorkOrdersCount(BranchId);
            //StatisticsVM.ProjectGovernmentCount = _ProjectRepository.GetProjectGovernmentCount(BranchId);
            //StatisticsVM.ProjectDesignCount = _ProjectRepository.GetProjectDesignCount(BranchId);
            //StatisticsVM.ProjectPlanningCount = _ProjectRepository.GetProjectPlanningCount(BranchId);
            //StatisticsVM.ProjectSupervisionCount = _ProjectRepository.GetProjectSupervisionCount(BranchId);
            StatisticsVM.ExpensesCount = _revenuExpensesRepository.GetExpensesCount(BranchId);
            StatisticsVM.RevenuesCount = _revenuExpensesRepository.GetRevenuesCount(BranchId);
            //StatisticsVM.ExpensesAmount = _revenuExpensesRepository.GetExpensesAmountAllBranches();
            //StatisticsVM.RevenuesAmount = _revenuExpensesRepository.GetRevenuesAmountAllBranches();
            //StatisticsVM.UnderCollectionCount = _ChecksRepository.GetUnderCollectionCount(BranchId);
            //StatisticsVM.ExportsCount = _ChecksRepository.GetExportsCount(BranchId);
            //StatisticsVM.InvoicesAndServicesCount = _ServicesRepository.GetInvoicesAndServicesCount(BranchId);
            //StatisticsVM.NotificationCount = _NotificationRepository.GetNotificationCount();
            //StatisticsVM.AlertCount = _NotificationRepository.GetAlertCount();
            //StatisticsVM.CustomersCount = _CustomerRepository.GetCustomersCount(BranchId);
            StatisticsVM.AllProjectCount = _ProjectRepository.GetAllProjectCount(BranchId).Result;
            //StatisticsVM.AllUsersCount = _UsersRepository.GetAllUsersCount();
            //StatisticsVM.OutboxCount = _OutInBoxRepository.GetOutboxCount(BranchId);
            //StatisticsVM.InboxCount = _OutInBoxRepository.GetInboxCount(BranchId);
            //StatisticsVM.OfficialDocumentsCount = _OfficialDocumentsRepository.GetOfficialDocumentsCount();
            // StatisticsVM.VacationsCount = _EmployeesRepository.GetVacationsCount(BranchId);
            //StatisticsVM.NationalityIdCount = _EmployeesRepository.GetNationalityIdCount(BranchId);
            // StatisticsVM.PassportNoCount = _EmployeesRepository.GetPassportNoCount(BranchId);
            // StatisticsVM.Projects = _ProjectRepository.GetAllProjectAllBranches().ToList();
            //string NDay = DateTime.Now.Day.ToString();
            //if (Convert.ToInt32(NDay) < 10)
            //{
            //    NDay = "0" + NDay;
            //}
            //string NMonth = DateTime.Now.Month.ToString();
            //if (Convert.ToInt32(NMonth) < 10)
            //{
            //    NMonth = "0" + NMonth;
            //}
            //StatisticsVM.LateTasksCountByUserId = _ProjectPhasesTasksRepository.GetLateTasksCountByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId);
            var res = _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatus(null, 2, BranchId).Result;
            if (res == null)
            {
                res = 0;
            }
            StatisticsVM.InProgressTasksCount = res;
            return StatisticsVM;
        }
        public async Task<StatisticsVM> GetAllUserStatistics(int UserId, int BranchId, string Lang)
        {
            var StatisticsVM = new StatisticsVM();

            //StatisticsVM.UserProjectsCount = _ProjectWorkersRepository.GetUserProjectWorkerCount(UserId, BranchId);//_ProjectRepository.GetUserProjectsCount(UserId, BranchId);
            StatisticsVM.UserMailsCount = await _UserMailsRepository.GetAllUserMailsCount(UserId, BranchId);
            // StatisticsVM.UserTasksCount = _ProjectPhasesTasksRepository.GetUserTaskCount(UserId, BranchId);
            // StatisticsVM.UserWorkOrdersCount = _WorkOrdersRepository.GetUserWorkOrderCount(UserId, BranchId);
            // StatisticsVM.ProjectCountByStatus = _ProjectRepository.GetProjectCountByStatus(UserId, BranchId);
            // StatisticsVM.NotStartedTasksCount = _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatus(UserId, 1, BranchId);
            // StatisticsVM.InProgressTasksCount = _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatus(UserId, 2, BranchId);
            // StatisticsVM.FinishedTasksCount = _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatus(UserId, 4, BranchId);
            var emp =await _employeesRepository.GetEmployeeByUserid(UserId);
            string NDay = DateTime.Now.Day.ToString();
            if (Convert.ToInt32(NDay) < 10)
            {
                NDay = "0" + NDay;
            }
            string NMonth = DateTime.Now.Month.ToString();
            if (Convert.ToInt32(NMonth) < 10)
            {
                NMonth = "0" + NMonth;
            }
            // StatisticsVM.UserNewTaskCount = _ProjectPhasesTasksRepository.GetNewTasksByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang).Count();//_ProjectRepository.GetUserProjectsCount


                var LateTasksCount = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdHome(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang);
            StatisticsVM.LateTasksCount = LateTasksCount.Count();

           var UserLateWorkOrderCount = await _WorkOrdersRepository.GetLateWorkOrdersByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId);
            StatisticsVM.UserLateWorkOrderCount = UserLateWorkOrderCount.Count();
            var NewTasksCountByUserId = await _ProjectPhasesTasksRepository.GetNewTasksByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang, true);
            StatisticsVM.NewTasksCountByUserId = NewTasksCountByUserId.Count();
            var LateTasksCountByUserId = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdHome(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang);
            StatisticsVM.LateTasksCountByUserId = LateTasksCountByUserId.Count();
            var NewWorkOrdersCountByUserId = await _WorkOrdersRepository.GetNewWorkOrdersByUserId(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId);
            StatisticsVM.NewWorkOrdersCountByUserId = NewWorkOrdersCountByUserId.Count();
            StatisticsVM.TasksPercentByUserIdAndProjectId = await _ProjectPhasesTasksRepository.GetTasksPercentByUserIdAndProjectId(UserId, BranchId);
            StatisticsVM.WorkOrdersPercentByUserIdAndProjectId = await _WorkOrdersRepository.GetWorkOrdersPercentByUserIdAndProjectId(UserId, BranchId);
            StatisticsVM.ProjectsPercentByUserIdAndProjectId = await _ProjectRepository.GetUserProjectsCount(UserId, BranchId);
            // StatisticsVM.UserTotalCount = StatisticsVM.UserProjectsCount + StatisticsVM.UserTasksCount + StatisticsVM.UserWorkOrdersCount;

            StatisticsVM.NotStartedTasksCountPrecnt = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatusPercent(UserId, 1, BranchId);
            StatisticsVM.InProgressTasksCountPrecnt = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatusPercent(UserId, 2, BranchId);
            StatisticsVM.FinishedTasksCountPrecnt = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatusPercent(UserId, 4, BranchId);
            StatisticsVM.LateTasksCountPrecnt = await _ProjectPhasesTasksRepository.GetLateTasksByUserIdCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang);



            StatisticsVM.InProgressWorkOrdersCount = await _WorkOrdersRepository.GetWorkOrderCountByStatus(UserId, 2, BranchId);
            StatisticsVM.FinishedWorkOrdersCount = await _WorkOrdersRepository.GetWorkOrderCountByStatus(UserId, 3, BranchId);
            StatisticsVM.LateWorkOrdersCount = await _WorkOrdersRepository.GetLateWorkOrdersCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId);
            StatisticsVM.NotStartedWorkOrdersCount = await _WorkOrdersRepository.GetWorkOrderCountByStatus(UserId, 1, BranchId);
            StatisticsVM.TotalLateCount = await _WorkOrdersRepository.GetLateWorkOrdersCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId) + await _ProjectPhasesTasksRepository.GetLateTasksByUserIdCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang);
            StatisticsVM.TotalInProressCount = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatusPercent(UserId, 2, BranchId) + await _WorkOrdersRepository.GetWorkOrderCountByStatus(UserId, 2, BranchId);
            if (emp != null && emp.Count() > 0)
            {
                StatisticsVM.Custodies = await _custodyService.GetEmployeeCustodies(emp.FirstOrDefault().EmployeeId);
            }
            else
            {
                StatisticsVM.Custodies = "0";
            }
                return StatisticsVM;
        }
        public async Task<StatisticsVM> GetUserStatisticsPercentData(int UserId, int BranchId, string Lang)
        {
            var StatisticsVM = new StatisticsVM();
            string NDay = DateTime.Now.Day.ToString();
            if (Convert.ToInt32(NDay) < 10)
            {
                NDay = "0" + NDay;
            }
            string NMonth = DateTime.Now.Month.ToString();
            if (Convert.ToInt32(NMonth) < 10)
            {
                NMonth = "0" + NMonth;
            }

            StatisticsVM.TasksPercentByUserIdAndProjectId = await _ProjectPhasesTasksRepository.GetTasksPercentByUserIdAndProjectId(UserId, BranchId);
            StatisticsVM.WorkOrdersPercentByUserIdAndProjectId = await _WorkOrdersRepository.GetWorkOrdersPercentByUserIdAndProjectId(UserId, BranchId);
            StatisticsVM.ProjectsPercentByUserIdAndProjectId = await _ProjectRepository.GetUserProjectsCount(UserId, BranchId);

            StatisticsVM.TotalLateCount = await _WorkOrdersRepository.GetLateWorkOrdersCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId) + await _ProjectPhasesTasksRepository.GetLateTasksByUserIdCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang);
            StatisticsVM.TotalInProressCount = await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatusPercent(UserId, 2, BranchId) + await _WorkOrdersRepository.GetWorkOrderCountByStatus(UserId, 2, BranchId);
            return StatisticsVM;
        }

        public async Task<StatisticsVM> GetAllUserCustodiesStatistics(int UserId)
        {
            var StatisticsVM = new StatisticsVM();
            var emp = await _employeesRepository.GetEmployeeByUserid(UserId);
            if (emp != null && emp.Count() > 0)
            {
                StatisticsVM.Custodies = await _custodyService.GetEmployeeCustodies(emp.FirstOrDefault().EmployeeId);
            }
            else
            {
                StatisticsVM.Custodies = "0";
            }
            return StatisticsVM;
        }
        public async Task< StatisticsVM> GetAllUserStatisticsFullReport(int UserId, int BranchId, string Lang)
        {
            var StatisticsVM = new StatisticsVM();
            string NDay = DateTime.Now.Day.ToString();
            if (Convert.ToInt32(NDay) < 10)
            {
                NDay = "0" + NDay;
            }
            string NMonth = DateTime.Now.Month.ToString();
            if (Convert.ToInt32(NMonth) < 10)
            {
                NMonth = "0" + NMonth;
            }
            StatisticsVM.TotalLateCount =await _WorkOrdersRepository.GetLateWorkOrdersCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId) +await _ProjectPhasesTasksRepository.GetLateTasksByUserIdCount(DateTime.Now.Year.ToString() + "-" + NMonth + "-" + NDay, UserId, BranchId, Lang);
            StatisticsVM.TotalInProressCount =await _ProjectPhasesTasksRepository.GetProjectPhasesTasksCountByStatusPercent(UserId, 2, BranchId) + await _WorkOrdersRepository.GetWorkOrderCountByStatus(UserId, 2, BranchId);
            return StatisticsVM;
        }

        public LayoutVM GetLayoutVm(int UserId, int BranchId, string Lang)
        {
            var layout = new LayoutVM();
            layout.NotificationsCount = _NotificationRepository.GetNotificationReceived(UserId).Result.Count();
            layout.AllertCount = _NotificationRepository.GetUserlAlerts(BranchId, UserId).Result.Count();
            layout.TasksByUserCount = _ProjectPhasesTasksRepository.GetTasksByUserId(UserId, 0, BranchId).Result.Count();
            layout.MyInboxCount = _UserMailsRepository.GetAllUserMails(UserId, BranchId).Result.Count();
            //layout.ProjectCount = _ProjectRepository.GetMatching(s => s.MangerId == UserId && s.IsDeleted == false).Count();
            layout.ProjectCount = _ProjectRepository.GetUserProjects2(UserId, BranchId, "").Result.Count();
            layout.WorkOrderCount = _WorkOrdersRepository.GetWorkOrdersByUserId(UserId, BranchId).Result.Count();
            layout.TaskCount = _ProjectPhasesTasksRepository.GetTasksByUserIdUser(UserId, Lang, 0, BranchId).Result.Count();
            layout.OnlineUser = _UsersRepository.GetAllUsersOnline2().Result.Count();
            return layout;
        }
    }
}
