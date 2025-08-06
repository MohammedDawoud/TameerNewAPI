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
using System.Net.Mail;
using System.Net.Mime;
using Twilio.Rest.Api.V2010.Account;
using Twilio;
using Microsoft.EntityFrameworkCore;
using Twilio.Types;
using TaamerP.Service.LocalResources;
//using iTextSharp.text.pdf;
using TaamerProject.Repository.Repositories;
using Twilio.TwiML.Messaging;
using TaamerProject.Models.Enums;

namespace TaamerProject.Service.Services
{
    public class SupervisionsService :  ISupervisionsService
    {
        private readonly ISupervisionsRepository _SupervisionsRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IPro_SupervisionDetailsRepository _Pro_SupervisionDetailsRepository;
        private readonly ISMSSettingsRepository _sMSSettingsRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly INotificationRepository _NotificationRepository;
        private readonly INotificationService _notificationService;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IProjectService _projectservice;
        private readonly IWhatsAppSettingsRepository _whatsAppSettingsRepository;



        public SupervisionsService(ISupervisionsRepository supervisionsRepository,
            IUsersRepository usersRepository,
            IBranchesRepository branchesRepository,
            IEmailSettingRepository emailSettingRepository,
            IProjectRepository projectRepository,
            IOrganizationsRepository _organizationsRepository,
            IPro_SupervisionDetailsRepository pro_SupervisionDetailsRepository,
            ISMSSettingsRepository SMSSettingsRepository,
            ICustomerRepository customerRepository,
            IUserNotificationPrivilegesService userNotificationPrivilegesService,
            INotificationRepository notificationRepository,
            INotificationService notificationService,
            IWhatsAppSettingsRepository whatsAppSettingsRepository,
            ISys_SystemActionsRepository sys_SystemActionsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction,
            IProjectService projectService)
        {
            _SupervisionsRepository = supervisionsRepository;
            _UsersRepository = usersRepository;
            _EmailSettingRepository = emailSettingRepository;
            _BranchesRepository = branchesRepository;
            _ProjectRepository = projectRepository;
            _OrganizationsRepository = _organizationsRepository;
            _Pro_SupervisionDetailsRepository = pro_SupervisionDetailsRepository;
            _sMSSettingsRepository = SMSSettingsRepository;
            _CustomerRepository = customerRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _NotificationRepository = notificationRepository;
            _notificationService = notificationService;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            this._projectservice = projectService;
            _whatsAppSettingsRepository = whatsAppSettingsRepository;

        }
        public Task<IEnumerable<SupervisionsVM>> GetAllSupervisions(int? ProjectId)
        {
            var Supervisions = _SupervisionsRepository.GetAllSupervisions(ProjectId);
            return Supervisions;
        }
        public IEnumerable<Supervisions> GetAllSupervisionsProject(int BranchId)
        {
           // var Supervisions = _SupervisionsRepository.GetMatching(s=>s.IsDeleted==false && s.SuperStatus==1);
            var Supervisions = _TaamerProContext.Supervisions.Where(s => s.IsDeleted == false && s.SuperStatus == 1);
            return Supervisions;
        }

        public Task<IEnumerable<SupervisionsVM>> GetAllBySupervisionId(int SupervisionId)
        {
            var Supervisions = _SupervisionsRepository.GetAllBySupervisionId(SupervisionId);
            return Supervisions;
        }
        public Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserId(int? UserId)
        {
            var Supervisions = _SupervisionsRepository.GetAllSupervisionsByUserId(UserId);
            return Supervisions;
        }
        public Task<IEnumerable<SupervisionsVM>> GetAllSupervisionsByUserIdHome(int? UserId)
        {
            var Supervisions = _SupervisionsRepository.GetAllSupervisionsByUserIdHome(UserId);
            return Supervisions;
        }
         
        public Task<IEnumerable<SupervisionsVM>> GetAllSupervision_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto)
        {
            var Supervisions = _SupervisionsRepository.GetAllSupervisions_Search(ProjectId, UserId,  EmpId,PhaseId,  DateFrom,  Dateto);
            return Supervisions;
        }

        public Task<IEnumerable<SupervisionsVM>> GetAllSupervision_Search(int? ProjectId, int? UserId, int? EmpId, int? PhaseId, string DateFrom, string Dateto,string? SearchText)
        {
            var Supervisions = _SupervisionsRepository.GetAllSupervisions_Search(ProjectId, UserId, EmpId, PhaseId, DateFrom, Dateto, SearchText);
            return Supervisions;
        }

