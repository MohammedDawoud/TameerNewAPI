using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using System.Net.Mail;
using System.Net.Mime;
using TaamerProject.Repository.Repositories;
using TaamerProject.Models.Enums;

namespace TaamerProject.Service.Services
{
    public class Pro_DestinationsService : IPro_DestinationsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IPro_DestinationsRepository _Pro_DestinationsRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUsersRepository _IUsersRepository;
        private readonly INotificationService _notificationService;
        private readonly IProjectService _projectService;

        public Pro_DestinationsService(IPro_DestinationsRepository pro_DestinationsRepository
            , TaamerProjectContext dataContext, ISystemAction systemAction, IEmailSettingRepository emailSettingRepository, IUsersRepository iUsersRepository, INotificationService notificationService,
            IProjectService projectService)
        {
            _TaamerProContext = dataContext; _SystemAction = systemAction;
            _Pro_DestinationsRepository = pro_DestinationsRepository;
            _EmailSettingRepository = emailSettingRepository;
            _IUsersRepository = iUsersRepository;
            _notificationService = notificationService;
            _projectService = projectService;
        }

        public Task<IEnumerable<Pro_DestinationsVM>> GetAllDestinations(int BranchId, List<int> BranchesList)
        {
            var Destinations = _Pro_DestinationsRepository.GetAllDestinations(BranchId, BranchesList);
            return Destinations;
        }
        public Task<Pro_DestinationsVM> GetDestinationByProjectId(int projectId)
        {
            var Destinations = _Pro_DestinationsRepository.GetDestinationByProjectId(projectId);
            return Destinations;
        }
        public Task<Pro_DestinationsVM> GetDestinationByProjectIdToReplay(int projectId)
        {
            var Destinations = _Pro_DestinationsRepository.GetDestinationByProjectIdToReplay(projectId);
            return Destinations;
        }
        public GeneralMessage SaveDestination(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl)
        {
            try
            {

                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();

                if (settings.DestinationCheckCode == Destination.Checkcode)
                {
                    settings.DestinationCheckCode = null;
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "كود التفعيل غير صحيح" };
                }


                if (Destination.DestinationId==0)
                {
                    Destination.BranchId = BranchId;
                    Destination.Status = 1;
                    Destination.AddUser = UserId;
                    Destination.AddDate = DateTime.Now;

                    var DATE = (Destination.AddFileDate ?? new DateTime()).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    DateTime AccDate = DateTime.ParseExact(DATE + " " + Destination.AddFileDateTime, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                    Destination.AddFileDate = AccDate;


                    _TaamerProContext.Pro_Destinations.Add(Destination);
                    _TaamerProContext.SaveChanges();
                    var Project = _TaamerProContext.Project.Where(s => s.ProjectId == Destination.ProjectId).FirstOrDefault();
                    if (Project != null)
                    {
                        Project.DestinationsUpload = Destination.DestinationId;
                    }
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = "رفع لجهه حكومية";
                    _SystemAction.SaveAction("SaveDestination", "Pro_DestinationsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفع ملف لجهة حكومية";
                _SystemAction.SaveAction("SaveDestination", "Pro_DestinationsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveDestinationNotifi(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl)
        {
            try
            {
                #region Sending email and notifications using configuration

                var Project = _TaamerProContext.Project.FirstOrDefault(s => s.ProjectId == Destination.ProjectId);
                if (Project == null)
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "Project not found" };

                var distinationtype = _TaamerProContext.Pro_DestinationTypes.FirstOrDefault(x => x.DestinationTypeId == Destination.DestinationTypeId);
                var customer = _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == Project.CustomerId);
                var Manager = _TaamerProContext.Users.FirstOrDefault(x => x.UserId == Project.MangerId);
                var branch = _TaamerProContext.Branch.FirstOrDefault(x => x.BranchId == Project.BranchId);

                string subject = "رفع مشروع إلى " + distinationtype?.NameAr;
                string headertxt = "تم رفع المشروع إلى " + distinationtype?.NameAr;
                string notitxt = "تم رفع مشروع رقم " + Project.ProjectNo + " إلى " + distinationtype?.NameAr;

                // HTML body
                string strbody = $@"<!DOCTYPE html><html lang='ar'><head>
        <style>.square {{ height: 35px; width: 35px; background-color: #ffffff; border: ridge; text-align: center; align-content: center; font-size: 28px; }}</style>
        </head><body>                  
        <h3 style='text-align:center;'>{headertxt}</h3>
        <table align='center' border='1'>
            <tr><td>رقم المشروع</td><td>{Project.ProjectNo}</td></tr>
            <tr><td>اسم العميل</td><td>{customer?.CustomerNameAr}</td></tr>
            <tr><td>الفرع</td><td>{branch?.NameAr}</td></tr>
            <tr><td>مدير المشروع</td><td>{Manager?.FullNameAr}</td></tr>
            <tr><td>اسم الجهة الخارجية</td><td>{distinationtype?.NameAr}</td></tr>
        </table>
        <h7>مع تحيات قسم إدارة المشاريع</h7>
        </body></html>";

                var (usersList, descriptionFromConfig) = _projectService.GetNotificationRecipients(NotificationCode.Project_UploadToExternal, Project.ProjectId);

                if (usersList == null || usersList.Count == 0)
                {

                    var listusers = _TaamerProContext.ProjectWorkers.Where(x => !x.IsDeleted && x.ProjectId == Project.ProjectId && x.UserId.HasValue).Select(x => x.UserId.Value).Distinct().ToList();

                    var tasksuser = _TaamerProContext.ProjectPhasesTasks.Where(x => !x.IsDeleted && x.ProjectId == Project.ProjectId && x.Type == 3 && x.Status != 4 && x.UserId.HasValue).Select(x => x.UserId.Value).Distinct().ToList(); 
                    if (listusers != null && listusers.Count()>0)
                        usersList.AddRange(listusers);
                    if (tasksuser !=null && tasksuser.Count()>0)
                        usersList.AddRange(tasksuser);

                }
                if(descriptionFromConfig !=null && descriptionFromConfig != "")
                {
                    subject = descriptionFromConfig;
                }

                foreach (var userId in usersList.Distinct())
                {
                    try
                    {
                        var issent= SendMail_Destination(Organization, branch, UserId, userId, subject, strbody, Url, ImgUrl, 1, true);
                    }
                    catch (Exception ex)
                    {
                        // يمكنك تسجيل الخطأ هنا حسب الحاجة
                    }

                    // حفظ إشعار في قاعدة البيانات
                    _TaamerProContext.Notification.Add(new Notification
                    {
                        ReceiveUserId = userId,
                        Name = subject,
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                        SendUserId = 1,
                        Type = 1,
                        Description = notitxt,
                        AllUsers = false,
                        SendDate = DateTime.Now,
                        ProjectId = Project.ProjectId,
                        TaskId = 0,
                        AddUser = UserId,
                        BranchId = branch?.BranchId ?? 0,
                        AddDate = DateTime.Now,
                        IsHidden = false,
                        NextTime = null,
                    });

                    _TaamerProContext.SaveChanges();

                    // إرسال إشعار موبايل
                    _notificationService.sendmobilenotification(userId, subject, notitxt);
                }

                #endregion

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveDestinationReplay(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl)
        {
            try
            {

                var settings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false).FirstOrDefault();

                if (settings.DestinationCheckCode == Destination.Checkcode)
                {
                    settings.DestinationCheckCode = null;
                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "كود التفعيل غير صحيح", ReturnedParm=0 };
                }


                var DestinationsUpdated = _TaamerProContext.Pro_Destinations.Where(s => s.IsDeleted==false && s.ProjectId == Destination.ProjectId && (s.Status==1 || s.Status == 0 || s.Status == null)).FirstOrDefault();
                if (DestinationsUpdated != null)
                {
                    DestinationsUpdated.ProjectId = Destination.ProjectId;

                    if (Destination.UserIdRec != null)
                        DestinationsUpdated.UserIdRec = Destination.UserIdRec;
                    DestinationsUpdated.CounterRec = (DestinationsUpdated.CounterRec ?? 0) + 1;

                    if (Destination.NotesRec != null)
                        DestinationsUpdated.NotesRec = Destination.NotesRec;
                    if (Destination.Status != null)
                        DestinationsUpdated.Status = Destination.Status??1;
                    DestinationsUpdated.BranchId = BranchId;
                    DestinationsUpdated.UpdateUser = UserId;
                    DestinationsUpdated.UpdateDate = DateTime.Now;


                    if (Destination.AddFileDateRec != null)
                    {
                        var DATE = (Destination.AddFileDateRec ?? new DateTime()).ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        DateTime AccDate = DateTime.ParseExact(DATE + " " + Destination.AddFileDateRecTime, "yyyy-MM-dd h:mm tt", CultureInfo.InvariantCulture);
                        DestinationsUpdated.AddFileDateRec = AccDate;
                    }


                    if (Destination.AddFileDateRec <= DestinationsUpdated.AddFileDate) {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "تأكد من ان يكون تاريخ الاستلام بعد تاريخ الرفع "+ DestinationsUpdated.AddFileDate };
                    }


                    var Proj = _TaamerProContext.Project.Where(s => s.ProjectId == Destination.ProjectId).FirstOrDefault();
                    if (Proj != null)
                    {
                        var Days = Math.Ceiling(((DestinationsUpdated.AddFileDateRec ?? new DateTime()) - (DestinationsUpdated.AddFileDate ?? new DateTime())).TotalDays);
                        //var AccDays = Math.Ceiling(Days);
                        var ProDate = Convert.ToDateTime(Proj.ProjectExpireDate).AddDays(Days);
                        var AccProDate = ProDate.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

                        Proj.ProjectExpireDate = AccProDate;
                        Proj.FirstProjectExpireDate = AccProDate;
                        Proj.DestinationsUpload = null;
                        var ProjectDays = Math.Ceiling((Convert.ToDateTime(Proj.ProjectExpireDate) - Convert.ToDateTime(Proj.ProjectDate)).TotalDays)+1;
                        Proj.NoOfDays = Convert.ToInt32(ProjectDays);

                    }
                    _TaamerProContext.SaveChanges();
                    
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل ملف لجهه حكومية رقم " + DestinationsUpdated!.DestinationId;
                _SystemAction.SaveAction("SaveDestination", "Pro_DestinationsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedParm= DestinationsUpdated!.DestinationId };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفع ملف لجهة حكومية";
                _SystemAction.SaveAction("SaveDestination", "Pro_DestinationsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed, ReturnedParm=0 };
            }
        }

        public GeneralMessage SaveDestinationReplayNotifi(Pro_Destinations Destination, int UserId, int BranchId, OrganizationsVM Organization, string Url, string ImgUrl)
        {
            try
            {
                var DestinationsUpdated = _TaamerProContext.Pro_Destinations.Where(s => s.IsDeleted == false && s.DestinationId == Destination.DestinationId).FirstOrDefault();
                if (DestinationsUpdated != null)
                {

                    var Proj = _TaamerProContext.Project.Where(s => s.ProjectId == Destination.ProjectId).FirstOrDefault();
                   

                    #region إرسال بريد إلكتروني وإشعارات عند الرد من الجهة الخارجية

                    var distinationtype = _TaamerProContext.Pro_DestinationTypes
                        .FirstOrDefault(x => x.DestinationTypeId == DestinationsUpdated.DestinationTypeId);

                    var customer = _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == Proj.CustomerId);
                    var Manager = _TaamerProContext.Users.FirstOrDefault(x => x.UserId == Proj.MangerId);
                    var branch = _TaamerProContext.Branch.FirstOrDefault(x => x.BranchId == Proj.BranchId);

                    var status = DestinationsUpdated.Status == 2 ? "موافقة" : "رفض";

                    var subject = $"رد {distinationtype?.NameAr} على المشروع";
                    var headertxt = $"تم رد {distinationtype?.NameAr}";
                    var notitxt = $"تم رد {distinationtype?.NameAr} بـ {status} على مشروع رقم {Proj.ProjectNo}";

                    // HTML body
                    var strbody = $@"<!DOCTYPE html><html lang='ar'><head>
                                <style>
                                .square {{
                                    height: 35px; width: 35px; background-color: #ffffff; border: ridge;
                                    text-align: center; align-content: center; font-size: 28px;
                                }}
                                </style></head>
                                <body>
                                <h3 style='text-align:center;'>{notitxt}</h3>
                                <table align='center' border='1'>
                                <tr><td>رقم المشروع</td><td>{Proj.ProjectNo}</td></tr>
                                <tr><td>اسم العميل</td><td>{customer?.CustomerNameAr}</td></tr>
                                <tr><td>الفرع</td><td>{branch?.NameAr}</td></tr>
                                <tr><td>مدير المشروع</td><td>{Manager?.FullNameAr}</td></tr>
                                <tr><td>اسم الجهة الخارجية</td><td>{distinationtype?.NameAr}</td></tr>
                                <tr><td>حالة الرد</td><td>{status}</td></tr>
                                </table>
                                <h7>مع تحيات قسم إدارة المشاريع</h7>
                                </body></html>";

                    var (usersList, description) =_projectService.GetNotificationRecipients(NotificationCode.Project_ReplyFromExternal, Proj.ProjectId);

                    if (usersList == null || usersList.Count == 0)
                    {

                        var listusers = _TaamerProContext.ProjectWorkers.Where(x => !x.IsDeleted && x.ProjectId == Proj.ProjectId && x.UserId.HasValue).Select(x => x.UserId.Value).Distinct().ToList();

                        var tasksuser = _TaamerProContext.ProjectPhasesTasks.Where(x => !x.IsDeleted && x.ProjectId == Proj.ProjectId && x.Type == 3 && x.Status != 4 && x.UserId.HasValue).Select(x => x.UserId.Value).Distinct().ToList();
                        if (listusers != null && listusers.Count() > 0)
                            usersList.AddRange(listusers);
                        if (tasksuser != null && tasksuser.Count() > 0)
                            usersList.AddRange(tasksuser);
                    }
                    if (description != null && description != "")
                    {
                        subject = description;
                    }
                    foreach (var userId in usersList.Distinct())
                    {
                        try
                        {
                            var issent =  SendMail_Destination(Organization, branch, userId, userId, subject, strbody, Url, ImgUrl, 1, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"فشل إرسال البريد للمستخدم {userId}: {ex.Message}");
                        }

                        _TaamerProContext.Notification.Add(new Notification
                        {
                            ReceiveUserId = userId,
                            Name = subject,
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                            SendUserId = 1,
                            Type = 1,
                            Description = notitxt,
                            AllUsers = false,
                            SendDate = DateTime.Now,
                            ProjectId = Proj.ProjectId,
                            TaskId = 0,
                            AddUser = UserId,
                            BranchId = branch?.BranchId ?? 0,
                            AddDate = DateTime.Now,
                            IsHidden = false,
                            NextTime = null,
                        });

                        _TaamerProContext.SaveChanges();

                        _notificationService.sendmobilenotification(userId, subject, notitxt);
                    }

                    #endregion


                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteDestination(int DestinationId, int UserId, int BranchId)
        {
            try
            {
                Pro_Destinations? Destination = _TaamerProContext.Pro_Destinations.Where(s => s.DestinationId == DestinationId).FirstOrDefault();
                if (Destination != null)
                {
                    Destination.IsDeleted = true;
                    Destination.DeleteDate = DateTime.Now;
                    Destination.DeleteUser = UserId;

                    var DestinationsUpdated2 = _TaamerProContext.Pro_Destinations.Where(s => s.IsDeleted == false && s.ProjectId == Destination.ProjectId && (s.Status == 1 || s.Status == 0 || s.Status == null)).ToList();
                    var DestinationsUpdated = DestinationsUpdated2.Where(s => s.DestinationId != DestinationId).FirstOrDefault();
                    if (DestinationsUpdated==null)
                    {
                        var proj = _TaamerProContext.Project.Where(s => s.ProjectId == Destination.ProjectId).FirstOrDefault();
                        if (proj != null)
                        {
                            proj.DestinationsUpload = null;
                        }
                    }
                  
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف ملف لجهه حكومية رقم " + DestinationId;
                    _SystemAction.SaveAction("DeleteDestination", "Pro_DestinationsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في رفع ملف لجهه حكومية رقم " + DestinationId; ;
                _SystemAction.SaveAction("DeleteDestination", "Pro_DestinationsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public async Task<bool> SendMail_Destination(OrganizationsVM org, Branch branch, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, bool IsBodyHtml = false)
        {
            try
            {
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
                var title = "لقد طلبت رفع ملف لجهة خارجية";
                var body = "";
                title = "";
                body = PopulateBody(textBody, _IUsersRepository.GetUserById(ReceivedUser, "rtl").Result!.FullName??"", title, "مع تمنياتنا لكم بالتوفيق", Url, org.NameAr);


                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);


                var userReciver = _TaamerProContext.Users.Where(s => s.UserId == ReceivedUser).FirstOrDefault();
                mail.To.Add(new MailAddress(userReciver?.Email ?? ""));

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
