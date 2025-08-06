using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _LoanRepository;
        private readonly ILoanDetailsRepository _LoanDetailsRepository;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly IAccountsRepository _AccountsRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly ICustomerMailService _customerMailService;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IPayrollMarchesRepository _payrollMarchesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly INotificationService _notificationService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IEmployeesService _employeesService;
        public LoanService(ILoanRepository loanRepository, ILoanDetailsRepository loanDetailsRepository, IEmployeesRepository employeesRepository
            , ITransactionsRepository transactionsRepository, IAccountsRepository accountsRepository, INotificationRepository notificationRepository,
            IBranchesRepository branchesRepository, ISys_SystemActionsRepository sys_SystemActionsRepository, ICustomerMailService customerMailService,
            IUserNotificationPrivilegesService userNotificationPrivilegesService, IPayrollMarchesRepository payrollMarchesRepository,
            IUsersRepository usersRepository, IEmailSettingRepository emailSettingRepository, IOrganizationsRepository organizationsRepository,
            INotificationService notificationService, TaamerProjectContext dataContext, ISystemAction systemAction,
            IEmployeesService employeesService)
        {
            _LoanRepository = loanRepository;
            _LoanDetailsRepository = loanDetailsRepository;
            _EmployeesRepository = employeesRepository;
            _TransactionsRepository = transactionsRepository;
            _AccountsRepository = accountsRepository;
            _NotificationRepository = notificationRepository;
            _BranchesRepository = branchesRepository;
            _customerMailService = customerMailService;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _payrollMarchesRepository = payrollMarchesRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _usersRepository = usersRepository;
            _EmailSettingRepository = emailSettingRepository;
            _OrganizationsRepository = organizationsRepository;
            _notificationService = notificationService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _employeesService = employeesService;
        }
        public async Task< IEnumerable<LoanVM>> GetAllLoans(int? EmpId, string SearchText)
        {
            var Loans =await _LoanRepository.GetAllLoans(EmpId, SearchText);
            return Loans;
        }
        public IEnumerable<Loan> GetAllLoansDataModel(int? EmpId)
        {

            var Loans = _LoanRepository.GetAll().Where(s => s.IsDeleted == false && s.EmployeeId == EmpId ).ToList();
            return Loans;
        }
        public async Task<IEnumerable<LoanVM>> GetAllLoansE(int? EmpId, string SearchText)
        {
            var UserID_E = _EmployeesRepository.GetById((int)EmpId).UserId;

            var Loans = await _LoanRepository.GetAllLoansE(EmpId, SearchText);
            return Loans;
        }
        public async Task<IEnumerable<LoanVM>> GetAllLoansAfterDecision(int? EmpId, string SearchText)
        {
            var Loans = await _LoanRepository.GetAllLoansAfterDecision(EmpId, SearchText);
            return Loans;
        }
        public async Task<IEnumerable<LoanVM>> GetAllLoans2(int? UserId, string SearchText)
        {
            var Loans = await _LoanRepository.GetAllLoans2(UserId, SearchText);
            return Loans;
        }
        public async Task<IEnumerable<LoanVM>> GetAllLoansW(string SearchText)
        {
            var Loans = await _LoanRepository.GetAllLoansW(SearchText);
            return Loans;
        }
        public async Task<IEnumerable<LoanVM>> GetAllLoansW2(string SearchText, int status)
        {
            var Loans = await _LoanRepository.GetAllLoansW2(SearchText, status);
            return Loans;
        }
        public async Task<IEnumerable<LoanDetailsVM>> GetAllLoanDetails(int? LoanId)
        {
            
            var loanDetails = await _LoanDetailsRepository.GetAllLoanDetails(LoanId);
            return loanDetails;
        }

        public IEnumerable<LoanDetailsVM> GetAllLoanDetails2(int LoanId)
        {

            var loanDetails =  _LoanDetailsRepository.GetAllLoanDetails2(LoanId);
            return loanDetails;
        }
        public async Task<LoanDetailsVM> GetAmountPayedAndNotPayed(int? LoanId)
        {
            var loanDetails = await _LoanDetailsRepository.GetAmountPayedAndNotPayed(LoanId);
            return loanDetails;
        }

        public GeneralMessage SaveLoan(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {

            var ExistEmp = _EmployeesRepository.GetEmployeeByUserid(UserId).Result.FirstOrDefault();
            if (ExistEmp == null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
               _SystemAction.SaveAction("SaveLoan", "LoanService", 1, "لا يمكنك ارسال طلب سلفة وذلك لعدم ربط المستخدم الخاص بك بموطف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Not_Send };
            }
            else if (string.IsNullOrEmpty(ExistEmp.WorkStartDate) || !string.IsNullOrEmpty(ExistEmp.EndWorkDate))
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                string msg = string.IsNullOrEmpty(ExistEmp.WorkStartDate) ? "لا يمكنك ارسال طلب سلفة وذلك لعدم مباشرة الموظف للعمل" :
                    "لا يمكنك ارسال طلب سلفة وذلك لإنتهاء خدمات الموظف";
                _SystemAction.SaveAction("SaveLoan", "LoanService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = msg };
            }
            else
            {
                try
                {
                    DateTime loandate = DateTime.ParseExact(loan.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (loan.LoanId == 0)
                    {
                        loan.Money = 0;
                        loan.AddUser = UserId;
                        loan.EmployeeId = ExistEmp.EmployeeId;
                        loan.UserId = UserId;
                        loan.BranchId = BranchId;
                        loan.DecisionType = 0;
                        loan.StartDate = Utilities.ConvertDateCalendar(new DateTime(DateTime.Now.Year, loan.StartMonth.Value, DateTime.Now.Day), "Gregorian", "en-US");
                        loan.AddDate = DateTime.Now;
                        _TaamerProContext.Loan.Add(loan);

                        NewLoan_Notification(Lang, loan, ExistEmp, UserId, BranchId, Url, ImgUrl);
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة سلفة جديدة";
                        _SystemAction.SaveAction("SaveLoan", "LoanService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                    else
                    {
                        var LoanUpdated = _LoanRepository.GetById(loan.LoanId);
                        if (LoanUpdated != null)
                        {
                            if (loan.EmployeeId != null)
                            {
                                LoanUpdated.EmployeeId = loan.EmployeeId;
                            }
                            LoanUpdated.Date = loan.Date;
                            LoanUpdated.Amount = loan.Amount;
                            LoanUpdated.MonthNo = loan.MonthNo;
                            LoanUpdated.Money = 0;
                            LoanUpdated.Note = loan.Note;
                            LoanUpdated.Status = loan.Status;
                            if(loan.UserId != null) { 
                            LoanUpdated.UserId = loan.UserId;
                            }
                            LoanUpdated.UpdateUser = UserId;
                            LoanUpdated.UpdateDate = DateTime.Now;
                            // update detail
                            for (int i = 0; i < LoanUpdated.MonthNo; i++)
                            {
                                var existingDetails = _TaamerProContext.LoanDetails.Where(s => s.LoanId == loan.LoanId);
                                if (existingDetails != null)
                                {
                                    _TaamerProContext.LoanDetails.RemoveRange(existingDetails);
                                }
                                var loandetailsupdate = new LoanDetails();
                                loandetailsupdate.LoanId = loan.LoanId;
                                loandetailsupdate.Amount = loan.Amount / loan.MonthNo;
                                loandetailsupdate.Date = loandate.AddMonths(i + 1);
                                loandetailsupdate.Finished = false;
                                loandetailsupdate.AddUser = UserId;
                                loandetailsupdate.AddDate = DateTime.Now;
                                _TaamerProContext.LoanDetails.Add(loandetailsupdate);
                            }
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل السلفة رقم " + loan.LoanId;
                        _SystemAction.SaveAction("SaveLoan", "LoanService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    _SystemAction.SaveAction("SaveLoan", "LoanService", 1, "فشل في حفظ السلفة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }

            }
        }


        public GeneralMessage SaveLoan_Management(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {

            var ExistEmp = _EmployeesRepository.GetEmployeeByUserid(UserId).Result.FirstOrDefault();
            if (ExistEmp == null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveLoan", "LoanService", 1, "لا يمكنك ارسال طلب سلفة وذلك لعدم ربط المستخدم الخاص بك بموطف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك ارسال طلب سلفة وذلك لعدم ربط المستخدم الخاص بك بموطف" };
            }
            else if (string.IsNullOrEmpty(ExistEmp.WorkStartDate) || !string.IsNullOrEmpty(ExistEmp.EndWorkDate))
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                string msg = string.IsNullOrEmpty(ExistEmp.WorkStartDate) ? "لا يمكنك ارسال طلب سلفة وذلك لعدم مباشرة الموظف للعمل" :
                    "لا يمكنك ارسال طلب سلفة وذلك لإنتهاء خدمات الموظف";
                _SystemAction.SaveAction("SaveLoan", "LoanService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = msg };
            }
            else
            {
                try
                {
                    DateTime loandate = DateTime.ParseExact(loan.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (loan.LoanId == 0)
                    {
                        loan.Money = 0;
                        loan.AddUser = UserId;
                        loan.EmployeeId = ExistEmp.EmployeeId;
                        loan.UserId = UserId;
                        loan.BranchId = BranchId;
                        loan.DecisionType = 0;
                        loan.StartDate = Utilities.ConvertDateCalendar(new DateTime(DateTime.Now.Year, loan.StartMonth.Value, DateTime.Now.Day), "Gregorian", "en-US");
                        loan.AddDate = DateTime.Now;
                        _TaamerProContext.Loan.Add(loan);
                        try
                        {
                            NewLoan_Notification(Lang, loan, ExistEmp, UserId, BranchId, Url, ImgUrl);
                        }catch(Exception ex) { }
                            _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة سلفة جديدة";
                        _SystemAction.SaveAction("SaveLoan", "LoanService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                    else
                    {
                        var LoanUpdated = _LoanRepository.GetById(loan.LoanId);
                        if (LoanUpdated != null)
                        {
                            LoanUpdated.EmployeeId = loan.EmployeeId;
                            LoanUpdated.Date = loan.Date;
                            LoanUpdated.Amount = loan.Amount;
                            LoanUpdated.MonthNo = loan.MonthNo;
                            LoanUpdated.Money = 0;
                            LoanUpdated.Note = loan.Note;
                            LoanUpdated.Status = loan.Status;
                            LoanUpdated.UserId = loan.UserId;
                            LoanUpdated.UpdateUser = UserId;
                            LoanUpdated.UpdateDate = DateTime.Now;
                            // update detail
                            for (int i = 0; i < LoanUpdated.MonthNo; i++)
                            {
                                var existingDetails = _TaamerProContext.LoanDetails.Where(s => s.LoanId == loan.LoanId);
                                if (existingDetails != null)
                                {
                                    _TaamerProContext.LoanDetails.RemoveRange(existingDetails);
                                }
                                var loandetailsupdate = new LoanDetails();
                                loandetailsupdate.LoanId = loan.LoanId;
                                loandetailsupdate.Amount = loan.Amount / loan.MonthNo;
                                loandetailsupdate.Date = loandate.AddMonths(i + 1);
                                loandetailsupdate.Finished = false;
                                loandetailsupdate.AddUser = UserId;
                                loandetailsupdate.AddDate = DateTime.Now;
                                _TaamerProContext.LoanDetails.Add(loandetailsupdate);
                            }
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل السلفة رقم " + loan.LoanId;
                        _SystemAction.SaveAction("SaveLoan", "LoanService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                   _SystemAction.SaveAction("SaveLoan", "LoanService", 1, "فشل في حفظ السلفة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }

            }
        }
        public GeneralMessage SaveLoan2(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {

            var UserID_E = _EmployeesRepository.GetById((int)loan.EmployeeId).UserId;
            var ExistEmp = _EmployeesRepository.GetEmployeeById(loan.EmployeeId.Value, Lang).Result;

            //if (UserID_E == null)
            //{
            //    //-----------------------------------------------------------------------------------------------------------------
            //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            //    string ActionNote = Resources.General_SavedFailed;
            //    _SystemAction.SaveAction("SaveLoan2", "LoanService", 1, "لا يمكنك ارسال طلب سلفة وذلك لعدم ربط المستخدم الخاص بك بموطف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
            //    //-----------------------------------------------------------------------------------------------------------------
            //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك ارسال طلب سلفة وذلك لعدم ربط المستخدم الخاص بك بموطف" };
            //}
            if (string.IsNullOrEmpty(ExistEmp.WorkStartDate) || !string.IsNullOrEmpty(ExistEmp.EndWorkDate))
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                string msg = string.IsNullOrEmpty(ExistEmp.WorkStartDate) ? "لا يمكنك ارسال طلب سلفة وذلك لعدم مباشرة الموظف للعمل" :
                    "لا يمكنك ارسال طلب سلفة وذلك لإنتهاء خدمات الموظف";
                _SystemAction.SaveAction("SaveLoan", "LoanService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = msg };
            }
            else
            {
                try
                {
                    DateTime dateTime = DateTime.ParseExact(loan.Date, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                    var dateT = dateTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    DateTime loandate = DateTime.ParseExact(dateT.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    int LastDay = DateTime.DaysInMonth(DateTime.Now.Year, loan.StartMonth.Value);

                    if (loan.LoanId == 0)
                    {
                        loan.Money = 0;
                        loan.AddUser = UserId;
                        loan.EmployeeId = loan.EmployeeId;
                        loan.UserId = UserID_E;
                        loan.BranchId = BranchId;
                        loan.Date = dateT.ToString();
                        loan.DecisionType = 0;
                        loan.StartDate = Utilities.ConvertDateCalendar(new DateTime(DateTime.Now.Year, loan.StartMonth.Value, LastDay), "Gregorian", "en-US");
                        loan.AddDate = DateTime.Now;
                        _TaamerProContext.Loan.Add(loan);

                       // NewLoan_Notification(Lang, loan, ExistEmp, UserId, BranchId, Url, ImgUrl);
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة سلفة جديدة";
                        _SystemAction.SaveAction("SaveLoan2", "LoanService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                    else
                    {
                        DateTime dateTime2 = DateTime.ParseExact(loan.Date, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                        var dateT2 = dateTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        DateTime loandate2 = DateTime.ParseExact(dateT.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var LoanUpdated = _LoanRepository.GetById(loan.LoanId);
                        if (LoanUpdated != null)
                        {
                            LoanUpdated.EmployeeId = loan.EmployeeId;
                            LoanUpdated.Date = dateT2.ToString();
                            LoanUpdated.Amount = loan.Amount;
                            LoanUpdated.MonthNo = loan.MonthNo;
                            LoanUpdated.Money = 0;
                            LoanUpdated.Note = loan.Note;
                            LoanUpdated.Status = loan.Status;
                            LoanUpdated.UserId = loan.UserId;
                            LoanUpdated.UpdateUser = UserId;
                            LoanUpdated.UpdateDate = DateTime.Now;
                            // update detail
                            for (int i = 0; i < LoanUpdated.MonthNo; i++)
                            {
                                var existingDetails = _LoanDetailsRepository.GetAll().Where(s => s.LoanId == loan.LoanId);
                                if (existingDetails != null)
                                {
                                    _TaamerProContext.LoanDetails.RemoveRange(existingDetails);
                                }
                                var loandetailsupdate = new LoanDetails();
                                loandetailsupdate.LoanId = loan.LoanId;
                                loandetailsupdate.Amount = loan.Amount / loan.MonthNo;
                                loandetailsupdate.Date = loandate.AddMonths(i + 1);
                                loandetailsupdate.Finished = false;
                                loandetailsupdate.AddUser = UserId;
                                loandetailsupdate.AddDate = DateTime.Now;
                                _TaamerProContext.LoanDetails.Add(loandetailsupdate);
                            }
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل سلفة رقم " + loan.LoanId;
                       _SystemAction.SaveAction("SaveLoan2", "LoanService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    _SystemAction.SaveAction("SaveLoan2", "LoanService", 1, "فشل في حفظ السلفة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
                }

            }
        }

        public GeneralMessage SaveLoanWorkers(Loan loan, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {
            try
            {
                var ValMo = "";
                if (loan.StartMonth < 10)
                {
                    ValMo = "0" + loan.StartMonth;
                }
                else
                {
                    ValMo = loan.StartMonth.ToString();
                }
                //loan.Date = Convert.ToDateTime(loan.Date).ToString();
                //loan.Date = DateTime.ParseExact(loan.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString();
                DateTime dateTime = DateTime.ParseExact(loan.Date, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
               var dateT= dateTime.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                DateTime loandate = DateTime.ParseExact(dateT.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                int LastDay = DateTime.DaysInMonth(DateTime.Now.Year, loan.StartMonth.Value);
                if (loan.LoanId == 0)
                {
                    loan.Money = 0;
                    loan.AddUser = UserId;
                    loan.EmployeeId = loan.EmployeeId;
                    loan.UserId = UserId;
                    loan.BranchId = BranchId;
                    loan.DecisionType = 0;
                    loan.StartDate = Utilities.ConvertDateCalendar(new DateTime(DateTime.Now.Year, loan.StartMonth.Value, LastDay), "Gregorian", "en-US");
                    loan.AddDate = DateTime.Now;

                    _TaamerProContext.Loan.Add(loan);
                    _TaamerProContext.SaveChanges();
                    try
                    {
                        var ExistEmp = _EmployeesRepository.GetEmployeeById(loan.EmployeeId.Value, Lang).Result;
                        NewLoan_Notification(Lang, loan, ExistEmp, UserId, BranchId, Url, ImgUrl);
                    }
                    catch (Exception ex)
                    {

                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سلفة جديدة";
                    _SystemAction.SaveAction("SaveLoan2", "LoanService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var LoanUpdated = _LoanRepository.GetById(loan.LoanId);
                    if (LoanUpdated != null)
                    {
                        if (loan.EmployeeId != null)
                        {
                            LoanUpdated.EmployeeId = loan.EmployeeId;
                        }
                        LoanUpdated.Date = loan.Date;
                        LoanUpdated.Amount = loan.Amount;
                        LoanUpdated.MonthNo = loan.MonthNo;
                        LoanUpdated.Money = 0;
                        LoanUpdated.Note = loan.Note;
                        LoanUpdated.Status = loan.Status;
                        if (loan.UserId != null)
                        {
                            LoanUpdated.UserId = loan.UserId;
                        }
                        LoanUpdated.UpdateUser = UserId;
                        LoanUpdated.UpdateDate = DateTime.Now;
                        LoanUpdated.StartMonth = loan.StartMonth;
                        // update detail
                        for (int i = 0; i < LoanUpdated.MonthNo; i++)
                        {
                            var existingDetails = _TaamerProContext.LoanDetails.Where(s => s.LoanId == loan.LoanId);
                            if (existingDetails != null)
                            {
                               _TaamerProContext.LoanDetails.RemoveRange(existingDetails);
                            }
                            var loandetailsupdate = new LoanDetails();
                            loandetailsupdate.LoanId = loan.LoanId;
                            loandetailsupdate.Amount = loan.Amount / loan.MonthNo;
                            loandetailsupdate.Date = loandate.AddMonths(i + 1);
                            loandetailsupdate.Finished = false;
                            loandetailsupdate.AddUser = UserId;
                            loandetailsupdate.AddDate = DateTime.Now;
                             _TaamerProContext.LoanDetails.Add(loandetailsupdate);
                        }
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل سلفة رقم " + loan.LoanId;
                    _SystemAction.SaveAction("SaveLoanWorkers", "LoanService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveLoanWorkers", "LoanService", 1, "فشل في حفظ السلفة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }

        }

        public GeneralMessage PayLoan(int LoanDetailsId, int UserId, int BranchId)
        {
            try
            {
                LoanDetails loanDetails = _LoanDetailsRepository.GetById(LoanDetailsId);
                loanDetails.Finished = true;
                var loan = _LoanRepository.GetById((int)loanDetails.LoanId);
                loan.Money += loanDetails.Amount;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تسديد سلفة رقم  " + loanDetails.LoanId;
                _SystemAction.SaveAction("PayLoan", "LoanService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("PayLoan", "LoanService", 1, "فشل في حفظ السلفة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
            }
        }

        public GeneralMessage DeleteLoan(int LoanId, int UserId, int BranchId)
        {
            try
            {
                Loan loan = _LoanRepository.GetById(LoanId);
                loan.IsDeleted = true;
                loan.DeleteDate = DateTime.Now;
                loan.DeleteUser = UserId;
                //delete details
                var loanDetails = _TaamerProContext.LoanDetails.Where(s => s.LoanId == LoanId);
                if (loanDetails != null)
                {
                   _TaamerProContext.LoanDetails.RemoveRange(loanDetails);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف سلفة رقم " + LoanId;
                _SystemAction.SaveAction("DeleteLoan", "LoanService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف سلفة رقم " + LoanId; ;
                _SystemAction.SaveAction("DeleteLoan", "LoanService", 3, "لا يمكنك حذف هذه السلفة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public async Task< IEnumerable<LoanVM>> GetAllImprestSearch(LoanVM ImprestSearch, int BranchId)
        {
            try {
            IEnumerable<LoanVM> result = null;
            List<int> Emps = null;
            if (ImprestSearch.IsSearch)
            {

                result =await _LoanRepository.GetAllImprestBySearchObject(ImprestSearch, BranchId);

                if (ImprestSearch.StartDate != null)
                {
                    Emps = await _EmployeesRepository.GetAllActiveEmpsByDate(ImprestSearch.StartDate, ImprestSearch.EndDate);
                }
            }
            else
            {
                result = await _LoanRepository.GetAllImprestSearch(BranchId);
            }
            if (!ImprestSearch.IsSearch || ImprestSearch.StartDate == null)
            {
                var employees = _TaamerProContext.Employees.Where(x => x.IsDeleted == false).ToList();
                    employees.Where(x=>(!string.IsNullOrEmpty(x.WorkStartDate) && string.IsNullOrEmpty(x.EndWorkDate))).ToList();
                Emps=employees.Select(y => y.EmployeeId).ToList();
            }
            result = result.Where(x => Emps.Contains(x.EmployeeId.Value));
            //foreach (var item in result)
            //{

            //}
            return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public GeneralMessage UpdateStatus(int ImprestId, int UserId, int BranchId, string Lang, int Type, int? YearId, string Url, string ImgUrl,string? Reason)
        {
            try
            {
                var ImprestUpdated = _LoanRepository.GetById(ImprestId);
                DateTime FirstTerm = new DateTime();
                decimal termAmount = 0;
                var AccountID_OF_Emp = _EmployeesRepository.GetEmployeeById(Convert.ToInt32(ImprestUpdated.EmployeeId), Lang).Result;

                var payroll = _payrollMarchesRepository.GetPayrollMarches(ImprestUpdated.EmployeeId.Value, DateTime.Now.Month).Result;
                if (payroll != null && payroll.PostDate.HasValue && Type == 2 && int.Parse(ImprestUpdated.StartDate.Split('-')[1]) <= DateTime.Now.Month)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في حفظ مسيرات الرواتب (السلف)";
                    _SystemAction.SaveAction("UpdateStatus", "LoanService", 2, "لا يمكن التعديل على مسير رواتب مُرحل", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.notPossibleModifyRelayPath };
                }

                var Branch = _BranchesRepository.GetById(BranchId);
                if (Branch == null || Branch.LoanAccId == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = Resources.General_SavedFailed;
                    _SystemAction.SaveAction("UpdateStatus", "LoanService", 1, "تأكد من حساب السلف في حسابات الفرع", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.EnsureAdvancesBranchAccounts };
                }


                if (ImprestUpdated != null)
                {


                    if (ImprestUpdated.Status == 2)

                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في تعديل السلفة رقم " + ImprestId;
                        _SystemAction.SaveAction("UpdateStatus", "LoanService", 1, "لا يمكنك التعديل لقد تم الموافقة عليها مسبقا", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_EditedFaild };
                    }
                    DateTime loandate = DateTime.ParseExact(ImprestUpdated.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    ImprestUpdated.Status = Type;
                    if (Type == 2 || Type == 4 || Type == 5) //قبول او رفض
                    {
                        ImprestUpdated.DecisionType = 1;
                    }
                    else
                    {
                        ImprestUpdated.DecisionType = 0;
                    }
                    int LastDate;
                    termAmount = ImprestUpdated.Amount.Value / ImprestUpdated.MonthNo.Value;
                    if (Type == 2)
                    {
                        ImprestUpdated.AcceptedDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); //DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        ImprestUpdated.AcceptedUser = UserId;


                        for (int i = 0; i < ImprestUpdated.MonthNo; i++)
                        {
                            var loandetails = new LoanDetails();
                            loandetails.LoanId = ImprestUpdated.LoanId;
                            loandetails.Amount = ImprestUpdated.Amount / ImprestUpdated.MonthNo;
                            //loandetails.Date = loandate.AddMonths(i + 1);
                            LastDate = DateTime.DaysInMonth(DateTime.Now.Year, loandate.AddMonths(i).Month);
                            if (i == 0)
                            {
                                loandetails.Date = new DateTime(DateTime.Now.Year, loandate.AddMonths(i).Month, LastDate);
                                FirstTerm = loandetails.Date.Value;
                            }
                            loandetails.Date = FirstTerm.AddMonths(i);
                            loandetails.Finished = false;
                            loandetails.AddUser = UserId;
                            loandetails.AddDate = DateTime.Now;
                             _TaamerProContext.LoanDetails.Add(loandetails);
                        }

                        var Depit = ImprestUpdated.Amount;
                       
                    }

                    string Subject =
                       Type == 2 ? "الموافقة علي السلفه" : Type == 3 ? "تم رفض  السلفه" :
                       Type == 4 || Type == 5 ? Lang == "rtl" ? "إشعار بتحديث  حالة السلفة" : "Notice of imprest status modification" : "";


                    LastDate = DateTime.DaysInMonth(DateTime.Now.Year, loandate.Month);
                    FirstTerm = new DateTime(DateTime.Now.Year, loandate.Month, LastDate);

                    //string Subject = Lang == "rtl" ? "إشعار بالموافقة على السلفة" : "Notice of Loan acceptance";
                    string NotStr = string.Format(" السلفة الخاصة بالموظف {0} بمبلغ {1} - تاريخ بداية الاستقطاع {2}", AccountID_OF_Emp.EmployeeName, ImprestUpdated.Amount, FirstTerm.ToString("yyyy-MM-dd", new CultureInfo("en-US")));
                    NotStr = NotStr + " " + (Type == 2 ? "تم الموافقة عليها" :
                                       Type == 3 ? "تم رفضها" :
                                       Type == 4 || Type == 5 ? "أصبحت تحت الطلب" : "أصبحت تحت الطلب");
                    var directmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == AccountID_OF_Emp.DirectManager).FirstOrDefault();
                    string OrgName = _OrganizationsRepository.GetBranchOrganization().Result.NameAr;

                    if (ImprestUpdated.UserId != null && ImprestUpdated.UserId != 0)
                    {
                        #region Notifications
                        string htmlBody = "";

                        if (Type == 2 || Type == 3)
                        {
                            var code = Type == 2 ? Models.Enums.NotificationCode.HR_AdvanceAccepted : Models.Enums.NotificationCode.HR_AdvanceRejected;
                            var config = _employeesService.GetNotificationRecipients(code, ImprestUpdated.EmployeeId);
                            try
                            {
                                if (config.Description != null && config.Description != "")
                                    Subject = config.Description;
                                //Notification
                                var branch = _BranchesRepository.GetById(BranchId);
                                var rejectreason = "";
                                if (Type == 3 && Reason != null && Reason != "")
                                {
                                    rejectreason = " <tr> <td> السبب</td> <td>" + Reason + @"</td> </tr>";



                                }

                                htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
    <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
    <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
    <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
    <h4> عزيزي الموظف " + AccountID_OF_Emp.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > " + NotStr + @"  </h3><table align = 'center' border = '1' ><tr> <td>  المبلغ</td><td>" + ImprestUpdated.Amount + @"</td> </tr> <tr> <td>   ت. بداية الاستقطاع  </td> <td>" + FirstTerm.ToString("yyyy-MM-dd", new CultureInfo("en-US")) + @"</td>
     </tr> <tr> <td>   مبلغ الاستقطاع</td> <td>" + termAmount.ToString("N2") + @"</td> </tr> " + rejectreason + @" </table> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
    </div> </div></div></div></body></html> ";
                                if (config.Users != null && config.Users.Count() > 0)
                                {
                                    foreach (var usr in config.Users)
                                    {


                                        var UserNotification = new Notification();
                                        UserNotification.ReceiveUserId = usr;
                                        UserNotification.Name = Subject;
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
                                        UserNotification.NextTime = null;
                                        UserNotification.AddUser = UserId;
                                        UserNotification.AddDate = DateTime.Now;
                                        UserNotification.NextTime = null;

                                        _TaamerProContext.Notification.Add(UserNotification);
                                        _TaamerProContext.SaveChanges();
                                        _notificationService.sendmobilenotification(usr, Subject, NotStr);
                                        _customerMailService.SendMail_SysNotification((int)BranchId, usr, usr, Subject, htmlBody, true);
                                        var userObj = _usersRepository.GetById((int)usr);
                                        var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                                else
                                {


                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = AccountID_OF_Emp.UserId;
                                    UserNotification.Name = Subject;
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
                                    UserNotification.NextTime = null;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.AddDate = DateTime.Now;
                                    UserNotification.NextTime = null;

                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _TaamerProContext.SaveChanges();
                                    if (directmanager != null)
                                    {
                                        var notdirectmanager = new Notification();
                                        notdirectmanager = UserNotification;
                                        notdirectmanager.ReceiveUserId = directmanager.UserId ?? 0;
                                        notdirectmanager.NotificationId = 0;
                                        _TaamerProContext.Notification.Add(notdirectmanager);
                                        _TaamerProContext.SaveChanges();
                                    }


                                    _notificationService.sendmobilenotification((int)AccountID_OF_Emp.UserId, Subject, NotStr);
                                    if (directmanager != null)
                                    {
                                        _notificationService.sendmobilenotification((int)directmanager?.UserId, Subject, NotStr);

                                    }

                                    //Mail
                                    


                                    if (AccountID_OF_Emp.Email != null)
                                    {
                                        _customerMailService.SendMail_SysNotification((int)BranchId, 0, 0, Subject, htmlBody, true, AccountID_OF_Emp.Email);

                                    }
                                    if (directmanager != null && directmanager?.Email != null)
                                    {
                                        _customerMailService.SendMail_SysNotification((int)BranchId, 0, 0, Subject, htmlBody, true, directmanager?.Email);
                                    }

                                    var userObj = _usersRepository.GetById((int)ImprestUpdated.UserId);
                                    var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }
                                
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        #endregion

                    }
                    else
                    {



                        

                    }


                }
                ImprestUpdated.Note = Reason;
                _TaamerProContext.SaveChanges();
              
                ////-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل سلفة ";
                _SystemAction.SaveAction("UpdateStatus", "LoanService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                ////-----------------------------------------------------------------------------------------------------------------

                var massage = "";

                massage = (Type == 2) ? Resources.MP_ImprestAccept : (Type == 3) ? Resources.MP_ImprestReject : Resources.General_SavedSuccessfully;


                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = massage };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveLoanWorkers", "LoanService", 1, "فشل في حفظ السلفة", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage UpdateDecisionType(int ImprestId, int UserId, int BranchId, string Lang, int DecisionType)
        {
            try
            {
                var ImprestUpdated = _LoanRepository.GetById(ImprestId);


                if (ImprestUpdated != null)
                {
                    ImprestUpdated.DecisionType = DecisionType;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "التحويل الي الادارة سلفة رقم " + ImprestId;
                _SystemAction.SaveAction("UpdateDecisionType", "LoanService", 1, "تم التحويل الي الادارة", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //var massage = "";

                //massage = (DecisionType == 0) ? Resources.MP_ImprestAccept : (DecisionType == 1) ? "تم التحويل الي الادارة";


                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                var massage = "";
                if (Lang == "rtl")
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    _SystemAction.SaveAction("UpdateDecisionType", "LoanService", 1, "فشل في التحويل الي الادارة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
                    //massage = Resources.General_SavedFailed;
                }
                else
                {
                    massage = "Failed Successfully";
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("UpdateDecisionType", "LoanService", 1, "فشل في التحويل الي الادارة", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage };
            }
        }



        //convert to account
        public GeneralMessage Updateconverttoaccounts(int ImprestId, int UserId, int BranchId, string Lang)
        {
            try
            {
                var ImprestUpdated = _LoanRepository.GetById(ImprestId);


                if (ImprestUpdated != null)
                {
                    ImprestUpdated.Isconverted = 1;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "التحويل الي الحسابات سلفة رقم " + ImprestId;
                _SystemAction.SaveAction("Updateconverttoaccounts", "LoanService", 1, "تم التحويل الي الحسابات", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                //var massage = "";

                //massage = (DecisionType == 0) ? Resources.MP_ImprestAccept : (DecisionType == 1) ? "تم التحويل الي الادارة";


                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                var massage = "";
                if (Lang == "rtl")
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    _SystemAction.SaveAction("Updateconverttoaccounts", "LoanService", 1, "فشل في التحويل الي الحسابات", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed};
                    //massage = Resources.General_SavedFailed;
                }
                else
                {
                    massage = "Failed Successfully";
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("Updateconverttoaccounts", "LoanService", 1, "فشل في التحويل الي الادارة", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage };
            }
        }




        private void NewLoan_Notification(string Lang, Loan loan, EmployeesVM emp, int UserId, int BranchId, string Url, string ImgUrl)
        {

            string Subject = "لديك اشعار ، تقديم طلب السلفة ";

            DateTime loandate = DateTime.ParseExact(loan.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int LastDate = DateTime.DaysInMonth(DateTime.Now.Year, loandate.Month);
            DateTime FirstTerm = new DateTime(DateTime.Now.Year, loandate.Month, LastDate);
            decimal termAmount = loan.Amount.Value / loan.MonthNo.Value;
            var ldate = DateTime.Now.Date;
            try
            {
                ldate = DateTime.ParseExact(loan.Date, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);


            }
            catch
            {

            }
            string NotStr = string.Format(" تم تقديم طلب سلفة خاصة بالموظف {0} بمبلغ {1} - تاريخ بداية الاستقطاع {2}", emp.EmployeeName, loan.Amount, FirstTerm.ToString("yyyy-MM-dd", new CultureInfo("en-US")));
            if (loan.UserId != null)
            {
             
                if (loan.UserId.Value > 0)
                {
                    var branch = _BranchesRepository.GetById(BranchId);
                   
                    string DepartmentNameAr = "";
                    Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == emp.DepartmentId).FirstOrDefault();
                    if (DepName != null)
                    {
                        DepartmentNameAr = DepName.DepartmentNameAr;
                    }
                    string NameAr = "";
                    Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == emp.BranchId).FirstOrDefault();
                    var job = _TaamerProContext.Job.FirstOrDefault(x => x.JobId == emp.JobId);
                    string OrgName = _OrganizationsRepository.GetBranchOrganization().Result.NameAr;

                    if (BranchName != null)
                    {
                        NameAr = BranchName.NameAr;
                    }
                    string htmlBody = "";

                     htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
    <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
    <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
    <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
    <h4> عزيزي الموظف " + emp.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > تم تقديم طلب السلفه الخاصه بكم</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + emp.EmployeeNameAr + @"</td> </tr> <tr> <td>   القسم  </td> <td>" + DepartmentNameAr + @"</td>
     </tr> <tr> <td>   الفرع</td> <td>" + NameAr + @"</td> </tr><tr> <td>   تاريخ الطلب</td> <td>" + ldate + @"</td> </tr> </table> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
    </div> </div></div></div></body></html> ";
                    var config = _employeesService.GetNotificationRecipients(Models.Enums.NotificationCode.HR_AdvanceRequest, emp.EmployeeId);
                    if ( config.Description != null && config.Description != "")
                        Subject = config.Description;

                    if (config.Users != null && config.Users.Count() > 0)
                    {
                        foreach (var usr in config.Users)
                        {
                            var UserNotification = new Notification();
                            UserNotification.ReceiveUserId = usr;
                            UserNotification.Name = Subject;
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
                            UserNotification.NextTime = null;
                            UserNotification.AddUser = UserId;
                            UserNotification.AddDate = DateTime.Now;

                            _TaamerProContext.Notification.Add(UserNotification);
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(usr, Subject, NotStr);

                            _customerMailService.SendMail_SysNotification((int)emp.BranchId, usr, usr, Subject, htmlBody, true);

                            var userObj = _usersRepository.GetById(usr);
                            var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, usr, BranchId);


                        }
                    }
                    else
                    {
                        var UserNotification = new Notification();
                        UserNotification.ReceiveUserId = loan.UserId.Value;
                        UserNotification.Name = Subject;
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
                        UserNotification.NextTime = null;
                        UserNotification.AddUser = UserId;
                        UserNotification.AddDate = DateTime.Now;

                        _TaamerProContext.Notification.Add(UserNotification);
                        _TaamerProContext.SaveChanges();
                        _notificationService.sendmobilenotification((int)loan.UserId.Value, Subject, NotStr);

                        _customerMailService.SendMail_SysNotification((int)emp.BranchId, 0, 0, Subject, htmlBody, true, emp.Email);

                        var userObj = _usersRepository.GetById(loan.UserId.Value);
                        var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                    }
                }
            }
        }



        public bool SendMail_Loan(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false,string empmail=null)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var org = _OrganizationsRepository.GetById(branch);

                var mail = new MailMessage();
                var email = _EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail;
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (_EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(email, _EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(email, "لديك اشعار من نظام تعمير السحابي");
                }
                var title = "";
                var body = "";
                if (type == 1)
                {
                    title = "تم تقديم طلبكم المبين ادناة بنجاح وسيتم الرد عليكم قريبا";
                    body = PopulateBody(textBody, _EmployeesRepository.GetEmployeeById(ReceivedUser, "rtl").Result.EmployeeNameAr, title, "مع تحيات قسم ادارة الموارد البشرية", Url, org.NameAr);
                }
                if (type == 2 || type == 3)
                {
                    body = PopulateBody(textBody, _EmployeesRepository.GetEmployeeById(ReceivedUser, "rtl").Result.EmployeeNameAr, Subject, "مع تحيات قسم ادارة الموارد البشرية", Url, org.NameAr);

                }


                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                if (empmail == null)
                {


                    mail.To.Add(new MailAddress(_usersRepository.GetById(ReceivedUser).Email));
                }
                else
                {
                    mail.To.Add(new MailAddress(empmail));
                }

                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string PopulateBody(string bodytxt, string fullname, string header, string footer, string url, string orgname)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(url))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FullName}", fullname);
            body = body.Replace("{Body}", bodytxt);
            body = body.Replace("{Header}", header);
            body = body.Replace("{Footer}", footer);

            body = body.Replace("{orgname}", orgname);




            return body;
        }


    }
}
