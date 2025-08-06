using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Generic;
using System.Net.Mail;
using System.Net.Mime;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ProUserPrivilegesService :   IProUserPrivilegesService
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly IProUserPrivilegesRepository _ProUserPrivilegesRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IVacationRepository _VacationRepository;
        private readonly IProjectWorkersRepository _ProjectWorkersRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IOrganizationsRepository _organizationsRepository;
        private readonly INotificationService _notificationService;

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ProUserPrivilegesService(IUsersRepository usersRepository, IProUserPrivilegesRepository proUserPrivilegesRepository,
             INotificationRepository notificationRepository, IEmailSettingRepository emailSettingRepository,
             IBranchesRepository branchesRepository, IProjectRepository projectRepository, ICustomerRepository customerRepository,
             IVacationRepository vacationRepository,
             IProjectWorkersRepository projectWorkersRepository,
             IUserNotificationPrivilegesService userNotificationPrivilegesService,
            IOrganizationsRepository organizationsRepository,
            INotificationService notificationService,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _UsersRepository = usersRepository;
            _ProUserPrivilegesRepository = proUserPrivilegesRepository;
            _NotificationRepository = notificationRepository;
            _EmailSettingRepository = emailSettingRepository;
            _BranchesRepository = branchesRepository;
            _ProjectRepository = projectRepository;
            _CustomerRepository = customerRepository;
            _VacationRepository = vacationRepository;
            _ProjectWorkersRepository = projectWorkersRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _organizationsRepository = organizationsRepository;
            _notificationService = notificationService;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public GeneralMessage SavePrivProList(List<ProUserPrivileges> Privs, int UserId, int BranchId)
        {
            bool checkAddorEdit = false;
            //var UserVacation = _VacationRepository.GetMatching(s => s.IsDeleted == false && s.UserId == ProjectPhasesTasks.UserId && s.VacationStatus != 4).Count();
            //if (UserVacation != 0)
            //{
            //    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.UserVac };
            //}


            var ListOfTaskNotify = new List<Notification>();

            //var customer = _ProjectRepository.GetById(Privs.ProjectID).customer;
            var projectWorkers = new List<ProjectWorkers>();
            var projectno = "";

            Project proj = _ProjectRepository.GetById(Privs[0].ProjectID ?? 0);
            if (proj != null)
            {
                proj.UpdateUser = UserId;
                proj.UpdateDate = DateTime.Now;
            }

            try
            {
                foreach (ProUserPrivileges priv in Privs)
                {
                    //var customer = _CustomerRepository.GetById(priv.CustomerID);
                    //   var customer = _ProjectRepository.GetById(priv.ProjectID).customer;

                    // Project? customer = _TaamerProContext.Project.Where(s => s.ProjectId == priv.ProjectID).FirstOrDefault().customer;
                    projectno = priv.Projectno;

                     var CustomerNameAr = "";
                    Customer? customer = _TaamerProContext.Customer.Where(s => s.CustomerId == priv.CustomerID).FirstOrDefault();
                    if (customer != null)
                    {
                        CustomerNameAr = customer.CustomerNameAr;
                    }


                    // var projectusers = _ProjectWorkersRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == priv.ProjectID && s.WorkerType!=1); /////////// 

                    var projectusers = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && s.ProjectId == priv.ProjectID && s.WorkerType != 1);

                    if (projectusers!=null)
                    {
                        _TaamerProContext.ProjectWorkers.RemoveRange(projectusers);
                    }


                    if (priv.UserPrivId == 0)
                    {

                        projectWorkers.Add(new ProjectWorkers
                        {
                            UserId = priv.UserId,
                            ProjectId = priv.ProjectID,
                            BranchId = BranchId,
                            WorkerType = 2,
                            AddUser = UserId,
                            AddDate = DateTime.Now,
                        });


                        priv.AddUser = UserId;
                        priv.AddDate = DateTime.Now;
                        _TaamerProContext.ProUserPrivileges.Add(priv);

                        var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(priv.UserId ?? 0);
                        if (UserNotifPriv.Result.Count() != 0)
                        {
                            if (UserNotifPriv.Result.Contains(392))
                            {
                                try
                                {
                                    ListOfTaskNotify.Add(new Notification
                                    {
                                        ReceiveUserId = priv.UserId,
                                        Name = "صلاحيات مشروع",
                                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                        HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                        SendUserId = 1,
                                        Type = 1, // notification
                                        Description = "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + customer.CustomerNameAr,
                                        AllUsers = false,
                                        SendDate = DateTime.Now,
                                        ProjectId = priv.ProjectID,
                                        TaskId = 0,
                                        AddUser = UserId,
                                        AddDate = DateTime.Now,
                                        IsHidden = false
                                    });
                                    _notificationService.sendmobilenotification((int)priv.UserId, "صلاحيات مشروع", "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + customer.CustomerNameAr);
                                }
                                catch (Exception)
                                {

                                }


                            }


                            if (UserNotifPriv.Result.Contains(393))
                            {
                                try
                                {
                                    //var userObj = _UsersRepository.GetById(priv.UserId);
                                    Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == priv.UserId).FirstOrDefault();

                                    var NotStr = customer.CustomerNameAr + " للعميل  " + priv.Projectno + " تم اضافتك علي مشروع رقم  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }
                                }
                                catch (Exception)
                                {

                                }

                            }
                        }

                        checkAddorEdit = true;
                        //SendMail(priv, BranchId, UserId);

                    }
                    else
                    {
                        //var PrivsUpdated = _ProUserPrivilegesRepository.GetById(priv.UserPrivId);
                        ProUserPrivileges? PrivsUpdated = _TaamerProContext.ProUserPrivileges.Where(s => s.UserPrivId == priv.UserPrivId).FirstOrDefault();
                        if (PrivsUpdated != null)
                        {


                            projectWorkers.Add(new ProjectWorkers
                            {
                                UserId = priv.UserId,
                                ProjectId = priv.ProjectID,
                                BranchId = BranchId,
                                WorkerType = 2,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                            });
                            PrivsUpdated.UserId = priv.UserId;
                            PrivsUpdated.Projectno = priv.Projectno;
                            PrivsUpdated.PrivilegeId = priv.PrivilegeId;
                            PrivsUpdated.Select = priv.Select;
                            PrivsUpdated.Insert = priv.Insert;
                            PrivsUpdated.Update = priv.Update;
                            PrivsUpdated.Delete = priv.Delete;
                            PrivsUpdated.UpdateDate = DateTime.Now;

                            checkAddorEdit = false;
                        }
                    }
                    //notification
                }
                _TaamerProContext.ProjectWorkers.AddRange(projectWorkers); // add project users

                if (checkAddorEdit ==true)
                {
                    if (ListOfTaskNotify.Count > 0)
                    {
                        _TaamerProContext. Notification.AddRange(ListOfTaskNotify);
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صلاحيات مستخدم على مشروع جديد" + projectno;
                    _SystemAction.SaveAction("SavePrivProList", "ProUserPrivilegesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                else
                {
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل صلاحيات مستخدم على مشروع  " + projectno;
                    _SystemAction.SaveAction("SavePrivProList", "ProUserPrivilegesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات مستخدم على مشروع";
                _SystemAction.SaveAction("SavePrivProList", "ProUserPrivilegesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                }

        }

        private bool SendMail(ProUserPrivileges PrivObj, int BranchId, int UserId)
        {
            try
            {
                //PrivObj.AddDate.Value
               DateTime date = new DateTime();
                // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                int OrganizationId;
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                if (branch != null)
                {
                    OrganizationId = branch.OrganizationId;
                }

                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //var str = date.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
               // var project = _ProjectRepository.GetById(PrivObj.ProjectID);

                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == PrivObj.ProjectID).FirstOrDefault();
                // var fullname = _UsersRepository.GetById(PrivObj.UserId).FullName;


                string fullName;
                Users? fullname = _TaamerProContext.Users.Where(s => s.UserId == PrivObj.UserId).FirstOrDefault();
                if (fullname != null)
                {
                    fullName = fullname.FullName;
                }

                string textBody = "Dear Mr." + fullname + ", you have  new Privileges in Date " + DateOfTask + "on Project number " + PrivObj.Projectno +" Client : "+ project.customer.CustomerNameAr;

                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }
               // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
               // mail.To.Add(new MailAddress(_UsersRepository.GetById(PrivObj.UserId).Email));

                var receivedUser = _TaamerProContext.Users.Where(s => s.UserId == PrivObj.UserId).FirstOrDefault();
                mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));



                mail.Subject = "New Privileges";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = "you have  new Privileges on project " + PrivObj.Projectno ?? "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                return false;
            }
        }
        private bool SendMailNoti(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
        {

            try
            {
                // string Email = _UsersRepository.GetById(ToUserID).Email ?? "";
                string Email = _TaamerProContext.Users.Where(s => s.UserId == ToUserID)?.FirstOrDefault()?.Email ?? "";
                if (Email != "")
                {
                    // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    int OrganizationId;
                    Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                    if (branch != null)
                    {
                        OrganizationId = branch.OrganizationId;
                    }
                    string textBody = Desc;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }


                   // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    mail.Body = textBody;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

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


        private bool SendMailNoti1(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID,string fulname,string prono,string customer)
        {
            var Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));

            try
            {
                //string Email = _UsersRepository.GetById(ToUserID).Email ?? "";
                string Email = _TaamerProContext.Users.Where(s => s.UserId == ToUserID)?.FirstOrDefault()?.Email ?? "";
                if (Email != "")
                {
                    // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    int OrganizationId;
                    Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                    if (branch != null)
                    {
                        OrganizationId = branch.OrganizationId;
                    }
                    string textBody = Desc;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }

                    string htmlBody = "";
                    htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                                 <p>   السيد /ة     "
                               + fulname +
                                  @"  المحترم   " +
                                    @"<br/>" +
                                    @" السلام عليكم ورحمة الله وبركاتة"
                                + @"</p><p>:تم منحك صلاحية جديدة علي مشروع</p>
                                           <p>  رقم المشروع     "
                               + prono +

                                   @"</p>
                                         <p>   العميل     "
                               + customer +

                                   @"</p>

                                         < p > تاريخ العملية     "
                               + Date1 +

                                   @"</p>

                                < p > مع تحيات قسم ادارة المشاريع      </p>

                                            </html>";

                    // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    mail.Body = htmlBody;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

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



        public bool SendMailNoti12(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, bool IsBodyHtml = false)
        {
            try
            {
                //var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                int OrganizationId;
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                if (branch != null)
                {
                    OrganizationId = branch.OrganizationId;
                }


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
                //mail.To.Add(new MailAddress(_UsersRepository.GetById(ReceivedUser).Email));
                var receivedUser = _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));

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
        public Task<IEnumerable<ProUserPrivilegesVM>> GetAllPriv(string SearchText, string Projectno, int BranchId)
        {
            var Privs = _ProUserPrivilegesRepository.GetAllPriv(SearchText, Projectno, BranchId);
            return Privs;
        }
        public Task<IEnumerable<ProUserPrivilegesVM>> GetAllPrivUser(int UserId, int projectId)
        {
            var Privs = _ProUserPrivilegesRepository.GetAllPrivUser(UserId, projectId);
            return Privs;
        }
        public GeneralMessage DeletePriv(int PrivID, int UserId, int BranchId)
        {
            try
            {
                //ProUserPrivileges priv = _ProUserPrivilegesRepository.GetById(PrivID);
                
                ProUserPrivileges? priv = _TaamerProContext.ProUserPrivileges.Where(s => s.UserPrivId == PrivID).FirstOrDefault();
                if (priv != null) {
                    Project proj = _ProjectRepository.GetById(priv.ProjectID ?? 0);
                    if (proj != null)
                    {
                        proj.UpdateUser = UserId;
                        proj.UpdateDate = DateTime.Now;
                    }
                    priv.IsDeleted = true;
                    priv.DeleteDate = DateTime.Now;
                    priv.DeleteUser = UserId;

                    // var projectusers2 = _ProjectWorkersRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == priv.ProjectID && s.WorkerType != 1 && s.UserId == priv.UserId).FirstOrDefault(); /////////// 

                    var projectusers2 = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && s.ProjectId == priv.ProjectID && s.WorkerType != 1 && s.UserId == priv.UserId).FirstOrDefault();

                    if (projectusers2 != null)
                    {
                        projectusers2.IsDeleted = true;
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف صلاحيات مشروع رقم " + PrivID;
                    _SystemAction.SaveAction("DeletePriv", "ProUserPrivilegesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
                }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف صلاحيات مشروع رقم " + PrivID; ;
                _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
                }
        }

        public GeneralMessage SavePriv2(ProUserPrivileges Privs, int UserId, int BranchId, string Url, string ImgUrl)
        {
            var Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
            var ListOfTaskNotify = new List<Notification>();
            var CustomerNameAr = "";
            Customer? customer = _TaamerProContext.Customer.Where(s => s.CustomerId == Privs.CustomerID).FirstOrDefault();
            Project proj = _ProjectRepository.GetById(Privs.ProjectID??0);
            if (proj != null) {
                proj.UpdateUser = UserId;
                proj.UpdateDate = DateTime.Now;
            }


            if (customer != null)
            {
                CustomerNameAr = customer.CustomerNameAr;
            }
            //var customer = _CustomerRepository.GetById(Privs.Projects.CustomerId);
            // var user = _UsersRepository.GetById(Privs.UserId);
            Users? user = _TaamerProContext.Users.Where(s => s.UserId == Privs.UserId).FirstOrDefault();
            var projectWorkers = new List<ProjectWorkers>();


            try
            {
                if (Privs.UserPrivId == 0)
                {
                    Privs.AddUser = UserId;
                    //priv.Select = priv.Select;

                    Privs.AddDate = DateTime.Now;
                    _TaamerProContext.ProUserPrivileges.Add(Privs);

                    //Description = "بتاريخ " + priv.AddDate.ToString() + "  تم اضافتك علي مشروع رقم " + priv.Projectno + " للعميل " + customer.CustomerNameAr ,


                    projectWorkers.Add(new ProjectWorkers
                    {
                        UserId = Privs.UserId,
                        ProjectId = Privs.ProjectID,
                        BranchId = BranchId,
                        WorkerType = 2,
                        AddUser = UserId,
                        AddDate = DateTime.Now,
                    });
                    _TaamerProContext.ProjectWorkers.AddRange(projectWorkers); // add project users


                    var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(Privs.UserId ?? 0);
                    if (UserNotifPriv.Result.Count() != 0)
                    {
                        if (UserNotifPriv.Result.Contains(392))
                        {
                            ListOfTaskNotify.Add(new Notification
                            {
                                ReceiveUserId = Privs.UserId,
                                Name = "صلاحيات مشروع",
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = 1,
                                Type = 1, // notification
                                Description = "بتاريخ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")) + "  تم اضافتك علي مشروع رقم " + Privs.Projectno + " للعميل " + customer.CustomerNameAr,
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = Privs.ProjectID,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false
                            });
                            _notificationService.sendmobilenotification((int)Privs.UserId, "صلاحيات مشروع", "بتاريخ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")) + "  تم اضافتك علي مشروع رقم " + Privs.Projectno + " للعميل " + customer.CustomerNameAr);

                        }



                        try {
                        if (UserNotifPriv.Result.Contains(391))
                        {

                            string htmlBody = "";

                            //htmlBody = @"<!DOCTYPE html>
                            //                <html>
                            //                 <head></head>
                            //                <body  style='direction: rtl;'>
                            //                        <p>   السيد /ة     "
                            //                   + user.FullName +
                            //                      @"  المحترم   " +
                            //                        @"<br/>" +
                            //                        @" السلام عليكم ورحمة الله وبركاتة"
                            //                    + @"</p><p>:تم منحك صلاحية جديدة علي مشروع</p>

                            //                       <p>    رقم المشروع     "
                            //                   + Privs.Projectno +

                            //                        @"<br/>" +
                            //                  @"</p>

                            //                          <p>    العميل     "
                            //                   + customer.CustomerNameAr +

                            //                        @"<br/>" +
                            //                  @"</p>
                                                
                            //                       <p>    تاريخ العملية     "
                            //                   + Date1 +

                            //                        @"<br/>" +
                            //                  @"</p>
                            //        <p>:مع تحيات قسم ادارة المشاريع</p>
                            //                </body>
                            //                </html>";


                                htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'> تاريخ العملية</th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Privs.Projectno + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Date1 + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                //SendMailNoti(projectId, Desc, "ايقاف مشروع", BranchId, UserId, proj.MangerId ?? 0);
                                var Desc = customer.CustomerNameAr + " للعميل  " + Privs.Projectno + " تم اضافتك علي مشروع رقم  ";

                                SendMail_ProjectStamp(BranchId, UserId, Privs.UserId ?? 0, "اضافة صلاحية جديدة", htmlBody, Url, ImgUrl, 1, true);
                           // SendMailNoti12( BranchId, UserId, Privs.UserId ?? 0,"اضافة صلاحية جديدة", htmlBody, true);
                            //SendMailNoti(Privs.ProjectID ?? 0, Desc, "اضافة صلاحية جديدة", BranchId, UserId, Privs.UserId ?? 0);

                        }

                        }
                        catch {
                        
                        }

                        if (UserNotifPriv.Result.Contains(393))
                        {
                            //var userObj = _UsersRepository.GetById(Privs.UserId);
                            Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == Privs.UserId).FirstOrDefault();
                            if (userObj != null)
                            {
                                var NotStr = customer.CustomerNameAr + " للعميل  " + Privs.Projectno + " تم اضافتك علي مشروع رقم  ";
                                if (userObj.Mobile != null && userObj.Mobile != "")
                                {
                                    var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                }
                            }
                        }

                    }
                    if (ListOfTaskNotify.Count > 0)
                    {
                        _TaamerProContext.Notification.AddRange(ListOfTaskNotify);
                    }


                    //SendMail(Privs, BranchId, UserId);


                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صلاحيات مستخدم على مشروع جديد";
                    _SystemAction.SaveAction("SavePriv2", "ProUserPrivilegesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
                else
                {
                   // var PrivsUpdated = _ProUserPrivilegesRepository.GetById(Privs.UserPrivId);
                    ProUserPrivileges? PrivsUpdated = _TaamerProContext.ProUserPrivileges.Where(s => s.UserPrivId == Privs.UserPrivId).FirstOrDefault();
                    if (PrivsUpdated != null)
                    {


                         if (PrivsUpdated.UserId != Privs.UserId)
                        {
                          //  var projectusers2 = _ProjectWorkersRepository.GetMatching(s => s.IsDeleted == false && s.ProjectId == Privs.ProjectID && s.WorkerType != 1 && s.UserId == PrivsUpdated.UserId).FirstOrDefault(); /////////// 
                            var projectusers2 = _TaamerProContext.ProjectWorkers.Where(s => s.IsDeleted == false && s.ProjectId == Privs.ProjectID && s.WorkerType != 1 && s.UserId == PrivsUpdated.UserId).FirstOrDefault();
                            if (projectusers2 != null)
                            {
                                projectusers2.UserId = Privs.UserId;
                            }
                        }

                        PrivsUpdated.UserId = Privs.UserId;
                        PrivsUpdated.Projectno = Privs.Projectno;
                        PrivsUpdated.PrivilegeId = Privs.PrivilegeId;
                        PrivsUpdated.Select = Privs.Select;
                        PrivsUpdated.Insert = Privs.Insert;
                        PrivsUpdated.Update = Privs.Update;
                        PrivsUpdated.Delete = Privs.Delete;
                        PrivsUpdated.UpdateUser = UserId;
                        PrivsUpdated.UpdateDate = DateTime.Now;



                        var UserNotifPriv = _userNotificationPrivilegesService.GetPrivilegesIdsByUserId(Privs.UserId ?? 0);
                        if (UserNotifPriv.Result.Count() != 0)
                        {
                            if (UserNotifPriv.Result.Contains(392))
                            {
                                ListOfTaskNotify.Add(new Notification
                                {
                                    ReceiveUserId = Privs.UserId,
                                    Name = "صلاحيات مشروع",
                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                    SendUserId = 1,
                                    Type = 1, // notification
                                    Description = "  تم اضافتك علي مشروع رقم " + Privs.Projectno + " للعميل " + customer.CustomerNameAr,
                                    AllUsers = false,
                                    SendDate = DateTime.Now,
                                    ProjectId = Privs.ProjectID,
                                    TaskId = 0,
                                    AddUser = UserId,
                                    AddDate = DateTime.Now,
                                    IsHidden = false
                                });
                                _notificationService.sendmobilenotification((int)Privs.UserId, "صلاحيات مشروع", "بتاريخ " + "  تم اضافتك علي مشروع رقم " + Privs.Projectno + " للعميل " + customer.CustomerNameAr);

                            }

                            try {
                            if (UserNotifPriv.Result.Contains(391))
                            {

                                string htmlBody = "";

                                //htmlBody = @"<!DOCTYPE html>
                                //            <html>
                                //             <head></head>
                                //            <body  style='direction: rtl;'>
                                //                    <p>   السيد /ة     "
                                //                   + user.FullName +
                                //                      @"  المحترم   " +
                                //                        @"<br/>" +
                                //                        @" السلام عليكم ورحمة الله وبركاتة"
                                //                    + @"</p><p>:تم منحك صلاحية جديدة علي مشروع</p>

                                //                   <p>    رقم المشروع     "
                                //                   + Privs.Projectno +

                                //                        @"<br/>" +
                                //                  @"</p>

                                //                      <p>    العميل     "
                                //                   + customer.CustomerNameAr +

                                //                        @"<br/>" +
                                //                  @"</p>
                                                
                                //                   <p>    تاريخ العملية     "
                                //                   + Date1 +

                                //                        @"<br/>" +
                                //                  @"</p>
                                //    <p>:مع تحيات قسم ادارة المشاريع</p>
                                //            </body>
                                //            </html>";


                                    htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'> تاريخ العملية</th>
                                                  </tr>
                                                    <tr>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Privs.Projectno + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer.CustomerNameAr + @"</td>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + Date1 + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                    var Desc = customer.CustomerNameAr + " للعميل  " + Privs.Projectno + " تم اضافتك علي مشروع رقم  ";
                                    //SendMailNoti1(Privs.ProjectID ?? 0, Desc, "اضافة صلاحية جديدة", BranchId, UserId, Privs.UserId ?? 0,Privs.Users.FullName,Privs.Projectno,customer.CustomerNameAr);
                                    //SendMailNoti(Privs.ProjectID ?? 0, Desc, "اضافة صلاحية جديدة", BranchId, UserId, Privs.UserId ?? 0);
                                    SendMail_ProjectStamp(BranchId, UserId, Privs.UserId ?? 0, "اضافة صلاحية جديدة", htmlBody, Url, ImgUrl, 1, true);

                                    //SendMailNoti12(BranchId, UserId, Privs.UserId ?? 0, "اضافة صلاحية جديدة", htmlBody, true);

                            }
                            }
                            catch { }


                            if (UserNotifPriv.Result.Contains(393))
                            {
                              //  var userObj = _UsersRepository.GetById(Privs.UserId);
                                Users? userObj = _TaamerProContext.Users.Where(s => s.UserId == Privs.UserId).FirstOrDefault();
                                if (userObj != null)
                                {

                                    var NotStr = customer.CustomerNameAr + " للعميل  " + Privs.Projectno + " تم اضافتك علي مشروع رقم  ";
                                    if (userObj.Mobile != null && userObj.Mobile != "")
                                    {
                                        var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
                                    }

                                }
                            }
                        }
                        if (ListOfTaskNotify.Count > 0)
                        {
                            _TaamerProContext.Notification.AddRange(ListOfTaskNotify);
                        }

                    }

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل صلاحيات مستخدم على مشروع برقم  " + Privs.Projectno;
                    _SystemAction.SaveAction("SavePriv2", "ProUserPrivilegesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                    }
            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صلاحيات مستخدم على مشروع برقم  " + Privs.Projectno;
                _SystemAction.SaveAction("SavePriv2", "ProUserPrivilegesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }

        }



        public bool SendMail_ProjectStamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
                //var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                //var org = _organizationsRepository.GetById(branch);

                int OrganizationId;
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                if (branch != null)
                {
                    OrganizationId = branch.OrganizationId;
                }

                Organizations? org = _TaamerProContext.Organizations.Where(s=>s.OrganizationId == branch.OrganizationId).FirstOrDefault();
                 
                string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

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
                    title = "تم منحك صلاحية جديدة علي مشروع المبين تفاصيلة في الجدول ادناه:";
                    body = PopulateBody(textBody, _UsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }
              

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                //  mail.To.Add(new MailAddress(_UsersRepository.GetById(ReceivedUser).Email));

                var receivedUser = _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));

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

    }
}