        public GeneralMessage SupervisionAvailability(Supervisions supervisions, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
                var UserVacation = _TaamerProContext.Vacation.AsEnumerable().Where(s => s.IsDeleted == false && s.UserId == supervisions.ReceivedEmpId && s.VacationStatus == 2 && s.DecisionType == 1 && (s.BackToWorkDate == null || (s.BackToWorkDate ?? "") == "")).ToList();
                UserVacation = UserVacation.Where(s =>
                // أو عنده إجازة في نفس وقت المهمة
                ((!(s.StartDate == null || s.StartDate.Equals("")) && !(supervisions.Date == null || supervisions.Date.Equals("")) && DateTime.ParseExact(s.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(supervisions.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                (!(s.EndDate == null || s.EndDate.Equals("")) && !(supervisions.Date == null || supervisions.Date.Equals("")) && DateTime.ParseExact(s.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(supervisions.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                ).ToList();

                if (UserVacation.Count() != 0)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "المستخدم في اجازة لا يمكنك عمل طلعة عليه" };
                }
                if (supervisions.SupervisionId == 0)
                {
                    //var PhaseDetBefore = _Pro_SupervisionDetailsRepository.GetMatching(s => s.SupervisionId == supervisions.SupervisionId).ToList();
                    var PhaseDetBefore = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SupervisionId == supervisions.SupervisionId).ToList();

                    _TaamerProContext.Pro_SupervisionDetails.RemoveRange(PhaseDetBefore);

                    supervisions.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    supervisions.UserId = UserId;
                    supervisions.ReceiveStatus = false;
                    supervisions.AddUser = UserId;
                    supervisions.BranchId = BranchId;
                    supervisions.IsRead = false;
                    supervisions.AddDate = DateTime.Now;

                    _TaamerProContext.Supervisions.Add(supervisions);
                    var ListOfPrivNotify = new List<Notification>();

                   // var project = _ProjectRepository.GetById(supervisions.ProjectId);
                    Project? project =   _TaamerProContext.Project.Where(s => s.ProjectId == supervisions.ProjectId).Include(x=>x.customer).FirstOrDefault();

                    var customer = _TaamerProContext.Customer.Where(x => x.CustomerId == project.CustomerId).FirstOrDefault();

                    //get notification configuration users and description
                    var (usersList, descriptionFromConfig) = _projectservice.GetNotificationRecipients(NotificationCode.Supervision_Assigned, project.ProjectId);
                    var description = "اضافة طلعة اشراف";

                    if (descriptionFromConfig != null && descriptionFromConfig != "")
                        description = descriptionFromConfig;

                    //if no configuration send to emp and manager
                    if (usersList == null || usersList.Count == 0)
                    {
                        var notification = new Notification
                        {
                            ReceiveUserId = supervisions.ReceivedEmpId,
                            Name = description,
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                            HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                            SendUserId = UserId,
                            Type = 1, // notification
                            Description = " أضافة طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + project?.customer?.CustomerNameAr,
                            AllUsers = false,
                            SendDate = DateTime.Now,
                            ProjectId = supervisions.ProjectId,
                            TaskId = 0,
                            AddUser = UserId,
                            AddDate = DateTime.Now,
                            IsHidden = false,
                            IsDeleted = false,
                        };
                        _TaamerProContext.Notification.Add(notification);
                        _TaamerProContext.SaveChanges();
                        _notificationService.sendmobilenotification(supervisions.ReceivedEmpId ?? 0, description, " أضافة طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + customer?.CustomerNameAr);
                        if (project != null && project.MangerId != null)
                        {
                            Notification mangernotification = new Notification();
                            mangernotification = notification;
                            mangernotification.NotificationId = 0;
                            mangernotification.ReceiveUserId = project.MangerId;
                            _TaamerProContext.Notification.Add(mangernotification);
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(project.MangerId ?? 0, description, " أضافة طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + customer?.CustomerNameAr);
                        }
                        //}

                        //if (UserNotifPriv.Contains(3181))
                        //{
                        var htmlBody = "";
                        var Desc = customer?.CustomerNameAr + " للعميل " + project?.ProjectNo + " أضافة طلعة اشراف علي مشروع رقم  ";
                        htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + project?.ProjectNo + @"</td>

                                                  </tr>
                                                    <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>

                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer?.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                        //SendMailNoti(supervisions.ProjectId ?? 0, Desc, "اضافة طلعة اشراف", BranchId, UserId, supervisions.ReceivedEmpId ?? 0);
                        SendMail_ProjectStamp(BranchId, UserId, supervisions.ReceivedEmpId ?? 0, description, htmlBody, Url, ImgUrl, 1, true);
                        if (project.MangerId != null)
                        {
                            SendMail_ProjectStamp(BranchId, UserId, project.MangerId ?? 0, description, htmlBody, Url, ImgUrl, 1, true);
                        }
                    }
                    else
                    {
                        foreach (var user in usersList)
                        {
                            var notification = new Notification
                            {
                                ReceiveUserId = user,
                                Name = description,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = UserId,
                                Type = 1, // notification
                                Description = " أضافة طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + project?.customer?.CustomerNameAr,
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = supervisions.ProjectId,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false,
                                IsDeleted = false,
                            };
                            _TaamerProContext.Notification.Add(notification);
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(user, description, " أضافة طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + customer?.CustomerNameAr);
                            var htmlBody = "";
                            var Desc = customer?.CustomerNameAr + " للعميل " + project?.ProjectNo + " أضافة طلعة اشراف علي مشروع رقم  ";
                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + project?.ProjectNo + @"</td>

                                                  </tr>
                                                    <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>

                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer?.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            SendMail_ProjectStamp(BranchId, UserId, user, description, htmlBody, Url, ImgUrl, 1, true);


                        }
                    }

                        _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اتاحة طلعة اشراف ";
                    _SystemAction.SaveAction("SupervisionAvailability", "SupervisionsService", 1, "تم اضافة طلعة اشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                   // var SupervisionsUpdated = _SupervisionsRepository.GetById(supervisions.SupervisionId);
                    Supervisions? SupervisionsUpdated = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == supervisions.SupervisionId).FirstOrDefault();
                    if (SupervisionsUpdated != null)
                    {
                        //var PhaseDetBefore = _Pro_SupervisionDetailsRepository.GetMatching(s => s.SupervisionId == supervisions.SupervisionId).ToList();
                        //_Pro_SupervisionDetailsRepository.RemoveRange(PhaseDetBefore.ToList());


                        SupervisionsUpdated.ProjectId = supervisions.ProjectId;
                        SupervisionsUpdated.Date = supervisions.Date;
                        SupervisionsUpdated.ReceivedEmpId = supervisions.ReceivedEmpId;
                        SupervisionsUpdated.PhaseId = supervisions.PhaseId;
                        SupervisionsUpdated.PieceNo = supervisions.PieceNo;
                        SupervisionsUpdated.LicenseNo = supervisions.LicenseNo;
                        SupervisionsUpdated.OutlineNo = supervisions.OutlineNo;
                        SupervisionsUpdated.WorkerId = supervisions.WorkerId;
                        SupervisionsUpdated.VisitDate = supervisions.VisitDate;


                        SupervisionsUpdated.MunicipalSelectId = supervisions.MunicipalSelectId;
                        SupervisionsUpdated.SubMunicipalitySelectId = supervisions.SubMunicipalitySelectId;
                        SupervisionsUpdated.ProBuildingTypeSelectId = supervisions.ProBuildingTypeSelectId;
                        SupervisionsUpdated.DistrictName = supervisions.DistrictName;
                        SupervisionsUpdated.ProBuildingDisc = supervisions.ProBuildingDisc;
                        SupervisionsUpdated.AdwARid = supervisions.AdwARid;
                        SupervisionsUpdated.RequiredServiceId = supervisions.RequiredServiceId;

                        //SupervisionsUpdated.SupervisionDetails = supervisions.SupervisionDetails;
                        SupervisionsUpdated.UpdateUser = UserId;
                        SupervisionsUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل طلعة اشراف ";
                    _SystemAction.SaveAction("SupervisionAvailability", "SupervisionsService", 1, "تم تعديل طلعة اشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_Edit };
                }


            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في إتاحة طلعة الإشراف";
                _SystemAction.SaveAction("SupervisionAvailability", "SupervisionsService", 1, "فشل في اتاحة طلعة اشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        private bool SendMail(Supervisions Supervisionss, int BranchId, int UserId)
        {
            try
            {
                DateTime date = new DateTime();
               // var branch = _BranchesRepository.GetById(BranchId);
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

                //var DateOfTask = ProjectPhasesTasks.AddDate.Value.ToString("yyyy-MM-dd HH:MM");
                var DateOfTask = Supervisionss.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
               // var user = _UsersRepository.GetById(Supervisionss.ReceivedEmpId);

                Users? user = _TaamerProContext.Users.Where(s => s.UserId == Supervisionss.ReceivedEmpId).FirstOrDefault();


               // var userSend = _UsersRepository.GetById(Supervisionss.UserId);

                Users? userSend = _TaamerProContext.Users.Where(s => s.UserId == Supervisionss.UserId).FirstOrDefault();

                //var str = date.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
               // var project = _ProjectRepository.GetById(Supervisionss.ProjectId);

                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == Supervisionss.ProjectId).FirstOrDefault();

                //string textBody = "<table><tr><td>" + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + "</td><td> : لديك مهمه جديدة </td></tr><tr><td>" + ProjectPhasesTasks.project?.ProjectNo + "</td><td> : علي مشروع رقم </td></tr><tr><td>" + ProjectPhasesTasks.project?.customer?.CustomerNameAr + "</td><td> : للعميل </td></tr></table>";
                string textBody = "Dear Mr." + user.FullName + ", you have a new project notification from " + userSend.FullName + " in Date " + DateOfTask + "<br/> " + "<table border='1'style='text-align:center;padding:3px;'><tr><td style='border=1px solid #eee'>Text Message</td><td>" + Supervisionss.ManagerNotes + "</td></tr><tr><td>Project Type</td><td>" + project.projecttype.NameAr + "</td></tr><tr><td>Project No</td><td>" + project?.ProjectNo + "</td></tr><tr><td>Customer name </td><td>" + project?.customer?.CustomerNameAr + "</td></tr></table>";
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.Password);

                if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                }

               // mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(BranchId).SenderEmail);
               // mail.To.Add(new MailAddress(_UsersRepository.GetById(Supervisionss.ReceivedEmpId).Email));

                var receivedUser =   _TaamerProContext.Users.Where(s => s.UserId == Supervisionss.ReceivedEmpId).FirstOrDefault();
                mail.To.Add(new MailAddress(receivedUser?.Email ?? ""));

                //mail.Subject = "لديك مهمة جديده علي مشروع رقم " + project?.ProjectNo + "";
                mail.Subject = "اشعار جديد علي مشروع";
                try
                {
                    mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.project?.ProjectNo + " للعميل " + ProjectPhasesTasks.project?.customer?.CustomerNameAr;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = "";
                }
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(BranchId).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }


        private bool SendMailSupervision(SupervisionsVM Supervisionss,string ToEmail,string ToEmail2, int BranchId, int UserId,string AttachmentFile)
        {
            try
            {
                DateTime date = new DateTime();
                //var branch = _BranchesRepository.GetById(BranchId);
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

                // var Org = _OrganizationsRepository.GetById(branch.OrganizationId);
                Organizations? Org = _TaamerProContext.Organizations.Where(s => s.OrganizationId == branch.OrganizationId).FirstOrDefault();

                var DateOfTask = Supervisionss.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
               // var user = _UsersRepository.GetById(Supervisionss.ReceivedEmpId);
                Users? user = _TaamerProContext.Users.Where(s => s.UserId == Supervisionss.ReceivedEmpId).FirstOrDefault();

                //var userSend = _UsersRepository.GetById(Supervisionss.UserId);
                Users? userSend = _TaamerProContext.Users.Where(s => s.UserId == Supervisionss.UserId).FirstOrDefault();

               // var project = _ProjectRepository.GetById(Supervisionss.ProjectId);
                Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == Supervisionss.ProjectId).FirstOrDefault();
                ProjectType? projecttype = _TaamerProContext.ProjectType.Where(s => s.TypeId == project.ProjectTypeId).FirstOrDefault();
                Customer? customer = _TaamerProContext.Customer.Where(s => s.CustomerId == project.CustomerId).FirstOrDefault();

                string textBody = "Dear Mr." + user.FullName + ", you have a new project notification from " + userSend.FullName + " in Date " + DateOfTask + "<br/> " + "<table border='1'style='text-align:center;padding:3px;'><tr><td style='border=1px solid #eee'>Phase Name</td><td>" + Supervisionss.PhaseName + "</td></tr><tr><td>Project Type</td><td>" + projecttype!.NameAr??"" + "</td></tr><tr><td>Project No</td><td>" + project?.ProjectNo + "</td></tr><tr><td>Customer name </td><td>" + customer!.CustomerNameAr??"" + "</td></tr></table>";
                var mail = new MailMessage();
                
                var displaynm = ""; var SenderEmail = ""; var SenderPassword = ""; var SenderPort = "587"; var SenderHost = "";

                SenderEmail = Org.Email;
                SenderPassword = Org.Password;
                SenderPort = Org.Port;
                SenderHost = Org.Host;
                if (Org.SenderName != null)
                {
                    displaynm = Org.SenderName;

                }
                else
                {
                    displaynm = Org.NameAr;

                }

                var loginInfo = new NetworkCredential(SenderEmail, SenderPassword);
                mail.From = new MailAddress(SenderEmail, displaynm);

                mail.To.Add(new MailAddress(ToEmail));
                if(ToEmail2!="")
                {
                    mail.To.Add(new MailAddress(ToEmail2));
                }
                mail.Subject = "طلعة اشراف";
                try
                {
                    mail.Body = textBody;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = "";
                }

                try
                {
                    if (AttachmentFile != "")
                    {
                        System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(AttachmentFile, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(AttachmentFile);
                        disposition.ModificationDate = File.GetLastWriteTime(AttachmentFile);
                        disposition.ReadDate = File.GetLastAccessTime(AttachmentFile);
                        disposition.FileName = Path.GetFileName(AttachmentFile);
                        disposition.Size = new FileInfo(AttachmentFile).Length;
                        disposition.DispositionType = DispositionTypeNames.Attachment;
                        mail.Attachments.Add(attachment);
                    }
                }
                catch (Exception ex)
                {

                }



                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(SenderHost);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = Convert.ToInt32(SenderPort);
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool SendWhatsAppSupervision(SupervisionsVM Supervisionss, string ToPhone, int BranchId, int UserId, string AttachmentFile)
        {
            var smsSettings = _sMSSettingsRepository.GetsmsSetting(BranchId);
            if (smsSettings != null)
            {
                string apiLink = smsSettings.Result.ApiUrl??"";
                string apiKey = smsSettings.Result.Password??"";
                //string sender = smsSettings.SenderName??"";
                string FromPhone = smsSettings.Result.MobileNo??"";
                //string FromPhone = "14155238886";
                string userName = smsSettings.Result.UserName ?? "";


                if (apiLink=="" || apiKey == "" || FromPhone == "" )
                {
                    return false;
                }
                else
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
                    try
                    {
                        //var accountSid = "ACf1e12c3d2e16e0a9a2b88f1b64ac8a8a";
                        //var authToken = "928deb490ba20156541f12f7c990cb84";
                        string accountSid = Environment.GetEnvironmentVariable(apiLink);
                        string authToken = Environment.GetEnvironmentVariable(apiKey);
                        TwilioClient.Init(accountSid, authToken);

                        var messageOptions = new CreateMessageOptions(
                        //new PhoneNumber("whatsapp:+201020412606"));
                        //new PhoneNumber("whatsapp:+966503326610"));
                        new PhoneNumber("whatsapp:" + "+966" + ToPhone));
                        //messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                        messageOptions.From = new PhoneNumber("whatsapp:" + "+" + FromPhone);

                        messageOptions.Body = "Message From " + userName;
                        //Uri uri = new Uri("https://tameercloud.com/Uploads/Attachment/EmpIdentity_637658432120598472.pdf");
                        //Uri uri2 = new Uri("https://tameercloud.com/Uploads/Supervision/PDFFile_637663897259891763.pdf");
                        Uri uri = new Uri(AttachmentFile);


                        //Uri uri = new Uri(AttachmentFile);
                        messageOptions.MediaUrl.Add(uri);

                        var message = MessageResource.Create(messageOptions);
                        Console.WriteLine(message.Body);
                        GeneralMessage msg = new GeneralMessage();



                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }



        public GeneralMessage DeleteSupervision(int SupervisionId, int UserId, int BranchId)
        {
            try
            {
                //Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);

                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.IsDeleted = true;
                    supervisions.DeleteDate = DateTime.Now;
                    supervisions.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف طلعة إشراف رقم " + SupervisionId;
                    _SystemAction.SaveAction("DeleteSupervision", "SupervisionsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف طلعة إشراف رقم " + SupervisionId; ;
                _SystemAction.SaveAction("DeleteSupervision", "SupervisionsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage SendMSupervision(int SupervisionId, int EmailStatusCustomer, int EmailStatusContractor, int EmailStatusOffice, int UserId, int BranchId,string AttachmentFile)
        {
            try
            {
                var Supervisions = _SupervisionsRepository.GetAllBySupervisionId(SupervisionId).Result.FirstOrDefault();
               
                bool Check = false;

                if (EmailStatusCustomer == 1)
                {
                    if (Supervisions.CustomerEmail != "")
                    {
                        Check = SendMailSupervision(Supervisions, Supervisions.CustomerEmail, "", BranchId, UserId, AttachmentFile);

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                        _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, "فشل في الارسال لا يوجد بريد للعميل", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "فشل في الارسال لا يوجد بريد للعميل" };
                    }

                }

                if (EmailStatusContractor == 1) 
                {
                    if (Supervisions.ContractorEmail != "")
                    {
                        Check = SendMailSupervision(Supervisions, Supervisions.ContractorEmail, "", BranchId, UserId, AttachmentFile);

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                        _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, "فشل في الارسال لا يوجد بريد للمقاول", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "فشل في الارسال لا يوجد بريد للمقاول" };
                    }

                }
                if(EmailStatusOffice == 1) 
                {
                    if (Supervisions.OfficeEmail != "")
                    {
                        Check = SendMailSupervision(Supervisions, Supervisions.OfficeEmail, "", BranchId, UserId, AttachmentFile);

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                        _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, "فشل في الارسال لا يوجد بريد للمكتب المتعاون", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "فشل في الارسال لا يوجد بريد للمكتب المتعاون" };
                    }
                }

                if (Check==true)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " ارسال تقرير زيارة بريد رقم " + SupervisionId;
                    _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, Resources.sent_succesfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.sent_succesfully };
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                    _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, Resources.Not_Send, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.Not_Send };
                }


            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, Resources.Not_Send, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.Not_Send };
            }
        }

        public GeneralMessage SendWSupervision(int SupervisionId, int EmailStatusCustomer, int EmailStatusContractor, int EmailStatusOffice, int UserId, int BranchId, string PDFURL, string environmentURL)
        {
            try
            {
                var Supervisions = _SupervisionsRepository.GetAllBySupervisionId(SupervisionId).Result.FirstOrDefault();
                var Message = "طلعة إشراف";
                bool Check = false;
                var ReceiveNumber = "";
                if (EmailStatusCustomer == 1)
                {
                    if (Supervisions.CustomerPhone != "")
                    {
                        ReceiveNumber = Supervisions.CustomerPhone;

                        //Check = SendWhatsAppSupervision(Supervisions, Supervisions.CustomerPhone, BranchId, UserId, AttachmentFile);

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                        _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, "فشل في الارسال لا يوجد رقم واتس اب للعميل", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.WhatsAppNumberCustomer };
                    }

                }

                if (EmailStatusContractor == 1)
                {
                    if (Supervisions.ContractorPhone != "")
                    {
                        ReceiveNumber = Supervisions.ContractorPhone;

                        //Check = SendWhatsAppSupervision(Supervisions, Supervisions.ContractorPhone, BranchId, UserId, AttachmentFile);

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                        _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, "فشل في الارسال لا يوجد رقم واتس اب للمقاول", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.WhatsAppNumberContractor };
                    }

                }
                if (EmailStatusOffice == 1)
                {
                    if (Supervisions.OfficePhone != "")
                    {
                        ReceiveNumber = Supervisions.OfficePhone;

                        //Check = SendWhatsAppSupervision(Supervisions, Supervisions.OfficePhone, BranchId, UserId, AttachmentFile);

                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                        _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, Resources.WhatsAppNumberCooperatingOffice, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.WhatsAppNumberCooperatingOffice };
                    }
                }

                if (!string.IsNullOrEmpty(ReceiveNumber))
                {
                    try
                    {
                        var whatsAppSettings = _whatsAppSettingsRepository.GetWhatsAppSetting(BranchId).Result;
                        if (whatsAppSettings != null)
                        {
                            SMSProviders Provider = new SMSProviders();
                            GeneralMessage result = null;

                            if (whatsAppSettings.TypeName.Contains("ultramsg"))
                            {
                                result = Provider.SendWhatsApp_UltraMsg(whatsAppSettings.InstanceId, whatsAppSettings.Token, whatsAppSettings.MobileNo, ReceiveNumber, Message, environmentURL, PDFURL);
                            }
                            else if (whatsAppSettings.TypeName.Contains("twilio"))
                            {
                                result = Provider.SendWhatsApp_Twilio(whatsAppSettings.InstanceId, whatsAppSettings.Token, whatsAppSettings.MobileNo, ReceiveNumber, Message, environmentURL, PDFURL);
                            }
                            else
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في إرسال الرسالة (إشعارات)" + ReceiveNumber;
                                _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, Resources.messaging_service_settings, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.messaging_service_settings };
                            }
                            return result;
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال الرسالة (إشعارات)" + ReceiveNumber;
                            _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, Resources.messaging_service_settings, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.messaging_service_settings };
                        }
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote6 = "(إشعارات) فشل في ارسال الرسالة" + ReceiveNumber;
                        _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, Resources.faild_messaging_service_settings, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.messaging_service_settings };
                    }
                }
                else
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.messaging_service_settings };


            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في ارسال تقرير زيارة بريد رقم " + SupervisionId; ;
                _SystemAction.SaveAction("SendMSupervision", "SupervisionsService", 1, Resources.FailedSendCheckSettings, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.FailedSendCheckSettings };
            }

        }

        

        public GeneralMessage ReadSupervision(int SupervisionId, int UserId, int BranchId)
        {
            try
            {
                //Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.IsRead = true;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "قراءة طلعة إشراف رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "تم القراءه بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في قراءة طلعة الإشراف";
                _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "فشل في قراءه طلعه الاشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage ReciveSuper(int SupervisionId, int UserId, int BranchId, string Url, string ImgUrl)
        {
            try
            {
               // Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.SuperStatus = 2;
                    _TaamerProContext.SaveChanges();
                    try
                    {
                        Project? project = _TaamerProContext.Project.Where(s => s.ProjectId == supervisions.ProjectId).Include(x => x.customer).FirstOrDefault();

                        var customer = _TaamerProContext.Customer.Where(x => x.CustomerId == project.CustomerId).FirstOrDefault();
                        var (usersList, descriptionFromConfig) = _projectservice.GetNotificationRecipients(NotificationCode.Supervision_Received, project.ProjectId);
                        var description = "استلام طلعة اشراف";

                        if (descriptionFromConfig != null && descriptionFromConfig != "")
                            description = descriptionFromConfig;

                        //if no configuration send to emp and manager
                        if (usersList == null || usersList.Count == 0)
                        {
                            var notification = new Notification
                            {
                                ReceiveUserId = supervisions.ReceivedEmpId,
                                Name = description,
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = UserId,
                                Type = 1, // notification
                                Description = " استلام طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + project?.customer?.CustomerNameAr,
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = supervisions.ProjectId,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                                IsHidden = false,
                                IsDeleted = false,
                            };
                            _TaamerProContext.Notification.Add(notification);
                            _TaamerProContext.SaveChanges();
                            _notificationService.sendmobilenotification(supervisions.ReceivedEmpId ?? 0, "استلام طلعة اشراف", " استلام طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + customer?.CustomerNameAr);
                            if (project != null && project.MangerId != null)
                            {
                                Notification mangernotification = new Notification();
                                mangernotification = notification;
                                mangernotification.NotificationId = 0;
                                mangernotification.ReceiveUserId = project.MangerId;
                                _TaamerProContext.Notification.Add(mangernotification);
                                _TaamerProContext.SaveChanges();
                                _notificationService.sendmobilenotification(project.MangerId ?? 0, "استلام طلعة اشراف", " استلام طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + customer?.CustomerNameAr);
                            }
                            var htmlBody = "";
                            var Desc = customer?.CustomerNameAr + " للعميل " + project?.ProjectNo + " استلام طلعة اشراف علي مشروع رقم  ";
                            htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + project?.ProjectNo + @"</td>

                                                  </tr>
                                                    <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>

                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer?.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                            SendMail_ProjectStamp(BranchId, UserId, supervisions.ReceivedEmpId ?? 0, description, htmlBody, Url, ImgUrl, 2, true);
                            if (project.MangerId != null)
                            {
                                SendMail_ProjectStamp(BranchId, UserId, project.MangerId ?? 0, description, htmlBody, Url, ImgUrl, 2, true);
                            }
                        }
                        else
                        {
                            foreach (var user in usersList)
                            {
                                var notification = new Notification
                                {
                                    ReceiveUserId = user,
                                    Name = description,
                                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                    HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                    SendUserId = UserId,
                                    Type = 1, // notification
                                    Description = " استلام طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + project?.customer?.CustomerNameAr,
                                    AllUsers = false,
                                    SendDate = DateTime.Now,
                                    ProjectId = supervisions.ProjectId,
                                    TaskId = 0,
                                    AddUser = UserId,
                                    AddDate = DateTime.Now,
                                    IsHidden = false,
                                    IsDeleted = false,
                                };
                                _TaamerProContext.Notification.Add(notification);
                                _TaamerProContext.SaveChanges();
                                _notificationService.sendmobilenotification(user, description, " استلام طلعة اشراف علي مشروع رقم   : " + project?.ProjectNo + " للعميل " + customer?.CustomerNameAr);
                                var htmlBody = "";
                                var Desc = customer?.CustomerNameAr + " للعميل " + project?.ProjectNo + " استلام طلعة اشراف علي مشروع رقم  ";
                                htmlBody = @"<!DOCTYPE html>
                                            <html>
                                             <head></head>
                                            <body  style='direction: rtl;'>
                                             
                                                <table style=' border: 1px solid black; border-collapse: collapse;' align='center'>
                                                  <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>رقم المشروع</th>
                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + project?.ProjectNo + @"</td>

                                                  </tr>
                                                    <tr>
                                                    <th  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>اسم العميل</th>

                                                      <td  style=' border: 1px solid black; border-collapse: collapse;width: 150px;'>" + customer?.CustomerNameAr + @"</td>
                                              
                                                    </tr>
                                                </table>
                                            </body>
                                            </html>";
                                SendMail_ProjectStamp(BranchId, UserId, user, description, htmlBody, Url, ImgUrl, 2, true);

                            }
                        }

                            }
                    catch(Exception ex)
                    {

                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "استلام طلعة إشراف رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ReciveSuper", "SupervisionsService", 2, "تم الاستلام بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في استلام طلعة الإشراف";
                _SystemAction.SaveAction("ReciveSuper", "SupervisionsService", 2, "فشل في استلام طلعه الاشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage OutlineChangeSave(int SupervisionId, string OutlineChangetxt1, string OutlineChangetxt2, string OutlineChangetxt3, int UserId, int BranchId)
        {
            try
            {
               // Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.OutlineChangetxt1 = OutlineChangetxt1;
                    supervisions.OutlineChangetxt2 = OutlineChangetxt2;
                    supervisions.OutlineChangetxt3 = OutlineChangetxt3;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ التغييرات على المخططات رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ReciveSuper", "SupervisionsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ التغييرات على المخططات";
                _SystemAction.SaveAction("ReciveSuper", "SupervisionsService", 2, "فشل في حفظ التغييرات على المخططات", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage PointsNotWrittenSave(int SupervisionId, string PointsNotWrittentxt1, string PointsNotWrittentxt2, string PointsNotWrittentxt3, int UserId, int BranchId)
        {
            try
            {
               // Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.PointsNotWrittentxt1 = PointsNotWrittentxt1;
                    supervisions.PointsNotWrittentxt2 = PointsNotWrittentxt2;
                    supervisions.PointsNotWrittentxt3 = PointsNotWrittentxt3;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ البنود التي لم تذكر في التقرير رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ReciveSuper", "SupervisionsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ البنود التي لم تذكر في التقرير";
                _SystemAction.SaveAction("ReciveSuper", "SupervisionsService", 2, "فشل في حفظ البنود التي لم تذكر في التقرير", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.Save_failed_in_Points_Report };
            }
        }


        public GeneralMessage NotReciveSuper(int SupervisionId, int UserId, int BranchId)
        {
            try
            {
               // Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.SuperStatus = 3;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تأجيل استلام طلعة إشراف رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("NotReciveSuper", "SupervisionsService", 2, "تم تأجيل استلام بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.Make_it_later_successfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تأجيل استلام طلعة الإشراف";
                _SystemAction.SaveAction("NotReciveSuper", "SupervisionsService", 2, "فشل في تأجيل استلام طلعه الاشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage ConfirmSupervision(int SupervisionId, int UserId, int BranchId, int TypeId, int TypeIdAdmin)
        {
            try
            {
                // Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    Project proj = _ProjectRepository.GetById(supervisions.ProjectId??0);
                    var customer = _CustomerRepository.GetById(proj.CustomerId ?? 0);
                    var user = _TaamerProContext.Users.Where(s => s.UserId == supervisions.ReceivedEmpId).FirstOrDefault()!;

                    supervisions.SuperStatus = 1;
                    supervisions.SuperDateConfirm = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    SendGeneralCustomerAndUserEmailandSMS(user,customer, proj, TypeId, UserId, BranchId, TypeIdAdmin);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اتاحة طلعة إشراف رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ConfirmSupervision", "SupervisionsService", 2, "تم الاتاحة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.Can_Do_It_successfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في اتاحة طلعة الإشراف";
                _SystemAction.SaveAction("ConfirmSupervision", "SupervisionsService", 2, "فشل في اتاحة طلعه الاشراف", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed};
            }
        }
        public bool SendGeneralCustomerAndUserEmailandSMS(Users userObj, Customer customerObj, Project projectObj, int TypeId, int UserId, int BranchId, int TypeIdAdmin)
        {
            try
            {
                if (customerObj != null)
                {
                    var Desc = "تمت اتاحة طلعة اشراف لمشروع رقم" + " " + projectObj.ProjectNo + " " + "للعميل" + " " + customerObj.CustomerNameAr;
                    var Subject = "إشعار ب اتاحة طلعة اشراف لمشروع";

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
                    if (TypeIdAdmin == 1 || TypeIdAdmin == 3)
                    {
                        if (userObj.Email != null && userObj.Email != "")
                        {
                            try
                            {
                                SendMailCustomer(userObj.Email, Desc, Subject, BranchId);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    if (TypeIdAdmin == 2 || TypeIdAdmin == 3)
                    {
                        if (userObj.Mobile != null && userObj.Mobile != "")
                        {
                            try
                            {
                                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, Desc, UserId, BranchId);
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
        private bool SendMailCustomer(string Email, string Desc, string Subject, int BranchId)
        {

            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                string textBody = Desc;
                var mail = new MailMessage();
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

        public GeneralMessage UploadHeadImageFir(int SupervisionId, string ImageUrl, int UserId, int BranchId)
        {
            try
            {
                //Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.ImageUrl = ImageUrl;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ صورة رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "تم حفظ صورة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صورة";
                _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "فشل في حفظ صورة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
        }
        public GeneralMessage UploadHeadImageSec(int SupervisionId, string ImageUrl, int UserId, int BranchId)
        {
            try
            {
                //Supervisions supervisions = _SupervisionsRepository.GetById(SupervisionId);
                Supervisions? supervisions = _TaamerProContext.Supervisions.Where(s => s.SupervisionId == SupervisionId).FirstOrDefault();
                if (supervisions != null)
                {
                    supervisions.ImageUrl2 = ImageUrl;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ صورة رقم " + supervisions.SupervisionId;
                    _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "تم حفظ صورة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صورة";
                _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "فشل في حفظ صورة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedSuccessfully };
            }
        }
        public Task<int> GenerateNextSupNumber()
        {
            return _SupervisionsRepository.GenerateNextSupNumber();
        }


        private bool SendMailNoti(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
        {
            try
            {
                //string Email = _UsersRepository.GetById(ToUserID).Email ?? "";
                string Email = _TaamerProContext.Users.Where(s => s.UserId == ToUserID)?.FirstOrDefault()?.Email ?? "";

                if (Email != "")
                {
                    // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    int OrganizationId;
                    Branch? branch =   _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
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
                    //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
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



        public bool SendMail_ProjectStamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
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

                // var org = _OrganizationsRepository.GetById(branch);
                Branch? org = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();

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
                    title = "تم اضافة طلعة اشراف علي المشروع التالي";
                    body = PopulateBody(textBody, _UsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }

                if (type == 2)
                {
                    title = "تم استلام طلعة اشراف علي المشروع التالي";
                    body = PopulateBody(textBody, _UsersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                // mail.To.Add(new MailAddress(_UsersRepository.GetById(ReceivedUser).Email));
                var receivedUser =  _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
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
