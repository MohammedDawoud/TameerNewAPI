using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class EmployeeService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeeRepository;
        private readonly IEmpLocationsRepository _empLocationsRepository;

        private readonly IBranchesRepository _BranchesRepository;
        private readonly IAccountsRepository _accountsRepository;
        private readonly IEmpStructureRepository _EmpStructureRepository;
        private readonly ICustodyRepository _CustodyRepository;
        private readonly IJobRepository _JobRepository;
        private readonly IUsersRepository _UserRepository;
        private readonly IPayrollMarchesRepository _payrollMarchesRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IEmpContractRepository _empContractRepository;
        private readonly IContractRepository _contractRepository;

        private readonly ISettingsRepository _SettingsRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IWorkOrdersRepository _workordersRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly ISystemAction _SystemAction;
        private readonly TaamerProjectContext _TaamerProContext;
        private  IPayrollMarchesService _payrollMarchesService;
        private readonly INationalityRepository _NationalityRepository;
        private readonly IOrganizationsService _organizationsService;
        private readonly ICustomerMailService _customerMailService;
        private readonly INotificationService _notificationService;




        public EmployeeService(IEmployeesRepository employeesRepository, IEmpLocationsRepository empLocationsRepository, IBranchesRepository branchesRepository, IAccountsRepository accountsRepository, IEmpStructureRepository empStructureRepository,
            ICustodyRepository custodyRepository, IJobRepository jobRepository, IUsersRepository usersRepository, IPayrollMarchesRepository payrollMarchesRepository,
            ISys_SystemActionsRepository sys_SystemActions, IEmpContractRepository empContractRepository, IContractRepository contractRepository, ISettingsRepository settingsRepository,
            IProjectPhasesTasksRepository projectPhasesTasksRepository, IProjectRepository projectRepository, IWorkOrdersRepository workOrdersRepository,
            ITransactionsRepository transactionsRepository, ISystemAction systemAction, TaamerProjectContext dataContext, IPayrollMarchesService payrollMarchesService
            , INationalityRepository nationalityRepository,IOrganizationsService organizationsService,ICustomerMailService customerMailService,
            INotificationService notificationService)
        {
            _empLocationsRepository = empLocationsRepository;

            _employeeRepository = employeesRepository;
            _BranchesRepository = branchesRepository;
            _accountsRepository = accountsRepository;
            _EmpStructureRepository = empStructureRepository;
            _CustodyRepository = custodyRepository;
            _JobRepository = jobRepository;
            _UserRepository = usersRepository;
            _SystemAction = systemAction;
            _payrollMarchesRepository = payrollMarchesRepository;
            _Sys_SystemActionsRepository = sys_SystemActions;
            _empContractRepository = empContractRepository;
            _contractRepository = contractRepository;

            _SettingsRepository = settingsRepository;
            _ProjectPhasesTasksRepository = projectPhasesTasksRepository;
            _ProjectRepository = projectRepository;
            _workordersRepository = workOrdersRepository;
            _TransactionsRepository = transactionsRepository;
            _TaamerProContext = dataContext;
            _payrollMarchesService = payrollMarchesService;
            _NationalityRepository= nationalityRepository;
            _organizationsService = organizationsService;
            _customerMailService = customerMailService;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int BranchId)
        {
            var employees = await _employeeRepository.GetAllEmployees(lang, BranchId);
            return employees;
        }
        public async Task<IEnumerable<EmpLocationsVM>> GetAllEmployeesByLocationId(string lang, int LocationId)
        {
            //var employees = await _employeeRepository.GetAllEmployeesByLocationId(lang, LocationId);
            var employees = await _empLocationsRepository.GetLocationByLocationId(lang, LocationId);
            return employees;
        }
        public async Task<IEnumerable<EmployeesVM>> GetAllArchivesEmployees(string lang, int BranchId)
        {
            var employees =await _employeeRepository.GetAllArchivesEmployees(lang, BranchId);
            return employees;
        }
        public string GetEmployeeJobName(int EmpId, string lang, int BranchId)
        {
            int? Empjobid = _employeeRepository.GetMatching(s => s.EmployeeId == EmpId).FirstOrDefault().JobId ?? default(int);
            var jobsName = _JobRepository.GetMatching(w => w.JobId == Empjobid).FirstOrDefault().JobNameAr;
            return jobsName;
        }
        public Job GetEmployeeJob(int EmpId, string lang, int BranchId)
        {
            int? Empjobid = _employeeRepository.GetMatching(s => s.EmployeeId == EmpId).FirstOrDefault().JobId ?? default(int);
            var jobs = _JobRepository.GetMatching(w => w.JobId == Empjobid).FirstOrDefault();
            return jobs;
        }
        public async Task< IEnumerable<EmployeesVM>> GetAllBranchEmployees(string lang)
        {
            var employees =await _employeeRepository.GetAllBranchEmployees(lang);
            return employees;
        }

        public async Task<IEnumerable<EmployeesVM>> GetAllEmployees(string lang, int SearchAll, int Branch)
        {
            var employees =await _employeeRepository.GetAllEmployees(lang, SearchAll, Branch);
            return employees;
        }

        public async Task<IEnumerable<EmployeesVM>> GetAllEmployeesSearch(EmployeesVM SalarySearch, string lang, int UserId, int BranchId, string Con)
        {
            if (SalarySearch.IsSearch)
            {
                var Emps = await _employeeRepository.GetAllEmployeesBySearchObject(SalarySearch, lang, BranchId, Con);
                return Emps;
            }
            else
            {
                return await _employeeRepository.GetAllEmployeesSearch(lang, BranchId);
            }
        }
        public IEnumerable<EmployeesVM> GetAllEmployeesSearch(bool IsAllBranch, string lang, int UserId, int BranchId,int Monthno,int YearId, string Con)
        {
            var Emps = _employeeRepository.GetEmployeesForPayroll(IsAllBranch, lang, UserId, BranchId, Monthno, YearId, Con);

        
            return Emps;
        }


        public IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, string Con)
        {
            var Emps = _employeeRepository.GetEmployeesForPayroll(IsAllBranch, lang, UserId, BranchId, Con).Result;

            ///PayrollMarches
            foreach (var Emp in Emps)
            {

                var payroll = _payrollMarchesRepository.GetPayrollMarches(Emp.EmployeeId, DateTime.Now.Month).Result;
                if (payroll == null)
                {

                    var payrollObj = new PayrollMarches()
                    {
                        EmpId = Emp.EmployeeId,
                        MonthNo = DateTime.Now.Month,
                        MainSalary = Emp.Salary,
                        SalaryOfThisMonth = Emp.ThisMonthSalary,
                        Bonus = Emp.Bonus,
                        CommunicationAllawance = Emp.CommunicationAllawance,
                        ProfessionAllawance = Emp.ProfessionAllawance,
                        TransportationAllawance = Emp.TransportationAllawance,
                        HousingAllowance = Emp.HousingAllowance,
                        MonthlyAllowances = Emp.MonthlyAllowances,
                        ExtraAllowances = Emp.ExtraAllowances,
                        TotalRewards = Emp.TotalRewards,
                        TotalDiscounts = Emp.TotalDiscounts,
                        TotalLoans = Emp.TotalLoans,
                        TotalSalaryOfThisMonth = Emp.TotalySalaries,
                        TotalAbsDays = Emp.TotalDayAbs,
                        TotalVacations = Emp.TotalPaidVacations,
                        Taamen = Emp.Taamen


                    };
                    var AddPayroll = _payrollMarchesService.SavePayrollMarches(payrollObj, UserId, BranchId);
                }
                else if (!payroll.PostDate.HasValue)
                {
                    payroll.MainSalary = Emp.Salary;
                    payroll.SalaryOfThisMonth = Emp.ThisMonthSalary;
                    payroll.Bonus = Emp.Bonus;
                    payroll.CommunicationAllawance = Emp.CommunicationAllawance;
                    payroll.ProfessionAllawance = Emp.ProfessionAllawance;
                    payroll.TransportationAllawance = Emp.TransportationAllawance;
                    payroll.HousingAllowance = Emp.HousingAllowance;
                    payroll.MonthlyAllowances = Emp.MonthlyAllowances;
                    payroll.ExtraAllowances = Emp.ExtraAllowances;
                    payroll.TotalRewards = Emp.TotalRewards;
                    payroll.TotalDiscounts = Emp.TotalDiscounts;
                    payroll.TotalLoans = Emp.TotalLoans;
                    payroll.TotalSalaryOfThisMonth = Emp.TotalySalaries;
                    payroll.TotalAbsDays = Emp.TotalDayAbs;
                    payroll.TotalVacations = Emp.TotalPaidVacations;
                    payroll.Taamen = Emp.Taamen;
                    _payrollMarchesService.SavePayrollMarches(payroll, UserId, BranchId);
                }
            }
            return Emps;
        }


        public IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId,int Monthno, string Con)
        {
            var Emps = _employeeRepository.GetEmployeesForPayroll(IsAllBranch, lang, UserId, BranchId, Monthno,DateTime.Now.Year, Con);

            ///PayrollMarches
            foreach (var Emp in Emps)
            {

                  var payroll = _payrollMarchesRepository.GetPayrollMarches(Emp.EmployeeId, Monthno,DateTime.Now.Year).Result;
                if (payroll == null)
                {

                    var payrollObj = new PayrollMarches()
                    {
                        EmpId = Emp.EmployeeId,
                        MonthNo = Monthno,
                        MainSalary = Emp.Salary,
                        SalaryOfThisMonth = Emp.ThisMonthSalary,
                        Bonus = Emp.Bonus,
                        CommunicationAllawance = Emp.CommunicationAllawance,
                        ProfessionAllawance = Emp.ProfessionAllawance,
                        TransportationAllawance = Emp.TransportationAllawance,
                        HousingAllowance = Emp.HousingAllowance,
                        MonthlyAllowances = Emp.MonthlyAllowances,
                        ExtraAllowances = Emp.ExtraAllowances,
                        TotalRewards = Emp.TotalRewards,
                        TotalDiscounts = Emp.TotalDiscounts,
                        TotalLoans = Emp.TotalLoans,
                        TotalSalaryOfThisMonth = Emp.TotalySalaries,
                        TotalAbsDays = Emp.TotalDayAbs,
                        TotalVacations = Emp.TotalPaidVacations,
                        Taamen = Emp.Taamen,
                        YearId=DateTime.Now.Year,
                        TotalLateDiscount =Emp.TotalLateDiscount,
                        TotalAbsenceDiscount=Emp.TotalAbsenceDiscount,
                        


                    };
                    var AddPayroll = _payrollMarchesService.SavePayrollMarches(payrollObj, UserId, BranchId);
                }
                else if (!payroll.PostDate.HasValue)
                {
                    payroll.MainSalary = Emp.Salary;
                    payroll.SalaryOfThisMonth = Emp.ThisMonthSalary;
                    payroll.Bonus = Emp.Bonus;
                    payroll.CommunicationAllawance = Emp.CommunicationAllawance;
                    payroll.ProfessionAllawance = Emp.ProfessionAllawance;
                    payroll.TransportationAllawance = Emp.TransportationAllawance;
                    payroll.HousingAllowance = Emp.HousingAllowance;
                    payroll.MonthlyAllowances = Emp.MonthlyAllowances;
                    payroll.ExtraAllowances = Emp.ExtraAllowances;
                    payroll.TotalRewards = Emp.TotalRewards;
                    payroll.TotalDiscounts = Emp.TotalDiscounts;
                    payroll.TotalLoans = Emp.TotalLoans;
                    payroll.TotalSalaryOfThisMonth = Emp.TotalySalaries;
                    payroll.TotalAbsDays = Emp.TotalDayAbs;
                    payroll.TotalVacations = Emp.TotalPaidVacations;
                    payroll.Taamen = Emp.Taamen;
                    payroll.YearId = DateTime.Now.Year;
                    payroll.TotalLateDiscount = Emp.TotalLateDiscount;
                    payroll.TotalAbsenceDiscount = Emp.TotalAbsenceDiscount;
                    _payrollMarchesService.SavePayrollMarches(payroll, UserId, BranchId);
                }
            }
            return Emps;
        }


        public IEnumerable<EmployeesVM> GetEmployeesForPayroll(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno,int YearId, string Con)
        {
            var Emps = _employeeRepository.GetEmployeesForPayroll(IsAllBranch, lang, UserId, BranchId, Monthno, YearId, Con);

            ///PayrollMarches
            foreach (var Emp in Emps)
            {

                var payroll = _payrollMarchesRepository.GetPayrollMarches(Emp.EmployeeId, Monthno,YearId).Result;
                if (payroll == null)
                {

                    var payrollObj = new PayrollMarches()
                    {
                        EmpId = Emp.EmployeeId,
                        MonthNo = Monthno,
                        MainSalary = Emp.Salary,
                        SalaryOfThisMonth = Emp.ThisMonthSalary,
                        Bonus = Emp.Bonus,
                        CommunicationAllawance = Emp.CommunicationAllawance,
                        ProfessionAllawance = Emp.ProfessionAllawance,
                        TransportationAllawance = Emp.TransportationAllawance,
                        HousingAllowance = Emp.HousingAllowance,
                        MonthlyAllowances = Emp.MonthlyAllowances,
                        ExtraAllowances = Emp.ExtraAllowances,
                        TotalRewards = Emp.TotalRewards,
                        TotalDiscounts = Emp.TotalDiscounts,
                        TotalLoans = Emp.TotalLoans,
                        TotalSalaryOfThisMonth = Emp.TotalySalaries,
                        TotalAbsDays = Emp.TotalDayAbs,
                        TotalVacations = Emp.TotalPaidVacations,
                        Taamen = Emp.Taamen,
                        YearId=YearId,
                        TotalAbsenceDiscount = Emp.TotalAbsenceDiscount,
                        TotalLateDiscount = Emp.TotalLateDiscount,


                    };
                    var AddPayroll = _payrollMarchesService.SavePayrollMarches(payrollObj, UserId, BranchId);
                }
                else if (!payroll.PostDate.HasValue)
                {
                    payroll.MainSalary = Emp.Salary;
                    payroll.SalaryOfThisMonth = Emp.ThisMonthSalary;
                    payroll.Bonus = Emp.Bonus;
                    payroll.CommunicationAllawance = Emp.CommunicationAllawance;
                    payroll.ProfessionAllawance = Emp.ProfessionAllawance;
                    payroll.TransportationAllawance = Emp.TransportationAllawance;
                    payroll.HousingAllowance = Emp.HousingAllowance;
                    payroll.MonthlyAllowances = Emp.MonthlyAllowances;
                    payroll.ExtraAllowances = Emp.ExtraAllowances;
                    payroll.TotalRewards = Emp.TotalRewards;
                    payroll.TotalDiscounts = Emp.TotalDiscounts;
                    payroll.TotalLoans = Emp.TotalLoans;
                    payroll.TotalSalaryOfThisMonth = Emp.TotalySalaries;
                    payroll.TotalAbsDays = Emp.TotalDayAbs;
                    payroll.TotalVacations = Emp.TotalPaidVacations;
                    payroll.Taamen = Emp.Taamen;
                    payroll.YearId = YearId;
                    payroll.TotalLateDiscount=Emp.TotalLateDiscount;
                    payroll.TotalAbsenceDiscount=Emp.TotalAbsenceDiscount;
                    _payrollMarchesService.SavePayrollMarches(payroll, UserId, BranchId);
                }
            }
            return Emps;
        }
        public IEnumerable<EmployeesVM> GetEmployeesForPayrollPrint(bool IsAllBranch, string lang, int UserId, int BranchId, int Monthno, int YearId, string Con)
        {
            var Emps = _employeeRepository.GetEmployeesForPayroll(IsAllBranch, lang, UserId, BranchId, Monthno, YearId, Con);

            return Emps;
        }

        public async Task<IEnumerable<EmployeesVM>> GetEmployeeByUserid(int UserId)
        {
            return await _employeeRepository.GetEmployeeByUserid(UserId);
        }
        public string GetAccountCodeNewValue(int parentid, int accountid)
        {
            try
            {
                var AccountCodeNew = "";
                var CodeNewList = new List<decimal>();
                var Accounts = _accountsRepository.GetMatching(s => s.ParentId == parentid && s.IsDeleted == false).Where(a => a.AccountId != accountid);
                if (Accounts.Count() > 0)
                {
                    foreach (var v in Accounts)
                    {
                        if (v.AccountCodeNew != "")
                        {
                            decimal ValueDec = Convert.ToDecimal(v.AccountCodeNew);
                            CodeNewList.Add(ValueDec);
                        }

                    }
                    var getmax = CodeNewList.Max();
                    AccountCodeNew = (getmax + 1).ToString();

                }
                else
                {
                    var Accounts2 = _accountsRepository.GetById(parentid);
                    if (((Accounts2.Level ?? 0) + 1 == 1) || ((Accounts2.Level ?? 0) + 1 == 2))
                    {
                        AccountCodeNew = Accounts2.AccountCodeNew + "01";
                    }
                    else
                    {
                        AccountCodeNew = Accounts2.AccountCodeNew + "0001";

                    }
                }
                return AccountCodeNew;
            }
            catch (Exception ex)
            {

                return "";
            }


        }

        public GeneralMessage SaveEmployee(Employees emp, int UserId, int BranchId)
        {
            try
            {
                if(emp.BirthDate != null) {
                var bdate = DateTime.ParseExact(emp.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    emp.BirthDate = bdate.ToString();
                }
                string EmpAccCode = "";
                var codeExist = _employeeRepository.GetMatching(s => s.IsDeleted == false/*&& string.IsNullOrEmpty(s.EndWorkDate)*/ && s.EmployeeId != emp.EmployeeId && s.EmployeeNo == emp.EmployeeNo).FirstOrDefault();
                var EmpNational = _employeeRepository.SearchEmployeesOfNational(emp.NationalId, "", emp.BranchId.Value, emp.EmployeeId).Result;

                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ موظف";
                   _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "رقم الموظف موجود من قبل", "", "", ActionDate, UserId, emp.BranchId.Value, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "رقم الموظف موجود من قبل" };
                }
                if (emp.UserId != null && emp.UserId !=0)
                {
                    var UserExist = _employeeRepository.GetMatching(s => s.IsDeleted == false && s.UserId == emp.UserId);
                    if (UserExist.Count() > 0)
                    {
                        var x = UserExist.LastOrDefault().EmployeeId;
                        if (UserExist.LastOrDefault().EmployeeId != emp.EmployeeId)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = "فشل في حفظ موظف";
                           _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "تم ربط موظف اخر بهذا المستخدم", "", "", ActionDate, UserId, emp.BranchId.Value, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "تم ربط موظف اخر بهذا المستخدم" };

                        }
                    }
                }
                if (EmpNational != 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ موظف";
                    _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "قم بتغيير رقم الاقامه لانه موجود بالفعل", "", "", ActionDate, UserId, emp.BranchId.Value, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "قم بتغيير رقم الاقامه لانه موجود بالفعل" };
                }

                if (emp.EmployeeId == 0)
                {
                    emp.AddUser = UserId;
                    //emp.BranchId = BranchId;
                    emp.AddDate = DateTime.Now;
                    emp.Active = false;
                    //employee Acc
                    if (emp.AccountId == null)
                    {
                        if (emp.Email != null)
                        {
                            var EmpEmail = _employeeRepository.SearchEmployeesOfEmail(emp.Email, emp.BranchId.Value).Result;

                            if (EmpEmail != 0)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote1 = "فشل في حفظ موظف";
                               _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "قم بتغيير اسم الميل، فهو موجود بالفعل!", "", "", ActionDate1, UserId, emp.BranchId.Value, ActionNote1, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "قم بتغيير اسم الميل، فهو موجود بالفعل!" };
                            }
                        }


                        var Branch2 = _BranchesRepository.GetById(emp.BranchId.Value);
                        if (Branch2 == null || Branch2.EmployeesAccId == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ موظف";
                           _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, تأكد من انشاء حساب رئيسي للموظفين وربطه بالفرع الحالي", "", "", ActionDate2, UserId, emp.BranchId.Value, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "خطأ في الحفظ, تأكد من انشاء حساب رئيسي للموظفين وربطه بالفرع الحالي " };
                        }

                        if (Branch2 == null || Branch2.LoanAccId == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في حفظ موظف";
                           _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, تأكد من انشاء حساب رئيسي للسلف وربطه بالفرع الحالي", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "خطأ في الحفظ, تأكد من انشاء حساب رئيسي للسلف وربطه بالفرع الحالي " };
                        }
                        //if (Branch2 == null || Branch2.PurchaseDiscAccId == null)
                        //{
                        //    //-----------------------------------------------------------------------------------------------------------------
                        //    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        //    string ActionNote4 = "فشل في حفظ موظف";
                        //    SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, تأكد من انشاء حساب رئيسي لحصيلة جزاءات الموظف وربطه بالفرع الحالي", "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //    //-----------------------------------------------------------------------------------------------------------------

                        //    return new GeneralMessage { Result = false, Message = "خطأ في الحفظ, تأكد من انشاء حساب رئيسي لحصيلة جزاءات الموظف وربطه بالفرع الحالي " };
                        //}
                        //if (Branch2 == null || Branch2.PurchaseApprovalAccId == null)
                        //{
                        //    return new GeneralMessage { Result = false, Message = "خطأ في الحفظ, تأكد من انشاء حساب رئيسي لمكافئات الموظف وربطه بالفرع الحالي " };
                        //}
                        //if (Branch2 == null || Branch2.RevenuesAccountId == null)
                        //{
                        //    return new GeneralMessage { Result = false, Message = "خطأ في الحفظ, تأكد من انشاء حساب رئيسي لراتب الموظف وربطه بالفرع الحالي " };
                        //}
                        if (Branch2 == null || Branch2.PurchaseDelayAccId == null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote5 = "فشل في حفظ موظف";
                           _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, تأكد من انشاء حساب رئيسي لعهد الموظف وربطه بالفرع الحالي", "", "", ActionDate5, UserId, emp.BranchId.Value, ActionNote5, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "خطأ في الحفظ, تأكد من انشاء حساب رئيسي لعهد الموظف وربطه بالفرع الحالي " };
                        }

                        try
                        {
                            var Branch = _BranchesRepository.GetById(emp.BranchId.Value);
                            if (Branch != null && Branch.EmployeesAccId != null)
                            {
                                var parentEmpAcc = _accountsRepository.GetById((int)Branch.EmployeesAccId);
                                var newEmpAcc = new Accounts();

                                var AccCode = _accountsRepository.GetNewCodeByParentId(Branch.EmployeesAccId ?? 0,2).Result;
                                newEmpAcc.Code = AccCode;

                                //newEmpAcc.Code = parentEmpAcc.Code + substrCode;
                                newEmpAcc.Classification = parentEmpAcc.Classification??15;
                                newEmpAcc.ParentId = parentEmpAcc.AccountId;
                                newEmpAcc.IsMain = false;
                                newEmpAcc.Level = parentEmpAcc.Level + 1;
                                newEmpAcc.Nature = 2; //depit
                                newEmpAcc.Halala = true;
                                newEmpAcc.NameAr = "حساب الموظف" + "  " + emp.EmployeeNameAr;
                                newEmpAcc.NameEn = emp.EmployeeNameEn + " " + "Employee Account";
                                newEmpAcc.Type = 2; //bugget
                                newEmpAcc.Active = true;
                                newEmpAcc.AddUser = UserId;
                                newEmpAcc.BranchId = emp.BranchId.Value;
                                newEmpAcc.AddDate = DateTime.Now;

                                _TaamerProContext.Accounts.Add(newEmpAcc);
                                parentEmpAcc.IsMain = true; // update main acc
                                _TaamerProContext.SaveChanges();
                                emp.AccountId = newEmpAcc.AccountId;//_accountsRepository.GetMaxId() + 1;
                                var cutAcc = _accountsRepository.GetById((int)emp.AccountId);
                                if (cutAcc != null)
                                {
                                    EmpAccCode = cutAcc.Code;
                                }
                            }
                            else
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote6 = "فشل في حفظ موظف";
                               _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, فشل في انشاء حساب للموظف تأكد من انشاء حساب رئيسي للموظفين وربطه بالفرع الحالي", "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "خطأ في الحفظ, فشل في انشاء حساب للموظف تأكد من انشاء حساب رئيسي للموظفين وربطه بالفرع الحالي " };
                            }

                            if (Branch != null && Branch.LoanAccId != null)
                            {
                                var parentEmpAcc = _accountsRepository.GetById((int)Branch.LoanAccId);
                                var newEmpAcc = new Accounts();

                                var AccCode = _accountsRepository.GetNewCodeByParentId(Branch.LoanAccId ?? 0,2).Result;
                                newEmpAcc.Code = AccCode;

                                // newEmpAcc.Code = parentEmpAcc.Code + substrCode;
                                //newEmpAcc.Code = parentEmpAcc.Code + substrCode;
                                newEmpAcc.Classification = parentEmpAcc.Classification ?? 15;
                                newEmpAcc.ParentId = parentEmpAcc.AccountId;
                                newEmpAcc.IsMain = false;
                                newEmpAcc.Level = parentEmpAcc.Level + 1;
                                newEmpAcc.Nature = 1; //depit
                                newEmpAcc.Halala = true;
                                newEmpAcc.NameAr = " حساب السلف للموظف " + "  " + emp.EmployeeNameAr;
                                newEmpAcc.NameEn = emp.EmployeeNameEn + " " + "  Employee Loan Account ";
                                newEmpAcc.Type = 2; //bugget
                                newEmpAcc.Active = true;
                                newEmpAcc.AddUser = UserId;
                                newEmpAcc.BranchId = emp.BranchId.Value;
                                newEmpAcc.AddDate = DateTime.Now;

                                _TaamerProContext.Accounts.Add(newEmpAcc);
                                parentEmpAcc.IsMain = true; // update main acc
                                _TaamerProContext.SaveChanges();
                                emp.AccountIDs = newEmpAcc.AccountId;//_accountsRepository.GetMaxId() + 1;
                                //var cutAcc = _accountsRepository.GetById(emp.AccountIDs);
                                //if (cutAcc != null)
                                //{
                                //    EmpAccCode = cutAcc.Code;
                                //}
                            }
                            else
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote7 = "فشل في حفظ موظف";
                               _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, فشل في انشاء حساب سلف للموظف تأكد من انشاء حساب رئيسي للسلف وربطه بالفرع الحالي", "", "", ActionDate7, UserId, emp.BranchId.Value, ActionNote7, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "خطأ في الحفظ, فشل في انشاء حساب سلف للموظف تأكد من انشاء حساب رئيسي للسلف وربطه بالفرع الحالي " };
                            }


                           

                            if (Branch != null && Branch.PurchaseDelayAccId != null)
                            {
                                var parentEmpAcc = _accountsRepository.GetById((int)Branch.PurchaseDelayAccId);
                                var newEmpAcc = new Accounts();
                                var AccCode = _accountsRepository.GetNewCodeByParentId(Branch.PurchaseDelayAccId ?? 0,2).Result;
                                newEmpAcc.Code = AccCode;

                                // newEmpAcc.Code = parentEmpAcc.Code + substrCode;

                                //newEmpAcc.Code = parentEmpAcc.Code + substrCode;
                                newEmpAcc.Classification = parentEmpAcc.Classification ?? 15;
                                newEmpAcc.ParentId = parentEmpAcc.AccountId;
                                newEmpAcc.IsMain = false;
                                newEmpAcc.Level = parentEmpAcc.Level + 1;
                                newEmpAcc.Nature = 1;
                                newEmpAcc.Halala = true;
                                newEmpAcc.NameAr = " حساب عهد للموظف " + "  " + emp.EmployeeNameAr;
                                newEmpAcc.NameEn = emp.EmployeeNameEn + " " + "  Employee Custody Account ";
                                newEmpAcc.Type = 2; //bugget
                                newEmpAcc.Active = true;
                                newEmpAcc.AddUser = UserId;
                                newEmpAcc.BranchId = emp.BranchId.Value;
                                newEmpAcc.AddDate = DateTime.Now;

                                _TaamerProContext.Accounts.Add(newEmpAcc);
                                parentEmpAcc.IsMain = true; // update main acc
                                _TaamerProContext.SaveChanges();
                                emp.AccountIDs_Custody = newEmpAcc.AccountId;//_accountsRepository.GetMaxId() + 1;
                                //var cutAcc = _accountsRepository.GetById(emp.AccountIDs);
                                //if (cutAcc != null)
                                //{
                                //    EmpAccCode = cutAcc.Code;
                                //}
                            }
                            else
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate9 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote9 = "فشل في حفظ موظف";
                               _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, "خطأ في الحفظ, فشل في انشاء حساب عهدة للموظف تأكد من انشاء حساب رئيسي لعهد الموظف وربطه بالفرع الحالي", "", "", ActionDate9, UserId, emp.BranchId.Value, ActionNote9, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "خطأ في الحفظ, فشل في انشاء حساب عهدة للموظف تأكد من انشاء حساب رئيسي لعهد الموظف وربطه بالفرع الحالي " };
                            }

                        }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate10 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote10 = "فشل في حفظ موظف";
                           _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate10, UserId, emp.BranchId.Value, ActionNote10, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
                        }
                    }
                    _TaamerProContext.Employees.Add(emp);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة موظف جديد";
                   _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, emp.BranchId.Value, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = emp.EmployeeId, ReturnedStr = EmpAccCode };
                }
                else
                {
                    var empUpdated = _employeeRepository.GetById(emp.EmployeeId);
                    var SaveNationalDoc = _employeeRepository.GetById(emp.EmployeeId);

                    if (empUpdated != null)
                    {
                        empUpdated.EmployeeNo = emp.EmployeeNo;
                        empUpdated.Email = emp.Email;
                        empUpdated.Mobile = emp.Mobile;
                        empUpdated.Address = emp.Address;
                        empUpdated.EducationalQualification = emp.EducationalQualification;
                        empUpdated.BirthDate = emp.BirthDate;
                        empUpdated.BirthHijriDate = emp.BirthHijriDate;
                        empUpdated.BirthPlace = emp.BirthPlace;
                        empUpdated.MaritalStatus = emp.MaritalStatus;
                        empUpdated.ChildrenNo = emp.ChildrenNo;
                        empUpdated.Gender = emp.Gender;
                        empUpdated.NationalityId = emp.NationalityId;
                        empUpdated.ReligionId = emp.ReligionId;
                        empUpdated.UserId = emp.UserId;
                        empUpdated.PostalCode = emp.PostalCode;
                        if (emp.PhotoUrl != null)
                        {
                            empUpdated.PhotoUrl = emp.PhotoUrl;
                        }
                        empUpdated.DepartmentId = emp.DepartmentId;
                        empUpdated.DeppID = emp.DeppID;
                        empUpdated.Telephone = emp.Telephone;
                        empUpdated.Mailbox = emp.Mailbox;
                        empUpdated.UpdateUser = UserId;
                        empUpdated.UpdateDate = DateTime.Now;
                        empUpdated.DirectManager = emp.DirectManager;
                        empUpdated.Active = false;
                        empUpdated.Age=emp.Age;
                        if (empUpdated.NationalIdEndDate != emp.NationalIdEndDate)
                        {
                            empUpdated.IsRememberResident = null;
                            empUpdated.RememberDateResident = null;

                        }
                        if(empUpdated.BranchId != emp.BranchId)
                        {

                            var oldbranch = _TaamerProContext.Branch.Where(x => x.BranchId == empUpdated.BranchId).FirstOrDefault();
                            empUpdated.BranchId = emp.BranchId;
                            try
                            {
                                sendemployeemailNotification(empUpdated, 1, oldbranch?.NameAr);
                            }catch(Exception ex)
                            {

                            }
                        }
                        if (empUpdated.JobId != emp.JobId)
                        {
                            var oldjob = _TaamerProContext.Job.Where(x => x.JobId == empUpdated.JobId).FirstOrDefault();
                            empUpdated.JobId = emp.JobId;
                            try
                            { 
                            sendemployeemailNotification(empUpdated, 2, oldjob?.JobNameAr);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        //if (empUpdated.ContractEndDate != emp.ContractEndDate || empUpdated.ContractStartDate != emp.ContractStartDate)
                        //{
                        //    empUpdated.IsRememberContract = null;
                        //    empUpdated.RememberDateContract = null;

                        //}
                        //empUpdated.Taamen = emp.Taamen;
                        if (empUpdated.EmployeeNameAr != emp.EmployeeNameAr)
                        {
                            var cutAcc = _accountsRepository.GetById((int)empUpdated.AccountId);
                            if (cutAcc != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc.BranchId = emp.BranchId ?? 1;
                                cutAcc.NameAr = "حساب الموظف" + "  " + emp.EmployeeNameAr;
                                cutAcc.NameEn = emp.EmployeeNameEn + " " + "Employee Account";
                            }

                            var cutAcc_s = _accountsRepository.GetById((int)empUpdated.AccountIDs);
                            if (cutAcc_s != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc_s.BranchId = emp.BranchId ?? 1;
                                cutAcc_s.NameAr = "حساب السلف للموظف" + "  " + emp.EmployeeNameAr;
                                cutAcc_s.NameEn = emp.EmployeeNameEn + " " + "Employee Loan Account";
                            }

                            var cutAcc_c = _accountsRepository.GetById((int)empUpdated.AccountIDs_Custody);
                            if (cutAcc_c != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc_c.BranchId = emp.BranchId ?? 1;
                                cutAcc_c.NameAr = "حساب عهد للموظف" + "  " + emp.EmployeeNameAr;
                                cutAcc_c.NameEn = emp.EmployeeNameEn + " " + "Employee Custody Account";
                            }
                        }
                        else if (empUpdated.EmployeeNameEn != emp.EmployeeNameEn)
                        {
                            var cutAcc = _accountsRepository.GetById((int)empUpdated.AccountId);
                            if (cutAcc != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc.BranchId = emp.BranchId ?? 1;
                                cutAcc.NameAr = "حساب الموظف" + "  " + emp.EmployeeNameAr;
                                cutAcc.NameEn = emp.EmployeeNameEn + " " + "Employee Account";
                            }

                            var cutAcc_s = _accountsRepository.GetById((int)empUpdated.AccountIDs);
                            if (cutAcc_s != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc_s.BranchId = emp.BranchId ?? 1;
                                cutAcc_s.NameAr = "حساب السلف للموظف" + "  " + emp.EmployeeNameAr;
                                cutAcc_s.NameEn = emp.EmployeeNameEn + " " + "Employee Loan Account";
                            }

                            var cutAcc_c = _accountsRepository.GetById((int)empUpdated.AccountIDs_Custody);
                            if (cutAcc_c != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc_c.BranchId = emp.BranchId ?? 1;
                                cutAcc_c.NameAr = "حساب عهد للموظف" + "  " + emp.EmployeeNameAr;
                                cutAcc_c.NameEn = emp.EmployeeNameEn + " " + "Employee Custody Account";
                            }
                        }
                        else
                        {

                        }

                        /////////////////////////////////////////////////
                        if (empUpdated.BranchId != emp.BranchId)
                        {
                            var cutAcc = _accountsRepository.GetById((int)empUpdated.AccountId);
                            if (cutAcc != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc.BranchId = emp.BranchId ?? 1;
                                
                            }

                            var cutAcc_s = _accountsRepository.GetById((int)empUpdated.AccountIDs);
                            if (cutAcc_s != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc_s.BranchId = emp.BranchId ?? 1;
                            }

                            var cutAcc_c = _accountsRepository.GetById((int)empUpdated.AccountIDs_Custody);
                            if (cutAcc_c != null)
                            {
                                //customerAccCode = Convert.ToInt32(cutAcc.Code);
                                cutAcc_c.BranchId = emp.BranchId ?? 1;
                            }
                        }
                        ///////////////////////////////////////////
                        empUpdated.EmployeeNameAr = emp.EmployeeNameAr;
                        empUpdated.EmployeeNameEn = emp.EmployeeNameEn;

                        empUpdated.BranchId = emp.BranchId;
                        empUpdated.JobId = emp.JobId;


                        SaveNationalDoc.NationalId = emp.NationalId;
                        SaveNationalDoc.NationalIdSource = emp.NationalIdSource;
                        SaveNationalDoc.NationalIdDate = emp.NationalIdDate;
                        SaveNationalDoc.NationalIdHijriDate = emp.NationalIdHijriDate;
                        SaveNationalDoc.NationalIdEndDate = emp.NationalIdEndDate;
                      
                            SaveNationalDoc.NationalIdEndHijriDate = emp.NationalIdEndHijriDate;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل موظف رقم " + emp.EmployeeId;
                   _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully, ReturnedParm = emp.EmployeeId, ReturnedStr = EmpAccCode };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ موظف";
               _SystemAction.SaveAction("SaveEmployee", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveEmpBouns(int EmpId, int Bouns, int User, string Lang, int BranchId)
        {
            try
            {
                var Emp = _employeeRepository.GetMatching(x => x.EmployeeId == EmpId && !x.IsDeleted && !string.IsNullOrEmpty(x.WorkStartDate)).FirstOrDefault();
                if (Emp != null)
                {

                    var Payroll = _payrollMarchesRepository.GetPayrollMarches(EmpId, DateTime.Now.Month).Result;

                    if (Payroll != null && Payroll.PostDate.HasValue)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في تعديل مسير الرواتب (العلاوة)) ";
                       _SystemAction.SaveAction("SaveEmpBouns", "EmployeeService", 2, Resources.Posted_payroll_cannot_be_modified, "", "", ActionDate, User, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Posted_payroll_cannot_be_modified };
                    }

                    Emp.Bonus = Bouns;
                    Emp.UpdateDate = DateTime.Now;
                    Emp.UpdateUser = User;

                    //Payroll.Bonus = Bouns;
                    //Payroll.UpdatedDate = DateTime.Now;
                    //Payroll.UpdateUser = User;

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "اضافة بونص موظف جديد";
                   _SystemAction.SaveAction("SaveEmpBouns", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, User, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ بونص موظف";
                   _SystemAction.SaveAction("SaveEmpBouns", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ بونص موظف";
               _SystemAction.SaveAction("SaveEmpBouns", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteEmplocation(int EmpId, int LocationId, int User, string Lang, int BranchId)
        {
            try
            {
                var Emploc = _TaamerProContext.EmpLocations.Where(x => x.EmpId == EmpId && x.LocationId== LocationId && x.IsDeleted==false).FirstOrDefault();
                if (Emploc != null)
                {
                    Emploc.IsDeleted = true;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "حذف موقع موظف رقم "+ EmpId ;
                    _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, User, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحذف بنجاح" };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حذف موقع موظف";
                    _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحذف" };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف موقع موظف";
                _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحذف" };
            }
        }
        public GeneralMessage AllowEmployeesites(int EmpId, bool Check, int Type, int User, string Lang, int BranchId)
        {
            var Txtstr = "السماح";
            if (Check == false) Txtstr = "عدم السماح";
            try
            {

                var Emp = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmpId  && x.IsDeleted == false).FirstOrDefault();
                if (Emp != null)
                {
                    if(Type==1)
                    {
                        Emp.allowallsite = Check;
                    }
                    else
                    {
                        Emp.allowoutsidesite = Check;
                    }
                    
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = Txtstr + " بتسجيل الحضور والانصراف موظف رقم " + EmpId;
                    _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, User, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ بنجاح" };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في "+ Txtstr + " بتسجيل الحضور والانصراف موظف";
                    _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ" };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في " + Txtstr + " بتسجيل الحضور والانصراف موظف";
                _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ" };
            }
        }
        public GeneralMessage AllowEmployeesitesList(List<int> EmpIds, bool Check, int Type, int User, string Lang, int BranchId)
        {
            var Txtstr = "السماح";
            if (Check == false) Txtstr = "عدم السماح";
            if (EmpIds.Count()>0)
            {
                foreach (var EmpId in EmpIds)
                {

                    try
                    {

                        var Emp = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmpId && x.IsDeleted == false).FirstOrDefault();
                        if (Emp != null)
                        {
                            if (Type == 1)
                            {
                                Emp.allowallsite = Check;
                            }
                            else
                            {
                                Emp.allowoutsidesite = Check;
                            }


                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = "فشل في " + Txtstr + " بتسجيل الحضور والانصراف موظف";
                            _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "فشل في الحفظ" };

                        }
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في " + Txtstr + " بتسجيل الحضور والانصراف موظف";
                        _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ" };
                    }
                }
                _TaamerProContext.SaveChanges();
            }
            //-----------------------------------------------------------------------------------------------------------------
            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string ActionNote2 = Txtstr + " بتسجيل الحضور والانصراف موظف  ";
            _SystemAction.SaveAction("DeleteEmplocation", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, User, BranchId, ActionNote2, 1);
            //-----------------------------------------------------------------------------------------------------------------
            return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ بنجاح" };

        }

        public GeneralMessage ConvertEmplocation(int EmpId, int oldLocationId, int newLocationId, int User, string Lang, int BranchId)
        {
            try
            {
                var Emploc = _TaamerProContext.EmpLocations.Where(x => x.EmpId == EmpId && x.LocationId == oldLocationId && x.IsDeleted == false).FirstOrDefault();
                if (Emploc != null)
                {

                    var Emploc2 = _TaamerProContext.EmpLocations.Where(x => x.EmpId == EmpId && x.LocationId == newLocationId && x.IsDeleted == false).ToList();
                    if (Emploc2.Count() > 0)
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "هذا الموظف موجود من قبل علي هذا الموقع" };

                    }

                    Emploc.LocationId = newLocationId;

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "تغيير موقع موظف رقم " + EmpId;
                    _SystemAction.SaveAction("ConvertEmplocation", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, User, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم التغيير بنجاح" };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في تغيير موقع موظف";
                    _SystemAction.SaveAction("ConvertEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في التغيير" };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير موقع موظف";
                _SystemAction.SaveAction("ConvertEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, User, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في التغيير" };
            }
        }
        public GeneralMessage SaveEmplocation(int EmpId, int LocationId, int UserId, string Lang, int BranchId)
        {
            try
            {

                var Emploc = _TaamerProContext.EmpLocations.Where(x => x.EmpId == EmpId && x.LocationId == LocationId && x.IsDeleted == false).ToList();
                if (Emploc.Count()>0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "هذا الموظف موجود من قبل علي هذا الموقع" };

                }

                EmpLocations empLocations = new EmpLocations();
                empLocations.EmpLocationId = 0;
                empLocations.EmpId = EmpId;
                empLocations.LocationId = LocationId;
                _TaamerProContext.EmpLocations.Add(empLocations);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ موظف علي موقع";
                _SystemAction.SaveAction("SaveEmplocation", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم حفظ موظف علي موقع بنجاح" };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الموظف علي الموقع";
                _SystemAction.SaveAction("SaveEmplocation", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveEmplocationList(List<int> EmpList, int LocationId, int UserId, string Lang, int BranchId)
        {
            try
            {

                foreach (var EmpId in EmpList)
                {
                    var Emploc = _TaamerProContext.EmpLocations.Where(x => x.EmpId == EmpId && x.LocationId == LocationId && x.IsDeleted == false).ToList();
                    if (Emploc.Count() > 0)
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "يوجد موظف موجود من قبل علي هذا الموقع" };

                    }
                    EmpLocations empLocations = new EmpLocations();
                    empLocations.EmpLocationId = 0;
                    empLocations.EmpId = EmpId;
                    empLocations.LocationId = LocationId;
                    _TaamerProContext.EmpLocations.Add(empLocations);
                }
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "حفظ موظفيين علي موقع";
                _SystemAction.SaveAction("SaveEmplocationList", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم حفظ موظفيين علي موقع بنجاح" };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الموظفيين علي الموقع";
                _SystemAction.SaveAction("SaveEmplocationList", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage CheckifCodeIsExist(string empCode, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _employeeRepository.GetMatching(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate) && s.EmployeeNo == empCode).FirstOrDefault();
                if (codeExist != null)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "رقم الموظف موجود من قبل" };
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "رقم وظيفي ليس موجود من قبل" };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في مراجعة الرقم الوظيفي" };
            }
        }

        public async Task<int> GenerateNextEmpNumber(int BranchId)
        {
            return await _employeeRepository.GenerateNextEmpNumber(BranchId);
        }
        public GeneralMessage SaveOfficialDocuments(Employees OffDoc, int UserId, int BranchId, string lang)
        {
            try
            {
                var EmpPass = _employeeRepository.SearchEmployeesOfPass(OffDoc.PassportNo, lang, BranchId, OffDoc.EmployeeId).Result;
                var contractnum = _TaamerProContext.EmpContract.Where(x => x.IsDeleted == false && x.ContractCode == OffDoc.ContractNo && x.EmpId !=OffDoc.EmployeeId).FirstOrDefault();
                if(contractnum !=null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حفظ الاوراق الرسمية للموظف";
                    _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, "قم بتغيير رقم العقد لانه موجود بالفعل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "قم بتغيير رقم العقد لانه موجود بالفعل" };

                }
                //var EmpNational = _employeeRepository.SearchEmployeesOfNational(OffDoc.NationalId, lang, BranchId, OffDoc.EmployeeId);
                var SaveOffDoc = _employeeRepository.GetById(OffDoc.EmployeeId);
                if (EmpPass != 0 && !string.IsNullOrEmpty(OffDoc.PassportNo))
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حفظ الاوراق الرسمية للموظف";
                   _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, "قم بتغيير رقم جواز السفر لانه موجود بالفعل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "قم بتغيير رقم جواز السفر لانه موجود بالفعل" };
                }
                //else if (EmpNational != 0)
                //{
                //    return new GeneralMessage { Result = false, Message = "قم بتغيير رقم الاقامه لانه موجود بالفعل" };
                //}
                else
                {
                    if (SaveOffDoc != null)
                    {
                        if (SaveOffDoc.Bonus != OffDoc.Bonus)
                        {
                            if ((SaveOffDoc.Bonus != null && OffDoc.Bonus != 0) && (SaveOffDoc.Bonus != 0 && OffDoc.Bonus != null))
                            {

                                var Payroll = _payrollMarchesRepository.GetPayrollMarches(OffDoc.EmployeeId, DateTime.Now.Month).Result;
                                if (Payroll != null && Payroll.PostDate.HasValue)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote3 = "فشل في تعديل مسير الرواتب (البدلات)) ";
                                   _SystemAction.SaveAction("UpdatePayrollWithAllowances", "AllowanceService", 2, Resources.Posted_payroll_cannot_be_modified, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Posted_payroll_cannot_be_modified };
                                }
                            }
                        }
                        //SaveOffDoc.NationalId = OffDoc.NationalId;
                        //SaveOffDoc.NationalIdSource = OffDoc.NationalIdSource;
                        //SaveOffDoc.NationalIdDate = OffDoc.NationalIdDate;
                        //SaveOffDoc.NationalIdHijriDate = OffDoc.NationalIdHijriDate;
                        //SaveOffDoc.NationalIdEndDate = OffDoc.NationalIdEndDate;
                        //SaveOffDoc.NationalIdEndHijriDate = OffDoc.NationalIdEndHijriDate;
                        if (SaveOffDoc.ContractEndDate != OffDoc.ContractEndDate || SaveOffDoc.ContractStartDate != OffDoc.ContractStartDate)
                        {
                            SaveOffDoc.IsRememberContract = null;
                            SaveOffDoc.RememberDateContract = null;

                        }
                        SaveOffDoc.PassportNo = OffDoc.PassportNo;
                        SaveOffDoc.PassportSource = OffDoc.PassportSource;
                        SaveOffDoc.PassportNoDate = OffDoc.PassportNoDate;
                        SaveOffDoc.PassportNoHijriDate = OffDoc.PassportNoHijriDate;
                        SaveOffDoc.PassportEndDate = OffDoc.PassportEndDate;
                        SaveOffDoc.PassportEndHijriDate = OffDoc.PassportEndHijriDate;
                        SaveOffDoc.ContractNo = OffDoc.ContractNo;
                        SaveOffDoc.ContractSource = OffDoc.ContractSource;
                        if (OffDoc.BeginWork == "1")
                        {
                            SaveOffDoc.WorkStartDate = OffDoc.ContractStartDate;
                            var employ = _TaamerProContext.Employees.Where(x => x.EmployeeId == SaveOffDoc.EmployeeId).FirstOrDefault();
                            if(employ != null && employ.Email != null)
                            {
                             var issent= sendemployeemail(employ);
                            }
                        }
                        SaveOffDoc.ContractStartHijriDate = OffDoc.ContractStartHijriDate;
                        SaveOffDoc.ContractStartDate = OffDoc.ContractStartDate;
                        SaveOffDoc.ContractStartHijriDate = OffDoc.ContractStartHijriDate;
                        SaveOffDoc.ContractEndDate = OffDoc.ContractEndDate;
                        SaveOffDoc.ContractEndHijriDate = OffDoc.ContractEndHijriDate;

                        SaveOffDoc.MedicalNo = OffDoc.MedicalNo;
                        SaveOffDoc.MedicalSource = OffDoc.MedicalSource;
                        SaveOffDoc.MedicalStartDate = OffDoc.MedicalStartDate;
                        SaveOffDoc.MedicalStartHijriDate = OffDoc.MedicalStartHijriDate;
                        SaveOffDoc.MedicalEndDate = OffDoc.MedicalEndDate;
                        SaveOffDoc.MedicalEndHijriDate = OffDoc.MedicalEndHijriDate;

                        SaveOffDoc.LicenceNo = OffDoc.LicenceNo;
                        SaveOffDoc.LiscenseSourceId = OffDoc.LiscenseSourceId;
                        SaveOffDoc.LicenceStartDate = OffDoc.LicenceStartDate;
                        SaveOffDoc.LicenceStartHijriDate = OffDoc.LicenceStartHijriDate;
                        SaveOffDoc.LicenceEndDate = OffDoc.LicenceEndDate;
                        SaveOffDoc.LicenceEndHijriDate = OffDoc.LicenceEndHijriDate;
                        SaveOffDoc.DawamId = OffDoc.DawamId;
                        SaveOffDoc.TimeDurationLate = OffDoc.TimeDurationLate;
                        SaveOffDoc.LogoutDuration = OffDoc.LogoutDuration;
                        SaveOffDoc.AfterLogoutTime = OffDoc.AfterLogoutTime;
                        SaveOffDoc.Salary = OffDoc.Salary;
                        SaveOffDoc.Bonus = OffDoc.Bonus;
                        SaveOffDoc.VacationsCount = OffDoc.VacationsCount;
                        SaveOffDoc.VacationEndCount = OffDoc.VacationEndCount;
                        SaveOffDoc.EarlyLogin = OffDoc.EarlyLogin;
                        SaveOffDoc.Allowances = OffDoc.Allowances;
                        SaveOffDoc.OtherAllownces=OffDoc.OtherAllownces;
                        SaveOffDoc.AttendenceLocationId = OffDoc.AttendenceLocationId;
                        SaveOffDoc.allowoutsidesite = OffDoc.allowoutsidesite??false;
                        SaveOffDoc.allowallsite = OffDoc.allowallsite??false;
                        SaveOffDoc.DailyWorkinghours = OffDoc.DailyWorkinghours;
                        SaveOffDoc.EmpHourlyCost = OffDoc.EmpHourlyCost;

                  
                        //SaveOffDoc.WorkEndDate = OffDoc.WorkEndDate;
                        //SaveOffDoc.WorkEndHijriDate = OffDoc.WorkEndHijriDate;

                        if (SaveOffDoc.QuaContract != null && SaveOffDoc.ContractStartDate != null)
                        {
                            EmpContract? Updated = _TaamerProContext.EmpContract.Where(s => s.EmpId == SaveOffDoc.EmployeeId && s.IsDeleted == false).FirstOrDefault();
                            if (Updated != null)
                            {
                                Updated.StartDatetxt = SaveOffDoc.ContractStartDate;
                                Updated.EndDatetxt = SaveOffDoc.ContractEndDate;
                                Updated.ContractCode = SaveOffDoc.ContractNo;
                                if (SaveOffDoc.Salary != null)
                                {
                                    Updated.FreelanceAmount = SaveOffDoc.Salary;
                                }
                                Updated.Dailyworkinghours =Convert.ToInt32(SaveOffDoc.DailyWorkinghours);
                                Updated.EmpHourlyCost = SaveOffDoc.EmpHourlyCost;
                            }
                            //var employ = _TaamerProContext.Employees.Where(x => x.EmployeeId == SaveOffDoc.EmployeeId).FirstOrDefault();
                            //if (employ != null && employ.Email !=null)
                            //{
                            //    var issent = sendemployeemail(employ);
                            //}

                        }

                        SaveOffDoc.BankId = OffDoc.BankId;
                        SaveOffDoc.BankCardNo = OffDoc.BankCardNo;
                        if (SaveOffDoc.NationalityId == 3)
                        {
                            SaveOffDoc.Taamen = "9.75";
                        }

                        SaveOffDoc.AddUser = UserId;
                        SaveOffDoc.AddDate = DateTime.Now;

                        var emp = _employeeRepository.GetById(SaveOffDoc.EmployeeId);
                        if (emp.UserId != null)
                        {
                            var user = _UserRepository.GetMatching(s => s.IsDeleted == false && s.UserId == emp.UserId);
                            if (user.Count() > 0)
                            {
                                user.FirstOrDefault().TimeId = OffDoc.DawamId;
                            }
                        }

                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة اوراق رسمية للموظف";
               _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الاوراق الرسمية للموظف";
               _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public bool sendemployeemail(Employees EmployeeUpdated)
        {
            bool IsSent=false;
            string OrgName = _organizationsService.GetBranchOrganization().Result.NameAr;
            string DepartmentNameAr = "";
            Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == EmployeeUpdated.DepartmentId).FirstOrDefault();
            if (DepName != null)
            {
                DepartmentNameAr = DepName.DepartmentNameAr;
            }

            //string BranchName = _BranchRepository.GetById(EmployeeUpdated.BranchId).NameAr;
            string NameAr = "";
            Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
            var job = _TaamerProContext.Job.FirstOrDefault(x => x.JobId == EmployeeUpdated.JobId);
            if (BranchName != null)
            {
                NameAr = BranchName.NameAr;
            }
            var directmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmployeeUpdated.DirectManager).FirstOrDefault();

            var directmanagername = directmanager == null ? "" : directmanager.EmployeeNameAr;
            var htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
    <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
    <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
    <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
    <h4> عزيزي الموظف " + EmployeeUpdated.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > يسر " + OrgName + "  ان يعبر عن سعادته بانضمامكم اليه ونسال الله لك التوفيق</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + EmployeeUpdated.EmployeeNameAr + @"</td> </tr> <tr> <td>   الوظيفه</td><td>" + job.JobNameAr + @"</td> </tr><tr> <td>   القسم  </td> <td>" + DepartmentNameAr + @"</td>
     </tr> <tr> <td>   الفرع</td> <td>" + NameAr + @"</td> </tr> <tr> <td>  تاريخ المباشرة  </td> <td>" + EmployeeUpdated.WorkStartDate + @"</td> </tr>  <tr> <td>   المدير المباشر  </td> <td>" + directmanagername + @"</td> </tr></table><h4>صورة مع التحية للمدير المباشر للموظف</h4> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
    </div> </div></div></div></body></html> ";
            var Note_Cinfig = GetNotificationRecipients(NotificationCode.HR_EmployeeStart, EmployeeUpdated.EmployeeId);
            var desc = Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
            if (Note_Cinfig.Description != null && Note_Cinfig.Description != "")
                desc = Note_Cinfig.Description;

            if (Note_Cinfig.Users != null && Note_Cinfig.Users.Count() > 0)
            {
                foreach (var usr in Note_Cinfig.Users)
                {
                    string NotStr = "تم انضمام الموظف " + EmployeeUpdated.EmployeeNameAr + " إلى فريق " + OrgName + ", الوظيفة: " + job.JobNameAr + " قسم : " + DepartmentNameAr + " فرع: " + NameAr;
                    Notification UserNotification = new Notification();
                    UserNotification.ReceiveUserId = usr;// EmployeeUpdated.UserId.Value;
                    UserNotification.Name = desc;// Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                    UserNotification.SendUserId = 1;
                    UserNotification.Type = 1; // notification
                    UserNotification.Description = NotStr;
                    UserNotification.AllUsers = false;
                    UserNotification.SendDate = DateTime.Now;
                    UserNotification.ProjectId = 0;
                    UserNotification.TaskId = 0;
                    UserNotification.IsHidden = false;
                    UserNotification.AddUser = EmployeeUpdated.UserId.Value;
                    UserNotification.AddDate = DateTime.Now;
                    UserNotification.IsRead = false;
                    _TaamerProContext.Notification.Add(UserNotification);
                    _TaamerProContext.SaveChanges();
                    _notificationService.sendmobilenotification(usr, desc, NotStr);
                    IsSent = _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, 0, usr, desc, htmlBody, true);

                }
            }
            else
            {
                //Mail
                if (EmployeeUpdated.Email != null && EmployeeUpdated.Email != "")
                {
                    IsSent = _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, 0, 0, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), htmlBody, true, EmployeeUpdated.Email);
                }
                if (directmanager != null)
                {
                    _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, 0, 0, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), htmlBody, true, directmanager.Email);

                }
                string NotStr = "تم انضمام الموظف " + EmployeeUpdated.EmployeeNameAr + " إلى فريق " + OrgName + ", الوظيفة: " + job.JobNameAr + " قسم : " + DepartmentNameAr + " فرع: " + NameAr;
                Notification UserNotification = new Notification();
                UserNotification.ReceiveUserId = EmployeeUpdated.UserId.Value;
                UserNotification.Name = Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar"));
                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                UserNotification.SendUserId = 1;
                UserNotification.Type = 1; // notification
                UserNotification.Description = NotStr;
                UserNotification.AllUsers = false;
                UserNotification.SendDate = DateTime.Now;
                UserNotification.ProjectId = 0;
                UserNotification.TaskId = 0;
                UserNotification.IsHidden = false;
                UserNotification.AddUser = EmployeeUpdated.UserId.Value;
                UserNotification.AddDate = DateTime.Now;
                UserNotification.IsRead = false;
                _TaamerProContext.Notification.Add(UserNotification);
                _TaamerProContext.SaveChanges();
                if (directmanager != null)
                {
                    var Not_directmanager = new Notification();
                    Not_directmanager = UserNotification;
                    Not_directmanager.ReceiveUserId = directmanager.UserId.Value;
                    Not_directmanager.NotificationId = 0;
                    _TaamerProContext.Notification.Add(Not_directmanager);
                    _TaamerProContext.SaveChanges();
                }
                _notificationService.sendmobilenotification(EmployeeUpdated.UserId.Value, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), NotStr);
                if (directmanager != null)
                {
                    _notificationService.sendmobilenotification(directmanager.UserId.Value, Resources.ResourceManager.GetString("Con_StartWork", CultureInfo.CreateSpecificCulture("ar")), NotStr);
                }
            }
            return IsSent;
        }



        public bool sendemployeemailNotification(Employees EmployeeUpdated,int type,string value)
        {
            bool IsSent = false;
            string OrgName = _organizationsService.GetBranchOrganization().Result.NameAr;
            string DepartmentNameAr = "";
            Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == EmployeeUpdated.DepartmentId).FirstOrDefault();
            if (DepName != null)
            {
                DepartmentNameAr = DepName.DepartmentNameAr;
            }

            string NameAr = "";
            Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == EmployeeUpdated.BranchId).FirstOrDefault();
            var job = _TaamerProContext.Job.FirstOrDefault(x => x.JobId == EmployeeUpdated.JobId);
            if (BranchName != null)
            {
                NameAr = BranchName.NameAr;
            }
            var directmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmployeeUpdated.DirectManager).FirstOrDefault();

            var directmanagername = directmanager == null ? "" : directmanager.EmployeeNameAr;
            var label1 = type == 1 ? " الفرع السابق" : "المسمي الوظيفي السابق" ;
            var label2 = type == 1 ? " الفرع الجديد" : "المسمي الوظيفي الجديد";
            var value2 = type == 1 ? NameAr : job.JobNameAr;
            var title2 = type == 1 ? " نفيدكم علما بانه تم نقلك الي فرع اخر حسب الجدول التالي " :
                "نفيدكم علما بان مسماكم الوظيفي قد تغير حسب الجدول التالي";
            var htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                    <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                    <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                    <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                    <h4> عزيزي الموظف " + EmployeeUpdated.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > "+ title2 + "</h3><table align = 'center' border = '1' ><tr> <td>  "+ label1+ "</td><td>" + value + @"</td> </tr> <tr> 
                     <td>   "+ label2 + "</td><td>" + value2 + @"</td> </tr> <tr> <td>   المدير المباشر  </td> <td>" + directmanagername + @"</td> </tr>  <tr> <td>تاريخ العملية</td> <td>" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")) + @"</td> </tr></table><h4>صورة مع التحية للمدير المباشر للموظف</h4> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
                    </div> </div></div></div></body></html> ";

            //Mail
            var label1_not = type == 1 ? " من فرع" : " من مسمي الوظيفي ";
            var label2_not = type == 1 ? "  الي فرع" : " الي مسمي  وظيفي اخر ";
            var title = "نقل موظف " + label1_not + " " + label2_not;

            var config =type==1?
                GetNotificationRecipients(Models.Enums.NotificationCode.HR_TransferBranch, EmployeeUpdated.EmployeeId)
                :GetNotificationRecipients(Models.Enums.NotificationCode.HR_ChangeJobTitle, EmployeeUpdated.EmployeeId);
            
            if (config.Description != null && config.Description != "")
                title = config.Description;

            if (config.Users != null && config.Users.Count() > 0)
            {
                foreach (var usr in config.Users)
                {
                    try
                    {
                        IsSent = _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId,usr, usr, title, htmlBody, true);
                        string NotStr = "تم نقل الموظف  " + EmployeeUpdated.EmployeeNameAr + " " + label1_not + " " + value + " " + label2_not + " " + value2;

                        Notification UserNotification = new Notification();
                        UserNotification.ReceiveUserId = usr;
                        UserNotification.Name = title;
                        UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                        UserNotification.SendUserId = 1;
                        UserNotification.Type = 1; // notification
                        UserNotification.Description = NotStr;
                        UserNotification.AllUsers = false;
                        UserNotification.SendDate = DateTime.Now;
                        UserNotification.ProjectId = 0;
                        UserNotification.TaskId = 0;
                        UserNotification.IsHidden = false;
                        UserNotification.AddUser = EmployeeUpdated.UserId.Value;
                        UserNotification.AddDate = DateTime.Now;
                        UserNotification.IsRead = false;
                        _TaamerProContext.Notification.Add(UserNotification);
                        _TaamerProContext.SaveChanges();
                        _notificationService.sendmobilenotification(usr, title, NotStr);

                    }
                    catch (Exception ex)
                    {

                    }


                }
            }
            else
            {



                if (EmployeeUpdated.Email != null && EmployeeUpdated.Email != "")
                {
                    try
                    {
                        IsSent = _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, EmployeeUpdated.UserId.Value, EmployeeUpdated.UserId.Value, title, htmlBody, true, EmployeeUpdated.Email);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (directmanager != null && directmanager.Email != null)
                {
                    try
                    {
                        _customerMailService.SendMail_SysNotification((int)EmployeeUpdated.BranchId, directmanager.UserId.Value, directmanager.UserId.Value, title, htmlBody, true, directmanager?.Email);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                string NotStr = "تم نقل الموظف  " + EmployeeUpdated.EmployeeNameAr + " " + label1_not + " " + value + " " + label2_not + " " + value2;

                Notification UserNotification = new Notification();
                UserNotification.ReceiveUserId = EmployeeUpdated.UserId.Value;
                UserNotification.Name = title;
                UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                UserNotification.SendUserId = 1;
                UserNotification.Type = 1; // notification
                UserNotification.Description = NotStr;
                UserNotification.AllUsers = false;
                UserNotification.SendDate = DateTime.Now;
                UserNotification.ProjectId = 0;
                UserNotification.TaskId = 0;
                UserNotification.IsHidden = false;
                UserNotification.AddUser = EmployeeUpdated.UserId.Value;
                UserNotification.AddDate = DateTime.Now;
                UserNotification.IsRead = false;
                _TaamerProContext.Notification.Add(UserNotification);
                _TaamerProContext.SaveChanges();
                if (directmanager != null)
                {
                    var Not_directmanager = new Notification();
                    Not_directmanager = UserNotification;
                    Not_directmanager.ReceiveUserId = directmanager.UserId.Value;
                    Not_directmanager.NotificationId = 0;
                    _TaamerProContext.Notification.Add(Not_directmanager);
                    _TaamerProContext.SaveChanges();
                }

                _notificationService.sendmobilenotification(EmployeeUpdated.UserId.Value, title, NotStr);
                if (directmanager != null)
                {
                    _notificationService.sendmobilenotification(directmanager.UserId.Value, title, NotStr);
                }
            }
            return IsSent;
        }
        public  GeneralMessage Savequacontract(int EmpId,string quacontract, int UserId, int BranchId,int yearid,string lang)
        {
            try
            {
                var empqua = _employeeRepository.GetById(EmpId);
        
               
                    if (empqua != null)
                    {
                    empqua.QuaContract = quacontract;
                   
                    //var contractno = _empContractRepository.GenerateNextEmpContractNumber(BranchId).Result;
                    //empqua.ContractNo = contractno.ToString();
                    _TaamerProContext.SaveChanges();


                    EmpContract contracts = new EmpContract()
                    {
                        EmpId = EmpId,
                        AddDate = DateTime.Now,
                        AddUser = UserId,
                        BranchId = BranchId,
                        StartDatetxt = empqua.ContractStartDate,
                        EndDatetxt = empqua.ContractEndDate,
                        StartWorkDate = empqua.ContractStartDate,
                        FreelanceAmount = empqua.Salary,
                        ContractCode = empqua.ContractNo.ToString(),
                        ContractSource = 1,
                        Durationofannualleave=empqua.VacationEndCount.Value,
                        

                    };
                   var res= SaveEmpContract(contracts,UserId, BranchId, yearid, lang);


                      

                    }
                
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة اوراق رسمية للموظف";
                _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedParm= EmpId };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الاوراق الرسمية للموظف";
                _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveEmpContract(EmpContract data, int UserId, int BranchId, int? Year, string lang)
        {
            int Bra = Convert.ToInt32(_employeeRepository.GetEmployeeById(data.EmpId, lang).Result.BranchId);
            int Nat = Convert.ToInt32(_employeeRepository.GetEmployeeById(data.EmpId, lang).Result.NationalityId);
            int? branchid = _BranchesRepository.GetAllBranches(lang).Result.Where(w => w.BranchId == Bra).Select(s => s.BranchId).FirstOrDefault();
            int? NationalityId = _NationalityRepository.GetAllNationalitiesById(Nat).Result.Select(s => s.NationalityId).FirstOrDefault();
            int? orgId = _BranchesRepository.GetAllBranches(lang).Result.Where(w => w.BranchId == Bra).Select(s => s.OrganizationId).FirstOrDefault();
            if (branchid <= 0)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.MC_EnterBranchForEachEmp, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.MC_EnterBranchForEachEmp };
            }
            if (NationalityId <= 0)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.MC_EnternationalityForEachEmp, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.MC_EnternationalityForEachEmp };
            }

            var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && data.ContractId == 0 && s.EmpId == data.EmpId).FirstOrDefault();
            if (EmpContract != null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1,  Resources.MC_EmpContractCodeExist, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.MC_EmpContractAlreadyExists };
            }

            //  var codeExist = _empContractRepository.GetMatching(s => s.IsDeleted == false && s.ContractId != data.ContractId && s.ContractCode == data.ContractCode).FirstOrDefault();
            var codeExist = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId != data.ContractId && s.ContractCode == data.ContractCode).FirstOrDefault();


            if (codeExist != null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.MC_EmpContractCodeExist, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.MC_EmpContractCodeExist };
            }

            try
            {

                if (Year == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ عقد موظف";
                    _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.choosefinYear, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.choosefinYear };
                }
                if (data.ContractId == 0)
                {

                    data.AddUser = UserId;
                    data.AddDate = DateTime.Now;
                    data.IsDeleted = false;
                    data.BranchId = Convert.ToInt32(branchid);
                    data.NationalityId = Convert.ToInt32(NationalityId);
                    data.OrgId = Convert.ToInt32(orgId);
                    _TaamerProContext.EmpContract.Add(data);
                    //-----------------------------------------------------
                    // var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
                    Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == data.EmpId).FirstOrDefault();

                    EmployeeUpdated.ContractNo = data.ContractCode;
                    EmployeeUpdated.ContractStartDate = data.StartDatetxt;
                    EmployeeUpdated.ContractEndDate = data.EndDatetxt;
                    EmployeeUpdated.Salary = data.FreelanceAmount;
                    //EmployeeUpdated.VacationsCount = data.Durationofannualleave;
                    //-------------------------------------------------------
                    try
                    {
                        var ObjList = new List<object>();
                        foreach (var item in data.EmpContractDetails.ToList())
                        {

                            ObjList.Add(new { item.ContractId });
                            item.ContractId = data.ContractId;
                            item.SerialId = item.SerialId;
                            item.Clause = item.Clause;
                            item.AddDate = DateTime.Now;
                            item.AddUser = UserId;
                            item.IsDeleted = false;
                            _TaamerProContext.EmpContractDetail.Add(item);
                        }
                    }
                    catch
                    {
                        //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        //string ActionNote = "فشل في حفظ عقد موظف";
                        // _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        ////-----------------------------------------------------------------------------------------------------------------

                        //return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
                    }

                    _TaamerProContext.SaveChanges();
                    //To Make or update payrolls
                    //_employeesService.GetAllEmployeesSearch( new EmployeesVM() { IsSearch= true, MonthNo = DateTime.Now.Month},lang, UserId, branchid.Value, Con);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عقد موظف جديد";
                    _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };


                }
                else
                {
                    // var Updated = _empContractRepository.GetById(data.ContractId);
                    EmpContract? Updated = _TaamerProContext.EmpContract.Where(s => s.ContractId == data.ContractId).FirstOrDefault();

                    if (Updated != null)
                    {
                        Updated.CompanyRepresentativeId = data.CompanyRepresentativeId;
                        Updated.Compensation = data.Compensation;
                        Updated.CompensationBothParty = data.CompensationBothParty;
                        Updated.ContTypeId = data.ContTypeId;
                        Updated.ContDuration = data.ContDuration;
                        Updated.ContractTerminationNotice = data.ContractTerminationNotice;
                        Updated.Dailyworkinghours = data.Dailyworkinghours;
                        Updated.Durationofannualleave = data.Durationofannualleave;

                        Updated.EndDatetxt = data.EndDatetxt;
                        Updated.Firstpartycompensation = data.Firstpartycompensation;
                        Updated.FreelanceAmount = data.FreelanceAmount;
                        Updated.HijriDate = data.HijriDate;
                        Updated.NotTodivulgeSecrets = data.NotTodivulgeSecrets;
                        Updated.NotTodivulgeSecretsDuration = data.NotTodivulgeSecretsDuration;
                        Updated.Paycase = data.Paycase;
                        Updated.PerSe = data.PerSe;
                        Updated.ProbationDuration = data.ProbationDuration;
                        Updated.ProbationTypeId = data.ProbationTypeId;
                        Updated.Restrictedmode = data.Restrictedmode;
                        Updated.RestrictionDuration = data.RestrictionDuration;
                        Updated.Secondpartycompensation = data.Secondpartycompensation;
                        Updated.SecretsIdentifyplaces = data.SecretsIdentifyplaces;
                        Updated.SecretsWithregardtowork = data.SecretsWithregardtowork;
                        Updated.StartDatetxt = data.StartDatetxt;
                        Updated.Withregardtowork = data.Withregardtowork;
                        Updated.Workingdaysperweek = data.Workingdaysperweek;
                        Updated.Workinghoursperweek = data.Workinghoursperweek;
                        Updated.DailyEmpCost = data.DailyEmpCost;

                        Updated.UpdateDate = DateTime.Now;
                        Updated.UpdateUser = UserId;

                        //-----------------------------------------------------
                        //var EmployeeUpdated = _EmpRepository.GetById(data.EmpId);
                        Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == data.EmpId).FirstOrDefault();

                        //if (Updated.EmpId != data.EmpId)
                        //{
                        //    var oldEmp = _EmpRepository.GetById(data.EmpId);
                        //    oldEmp.ContractNo = null;
                        //    oldEmp.ContractStartDate = null;
                        //    oldEmp.ContractEndDate = null;
                        //    oldEmp.Salary = null;
                        //}

                        //Updated.EmpId = data.EmpId;
                        Updated.ContractCode = data.ContractCode;
                        EmployeeUpdated.ContractNo = data.ContractCode;
                        EmployeeUpdated.ContractStartDate = data.StartDatetxt;
                        EmployeeUpdated.ContractEndDate = data.EndDatetxt;
                        EmployeeUpdated.Salary = data.FreelanceAmount;
                        //EmployeeUpdated.VacationsCount = data.Durationofannualleave;
                    }

                    try
                    {
                        //delete existing details 
                        if (Updated.EmpContractDetails != null)
                        {
                            _TaamerProContext.EmpContractDetail.RemoveRange(Updated.EmpContractDetails.ToList());
                        }
                        try
                        {
                            // add new details
                            var ObjList = new List<object>();
                            foreach (var item in data.EmpContractDetails.ToList())
                            {

                                ObjList.Add(new { item.ContractId });
                                item.ContractId = data.ContractId;
                                item.SerialId = item.SerialId;
                                item.Clause = item.Clause;
                                item.AddUser = UserId;
                                item.AddDate = DateTime.Now;
                                item.IsDeleted = false;
                                _TaamerProContext.EmpContractDetail.Add(item);
                            }


                        }
                        catch (Exception ex)
                        {
                            ////-----------------------------------------------------------------------------------------------------------------
                            //string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            //string ActionNote = "فشل في حفظ عقد موظف";
                            // _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            ////-----------------------------------------------------------------------------------------------------------------

                            //return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل عقد موظف رقم " + data.ContractId;
                        _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "فشل في حفظ عقد موظف";
                        _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
                }


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عقد موظف";
                _SystemAction.SaveAction("SaveEmpContract", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }



        }


        public GeneralMessage DeleteEmployee(int EmpId, int UserId, int BranchId)
        {
            try
            {
                Employees emp = _employeeRepository.GetById(EmpId);
                var empcon = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.CompanyRepresentativeId == EmpId);
                if (empcon.Count() > 0)
                {
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = " فشل في حذف موظف رقم " + EmpId; ;
                   _SystemAction.SaveAction("DeleteEmployee", "EmployeeService", 3, Resources.General_DeletedFailed, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.establishment_in_contracts };
                }
                string today = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                if ((!string.IsNullOrEmpty(emp.ContractStartDate) && string.IsNullOrEmpty(emp.ContractEndDate)) || (!string.IsNullOrEmpty(emp.ContractStartDate) && (DateTime.ParseExact(emp.ContractEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(today, "yyyy-MM-dd", CultureInfo.InvariantCulture))))

                //if (!string.IsNullOrEmpty(emp.ContractEndDate) && (DateTime.ParseExact(emp.ContractEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(today, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                {
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = " فشل في حذف موظف رقم " + EmpId; ;
                    _SystemAction.SaveAction("DeleteEmployee", "EmployeeService", 3, Resources.General_DeletedFailed, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.The_employee_cannot_be_deleted_for_contiouns_in_working };

                }




                var EmployeeUpdated = _employeeRepository.GetById(EmpId);
                //check Loans, Tasks and Projects
                if(EmployeeUpdated.Loans != null) { 
                //1: loans
                var loans = EmployeeUpdated.Loans.Where(x => !x.IsDeleted).ToList();
                int LoansCount = 0;
                DateTime Today = DateTime.Now.Date;
                foreach (var loan in loans)
                {
                    if (loan.LoanDetails.Count > 0)
                    {
                        DateTime MaxDate = loan.LoanDetails.Select(x => x.Date.HasValue ? x.Date.Value : DateTime.MinValue).Max();
                        if (MaxDate.Year >= Today.Year && MaxDate.Month >= Today.Month)
                            LoansCount++;
                    }
                }
                if (LoansCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حذف الموظف";
                    _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن فذف الموظف بسبب وجود سلف", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage() {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.contractCannotDeletedDueAdvance };

                }
                }
                //2: Custody
                var CustodyCount = _CustodyRepository.GetMatching(x => !x.IsDeleted && x.EmployeeId == EmployeeUpdated.EmployeeId && x.Status == false).Count();
                if (CustodyCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حذف الموظف";
                    _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, Resources.General_SavedFailed, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = "لا يمكن حذف الموظف بسبب وجود عُهد"
                    };
                }


                // 3: Tasks, projects,workOrders
                if (EmployeeUpdated.UserId.HasValue && EmployeeUpdated.UserId !=0)
                {
                    var vUser = _UserRepository.GetById(EmployeeUpdated.UserId.Value);
                    int UserId2 = vUser.UserId;
                    var UserF = _UserRepository.GetMatching(s => s.IsDeleted == false && s.IsAdmin == true && s.UserId == UserId2);
                    if (UserF != null && UserF.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حذف الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكنك إيقاف حساب الادمن", "", "", ActionDate4, UserId2, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.cannotDeactivateAdminAccount };
                    }

                    var SettingProjUser = _TaamerProContext.Settings.Where(s => s.IsDeleted == false && s.Type == 3 && s.UserId == UserId2);
                    if (SettingProjUser != null && SettingProjUser.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote5 = "فشل في حذف الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "المستخدم موجود علي  سير مشروع لا يمكن إيقاف حسابه", "", "", ActionDate5, UserId2, BranchId, ActionNote5, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.account_cannot_be_suspended };
                    }
                    var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false /*&& s.Project.StopProjectType != 1 */&& s.UserId == UserId2 && s.Type == 3 && s.Status != 4 && s.IsMerig == -1).Where(x => x.Project.StopProjectType != 1);
                    if (userTasks.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote6 = "فشل في حذف الموظف";
                        _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن حذف الموظف بسبب وجود مهام", "", "", ActionDate6, UserId2, BranchId, ActionNote6, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = "لا يمكن حذف الموظف بسبب وجود مهام " //Resources.userHave + userTasks + Resources.userTasks 
                        };
                    }
                    var userProject = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.MangerId == UserId && s.Status != 1).Count();
                    if (userProject > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote7 = "فشل في حذف الموظف";
                       _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن حذف الموظف بسبب وجود مشاريع", "", "", ActionDate7, UserId2, BranchId, ActionNote7, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = "لا يمكن حذف الموظف بسبب وجود مشاريع"
                            //Resources.userHave + userProject + Resources.UserProjects 
                        };
                    }
                    var userWorkOrder = _TaamerProContext.WorkOrders.Where(s => s.IsDeleted == false && (s.ExecutiveEng == UserId2 || s.ResponsibleEng == UserId2) && (s.WOStatus == 1 || s.WOStatus == 2)).Count();
                    if (userWorkOrder > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote8 = "فشل في حذفالموظف";
                       _SystemAction.SaveAction("EndWorkforAnEmployee", "EmpContractService", 1, "لا يمكن حذف الموظف بسبب وجود أوامر عمل", "", "", ActionDate8, UserId2, BranchId, ActionNote8, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ReasonPhrase = "لا يمكن حذفالموظف بسبب وجود أوامر عمل"
                            //Resources.userHave + userWorkOrder + Resources.userWorkOrder 
                        };
                    }


                }
                //emp account

                if (emp.AccountId != null)
                {
                    var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == (emp.AccountId));
                    if (AccTrans != null && AccTrans.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حذف الحساب";
                        _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Cannot_Delete_Emp_Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Cannot_Delete_Emp_Financial_Transactions_Message_Error };
                    }
                    else
                    {
                        Accounts account = _accountsRepository.GetById((int)emp.AccountId);
                        account.IsDeleted = true;
                        account.DeleteDate = DateTime.Now;
                        account.DeleteUser = UserId;
                    }
                }

                if (emp.AccountIDs != null)
                {
                    var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == (emp.AccountIDs));
                    if (AccTrans != null && AccTrans.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حذف الحساب";
                       _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Cannot_Delete_Emp_Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Cannot_Delete_Emp_Financial_Transactions_Message_Error };
                    }
                    else
                    {
                        Accounts loanaccount = _accountsRepository.GetById((int)emp.AccountIDs);
                        loanaccount.IsDeleted = true;
                        loanaccount.DeleteDate = DateTime.Now;
                        loanaccount.DeleteUser = UserId;
                    }
                }


                if (emp.AccountIDs_Custody != null)
                {
                    var AccTrans = _TaamerProContext.Transactions.Where(s => s.IsDeleted == false && s.AccountId == (emp.AccountIDs_Custody));
                    if (AccTrans != null && AccTrans.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حذف الحساب";
                       _SystemAction.SaveAction("DeleteAccount", "AccountsService", 1, Resources.Cannot_Delete_Emp_Financial_Transactions_Message_Error, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Cannot_Delete_Emp_Financial_Transactions_Message_Error };
                    }
                    else
                    {
                        Accounts custoday = _accountsRepository.GetById((int)emp.AccountIDs_Custody);
                        custoday.IsDeleted = true;
                        custoday.DeleteDate = DateTime.Now;
                        custoday.DeleteUser = UserId;
                    }
                }

                emp.IsDeleted = true;
                emp.DeleteDate = DateTime.Now;
                emp.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف موظف رقم " + EmpId;
                _SystemAction.SaveAction("DeleteEmployee", "EmployeeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف موظف رقم " + EmpId; ;
                _SystemAction.SaveAction("DeleteEmployee", "EmployeeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public GeneralMessage RemoveEmployee(int EmpId, int UserId, int BranchId)
        {
            try
            {
                var emp = _TaamerProContext.Employees.Where(x => x.EmployeeId == EmpId).FirstOrDefault();


                _TaamerProContext.Remove(emp);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف موظف رقم " + EmpId;
                _SystemAction.SaveAction("DeleteEmployee", "EmployeeService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف موظف رقم " + EmpId; ;
                _SystemAction.SaveAction("DeleteEmployee", "EmployeeService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public IEnumerable<NodeVM> FillEmployeeSelect(string lang, int BranchId, bool IsNewContract, int? EmpId)
        {
            if (EmpId.HasValue)
                return _employeeRepository.GetAllEmployees(lang, BranchId).Result.Where(x => x.EmployeeId == EmpId.Value || (IsNewContract ? x.ContractNo == null : true)).Select(s => new NodeVM()
                {
                    Id = s.EmployeeId,
                    Name = s.EmployeeName
                });
            else
                return _employeeRepository.GetAllEmployees(lang, BranchId).Result.Where(x => IsNewContract ? x.ContractNo == null : true).Select(s => new NodeVM()
                {
                    Id = s.EmployeeId,
                    Name = s.EmployeeName
                });
        }
        public IEnumerable<object> FillSelectEmployee(string lang, int BranchId)
        {
            return _employeeRepository.FillAllEmployees(lang, BranchId).Result.Select(s => new
            {
                Id = s.EmployeeId,
                Name = s.EmployeeName
            });
        }
        public IEnumerable<object> FillSelectEmployeeWorkers(string lang, int BranchId)
        {
            return _employeeRepository.FillSelectEmployeeWorkers(lang, BranchId).Result.Select(s => new
            {
                Id = s.EmployeeId,
                Name = s.EmployeeName
            });
        }
        public IEnumerable<object> FillEmpAppraisSelect(string lang, int BranchId, int UserId)
        {
            var CurrentEmp = _employeeRepository.GetMatching(s => s.IsDeleted == false && s.UserId == UserId).FirstOrDefault();
            List<int> EmpIdsInts = new List<int>();
            if (CurrentEmp != null)
            {
                EmpIdsInts = _TaamerProContext.EmpStructure.Where(s => s.IsDeleted == false && s.BranchId == BranchId && s.ManagerId == CurrentEmp.EmployeeId).Select(e => e.EmpId).ToList();
            }
            var empstructuer = _employeeRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId && EmpIdsInts.Contains(s.EmployeeId));
            return empstructuer.Select(s => new
            {
                Id = s.EmployeeId,
                Name = lang == "ltr" ? s.EmployeeNameEn : s.EmployeeNameAr,
            });
        }
        public async Task<EmployeesVM> GetEmployeeById(int EmpId, string lang)
        {
            return await _employeeRepository.GetEmployeeById(EmpId, lang);
        }
        public async Task<EmployeesVM> GetEmployeeById_d(int EmpId, string lang)
        {
            return await _employeeRepository.GetEmployeeById_d(EmpId, lang);
        }
        public async Task<IEnumerable<EmployeesVM>> SearchEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId)
        {
            var employees =await _employeeRepository.SearchEmployees(EmployeesSearch, lang, BranchId);
            return employees.ToList();
        }
        public async Task<IEnumerable<EmployeesVM>> SearchArchiveEmployees(EmployeesVM EmployeesSearch, string lang, int BranchId)
        {
            var employees =await _employeeRepository.SearchArchiveEmployees(EmployeesSearch, lang, BranchId);
            return employees.ToList();
        }
        public async Task<EmployeesVM> GetEmployeeInfo(int EmployeeId, string lang, int BranchId)
        {
            var employees =await _employeeRepository.GetEmployeeInfo(EmployeeId, lang, BranchId);
            return employees;
        }
        public Object GetEmployeeStatistics()
        {
            var obj = _employeeRepository.GetEmployeeStatistics();
            return obj;
        }
        public async Task<IEnumerable<rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con)
        {
            var employees = await _employeeRepository.GetResDencesAbouutToExpire(Con, null);
            return employees.ToList();
        }
        public async Task<IEnumerable< rptGetResdencesAboutToExpireVM>> GetResDencesAbouutToExpire(string Con,int? DepartmectId)
        {
            var employees =await _employeeRepository.GetResDencesAbouutToExpire(Con, DepartmectId);
            return employees.ToList();
        }

        public async Task<IEnumerable< rptGetResdencesExpiredVM>> GetResDencesExpired(string Con)
        {
            var employees =await _employeeRepository.GetResDencesExpired(Con,null);
            return employees.ToList();
        }
        public async Task<IEnumerable<rptGetResdencesExpiredVM>> GetResDencesExpired(string Con, int? DepartmectId)
        {
            var employees = await _employeeRepository.GetResDencesExpired(Con, DepartmectId);
            return employees.ToList();
        }

        public async Task<IEnumerable< rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con)
        {
            var employees = await _employeeRepository.GetOfficialDocsAboutToExpire(Con,null);
            return employees.ToList();
        }

        public async Task<IEnumerable<rptGetOfficialDocsAboutToExpire>> GetOfficialDocsAboutToExpire(string Con, int? DepartmectId)
        {
            var employees = await _employeeRepository.GetOfficialDocsAboutToExpire(Con, DepartmectId);
            return employees.ToList();
        }

        public async Task<IEnumerable< rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con)
        {
            var employees =await _employeeRepository.GetOfficialDocsExpired(Con,null);
            return employees.ToList();
        }
        public async Task<IEnumerable<rptGetOfficialDocsExpiredVM>> GetOfficialDocsExpired(string Con, int? DepartmectId)
        {
            var employees = await _employeeRepository.GetOfficialDocsExpired(Con, DepartmectId);
            return employees.ToList();
        }

        public async Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract( int? DepartmectId, string lang)
        {
            var employees = await _employeeRepository.GetEmployeeWithoutContract(DepartmectId, lang);
            return employees.ToList();
        }

        public async Task<IEnumerable<EmployeesVM>> GetEmployeeWithoutContract(int? DepartmectId, string lang,string? Searchtext)
        {
            var employees = await _employeeRepository.GetEmployeeWithoutContract(DepartmectId, lang, Searchtext);
            return employees.ToList();
        }

        
        public IEnumerable< rptGetEmpLoans> GetEmpLoans(string Con)
        {
            try
            {
                List< rptGetEmpLoans> lmd = new List< rptGetEmpLoans>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetEmpLoans";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetEmpLoans
                            {
                                LoanID = (dr[0]).ToString(),
                                date = dr[1].ToString(),
                                Amount = dr[2].ToString(),
                                MonthNo = dr[3].ToString(),
                                NameAr = dr[4].ToString(),
                                Payed = dr[5].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List< rptGetEmpLoans> lmd = new List< rptGetEmpLoans>();
                return lmd;
            }
        }

        public IEnumerable< rptGetEmpContractsAboutToExpireVM> GetEmpContractsAboutToExpire(string Con)
        {
            try
            {
                List< rptGetEmpContractsAboutToExpireVM> lmd = new List< rptGetEmpContractsAboutToExpireVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetEmpContractsAboutToExpire";
                        command.Connection = con;
                        //fthis

                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetEmpContractsAboutToExpireVM
                            {
                                ContractNo = dr[0].ToString(),
                                NameAr = dr[1].ToString(),
                                Nationality = dr[2].ToString(),
                                Department = dr[3].ToString(),
                                Branch = dr[4].ToString(),
                                ContractEndDate = dr[5].ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<rptGetEmpContractsAboutToExpireVM> lmd = new List< rptGetEmpContractsAboutToExpireVM>();
                return lmd;
            }
        }


        public IEnumerable<rptGetEmpContractsAboutToExpireVM> GetEmpContractsAboutToExpire(string Con, int? DepartmentID)
        {
            try
            {
                List<rptGetEmpContractsAboutToExpireVM> lmd = new List<rptGetEmpContractsAboutToExpireVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                        command.CommandText = "rptGetEmpContractsAboutToExpire";
                        command.Connection = con;
                        //fthis

                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new rptGetEmpContractsAboutToExpireVM
                            {
                                ContractNo = dr[0].ToString(),
                                NameAr = dr[1].ToString(),
                                Nationality = dr[2].ToString(),
                                Department = dr[3].ToString(),
                                Branch = dr[4].ToString(),
                                ContractEndDate = dr[5].ToString(),
                                JobName = dr[6].ToString(),
                                Salary = dr[7].ToString(),
                                Duration = (Convert.ToInt32(dr[8].ToString()) < 0) ? "-" : (Convert.ToInt32(dr[8].ToString()) < 30) ? Convert.ToInt32(dr[8].ToString()) + " يوم " : (Convert.ToInt32(dr[8].ToString()) == 30) ? Convert.ToInt32(dr[8].ToString()) / 30 + " شهر " : (Convert.ToInt32(dr[8].ToString()) > 30) ? ((Convert.ToInt32(dr[8].ToString()) / 30) + " شهر " + (Convert.ToInt32(dr[8].ToString()) % 30) + " يوم  ") : "",


                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<rptGetEmpContractsAboutToExpireVM> lmd = new List<rptGetEmpContractsAboutToExpireVM>();
                return lmd;
            }
        }

        public IEnumerable<rptGetEmpContractsAboutToExpireVM> GetEmpContractsExpired(string Con, int? DepartmentID)
        {
            try
            {
                List<rptGetEmpContractsAboutToExpireVM> lmd = new List<rptGetEmpContractsAboutToExpireVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));
                        command.CommandText = "rptGetEmpContractsExpired";
                        command.Connection = con;
                        //fthis

                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new rptGetEmpContractsAboutToExpireVM
                            {
                                ContractNo = dr[0].ToString(),
                                NameAr = dr[1].ToString(),
                                Nationality = dr[2].ToString(),
                                Department = dr[3].ToString(),
                                Branch = dr[4].ToString(),
                                ContractEndDate = dr[5].ToString(),
                           
                                JobName = dr[6].ToString(),
                                Salary = dr[7].ToString(),
                                Duration = (Convert.ToInt32(dr[8].ToString()) < 0)? "-":(Convert.ToInt32(dr[8].ToString()) < 30) ? Convert.ToInt32(dr[8].ToString()) + " يوم " : (Convert.ToInt32(dr[8].ToString()) == 30) ? Convert.ToInt32(dr[8].ToString()) / 30 + " شهر " : (Convert.ToInt32(dr[8].ToString()) > 30) ? ((Convert.ToInt32(dr[8].ToString()) / 30) + " شهر " + (Convert.ToInt32(dr[8].ToString()) % 30) + " يوم  ") : "",

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                List<rptGetEmpContractsAboutToExpireVM> lmd = new List<rptGetEmpContractsAboutToExpireVM>();
                return lmd;
            }
        }


        public GeneralMessage DeleteQuacontractDetails(int EmployeeId)
        {
            try
            {
                var emp = _employeeRepository.GetById(EmployeeId);


                if (emp != null)
                {
                    emp.QuaContract = null;
                    emp.ContractStartDate = null;
                    emp.ContractEndDate = null;
                    emp.ContractStartHijriDate = null;
                    emp.ContractNo = null;
                    emp.ContractEndCount = null;
                    emp.ContractEndHijriDate = null;
                    emp.WorkStartDate = null;
                    emp.Allowances= null;
                    emp.VacationsCount = null;
                    


                    _TaamerProContext.SaveChanges();

                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة اوراق رسمية للموظف";
                _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, 1, 1, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الاوراق الرسمية للموظف";
                _SystemAction.SaveAction("SaveOfficialDocuments", "EmployeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, 1, 1, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public (List<int> Users, string Description, string mail) GetNotificationRecipients(NotificationCode code, int? EmpId)
        {
            var usersnote = new List<int>();
            string Email = "";
            var config = _TaamerProContext.NotificationConfigurations
                .Include(x => x.NotificationConfigurationsAssines)
                .FirstOrDefault(x => x.ConfigurationId == (int)code);

            // الوصف الافتراضي إذا لم يوجد
            string description = string.IsNullOrWhiteSpace(config?.Description)
                                 ? "تم إرسال إشعار جديد."
                                 : config.Description;

            var to = (Beneficiary_type)(config?.To ?? 0);

            switch (to)
            {
                case Beneficiary_type.مستخدمين:
                    if (config?.NotificationConfigurationsAssines != null)
                    {
                        usersnote.AddRange(
                            config.NotificationConfigurationsAssines
                            .Where(x => x.UserId.HasValue && x.UserId.Value > 0)
                            .Select(x => x.UserId.Value)
                        );
                    }
                    break;

                case Beneficiary_type.المدير_المباشر:
                    if (EmpId.HasValue)
                    {
                        var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);
                        if (emp?.DirectManager != null)
                        {
                            var mgrUserId = _TaamerProContext.Employees
                                .Where(e => e.EmployeeId == emp.DirectManager && e.UserId.HasValue)
                                .Select(e => e.UserId.Value)
                                .FirstOrDefault();
                            if (mgrUserId > 0)
                                usersnote.Add(mgrUserId);
                        }
                    }
                    break;

                case Beneficiary_type.المدير_المباشر_و_الموظف:
                    if (EmpId.HasValue)
                    {
                        var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);
                        if (emp != null)
                        {
                            if (emp.UserId.HasValue && emp.UserId.Value > 0)
                            {
                                usersnote.Add(emp.UserId.Value);
                            }
                            else
                            {
                                Email = emp.Email;
                            }

                            if (emp.DirectManager != null)
                            {
                                var mgrUserId = _TaamerProContext.Employees
                                    .Where(e => e.EmployeeId == emp.DirectManager && e.UserId.HasValue)
                                    .Select(e => e.UserId.Value)
                                    .FirstOrDefault();
                                if (mgrUserId > 0)
                                    usersnote.Add(mgrUserId);
                            }
                        }
                    }
                    break;

                case Beneficiary_type.الموظف_و_المدير_المباشر_و_المحاسب:
                    if (EmpId.HasValue)
                    {
                        var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);
                        if (emp != null)
                        {
                            if (emp.UserId.HasValue && emp.UserId.Value > 0)
                            {
                                usersnote.Add(emp.UserId.Value);
                            }
                            else
                            {
                                Email = emp.Email;
                            }

                            if (emp.DirectManager != null)
                            {
                                var mgrUserId = _TaamerProContext.Employees
                                    .Where(e => e.EmployeeId == emp.DirectManager && e.UserId.HasValue)
                                    .Select(e => e.UserId.Value)
                                    .FirstOrDefault();
                                if (mgrUserId > 0)
                                    usersnote.Add(mgrUserId);
                            }


                        }
                    }
                    break;

                case Beneficiary_type.الموظف:
                    if (EmpId.HasValue)
                    {
                        var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);
                        if (emp?.UserId.HasValue == true && emp.UserId.Value > 0)
                        {
                            usersnote.Add(emp.UserId.Value);
                        }
                        else
                        {
                            Email = emp.Email;
                        }
                    }
                    break;
            }

            // إزالة القيم المكررة والأصفار
            usersnote = usersnote
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            // إذا لم يوجد أي مستخدم، عيّن مستخدم افتراضي (مثلاً المستخدم رقم 1)
            if (!usersnote.Any())
            {
                usersnote.Add(1); // default fallback user
            }

            return (usersnote, description, Email);
        }

        //public (List<int> Users, string Description) GetNotificationRecipients(NotificationCode code, int? EmpId, int? UserId)
        //{
        //    var usersnote = new List<int>();

        //    var config = _TaamerProContext.NotificationConfigurations
        //        .Include(x => x.NotificationConfigurationsAssines)
        //        .FirstOrDefault(x => x.ConfigurationId == (int)code);

        //    if (config == null)
        //        return (usersnote, ""); // default if config not found

        //    string description = string.IsNullOrEmpty(config.Description) ? "" : config.Description;

        //    var to = (Beneficiary_type)(config.To ?? 0);

        //    switch (to)
        //    {
        //        case Beneficiary_type.مستخدمين:
        //            if (config.NotificationConfigurationsAssines != null)
        //                usersnote.AddRange((List<int>)config.NotificationConfigurationsAssines.Select(x => x.UserId));
        //            break;

        //        case Beneficiary_type.الموظف:
        //            if (EmpId.HasValue)
        //            {
        //                var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);
        //                if (emp?.UserId != null)
        //                    usersnote.Add(emp.UserId.Value);
        //            }
        //            else if (UserId.HasValue)
        //            {
        //                usersnote.Add(UserId.Value);
        //            }
        //            break;

        //        case Beneficiary_type.المدير_المباشر:
        //            if (EmpId.HasValue)
        //            {
        //                var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);
        //                if (emp?.DirectManager != null)
        //                {
        //                    var mgrUserId = _TaamerProContext.Employees
        //                        .Where(e => e.EmployeeId == emp.DirectManager && e.UserId.HasValue)
        //                        .Select(e => e.UserId.Value).FirstOrDefault();
        //                    if (mgrUserId > 0)
        //                        usersnote.Add(mgrUserId);
        //                }
        //            }
        //            break;

        //        case Beneficiary_type.المدير_المباشر_و_المحاسب:
        //        case Beneficiary_type.الموظف_و_المدير_المباشر_و_المحاسب:
        //            if (EmpId.HasValue)
        //            {
        //                var emp = _TaamerProContext.Employees.FirstOrDefault(e => e.EmployeeId == EmpId.Value);

        //                // Add employee user
        //                if (to == Beneficiary_type.الموظف_و_المدير_المباشر_و_المحاسب && emp?.UserId.HasValue == true)
        //                    usersnote.Add(emp.UserId.Value);

        //                // Add direct manager user
        //                if (emp?.DirectManager != null)
        //                {
        //                    var mgrUserId = _TaamerProContext.Employees
        //                        .Where(e => e.EmployeeId == emp.DirectManager && e.UserId.HasValue)
        //                        .Select(e => e.UserId.Value).FirstOrDefault();
        //                    if (mgrUserId > 0)
        //                        usersnote.Add(mgrUserId);
        //                }
        //            }
        //            else if (UserId.HasValue && to == Beneficiary_type.الموظف_و_المدير_المباشر_و_المحاسب)
        //            {
        //                usersnote.Add(UserId.Value);
        //            }
        //            break;
        //    }

        //    return (usersnote.Distinct().ToList(), description);
        //}

    }
}
