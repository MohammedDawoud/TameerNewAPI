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
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Mime;
using Newtonsoft.Json;
using System.Security.Cryptography;
using TaamerP.Service.LocalResources;
using TaamerProject.Models.Enums;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using Twilio.TwiML.Voice;

namespace TaamerProject.Service.Services
{
    public class SupportRequestsService :  ISupportRequestsService
    {
        private readonly ISupportResquestsRepository _supportRequestsRepository;
        private readonly IOrganizationsRepository _OrganizationsRepository;
        private readonly IUsersRepository _UsersRepository;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ILicencesRepository _LicencesRepository;
        private readonly ISupportRequestsReplayRepository _supportRequestsReplayRepository;



        public SupportRequestsService(TaamerProjectContext dataContext, ISystemAction systemAction, ISupportResquestsRepository supportRequestsRepository
            , IOrganizationsRepository organizationsRepository, IUsersRepository usersRepository, IEmailSettingRepository emailSettingRepository
            , IBranchesRepository branchesRepository, ILicencesRepository licencesRepository, ISupportRequestsReplayRepository supportRequestsReplayRepository)
        {
            _supportRequestsRepository = supportRequestsRepository;
            _OrganizationsRepository = organizationsRepository;
            _UsersRepository = usersRepository;
            _EmailSettingRepository = emailSettingRepository;
            _BranchesRepository = branchesRepository;         
            _LicencesRepository = licencesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _supportRequestsReplayRepository = supportRequestsReplayRepository;


        }

        public GeneralMessage SaveSupportResquests(SupportResquests supportResquests, int UserId, int BranchId, string VersionCode, string AttachmentFile)
        {
            try
            {
                //string stat = "";
                //var user=_UsersRepository.GetById(UserId);
                //var Licences = _LicencesRepository.GetAllLicences("");
                //Licences.Result.FirstOrDefault().Support_Expiry_Date = DecryptValue((Licences).Result.FirstOrDefault().Support_Expiry_Date);
                //var expdate = Licences.Result.FirstOrDefault().Support_Expiry_Date;
                //var datenw =  DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //if (datenw.CompareTo(expdate) < 0 ) {
                //     stat = "مستمر";
                //}
                //else { stat = "منتهي"; }
               // var OrgName = "";
                if (supportResquests.RequestId == 0) 
                {
                    //var organization = _OrganizationsRepository.GetMatching(s => s.IsDeleted == false).FirstOrDefault();
                    var organization = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false).FirstOrDefault();
                    supportResquests.OrganizationId = organization.OrganizationId;
                    supportResquests.AddUser = UserId;
                    supportResquests.UserId = UserId;
                    supportResquests.Status =(int)SupportRequestStatus.Opend;
                    supportResquests.AddDate = DateTime.Now;
                    supportResquests.BranchId = BranchId;
                    var now = DateTime.Now;
                    var date = new DateTime(now.Year, now.Month, now.Day,
                                            now.Hour, now.Minute, now.Second);
                    supportResquests.Date = date;
                    //OrgName = organization.NameAr;
                     _TaamerProContext.SupportResquests.Add(supportResquests);

                    _TaamerProContext.SaveChanges();
                    //SendMail(supportResquests, organization.BranchId.Value, UserId, VersionCode, OrgName, AttachmentFile, stat,user.Email);
                    //SaveLabaikrequest(supportResquests, AttachmentFile, organization.Mobile);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة طلب دعم جديد" + supportResquests.Topic;
                     _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully ,ReturnedStr= organization.Mobile,ReturnedParm=supportResquests.RequestId };
                }
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ  طلب الدعم";
                 _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }
        public Task<IEnumerable<SupportRequestVM>> GetAllSupportResquests(string lang, int BranchId ,int UserId)
        {
            var SupportRequest = _supportRequestsRepository.GetAllSupportResquests(lang, BranchId, UserId);
            
            return SupportRequest;
        }
        public Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquests(string lang, int BranchId, int UserId)
        {
            var SupportRequest = _supportRequestsRepository.GetAllOpenSupportResquests(lang, BranchId, UserId);

            return SupportRequest;
        }

        public Task<IEnumerable<SupportRequestVM>> GetAllOpenSupportResquestsWithReplay(int UserId)
        {
            var SupportRequest = _supportRequestsRepository.GetAllOpenSupportResquestsWithReplay(UserId);

            return SupportRequest;
        }

