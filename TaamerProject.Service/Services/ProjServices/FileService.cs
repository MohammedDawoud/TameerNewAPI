
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _FileRepository;
        private readonly IProjectPhasesTasksRepository _ProjectPhasesTasksRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IUsersRepository _usersRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly INotificationService _notificationService;



        public FileService(IFileRepository fileRepository, IProjectPhasesTasksRepository projectPhasesTasksRepository, INotificationRepository notificationRepository,
            IEmailSettingRepository emailSettingRepository, ICustomerRepository customerRepository, IBranchesRepository branchesRepository, IOrganizationsRepository organizationsRepository,
           IProjectRepository projectRepository , ISys_SystemActionsRepository sys_SystemActionsRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService,
           IUsersRepository usersRepository, TaamerProjectContext dataContext, ISystemAction systemAction, INotificationService notificationService)
        {
            _FileRepository = fileRepository;
            _ProjectPhasesTasksRepository = projectPhasesTasksRepository;
            _NotificationRepository = notificationRepository;
            _EmailSettingRepository = emailSettingRepository;

            _BranchesRepository = branchesRepository;
            _OrganizationsRepository = organizationsRepository;
            _CustomerRepository = customerRepository;
            _ProjectRepository = projectRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _usersRepository = usersRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _notificationService = notificationService;

        }

        public async Task<IEnumerable<FileVM>> GetAllFiles(int? ProjectId, string SearchText, string DateFrom, string DateTo, int? Filetype, int BranchId)
        {
            var Files = await _FileRepository.GetAllFiles(ProjectId, SearchText, DateFrom, DateTo,Filetype, BranchId);
            return Files;
        }
        public async Task<IEnumerable<TasksLoadVM>> GetAllFilesTree(int? ProjectId, string? SearchText, bool? IsCertified, string DateFrom, string DateTo, int? Filetype, int BranchId)
        {
            var Files =  _FileRepository.GetAllFilesTree(ProjectId, SearchText, IsCertified, DateFrom, DateTo, Filetype, BranchId).Result.ToList();
            var Projects = Files.GroupBy(x => x.ProjectId).Select(g => g.First()).ToDictionary(x => x.ProjectId);

            if (Files != null && Files.Count() > 0)
            {
                List<TasksVM> treeItems = new List<TasksVM>();
                foreach (var item in Files)
                {
                    treeItems.Add(new TasksVM(item.FileId.ToString(), ((item.ProjectId == 0 || item.ProjectId == null) ? "#" : item.ProjectId.ToString() + "pr"), item.FileName + "-" + item.FileTypeName + "-" + item.UserFullName, item.FileId.ToString(),null,null));
                }

                List<TasksVM> treepro = new List<TasksVM>();
                foreach (var item in Projects)
                {
                    var Pro = (item.Key.ToString() + "pr");
                    var ListChild = treeItems.Where(s => s.parent == Pro).ToList();
                    treepro.Add(new TasksVM(item.Key.ToString() + "pr", "#", item.Value?.ProjectNo?.ToString() + " - " + item.Value?.CustomerName?.ToString(), ListChild, item.Key.ToString(),null,null));
                }
                var IteUnion = treeItems.Union(treepro);

                var objList = new List<TasksLoadVM>();
                var obj = new TasksLoadVM();
                var objCh = new ChildrenVM();
                var objChitem = new itemVM();

                foreach (var item in treepro)
                {
                    obj = new TasksLoadVM();
                    obj.id = item.id;
                    obj.name = item.name;
                    obj.children = new List<ChildrenVM>();
                    foreach (var item2 in item.children)
                    {
                        objCh = new ChildrenVM();
                        objCh.id = item2.id;
                        objCh.name = item2.name;
                        objChitem = new itemVM();
                        objChitem.phaseid = item2.id;
                        objChitem.phrase = item2.name;
                        objCh.item = objChitem;
                        obj.children!.Add(objCh);
                    }
                    objList.Add(obj);
                }
                return objList;
            }
            else
            {
                return new List<TasksLoadVM>();
            }
        }
        public FileVM GetFileByBarcode(string Barcode, string taxCode)
        {

            var Files = _FileRepository.GetFileByBarcode(Barcode, taxCode).Result;
            if (Files != null)
            {
                var ListOfPrivNotify = new List<Notification>();
                var Desc = Files.CustomerName + " والعميل " + Files.ProjectNo + " قام العميل بعملية تحقق ناجحة للمشروع  ";
                var UserNotifPriv = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3172).Result;
                if (UserNotifPriv.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv)
                    {
                        ListOfPrivNotify.Add(new Notification
                        {
                            ReceiveUserId = userCounter.UserId,
                            Name = "إجراء عملية تحقق من الوثائق ناجحة",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                            HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                            SendUserId = 1,
                            Type = 1,
                            Description = " العميل : " + Files.CustomerName + " تم التحقق من مشروع رقم  : " + Files.ProjectNo + " باركود رقم " + Barcode,
                            AllUsers = false,
                            SendDate = DateTime.Now,
                            ProjectId = Files.ProjectId,
                            TaskId = 0,
                            AddUser = 1,
                            BranchId = 1,
                            AddDate = DateTime.Now,
                            IsHidden = false
                        });
                        _notificationService.sendmobilenotification((int)userCounter.UserId, "إجراء عملية تحقق من الوثائق ناجحة", " العميل : " + Files.CustomerName + " تم التحقق من مشروع رقم  : " + Files.ProjectNo + " باركود رقم " + Barcode);
                    }
                    _TaamerProContext.Notification.AddRange(ListOfPrivNotify);
                    _TaamerProContext.SaveChanges();

                }

                var UserNotifPriv_email = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3171).Result;
                if (UserNotifPriv_email.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv_email)
                    {
                        SendMailFileBarcode(Files.ProjectId ?? 0, Desc, "إجراء عملية تحقق من الوثائق ناجحة", 1, 1, userCounter.UserId ?? 0);
                    }
                }
            }
            return Files;
        }

        private bool SendMailFileBarcode(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
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

                    //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    mail.Body = textBody;
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



        public async Task<IEnumerable<FileVM>> GetFileByBarcodeShare(string ProjectNo, string taxCode)
        {

            var Files =await _FileRepository.GetFileByBarcodeShare(ProjectNo, taxCode);
            return Files;
        }

        public FileVM GetFileByBarcode2(string Barcode, string taxCode)
        {

            var Files = _FileRepository.GetFileByBarcode2(Barcode, taxCode).Result;
            if (Files != null)
            {
                var ListOfPrivNotify = new List<Notification>();
                var Desc = Files.CustomerName + " والعميل " + Files.ProjectNo + " قام العميل بعملية تحقق ناجحة للمشروع  ";
                var UserNotifPriv = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3172).Result;
                if (UserNotifPriv.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv)
                    {
                        ListOfPrivNotify.Add(new Notification
                        {
                            ReceiveUserId = userCounter.UserId,
                            Name = "إجراء عملية تحقق من الوثائق ناجحة",
                            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                            HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                            SendUserId = 1,
                            Type = 1,
                            Description = " العميل : " + Files.CustomerName + " تم التحقق من مشروع رقم  : " + Files.ProjectNo + " باركود رقم " + Barcode,
                            AllUsers = false,
                            SendDate = DateTime.Now,
                            ProjectId = Files.ProjectId,
                            TaskId = 0,
                            AddUser = 1,
                            BranchId = 1,
                            AddDate = DateTime.Now,
                            IsHidden = false
                        });
                        _notificationService.sendmobilenotification((int)userCounter.UserId, "إجراء عملية تحقق من الوثائق ناجحة", " العميل : " + Files.CustomerName + " تم التحقق من مشروع رقم  : " + Files.ProjectNo + " باركود رقم " + Barcode);

                    }
                    _TaamerProContext.Notification.AddRange(ListOfPrivNotify);
                    _TaamerProContext.SaveChanges();
                }

                var UserNotifPriv_email = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(3171).Result;
                if (UserNotifPriv_email.Count() != 0)
                {
                    foreach (var userCounter in UserNotifPriv_email)
                    {
                        SendMailFileBarcode(Files.ProjectId ?? 0, Desc, "إجراء عملية تحقق من الوثائق ناجحة", 1, 1, userCounter.UserId ?? 0);
                    }
                }


            }
            return Files;
        }
        public async Task<IEnumerable<FileVM>> GetAllTaskFiles(int TaskId, string SearchText)
        {
            var Files =await _FileRepository.GetAllTaskFiles(TaskId, SearchText);
            return Files;
        }
        public async Task<IEnumerable<FileVM>> GetAllCertificateFiles(int? ProjectId, bool IsCertified, int BranchId)
        {
            var Files =await _FileRepository.GetAllCertificateFiles(ProjectId, IsCertified, BranchId);
            return Files;
        }
        public Task<IEnumerable<FileVM>> GetAllFilesByDateSearch(int? ProjectId, DateTime DateFrom, DateTime ToData, int BranchId)
        {
            var Files = _FileRepository.GetAllFilesByDateSearch(ProjectId, DateFrom, ToData, BranchId);
            return Files;
        }
        public GeneralMessage SaveFile(ProjectFiles file, int UserId, int BranchId)
        {
            try
            {
                if (file.FileId == 0)
                {
                    if (file.ProjectId == null)
                    {
                        if (file.TaskId != null)
                        {
                            file.ProjectId = _TaamerProContext.ProjectPhasesTasks.Where(x=>x.PhaseTaskId==file.TaskId).FirstOrDefault().ProjectId;
                        }
                        else
                        {
                            file.ProjectId = _TaamerProContext.Notification.Where(x=>x.NotificationId==file.NotificationId).FirstOrDefault().ProjectId;
                        }
                    }

                    file.UploadDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    //file.UploadDate = DateTime.Now.ToShortDateString();
                    file.BranchId = BranchId;
                    file.UserId = UserId;
                    file.AddUser = UserId;
                    file.AddDate = DateTime.Now;
                    _TaamerProContext.ProjectFiles.Add(file);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة ملف جديد";
               _SystemAction.SaveAction("SaveFile", "FileService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الملف";
                _SystemAction.SaveAction("SaveFile", "FileService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage UpdateFileShare(ProjectFiles file, int BranchId, string Link, int UserId, string imgurl, string url)
        {
            try
            {
                if (file.FileId != 0)
                {
                    ProjectFiles ProFile = _FileRepository.GetById(file.FileId);
                    DateTime DateTime_V2 = DateTime.Now;
                    double Val = Convert.ToDouble(file.TimeShare);
                    if (file.TimeTypeShare == 2) //2-day
                    {
                        DateTime_V2 = DateTime_V2.AddDays(Val);
                    }
                    else//1-hour
                    {
                        DateTime_V2 = DateTime_V2.AddHours(Val);
                    }
                    var DateTime_V = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    var ActDateTime = DateTime_V2.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    var ActDateTime2 = DateTime_V2.ToString("dddd, dd MMMM yyyy HH: mm:ss", CultureInfo.InvariantCulture);

                    var Da = String.Format("{0:F}", DateTime_V2);  // "Sunday, March 09, 2008 4:05:07 PM" FullDateTime
                    ProFile.IsShare = file.IsShare;
                    ProFile.ViewShare = file.ViewShare;
                    ProFile.DonwloadShare = file.DonwloadShare;
                    ProFile.TimeShare = file.TimeShare;
                    ProFile.TimeTypeShare = file.TimeTypeShare;
                    ProFile.TimeShareDate = ActDateTime;



                    var project = _ProjectRepository.GetById((int)ProFile.ProjectId);
                    var Customer = _CustomerRepository.GetById((int)project.CustomerId);


                    //SendCustomerMail_Share(project.ProjectNo, ProFile.FileName, BranchId, ProFile.BarcodeFileNum, Customer.CustomerNationalId, Customer.CustomerNameAr, Link, Customer.CustomerEmail, UserId);

                    SendMail_CustomersharefileStamp(project.ProjectNo, ProFile.FileName, BranchId, ProFile.BarcodeFileNum, Customer.CustomerNationalId, Customer.CustomerNameAr, Link, Customer.CustomerEmail, UserId, 1, url, imgurl, true);


                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " اضافة خاصية المشاركة للملف رقم " + file.FileId;
                _SystemAction.SaveAction("UpdateFileShare", "FileService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }

            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل ف اضافة خاصية مشاركة للملف رقم " + file.FileId;
                _SystemAction.SaveAction("UpdateFileShare", "FileService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }

        public GeneralMessage UpdateFileUploadFileId(int FileId, string UploadFileId,int type)
        {
            try
            {
                if (FileId != 0)
                {
                    ProjectFiles ProFile = _FileRepository.GetById(FileId);
                    if(type==1)
                    {
                        ProFile.UploadFileId = UploadFileId;
                    }
                    else
                    {
                        ProFile.UploadFileIdB = UploadFileId;

                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }

            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }

        public GeneralMessage NotUpdateFileShare(ProjectFiles file, int BranchId, string Link, int UserId, string imgurl, string url)
        {
            try
            {
                if (file.FileId != 0)
                {
                    ProjectFiles ProFile = _FileRepository.GetById(file.FileId);
                    ProFile.IsShare = null;
                    ProFile.ViewShare = null;
                    ProFile.DonwloadShare = null;
                    ProFile.TimeShare = null;
                    ProFile.TimeTypeShare = null;
                    ProFile.TimeShareDate = null;



                    var project = _ProjectRepository.GetById((int)ProFile.ProjectId);
                    var Customer = _CustomerRepository.GetById((int)project.CustomerId);


                    //SendCustomerMail_NotShare(project.ProjectNo, ProFile.FileName, BranchId, ProFile.BarcodeFileNum, Customer.CustomerNationalId, Customer.CustomerNameAr, Link, Customer.CustomerEmail, UserId);
                    SendMail_CustomersharefileStamp(project.ProjectNo, ProFile.FileName, BranchId, ProFile.BarcodeFileNum, Customer.CustomerNationalId, Customer.CustomerNameAr, Link, Customer.CustomerEmail, UserId, 2, url, imgurl, true);



                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " اضافة خاصية المشاركة للملف رقم " + file.FileId;
                _SystemAction.SaveAction("UpdateFileShare", "FileService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }

            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل ف اضافة خاصية مشاركة للملف رقم " + file.FileId;
                _SystemAction.SaveAction("UpdateFileShare", "FileService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }

        public bool SendMail_CustomersharefileStamp(string ProjectNo, string Filename, int BranchId, string FileNumber, string NationalNumber, string CustomerName, string Link, string CustomerEmail, int UserId, int type, string url, string ImgUrl, bool IsBodyHtml = false)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                var Org = _OrganizationsRepository.GetById(Organization);


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
                var textbody = "";
                if (type == 1)
                {
                    title = " الاستاذ " + "/" + CustomerName + " دعوي لمشاهدة ملف بواسطة  " + Org.NameAr;
                    //textbody = "<br />للوصول لرابط الملف, فضلا قم بالضغط علي الرابط المبين واختر رابط الخدمة الالكترونية<br /><button class='btn btn-success' style='background-color:#4CAF50;border:none;color:white;padding:15px 32px;text-align:center;text-decoration:none; display:inline-block;font-size:19px;border-radius:2%;height:47px;'><a  href=" + Link + " target='_blank'>اضغط هنا للوصول للملف</a></button><br />" + " فضلا أدخل المعلومات التالية للتحقق " + "<br />" + " رقم الهوية " + " : " + NationalNumber + "<br />" + " رقم المشروع " + " : " + ProjectNo + "<br />";
                    textbody = "<br />للوصول لرابط الملف, فضلا قم بالضغط علي الرابط المبين واختر رابط الخدمة الالكترونية<br /><a  href=" + Link + " target='_blank'>اضغط هنا للوصول للملف</a><br />" + " فضلا أدخل المعلومات التالية للتحقق " + "<br />" + " رقم الهوية " + " : " + NationalNumber + "<br />" + " رقم المشروع " + " : " + ProjectNo + "<br />";

                    body = PopulateBody(textbody, CustomerName, title, "مع تحيات قسم ادارة المشاريع", url, Org.NameAr);
                }
                else if (type == 2)
                {
                    title = " الاستاذ " + "/" + CustomerName + "  الغاء دعوي لمشاهدة ملف بواسطة  " + Org.NameAr;
                    textbody = "تم الغاء دعوي المشاركة";
                    body = PopulateBody(textbody, CustomerName, title, "مع تحيات قسم ادارة المشاريع", url, Org.NameAr);
                }

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                mail.To.Add(new MailAddress(CustomerEmail));


                mail.Subject = title;

                mail.Body = textbody;
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
      
        public GeneralMessage SendCustomerMail_Share(string ProjectNo, string Filename, int BranchId, string FileNumber, string NationalNumber, string CustomerName, string Link, string CustomerEmail, int UserId)
        {
            try
            {
                var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                var Org = _OrganizationsRepository.GetById(Organization);
                var mail = new MailMessage();
                //var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(Organization).SenderEmail, _EmailSettingRepository.GetEmailSetting(Organization).Password);
                var loginInfo = new NetworkCredential(Org.Email, Org.Password);
                var SenderEmail = _EmailSettingRepository.GetEmailSetting(Organization).Result.SenderEmail;
                //var loginInfo = new NetworkCredential(SenderEmail, _EmailSettingRepository.GetEmailSetting(Organization).Password);

                var EmailFrom1 = Org.Email;

                if (_EmailSettingRepository.GetEmailSetting(Organization).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailFrom1, _EmailSettingRepository.GetEmailSetting(Organization).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailFrom1, "لديك اشعار من نظام تعمير السحابي");
                }
                // mail.From= new MailAddress(EmailFrom1);

                mail.To.Add(new MailAddress(CustomerEmail));
                //mail.Subject = " الاستاذ "+"/"+ CustomerName + " دعوي لمشاهدة ملف بواسطة مكتب بياناتك للاستشارات الهندسية ";
                mail.Subject = " الاستاذ " + "/" + CustomerName + " دعوي لمشاهدة ملف بواسطة  " + Org.NameAr;

                mail.Body = "<br />للوصول لرابط الملف, فضلا قم بالضغط علي الرابط المبين واختر رابط الخدمة الالكترونية<br />" + Link + "<br />" + " فضلا أدخل المعلومات التالية للتحقق " + "<br />" + " رقم الهوية " + " : " + NationalNumber + "<br />" + " رقم المشروع " + " : " + ProjectNo + "<br />";

                mail.IsBodyHtml = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(Organization).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port =Convert.ToInt32(Org.Port);
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(Organization).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم ارسال ملف ميل مشاركة";
                _SystemAction.SaveAction("SendCustomerMail_Share", "FileService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في ارسال ميل مشاركة ملف";
                _SystemAction.SaveAction("SendCustomerMail_Share", "FileService", 1, Resources.Failed_to_send, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_to_send };
            }
        }
        public GeneralMessage SendCustomerMail_NotShare(string ProjectNo, string Filename, int BranchId, string FileNumber, string NationalNumber, string CustomerName, string Link, string CustomerEmail, int UserId)
        {
            try
            {
                var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                var Org = _OrganizationsRepository.GetById(Organization);
                var mail = new MailMessage();
                //var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(Organization).SenderEmail, _EmailSettingRepository.GetEmailSetting(Organization).Password);
                var loginInfo = new NetworkCredential(Org.Email, Org.Password);

                var EmailFrom1 = Org.Email;
                if (_EmailSettingRepository.GetEmailSetting(Organization).Result.DisplayName != null)
                {
                    mail.From = new MailAddress(EmailFrom1, _EmailSettingRepository.GetEmailSetting(Organization).Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress(EmailFrom1, "لديك اشعار من نظام تعمير السحابي");
                }

                // mail.From = new MailAddress(EmailFrom1);
                mail.To.Add(new MailAddress(CustomerEmail));
                mail.Subject = " الاستاذ " + "/" + CustomerName + " الغاء دعوي لمشاهدة ملف بواسطة  " + Org.NameAr;

                mail.Body = "تم الغاء دعوي المشاركة";

                //mail.IsBodyHtml = true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(Organization).Result.Host);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Port =Convert.ToInt32(Org.Port);
                //smtpClient.Port = 587;
                smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(Organization).Result.Port);

                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تم ارسال ملف ميل بالغاء المشاركة";
                _SystemAction.SaveAction("SendCustomerMail_NotShare", "FileService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في ارسال ميل بالغاء مشاركة ملف";
                _SystemAction.SaveAction("SendCustomerMail_NotShare", "FileService", 1, Resources.Failed_to_send, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_to_send };
            }
        }


        public GeneralMessage SaveFile_Bar(ProjectFiles file, int UserId, int BranchId)
        {
            try
            {
                if (file.FileId == 0)
                {
                    if (file.ProjectId == null)
                    {
                        if (file.TaskId != null)
                        {
                            file.ProjectId = _ProjectPhasesTasksRepository.GetById((int)file.TaskId).ProjectId;
                        }
                        else
                        {
                            file.ProjectId = _NotificationRepository.GetById((int)file.NotificationId).ProjectId;
                        }
                    }

                    file.UploadDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    //file.UploadDate = DateTime.Now.ToShortDateString();
                    file.BranchId = BranchId;
                    file.UserId = UserId;
                    file.AddUser = UserId;
                    file.AddDate = DateTime.Now;
                    file.IsDeleted = false;
                    _TaamerProContext.ProjectFiles.Add(file);
                }
                _TaamerProContext.SaveChanges();

                var FileUpdated = _FileRepository.GetById(file.FileId);
                FileUpdated.BarcodeFileNum = file.BarcodeFileNum + file.FileId;
                _TaamerProContext.SaveChanges();


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة ملف جديد";
                _SystemAction.SaveAction("SaveFile_Bar", "FileService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = FileUpdated.BarcodeFileNum };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الملف";
                _SystemAction.SaveAction("SaveFile_Bar", "FileService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------


                var x = ex.Message;
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteFile(int FileId, int UserId, int BranchId)
        {
            try
            {
                ProjectFiles file = _FileRepository.GetById(FileId);
                file.IsDeleted = true;
                file.DeleteDate = DateTime.Now;
                file.DeleteUser = UserId;

                //Clear Signed Contract of the project

                var project = _ProjectRepository.GetMatching(x => x.ProjectId == file.ProjectId).FirstOrDefault();
                var contract = project.Contracts;
                if (contract != null && !string.IsNullOrEmpty(contract.AttachmentUrl) && contract.AttachmentUrl.Contains(file.FileUrl))
                    contract.AttachmentUrl = null;

                _TaamerProContext.SaveChanges();


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف ملف رقم " + FileId;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully,ReturnedStr= file.UploadName, ReturnedStrExtra = file.UploadName, ReturnedStrNeeded = file.Extension };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مبف رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage DeleteFileDrive(int FileId, int UserId, int BranchId)
        {
            try
            {
                ProjectFiles file = _FileRepository.GetById(FileId);
                file.IsDeleted = true;
                file.DeleteDate = DateTime.Now;
                file.DeleteUser = UserId;

                //Clear Signed Contract of the project

                var project = _ProjectRepository.GetMatching(x => x.ProjectId == file.ProjectId).FirstOrDefault();
                var contract = project.Contracts;
                if (contract != null && !string.IsNullOrEmpty(contract.AttachmentUrl) && contract.AttachmentUrl.Contains(file.FileUrl))
                    contract.AttachmentUrl = null;

                _TaamerProContext.SaveChanges();


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف ملف رقم " + FileId;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully, ReturnedStr = file.UploadFileId, ReturnedStrExtra = file.UploadFileIdB, ReturnedStrNeeded = file.Extension };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مبف رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage DeleteFileOneDrive(int FileId, int UserId, int BranchId)
        {
            try
            {
                ProjectFiles file = _FileRepository.GetById(FileId);
                file.IsDeleted = true;
                file.DeleteDate = DateTime.Now;
                file.DeleteUser = UserId;

                //Clear Signed Contract of the project

                var project = _ProjectRepository.GetMatching(x => x.ProjectId == file.ProjectId).FirstOrDefault();
                var contract = project.Contracts;
                if (contract != null && !string.IsNullOrEmpty(contract.AttachmentUrl) && contract.AttachmentUrl.Contains(file.FileUrl))
                    contract.AttachmentUrl = null;

                _TaamerProContext.SaveChanges();


                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف ملف رقم " + FileId;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully, ReturnedStr = file.UploadFileId, ReturnedStrExtra = file.UploadFileIdB, ReturnedStrExtra2 = file.UploadName, ReturnedStrNeeded = file.Extension };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مبف رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public GeneralMessage ADDFileComment(int FileId, int UserId, int BranchId, string Comment, string Url, string ImgUrl)
        {
            try
            {
                ProjectFiles file = _FileRepository.GetById(FileId);
                file.CustomeComment = Comment;
                file.UpdateDate = DateTime.Now;
                file.UpdateUser = UserId;
                var pro = _ProjectRepository.GetProjectById("", (int)file.ProjectId).Result;

                _TaamerProContext.SaveChanges();
                try
                {
                    var UserNotification = new Notification();
                    UserNotification.ReceiveUserId = pro.MangerId;
                    UserNotification.Name = "تعلق من العميل علي الملف";
                    UserNotification.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    UserNotification.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                    UserNotification.SendUserId = 1;
                    UserNotification.Type = 1; // notification
                    UserNotification.Description = " تم ارسال تعليق من العميل  : " + pro.CustomerName_W + " بنص " + Comment + " علي ملف علي مشروع رقم " + pro.ProjectNo + "";
                    UserNotification.AllUsers = false;
                    UserNotification.SendDate = DateTime.Now;
                    UserNotification.ProjectId = pro.ProjectId;

                    UserNotification.AddUser = UserId;
                    UserNotification.AddDate = DateTime.Now;
                    UserNotification.BranchId = BranchId;
                    UserNotification.IsHidden = false;
                    UserNotification.NextTime = null;
                    _TaamerProContext.Notification.Add(UserNotification);
                    _notificationService.sendmobilenotification(pro.MangerId ?? 0, "  تعليق من العميل   " + pro.CustomerName_W + " علي مشروع " + pro.ProjectNo, Comment);

                    var body = "";
                    body = "<table border='1'style='text-align:center;padding:10px;'><tr><th style='border=1px solid #eee'>نص التعليق  </th> <th style='border=1px solid #eee'>رقم المشروع   </th></tr><tr><td>" + Comment + "</td><td>" + pro.ProjectNo + "</td></tr></table>";


                    SendMail_CustomerStamp(1, UserId, pro.MangerId ?? 0, " رسالة العميل", body, Url, ImgUrl, 1, pro.CustomerName_W, true);
                }
                catch
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " فشل في حفظ تعليق  ملف رقم " + FileId; ;
                    _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة تعليق لملف رثم " + FileId;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ تعليق  ملف رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteFile", "FileService", 3, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public async Task<FileVM> GetFilesById(int FileId)
        {
            return await _FileRepository.GetFilesById(FileId);
        }
        public async Task<int> GetUserFileUploadCount(int? UserId)
        {
            var UserFileUploadCount =await _FileRepository.GetUserFileUploadCount(UserId);
            return UserFileUploadCount;
        }


        public bool SendMail_CustomerStamp(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, string Url, string ImgUrl, int type, string customername, bool IsBodyHtml = false)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                var org = _OrganizationsRepository.GetById(branch);

                var mailuser = _usersRepository.GetById(ReceivedUser).Email;


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
                    title = " ارسل اليك العميل " + customername + " تعليق ";
                    body = PopulateBody(textBody, _usersRepository.GetUserById(ReceivedUser, "rtl").Result.FullName, title, "مع تحيات قسم ادارة المشاريع", Url, org.NameAr);
                }

                LinkedResource logo = new LinkedResource(ImgUrl);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my bayanatech logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body.Replace("{Header}", title), null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);
                mail.AlternateViews.Add(av1);
                mail.To.Add(new MailAddress(mailuser));


                mail.Subject = title;

                mail.Body = body;
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
