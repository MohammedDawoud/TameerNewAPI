using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.Enums;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using static Dropbox.Api.UsersCommon.AccountType;

namespace TaamerProject.Service.Services
{
    public class ProjectService : IProjectService
    {


        private readonly IProjectRepository _ProjectRepository;
        private readonly IProjectStatusTasksRepository _ProjectStatusTasksRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProjectWorkersRepository _ProjectWorkersRepository;
        private readonly IInvoicesRepository _InvoicesRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICustomerPaymentsRepository _customerPaymentsRepository;
        private readonly ISystemSettingsRepository _SystemSettingsRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly IBranchesRepository _branchesRepository;
        private readonly IVoucherDetailsRepository _voucherDetailsRepository;
        private readonly IUserPrivilegesRepository _userPrivilegesRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly ICostCenterRepository _CostCenterRepository;
        private readonly IProjectArchivesReRepository _ProjectArchivesReRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly ITasksDependencyRepository _TasksDependencyRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IOffersPricesRepository _offersPricesRepository;
        private readonly IProjectRequirementsGoalsRepository _projectRequirementsGoalsRepository;
        private readonly INotificationService _notificationService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;





        public ProjectService(IProjectRepository projectRepository, IProjectStatusTasksRepository projectStatusTasksRepository, IUsersRepository usersRepository,
            IProjectWorkersRepository projectWorkersRepository, IInvoicesRepository invoicesRepository, IContractRepository contractRepository, ICustomerPaymentsRepository customerPaymentsRepository,
            ISystemSettingsRepository systemSettingsRepository, IProjectPhasesTasksRepository projectPhasesTasksRepository, IBranchesRepository branchesRepository,
            IVoucherDetailsRepository voucherDetailsRepository, IUserPrivilegesRepository userPrivilegesRepository, INotificationRepository notificationRepository,
            IBranchesRepository branchesRepository1, ICustomerRepository customerRepository, ICostCenterRepository costCenterRepository, IProjectArchivesReRepository projectArchivesReRepository,
            ISys_SystemActionsRepository sys_SystemActionsRepository, ITasksDependencyRepository tasksDependencyRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService,
            IEmailSettingRepository emailSettingRepository, IOrganizationsRepository organizationsRepository, IOffersPricesRepository offersPricesRepository,
            IProjectRequirementsGoalsRepository projectRequirementsGoalsRepository, INotificationService notificationService, TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectRepository = projectRepository;
            _ProjectStatusTasksRepository = projectStatusTasksRepository;
            _usersRepository = usersRepository;
            _ProjectWorkersRepository = projectWorkersRepository;
            _InvoicesRepository = invoicesRepository;
            _contractRepository = contractRepository;
            _customerPaymentsRepository = customerPaymentsRepository;
            _SystemSettingsRepository = systemSettingsRepository;
            _ProjectPhasesTasksRepository = projectPhasesTasksRepository;
            _branchesRepository = branchesRepository;
            _voucherDetailsRepository = voucherDetailsRepository;
            _userPrivilegesRepository = userPrivilegesRepository;
            _NotificationRepository = notificationRepository;
            _BranchesRepository = branchesRepository1;
            _CustomerRepository = customerRepository;
            _CostCenterRepository = costCenterRepository;
            _ProjectArchivesReRepository = projectArchivesReRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TasksDependencyRepository = tasksDependencyRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _EmailSettingRepository = emailSettingRepository;
            _OrganizationsRepository = organizationsRepository;

            _offersPricesRepository = offersPricesRepository;
            _projectRequirementsGoalsRepository = projectRequirementsGoalsRepository;

            _notificationService = notificationService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }




        public async Task<IEnumerable<ProjectVM>> GetAllProject(string Lang, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllProject(Lang, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectCustomerBranch(string Lang, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllProjectCustomerBranch(Lang, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectsmartfollow(string Lang, int BranchId, int UserId)
        {
            var projects = await _ProjectRepository.GetAllProjectsmartfollow(Lang, BranchId, UserId);
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjectsmartfollowforadmin(string Lang, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllProjectsmartfollowforadmin(Lang, BranchId);
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjects2(string Lang, int BranchId, int UserId)
        {
            var projects = await _ProjectRepository.GetAllProjects2(Lang, BranchId, UserId);
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjects3(string con, string Lang, int BranchId, int UserId)
        {
            var projects = await _ProjectRepository.GetAllProjects3(con, Lang, BranchId, UserId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectsStatusTasks(string Lang, int BranchId)
        {
            var projects = await _ProjectStatusTasksRepository.GetAllProjectsStatusTasks(Lang, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectWithout(string Lang, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllProjectWithout(Lang, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectAllBranches()
        {
            var projects = await _ProjectRepository.GetAllProjectAllBranches();
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectAllBranches2()
        {
            var projects = await _ProjectRepository.GetAllProjectAllBranches2();
            return projects;
        }

        public GeneralMessage BarcodePDF(int FileID, int UserId)
        {

            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Done };
        }


        public async Task<IEnumerable<ProjectVM>> GetAllProjectNumber(string SearchText, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllProjectNumber(SearchText, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllArchiveProject(int BranchId)
        {
            var projects = await _ProjectRepository.GetAllArchiveProject(BranchId);
            return projects;
        }
        public GeneralMessage SaveProject(Project project, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var WhichPart = "Part (1)";
            var codeExist = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId != project.ProjectId && s.ProjectNo == project.ProjectNo).FirstOrDefault();
            if (codeExist != null)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.General_SavedFailed;
                _SystemAction.SaveAction("SaveProject", "ProjectService", 1, "Resources.ProjectNumberAlready", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.ProjectNumberAlready };
            }
            try
            {
                WhichPart = "Part (2)";
                var Privs = project.ProUserPrivileges;
                project.ProUserPrivileges = new List<ProUserPrivileges>();
                if (project.ProjectId == 0)
                {

                    var totaldays = 0.0;
                    DateTime resultEnd = DateTime.ParseExact(project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime resultStart = DateTime.ParseExact(project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    totaldays = (resultEnd - resultStart).TotalDays + 1;

                    project.BranchId = BranchId;
                    project.Status = 0;
                    project.NoOfDays = Convert.ToInt32(totaldays);
                    project.FirstProjectDate = project.ProjectDate;
                    project.FirstProjectExpireDate = project.ProjectExpireDate;
                    project.AddUser = project.UserId = UserId;
                    project.AddDate = DateTime.Now;
                    _TaamerProContext.Project.Add(project);
                }
                WhichPart = "Part (3)";
                _TaamerProContext.SaveChanges();
                WhichPart = "Part (4)";
                project.ProUserPrivileges = Privs;

                try
                {
                    if (project.ProjectRequirementsGoals.Count() > 0)
                    {

                        foreach (var item in project.ProjectRequirementsGoals.ToList())
                        {
                            item.RequirementGoalId = 0;
                            item.AddDate = DateTime.Now;
                            item.AddUser = UserId;
                            item.ProjectId = project.ProjectId;
                            _TaamerProContext.ProjectRequirementsGoals.Add(item);
                            _TaamerProContext.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote7 = "فشل في حفظ أهداف المشروع";
                    _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                WhichPart = "Part (5)";
                //notification of Invoice
                if (project.TransactionTypeId == 1)
                {
                    project.MotionProject = 1;
                    project.MotionProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    project.MotionProjectNote = "أضافة فاتورة علي مشروع";

                    var ListOfPrivNotify = new List<Notification>();
                    var branch = _BranchesRepository.GetById(BranchId);
                    var customer = _CustomerRepository.GetById(project.CustomerId ?? 0);
                    List<int> usersnote = new List<int>();
                    try
                    {
                        var beneficiary = GetNotificationRecipients(NotificationCode.Project_ServiceInvoiceIssued, project.ProjectId);
                    if (beneficiary.Users.Count() > 0)
                    {
                        usersnote = beneficiary.Users;

                    }
                    else
                    {
                        usersnote.Add(project.MangerId.Value);
                    }

                        foreach (var usr in usersnote)
                        {


                            ListOfPrivNotify.Add(new Notification
                            {
                                ReceiveUserId = usr,
                                Name = @Resources.MNAcc_Invoice,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1, // notification
                                Description = " يوجد فاتورة جديدة علي مشروع رقم   : " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " فرع " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = 0,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false
                            });
                            _TaamerProContext.Notification.AddRange(ListOfPrivNotify);

                            _notificationService.sendmobilenotification((int)usr, string.IsNullOrWhiteSpace(beneficiary.Description) ? "أضافة فاتورة علي مشروع" : beneficiary.Description, " يوجد فاتورة جديدة علي مشروع رقم   : " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " فرع " + branch.NameAr + "");




                            var manager = _TaamerProContext.Users.Where(x => x.UserId == usr).FirstOrDefault();


                            var htmlBody = "";
                            var Desc = " السيد /ة " + manager.FullNameAr + "المحترم " + "<br/>" + "السلام عليكم ورحمة الله وبركاتة " + "<br/>" + "<h2> تم اصدار فاتورة أثناء انشاء المشروع</h2>" + "<br/>" + " للعميل " + customer.CustomerNameAr + "<br/>" + " تابع لفرع " + branch.NameAr + "<br/>" + "مع تحيات قسم ادارة المشاريع ";


                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>الفرع </th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + project.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + branch.NameAr + @"</td>
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            SendMail_ProjectStamp(BranchId, usr, usr ,string.IsNullOrWhiteSpace(beneficiary.Description) ? "صدور فاتورة جديدة علي مشروع" : beneficiary.Description, htmlBody, Url, ImgUrl, 6, true);

                        }
                            }
                            catch (Exception ex2)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote5 = "فشل في ارسال ميل لمن لدية صلاحية فاتورة";
                                _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                            }

                        //}
                    //}

                    //var UserNotifPriv_Mobile = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3253).Result;
                    //if (UserNotifPriv_Mobile.Count() != 0)
                    //{
                    //    foreach (var userCounter in UserNotifPriv_Mobile)
                    //    {
                            try
                            {
                        if (project.MangerId != null) {
                            var manager = _TaamerProContext.Users.Where(x => x.UserId == project.MangerId).FirstOrDefault();

                            var userObj = _usersRepository.GetById(project.MangerId ?? 0);
                            var NotStr = " المستخدم " + manager.FullNameAr + " تم اصدار فاتورة لمشروع رقم " + project.ProjectNo + " للعميل " + customer.CustomerNameAr + " فرع " + branch.NameAr;
                            if (userObj.Mobile != null && userObj.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                            }
                        }
                            }
                            catch (Exception ex)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote6 = "فشل في ارسال SMS لمن لدية صلاحية فاتورة";
                                _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                            }

                    //    }
                    //}
                }
                // add project workers 
                WhichPart = "Part (6)";
                try
                {
                    var worker = new ProjectWorkers();
                    worker.ProjectId = project.ProjectId;
                    worker.BranchId = BranchId;
                    worker.WorkerType = 1; // senior
                    worker.UserId = project.MangerId;
                    worker.IsDeleted = false;
                    worker.AddDate = DateTime.Now;
                    worker.AddUser = UserId;
                    _TaamerProContext.ProjectWorkers.Add(worker);
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate7 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote7 = "فشل في حفظ العاملين علي المشروع";
                    _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate7, UserId, BranchId, ActionNote7, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                WhichPart = "Part (7)";
                /////add projectno to offerprice
                if (project.OffersPricesId != null)
                {
                    try
                    {
                        var offer = _offersPricesRepository.GetById((int)project.OffersPricesId);
                        var oldoffer = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.ProjectId == project.ProjectId).FirstOrDefault();
                        if (oldoffer != null)
                        {
                            if (oldoffer.OffersPricesId != offer.OffersPricesId)
                            {
                                oldoffer.ProjectId = null;
                            }
                        }
                        offer.ProjectId = project.ProjectId;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                WhichPart = "Part (8)";

                //// add cost center

                try
                {
                    var newcostCenter = new CostCenters();
                    var CostCenterByid = _CostCenterRepository.GetById((int)project.CostCenterId);
                    newcostCenter.ParentId = project.CostCenterId;
                    newcostCenter.BranchId = CostCenterByid.BranchId;
                    newcostCenter.Code = project.ProjectNo;
                    newcostCenter.NameAr = _CustomerRepository.GetById((int)project.CustomerId).CustomerNameAr;
                    newcostCenter.NameEn = _CustomerRepository.GetById((int)project.CustomerId).CustomerNameEn;
                    newcostCenter.AddDate = DateTime.Now;
                    newcostCenter.AddUser = UserId;
                    newcostCenter.CustomerId = project.CustomerId;
                    newcostCenter.ProjId = project.ProjectId;
                    _TaamerProContext.CostCenters.Add(newcostCenter);
                }
                catch (Exception ex)
                {

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate8 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote8 = "فشل في حفظ مركز تكلفة للمشروع";
                    _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate8, UserId, BranchId, ActionNote8, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                WhichPart = "Part (9)";

                _TaamerProContext.SaveChanges();
                WhichPart = "Part (10)";
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة مشروع جديد" + "برقم" + project.ProjectNo;
                _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = project.ProjectId.ToString() };
            }
            catch (Exception ex)
            {
                SendMail_ProjectSavedWrong(BranchId, WhichPart + " " + ex.Message + ">>>>" + ex.InnerException, false);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المشروع";
                _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = WhichPart + " " + ex.Message + ">>>>" + ex.InnerException };
            }
        }

        public bool SendMail_ProjectSavedWrong(int BranchId, string textBody, bool IsBodyHtml = false)
        {
            try
            {
                var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                var Org = _OrganizationsRepository.GetById(Organization);

                var mail = new MailMessage();
                var email = "noreply-tameer@bayanatech.com.sa";
                var password = "eA4LQkrbQdCm5jqt";
                var loginInfo = new NetworkCredential(email, password);
                mail.From = new MailAddress(email, "TAMEER-CLOUD-SYSTEM");

                mail.To.Add(new MailAddress("mohammeddawoud66@gmail.com"));
                mail.Subject = "فشل حفظ مشروع " + " " + Org.NameAr;

                mail.Body = textBody;
                mail.IsBodyHtml = IsBodyHtml;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var smtpClient = new SmtpClient("mail.bayanatech.com.sa");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public GeneralMessage PostProjectsCheckBox(List<string> ProjectIds, string MangerId, int BranchId, int? yearid)
        {
            try
            {
                //var year = _fiscalyearsRepository.GetCurrentYear();
                if (yearid == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في الترحيل";
                    _SystemAction.SaveAction("PostProjects", "ProjectService", 1, Resources.choosefinYear, "", "", ActionDate2, Convert.ToInt32(MangerId), BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.choosefinYear };
                }
                int CountNMora7l = 0;
                int CountMora7l = 0;

                for (int i = 0; i < ProjectIds.Count(); i++)
                {
                    var postedProject = _ProjectRepository.GetById(Convert.ToInt32(ProjectIds[i]));

                    CountNMora7l += 1;
                    postedProject.MangerId = Convert.ToInt32(MangerId);
                    _TaamerProContext.SaveChanges();
                }

                string Message = "";
                Message = Resources.Deported;

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم الترحيل بنجاح";
                _SystemAction.SaveAction("PostProjects", "ProjectService", 2, Message, "", "", ActionDate, Convert.ToInt32(MangerId), BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Message };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الترحيل ";
                _SystemAction.SaveAction("PostProjects", "ProjectService", 1, "Resources.PostesFailed", "", "", ActionDate, Convert.ToInt32(MangerId), BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.PostesFailed };
            }
        }

        public GeneralMessage DeleteProject(int projectId, int UserId, int BranchId)
        {
            try
            {
                Project proj = _ProjectRepository.GetById(projectId);
                proj.IsDeleted = true;
                proj.DeleteDate = DateTime.Now;
                proj.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف المشروع رقم " + projectId;
                _SystemAction.SaveAction("DeleteProject", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                _SystemAction.SaveAction("DeleteProject", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage SendCustomerEmail_SMS(int CustomerId, int ProjectId, int TypeId, int UserId, int BranchId)
        {
            try
            {

                var customerObj = _CustomerRepository.GetById(CustomerId);
                var projectObj = _ProjectRepository.GetById(ProjectId);

                if (customerObj != null)
                {
                    var Desc = "تم انشاء مشروع رقم" + " " + projectObj.ProjectNo + " " + "للعميل" + " " + customerObj.CustomerNameAr;
                    var Subject = "إشعار بإنشاء المشروع";
                    if (TypeId == 1 || TypeId == 3)
                    {
                        if (customerObj.CustomerEmail != null && customerObj.CustomerEmail != "")
                        {
                            try
                            {
                                SendMailCustomer(customerObj.CustomerEmail, Desc, Subject, BranchId);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }
                    if (TypeId == 2 || TypeId == 3)
                    {
                        if (customerObj.CustomerMobile != null && customerObj.CustomerMobile != "")
                        {
                            try
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(customerObj.CustomerMobile, Desc, UserId, BranchId);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }

                }

                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "ارسال ايميل و رسالة للعميل";
                _SystemAction.SaveAction("DeleteProject", "ProjectService", 3, Resources.Gerneral_send, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Gerneral_send };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل ارسال ايميل و رسالة للعميل ";
                _SystemAction.SaveAction("DeleteProject", "ProjectService", 3, Resources.sendfailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.sendfailed };
            }
        }

        public GeneralMessage Updateskiptime(int projectId, int UserId, int BranchId)
        {
            try
            {
                Project proj = _ProjectRepository.GetById(projectId);
                if (proj.SkipCount == null)
                {
                    proj.SkipCount = 1;
                }
                else
                {
                    proj.SkipCount = proj.SkipCount + 1;
                }
                proj.UpdateDate = DateTime.Now;
                proj.UpdateUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف المشروع رقم " + projectId;
                _SystemAction.SaveAction("DeleteProject", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                _SystemAction.SaveAction("DeleteProject", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage UpdateProjectnoSpaces(int UserId, int BranchId)
        {
            try
            {
                var AllProjects = _TaamerProContext.Project.Where(s => s.IsDeleted == false).ToList();

                foreach (var Project in AllProjects)
                {
                    Project.ProjectNo = Project.ProjectNo.Trim();
                }
                _TaamerProContext.SaveChanges();

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailedEnd };
            }
        }



        public GeneralMessage DeleteProjectNEW(int projectId, int UserId, int BranchId)
        {
            try
            {
                Project proj = _ProjectRepository.GetById(projectId);

                if (proj.ContractId != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_C };
                }
                else
                {
                    var phasesCount = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId);
                    if (phasesCount.Count() > 0)
                    {

                        var Tasksp = phasesCount.Where(s => s.Type == 3 && s.Status != 4);
                        if (Tasksp.Count() > 0)
                        {

                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                            _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_P };

                        }

                        if (phasesCount.FirstOrDefault().DescriptionEn == "without Main Phase")
                        {
                            int InvoiceCount = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.IsPost == false).Count();
                            if (InvoiceCount > 0)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                                _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                            }
                            else
                            {


                                int InvoiceCount2 = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.Rad == false).Count();
                                if (InvoiceCount2 > 0)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = " فشل في حذف المشروع رقم " + projectId; ;
                                    _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                                }

                                var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == projectId).FirstOrDefault();
                                if (CostCenterProject != null)
                                {

                                    int VoucherCount = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterProject.CostCenterId && s.Invoices.Rad == false).Count();
                                    if (VoucherCount > 0)
                                    {
                                        //-----------------------------------------------------------------------------------------------------------------
                                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                        string ActionNote4 = " فشل في حذف المشروع رقم " + projectId; ;
                                        _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                                        //-----------------------------------------------------------------------------------------------------------------
                                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                                    }
                                    else
                                    {
                                        CostCenterProject.IsDeleted = true;
                                        CostCenterProject.DeleteDate = DateTime.Now;
                                        CostCenterProject.DeleteUser = UserId;
                                    }


                                }

                                //_ProjectPhasesTasksRepository.RemoveMatching(s => s.ProjectId == projectId);
                                var tsks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId);
                                foreach (var item in tsks)
                                {
                                    item.IsDeleted = true;
                                    item.DeleteDate = DateTime.Now;
                                    item.DeleteUser = UserId;

                                }
                                proj.IsDeleted = true;
                                proj.DeleteDate = DateTime.Now;
                                proj.DeleteUser = UserId;


                                _TaamerProContext.SaveChanges();
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote = " حذف المشروع رقم " + projectId;
                                _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };



                            }
                        }
                        else
                        {
                            var Tasks = phasesCount.Where(s => s.Type == 3 && s.Status != 4);
                            if (Tasks.Count() > 0)
                            {

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                                _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_P };


                            }
                            else
                            {
                                proj.IsDeleted = true;
                                proj.DeleteDate = DateTime.Now;
                                proj.DeleteUser = UserId;

                                var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == projectId).FirstOrDefault();
                                if (CostCenterProject != null)
                                {
                                    CostCenterProject.IsDeleted = true;
                                    CostCenterProject.DeleteDate = DateTime.Now;
                                    CostCenterProject.DeleteUser = UserId;
                                }

                                _TaamerProContext.SaveChanges();
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote = " حذف المشروع رقم " + projectId;
                                _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                            }

                        }
                    }
                    else
                    {
                        int InvoiceCount = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.Rad == false).Count();
                        if (InvoiceCount > 0)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                            _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                        }
                        else
                        {

                            var CostCenterProject2 = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == projectId).FirstOrDefault();
                            if (CostCenterProject2 != null)
                            {
                                int VoucherCount = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterProject2.CostCenterId && s.Invoices.Rad == false).Count();
                                if (VoucherCount > 0)
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = " فشل في حذف المشروع رقم " + projectId; ;
                                    _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------
                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                                }
                            }
                            proj.IsDeleted = true;
                            proj.DeleteDate = DateTime.Now;
                            proj.DeleteUser = UserId;

                            var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == projectId).FirstOrDefault();
                            if (CostCenterProject != null)
                            {
                                CostCenterProject.IsDeleted = true;
                                CostCenterProject.DeleteDate = DateTime.Now;
                                CostCenterProject.DeleteUser = UserId;
                            }

                            _TaamerProContext.SaveChanges();
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote = " حذف المشروع رقم " + projectId;
                            _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };



                        }

                    }
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                _SystemAction.SaveAction("DeleteProjectNEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage DeleteAllProject_NEW(int projectId, int UserId, int BranchId)
        {
            try
            {
                Project proj = _ProjectRepository.GetById(projectId);
                if (proj.ContractId != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_C };
                }

                int InvoiceCount = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.IsPost == false).Count();
                if (InvoiceCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                }

                int InvoiceCount2 = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.Rad == false).Count();
                if (InvoiceCount2 > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                }

                var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == projectId).FirstOrDefault();
                if (CostCenterProject != null)
                {

                    int VoucherCount = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterProject.CostCenterId && s.Invoices.Rad == false).Count();
                    if (VoucherCount > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = " فشل في حذف المشروع رقم " + projectId; ;
                        _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                    }
                    else
                    {
                        CostCenterProject.IsDeleted = true;
                        CostCenterProject.DeleteDate = DateTime.Now;
                        CostCenterProject.DeleteUser = UserId;
                    }
                }

                var tskremov = _TaamerProContext.ProjectPhasesTasks.Where(s => s.ProjectId == projectId).ToList();
                _TaamerProContext.ProjectPhasesTasks.RemoveRange(tskremov);
                proj.IsDeleted = true;
                proj.DeleteDate = DateTime.Now;
                proj.DeleteUser = UserId;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف المشروع رقم " + projectId;
                _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        private string DecryptValue(string value)
        {
            string hash = "f0xle@rn";
            byte[] data = Convert.FromBase64String(value); ;
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDesc = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform cryptoTransform = tripDesc.CreateDecryptor();
                    byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }
        public GeneralMessage DeleteAllProject_NEWWithVouchers(int projectId, string password, int UserId, int BranchId)
        {
            try
            {
                var UserV = _TaamerProContext.Users.Where(s => s.UserId == UserId && s.IsDeleted == false).FirstOrDefault();
                var pass = DecryptValue(UserV!.Password);
                if (pass != password)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "يرجي التأكد من الرقم السري" };
                }
                Project proj = _ProjectRepository.GetById(projectId);
                if (proj.ContractId != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_C };
                }

                int InvoiceCount = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.IsPost == false).Count();
                if (InvoiceCount > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                }

                int InvoiceCount2 = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.Rad == false).Count();
                if (InvoiceCount2 > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حذف المشروع رقم " + projectId; ;
                    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                }

                var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == projectId).FirstOrDefault();
                if (CostCenterProject != null)
                {

                    //int VoucherCount = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterProject.CostCenterId && s.Invoices.Rad == false).Count();
                    //if (VoucherCount > 0)
                    //{
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    string ActionNote4 = " فشل في حذف المشروع رقم " + projectId; ;
                    //    _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedDeletedProject_F };
                    //}
                    //else
                    //{
                    //    CostCenterProject.IsDeleted = true;
                    //    CostCenterProject.DeleteDate = DateTime.Now;
                    //    CostCenterProject.DeleteUser = UserId;
                    //}
                    CostCenterProject.IsDeleted = true;
                    CostCenterProject.DeleteDate = DateTime.Now;
                    CostCenterProject.DeleteUser = UserId;
                }
                var voucherList = _TaamerProContext.VoucherDetails.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterProject.CostCenterId
                && (s.Invoices.Type == 5 || s.Invoices.Type == 6 || s.Invoices.Type == 8) && s.Invoices.Rad == false).ToList();
                var invoiceDaily = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterProject.CostCenterId
                && (s.Type == 8) && s.Rad == false).ToList();
                var tskremov = _TaamerProContext.ProjectPhasesTasks.Where(s => s.ProjectId == projectId);
                _TaamerProContext.ProjectPhasesTasks.RemoveRange(tskremov);
                proj.IsDeleted = true;
                proj.DeleteDate = DateTime.Now;
                proj.DeleteUser = UserId;
                //---------------------------------------------------------
                //Invoices? voucher = _TaamerProContext.Invoices.Where(s => s.InvoiceId == VoucherId).FirstOrDefault();
                foreach (var detaile in voucherList)
                {
                    var voucher = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.InvoiceId == detaile.InvoiceId).FirstOrDefault();
                    if (voucher != null)
                    {
                        voucher.IsDeleted = true;
                        voucher.DeleteDate = DateTime.Now;
                        voucher.DeleteUser = UserId;


                        if (voucher.Type == 8)
                        {
                            var cust = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.InvoiceId == voucher.InvoiceId).ToList();
                            if (cust.Count() > 0)
                            {
                                cust.FirstOrDefault().InvoiceId = null;
                            }
                        }
                        var VoucherDetails = _TaamerProContext.VoucherDetails.Where(s => s.InvoiceId == voucher.InvoiceId).ToList();
                        var TransactionDetails = _TaamerProContext.Transactions.Where(s => s.InvoiceId == voucher.InvoiceId).ToList();
                        if (VoucherDetails.Count() > 0)
                        {
                            _TaamerProContext.VoucherDetails.RemoveRange(VoucherDetails);
                        }
                        if (TransactionDetails.Count() > 0)
                        {
                            _TaamerProContext.Transactions.RemoveRange(TransactionDetails);
                        }
                    }

                }
                foreach (var voucher in invoiceDaily)
                {
                    if (voucher != null)
                    {
                        voucher.IsDeleted = true;
                        voucher.DeleteDate = DateTime.Now;
                        voucher.DeleteUser = UserId;


                        if (voucher.Type == 8)
                        {
                            var cust = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.InvoiceId == voucher.InvoiceId).ToList();
                            if (cust.Count() > 0)
                            {
                                cust.FirstOrDefault().InvoiceId = null;
                            }
                        }
                        var VoucherDetails = _TaamerProContext.VoucherDetails.Where(s => s.InvoiceId == voucher.InvoiceId).ToList();
                        var TransactionDetails = _TaamerProContext.Transactions.Where(s => s.InvoiceId == voucher.InvoiceId).ToList();
                        if (VoucherDetails.Count() > 0)
                        {
                            _TaamerProContext.VoucherDetails.RemoveRange(VoucherDetails);
                        }
                        if (TransactionDetails.Count() > 0)
                        {
                            _TaamerProContext.Transactions.RemoveRange(TransactionDetails);
                        }
                    }

                }

                var projectstepsList = _TaamerProContext.Pro_ProjectSteps.Where(s => s.IsDeleted == false && s.ProjectId == projectId).ToList();
                if (projectstepsList.Count() > 0)
                {
                    _TaamerProContext.Pro_ProjectSteps.RemoveRange(projectstepsList);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف المشروع رقم " + projectId;
                _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف المشروع رقم " + projectId; ;
                _SystemAction.SaveAction("DeleteAllProject_NEW", "ProjectService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage DestinationsUploadProject(int projectId, int status, int UserId, int BranchId)
        {
            try
            {
                Project proj = _ProjectRepository.GetById(projectId);
                if (proj != null)
                {
                    proj.DestinationsUpload = status;
                    proj.UpdateUser = UserId;
                    proj.UpdateDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "رفع المشروع لجهات";
                    _SystemAction.SaveAction("DestinationsUploadProject", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفع المشروع لجهات";
                _SystemAction.SaveAction("DestinationsUploadProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage StopProject(int projectId, int UserId, int BranchId, string Url, string ImgUrl, int TypeId, int whichClickDesc)
        {
            try
            {
                List<int> usersnote = new List<int>();

                Project proj = _ProjectRepository.GetById(projectId);
                var customer = _CustomerRepository.GetById(proj.CustomerId ?? 0);

                var branch = _BranchesRepository.GetById(BranchId);
                proj.StopProjectType = 1;
                proj.StopProjectDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                proj.UpdateUser = UserId;
                proj.UpdateDate = DateTime.Now;

                var phasesTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId && s.Type == 3 && s.IsMerig == -1 && s.Status == 2);

                foreach (var phase in phasesTasks)
                {
                    phase.Status = 3;
                    phase.StopCount += 1;
                    //usersnote.Add(phase.UserId.Value);
                }

                #region Notifications

                try
                {

                    var beneficiary = GetNotificationRecipients(NotificationCode.Project_Stopped, proj.ProjectId);
                    if (beneficiary.Users.Count() > 0)
                    {
                        usersnote = beneficiary.Users;

                    }
                    else {
                        foreach (var phase in phasesTasks)
                        {
                            usersnote.Add(phase.UserId.Value);

                        }
                        usersnote.Add(proj.MangerId.Value);
                        var worker = _TaamerProContext.ProUserPrivileges
                             .Where(x => x.ProjectID == projectId)
                             .Select(x => x.UserId)
                             .Where(id => id.HasValue)  // Filters non-null values if UserId is int?
                             .Select(id => id.Value)
                             .ToList();

                        usersnote.AddRange(worker);
                    }
                    if (usersnote.Count() != 0)
                    {
                        var manager = _usersRepository.GetById(proj.MangerId.Value);
                        var updatetduse = _usersRepository.GetById(UserId);

                        foreach (var usr in usersnote.Distinct())
                        {

                            Users user = _usersRepository.GetById(usr);
                            var UserNotification = new Notification                           
                            {
                                ReceiveUserId = usr,
                                Name = "Resources.Pro_Projectmanagement",
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1,
                                Description = " تم ايقاف مشروع رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع  " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = projectId,
                                TaskId = 0,
                                AddUser = UserId,
                                BranchId = BranchId,
                                AddDate = DateTime.Now,
                                IsHidden = false,
                                NextTime = null,
                            };
                            _TaamerProContext.Notification.Add(UserNotification);
                            _TaamerProContext.SaveChanges();

                            _notificationService.sendmobilenotification(usr, string.IsNullOrWhiteSpace(beneficiary.Description) ? "ايقاف مشروع" : beneficiary.Description, "  تم ايقاف مشروع رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع  " + branch.NameAr + " مدير المشروع  " + manager.FullNameAr + "");

                            var htmlBody = "";

                            var Desc = "السيد / ة  " + user.FullName + "المحترم  " + "<br/>" + "السلام عليكم ورحمة الله وبركاتة " + "<br/>" + "<h2> نشعركم بايقاف المشروع </h2>" + " رقم المشروع  " + proj.ProjectNo + "<br/>" + " للعميل  " + customer.CustomerNameAr + "<br/>" + "مع تحيات قسم ادارة المشاريع";

                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + proj.ProjectNo + @"</td>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>مدير المشروع </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + manager.FullNameAr + @"</td>
                                              
                                                    </tr>
       
                                                                <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ الايقاف  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")) + @"</td>
                                              
                                                    </tr>
                                                                     <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تم الايقاف بواسطة  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + updatetduse.FullNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            SendMail_ProjectStamp(BranchId, UserId, usr, string.IsNullOrWhiteSpace(beneficiary.Description) ? "ايقاف مشروع" : beneficiary.Description, htmlBody, Url, ImgUrl, 1, true);


                            var NotStr = "Dear : " + user.FullName + " Project No " + proj.ProjectNo + " For Customer " + customer.CustomerNameAr + " has Stopped ";
                            if (user.Mobile != null && user.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(user.Mobile, NotStr, UserId, BranchId);
                            }
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }

                #endregion
                if (whichClickDesc != 0)
                {
                    SendGeneralCustomerEmailandSMS(customer, proj, TypeId, UserId, BranchId, whichClickDesc);
                }


                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم ايقاف المشروع مؤقتا";
                _SystemAction.SaveAction("StopProject", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = ActionNote };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في ايقاف المشروع مؤقتا";
                _SystemAction.SaveAction("StopProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNote };
            }
        }

        public string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            DateLangCulture = DateLangCulture.ToLower();
            string formattedDate = DateConv.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string DAtee = GregToHijri2(formattedDate);
            return (DAtee);
        }
        private string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
           "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"
        };
        public string GregToHijri2(string Greg)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en");
            DateTime tempDate = DateTime.ParseExact(Greg, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate.ToString("yyyy-MM-dd", arCul.DateTimeFormat);
        }
        public GeneralMessage PlayProject(int projectId, int UserId, int BranchId, string Url, string ImgUrl, int TypeId, int whichClickDesc)
        {
            try
            {
                Project proj = _ProjectRepository.GetById(projectId);
                var customer = _CustomerRepository.GetById(proj.CustomerId ?? 0);
                //Users user = _usersRepository.GetById(proj.MangerId ?? 0);
                var branch = _BranchesRepository.GetById(BranchId);
                List<int> usersnote = new List<int>();

                proj.StopProjectType = 0;
                proj.UpdateUser = UserId;
                proj.UpdateDate = DateTime.Now;
                if (proj.StopProjectDate != "" && proj.StopProjectDate != null)
                {
                    DateTime StopDate = DateTime.ParseExact(proj.StopProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime EndProjectDateFirst = DateTime.ParseExact(proj.FirstProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime EndProjectDate = DateTime.ParseExact(proj.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    var Time_betTwodates = EndProjectDate - StopDate;
                    if (Time_betTwodates.Days > 0)
                    {
                        DateTime Datenow = DateTime.Now;
                        var Time_Add = Datenow - StopDate;
                        if (Time_Add.Days > 0)
                        {

                            if (Time_Add.Days > Time_betTwodates.Days)
                            {
                                EndProjectDateFirst = EndProjectDateFirst.AddDays(Time_betTwodates.Days);
                                EndProjectDate = EndProjectDate.AddDays(Time_betTwodates.Days);
                            }
                            else
                            {
                                EndProjectDateFirst = EndProjectDateFirst.AddDays(Time_Add.Days);
                                EndProjectDate = EndProjectDate.AddDays(Time_Add.Days);
                            }


                            var EndProjectDateHijri = ConvertDateCalendar(EndProjectDate, "Hijri", "en-US");


                            proj.FirstProjectExpireDate = EndProjectDateFirst.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            proj.ProjectExpireDate = EndProjectDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            proj.ProjectExpireHijriDate = EndProjectDateHijri;

                        }

                    }
                }


                var phasesTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId && s.Type == 3 && s.IsMerig == -1 && s.Status == 3 && s.Active == true);
                foreach (var phase in phasesTasks)
                {
                    phase.Status = 2;
                    //usersnote.Add(phase.UserId.Value);

                }

                try
                {
                    var ListOfPrivNotify = new List<Notification>();
                    var beneficiary = GetNotificationRecipients(NotificationCode.Project_Reactivated, proj.ProjectId);
                    if (beneficiary.Users.Count() > 0)
                    {
                        usersnote = beneficiary.Users;

                    }
                    else
                    {
                        foreach (var phase in phasesTasks)
                        {
                           // phase.Status = 2;
                            usersnote.Add(phase.UserId.Value);

                        }
                        usersnote.Add(proj.MangerId.Value);
                        var worker = _TaamerProContext.ProUserPrivileges
                             .Where(x => x.ProjectID == projectId)
                             .Select(x => x.UserId)
                             .Where(id => id.HasValue)  // Filters non-null values if UserId is int?
                             .Select(id => id.Value)
                             .ToList();

                        usersnote.AddRange(worker);
                    }
                    if (usersnote.Count() != 0)
                    {
                        var manager = _usersRepository.GetById(proj.MangerId.Value);
                        var updatetduse = _usersRepository.GetById(UserId);

                        foreach (var usr in usersnote.Distinct())
                        {

                            Users user = _usersRepository.GetById(usr);

                            var UserNotification = new Notification
                            {
                                ReceiveUserId = usr,
                                Name = "Resources.Pro_Projectmanagement",
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1, // notification
                                Description = " تم تشغيل مشروع رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع  " + branch.NameAr + "",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = projectId,
                                TaskId = 0,
                                BranchId = BranchId,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false,
                                NextTime = null,
                            };
                            _TaamerProContext.Notification.Add(UserNotification);
                            _TaamerProContext.SaveChanges();

                            _notificationService.sendmobilenotification(usr, string.IsNullOrWhiteSpace(beneficiary.Description)? "اعادة تشغيل مشروع" : beneficiary.Description, " تم تشغيل مشروع رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + " " + " فرع  " + branch.NameAr + "");

                            var htmlBody = "";


                            //var Desc = "Dear : " + user.FullName + " Project No " + proj.ProjectNo + " For Customer " + customer.CustomerNameAr + " has working ";



                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + proj.ProjectNo + @"</td>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>مدير المشروع </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + manager.FullNameAr + @"</td>
                                              
                                                    </tr>
       
                                                                <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ التشغيل  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DateTime.Now.Date + @"</td>
                                              
                                                    </tr>
                                                                     <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تم التشغيل بواسطة  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + updatetduse.FullNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            SendMail_ProjectStamp(BranchId, UserId, usr, string.IsNullOrWhiteSpace(beneficiary.Description) ? "اعادة تشغيل مشروع" : beneficiary.Description, htmlBody, Url, ImgUrl, 2, true);



                             var NotStr = "Dear : " + user.FullName + " Project No " + proj.ProjectNo + " For Customer " + customer.CustomerNameAr + " has working ";
                            if (user.Mobile != null && user.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(user.Mobile, NotStr, UserId, BranchId);
                            }
                           
                        }
                    }


                }
                catch (Exception ex)
                {

                }

                if (whichClickDesc != 0)
                {
                    SendGeneralCustomerEmailandSMS(customer, proj, TypeId, UserId, BranchId, whichClickDesc);
                }

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم تشغيل المشروع ";
                _SystemAction.SaveAction("PlayProject", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = ActionNote };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تشغيل المشروع";
                _SystemAction.SaveAction("PlayProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ActionNote };
            }
        }

        public async Task<ProjectVM> GetProjectById(string Lang, int ProjectId)
        {
            return await _ProjectRepository.GetProjectById(Lang, ProjectId);
        }
        public async Task<ProjectVM> GetProjectByIdSome(string Lang, int ProjectId)
        {
            return await _ProjectRepository.GetProjectByIdSome(Lang, ProjectId);
        }
        public async Task<ProjectVM> GetCostCenterByProId(string Lang, int ProjectId)
        {
            return await _ProjectRepository.GetCostCenterByProId(Lang, ProjectId);
        }
        public Project GetProjectByOfferId(string OfferId)
        {
            var Va = Convert.ToInt32(OfferId);
            return _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.OffersPricesId == Va).FirstOrDefault();
        }
        public async Task<ProjectVM> GetProjectDataOffice(string Lang, int ProjectId)
        {
            return await _ProjectRepository.GetProjectDataOffice(Lang, ProjectId);
        }
        public async Task<ProjectVM> GetProjectByIdStopType(string Lang, int ProjectId)
        {
            return await _ProjectRepository.GetProjectByIdStopType(Lang, ProjectId);
        }
        public async Task<ProjectVM> GetProjectAddUser(int ProjectId)
        {
            return await _ProjectRepository.GetProjectAddUser(ProjectId);
        }
        public int GetTypeOfProjct(int projectId)
        {
            var proj = _TaamerProContext.Project.Where(x => x.ProjectId == projectId && x.IsDeleted == false).FirstOrDefault();
            if (proj != null)
                return proj.ProjectTypeId ?? 0;
            else
                return 0;
        }
        public GeneralMessage SaveProjectDetails(ProjectVM project, int UserId, int BranchId)
        {
            var totaldays = 0.0;

            DateTime resultEnd = DateTime.ParseExact(project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime resultStart = DateTime.ParseExact(project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            totaldays = (resultEnd - resultStart).TotalDays + 1;



            var projectUpdated = _ProjectRepository.GetById(project.ProjectId);
            projectUpdated.NoOfDays = Convert.ToInt32(totaldays);
            projectUpdated.CustomerId = project.CustomerId;
            projectUpdated.CityId = project.CityId;
            projectUpdated.ProjectDescription = project.ProjectDescription;
            projectUpdated.TransactionTypeId = project.TransactionTypeId;
            projectUpdated.RegionTypeId = project.RegionTypeId;
            projectUpdated.PieceNo = project.PieceNo;
            projectUpdated.SketchName = project.SketchName;
            projectUpdated.SketchNo = project.SketchNo;
            projectUpdated.SiteName = project.SiteName;
            projectUpdated.SiteNo = project.SiteNo;
            projectUpdated.GeneralLocation = project.GeneralLocation;
            projectUpdated.AdwAR = project.AdwAR;
            projectUpdated.AreaSpace = project.AreaSpace;
            projectUpdated.Ertedad = project.Ertedad;
            projectUpdated.OrderNo = project.OrderNo;
            projectUpdated.SpaceBuild = project.SpaceBuild;
            projectUpdated.XPoint = project.XPoint;
            projectUpdated.YPoint = project.YPoint;
            projectUpdated.Basement = project.Basement;
            projectUpdated.elevators = project.elevators;
            projectUpdated.entries = project.entries;
            projectUpdated.LicenseContent = project.LicenseContent;
            projectUpdated.Period = project.Period;
            projectUpdated.FloorEstablishing = project.FloorEstablishing;
            projectUpdated.Roof = project.Roof;
            projectUpdated.Electric = project.Electric;
            projectUpdated.AreaSpace = project.AreaSpace;
            projectUpdated.brozat = project.brozat;
            projectUpdated.FirstFloor = project.FirstFloor;
            projectUpdated.Motkrr = project.Motkrr;
            projectUpdated.DesiningOffice = project.DesiningOffice;
            projectUpdated.GroundFloor = project.GroundFloor;
            projectUpdated.ProjectDate = project.ProjectDate;
            projectUpdated.ProjectHijriDate = project.ProjectHijriDate;
            projectUpdated.Licensedate = project.Licensedate;
            projectUpdated.LicenseHijridate = project.LicenseHijridate;
            projectUpdated.LicenseNo = project.LicenseNo;
            projectUpdated.Takeem = project.Takeem;
            projectUpdated.AgentDate = project.AgentDate;
            projectUpdated.Takeef = project.Takeef;
            projectUpdated.AgentHijriDate = project.AgentHijriDate;
            projectUpdated.SupervisionNotes = project.SupervisionNotes;
            projectUpdated.DistrictName = project.DistrictName;
            projectUpdated.SupervisionSatartDate = project.SupervisionSatartDate;
            projectUpdated.SupervisionSatartHijriDate = project.SupervisionSatartHijriDate;
            projectUpdated.SupervisionEndDate = project.SupervisionEndDate;
            projectUpdated.SupervisionEndHijriDate = project.SupervisionEndHijriDate;
            projectUpdated.StreetName = project.StreetName;
            projectUpdated.SpaceName = project.SpaceName;
            projectUpdated.BuildingType = project.BuildingType;
            projectUpdated.ProjectName = project.ProjectName;
            projectUpdated.ProjectValue = project.ProjectValue;
            projectUpdated.ContractDate = project.ContractDate;
            projectUpdated.ContractHijriDate = project.ContractHijriDate;
            projectUpdated.ContractPeriod = project.ContractPeriod;
            projectUpdated.ProjectRecieveLoaction = project.ProjectRecieveLoaction;
            projectUpdated.ProjectTaslemFirst = project.ProjectTaslemFirst;
            projectUpdated.ProjectObserveName = project.ProjectObserveName;
            projectUpdated.ProjectObserveMobile = project.ProjectObserveMobile;
            projectUpdated.ProjectObserveMail = project.ProjectObserveMail;
            projectUpdated.ContractSource = project.ContractSource;
            projectUpdated.NesbaEngaz = project.NesbaEngaz;
            projectUpdated.MangerId = project.MangerId;
            projectUpdated.UpdateUser = UserId;
            projectUpdated.UpdateDate = DateTime.Now;


            projectUpdated.molhqalwisaqffash = project.molhqalwisaqffash;
            projectUpdated.molhqardisaqffash = project.molhqardisaqffash;


            //projectUpdated.RegionTypeName = project.RegionTypeName;
            projectUpdated.ZaraaSak = project.ZaraaSak;
            projectUpdated.ZaraaNatural = project.ZaraaNatural;
            //projectUpdated.ContractNo = project.ContractNo;

            //projectUpdated.SiteName = project.SiteName;
            //projectUpdated.SiteNo = project.SiteNo;
            projectUpdated.PayanNo = project.PayanNo;
            //projectUpdated.Ertedad = project.Ertedad;
            projectUpdated.Brooz = project.Brooz;
            projectUpdated.PieceNo = project.PieceNo;
            projectUpdated.UserId = project.UserId;
            //projectUpdated.XPoint = project.XPoint;
            //projectUpdated.YPoint = project.YPoint;
            projectUpdated.Office = project.Office;
            projectUpdated.ExtensionName = project.ExtensionName;
            projectUpdated.AreaSak = project.AreaSak;
            projectUpdated.AreaNatural = project.AreaNatural;
            projectUpdated.AreaArrange = project.AreaArrange;
            projectUpdated.Notes1 = project.Notes1;
            projectUpdated.RegionTypeId = project.RegionTypeId;
            //projectUpdated.regionTypes = project.regionTypes;
            projectUpdated.RegionName = project.RegionName;
            projectUpdated.ProjectRegionName = project.ProjectRegionName;
            projectUpdated.DistrictName = project.DistrictName;
            projectUpdated.BuildingPercent = project.BuildingPercent;
            projectUpdated.RegionName = project.RegionName;
            projectUpdated.SpaceNotes = project.SpaceNotes;


            projectUpdated.Co_opOfficeName = project.Co_opOfficeName;
            projectUpdated.Co_opOfficeEmail = project.Co_opOfficeEmail;
            projectUpdated.Co_opOfficePhone = project.Co_opOfficePhone;
            projectUpdated.ContractorSelectId = project.ContractorSelectId;

            projectUpdated.MunicipalId = project.MunicipalId;
            projectUpdated.SubMunicipalityId = project.SubMunicipalityId;
            projectUpdated.Catego = project.Catego;

            projectUpdated.ProBuildingDisc = project.ProBuildingDisc;
            projectUpdated.Cons_components = project.Cons_components;

            //projectUpdated.RegionTypeId = project.RegionName;
            //projectUpdated.reg = project.BuildingPercent;
            //projectUpdated.note = project.ProjectRegionName;



            //projectUpdated.RegionName = project.RegionName;
            //projectUpdated.Office = project.Office;
            //End


            _TaamerProContext.SaveChanges();
            //-----------------------------------------------------------------------------------------------------------------
            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string ActionNote = "حفظ تفاصيل المشروع " + projectUpdated.ProjectNo;
            _SystemAction.SaveAction("SaveProjectDetails", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
            //-----------------------------------------------------------------------------------------------------------------
            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = projectUpdated.ProjectTypeId };

        }
        public async Task<IEnumerable<ProjectVM>> GetProjectsByCustomerId(int? CustomerId, int? Status)
        {
            var projects = await _ProjectRepository.GetProjectsByCustomerId(CustomerId, Status);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetSomeDataByProjId(int? ProjectId)
        {
            var projects = await _ProjectRepository.GetSomeDataByProjId(ProjectId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetUserProjects(int UserId, int BranchId, string DateNow)
        {
            var projects = await _ProjectRepository.GetUserProjects(UserId, BranchId, DateNow);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetUserProjects2(int UserId, int BranchId, string DateNow)
        {
            var projects = await _ProjectRepository.GetUserProjects2(UserId, BranchId, DateNow);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int UserId, int BranchId, string DateNow)
        {
            var projects = await _ProjectRepository.GetUserProjectsReport(UserId, BranchId, DateNow);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, List<int> BranchesList)
        {
            var projects = await _ProjectRepository.GetUserProjectsReport(UserId, CustomerId, BranchId, DateFrom, DateTo,BranchesList);
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetUserProjectsReport(int? UserId, int? CustomerId, int BranchId, string DateFrom, string DateTo, string? Searchtext)
        {
            var projects = await _ProjectRepository.GetUserProjectsReport(UserId, CustomerId, BranchId, DateFrom, DateTo, Searchtext);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetUserProjectsReportW(int BranchId, string DateNow)
        {
            var projects = await _ProjectRepository.GetUserProjectsReportW(BranchId, DateNow);
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetUserProjectsReportW2(int BranchId, string DateFrom, string DateTo)
        {
            var projects = await _ProjectRepository.GetUserProjectsReportW2(BranchId, DateFrom, DateTo);
            return projects;
        }
        public GeneralMessage checkprojectpercentage(int branchid, string date, int phasetakid)
        {
            var projects = _ProjectRepository.GetUserProjectsReportW(branchid, date);

            var task = _ProjectPhasesTasksRepository.GetById(phasetakid);
            var project = projects.Result.Where(s => s.ProjectId == task.ProjectId).FirstOrDefault();
            var excount = project.TaskExecPercentage_Count * 100;
            double percent = Convert.ToDouble(excount);
            double percent2 = Convert.ToDouble(project.TaskExecPercentage_Sum);
            double perc = (double)(percent2 / excount);
            double total = perc * 100;

            if (total >= 70)
            {
                ///do here

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "" };
            }
            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "" };

        }

        public async Task<IEnumerable<ProjectVM>> GetProjectsSearch(ProjectVM ProjectsSearch, int BranchId, string Con, string lang)
        {
            var projects = await _ProjectRepository.GetProjectsSearch(ProjectsSearch, BranchId, Con, lang);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetProjectsStatusTasksSearch(ProjectVM ProjectsSearch, int BranchId, string Con, string lang)
        {
            var projects = await _ProjectStatusTasksRepository.GetProjectsStatusTasksSearch(ProjectsSearch, lang, Con, BranchId);
            return projects;
        }
        public async Task<IEnumerable<object>> FillAllUsersByProject(int BranchId, string lang)
        {
            var projects = await _ProjectRepository.FillAllUsersByProject(BranchId, lang);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjectsByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllProjectsByDateSearch(DateFrom, DateTo, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetArchiveProjectsSearch(ProjectVM ProjectsSearch, int BranchId)
        {
            var projects = await _ProjectRepository.GetArchiveProjectsSearch(ProjectsSearch, BranchId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllArchiveProjectsByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var projects = await _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);
            return projects;
        }

        public IEnumerable<object> GetAllWOStatuses(string Con)
        {
            //var Statuses = _ProjectRepository.GetAllArchiveProjectsByDateSearch(DateFrom, DateTo, BranchId);

            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter("select TaskStatusID,NameAr from Pro_TaskStatus", Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString()
            });
        }

        public GeneralMessage FinishProject(int projectId, int ReasonsId, string reason, int Reasontype, string Reasontext, string Date, int UserId, string Con, int BranchId, string Url, string ImgUrl, int TypeId, int whichClickDesc)
        {
            try
            {
                List<int> usersnote = new List<int>();

                if (Reasontype != 3)
                {
                    var projectContracts = _TaamerProContext.Contracts.Where(s => s.IsDeleted == false && s.ProjectId == projectId);
                    foreach (var item in projectContracts)
                    {
                        var contractPayment = _customerPaymentsRepository.GetMatching(s => s.IsDeleted == false && s.ContractId == item.ContractId && s.IsPaid == false).FirstOrDefault();
                        if (contractPayment != null)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = Resources.General_SavedFailed;
                            _SystemAction.SaveAction("FinishProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.cantFinishproject };
                        }
                    }

                    var InvoiceCount = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.ProjectId == projectId && s.IsPost == false && s.Type == 2);
                    if (InvoiceCount.Count() > 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = Resources.General_SavedFailed;
                        _SystemAction.SaveAction("FinishProject", "ProjectService", 1, Resources.project_has_unpaid_bills, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.project_has_unpaid_bills };
                    }

                    var ProjectTasks = _ProjectPhasesTasksRepository.GetAllTasksByProjectIdForFinish(projectId).Result;
                    if (ProjectTasks.Count() != 0)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = Resources.General_SavedFailed;
                        _SystemAction.SaveAction("FinishProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.cantFinishproject1 };
                    }
                }

                Project proj = _ProjectRepository.GetById(projectId);
                var customer = _CustomerRepository.GetById(proj.CustomerId ?? 0);
                proj.UpdateUser = UserId;
                proj.UpdateDate = DateTime.Now;
                proj.Status = 1; // finsihed
                proj.Reason1 = reason;
                proj.ReasonID = Reasontype;
                proj.ReasonText = Reasontext;
                proj.DateOfFinish = Date;
                proj.FinishDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                proj.FinishHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                proj.ReasonsId = ReasonsId;

                // Project proj = _ProjectRepository.GetById(projectId);

                ////////////////////////////////////////////////////
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                var resn = _TaamerProContext.Pro_projectsReasons.Where(x => x.ReasonsId == ReasonsId).FirstOrDefault();
                //var phasesTasks = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId && s.Type == 3 && s.IsMerig == -1 && s.Status == 3 && s.Active == true);

                //foreach (var phase in phasesTasks)
                //{
                //    phase.Status = 2;
                //    usersnote.Add(phase.UserId.Value);

                //}
                #region Notifications

                try
                {
                    //get notification configuration users and description
                    var (usersList, descriptionFromConfig) = GetNotificationRecipients(NotificationCode.Project_Completed, projectId);
                    var description = "  انهاء المشروع وتحويلة الي الارشيف";

                    if (descriptionFromConfig != null && descriptionFromConfig != "")
                        description = descriptionFromConfig;
                    var manager = _usersRepository.GetById(proj.MangerId.Value);
                    var updatetduse = _usersRepository.GetById(UserId);
                    //if no configuration send to emp and manager
                    if (usersList == null || usersList.Count == 0)
                    {

                        var ListOfPrivNotify = new List<Notification>();
                        usersnote.Add(proj.MangerId.Value);
                        var worker = _TaamerProContext.ProUserPrivileges
                             .Where(x => x.ProjectID == projectId)
                             .Select(x => x.UserId)
                             .Where(id => id.HasValue)  // Filters non-null values if UserId is int?
                             .Select(id => id.Value)    // Converts int? to int
                             .ToList();

                        usersnote.AddRange(worker);
                        if (usersnote.Count() != 0)
                        {
                          

                            foreach (var usr in usersnote.Distinct())
                            {

                                Users user = _usersRepository.GetById(usr);

                                var UserNotification = new Notification
                                {
                                    ReceiveUserId = usr,
                                    Name = description,
                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                    SendUserId = 1,
                                    Type = 1, // notification
                                    Description = " تم انهاء المشروع   رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + "وتحويلة من جاري الي الارشيف" + " بسبب " + resn.NameAr,
                                    AllUsers = false,
                                    SendDate = DateTime.Now,
                                    ProjectId = projectId,
                                    TaskId = 0,
                                    BranchId = BranchId,
                                    AddUser = UserId,
                                    AddDate = DateTime.Now,
                                    IsHidden = false,
                                    NextTime = null,
                                };
                                _TaamerProContext.Notification.Add(UserNotification);
                                _TaamerProContext.SaveChanges();

                                _notificationService.sendmobilenotification(usr, description, " تم انهاء مشروع رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + " وتحويلة من جاري الي الارشيف" + " بسبب " + resn.NameAr);

                                var htmlBody = "";


                                htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + proj.ProjectNo + @"</td>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>مدير المشروع </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + manager.FullNameAr + @"</td>
                                              
                                                    </tr>
       
                                                                <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ الانهاء  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")) + @"</td>
                                              
                                                    </tr>
                                                                     <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تم الانهاء بواسطة  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + updatetduse.FullNameAr + @"</td>
                                              
                                                    </tr>
   <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>  سبب انهاء المشروع   </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Reasontext + @"</td>
                                              
                                                    </tr>

                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>  السبب   </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + resn.NameAr + @"</td>
                                              
                                                    </tr>

                                                </table>
                                            </body>
                                            </html>";
                                SendMail_ProjectStamp(BranchId, UserId, usr, description, htmlBody, Url, ImgUrl, 1, true);


                                var NotStr = "Dear : " + user.FullName + " Project No " + proj.ProjectNo + " For Customer " + customer.CustomerNameAr + " has working ";
                                if (user.Mobile != null && user.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(user.Mobile, NotStr, UserId, BranchId);
                                }
                                //}
                            }
                        }
                    }
                    else
                    {

                        foreach (var usr in usersList.Distinct())
                        {

                            Users user = _usersRepository.GetById(usr);

                            var UserNotification = new Notification
                            {
                                ReceiveUserId = usr,
                                Name = description,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1, // notification
                                Description = " تم انهاء المشروع   رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + "وتحويلة من جاري الي الارشيف" + " بسبب " + resn.NameAr,
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = projectId,
                                TaskId = 0,
                                BranchId = BranchId,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false,
                                NextTime = null,
                            };
                            _TaamerProContext.Notification.Add(UserNotification);
                            _TaamerProContext.SaveChanges();

                            _notificationService.sendmobilenotification(usr, description, " تم انهاء مشروع رقم  : " + proj.ProjectNo + " للعميل " + customer.CustomerNameAr + " وتحويلة من جاري الي الارشيف" + " بسبب " + resn.NameAr);

                            var htmlBody = "";


                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + proj.ProjectNo + @"</td>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>مدير المشروع </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + manager.FullNameAr + @"</td>
                                              
                                                    </tr>
       
                                                                <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تاريخ الانهاء  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")) + @"</td>
                                              
                                                    </tr>
                                                                     <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>تم الانهاء بواسطة  </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + updatetduse.FullNameAr + @"</td>
                                              
                                                    </tr>
   <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>  سبب انهاء المشروع   </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Reasontext + @"</td>
                                              
                                                    </tr>

                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>  السبب   </td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + resn.NameAr + @"</td>
                                              
                                                    </tr>

                                                </table>
                                            </body>
                                            </html>";
                            SendMail_ProjectStamp(BranchId, UserId, usr, description, htmlBody, Url, ImgUrl, 1, true);


                            var NotStr = "Dear : " + user.FullName + " Project No " + proj.ProjectNo + " For Customer " + customer.CustomerNameAr + " has working ";
                            if (user.Mobile != null && user.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(user.Mobile, NotStr, UserId, BranchId);
                            }
                            
                        }
                    }


                    }
                catch (Exception ex)
                {

                }
                #endregion




                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " انهاء المشروع " + " " + "رقم " + " " + proj.ProjectNo;
                _SystemAction.SaveAction("FinishProject", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                ////////////////////////////////
                ///
                SqlConnection con = new SqlConnection(Con);
                con.Open();
                SqlCommand cmd = new SqlCommand("  update [Pro_PhasesTasks] set [Status]=5 where projectid =" + projectId + " and ([status] <>4 and status <>0)");
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();

                ///////////////////////////////////////////
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.FinishProject };



            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في انهاء المشروع" + projectId;
                _SystemAction.SaveAction("FinishProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.failedFinishProject };
            }
        }
        public async Task<string> GenerateNextProjectNumber(int BranchId)
        {
            var OrganzationId = _branchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            var codePrefix = "";
            var prostartcode = _branchesRepository.GetById(BranchId).ProjectStartCode;
            if (prostartcode != null && prostartcode != "")
            {
                codePrefix = prostartcode;
            }
            else if (sysSetting != null && sysSetting.ProjGenerateCode != null)
            {
                codePrefix = sysSetting.ProjGenerateCode;
            }
            return (codePrefix + await _ProjectRepository.GenerateNextProjectNumber(BranchId, codePrefix));
        }
        public async Task<string> GetProjectCode_S(int BranchId)
        {
            var OrganzationId = _branchesRepository.GetById(BranchId).OrganizationId;
            var sysSetting = _SystemSettingsRepository.GetSystemSettingsByBranchId(OrganzationId).Result;
            //var codePrefix = "";
            //if (sysSetting != null && sysSetting.ProjGenerateCode != null)
            //{
            //    codePrefix = sysSetting.ProjGenerateCode;
            //}
            var codePrefix = "";
            var prostartcode = _branchesRepository.GetById(BranchId).ProjectStartCode;
            if (prostartcode != null && prostartcode != "")
            {
                codePrefix = prostartcode;
            }
            else if (sysSetting != null && sysSetting.ProjGenerateCode != null)
            {
                codePrefix = sysSetting.ProjGenerateCode;
            }
            return codePrefix;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllHirearchialProject(int BranchId, int UserId)
        {
            var projects = await _ProjectRepository.GetAllHirearchialProject(BranchId, UserId);
            return projects;
        }


        public GeneralMessage UpdateStatusProject(int projectId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var projectUpdated = _ProjectRepository.GetById(projectId);

                if (projectUpdated != null)
                {
                    var customer = _CustomerRepository.GetById(projectUpdated.CustomerId ?? 0);

                    projectUpdated.Status = 0;
                    projectUpdated.UpdateUser = projectUpdated.UserId = UserId;
                    projectUpdated.UpdateDate = DateTime.Now;
                    //_ProjectRepository.Add(projectUpdated);
                    _TaamerProContext.SaveChanges();

                    //DateTime date = DateTime.ParseExact(DateTime.Now.ToLongDateString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    ProjectArchivesRe Pro = new ProjectArchivesRe();
                    Pro.ProjectId = projectId;
                    Pro.ReDate = formattedDate;
                    Pro.Re_TypeID = 1;
                    Pro.Re_TypeName = "تحويل المشروع من ارشيف الي جاري";
                    _TaamerProContext.ProjectArchivesRe.Add(Pro);

                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(projectUpdated.MangerId ?? 0).Result;
                    var body = "";
                    if (UserNotifPriv.Count() != 0)
                    {



                        if (UserNotifPriv.Contains(3131))
                        {
                            var Desc = formattedDate + " بتاريخ " + " من ارشيف الي جاري " + projectUpdated.ProjectNo + " تحويل المشروع  ";

                            //SendMailNoti(projectUpdated.ProjectId, Desc, "تحويل المشروع من ارشيف الي جاري", BranchId, UserId, projectUpdated.MangerId ?? 0);

                            body = "<table border='1'style='text-align:center;padding:10px;'><tr><th style='border=1px solid #eee'>رقم المشروع  </th><th>اسم العميل  </th><th>التاريخ </th><th>الساعة </th><th>بواسطة </th><th>الفرع </th></tr><tr><td>" + projectUpdated.ProjectNo + "</td><td>" + customer.CustomerNameAr + "</td><td>" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "</td><td>" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "</td><td>" + _usersRepository.GetById((int)projectUpdated.MangerId).FullName + "</td><td>" + _BranchesRepository.GetById(BranchId).NameAr + "</td></tr></table>";


                            SendMail_ProjectStamp(BranchId, UserId, projectUpdated.MangerId ?? 0, "انهاء مشروع", body, Url, ImgUrl, 4, true);


                        }
                        if (UserNotifPriv.Contains(3133))
                        {
                            var userObj = _usersRepository.GetById((int)projectUpdated.MangerId);
                            var NotStr = formattedDate + " بتاريخ " + " من ارشيف الي جاري " + projectUpdated.ProjectNo + " تحويل المشروع  ";
                            if (userObj.Mobile != null && userObj.Mobile != "")
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                            }
                        }

                    }



                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تحويل المشروع من ارشيف الي جاري";
                    _SystemAction.SaveAction("UpdateStatusProject", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------


                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في تحويل المشروع من ارشيف الي جاري";
                    _SystemAction.SaveAction("UpdateStatusProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("UpdateStatusProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage ConvertManagerProjectsSome(int projectId, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var projectUpdated = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId);

                if (projectUpdated != null && projectUpdated.Count() > 0)
                {

                    foreach (var item in projectUpdated)
                    {
                        var customer = _CustomerRepository.GetById(item.CustomerId ?? 0);

                        var userFrom = FromUserId;
                        item.MangerId = ToUserId;
                        item.UpdateUser = UserId;
                        item.UpdateDate = DateTime.Now;
                        try
                        {
                            var branch = _BranchesRepository.GetById(BranchId);

                            var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId).Result;
                            if (UserNotifPriv.Count() != 0)
                            {
                                if (UserNotifPriv.Contains(3212))
                                {
                                    var UserNotification = new Notification();
                                    UserNotification.ReceiveUserId = ToUserId;
                                    UserNotification.Name = "Resources.General_Newtasks";
                                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                    UserNotification.SendUserId = 1;
                                    UserNotification.Type = 1; // notification
                                    UserNotification.Description = "تم تحويل مدير المشروع رقم  : " + item.ProjectNo + " من " + _usersRepository.GetById(userFrom).FullName + " الي  " + _usersRepository.GetById(ToUserId).FullName;
                                    UserNotification.AllUsers = false;
                                    UserNotification.SendDate = DateTime.Now;
                                    UserNotification.ProjectId = item.ProjectId;
                                    UserNotification.TaskId = 0;
                                    UserNotification.AddUser = UserId;
                                    UserNotification.IsHidden = false;
                                    UserNotification.NextTime = null;
                                    UserNotification.AddDate = DateTime.Now;
                                    _TaamerProContext.Notification.Add(UserNotification);
                                    _notificationService.sendmobilenotification(ToUserId, "Resources.General_Newtasks", "تم تحويل مدير المشروع رقم  : " + item.ProjectNo + " من " + _usersRepository.GetById(userFrom).FullName + " الي  " + _usersRepository.GetById(ToUserId).FullName);
                                }

                                if (UserNotifPriv.Contains(3213))
                                {
                                    var userObj = _usersRepository.GetById(ToUserId);

                                    var NotStr = _usersRepository.GetById(ToUserId).FullName + " الي  " + _usersRepository.GetById(userFrom).FullName + " من " + item.ProjectNo + " تم تحويل مدير المشروع رقم   ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }

                                if (UserNotifPriv.Contains(3211))
                                {
                                    //var Desc = _usersRepository.GetById(ToUserId).FullName + " الي  " + _usersRepository.GetById(userFrom).FullName + " من " + item.ProjectNo + " تم تحويل مدير المشروع رقم   ";

                                    //SendMailNoti(item.ProjectId, Desc, "تم اضافتك مدير علي مشروع", BranchId, UserId, ToUserId);

                                    string htmlBody = "";

                                    var Desc = "السيد / ة  " + _usersRepository.GetById(ToUserId).FullName + "المحترم  " + "<br/>" + "السلام عليكم ورحمة الله وبركاتة " + "<br/>" + "<h2> تم اضافتك مدير علي مشروع </h2>" + " رقم المشروع  " + item.ProjectNo + "<br/>" + " للعميل  " + customer.CustomerNameAr + "<br/>" + "مع تحيات قسم ادارة المشاريع";


                                    htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + item.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                    //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                                    SendMail_ProjectStamp(BranchId, UserId, ToUserId, "مشروع جديد محول", htmlBody, Url, ImgUrl, 5, true);
                                }
                            }

                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل المشروع";
                _SystemAction.SaveAction("ConvertManagerProjectsSome", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المشروع";
                _SystemAction.SaveAction("ConvertManagerProjectsSome", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage ConvertMoreManagerProjects(List<int> projectIds, int FromUserId, int ToUserId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                if (projectIds.Count() > 0)
                {
                    foreach (var projectId in projectIds)
                    {


                        var projectUpdated = _ProjectRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == projectId);
                        if (projectUpdated != null && projectUpdated.Count() > 0)
                        {
                            foreach (var item in projectUpdated)
                            {
                                var customer = _CustomerRepository.GetById(item.CustomerId ?? 0);

                                var userFrom = FromUserId;
                                item.MangerId = ToUserId;
                                item.UpdateUser = UserId;
                                item.UpdateDate = DateTime.Now;
                                try
                                {
                                    var branch = _BranchesRepository.GetById(BranchId);

                                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(ToUserId).Result;
                                    if (UserNotifPriv.Count() != 0)
                                    {
                                        if (UserNotifPriv.Contains(3212))
                                        {
                                            var UserNotification = new Notification();
                                            UserNotification.ReceiveUserId = ToUserId;
                                            UserNotification.Name = "Resources.General_Newtasks";
                                            UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                            UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                            UserNotification.SendUserId = 1;
                                            UserNotification.Type = 1; // notification
                                            UserNotification.Description = "تم تحويل مدير المشروع رقم  : " + item.ProjectNo + " من " + _usersRepository.GetById(userFrom).FullName + " الي  " + _usersRepository.GetById(ToUserId).FullName;
                                            UserNotification.AllUsers = false;
                                            UserNotification.SendDate = DateTime.Now;
                                            UserNotification.ProjectId = item.ProjectId;
                                            UserNotification.TaskId = 0;
                                            UserNotification.AddUser = UserId;
                                            UserNotification.IsHidden = false;
                                            UserNotification.NextTime = null;
                                            UserNotification.AddDate = DateTime.Now;
                                            _TaamerProContext.Notification.Add(UserNotification);
                                            _notificationService.sendmobilenotification(ToUserId, "Resources.General_Newtasks", "تم تحويل مدير المشروع رقم  : " + item.ProjectNo + " من " + _usersRepository.GetById(userFrom).FullName + " الي  " + _usersRepository.GetById(ToUserId).FullName);
                                        }

                                        if (UserNotifPriv.Contains(3213))
                                        {
                                            var userObj = _usersRepository.GetById(ToUserId);

                                            var NotStr = _usersRepository.GetById(ToUserId).FullName + " الي  " + _usersRepository.GetById(userFrom).FullName + " من " + item.ProjectNo + " تم تحويل مدير المشروع رقم   ";
                                            if (userObj.Mobile != null && userObj.Mobile != "")
                                            {
                                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                            }
                                        }

                                        if (UserNotifPriv.Contains(3211))
                                        {
                                            string htmlBody = "";
                                            //var Desc = _usersRepository.GetById(ToUserId).FullName + " الي  " + _usersRepository.GetById(userFrom).FullName + " من " + item.ProjectNo + " تم تحويل مدير المشروع رقم   ";

                                            //SendMailNoti(item.ProjectId, Desc, "تم اضافتك مدير علي مشروع", BranchId, UserId, ToUserId);




                                            var Desc = "السيد / ة  " + _usersRepository.GetById(ToUserId).FullName + "المحترم  " + "<br/>" + "السلام عليكم ورحمة الله وبركاتة " + "<br/>" + "<h2> تم اضافتك مدير علي مشروع </h2>" + " رقم المشروع  " + item.ProjectNo + "<br/>" + " للعميل  " + customer.CustomerNameAr + "<br/>" + "مع تحيات قسم ادارة المشاريع";


                                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + item.ProjectNo + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                            //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                                            SendMail_ProjectStamp(BranchId, UserId, ToUserId, "مشروع جديد محول", htmlBody, Url, ImgUrl, 5, true);


                                        }
                                    }

                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                    _TaamerProContext.SaveChanges();
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تحويل المشروع";
                _SystemAction.SaveAction("ConvertManagerProjectsSome", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تحويل المشروع";
                _SystemAction.SaveAction("ConvertManagerProjectsSome", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustId(string lang, int BranchId, int? CustId)
        {
            var Proj = await _ProjectRepository.GetAllProjByCustId(lang, BranchId, CustId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustIdWithout(string lang, int BranchId, int? CustId)
        {
            var Proj = await _ProjectRepository.GetAllProjByCustIdWithout(lang, BranchId, CustId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByBranch(string lang, int BranchId)
        {
            var Proj = await _ProjectRepository.GetAllProjByBranch(lang, BranchId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByBranchWithout(string lang, int BranchId)
        {
            var Proj = await _ProjectRepository.GetAllProjByBranchWithout(lang, BranchId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustomerId(int customerId)
        {
            var Proj = await _ProjectRepository.GetAllProjByCustomerId(customerId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdWithout(int customerId)
        {
            var Proj = await _ProjectRepository.GetAllProjByCustomerIdWithout(customerId);
            return Proj;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdHaveTasks(int customerId)
        {
            var Proj = await _ProjectRepository.GetAllProjByCustomerIdHaveTasks(customerId);
            return Proj;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjByCustomerIdandbranchHaveTasks(int customerId, int BranchId)
        {
            var Proj = await _ProjectRepository.GetAllProjByCustomerIdandbranchHaveTasks(customerId, BranchId);
            return Proj;
        }

        public async Task<IEnumerable<ProjectVM>> GetAllProjByFawater(string lang, int BranchId)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList();
            var Proj = _ProjectRepository.GetAllProjByFawater(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId));
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByMrdod(string lang, int BranchId)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2 && s.Rad == true).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList();
            var Proj = _ProjectRepository.GetAllProjByMrdod(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId));
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByNoti(string lang, int BranchId, int? yearid)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == yearid && (s.Type == 29 || s.Type == 30)).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList();
            var Proj = _ProjectRepository.GetAllProjByMrdod(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId));
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByMrdod_Pur(string lang, int BranchId)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 1 && s.Rad == true).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList();
            var Proj = _ProjectRepository.GetAllProjByMrdod(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId));
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByMrdod_C(string lang, int BranchId, int? CustomerId)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2 && s.Rad == true).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList();
            var Proj = _ProjectRepository.GetAllProjByMrdod(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId) && w.CustomerId == CustomerId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByNoti_C(string lang, int BranchId, int? CustomerId, int? yearid)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == yearid && (s.Type == 29 || s.Type == 30)).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList();
            var Proj = _ProjectRepository.GetAllProjByMrdod(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId) && w.CustomerId == CustomerId);
            return Proj;
        }
        public async Task<IEnumerable<ProjectVM>> GetAllProjByMrdod_C_Pur(string lang, int BranchId, int? SupplierId)
        {
            var AllProjInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 1 && s.Rad == true && s.SupplierId == SupplierId).Where(w => !(w.ProjectId == null || w.ProjectId.Equals(""))).Select(s => s.ProjectId).ToList(); ;
            var Proj = _ProjectRepository.GetAllProjByMrdod(lang, BranchId).Result.Where(w => AllProjInvoices.Contains(w.ProjectId));
            return Proj;
        }
        public IEnumerable<CustomerVM> GetAllCustByFawater(string lang, int BranchId)
        {
            var AllCustInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2).Where(w => !(w.CustomerId == null || w.CustomerId.Equals(""))).Select(s => s.CustomerId).ToList();
            var Cust = _CustomerRepository.GetAllCustomer().Result.Where(w => AllCustInvoices.Contains(w.CustomerId));
            return Cust;
        }
        public IEnumerable<CustomerVM> GetAllCustByMrdod(string lang, int BranchId)
        {
            var AllCustInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 2 && s.Rad == true).Where(w => !(w.CustomerId == null || w.CustomerId.Equals(""))).Select(s => s.CustomerId).ToList();
            var Cust = _CustomerRepository.GetAllCustomer().Result.Where(w => AllCustInvoices.Contains(w.CustomerId));
            return Cust;
        }
        public IEnumerable<CustomerVM> GetAllCustByNoti(string lang, int BranchId, int? yearid)
        {
            var AllCustInvoices = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.YearId == yearid && (s.Type == 29 || s.Type == 30)).Where(w => !(w.CustomerId == null || w.CustomerId.Equals(""))).Select(s => s.CustomerId).ToList();
            var Cust = _CustomerRepository.GetAllCustomer().Result.Where(w => AllCustInvoices.Contains(w.CustomerId));
            return Cust;
        }
        public IEnumerable<CustomerVM> GetAllCustByReVoucher(string lang, int BranchId)
        {
            var AllCustReVoucher = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.Type == 6).Where(w => !(w.CustomerId == null || w.CustomerId.Equals(""))).Select(s => s.CustomerId).ToList();
            var Cust = _CustomerRepository.GetAllCustomer().Result.Where(w => AllCustReVoucher.Contains(w.CustomerId));
            return Cust;
        }

        public GeneralMessage UpdateProject(Project project, int UserId, int BranchId)
        {

            try
            {
                var projectUpdated = _ProjectRepository.GetById(project.ProjectId);


                var totaldays = 0.0;
                DateTime resultEnd = DateTime.ParseExact(project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime resultStart = DateTime.ParseExact(project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                totaldays = (resultEnd - resultStart).TotalDays + 1;
                if (projectUpdated.ProjectDate != project.ProjectDate || projectUpdated.ProjectExpireDate != project.ProjectExpireDate)
                {
                    projectUpdated.IsNotSent = false;

                    if (projectUpdated.Plustimecount == null)
                    {
                        projectUpdated.Plustimecount = 1;
                    }
                    else
                    {
                        projectUpdated.Plustimecount = projectUpdated.Plustimecount + 1;
                    }

                }

                DateTime resultNow = DateTime.Now;

                DateTime resultEndSelect = DateTime.ParseExact(projectUpdated.FirstProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


                if (resultNow < resultEndSelect)
                {
                    projectUpdated.FirstProjectExpireDate = project.ProjectExpireDate;
                    projectUpdated.FirstProjectDate = project.ProjectDate;

                }

                projectUpdated.ProjectDate = project.ProjectDate;
                projectUpdated.ProjectHijriDate = project.ProjectHijriDate;
                projectUpdated.ProjectExpireDate = project.ProjectExpireDate;
                projectUpdated.ProjectExpireHijriDate = project.ProjectExpireHijriDate;


                projectUpdated.NoOfDays = Convert.ToInt32(totaldays);

                projectUpdated.BuildingType = project.BuildingType;



                if (projectUpdated.MangerId != project.MangerId)
                {
                    var ProjectWorkersUpdated = _TaamerProContext.ProjectWorkers.Where(s => s.ProjectId == project.ProjectId && s.WorkerType == 1).FirstOrDefault();
                    if (ProjectWorkersUpdated != null)
                    {
                        ProjectWorkersUpdated.UserId = project.MangerId;
                    }

                }

                if (projectUpdated.CostCenterId != project.CostCenterId)
                {
                    if (project.CostCenterId != null)
                    {
                        var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == project.ProjectId).FirstOrDefault();
                        var CostCenterParent = _CostCenterRepository.GetById((int)project.CostCenterId);

                        if (CostCenterProject != null)
                        {
                            try
                            {
                                CostCenterProject.BranchId = CostCenterParent.BranchId;
                                CostCenterProject.ParentId = CostCenterParent.CostCenterId;
                                CostCenterProject.Level = (CostCenterParent.Level ?? 0) + 1;
                            }
                            catch (Exception)
                            {
                            }
                        }


                    }

                }

                if (projectUpdated.CustomerId != project.CustomerId)
                {
                    if (project.CustomerId != null)
                    {
                        var CostCenterProjectNew = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == project.ProjectId).FirstOrDefault();
                        var CustomerNew = _CustomerRepository.GetById((int)project.CustomerId);

                        CostCenterProjectNew.NameAr = CustomerNew.CustomerNameAr;
                        CostCenterProjectNew.NameEn = CustomerNew.CustomerNameEn;

                    }

                }
                projectUpdated.CustomerId = project.CustomerId;

                projectUpdated.CostCenterId = project.CostCenterId;
                projectUpdated.MangerId = project.MangerId;
                projectUpdated.CityId = project.CityId;
                projectUpdated.ProjectDescription = project.ProjectDescription;
                projectUpdated.ProjectTypeId = project.ProjectTypeId;
                projectUpdated.SubProjectTypeId = project.SubProjectTypeId; //aa  
                projectUpdated.BranchId = project.BranchId;
                projectUpdated.OffersPricesId = project.OffersPricesId;
                projectUpdated.DepartmentId = project.DepartmentId;
                projectUpdated.UpdateUser = UserId;
                projectUpdated.UpdateDate = DateTime.Now;

                /////add projectno to offerprice
                if (project.OffersPricesId != null)
                {
                    try
                    {

                        var offer = _offersPricesRepository.GetById((int)project.OffersPricesId);

                        var oldoffer = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.ProjectId == project.ProjectId).FirstOrDefault();
                        if (oldoffer != null)
                        {
                            if (oldoffer.OffersPricesId != offer.OffersPricesId)
                            {
                                oldoffer.ProjectId = null;
                            }
                            offer.ProjectId = project.ProjectId;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }



                var ProTemp = "-1";

                var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == project.ProjectId).ToList();
                var Oldphases = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == project.ProjectId).ToList();

                if (ProjectIds.Count() > 0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "مشروع بسير لا يمكنك تعديله بدون سير" };
                }

                if (ProjectIds.Count() > 0)//mshro3 bseeer 
                {
                    if (Oldphases.Count() > 0)
                    {
                        foreach (var item in Oldphases)
                        {
                            if (item.Type == 3 && (item.Status == 2 || item.Status == 3))
                            {

                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في الحفظ .. يوجد مهام قيد التشغيل او متوقفة يجب انهائها اولا";
                                _SystemAction.SaveAction("UpdateProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------
                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ .. يوجد مهام قيد التشغيل او متوقفة يجب انهائها اولا", ReturnedStr = "-1" };


                            }
                        }

                        _TaamerProContext.ProjectPhasesTasks.RemoveRange(Oldphases);
                        _TaamerProContext.TasksDependency.RemoveRange(ProjectIds);

                        ProTemp = project.ProjectId.ToString();
                    }
                    else
                    {
                        ProTemp = project.ProjectId.ToString();
                    }

                }
                else //mshro3 mn 8er seeer
                {
                    ProTemp = "-1";
                }


                //delete existing requirments 

                if (project.ProjectRequirementsGoals.Count() > 0)
                {
                    _TaamerProContext.ProjectRequirementsGoals.RemoveRange(projectUpdated.ProjectRequirementsGoals.ToList());

                    foreach (var item in project.ProjectRequirementsGoals.ToList())
                    {
                        item.AddDate = DateTime.Now;
                        item.AddUser = UserId;

                        item.ProjectId = project.ProjectId;
                        _TaamerProContext.ProjectRequirementsGoals.Add(item);  //dd
                    }

                }

                _TaamerProContext.SaveChanges();



                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل المشروع رقم " + project.ProjectNo;
                _SystemAction.SaveAction("UpdateProject", "ProjectService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = ProTemp };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("UpdateProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = "-1" };
            }
        }




        public GeneralMessage UpdateProjectEnddate(int projectid, string Enddate, int UserId, int BranchId)
        {

            try
            {
                bool flag = false;
                var project = _ProjectRepository.GetById(projectid);
                if (project.ProjectExpireDate != Enddate)
                {
                    flag = true;
                }
                project.ProjectExpireDate = Enddate;
                var projectUpdated = _ProjectRepository.GetById(project.ProjectId);


                var totaldays = 0.0;
                DateTime resultEnd = DateTime.ParseExact(project.ProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime resultStart = DateTime.ParseExact(project.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (resultStart > resultEnd)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "فشل في الحفظ . تاريخ النهاية اصغر من تاريه البداية";
                    _SystemAction.SaveAction("UpdateProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ . تاريخ النهاية اصغر من تاريه البداية", ReturnedStr = "-1" };
                }

                totaldays = (resultEnd - resultStart).TotalDays + 1;
                if (projectUpdated.ProjectDate != project.ProjectDate || projectUpdated.ProjectExpireDate != project.ProjectExpireDate)
                {
                    projectUpdated.IsNotSent = false;

                }
                if (flag == true)
                {
                    if (projectUpdated.Plustimecount == null)
                    {
                        projectUpdated.Plustimecount = 1;
                    }
                    else
                    {
                        projectUpdated.Plustimecount = projectUpdated.Plustimecount + 1;
                    }
                }

                DateTime resultNow = DateTime.Now;

                DateTime resultEndSelect = DateTime.ParseExact(projectUpdated.FirstProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


                if (resultNow < resultEndSelect)
                {
                    projectUpdated.FirstProjectExpireDate = project.ProjectExpireDate;
                    projectUpdated.FirstProjectDate = project.ProjectDate;

                }

                projectUpdated.ProjectDate = project.ProjectDate;
                projectUpdated.ProjectHijriDate = project.ProjectHijriDate;
                projectUpdated.ProjectExpireDate = project.ProjectExpireDate;
                projectUpdated.ProjectExpireHijriDate = project.ProjectExpireHijriDate;


                projectUpdated.NoOfDays = Convert.ToInt32(totaldays);

                //projectUpdated.BuildingType = project.BuildingType;



                //if (projectUpdated.MangerId != project.MangerId)
                //{
                //    var ProjectWorkersUpdated = _TaamerProContext.ProjectWorkers.Where(s => s.ProjectId == project.ProjectId && s.WorkerType == 1).FirstOrDefault();
                //    if (ProjectWorkersUpdated != null)
                //    {
                //        ProjectWorkersUpdated.UserId = project.MangerId;
                //    }

                //}

                //if (projectUpdated.CostCenterId != project.CostCenterId)
                //{
                //    if (project.CostCenterId != null)
                //    {
                //        var CostCenterProject = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == project.ProjectId).FirstOrDefault();
                //        var CostCenterParent = _CostCenterRepository.GetById(project.CostCenterId??0);

                //        if (CostCenterProject != null)
                //        {
                //            try
                //            {
                //                CostCenterProject.BranchId = CostCenterParent.BranchId;
                //                CostCenterProject.ParentId = CostCenterParent.CostCenterId;
                //                CostCenterProject.Level = (CostCenterParent.Level ?? 0) + 1;
                //            }
                //            catch (Exception)
                //            {
                //            }
                //        }


                //    }

                //}

                //if (projectUpdated.CustomerId != project.CustomerId)
                //{
                //    if (project.CustomerId != null)
                //    {
                //        var CostCenterProjectNew = _CostCenterRepository.GetMatching(s => s.IsDeleted == false && s.ProjId == project.ProjectId).FirstOrDefault();
                //        var CustomerNew = _CustomerRepository.GetById(project.CustomerId??0);

                //        CostCenterProjectNew.NameAr = CustomerNew?.CustomerNameAr??"";
                //        CostCenterProjectNew.NameEn = CustomerNew?.CustomerNameEn??"";

                //    }

                //}
                //projectUpdated.CustomerId = project.CustomerId;

                //projectUpdated.CostCenterId = project.CostCenterId;
                //projectUpdated.MangerId = project.MangerId;
                //projectUpdated.CityId = project.CityId;
                //projectUpdated.ProjectDescription = project.ProjectDescription;
                //projectUpdated.ProjectTypeId = project.ProjectTypeId;
                //projectUpdated.SubProjectTypeId = project.SubProjectTypeId;
                //projectUpdated.BranchId = project.BranchId;
                //projectUpdated.OffersPricesId = project.OffersPricesId;
                //projectUpdated.DepartmentId = project.DepartmentId;


                /////add projectno to offerprice
                //if (project.OffersPricesId != null)
                //{
                //    try
                //    {

                //        var offer = _offersPricesRepository.GetById(project.OffersPricesId??0);

                //        var oldoffer = _offersPricesRepository.GetMatching(x => x.IsDeleted == false && x.ProjectId == project.ProjectId).FirstOrDefault();
                //        if (oldoffer != null)
                //        {
                //            if (oldoffer.OffersPricesId != offer.OffersPricesId)
                //            {
                //                oldoffer.ProjectId = null;
                //            }
                //            offer.ProjectId = project.ProjectId;
                //        }

                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}



                var ProTemp = "-1";

                //var ProjectIds = _TasksDependencyRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == project.ProjectId).ToList();
                //var Oldphases = _ProjectPhasesTasksRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == project.ProjectId).ToList();

                //if (ProjectIds.Count() > 0)//mshro3 bseeer 
                //{
                //    if (Oldphases.Count() > 0)
                //    {
                //        foreach (var item in Oldphases)
                //        {
                //            if (item.Type == 3 && (item.Status == 2 || item.Status == 3))
                //            {

                //                //-----------------------------------------------------------------------------------------------------------------
                //                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //                string ActionNote2 = "فشل في الحفظ .. يوجد مهام قيد التشغيل او متوقفة يجب انهائها اولا";
                //                 _SystemAction.SaveAction("UpdateProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //                //-----------------------------------------------------------------------------------------------------------------
                //                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ .. يوجد مهام قيد التشغيل او متوقفة يجب انهائها اولا", ReturnedStr = "-1" };

                //            }
                //        }

                //        _TaamerProContext.ProjectPhasesTasks.RemoveRange(Oldphases);
                //        _TaamerProContext.TasksDependency.RemoveRange(ProjectIds);

                //        ProTemp = project.ProjectId.ToString();
                //    }
                //    else
                //    {
                //        ProTemp = project.ProjectId.ToString();
                //    }

                //}
                //else //mshro3 mn 8er seeer
                //{
                //    ProTemp = "-1";
                //}


                ////delete existing requirments 

                //if (project.ProjectRequirementsGoals !=null)
                //{
                //    _TaamerProContext.ProjectRequirementsGoals.RemoveRange(projectUpdated.ProjectRequirementsGoals.ToList());

                //    foreach (var item in project.ProjectRequirementsGoals.ToList())
                //    {
                //        item.AddDate = DateTime.Now;
                //        item.AddUser = UserId;

                //        item.ProjectId = project.ProjectId;
                //        _TaamerProContext.ProjectRequirementsGoals.Add(item);
                //    }

                //}

                _TaamerProContext.SaveChanges();



                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تمديد المشروع رقم " + project.ProjectNo;
                _SystemAction.SaveAction("UpdateProjectEnddate", "ProjectService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = ProTemp };
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = Resources.General_SavedFailed;
                _SystemAction.SaveAction("UpdateProjectEnddate", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = "-1" };
            }
        }
        public async Task<IEnumerable<ProjectVM>> GetProjectsStoppedVM()
        {
            var projects = await _ProjectRepository.GetProjectsStoppedVM();
            return projects;
        }
        public async Task<List<ProjectVMNewCounting>> GetProjectVMNew(string Lang, string Con, int BranchId)
        {
            var projects = await _ProjectRepository.GetProjectVMNew(Lang, Con, BranchId);
            return projects;
        }
        public async Task<List<ProjectVMNewStat>> GetProjectVMStatNew(int ProjectId, string Lang, string Con, int BranchId)
        {
            var projects = await _ProjectRepository.GetProjectVMStatNew(ProjectId, Lang, Con, BranchId);
            return projects;
        }
        public async Task<List<ProjectVMPhasesDetails>> GetPhasesDetails(string Lang, string Con, int ProjectId)
        {
            var projects = await _ProjectRepository.GetPhasesDetails(Lang, Con, ProjectId);
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetdestinationsUploadVM()
        {
            var projects = await _ProjectRepository.GetdestinationsUploadVM();
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetAlmostFinish(int userid, int? branchid)
        {
            List<ProjectVM> allpro = new List<ProjectVM>();
            var projects = _ProjectRepository.Getprojectalmostfinish(userid, branchid).Result;
            string Today = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            foreach (var pro in projects)
            {
                var diff = (DateTime.ParseExact(pro.FirstProjectExpireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(pro.FirstProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays;
                var diff2 = (DateTime.ParseExact(Today, "yyyy-MM-dd", CultureInfo.InvariantCulture) - DateTime.ParseExact(pro.FirstProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).TotalDays;
                var percentage = (diff2 / diff) * 100;
                if (percentage >= 70 && percentage <= 100)
                {
                    allpro.Add(pro);
                }
            }
            return allpro;
        }
        public async Task<IEnumerable<ProjectVM>> GetProjectsWithoutContractVM()
        {
            var projects = await _ProjectRepository.GetProjectsWithoutContractVM();
            return projects;
        }
        public async Task<IEnumerable<ProjectVM>> GetProjectsLateVM()
        {
            string Today = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var projects = await _ProjectRepository.GetProjectsLateVM(Today);
            return projects;
        }

        public async Task<IEnumerable<ProjectVM>> GetMaxCosEManagerName()
        {
            return await _ProjectRepository.GetMaxCosEManagerName();
        }
        public IEnumerable<object> GetMaxCosEManagerName_StatmentTOP1(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                ManagerName = row[0].ToString(),
                ImgUrl = row[1].ToString(),
                CostES = row[2].ToString(),
            });
        }
        public IEnumerable<object> GetMaxCosECustomerName_StatmentTOP1(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {

                CustomerName = row[0].ToString(),
                ImgUrl = row[1].ToString(),
                CostES = row[2].ToString(),
            });
        }
        public IEnumerable<object> GetMaxCosEManagerName_Statment(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                ProjectNo = row[0].ToString(),
                ManagerName = row[1].ToString(),
                CostES = row[2].ToString(),
            });
        }


        public IEnumerable<object> GetViewDetailsGrid(string Con, string SelectStetment)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlDataAdapter da = new SqlDataAdapter(SelectStetment, Con);
            da.SelectCommand.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                InvoiceNumber = row[0].ToString(),
                NameAr = row[1].ToString(),
                Credit = row[2].ToString(),
                Depit = row[3].ToString(),
                Diff = row[4].ToString(),

            });
        }



        //public GeneralMessage UpdateImportant(int projectId,int important, int UserId, int BranchId)
        //{
        //    try
        //    {

        //        var project = _ProjectRepository.GetById(projectId);
        //        //project.IsImportant = important;
        //        project.UpdatedDate = DateTime.Now;
        //        project.UpdateUser = UserId;



        //        _TaamerProContext.SaveChanges();
        //        //-----------------------------------------------------------------------------------------------------------------
        //        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //        string ActionNote = "تغيير اهمية المشروع ";
        //         _SystemAction.SaveAction("UpdateImportant", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
        //        //-----------------------------------------------------------------------------------------------------------------
        //        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم تغيير الاهمية بنجاح" };
        //    }
        //    catch (Exception)
        //    {
        //        //-----------------------------------------------------------------------------------------------------------------
        //        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //        string ActionNote = "فشل في تغيير اهمية المشروع";
        //         _SystemAction.SaveAction("UpdateImportant", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
        //        //-----------------------------------------------------------------------------------------------------------------
        //        return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في تغيير اهمية المشروع" };
        //    }
        //}

        private bool SendMailNoti(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
        {

            try
            {
                string Email = _usersRepository.GetById(ToUserID).Email ?? "";

                if (Email != "")
                {
                    var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    string textBody = Desc;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.Password);
                    if (_EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    //mail.Body = textBody;

                    try
                    {
                        mail.Body = textBody;
                        mail.IsBodyHtml = true;
                    }
                    catch (Exception)
                    {
                        mail.Body = "Wrong message";
                    }
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Result.Port);

                    smtpClient.Send(mail);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception wx)
            {
                return false;
            }
        }
        private bool SendMailNoti2(int ProjectID, Project proj, string Subject, int BranchId, int UserId, int ToUserID)
        {

            try
            {
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                var customer = _CustomerRepository.GetById(proj.CustomerId ?? 0);

                string Email = _usersRepository.GetById(ToUserID).Email ?? "";

                if (Email != "")
                {
                    var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    // string textBody = Desc;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.Password);
                    if (_EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;

                    string Desc = "السد /ة " + _usersRepository.GetById(proj.MangerId ?? 0).FullName + " المحترم ";
                    Desc = Desc + "<br/>";
                    Desc = Desc + "السلام عليكم ورحمة الله وبركاتة ";
                    Desc = Desc + "<br/>";
                    Desc = Desc + "<h2>تم انهاء المشروع المبين في الجدول ادناة وتم نقلة الي قسم الارشيف </h2>";

                    Desc = Desc + "<table border='1'style='text-align:center;padding:10px;'><tr><th style='border=1px solid #eee'>رقم المشروع  </th><th>اسم العميل  </th><th>التاريخ </th><th>الساعة </th><th>بواسطة </th><th>الفرع </th></tr><tr><td>" + proj.ProjectNo + "</td><td>" + customer.CustomerNameAr + "</td><td>" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "</td><td>" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "</td><td>" + _usersRepository.GetById(proj.MangerId ?? 0).FullName + "</td><td>" + _BranchesRepository.GetById(BranchId).NameAr + "</td></tr></table>";
                    Desc = Desc + "<h4>مع تحيات قسم ادارة المشاريع </h4>";
                    Desc = Desc + "<br/>";
                    Desc = Desc + "تم ارسال هذه الرسالة بشكل آلي، الرجاء عدم الرد عليها";
                    Desc = Desc + "<br/>";
                    Desc = Desc + "-----------------------------------";
                    Desc = Desc + "<br/>";
                    Desc = Desc + "Disclaimer: This message and its attachment, if any, are confidential and may contain legally privileged information. If you are not the intended recipient, please contact the sender immediately and delete this message and its attachment, if any, from your system. You should not copy this message or disclose its contents to any other person or use it for any purpose. Statements and opinions expressed in this e-mail are those of the sender, and do not necessarily reflect those of bayanatech for IT services accepts no liability for damage caused by any virus transmitted by this email";
                    Desc = Desc + "<br/>";
                    Desc = Desc + "هذه الرسالة و مرفقاتها (إن وجدت) تمثل وثيقة سرية قد تحتوي على معلومات تتمتع بحماية وحصانة قانونية. إذا لم تكن الشخص المعني بهذه الرسالة يجب عليك تنبيه المُرسل بخطأ وصولها إليك، و حذف الرسالة و مرفقاتها (إن وجدت) من الحاسب الآلي الخاص بك. ولا يجوز لك نسخ هذه الرسالة أو مرفقاتها (إن وجدت) أو أي جزئ منها، أو البوح بمحتوياتها لأي شخص أو استعمالها لأي غرض. علماً بأن الإفادات و الآراء التي تحويها هذه الرسالة تعبر فقط عميل برنامج تعمير السحابي ،  و ليس بالضرورة رأي مؤسسة بياناتك لتقنية المعلومات ، ولا تتحمل مؤسسة بياناتك لتقنية المعلومات أي مسئولية عن الأضرار الناتجة عن أي فيروسات قد يحملها هذا البريد";
                    Desc = Desc + "<br/>";


                    try
                    {
                        mail.Body = Desc;
                        mail.IsBodyHtml = true;
                    }
                    catch (Exception)
                    {
                        mail.Body = "Wrong message";
                    }
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Result.Port);

                    smtpClient.Send(mail);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception wx)
            {
                return false;
            }
        }




        private bool SendMailCustomer(string Email, string Desc, string Subject, int BranchId)
        {

            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                string textBody = Desc;
                var mail = new MailMessage();
                //var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.Password);
                var loginInfo = new NetworkCredential(_TaamerProContext.Organizations.Where(x => x.OrganizationId == branch).FirstOrDefault()?.Email ?? "", _TaamerProContext.Organizations.Where(x => x.OrganizationId == branch).FirstOrDefault()?.Password ?? "");

                //if (_EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName != null)
                //{
                //    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch)?.Result?.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Result.DisplayName);
                //}
                //else
                //{
                //    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch)?.Result?.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                //}
                var displaynm = "لديك اشعار من نظام تعمير السحابي";

                mail.From = new MailAddress(_TaamerProContext.Organizations.Where(x => x.OrganizationId == branch).FirstOrDefault().Email, displaynm);

                mail.To.Add(new MailAddress(Email));
                mail.Subject = Subject;
                try
                {
                    mail.Body = textBody;
                    mail.IsBodyHtml = false;
                }
                catch (Exception)
                {
                    mail.Body = "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Result.Port);

                smtpClient.Send(mail);
                return true;

            }
            catch (Exception wx)
            {
                return false;
            }
        }


        public bool SendMail_ProjectStamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var org = _OrganizationsRepository.GetById(branch);


                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

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
                    title = "تم ايقاف المشروع المبين تفاصيلة في الجدول التالي";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }
                else if (type == 2)
                {
                    title = "تم تشغيل المشروع المبين تفاصيلة في الجدول التالي";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }

                else if (type == 3)
                {
                    title = " تم انهاء المشروع المبين تفاصيلة في الجدول التالي وتحويلة الي قسم الارشيف";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }
                else if (type == 4)
                {
                    title = "تم تحويل المشروع من الارشيف الي جاري بتاريخ" + formattedDate;
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }

                else if (type == 5)
                {
                    title = "تم اضافتك مدير مشروع لمشروع جديد";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }
                else if (type == 6)
                {
                    title = "اصدار فاتورة أثناء انشاء المشروع";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                mail.To.Add(new MailAddress(_usersRepository.GetById(ReceivedUser).Email));


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


        public bool SendGeneralCustomerEmailandSMS(Customer customerObj, Project projectObj, int TypeId, int UserId, int BranchId, int whichClickDesc)
        {
            try
            {

                //var customerObj = _CustomerRepository.GetById(CustomerId);

                if (customerObj != null)
                {
                    //var projectObj = _ProjectRepository.GetById(ProjectId);

                    var Desc = "";
                    var Subject = "";
                    if (whichClickDesc == 1) //انشاء مشروع
                    {
                        Desc = "تم انشاء مشروع رقم" + " " + projectObj.ProjectNo + " " + "للعميل" + " " + customerObj.CustomerNameAr;
                        Subject = "إشعار بإنشاء المشروع";
                    }
                    else if (whichClickDesc == 2) //ايقاف مؤقت للمشروع
                    {
                        Desc = "تم ايقاف مؤقت للمشروع رقم" + " " + projectObj.ProjectNo + " " + "للعميل" + " " + customerObj.CustomerNameAr;
                        Subject = "إشعار بايقاف مؤقت للمشروع";
                    }
                    else if (whichClickDesc == 3) //تشغيل المشروع 
                    {
                        Desc = "تم تشغيل المشروع رقم" + " " + projectObj.ProjectNo + " " + "للعميل" + " " + customerObj.CustomerNameAr;
                        Subject = "إشعار بتشغيل المشروع";
                    }
                    else if (whichClickDesc == 4) //انهاء المشروع
                    {
                        Desc = "تم انهاء المشروع رقم" + " " + projectObj.ProjectNo + " " + "للعميل" + " " + customerObj.CustomerNameAr;
                        Subject = "إشعار بانهاء المشروع";
                    }
                    if (TypeId == 1 || TypeId == 3)
                    {
                        if (customerObj.CustomerEmail != null && customerObj.CustomerEmail != "")
                        {
                            try
                            {
                                SendMailCustomer(customerObj.CustomerEmail, Desc, Subject, BranchId);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    if (TypeId == 2 || TypeId == 3)
                    {
                        if (customerObj.CustomerMobile != null && customerObj.CustomerMobile != "")
                        {
                            try
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(customerObj.CustomerMobile, Desc, UserId, BranchId);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
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

        public Task<List<ProjectVM>> GetAllProjectsNew(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int FilterType, int? BranchId)
        {
            var projects = _ProjectRepository.GetAllProjectsNew(Con, _project, UserId, AllUserBranchId, FilterType, BranchId);
            return projects;
        }



        public Task<List<ProjectVM>> GetAllProjectsNew_DashBoard(string Con, ProjectVM _project, int? UserId, int AllUserBranchId, int? BranchId)
        {
            var projects = _ProjectRepository.GetAllProjectsNew_DashBoard(Con, _project, UserId, AllUserBranchId, BranchId);
            return projects;
        }

        public Task<IEnumerable<ProjectVM>> GetprojectNewTasks(int UserId, int BranchId, string Lang)
        {
            var projects = _ProjectRepository.GetprojectNewTasks(UserId, BranchId, Lang);
            return projects;
        }
        public Task<IEnumerable<ProjectVM>> GetprojectLateTasks(int UserId, int BranchId, string Lang)
        {
            var projects = _ProjectRepository.GetprojectLateTasks(UserId, BranchId, Lang);
            return projects;
        }


        public Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang)
        {
            var projects = _ProjectRepository.GetprojectNewWorkOrder(UserId, BranchId, Lang);
            return projects;
        }

        public Task<IEnumerable<ProjectVM>> GetprojectNewWorkOrder(int UserId, int BranchId, string Lang, string EndDate)
        {
            var projects = _ProjectRepository.GetprojectNewWorkOrder(UserId, BranchId, Lang, EndDate);
            return projects;
        }
        public rptProjectStatus GetTaskData(int projectId, string con)
        {
            var pro = _ProjectRepository.GetTaskData(projectId, con);
            return pro;

        }

        public rptProjectStatus_phases GetTaskData_phases(int projectId, string con)
        {
            var pro = _ProjectRepository.GetTaskData_phases(projectId, con);
            return pro;

        }

        public GeneralMessage SaveProjectLocation(ProjectLocationVM locationVM)
        {
            try
            {

                var project = _TaamerProContext.Project.Where(x => x.ProjectId == locationVM.ProjectId).FirstOrDefault();
                if (project != null)
                {

                    project.Latitude = locationVM.Latitude;
                    project.Longitude = locationVM.Longitude;
                    project.xmax = locationVM.xmax;
                    project.xmin = locationVM.xmin;
                    project.ymax = locationVM.ymax;
                    project.ymin = locationVM.ymin;
                    _TaamerProContext.SaveChanges();
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "المشروع غير موجود" };

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحفظ" };


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public GeneralMessage deleteProjectLocation(int projectId)
        {
            try
            {

                var project = _TaamerProContext.Project.Where(x => x.ProjectId == projectId).FirstOrDefault();
                if (project != null)
                {

                    project.Latitude = null;
                    project.Longitude = null;
                    project.xmax = null;
                    project.xmin = null;
                    project.ymax = null;
                    project.ymin = null;
                    _TaamerProContext.SaveChanges();
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "المشروع غير موجود" };

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الحذف" };


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ProjectLocationVM GetProjectLocation(int ProjectId)
        {
            var loc = _ProjectRepository.GetProjectLocation(ProjectId);
            return loc;
        }

        public (List<int> Users, string Description) GetNotificationRecipients(NotificationCode code, int projectId)
        {
            var usersnote = new List<int>();

            var config = _TaamerProContext.NotificationConfigurations.Include(x => x.NotificationConfigurationsAssines)
                .FirstOrDefault(x => x.ConfigurationId == (int)code);

            if (config == null || config.To == 0)
                return (usersnote,config.Description);

            var proj = _TaamerProContext.Project.Include(X=>X.ProjectWorkers)
                .Include(x=>x.ProjectPhasesTasks).FirstOrDefault(p => p.ProjectId == projectId);
            if (proj == null)
                return (usersnote, config.Description);

            if (config.To == null || config.To == 0)
                return (usersnote, config.Description);
            var to = (Beneficiary_type)config.To;


            switch (to)
            {
                case Beneficiary_type.مستخدمين:
                    if(config.NotificationConfigurationsAssines !=null && config.NotificationConfigurationsAssines.Count()>0)
                        usersnote.AddRange(
                       config.NotificationConfigurationsAssines
                           .Where(x => x.UserId.HasValue)
                           .Select(x => x.UserId.Value)
                           .ToList()
                   );
                    break;

                case Beneficiary_type.مدير_المشروع:
                    if (proj.MangerId.HasValue)
                        usersnote.Add(proj.MangerId.Value);
                    break;

                case Beneficiary_type.مدير_المشروع_و_المحاسب:
                    if (proj.MangerId.HasValue)
                        usersnote.Add(proj.MangerId.Value);
                    break;

                case Beneficiary_type.مدير_المشروع_و_المشاركين:
                    if (proj.MangerId.HasValue)
                        usersnote.Add(proj.MangerId.Value);
                    if (proj.ProjectWorkers != null && proj.ProjectWorkers.Count() > 0)
                    usersnote.AddRange(proj.ProjectWorkers
.Where(x => x.UserId.HasValue)
.Select(x => x.UserId.Value)
.ToList());
                    break;

                case Beneficiary_type.مدير_المشروع_و_المشاركين_و_كل_من_لديه_مهمة:
                    if (proj.MangerId.HasValue)
                        usersnote.Add(proj.MangerId.Value);
                    if (proj.ProjectWorkers != null && proj.ProjectWorkers.Count() > 0)
                    usersnote.AddRange(proj.ProjectWorkers
     .Where(x => x.UserId.HasValue)
     .Select(x => x.UserId.Value)
     .ToList());
                    if (proj.ProjectPhasesTasks != null && proj.ProjectPhasesTasks.Count() > 0)
                    usersnote.AddRange(proj.ProjectPhasesTasks
     .Where(x => x.UserId.HasValue)
     .Select(x => x.UserId.Value)
     .ToList());
                    break;
                case Beneficiary_type.مشاركين:
                    if (proj.ProjectWorkers != null && proj.ProjectWorkers.Count() > 0)
                        usersnote.AddRange(proj.ProjectWorkers
                        .Where(x => x.UserId.HasValue)
                        .Select(x => x.UserId.Value));
                    break;
                default:
                    if (proj.MangerId.HasValue)
                        usersnote.Add(proj.MangerId.Value);
                    break;



            }

            return (usersnote.Distinct().ToList(), config.Description ); // remove duplicates
        }

    }
}