        public Task<IEnumerable<SupportRequestsReplayVM>> GetAllOpenSupportResquestsreplayesDashboard(int UserId)
        {
            var SupportRequest = _supportRequestsReplayRepository.GetAllUnReadedReplyByServiceId(UserId);

            return SupportRequest;
        }




        public int getpriority(string strpriority)
        {
            int intptiority = 0;
            if(strpriority == "منخفض")
            {
                intptiority = 1;
            }
            else if(strpriority== "متوسط")
            {
                intptiority = 2;
            }
            else
            {
                intptiority = 3;
            }
            return intptiority;
        }
        public ServiceRequest ServiceRequest(SupportResquests support,string attachment,string mobile)
        {
            ServiceRequest sr = new ServiceRequest();
            sr.ServiesName = support.Topic;
            sr.visitDate= DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            sr.VisitTime = "10:00-12:00";
            sr.ServiceType = 100;
            sr.Priority = getpriority( support.priority);
            sr.RequeterMobileNumber = mobile;
            sr.AttachUrl = attachment;

            return sr;
        }
        public bool SendMail(SupportResquests supportResquests, int BranchId, int UserId,string VersionCode,string OrgName,string AttachmentFile,string RequesterMail)
        {
            try
            {
                //var DateOfComplaint = supportResquests.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //var user = _UsersRepository.GetById(supportResquests.UserId);
                Users? user =  _TaamerProContext.Users.Where(s => s.UserId == supportResquests.UserId).FirstOrDefault();

               // var branch = _BranchesRepository.GetById(user.BranchId);
                Branch? branch =  _TaamerProContext.Branch.Where(s => s.BranchId == user.BranchId).FirstOrDefault();

                //string textBody = 
                //    "<table border='1'style='text-align:center;padding:3px;'><tr><td style='border=1px solid #eee'>اسم المنشأة </td>" +
                //    "<td>" + OrgName + "</td></tr><tr><td>اسم العميل </td>" +
                //    "<td>" + user.FullNameAr + "</td></tr><tr><td>وصف الطلب </td><td>" + supportResquests.Topic + "</td></tr><tr><td> رقم التذكرة</td>" +
                //    "<td>"  +"[" + "Ticket ID" + supportResquests.TicketNo + "]" + "  " + supportResquests.Date + "</td></tr></table>";


                string textBody = "<div style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\"," +
                    "sans-serif;border:none;border-bottom:solid windowtext 1.0pt;padding:0in 0in 1.0pt 0in;'>\r\n    <p style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;" +
                    "margin-left:0in;text-align:left;font-size:21px;font-family:\"Calibri\",sans-serif;border:none;padding:0in;'>BAYANATECH</p>" +
                    "\r\n</div>\r\n<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>اسم المنشأة : &nbsp;" + OrgName + "</span></p></strong>\r\n" +
                    "<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>اسم العميل : &nbsp;" + user.FullNameAr + "&nbsp;</span></p></strong>\r\n" +
                    "<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>وصف الطلب: &nbsp; " + supportResquests.Topic + " &nbsp;</span></p></strong>\r\n" +
                    "<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>رقم التذكرة &nbsp;:&nbsp;</span><strong><span style='font-size:19px;font-family:\"Arial\",sans-serif;'>" +
                    "[</span></strong><strong><span dir=\"LTR\" style=\"font-size:19px;line-height:107%;\">Tecket ID: " + supportResquests.TicketNo + "</span></strong><strong>" +
                    "<span style='font-size:19px;font-family:\"Arial\",sans-serif;'>]</span></strong><span style='font-family:\"Arial\",sans-serif;'>&nbsp; &nbsp;&nbsp;</span>" +
                    "<span dir=\"LTR\">" + supportResquests.Date + "</span><span style='font-family:\"Arial\",sans-serif;'>&nbsp;&nbsp;</span><span dir=\"LTR\"></span></p></strong>\r\n" +
                    "<div style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;border:none;border-bottom:solid windowtext 1.0pt;padding:0in 0in 1.0pt 0in;'>\r\n  " +
                    "  <p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;border:none;padding:0in;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>&nbsp;</span></p>\r\n</div>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>الرد الآلي &nbsp;</span><span style='font-family:\"Arial\",sans-serif;'>&nbsp;:&nbsp;</span><strong>" +
                    "<span style='font-size:19px;font-family:\"Arial\",sans-serif;'>[</span></strong><strong><span dir=\"LTR\" style=\"font-size:19px;line-height:107%;\">Tecket ID:  " + supportResquests.TicketNo + "</span><" +
                    "/strong><strong><span style='font-size:19px;font-family:\"Arial\",sans-serif;'>]</span></strong><span style='font-family:\"Arial\",sans-serif;'>&nbsp; &nbsp;</span>" +
                    "<strong><span dir=\"LTR\">" + supportResquests.Date + " &nbsp; </span></strong></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;color:red;'>&nbsp;</span></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<strong><span style='font-size:21px;font-family:\"Arial\",sans-serif;'>تم فتح التذكرة و جاري المتابعة والفحص وسيتم الرد عليك في أسرع وقت ممكن&nbsp;</span>" +
                    "</strong></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>&nbsp;</span></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>&nbsp;</span></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<strong><span style='font-family:\"Arial\",sans-serif;'>شكرا لتواصلكم مع الدعم الفني&nbsp;</span></strong></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<strong><span dir=\"LTR\">www.bayanatech.com.sa</span></strong></p>";

                var mail = new MailMessage();
               
                var loginInfo = new NetworkCredential("support@tameercloud.com", "aA4LQkrbQdCm5jqt@");

                if (_EmailSettingRepository.GetEmailSetting().Result.DisplayName != null)
                {
                    mail.From = new MailAddress("support@tameercloud.com", _EmailSettingRepository.GetEmailSetting().Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress("support@tameercloud.com", "لديك اشعار من نظام تعمير السحابي");
                }


                // mail.From = new MailAddress("support@tameercloud.com");
                // mail.To.Add(new MailAddress("ehab.r.sallam@gmail.com"));
                mail.To.Add(new MailAddress("noreply-tameer@bayanatech.com.sa"));
                mail.To.Add(new MailAddress(RequesterMail));
                mail.CC.Add(new MailAddress("tsupport@bayanatech.com.sa"));
                mail.CC.Add(new MailAddress("tsupport2@bayanatech.com.sa"));
                // mail.CC.Add(new MailAddress("mohammeddawoud66@gmail.com"));


                mail.Subject ="فتح تذكرة جديدة :" + supportResquests.Address  + "[" + "Ticket ID" +"  "+ supportResquests.TicketNo + "]"  ;
                try
                {
                    mail.Body = textBody;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = "Wrong message";
                }

                try
                {
                    if (supportResquests.AttachmentUrl != null)
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
                    return false;

                }



                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient("mail.bayanatech.com.sa");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Port = 587;
                //smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                var w = wx.Message;
                return false;
            }
        }


        public bool AutomationMail(SupportResquests supportResquests, int BranchId)
        {
            try
            {
               // var DateOfComplaint = supportResquests.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //var user = _UsersRepository.GetById(supportResquests.UserId);
                Users? user =  _TaamerProContext.Users.Where(s => s.UserId == supportResquests.UserId).FirstOrDefault();

                // var branch = _BranchesRepository.GetById(user.BranchId);
                Branch? branch =  _TaamerProContext.Branch.Where(s => s.BranchId == user.BranchId).FirstOrDefault();

                string textBody = "<p> تم فتح التذكرة و جاري المتابعة والفحص وسيتم الرد عليك في أسرع وقت ممكن</p> </br></br></br></br></br></br>  <p style='margin-top:10px;'>شكرا لتواصلكم مع الدعم الفني </p><p> www.bayanatech.com.sa </p> ";
                //textBody = textBody + "<br/>";
                //textBody = textBody + "تم ارسال هذه الرسالة بشكل آلي، الرجاء عدم الرد عليها";
                //textBody = textBody + "<br/>";
                //textBody = textBody + "-----------------------------------";
                //textBody = textBody + "<br/>";
                //textBody = textBody + "Disclaimer: This message and its attachment, if any, are confidential and may contain legally privileged information. If you are not the intended recipient, please contact the sender immediately and delete this message and its attachment, if any, from your system. You should not copy this message or disclose its contents to any other person or use it for any purpose. Statements and opinions expressed in this e-mail are those of the sender, and do not necessarily reflect those of bayanatech for IT services accepts no liability for damage caused by any virus transmitted by this email";
                //textBody = textBody + "<br/>";
                //textBody = textBody + "هذه الرسالة و مرفقاتها (إن وجدت) تمثل وثيقة سرية قد تحتوي على معلومات تتمتع بحماية وحصانة قانونية. إذا لم تكن الشخص المعني بهذه الرسالة يجب عليك تنبيه المُرسل بخطأ وصولها إليك، و حذف الرسالة و مرفقاتها (إن وجدت) من الحاسب الآلي الخاص بك. ولا يجوز لك نسخ هذه الرسالة أو مرفقاتها (إن وجدت) أو أي جزئ منها، أو البوح بمحتوياتها لأي شخص أو استعمالها لأي غرض. علماً بأن الإفادات و الآراء التي تحويها هذه الرسالة تعبر فقط عميل برنامج تعمير السحابي ،  و ليس بالضرورة رأي مؤسسة بياناتك لتقنية المعلومات ، ولا تتحمل مؤسسة بياناتك لتقنية المعلومات أي مسئولية عن الأضرار الناتجة عن أي فيروسات قد يحملها هذا البريد";
                //textBody = textBody + "<br/>";

                var mail = new MailMessage();

                var loginInfo = new NetworkCredential("support@tameercloud.com", "aA4LQkrbQdCm5jqt@");

                if (_EmailSettingRepository.GetEmailSetting().Result.DisplayName != null)
                {
                    mail.From = new MailAddress("support@tameercloud.com", _EmailSettingRepository.GetEmailSetting().Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress("support@tameercloud.com", "لديك اشعار من نظام تعمير السحابي");
                }


                //// mail.From = new MailAddress("support@bayanatech.com.sa");
                mail.To.Add(new MailAddress(user.Email));
               // mail.To.Add(new MailAddress("ehab.r.sallam@gmail.com"));
                //mail.To.Add(new MailAddress("info@bayanatech.com.sa"));
                //mail.To.Add(new MailAddress(RequesterMail));
                //mail.CC.Add(new MailAddress("tsupport@bayanatech.com.sa"));
                //mail.CC.Add(new MailAddress("tsupport2@bayanatech.com.sa"));
                //// mail.CC.Add(new MailAddress("mohammeddawoud66@gmail.com"));


                mail.Subject = "[" + "Ticket ID"+"  " + supportResquests.TicketNo + "]" + supportResquests.Date;
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

                var smtpClient = new SmtpClient("mail.bayanatech.com.sa");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Port = 587;
                //smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                var w = wx.Message;
                return false;
            }
        }

        public string DecryptValue(string value)
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




        public GeneralMessage SaveLabaikrequest(SupportResquests support, string attachment, string mobile)
        {
            try
            {
             
                    var uri = "https://localhost:44334/";
                    if (uri != null && uri != "")
                    {
                        //Generate Token
                        var token = getapitoken(uri);

                    var serviceobj = ServiceRequest(support, attachment, mobile);

                    var formData = new MultipartFormDataContent();

                    // Add the fields to the form data
                    formData.Add(new StringContent(serviceobj.ServiesName), "ServiesName");
                    formData.Add(new StringContent(serviceobj.visitDate), "visitDate");
                    formData.Add(new StringContent(serviceobj.VisitTime), "VisitTime");
                    formData.Add(new StringContent(serviceobj.ServiceType.ToString()), "ServiceType");
                    formData.Add(new StringContent(serviceobj.Priority.ToString()), "Priority");
                    formData.Add(new StringContent(serviceobj.RequeterMobileNumber), "RequeterMobileNumber");
                    formData.Add(new StringContent(serviceobj.RequeterMobileNumber), "postedFiles");


                    using (var client = new HttpClient())
                            {
                                //Base API URI
                                client.BaseAddress = new Uri(uri);
                                //JWT TOKEN
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                //HTTP POST API
                                //var responseTask = client.PostAsync("api/ServiceRequest/SaveServiceRequest", null);

                                                    var res = client.PostAsync("api/ServiceRequest/SaveServiceRequest",
                                 new StringContent(JsonConvert.SerializeObject(serviceobj ), Encoding.UTF8, "application/json") );
                                    res.Wait();
                        Console.WriteLine(res.Result);

                    }




                }
            }
            catch (Exception ex)
            {

            }
            return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.sent_succesfully };
        }



        public string getapitoken(string URI)
        {
            var result = "";
            try
            {
              
                        using (var client = new HttpClient())
                        {
                            //Base URI
                            client.BaseAddress = new Uri(URI);
                            //HTTP GET


                            //Http GET 
                            //Get Token API
                            var responseTask = client.GetAsync("api/Users/gettoken?userid=1");
                            responseTask.Wait();

                            var reslt = responseTask.Result;

                            result = reslt.Content.ReadAsStringAsync().Result;

                        }

                   
                
            }
            catch (Exception ex)
            {

            }
            return result;
        }


        public GeneralMessage UpdateSupportResquests(int Serviceid, string replay, int status,string SenderName,string SenderPhoto, int UserId, int BranchId,
            string AttachmentUrl=null)
        {
            var support = "";
            try
            {
              var service=_TaamerProContext.SupportResquests.Where(x=>x.RequestId== Serviceid).FirstOrDefault();
                if (service != null)
                {
                    support = service.Topic;
                    if (status != 0 && status != null)
                    {
                        service.Status= status;
                    }
                    if (replay != "" && replay != null)
                    {
                        service.Repaly= replay;
                        service.LastReplayDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.CreateSpecificCulture("en"));
                        service.LastReplayFrom = "2";
                        SupportRequestsReplay requestsReplay = new SupportRequestsReplay{
                            ServiceRequestId= Serviceid,
                            AddDate= DateTime.Now,
                            Contacttxt=replay,
                            SenderName= SenderName,
                            //SenderPhoto= SenderPhoto,
                            UserId= UserId,
                            ContactDate=DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.CreateSpecificCulture("en")),
                            ReplayFrom= "2",
                            AttachmentUrl= AttachmentUrl,


                        };
                        _TaamerProContext.RequestsReplays.Add(requestsReplay);
                    }
                }
                _TaamerProContext.SaveChanges();
                    //SaveLabaikrequest(supportResquests, AttachmentFile, organization.Mobile);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تعديل طلب الدعم" + support;
                    _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
               
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل طلب الدعم";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage UpdateSupportResquestsNo(int Serviceid, string TicketNo ,int UserId, int BranchId)
        {
            try
            {
                var service = _TaamerProContext.SupportResquests.Where(x => x.RequestId == Serviceid).FirstOrDefault();
                if (service != null)
                {
                    if ( TicketNo != null)
                    {
                        service.TicketNo = TicketNo;
                    }
                   
                }
                var organization = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false).FirstOrDefault();

                _TaamerProContext.SaveChanges();
               // SendMail(service, organization.BranchId.Value, UserId, VersionCode, OrgName, AttachmentFile, stat, user.Email);

                //SaveLabaikrequest(supportResquests, AttachmentFile, organization.Mobile);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل طلب الدعم" + TicketNo;
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل طلب الدعم";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage SaveRequestReplay(SupportRequestsReplay supportResquests, int UserId, int BranchId)
        {
            try
            {
              
                if (supportResquests.SupportRequestsReplayId == 0)
                {
                    supportResquests.UserId=UserId;
                    supportResquests.ContactDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.CreateSpecificCulture("en"));
                    supportResquests.AddUser = UserId;
                    supportResquests.AddDate = DateTime.Now;

                    _TaamerProContext.RequestsReplays.Add(supportResquests);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة طلب دعم جديد";
                    _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};
                }
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ  طلب الدعم";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage SaveRequestReplayFromTameer(int Serviceid, string replay, int UserId, int BranchId,string? AttachmentUrl=null)
        {
            var support = "";
            try
            {
                var service = _TaamerProContext.SupportResquests.Where(x => x.RequestId == Serviceid).FirstOrDefault();
                var Organization=_TaamerProContext.Organizations.FirstOrDefault();
                var User =_TaamerProContext.Users.Where(x=>x.UserId== UserId).FirstOrDefault(); 
                if (service != null)
                {
                    support = service.Topic;
                    if (replay != "" && replay != null)
                    {
                        service.Repaly = replay;
                        service.LastReplayDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.CreateSpecificCulture("en"));
                        service.LastReplayFrom = "1";
                        SupportRequestsReplay requestsReplay = new SupportRequestsReplay
                        {
                            ServiceRequestId = Serviceid,
                            AddDate = DateTime.Now,
                            Contacttxt = replay,
                            SenderName = User.FullNameAr,
                            SenderPhoto = Organization.TameerAPIURL + User.ImgUrl,
                            UserId = UserId,
                            ContactDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.CreateSpecificCulture("en")),
                            ReplayFrom="1",
                            AttachmentUrl= AttachmentUrl,


                        };
                        _TaamerProContext.RequestsReplays.Add(requestsReplay);
                    }
                }
                _TaamerProContext.SaveChanges();
                SendReplayMail(service, BranchId, Organization.NameAr, null, User.Email, replay, DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.CreateSpecificCulture("en")));
                //SaveLabaikrequest(supportResquests, AttachmentFile, organization.Mobile);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل طلب الدعم" + support;
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل طلب الدعم";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public Task<IEnumerable<SupportRequestsReplayVM>> GetAllReplyByServiceId(int RequestId)
        {
            var SupportRequest = _supportRequestsReplayRepository.GetAllReplyByServiceId(RequestId);

            return SupportRequest;
        }


        public GeneralMessage ReadReplay(int SupportReplayId, int UserId, int BranchId)
        {
            try
            {
                var service = _TaamerProContext.RequestsReplays.Where(x => x.SupportRequestsReplayId == SupportReplayId).FirstOrDefault();
                if (service != null)
                {
                    service.IsRead = true;

                }
                _TaamerProContext.SaveChanges();
                //SaveLabaikrequest(supportResquests, AttachmentFile, organization.Mobile);
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تعديل طلب الدعم"  + SupportReplayId;
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل طلب الدعم";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage Deleterequest(int Serviceid, int UserId, int BranchId)
        {
            try
            {
                var service = _TaamerProContext.SupportResquests.Where(x => x.RequestId == Serviceid).FirstOrDefault();
                if (service != null)
                {
                    service.IsDeleted=true;
                }

                _TaamerProContext.SaveChanges();
               //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حفظ الطلب ";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل طلب الدعم";
                _SystemAction.SaveAction("SaveSupportResquests", "SupportRequestsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public bool SendReplayMail(SupportResquests supportResquests, int BranchId, string OrgName, string AttachmentFile, string RequesterMail,string ReplayText,string ReplayDate)
        {
            try
            {
                //var DateOfComplaint = supportResquests.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                //var user = _UsersRepository.GetById(supportResquests.UserId);
                Users? user = _TaamerProContext.Users.Where(s => s.UserId == supportResquests.UserId).FirstOrDefault();

                // var branch = _BranchesRepository.GetById(user.BranchId);
                Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == user.BranchId).FirstOrDefault();

                //string textBody = 
                //    "<table border='1'style='text-align:center;padding:3px;'><tr><td style='border=1px solid #eee'>اسم المنشأة </td>" +
                //    "<td>" + OrgName + "</td></tr><tr><td>اسم العميل </td>" +
                //    "<td>" + user.FullNameAr + "</td></tr><tr><td>وصف الطلب </td><td>" + supportResquests.Topic + "</td></tr><tr><td> رقم التذكرة</td>" +
                //    "<td>"  +"[" + "Ticket ID" + supportResquests.TicketNo + "]" + "  " + supportResquests.Date + "</td></tr></table>";


                string textBody = "<div style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\"," +
                    "sans-serif;border:none;border-bottom:solid windowtext 1.0pt;padding:0in 0in 1.0pt 0in;'>\r\n    <p style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;" +
                    "margin-left:0in;text-align:left;font-size:21px;font-family:\"Calibri\",sans-serif;border:none;padding:0in;'>BAYANATECH</p>" +
                    "\r\n</div>\r\n<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>اسم المنشأة : &nbsp;" + OrgName + "</span></p></strong>\r\n" +
                    "<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>اسم العميل : &nbsp;" + user.FullNameAr + "&nbsp;</span></p></strong>\r\n" +
                    "<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>وصف الطلب: &nbsp; " + supportResquests.Topic + " &nbsp;</span></p></strong>\r\n" +
                    "<strong><p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>رقم التذكرة &nbsp;:&nbsp;</span><strong><span style='font-size:19px;font-family:\"Arial\",sans-serif;'>" +
                    "[</span></strong><strong><span dir=\"LTR\" style=\"font-size:19px;line-height:107%;\">Tecket ID: " + supportResquests.TicketNo + "</span></strong><strong>" +
                    "<span style='font-size:19px;font-family:\"Arial\",sans-serif;'>]</span></strong><span style='font-family:\"Arial\",sans-serif;'>&nbsp; &nbsp;&nbsp;</span>" +
                    "<span dir=\"LTR\">" + supportResquests.Date + "</span><span style='font-family:\"Arial\",sans-serif;'>&nbsp;&nbsp;</span><span dir=\"LTR\"></span></p></strong>\r\n" +
                    "<div style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;border:none;border-bottom:solid windowtext 1.0pt;padding:0in 0in 1.0pt 0in;'>\r\n  " +
                    "  <p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;border:none;padding:0in;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>&nbsp;</span></p>\r\n</div>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>رد العميل &nbsp;</span><span style='font-family:\"Arial\",sans-serif;'>&nbsp;:&nbsp;</span><strong>" +
                    "<span style='font-size:19px;font-family:\"Arial\",sans-serif;'></span></strong><strong><span dir=\"LTR\" style=\"font-size:19px;line-height:107%;\">تاريخ الرد:  " + ReplayDate + "</span><" +
                    "/strong><strong><span style='font-size:19px;font-family:\"Arial\",sans-serif;'></span></strong><span style='font-family:\"Arial\",sans-serif;'>&nbsp; &nbsp;</span>" +
                    "<strong><span dir=\"LTR\"> &nbsp; </span></strong></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;color:red;'>&nbsp;</span></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<strong><span style='font-size:21px;font-family:\"Arial\",sans-serif;'>"+ ReplayText + "&nbsp;</span>" +
                    "</strong></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>&nbsp;</span></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<span style='font-family:\"Arial\",sans-serif;'>&nbsp;</span></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<strong><span style='font-family:\"Arial\",sans-serif;'>شكرا لتواصلكم مع الدعم الفني&nbsp;</span></strong></p>\r\n<p dir=\"RTL\" style='margin-top:0in;margin-right:0in;margin-bottom:8.0pt;margin-left:0in;text-align:right;font-size:21px;font-family:\"Calibri\",sans-serif;'>" +
                    "<strong><span dir=\"LTR\">www.bayanatech.com.sa</span></strong></p>";

                var mail = new MailMessage();

                var loginInfo = new NetworkCredential("support@tameercloud.com", "aA4LQkrbQdCm5jqt@");

                if (_EmailSettingRepository.GetEmailSetting().Result.DisplayName != null)
                {
                    mail.From = new MailAddress("support@tameercloud.com", _EmailSettingRepository.GetEmailSetting().Result.DisplayName);
                }
                else
                {
                    mail.From = new MailAddress("support@tameercloud.com", "لديك اشعار من نظام تعمير السحابي");
                }


                // mail.From = new MailAddress("support@bayanatech.com.sa");
                //mail.To.Add(new MailAddress("ehab.r.sallam@gmail.com"));
                mail.To.Add(new MailAddress("noreply-tameer@bayanatech.com.sa"));
                mail.To.Add(new MailAddress(RequesterMail));
                mail.CC.Add(new MailAddress("tsupport@bayanatech.com.sa"));
                mail.CC.Add(new MailAddress("tsupport2@bayanatech.com.sa"));
                // mail.CC.Add(new MailAddress("mohammeddawoud66@gmail.com"));


                mail.Subject = "تحديث التذكرة :" + supportResquests.Address + "[" + "Ticket ID" + "  " + supportResquests.TicketNo + "]";
                try
                {
                    mail.Body = textBody;
                    mail.IsBodyHtml = true;
                }
                catch (Exception)
                {
                    mail.Body = "Wrong message";
                }

                try
                {
                    if (supportResquests.AttachmentUrl != null)
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
                    return false;

                }



                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient("mail.bayanatech.com.sa");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Port = 587;
                //smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Port);

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception wx)
            {
                var w = wx.Message;
                return false;
            }
        }




    }

    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public string? ServiesName { get; set; }
        public int? ServiceType { get; set; }
        public int? Priority { get; set; }
        public int? RequestStatus { get; set; }
        public string? VisitTime { get; set; }
        public string? visitDate { get; set; }
        public string? RequeterMobileNumber { get; set; }
        public string? RequeterMobileNumber2 { get; set; }
        public string? ServiceCode { get; set; }
        public string? TicketNumber { get; set; }

        public string? Note { get; set; }
        public string? AttachUrl { get; set; }

    }
}