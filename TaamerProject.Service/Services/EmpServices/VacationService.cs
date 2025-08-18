using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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
    public class VacationService :   IVacationService
    {
        private readonly ILoanRepository _LoanRepository;
        private readonly IVacationRepository _VacationRepository;
        private readonly IEmployeesRepository _EmployeesRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly ILoanDetailsRepository _LoanDetailsRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly ICustomerMailService _customerMailService;
        private readonly IVacationTypeRepository _vacationTypeRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
         private readonly IUsersRepository _usersRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IOrganizationsRepository _organizationsRepository;
        private readonly INotificationService _notificationService;
        private readonly IEmployeesService _employeesService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public VacationService(TaamerProjectContext dataContext, ISystemAction systemAction,
            ILoanRepository LoanRepository, IVacationRepository VacationRepository,
            IEmployeesRepository EmployeesRepository, IProjectPhasesTasksRepository ProjectPhasesTasksRepository,
            ILoanDetailsRepository LoanDetailsRepository, INotificationRepository NotificationRepository,
            ICustomerMailService customerMailService, IVacationTypeRepository vacationTypeRepository,
            IUserNotificationPrivilegesService userNotificationPrivilegesService, IUsersRepository usersRepository,
            IProjectRepository projectRepository, IBranchesRepository BranchesRepository, IEmailSettingRepository EmailSettingRepository,
            IOrganizationsRepository organizationsRepository, INotificationService notificationService,
            IEmployeesService employeesService
            )
        {
            _VacationRepository = VacationRepository;
            _EmployeesRepository = EmployeesRepository;
            _ProjectPhasesTasksRepository = ProjectPhasesTasksRepository;
            _LoanRepository = LoanRepository;
            _LoanDetailsRepository = LoanDetailsRepository;
            _NotificationRepository = NotificationRepository;
            _customerMailService = customerMailService;
            _vacationTypeRepository = vacationTypeRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            
            _usersRepository = usersRepository;
            _projectRepository = projectRepository;
            _BranchesRepository = BranchesRepository;
            _EmailSettingRepository = EmailSettingRepository;
            _organizationsRepository = organizationsRepository;
            _notificationService = notificationService;
            this._employeesService = employeesService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<VacationVM>> GetAllVacations(int? EmpId, string SearchText)
        {
            var Vacations = _VacationRepository.GetAllVacations(EmpId, SearchText);
            return Vacations;
        }

        public Task<IEnumerable<VacationVM>> GetAllVacationsArchived(int? EmpId, string SearchText)
        {
            var Vacations = _VacationRepository.GetAllVacationsArchived(EmpId, SearchText);
            return Vacations;
        }
        public Task<IEnumerable<VacationVM>> GetAllVacations2(int? UserId, string SearchText)
        {
            var Vacations = _VacationRepository.GetAllVacations2(UserId, SearchText);
            return Vacations;
        }
        public Task<IEnumerable<VacationVM>> GetAllVacationsw(string SearchText)
        {
            var Vacations = _VacationRepository.GetAllVacationsw(SearchText);
            return Vacations;
        }

        public Task<IEnumerable<VacationVM>> GetAllVacationsw2(string SearchText,int status)
        {
            var Vacations = _VacationRepository.GetAllVacationsw2(SearchText, status);
            return Vacations;
        }
        public async Task<IEnumerable<VacationVM>> GetAllVacationsSearch(VacationVM VacationSearch,int BranchId)
        {
            IEnumerable<VacationVM> result = null;
            List<int> Emps = null;
            if (VacationSearch.IsSearch)
            {
                result =await _VacationRepository.GetAllVacationsBySearchObject(VacationSearch, BranchId);

                if (VacationSearch.StartDate != null)
                {
                    Emps =await _EmployeesRepository.GetAllActiveEmpsByDate(VacationSearch.StartDate, VacationSearch.EndDate);
                }
            }
            else
            {
                result =await _VacationRepository.GetAllVacationsSearch(BranchId);
            }
            if (!VacationSearch.IsSearch || VacationSearch.StartDate == null)
            {
                //Emps = _EmployeesRepository.GetMatching(x => !x.IsDeleted && !string.IsNullOrEmpty(x.WorkStartDate) && string.IsNullOrEmpty(x.EndWorkDate)).Select(y=> y.EmployeeId).ToList();
                 Emps = _TaamerProContext.Employees.Where(x => !x.IsDeleted && !string.IsNullOrEmpty(x.WorkStartDate) && string.IsNullOrEmpty(x.EndWorkDate)).Select(y => y.EmployeeId).ToList();


            }
            result = result.Where(x => Emps.Contains(x.EmployeeId.Value));
            return result;
        }
        public GeneralMessage SaveVacation(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {

            var ExistEmp = _EmployeesRepository.GetEmployeeByUserid(UserId).Result.FirstOrDefault();
            DateTime vacend = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime vacstart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //DateTime oldvacend = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //DateTime oldvacstart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
           // var oldvac = _VacationRepository.GetMatching(s => s.IsDeleted == false && ((DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacend && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacend) || (DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacstart && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacstart)) && s.EmployeeId == ExistEmp.EmployeeId);
            var oldvac = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false && ((DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacend && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacend) || (DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacstart && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacstart)) && s.EmployeeId == ExistEmp.EmployeeId);

            if (ExistEmp == null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الإجازة";
                _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.UserNotAssociatedWithEmployee, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.UserNotAssociatedWithEmployee };
            }
            else if (string.IsNullOrEmpty(ExistEmp.WorkStartDate) || !string.IsNullOrEmpty(ExistEmp.EndWorkDate))
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                string msg = string.IsNullOrEmpty(ExistEmp.WorkStartDate) ? "لا يمكنك عمل طلب إجازة وذلك لعدم مباشرة الموظف للعمل" :
                    "لا يمكنك عمل طلب إجازة وذلك لإنتهاء خدمات الموظف";
                _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
            }else if (oldvac.Count() > 0)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote1 = Resources.General_SavedFailed;
                string msg = "لا يمكنك عمل طلب إجازة وذلك لوجود طلب اجازه في نفس الفترة لهذا الموظف";

                _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
            }
            
            else
            {
                try
                {

            
                  //  var emp = _EmployeesRepository.GetById(ExistEmp.EmployeeId);
                    Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == ExistEmp.EmployeeId).FirstOrDefault();

                    if (emp != null && vacation.VacationId == 0)
                    {
                      
                        var totaldays = 0.0;
                        DateTime resultEnd = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime resultStart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        totaldays = (resultEnd - resultStart).TotalDays + 1;


                        vacation.AddUser = UserId;
                        vacation.EmployeeId = ExistEmp.EmployeeId;
                        vacation.UserId = UserId;

                        vacation.DaysOfVacation = Convert.ToInt32(totaldays);
                        vacation.DecisionType = 0;

                        vacation.BranchId = BranchId;
                        vacation.UserId = emp.UserId;
                        vacation.AddDate = DateTime.Now;
                        _TaamerProContext.Vacation.Add(vacation);

                        // var vactionType = _vacationTypeRepository.GetById(vacation.VacationTypeId);
                        VacationType? vactionType = _TaamerProContext.VacationType.Where(s => s.VacationTypeId == vacation.VacationTypeId).FirstOrDefault();

                        _TaamerProContext.SaveChanges();
                        NewVacation_Notification(Lang, vacation, emp, Lang == "rtl" ? vactionType.NameAr : vactionType.NameEn, UserId, BranchId, Url, ImgUrl);

                        
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة اجازة جديدة";
                        _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm=vacation.VacationId };
                    }
                    else
                    {
                        if (emp != null)
                        {
                            if (emp.UserId != null)
                            {
                               // var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType !=1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                                var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                                if (userTasks > 0)
                                {
                                    var massage2 = "";
                                    if (Lang == "rtl")
                                    {
                                        massage2 =  userTasks + Resources.ExistingWorkTasksTransferredAnotherUser;
                                    }
                                    else
                                    {
                                        massage2 = userTasks + Resources.ExistingWorkTasksTransferredAnotherUser;
                                    }


                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = "فشل في حفظ الإجازة";
                                    _SystemAction.SaveAction("SaveVacation", "VacationService", 1, massage2, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------

                                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage2 };
                                }

                            }
                          //  var userLoan = _LoanRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);
                            var userLoan = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);

                            var userLoanDatails = 0;
                            if (userLoan != null)
                            {
                                foreach (var item in userLoan)
                                {
                                    //userLoanDatails = _LoanDetailsRepository.GetMatching(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();
                                     userLoanDatails = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();

                                }
                            }
                            if (userLoanDatails > 0)
                            {
                                var massage1 = "";
                                if (Lang == "rtl")
                                {
                                    massage1 = Resources.employeeEdvancesBeSettledFirst;
                                }
                                else
                                {
                                    massage1 = Resources.employeeEdvancesBeSettledFirst;
                                }

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote3 = "فشل في حفظ الإجازة";
                                _SystemAction.SaveAction("SaveVacation", "VacationService", 1, massage1 , "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage1 };
                            }
                            //if (userLoan != null)
                            //{
                            //    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =" الموظف عليه سلف لابد من تسويتها اولا " };
                            //}
                        }
                       // var VacationUpdated = _VacationRepository.GetById(vacation.VacationId);
                        Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacation.VacationId).FirstOrDefault();

                        if (VacationUpdated != null)
                        {
                            VacationUpdated.EmployeeId = vacation.EmployeeId;
                            VacationUpdated.VacationTypeId = vacation.VacationTypeId;
                            VacationUpdated.StartDate = vacation.StartDate;
                            VacationUpdated.StartHijriDate = vacation.StartHijriDate;
                            VacationUpdated.EndDate = vacation.EndDate;
                            VacationUpdated.EndHijriDate = vacation.EndHijriDate;
                            VacationUpdated.VacationReason = vacation.VacationReason;
                            VacationUpdated.VacationStatus = vacation.VacationStatus;
                            VacationUpdated.IsDiscount = vacation.IsDiscount;
                            VacationUpdated.DiscountAmount = vacation.DiscountAmount;
                            VacationUpdated.UserId = emp.UserId;
                            VacationUpdated.UpdateUser = UserId;
                            VacationUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل اجازة رقم " + vacation.VacationId;
                        _SystemAction.SaveAction("SaveVacation", "VacationService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                        }
                }
                catch (Exception ex)
                { 
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الإجازة";
                    _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }

            }

        }


        public GeneralMessage SaveVacation_Management(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {

            var ExistEmp = _EmployeesRepository.GetEmployeeByUserid(UserId).Result.FirstOrDefault();
            DateTime vacend = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime vacstart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //DateTime oldvacend = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //DateTime oldvacstart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var exemp = 0;
            if(ExistEmp != null)
            {
                exemp = ExistEmp.EmployeeId;
            }
            //var oldvac = _VacationRepository.GetMatching(s => s.IsDeleted == false && ((DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacend && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacend) || (DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacstart && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacstart)) && s.EmployeeId == exemp);
            //var oldvac = _TaamerProContext.Vacation.Where((s => s.IsDeleted == false && ((DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacend && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacend) || (DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacstart && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacstart)) && s.EmployeeId == exemp));
            
            if (ExistEmp == null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الإجازة";
                _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.UserNotAssociatedWithEmployee, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.UserNotAssociatedWithEmployee };
            }
            else if (string.IsNullOrEmpty(ExistEmp.WorkStartDate) || !string.IsNullOrEmpty(ExistEmp.EndWorkDate))
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                string msg = string.IsNullOrEmpty(ExistEmp.WorkStartDate) ? "لا يمكنك عمل طلب إجازة وذلك لعدم مباشرة الموظف للعمل" :
                    "لا يمكنك عمل طلب إجازة وذلك لإنتهاء خدمات الموظف";
                _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
            }
            //else if  (oldvac !=null &&oldvac.Count() > 0)
            //{
            //    //-----------------------------------------------------------------------------------------------------------------
            //    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            //    string ActionNote1 = Resources.General_SavedFailed;
            //    string msg = "لا يمكنك عمل طلب إجازة وذلك لوجود طلب اجازه في نفس الفترة لهذا الموظف";

            //    _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
            //    //-----------------------------------------------------------------------------------------------------------------

            //    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
            //}

            else
            {
                try
                {


                    //var emp = _EmployeesRepository.GetById(ExistEmp.EmployeeId);
                    Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == ExistEmp.EmployeeId).FirstOrDefault();


                    if (emp != null && vacation.VacationId == 0)
                    {

                        var totaldays = 0.0;
                        DateTime resultEnd = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime resultStart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        totaldays = (resultEnd - resultStart).TotalDays + 1;


                        vacation.AddUser = UserId;
                        vacation.EmployeeId = ExistEmp.EmployeeId;
                        vacation.UserId = UserId;

                        vacation.DaysOfVacation = Convert.ToInt32(totaldays);
                        vacation.DecisionType = vacation.DecisionType;

                        vacation.BranchId = BranchId;
                        vacation.UserId = emp.UserId;
                        vacation.AddDate = DateTime.Now;
                        _TaamerProContext.Vacation.Add(vacation);

                        //var vactionType = _vacationTypeRepository.GetById(vacation.VacationTypeId);
                        VacationType? vactionType = _TaamerProContext.VacationType.Where(s => s.VacationTypeId == vacation.VacationTypeId).FirstOrDefault();

                        _TaamerProContext.SaveChanges();
                        NewVacation_Notification(Lang, vacation, emp, Lang == "rtl" ? vactionType.NameAr : vactionType.NameEn, UserId, BranchId, Url, ImgUrl);


                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة اجازة جديدة";
                        _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm= vacation.VacationId};
                        }
                    else
                    {
                        if (emp != null)
                        {
                            if (emp.UserId != null)
                            {
                                //var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                                var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                                if (userTasks > 0)
                                {
                                    var massage2 = "";
                                    if (Lang == "rtl")
                                    {
                                        massage2 = "الموظف عليه " + userTasks + " مهام عمل حاليه يجب تحويلها لمستخدم اخر";
                                    }
                                    else
                                    {
                                        massage2 = "this Employee have" + userTasks + " tasks must be turn it";
                                    }


                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = "فشل في حفظ الإجازة";
                                    _SystemAction.SaveAction("SaveVacation", "VacationService", 1, massage2, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------

                                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage2 };
                                }

                            }
                           // var userLoan = _LoanRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);
                            var userLoan = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);

                            var userLoanDatails = 0;
                            if (userLoan != null)
                            {
                                foreach (var item in userLoan)
                                {
                                    userLoanDatails = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();

                                    //userLoanDatails = _LoanDetailsRepository.GetMatching(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();
                                }
                            }
                            if (userLoanDatails > 0)
                            {
                                var massage1 = "";
                                if (Lang == "rtl")
                                {
                                    massage1 = " الموظف عليه سلف لابد من تسويتها اولا ";
                                }
                                else
                                {
                                    massage1 = "An employee with ancestor who must be settled first";
                                }

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote3 = "فشل في حفظ الإجازة";
                                _SystemAction.SaveAction("SaveVacation", "VacationService", 1, massage1, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage1 };
                            }
                            //if (userLoan != null)
                            //{
                            //    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =" الموظف عليه سلف لابد من تسويتها اولا " };
                            //}
                        }
                        //var VacationUpdated = _VacationRepository.GetById(vacation.VacationId);
                        Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacation.VacationId).FirstOrDefault();

                        if (VacationUpdated != null)
                        {
                            VacationUpdated.EmployeeId = vacation.EmployeeId;
                            VacationUpdated.VacationTypeId = vacation.VacationTypeId;
                            VacationUpdated.StartDate = vacation.StartDate;
                            VacationUpdated.StartHijriDate = vacation.StartHijriDate;
                            VacationUpdated.EndDate = vacation.EndDate;
                            VacationUpdated.EndHijriDate = vacation.EndHijriDate;
                            VacationUpdated.VacationReason = vacation.VacationReason;
                            VacationUpdated.VacationStatus = vacation.VacationStatus;
                            VacationUpdated.IsDiscount = vacation.IsDiscount;
                            VacationUpdated.DiscountAmount = vacation.DiscountAmount;
                            VacationUpdated.UserId = emp.UserId;
                            VacationUpdated.UpdateUser = UserId;
                            VacationUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل اجازة رقم " + vacation.VacationId;
                        _SystemAction.SaveAction("SaveVacation", "VacationService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                        }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الإجازة";
                    _SystemAction.SaveAction("SaveVacation", "VacationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }

            }

        }
        public GeneralMessage SaveVacation2(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {
            //var ExistEmp = _EmployeesRepository.GetEmployeeByUserid(BranchId, UserId);
            //var UserID_E = _EmployeesRepository.GetById(vacation.EmployeeId).UserId;
            int userId;
            Employees? UserID_E = _TaamerProContext.Employees.Where(s => s.EmployeeId == vacation.EmployeeId).FirstOrDefault();
            if (UserID_E != null)
            {
                userId = (int)UserID_E.UserId;
            }
            if (UserID_E == null)
            {
              
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الإجازة";
                _SystemAction.SaveAction("SaveVacation2", "VacationService", 1,Resources.UserNotAssociatedWithEmployee, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.UserNotAssociatedWithEmployee };
            }
            else
            {
                try
                {
                    //var emp = _EmployeesRepository.GetById(vacation.EmployeeId);
                    Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == vacation.EmployeeId).FirstOrDefault();

                    if (string.IsNullOrEmpty(emp.WorkStartDate) || !string.IsNullOrEmpty(emp.EndWorkDate))
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = Resources.General_SavedFailed;
                        string msg = string.IsNullOrEmpty(emp.WorkStartDate) ? "لا يمكنك عمل طلب إجازة وذلك لعدم مباشرة الموظف للعمل" :
                            "لا يمكنك عمل طلب إجازة وذلك لإنتهاء خدمات الموظف";
                        _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
                    }
                    if (emp != null && vacation.VacationId == 0)
                    {
                        var totaldays = 0.0;
                        DateTime resultEnd = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime resultStart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        totaldays = (resultEnd - resultStart).TotalDays + 1;


                        vacation.AddUser = UserId;
                        vacation.EmployeeId = emp.EmployeeId; //ExistEmp.FirstOrDefault().EmployeeId;
                        vacation.UserId = UserID_E.UserId;

                        vacation.DaysOfVacation = Convert.ToInt32(totaldays);
                        vacation.DecisionType = 0;
                        
                        vacation.BranchId = BranchId;
                        //vacation.UserId = emp.UserId;
                        vacation.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        vacation.AddDate = DateTime.Now;
                        _TaamerProContext.Vacation.Add(vacation);

                       // var vactionType = _vacationTypeRepository.GetById(vacation.VacationTypeId);
                        VacationType? vactionType = _TaamerProContext.VacationType.Where(s => s.VacationTypeId == vacation.VacationTypeId).FirstOrDefault();

                        NewVacation_Notification(Lang, vacation, emp, Lang == "rtl" ? vactionType.NameAr : vactionType.NameEn, UserId, BranchId, Url, ImgUrl);
                        
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة اجازة جديدة";
                        _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------


                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                        }
                    else
                    {
                        if(vacation.VacationStatus==2)
                        {
                            string Message = Lang == "rtl" ? "لا يمكن التعديل على الإجازة, تم الموافقة عليها" : "Can't modify on an acceptted vacation";
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = "فشل في تعديل الإجازة";
                            _SystemAction.SaveAction("SaveVacation2", "VacationService", 2, Message, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Message };
                        }
                        if (emp != null)
                        {
                            if (emp.UserId != null)
                            {
                                //var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                                var userTasks = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();

                                if (userTasks > 0)
                                {
                                    var massage2 = "";
                                    if (Lang == "rtl")
                                    {
                                        massage2 = "الموظف عليه " + userTasks + " مهام عمل حاليه يجب تحويلها لمستخدم اخر";
                                    }
                                    else
                                    {
                                        massage2 = "this Employee have" + userTasks + " tasks must be turn it";
                                    }
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote3 = "فشل في تعديل الإجازة";
                                    _SystemAction.SaveAction("SaveVacation2", "VacationService", 2, massage2, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                                    //-----------------------------------------------------------------------------------------------------------------

                                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage2 };
                                }

                            }
                            //var userLoan = _LoanRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);
                            var userLoan = _TaamerProContext.Loan.Where(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);

                            var userLoanDatails = 0;
                           
                            if (userLoan != null)
                            {
                                foreach (var item in userLoan)
                                {
                                    //userLoanDatails = _LoanDetailsRepository.GetMatching(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();
                                    userLoanDatails = _TaamerProContext.LoanDetails.Where(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();

                                }
                            }
                            if (userLoanDatails > 0)
                            {
                                var massage1 = "";
                                if (Lang == "rtl")
                                {
                                    massage1 = " الموظف عليه سلف لابد من تسويتها اولا ";
                                }
                                else
                                {
                                    massage1 = "An employee with ancestor who must be settled first";
                                }

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في تعديل الإجازة";
                                _SystemAction.SaveAction("SaveVacation2", "VacationService", 2, massage1, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage1 };
                            }
                            //if (userLoan != null)
                            //{
                            //    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =" الموظف عليه سلف لابد من تسويتها اولا " };
                            //}
                        }
                        
                       // var VacationUpdated = _VacationRepository.GetById(vacation.VacationId);
                        Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacation.VacationId).FirstOrDefault();

                        if (VacationUpdated != null)
                        {
                            VacationUpdated.EmployeeId = vacation.EmployeeId;
                            VacationUpdated.VacationTypeId = vacation.VacationTypeId;
                            VacationUpdated.StartDate = vacation.StartDate;
                            VacationUpdated.StartHijriDate = vacation.StartHijriDate;
                            VacationUpdated.EndDate = vacation.EndDate;
                            VacationUpdated.EndHijriDate = vacation.EndHijriDate;
                            VacationUpdated.VacationReason = vacation.VacationReason;
                            VacationUpdated.VacationStatus = vacation.VacationStatus;
                            VacationUpdated.IsDiscount = vacation.IsDiscount;
                            VacationUpdated.DiscountAmount = vacation.DiscountAmount;
                            VacationUpdated.UserId = emp.UserId;
                            VacationUpdated.UpdateUser = UserId;
                            VacationUpdated.UpdateDate = DateTime.Now;

                        }
                        _TaamerProContext.SaveChanges();


                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل اجازة رقم " + vacation.VacationId;
                        _SystemAction.SaveAction("SaveVacation2", "VacationService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                        }
                   
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الإجازة";
                    _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
            }
        }
        public GeneralMessage SaveVacationWorkers(Vacation vacation, int UserId, int BranchId, string Lang, string Url, string ImgUrl)
        {
            try
            {
                // var emp = _EmployeesRepository.GetById(vacation.EmployeeId);
                Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == vacation.EmployeeId).FirstOrDefault();

                if (emp != null && string.IsNullOrEmpty(emp.WorkStartDate) || !string.IsNullOrEmpty(emp.EndWorkDate))
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    string msg = string.IsNullOrEmpty(emp.WorkStartDate) ? "لا يمكنك عمل طلب إجازة وذلك لعدم مباشرة الموظف للعمل" :
                        "لا يمكنك عمل طلب إجازة وذلك لإنتهاء خدمات الموظف";
                    _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
                }
                
                DateTime vacend = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime vacstart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //DateTime oldvacend = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //DateTime oldvacstart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                // var oldvac = _VacationRepository.GetMatching(s => s.IsDeleted == false && ( (DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)<= vacend && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacend) || ( DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacstart && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacstart)) && s.EmployeeId == vacation.EmployeeId);
                var oldvac = _TaamerProContext.Vacation.Where(s => s.IsDeleted == false).ToList();
                  var oldv=  oldvac.Where(s=>((DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacend && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacend) || (DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= vacstart && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= vacstart)) && s.EmployeeId == vacation.EmployeeId).ToList();

                if (oldv.Count() > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.General_SavedFailed;
                    string msg = "لا يمكنك عمل طلب إجازة وذلك لوجود طلب اجازه في نفس الفترة لهذا الموظف";

                    _SystemAction.SaveAction("SaveVacation2", "VacationService", 1, msg, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =msg };
                }

 
                if (emp != null && vacation.VacationId == 0)
                {
                    var totaldays = 0.0;
                    DateTime resultEnd = DateTime.ParseExact(vacation.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime resultStart = DateTime.ParseExact(vacation.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    totaldays = (resultEnd - resultStart).TotalDays + 1;


                    vacation.AddUser = UserId;
                    vacation.EmployeeId = vacation.EmployeeId;
                    vacation.UserId = UserId;

                    vacation.DaysOfVacation = Convert.ToInt32(totaldays);
                    vacation.DecisionType = 0;

                    vacation.BranchId = BranchId;
                    vacation.UserId = emp.UserId;
                    vacation.AddDate = DateTime.Now;
                    _TaamerProContext.Vacation.Add(vacation);
                    _TaamerProContext.SaveChanges();
                    try {
                        // var vactionType = _vacationTypeRepository.GetById(vacation.VacationTypeId);
                        VacationType? vactionType = _TaamerProContext.VacationType.Where(s => s.VacationTypeId == vacation.VacationTypeId).FirstOrDefault();

                        NewVacation_Notification(Lang, vacation, emp, Lang == "rtl" ? vactionType.NameAr : vactionType.NameEn, UserId, BranchId,  Url, ImgUrl);
                    }
                    catch (Exception ex)
                    {

                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل اجازة رقم " + vacation.VacationId;
                    _SystemAction.SaveAction("SaveVacationWorkers", "VacationService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = vacation.VacationId};
                    }
                else
                {
                    //if (emp != null && emp.UserId != null)
                    //{
                    //    var userTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.UserId == emp.UserId && s.Type == 3 && s.BranchId == BranchId && s.Status != 4).Count();
                    //    if (userTasks > 0)
                    //    {
                    //        var massage2 = "";
                    //        if (Lang == "rtl")
                    //        {
                    //            massage2 = "الموظف عليه " + userTasks + " مهام عمل حاليه يجب تحويلها لمستخدم اخر";
                    //        }
                    //        else
                    //        {
                    //            massage2 = "this Employee have" + userTasks + " tasks must be turn it";
                    //        }
                    //        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage2 };
                    //    }

                    //}
                    //if (emp != null)
                    //{
                    //    var userLoan = _LoanRepository.GetMatching(s => s.IsDeleted == false && s.EmployeeId == emp.EmployeeId);
                    //    var userLoanDatails = 0;
                    //    if (userLoan != null)
                    //    {
                    //        foreach (var item in userLoan)
                    //        {
                    //            userLoanDatails = _LoanDetailsRepository.GetMatching(s => s.IsDeleted == false && s.LoanId == item.LoanId && s.Finished == false).Count();
                    //        }
                    //    }
                    //    if (userLoanDatails > 0)
                    //    {
                    //        var massage1 = "";
                    //        if (Lang == "rtl")
                    //        {
                    //            massage1 = " الموظف عليه سلف لابد من تسويتها اولا ";
                    //        }
                    //        else
                    //        {
                    //            massage1 = "An employee with ancestor who must be settled first";
                    //        }
                    //        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage1 };
                    //    }
                    //    //if (userLoan != null)
                    //    //{
                    //    //    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =" الموظف عليه سلف لابد من تسويتها اولا " };
                    //    //}
                    //}
                    // var VacationUpdated = _VacationRepository.GetById(vacation.VacationId);
                    Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacation.VacationId).FirstOrDefault();

                    if (VacationUpdated != null)
                    {
                        VacationUpdated.EmployeeId = vacation.EmployeeId;
                        VacationUpdated.VacationTypeId = vacation.VacationTypeId;
                        VacationUpdated.StartDate = vacation.StartDate;
                        VacationUpdated.StartHijriDate = vacation.StartHijriDate;
                        VacationUpdated.EndDate = vacation.EndDate;
                        VacationUpdated.EndHijriDate = vacation.EndHijriDate;
                        VacationUpdated.VacationReason = vacation.VacationReason;
                        VacationUpdated.VacationStatus = vacation.VacationStatus;
                        VacationUpdated.IsDiscount = vacation.IsDiscount;
                        VacationUpdated.DiscountAmount = vacation.DiscountAmount;
                        VacationUpdated.UserId = emp.UserId;
                        VacationUpdated.UpdateUser = UserId;
                        VacationUpdated.UpdateDate = DateTime.Now;
                    }
                    
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة اجازة جديدة";
                    _SystemAction.SaveAction("SaveVacationWorkers", "VacationService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = vacation.VacationId };
                    }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الإجازة";
                _SystemAction.SaveAction("SaveVacationWorkers", "VacationService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }


        }


        public GeneralMessage SaveVacationImage(int vacationid, int UserId, int BranchId, int? yearid, string FileName, string FileUrl)
        {
            if (vacationid > 0)
            {
                if (FileName != "")
                {
                    // var vacupdate = _VacationRepository.GetById(vacationid);
                    Vacation? vacupdate = _TaamerProContext.Vacation.Where(s => s.VacationId == vacationid).FirstOrDefault();
                    if (vacupdate != null)
                    {
                        vacupdate.FileName = FileName;
                        vacupdate.FileUrl = FileUrl;
                        _TaamerProContext.SaveChanges();
                    }
                   
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                else
                {
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                    }
            }
            else
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
        }



        public GeneralMessage CheckIfHaveTasks(int VacationId, string Lang)
        {
            var massage2 = "";
           // var vac = _VacationRepository.GetById(VacationId);
            Vacation? vac = _TaamerProContext.Vacation.Where(s => s.VacationId == VacationId).FirstOrDefault();

            // var emp = _EmployeesRepository.GetById(vac.EmployeeId);
            Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == vac.EmployeeId).FirstOrDefault();

            if (emp != null && emp.UserId != null)
            {
                //var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 2 || s.Status == 1)).ToList(); //running or not started yet
                var PhasesTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false && (s.Project.StopProjectType != 1) && s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 2 || s.Status == 1)).ToList(); //running or not started yet
                
                //var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 2 || s.Status == 1)).ToList(); //running or not started yet

                PhasesTask = PhasesTask.Where(m => (string.IsNullOrEmpty(vac.StartDate) || (!string.IsNullOrEmpty(m.ExcpectedStartDate) &&
                                         DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(vac.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) &&
                                         (string.IsNullOrEmpty(vac.EndDate) || string.IsNullOrEmpty(m.ExcpectedEndDate) || DateTime.ParseExact(m.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(vac.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();

                if ((PhasesTask != null || PhasesTask.Count > 0)) // && Type == 2
                {
                    if (Lang == "rtl")
                    {
                        massage2 = "الموظف عليه " + PhasesTask.Count + " مهام عمل في فترة الإجازة";

                        //massage2 = "الموظف عليه " + PhasesTask.Count + " مهام عمل في فترة الإجازة يجب تحويلها لمستخدم اخر";
                    }
                    else
                    {
                        massage2 = "This Employee have" + PhasesTask.Count + " tasks at the vaction period must be turn it";
                    }
                }
            }
            return new GeneralMessage() {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage2};
        }

        private void NewVacation_Notification(string Lang, Vacation vac, Employees emp,string VactionTypeName, int UserId, int BranchId, string Url, string ImgUrl) 
        {
            try
            {
                string Subject = "لديك اشعار ، تقديم طلب إجازة";
                string NotStr = string.Format("تم تقديم طلب لإجازة خاصة بالموظف {0} من تاريخ {1} إلى تاريخ {2}", emp.EmployeeNameAr, vac.StartDate, vac.EndDate);

                //Notification
                int ResponSibleUser = vac.UserId.HasValue ? vac.UserId.Value : vac.AddUser.Value;
                string DepartmentNameAr = "";
                Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == emp.DepartmentId).FirstOrDefault();
                if (DepName != null)
                {
                    DepartmentNameAr = DepName.DepartmentNameAr;
                }
                string NameAr = "";
                Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == emp.BranchId).FirstOrDefault();
                var job = _TaamerProContext.Job.FirstOrDefault(x => x.JobId == emp.JobId);
                string OrgName = _organizationsRepository.GetBranchOrganization().Result.NameAr;

                if (BranchName != null)
                {
                    NameAr = BranchName.NameAr;
                }
                string htmlBody = "";
                List<int> recept = new List<int>();

                htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                            <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                            <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                            <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                            <h4> عزيزي الموظف " + emp.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > تم تقديم طلب الاجازة الخاصه بكم</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + emp.EmployeeNameAr + @"</td> </tr> <tr> <td>   نوع الإجازة  </td> <td>" + VactionTypeName + @"</td>
                             </tr> <tr> <td>   من</td> <td>" + vac.StartDate + @"</td> </tr><tr> <td>   إلى</td> <td>" + vac.EndDate + @"</td> </tr> </table> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
                            </div> </div></div></div></body></html> ";

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var config = _employeesService.GetNotificationRecipients(Models.Enums.NotificationCode.HR_LeaveRequest, emp.EmployeeId);
                if (config.Description != null && config.Description != "")
                    Subject = config.Description;

                if (config.Users != null && config.Users.Count() > 0)
                {
                    recept = config.Users;
                }
                else
                {
                    if (ResponSibleUser > 0)
                    {
                        recept.Add(ResponSibleUser);
                    }
                }
                foreach (var usr in recept)
                {
                    
              
                        // Notification

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
                        UserNotification.AddUser = UserId;
                        UserNotification.AddDate = DateTime.Now;
                        _TaamerProContext.Notification.Add(UserNotification);
                        _TaamerProContext.SaveChanges();
                        _notificationService.sendmobilenotification(usr, Subject, NotStr);

                        //mail
                        _customerMailService.SendMail_SysNotification((int)emp.BranchId, usr, usr, Subject, htmlBody, true);


                        Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == ResponSibleUser).FirstOrDefault();

                        var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                    
                }
            }
            catch(Exception ex)
            { 
            }
        }


        public bool SendMail_Vacation(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false,string empmail=null)
        {
            try
            {

                int OrganizationId;
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                if (branch != null)
                {
                    OrganizationId = branch.OrganizationId;
                }

                Organizations? org = _TaamerProContext.Organizations.Where(s => s.OrganizationId == branch.OrganizationId).FirstOrDefault();


                var mail = new MailMessage();
                var email = _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail;
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);
                // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(email, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
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
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة الموارد البشرية", Url, org.NameAr);
                }
                if( type==2 || type == 3)
                {
                    body = PopulateBody(textBody, _EmployeesRepository.GetEmployeeById(ReceivedUser, "rtl").Result.EmployeeNameAr, Subject, "مع تحيات قسم ادارة الموارد البشرية", Url,org.NameAr);

                }


                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);

               // var receivedUser = _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                if (empmail != null && empmail != "")
                {
                    mail.To.Add(new MailAddress(empmail));
                }
                else
                {


                    //mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));
                }
                mail.Subject = Subject;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string PopulateBody(string bodytxt, string fullname, string header, string footer, string url,string orgname)
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

        public GeneralMessage UpdateVacation(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl, string? reason)
        {
            try
            {
                // var VacationUpdated = _VacationRepository.GetById(vacationId);
                Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacationId).FirstOrDefault();

                //var VactionType = _vacationTypeRepository.GetById(VacationUpdated.VacationTypeId);
                VacationType? VactionType = _TaamerProContext.VacationType.Where(s => s.VacationTypeId == VacationUpdated.VacationTypeId).FirstOrDefault();

                // var emp = _EmployeesRepository.GetById(VacationUpdated.EmployeeId);
                Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == VacationUpdated.EmployeeId).FirstOrDefault();


                DateTime StartWork = DateTime.MinValue;
                if (string.IsNullOrEmpty(emp.WorkStartDate))
                    StartWork = DateTime.ParseExact(emp.WorkStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                string startDate = Utilities.ConvertDateCalendar(new DateTime(DateTime.Now.Year, 1, 1), "Gregorian", "en-US");
                string endDate = Utilities.ConvertDateCalendar(new DateTime(DateTime.Now.Year, 12, 31), "Gregorian", "en-US");



                if (emp != null && Type == 2)
                {
                    //Stop tasks untile come back
                    if (emp.UserId != null)
                    {


                        //var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false /*&& (s.Project.StopProjectType != 1) */&& s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 2)).ToList(); //running 
                        var PhasesTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false /*&& (s.Project.StopProjectType != 1) */&& s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 2)).ToList(); //running  
                        if ((PhasesTask != null || PhasesTask.Count > 0)) // && Type == 2
                        {

                            PhasesTask = PhasesTask.Where(s => s.Project?.StopProjectType != 1).ToList();
                            //var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 2)).ToList(); //running 

                            PhasesTask = PhasesTask.Where(m => (string.IsNullOrEmpty(VacationUpdated.StartDate) || (!string.IsNullOrEmpty(m.ExcpectedStartDate) &&
                                                    DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationUpdated.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) &&
                                                    (string.IsNullOrEmpty(VacationUpdated.EndDate) || string.IsNullOrEmpty(m.ExcpectedEndDate) || DateTime.ParseExact(m.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationUpdated.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();

                            if ((PhasesTask != null || PhasesTask.Count > 0)) // && Type == 2
                            {
                                foreach (var task in PhasesTask)
                                {
                                    task.Status = 3;
                                }
                            }
                        }
                    }


                    //يبدأ يبقى ليه رصيد إجازات (منصوص في العقد ) من بعد بداية عمله بعام 

                    #region Vacation Balance Commented
                    int vactionNetDays = this.GetVacationDays_WithoutHolidays(VacationUpdated.StartDate, VacationUpdated.EndDate, emp.EmployeeId, Lang, Con, (int)VacationUpdated.VacationTypeId).Count();

                    if (!(bool)VacationUpdated.IsDiscount && emp.VacationEndCount != null) //الخصم من الرصيد 
                    {
                        //if (VacationUpdated.DiscountAmount != null || vactionNetDays > emp.VacationEndCount)//الخصم من الرصيد < أيام الإجازة 
                        if (vactionNetDays > emp.VacationEndCount && (VacationUpdated.DiscountAmount != null && VacationUpdated.DiscountAmount > 0))//الخصم من الرصيد < أيام الإجازة 
                        {
                            if (VacationUpdated.IsDiscount == true && VacationUpdated.DiscountAmount == 0)
                            {
                                VacationUpdated.DiscountAmount = 0;

                            }
                            else
                            {


                                VacationUpdated.DiscountAmount = (vactionNetDays - emp.VacationEndCount) * (emp.Salary / 30);
                            }
                            emp.VacationEndCount = 0;
                        }
                        else  //الخصم من الرصيد إختياري و يكفي أيام الإجازة 
                        {
                            if (emp.VacationEndCount > 0)
                            {
                                emp.VacationEndCount = emp.VacationEndCount - vactionNetDays;
                            }
                            else // عند الطلب كان مختار رصيد الإجازات فقط و كان مكفي وقتها 
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في الموافقة على الاجازة";
                                _SystemAction.SaveAction("UpdateVacation", "VacationService", 2, "الموظف ليس لديه رصيد إجازات كافي", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "الموظف ليس لديه رصيد إجازات كافي" };
                            }
                        }
                    }
                    else //الخصم من المرتب إختياري 
                    {
                        if (VacationUpdated.IsDiscount == true && VacationUpdated.DiscountAmount == 0)
                        {
                            VacationUpdated.DiscountAmount = 0;
                        }
                        else
                        {
                            VacationUpdated.DiscountAmount = vactionNetDays * (emp.Salary / 30);
                        }
                        VacationUpdated.IsDiscount = true;
                    }
                }
                #endregion

                if (VacationUpdated != null)
                {
                    VacationUpdated.EmployeeId = VacationUpdated.EmployeeId;
                    VacationUpdated.AcceptedDate = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                    VacationUpdated.VacationTypeId = VacationUpdated.VacationTypeId;
                    VacationUpdated.StartDate = VacationUpdated.StartDate;
                    VacationUpdated.StartHijriDate = VacationUpdated.StartHijriDate;
                    VacationUpdated.EndDate = VacationUpdated.EndDate;
                    VacationUpdated.EndHijriDate = VacationUpdated.EndHijriDate;
                    VacationUpdated.VacationReason = VacationUpdated.VacationReason;
                    VacationUpdated.VacationStatus = Type;
                    VacationUpdated.IsDiscount = VacationUpdated.IsDiscount;
                    VacationUpdated.DiscountAmount = VacationUpdated.DiscountAmount;
                    VacationUpdated.UserId = emp.UserId;
                    VacationUpdated.UpdateUser = UserId;
                    VacationUpdated.UpdateDate = DateTime.Now;
                    VacationUpdated.AcceptedUser = UserId;
                    VacationUpdated.VacationReason = reason;
                    //emp.VacationEndCount = _VacationRepository.GetAllVacationsBySearchObject(new VacationVM()
                    //{
                    //    StartDate = startDate,
                    //    EndDate = endDate,
                    //    VacationStatus = 2,
                    //    EmployeeId = emp.EmployeeId
                    //}, emp.BranchId.HasValue ? emp.BranchId.Value : 0).Sum(x => x.DaysOfVacation);

                    string Subject = Lang == "rtl" ?
                       (Type == 2 ? "إشعار بالموافقة على الإجازة" :
                        Type == 3 ? "إشعار بالرفض على الإجازة" : Type == 4 || Type == 5 ? "إشعار بتحديث حالة طلب الإجازة" : "")
                        : (Type == 2 ? "Notice of vacation acceptance" :
                          Type == 3 ? "Notice of vacation rejection" : Type == 4 || Type == 5 ? "Notice of vacation status modification" : "");

                    string NotStr = string.Format("الإجازة الخاصة بالموظف {0} من تاريخ {1} إلى تاريخ {2}", emp.EmployeeNameAr, VacationUpdated.StartDate, VacationUpdated.EndDate);
                    NotStr = NotStr + " " + (Type == 2 ? "تم الموافقة عليها" :
                                Type == 3 ? "تم رفضها" :
                                Type == 4 || Type == 5 ? "أصبحت تحت الطلب" : "أصبحت تحت الطلب");

                    //Notification
                    int ResponSibleUser = VacationUpdated.UserId.HasValue ? VacationUpdated.UserId.Value : VacationUpdated.AddUser.Value;
                    var directmanager = _TaamerProContext.Employees.Where(x => x.EmployeeId == emp.DirectManager).FirstOrDefault();

                    try
                    {
                        if (Type == 2 || Type == 3)
                        {

                            string DepartmentNameAr = "";
                            Department? DepName = _TaamerProContext.Department.Where(s => s.DepartmentId == emp.DepartmentId).FirstOrDefault();
                            if (DepName != null)
                            {
                                DepartmentNameAr = DepName.DepartmentNameAr;
                            }
                            string NameAr = "";
                            Branch? BranchName = _TaamerProContext.Branch.Where(s => s.BranchId == emp.BranchId).FirstOrDefault();
                            var job = _TaamerProContext.Job.FirstOrDefault(x => x.JobId == emp.JobId);
                            string OrgName = _organizationsRepository.GetBranchOrganization().Result.NameAr;
                            if (BranchName != null)
                            {
                                NameAr = BranchName.NameAr;
                            }
                            var refusereason = "";
                            if (Type == 3 && reason != null && reason != "" && reason != "undefined")
                            {
                                refusereason = "<tr><td>السبب</td><td>" + reason + @"</td></tr>";

                            }
                            string htmlBody = "";

                            htmlBody = @"<!DOCTYPE html><html lang = ''><head><meta name='viewport' content='width=device-width, height=device-height, initial-scale=1.0, maximum-scale=1.0, user-scalable=0'><meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                        <meta charset = 'utf-8><meta name = 'description' content = ''><meta name = 'keywords' content = ''><meta name = 'csrf-token' content = ''><title></title><link rel = 'icon' type = 'image/x-icon' href = ''></head>
                                        <body style = 'background:#f9f9f9;direction:rtl'><div class='container' style='max-width:630px;padding-right: var(--bs-gutter-x, .75rem); padding-left: var(--bs-gutter-x, .75rem); margin-right: auto;  margin-left: auto;'>
                                        <style> .bordered {width: 550px; height: 700px; padding: 20px;border: 3px solid yellowgreen; background-color:lightgray;} </style>
                                        <div class= 'row' style = 'font-family: Cairo, sans-serif'>  <div class= 'card' style = 'padding: 2rem;background:#fff'> <div style = 'width: 550px; height: 700px; padding: 20px; border: 3px solid yellowgreen; background-color: lightgray;'> <p style='text-align:center'></p>
                                        <h4> عزيزي الموظف " + emp.EmployeeNameAr + "</h4> <h4> السلام عليكم ورحمة الله وبركاتة</h4> <h3 style = 'text-align:center;' > تم تقديم طلب الاجازة الخاصه بكم</h3><table align = 'center' border = '1' ><tr> <td>  الموظف</td><td>" + emp.EmployeeNameAr + @"</td> </tr> <tr> <td>   نوع الإجازة  </td> <td>" + VactionType.NameAr + @"</td>
                                         </tr> <tr> <td>   من</td> <td>" + VacationUpdated.StartDate + @"</td> </tr><tr> <td>   إلى</td> <td>" + VacationUpdated.EndDate + @"</td> </tr> " + refusereason + @"</table> <p style = 'text-align:center'> " + OrgName + @" </p> <h7> مع تحيات قسم ادارة الموارد البشرية</h7>
	
                                        </div> </div></div></div></body></html> ";
                            List<int> recept = new List<int>();
                            var code = Type == 2 ? Models.Enums.NotificationCode.HR_LeaveAccepted : Models.Enums.NotificationCode.HR_LeaveRejected;
                            var config = _employeesService.GetNotificationRecipients(code, VacationUpdated.EmployeeId);

                            if (config.Description != null && config.Description != "")
                                Subject = config.Description;

                            if (config.Users != null && config.Users.Count() > 0)
                            {
                                recept = config.Users;
                            }
                            else
                            {
                                recept.Add(emp.UserId.Value);
                                if (directmanager.UserId.Value > 0)
                                    recept.Add(directmanager.UserId.Value);

                            }
                            if ((emp.UserId == null || emp.UserId == 0) && config.mail != null && config.mail != "")
                            {
                                _customerMailService.SendMail_SysNotification((int)emp.BranchId, 0, 0, Subject, htmlBody, true, emp.Email);

                            }
                            foreach (var usr in recept.Distinct())
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
                                UserNotification.AddUser = UserId;
                                UserNotification.AddDate = DateTime.Now;
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();


                                _notificationService.sendmobilenotification(usr, Subject, NotStr);


                                _customerMailService.SendMail_SysNotification((int)emp.BranchId, usr, usr, Subject, htmlBody, true);

                                Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == usr).FirstOrDefault();

                                var res = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                //}
                            }
                        }
                    }
                    catch
                    {

                    }
                    /////
                }


                _TaamerProContext.SaveChanges();

                var massage = "";
                //if (Lang == "rtl")
                //{
                massage = (Type == 2) ? Resources.MP_VacationAccept : Resources.General_SavedSuccessfully;
                //}
                //else
                //{
                //    massage = "Saved Successfully";
                //}


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل اجازة برقم" + vacationId;
                _SystemAction.SaveAction("UpdateVacation", "VacationService", 2, massage, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = massage };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل الاجازة";
                _SystemAction.SaveAction("UpdateVacation", "VacationService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage UpdateDecisionType_V(int vacationId, int UserId, int BranchId, string Lang, int DecisionType)
        {
            try
            {
              

                // var vac = _VacationRepository.GetById(VacationId);
                Vacation? vac = _TaamerProContext.Vacation.Where(s => s.VacationId == vacationId).FirstOrDefault();

                // var emp = _EmployeesRepository.GetById(vac.EmployeeId);
                Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == vac.EmployeeId).FirstOrDefault();



                #region Check Phases Tasks
                //if (emp.UserId != null && vac != null)
                //{
                //    var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && s.Status == 2).ToList(); //running
                //    PhasesTask = PhasesTask.Where(m => (string.IsNullOrEmpty(vac.StartDate) || (!string.IsNullOrEmpty(m.ExcpectedStartDate) &&
                //                 DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(vac.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) &&
                //                 (string.IsNullOrEmpty(vac.EndDate) || string.IsNullOrEmpty(m.ExcpectedEndDate) || DateTime.ParseExact(m.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(vac.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();

                //    if (PhasesTask != null || PhasesTask.Count > 0)
                //    {
                //        var massage2 = "";
                //        if (Lang == "rtl")
                //        {
                //            massage2 = "الموظف عليه " + PhasesTask.Count + " مهام عمل في فترة الإجازة يجب تحويلها لمستخدم اخر";
                //        }
                //        else
                //        {
                //            massage2 = "This Employee have" + PhasesTask.Count + " tasks at the vaction period must be turn it";
                //        }
                //        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =massage2 };
                //    }
                //}
                #endregion

                if (vac != null)
                {
                    vac.DecisionType = DecisionType;
                    vac.UpdateDate = DateTime.Now;
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل الاجازة إلى الإدارة" + vacationId;
                _SystemAction.SaveAction("UpdateDecisionType_V", "VacationService", 2, Resources.TransferredAdministration, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =Resources.TransferredAdministration };
            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل تحويل الاجازةإلى الإدارة";
                _SystemAction.SaveAction("UpdateDecisionType_V", "VacationService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
        }
        public GeneralMessage UpdateBackToWork_V(int vacationId, int UserId, int BranchId)
        {

            try
            {
                //var VacationUpdated = _VacationRepository.GetById(vacationId);
                Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacationId).FirstOrDefault();
                //var emp = _EmployeesRepository.GetById(VacationUpdated.EmployeeId);
                Employees? emp = _TaamerProContext.Employees.Where(s => s.EmployeeId == VacationUpdated.EmployeeId).FirstOrDefault();
                if (VacationUpdated != null)
                {
                    //Run tasks back
                    if (emp.UserId != null)
                    {
                        //var PhasesTask = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false &&/* (s.Project.StopProjectType != 1) &&*/ s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 3)).ToList(); //running 
                        var PhasesTask = _TaamerProContext.ProjectPhasesTasks.Where(s => s.IsDeleted == false &&/* (s.Project.StopProjectType != 1) &&*/ s.IsMerig == -1 && s.Type == 3 && s.UserId == emp.UserId && (s.Status == 3)).ToList(); //running 
                        if(PhasesTask.Count > 0) { 
                      var  PhasesTask1 = PhasesTask.Where(s => s.Project?.StopProjectType != 1);
                            if (PhasesTask1 !=null && PhasesTask1.ToList().Count > 0)
                            {
                                var PhasesTask2 = PhasesTask1.Where(m => (string.IsNullOrEmpty(VacationUpdated.StartDate) || (!string.IsNullOrEmpty(m.ExcpectedStartDate) &&
                                                         DateTime.ParseExact(m.ExcpectedStartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(VacationUpdated.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))) &&
                                                         (string.IsNullOrEmpty(VacationUpdated.EndDate) || string.IsNullOrEmpty(m.ExcpectedEndDate) || DateTime.ParseExact(m.ExcpectedEndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(VacationUpdated.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();

                                if ((PhasesTask2 != null || PhasesTask2.Count > 0)) // && Type == 2
                                {
                                    foreach (var task in PhasesTask2)
                                    {
                                        task.Status = 2;
                                    }
                                }
                            }
                        }
                    }
                    
                    VacationUpdated.BackToWorkDate = Utilities.ConvertDateCalendar(DateTime.Now, "Gregorian", "en-US");
                    VacationUpdated.UpdateDate = DateTime.Now;
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "مباشرة العمل بعد إجازة رقم " + vacationId;
                _SystemAction.SaveAction("UpdateBackToWork_V", "VacationService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل المباشرة بعد الإجازة";
                _SystemAction.SaveAction("UpdateBackToWork_V", "VacationService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
        }
        public GeneralMessage DeleteVacation(int VacationId, int UserId, int BranchId)
        {
            try
            {
                // Vacation vacation = _VacationRepository.GetById(VacationId);
                Vacation? vacation = _TaamerProContext.Vacation.Where(s => s.VacationId == VacationId).FirstOrDefault();
                if (vacation != null)
                {
                    vacation.IsDeleted = true;
                    vacation.DeleteDate = DateTime.Now;
                    vacation.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حذف الاجازة ";
                    _SystemAction.SaveAction("DeleteVacation", "VacationService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف الاجازة ";
                _SystemAction.SaveAction("DeleteVacation", "VacationService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
                }
        }

        public IEnumerable< rptGetAboutToStartVacationsVM> GetVacationsAboutToStart(string Con)
        {
            try
            {
                List< rptGetAboutToStartVacationsVM> lmd = new List< rptGetAboutToStartVacationsVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "rptGetAboutToStartVacations";
                        command.Connection = con;


                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            lmd.Add(new  rptGetAboutToStartVacationsVM
                            {
                                EmpNo = (dr[0]).ToString(),
                                EmpName = dr[1].ToString(),
                                Nationality = dr[2].ToString(),
                                DepName = dr[3].ToString(),
                                Branch = dr[4].ToString(),
                                StartDate = dr[5].ToString(),

                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception )
            {
                List< rptGetAboutToStartVacationsVM> lmd = new List< rptGetAboutToStartVacationsVM>();
                return lmd;
            }


        }

        public List<string> GetVacationDays_WithoutHolidays(string StartDate, string EndDate, int EmpId, string Lang, string Con ,int vacationtypeid) 
        {
            int  DawamId = _EmployeesRepository.GetEmployeeById(EmpId, Lang).Result.DawamId ?? 0;
            List<string> days = _VacationRepository.GetVacationDays_WithoutHolidays(StartDate, EndDate, DawamId, Con, vacationtypeid).Result;
            return days;
        }

        /******************/





        public GeneralMessage CheckLoan(int vacationId, int UserId, int BranchId, string Lang, int Type, string Con, string Url, string ImgUrl,string? reason)
        {
            try
            {
                //var VacationUpdated = _VacationRepository.GetById(vacationId);
                Vacation? VacationUpdated = _TaamerProContext.Vacation.Where(s => s.VacationId == vacationId).FirstOrDefault();
                 
                
                var Employee = _EmployeesRepository.GetEmployeeById((int)VacationUpdated.EmployeeId, Lang);
                //var EmployeeUpdated = _EmployeesRepository.GetById(VacationUpdated.EmployeeId);
                Employees? EmployeeUpdated = _TaamerProContext.Employees.Where(s => s.EmployeeId == VacationUpdated.EmployeeId).FirstOrDefault();

                var loans = _TaamerProContext.Loan.Where(x => !x.IsDeleted && x.EmployeeId==EmployeeUpdated.EmployeeId).ToList();
                int LoansCount = 0;
                var branch = _BranchesRepository.GetBranchByBranchId(Lang, (int)Employee.Result.BranchId).Result.FirstOrDefault();
                DateTime Today = DateTime.Now.Date;
                foreach (var loan in loans)
                {
                    if (loan.LoanDetails !=null && loan.LoanDetails.Count > 0)
                    {
                        DateTime MaxDate = loan.LoanDetails.Select(x => x.Date.HasValue ? x.Date.Value : DateTime.MinValue).Max();
                        if (MaxDate.Year >= Today.Year && MaxDate.Month >= Today.Month)
                            LoansCount = (int)(LoansCount + loan.Amount);
                    }
                }
                if (Type == 2) { 
                if (LoansCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في طلب الاجازه";
                    _SystemAction.SaveAction("UpdateStatus", "LoanService", 2, "الموظف عليه اقساط", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage() {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.employee_pays_installments };
                }

                }
                var result = UpdateVacation(vacationId, UserId, BranchId, Lang, Type, Con, Url, ImgUrl, reason);




                

                return new GeneralMessage { StatusCode = result.StatusCode, ReasonPhrase =result.ReasonPhrase };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل الاجازة";
                _SystemAction.SaveAction("UpdateVacation", "VacationService", 2, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }
        }

    }
}
