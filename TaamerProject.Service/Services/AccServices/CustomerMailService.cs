//using Azure.Core.Pipeline;
//using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
//using iTextSharp.tool.xml.html;
//using iTextSharp.tool.xml.parser;
//using iTextSharp.tool.xml.pipeline.css;
//using iTextSharp.tool.xml.pipeline.end;
//using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using Twilio.TwiML.Messaging;

namespace TaamerProject.Service.Services
{
    public class CustomerMailService : ICustomerMailService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerMailRepository _CustomerMailRepository;
        private readonly IUserNotificationPrivilegesService _userNotificationPrivilegesService;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IUsersRepository _UsersRepository;
        public CustomerMailService(TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerMailRepository customerMailRepository, 
            IUsersRepository usersRepository, IUserNotificationPrivilegesService userNotificationPrivilegesService, IBranchesRepository branchesRepository,
            IOrganizationsRepository organizationsRepository, IEmailSettingRepository emailSettingRepository, ICustomerRepository customerRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CustomerMailRepository = customerMailRepository;
            _UsersRepository = usersRepository;
            _userNotificationPrivilegesService = userNotificationPrivilegesService;
            _BranchesRepository = branchesRepository;
            _OrganizationsRepository = organizationsRepository;
            _EmailSettingRepository = emailSettingRepository;
            _CustomerRepository = customerRepository;
        }
        public async Task<IEnumerable<CustomerMailVM>> GetAllCustomerMails(int BranchId)
        {
            var CustomerMails =await _CustomerMailRepository.GetAllCustomerMails(BranchId);
            return CustomerMails;
        }
        public async Task<IEnumerable<CustomerMailVM>> GetMailsByCustomerId(int? CustomerId)
        {
            var CustomerMails = await _CustomerMailRepository.GetMailsByCustomerId(CustomerId);
            return CustomerMails;
        }
        public GeneralMessage SaveCustomerMail(CustomerMail CustomerMail, int UserId, int BranchId, string AttachmentFile, bool? IsOrgEmail = null)
        {
            int custid = 0;
            try
            {
                foreach (var CustomerMailId in CustomerMail.AssignedCustomersIds)
                {
                    var CustomerMailObj = new CustomerMail();
                    CustomerMailObj.CustomerId = CustomerMailId;
                    CustomerMailObj.SenderUser = UserId;
                    CustomerMailObj.MailSubject = CustomerMail.MailSubject;
                    CustomerMailObj.MailText = CustomerMail.MailText;
                    CustomerMailObj.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    CustomerMailObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                    if (CustomerMail.FileUrl != null)
                    {
                        CustomerMailObj.FileUrl = CustomerMail.FileUrl;
                    }
                    CustomerMailObj.AddUser = UserId;
                    CustomerMailObj.BranchId = BranchId;
                    CustomerMailObj.AddDate = DateTime.Now;

                    _TaamerProContext.CustomerMail.Add(CustomerMailObj);
                    var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                    Organizations? org = _TaamerProContext.Organizations.Where(s => s.OrganizationId == Organization).FirstOrDefault();
                    var emailsetting = _EmailSettingRepository.GetEmailSetting(Organization).Result;

                    var mail = new MailMessage();
                    //var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(Organization).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(Organization).Result.Password);

                    custid = CustomerMailId;
                    var displaynm = "";  IsOrgEmail = true; var SenderEmail = "";var SenderPassword = ""; var SenderPort = "587"; var SenderHost = "";

                    if (IsOrgEmail.HasValue && IsOrgEmail.Value)
                    {
                        SenderEmail = org.Email;
                        SenderPassword=org.Password;
                        SenderPort = org.Port;
                        SenderHost = org.Host;
                        if (org.SenderName != null)
                        {
                            displaynm = org.SenderName;

                        }
                        else
                        {
                            displaynm = org.NameAr;

                        }
                    }
                    else
                    {
                        SenderEmail = emailsetting.SenderEmail;
                        SenderPassword = emailsetting.Password;
                        SenderPort = emailsetting.Port;
                        SenderHost = emailsetting.Host;
                        if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                        {
                            displaynm = _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName;

                        }
                        else
                        {
                            displaynm = "لديك اشعار من نظام تعمير السحابي";

                        }
                    }
                    var loginInfo = new NetworkCredential(SenderEmail, SenderPassword);
                    mail.From = new MailAddress(SenderEmail, displaynm);


                    if (_CustomerRepository.GetById(CustomerMailId).CustomerEmail != null && _CustomerRepository.GetById(CustomerMailId).CustomerEmail != "")
                    {
                        mail.To.Add(new MailAddress(_CustomerRepository.GetById(CustomerMailId).CustomerEmail));
                        mail.Subject = CustomerMail.MailSubject;
                        mail.Body = CustomerMail.MailText;

                        try
                        {
                            if (CustomerMail.FileUrl != null)
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

                            var exw = ex;
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في ارسال ميل العميل"+ _CustomerRepository.GetById(CustomerMailId).CustomerNameAr;
                           _SystemAction.SaveAction("SaveCustomerMail", "CustomerMailService", 1,  Resources.FailedToSendCheckEmailSetting, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =  Resources.FailedToSendCheckEmailSetting };

                        }
                        try
                        {
                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                            var smtpClient = new SmtpClient(SenderHost);
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            //smtpClient.Port = 587;

                            smtpClient.Port = Convert.ToInt32(SenderPort);

                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.FailedToSendCheckEmailSetting };
                        }
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "ارسال ميل العميل" + _CustomerRepository.GetById(custid).CustomerNameAr;
                _SystemAction.SaveAction("SaveCustomerMail", "CustomerMailService", 1, Resources.sent_succesfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في ارسال ميل العميل";
                _SystemAction.SaveAction("SaveCustomerMail", "CustomerMailService", 1,  Resources.FailedToSendCheckEmailSetting, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =  Resources.FailedToSendCheckEmailSetting };
            }
        }

        public GeneralMessage SaveCustomerMailOfferPrice(CustomerMailVM CustomerMail, int UserId, int BranchId, string AttachmentFile,string body, bool? IsOrgEmail = null)
        {
            try
            {
                int custid = 0;
                foreach (var CustomerMailId in CustomerMail.AssignedCustomersIds)
                {
                    var CustomerMailObj = new CustomerMail();
                    CustomerMailObj.CustomerId = CustomerMailId;
                    CustomerMailObj.SenderUser = UserId;
                    CustomerMailObj.MailSubject = CustomerMail.MailSubject;
                    CustomerMailObj.MailText = CustomerMail.MailText;
                    CustomerMailObj.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                    CustomerMailObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar"));
                    if (CustomerMail.FileUrl != null)
                    {
                        CustomerMailObj.FileUrl = CustomerMail.FileUrl;
                    }
                    CustomerMailObj.AddUser = UserId;
                    CustomerMailObj.BranchId = BranchId;
                    CustomerMailObj.AddDate = DateTime.Now;

                    _TaamerProContext.CustomerMail.Add(CustomerMailObj);
                    var Organization = _BranchesRepository.GetById(BranchId).OrganizationId;
                    Organizations? org = _TaamerProContext.Organizations.Where(s => s.OrganizationId == Organization).FirstOrDefault();
                    var emailsetting = _EmailSettingRepository.GetEmailSetting(Organization).Result;

                    var mail = new MailMessage();
                    IsOrgEmail = true;
                    custid = CustomerMailId;
                    var displaynm = ""; IsOrgEmail = true; var SenderEmail = ""; var SenderPassword = ""; var SenderPort = "587"; var SenderHost = "";

                    if (IsOrgEmail.HasValue && IsOrgEmail.Value)
                    {
                        SenderEmail = org.Email;
                        SenderPassword = org.Password;
                        SenderPort = org.Port;
                        SenderHost = org.Host;
                        if (org.SenderName != null)
                        {
                            displaynm = org.SenderName;

                        }
                        else
                        {
                            displaynm = org.NameAr;

                        }
                    }
                    else
                    {
                        SenderEmail = emailsetting.SenderEmail;
                        SenderPassword = emailsetting.Password;
                        SenderPort = emailsetting.Port;
                        SenderHost = emailsetting.Host;
                        if (_EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName != null)
                        {
                            displaynm = _EmailSettingRepository.GetEmailSetting(BranchId).Result.DisplayName;

                        }
                        else
                        {
                            displaynm = "لديك اشعار من نظام تعمير السحابي";

                        }
                    }
                    //string requestURL = @"http://[CloudServiceURL]/api/PdfGenerator/GetWebPageInfo/?url=http://google.com"; // I'm currently just using google as a testsite to generate the pdf of
                    //byte[] pdfBytes =  GetPdfBytesFromUrlAsync(requestURL);


                    var loginInfo = new NetworkCredential(SenderEmail, SenderPassword);
                    mail.From = new MailAddress(SenderEmail, displaynm);

                    if (CustomerMail.CustomerEmail != null && CustomerMail.CustomerEmail != "")
                    {
                        //mail.To.Add(new MailAddress(_CustomerRepository.GetById(CustomerMailId).CustomerEmail));
                        mail.To.Add(new MailAddress(CustomerMail.CustomerEmail));


                        mail.Subject = CustomerMail.MailSubject;
                        mail.Body = CustomerMail.MailText;



                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        //byte[]? bytesArray = null;
                        //using (var ms = new MemoryStream())
                        //{
                        //    var document = new iTextSharp.text.Document();
                        //    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        //    document.Open();
                        //    using (var strReader = new StringReader(body))
                        //    {
                        //        //Set factories
                        //        HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                        //        htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                        //        //Set css
                        //        ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                        //        string contentRootPath1 = Path.Combine("distnew/css/PrintConfig.css");
                        //        //string contentRootPath1 = "";

                        //        cssResolver.AddCssFile(contentRootPath1, true);

                        //        //Export
                        //        IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));
                        //        var worker = new XMLWorker(pipeline, true);
                        //        var xmlParse = new XMLParser(true, worker);
                        //        xmlParse.Parse(strReader);
                        //        xmlParse.Flush();
                        //    }
                        //    document.Close();
                        //    bytesArray = ms.ToArray();
                        //}

                        //mail.IsBodyHtml = true;
                        //mail.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(bytesArray), "Test.pdf"));

                        try
                        {
                            if (CustomerMail.FileUrl != null)
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

                            var exw = ex;
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في ارسال ميل العميل" + _CustomerRepository.GetById(CustomerMailId).CustomerNameAr;
                            _SystemAction.SaveAction("SaveCustomerMail", "CustomerMailService", 1,  Resources.FailedToSendCheckEmailSetting, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =  Resources.FailedToSendCheckEmailSetting };

                        }
                        try
                        {
                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                            var smtpClient = new SmtpClient(SenderHost);
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            //smtpClient.Port = 587;

                            smtpClient.Port = Convert.ToInt32(SenderPort);

                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "تأكد من ميل العميل" };
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "ارسال ميل العميل" + _CustomerRepository.GetById(custid).CustomerNameAr;
                _SystemAction.SaveAction("SaveCustomerMail", "CustomerMailService", 1, Resources.sent_succesfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في ارسال ميل العميل";
                _SystemAction.SaveAction("SaveCustomerMail", "CustomerMailService", 1,  Resources.FailedToSendCheckEmailSetting, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =  Resources.FailedToSendCheckEmailSetting };
            }
        }

        public GeneralMessage DeleteCustomerMail(int MailId, int UserId, int BranchId)
        {
            try
            {
                CustomerMail CustomerMail = _TaamerProContext.CustomerMail.Where(x=>x.MailId==MailId).FirstOrDefault();
                CustomerMail.IsDeleted = true;
                CustomerMail.DeleteDate = DateTime.Now;
                CustomerMail.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف  ميل العميل رقم " + MailId;
                _SystemAction.SaveAction("DeleteCustomerMail", "CustomerMailService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  ميل العميل رقم " + MailId; ;
                _SystemAction.SaveAction("DeleteCustomerMail", "CustomerMailService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public bool SendMail_SysNotification(int BranchId, int UserId, int ReceivedUser, string Subject, string textBody, bool IsBodyHtml = false,string empmail=null)
        {
            try
            {
                var branch = _BranchesRepository.GetById(BranchId).OrganizationId;


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
                if (empmail != null && empmail != "")
                {
                    mail.To.Add(new MailAddress(empmail));
                }
                else
                {


                    mail.To.Add(new MailAddress(_UsersRepository.GetById(ReceivedUser).Email));
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
    }
}
