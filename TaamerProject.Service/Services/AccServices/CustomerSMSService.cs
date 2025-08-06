using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
    public class CustomerSMSService : ICustomerSMSService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerSMSRepository _CustomerSMSRepository;
        private readonly ISMSSettingsRepository _sMSSettingsRepository;
        private readonly IWhatsAppSettingsRepository _whatsAppSettingsRepository;

        private readonly ICustomerRepository _CustomerRepository;

        public CustomerSMSService(TaamerProjectContext dataContext, ISystemAction systemAction,ICustomerSMSRepository customerSMSRepository, ISMSSettingsRepository sMSSettingsRepository
            , IWhatsAppSettingsRepository whatsAppSettingsRepository, ICustomerRepository customerRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CustomerSMSRepository = customerSMSRepository;
            _sMSSettingsRepository= sMSSettingsRepository;
            _whatsAppSettingsRepository = whatsAppSettingsRepository;
            _CustomerRepository = customerRepository;
        }
        public async Task<IEnumerable<CustomerSMSVM>> GetAllCustomerSMS(int BranchId)
        {
            var CustomerSMS =await _CustomerSMSRepository.GetAllCustomerSMS(BranchId);
            return CustomerSMS;
        }
        public async Task<IEnumerable<CustomerSMSVM>> GetSMSByCustomerId(int? CustomerId)
        {
            var CustomerSMS = await _CustomerSMSRepository.GetSMSByCustomerId(CustomerId);
            return CustomerSMS;
        }
        public GeneralMessage SaveCustomerSMS(CustomerSMS customerSMS, int UserId, int BranchId)
        {
            var cus = 0;

            try
            {
                if (customerSMS.AssignedCustomersSMSIds.Count() > 0)
                {
                    foreach (var CustomersmsId in customerSMS.AssignedCustomersSMSIds)
                    {
                        cus = CustomersmsId;
                        //SMS Providers
                        var smsSettings = _sMSSettingsRepository.GetsmsSetting(BranchId).Result;
                        if (smsSettings != null)
                        {
                            SMSProviders Provider = new SMSProviders();
                            string apiLink = smsSettings.ApiUrl;
                            string apiKey = smsSettings.Password; //"9REnB596OLw-Z8gA1w3S4F2QF1b6H3RtBQ4RWjRjDN";
                            string numbers = _CustomerRepository.GetById(CustomersmsId).CustomerMobile; // in a comma seperated list
                            string message = customerSMS.SMSText; //"This message is just to test TameerPro SMS Messaging Service";
                            string sender = smsSettings.SenderName; //"Tameer Pro Egypt Team";
                            string userName = smsSettings.UserName;

                            GeneralMessage result = null;

                            if (apiLink.Contains("taqnyat"))
                            {
                                result = Provider.Connect_Taqnyat(apiKey, sender, apiKey, numbers, message);
                            }
                            else if (apiLink.Contains("4jawaly"))
                            {
                                result = Provider.Connect_4Jawaley(apiLink, userName, apiKey, sender, numbers, message);
                            }
                            else if (apiLink.Contains("msegat"))
                            {
                                var baseAddress = new Uri("https://www.msegat.com");

                                if (numbers.Substring(0, 1) == "0")
                                    numbers = numbers.Substring(1, numbers.Length - 1);

                                var httpClient = new HttpClient { BaseAddress = baseAddress };

                                string contectStr = @"{" +
                                      "\"userName\": \"" + userName + "\"," +
                                      "\"numbers\": \"966" + numbers + "\"," +
                                      "\"userSender\": \"" + sender + "\"," +
                                      "\"apiKey\": \"" + apiKey + "\"," +
                                      "\"msg\": \"" + message + "\"," +
                                      "\"msgEncoding\": \"UTF8\"" +
                                    "}";
                                var content = new StringContent(contectStr); //, System.Text.Encoding.Default, "application/json");

                                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                                var response = httpClient.PostAsync("/gw/sendsms.php", content).Result;

                                string responseHeaders = response.Headers.ToString();
                                string responseData = response.Content.ReadAsStringAsync().Result;

                                string[] resData = responseData.Split('"');

                                if ((int)response.StatusCode == 200 && resData[3] == "1")
                                {
                                    //insert sms to our system
                                    result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
                                }
                                else
                                    result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = resData[7] };
                                //result = new GeneralMessage() { Result = true, Message = Resources.sent_succesfully }; //Provider.Connect_Mesegat(userName, numbers, sender, apiKey, message);
                            }
                            else if (apiLink.Contains("gateway"))
                            {
                                result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "" };//Provider.Connect_gateway(userName, apiKey, numbers, sender, message);
                            }
                            else if (apiLink.Contains("twilio"))
                            {
                                result = Provider.Connect_Twilio(userName, apiKey, smsSettings.MobileNo, numbers, message);
                            }
                            else
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote2 = "فشل في ارسال الرسالة الي" + _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == cus).CustomerNameAr ;
                              _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1,Resources.messaging_service_settings, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.messaging_service_settings };
                            }

                            ///////////////////////////////////////////////

                            var CustomerSMSObj = new CustomerSMS();
                            CustomerSMSObj.CustomerId = CustomersmsId;
                            CustomerSMSObj.SenderUser = UserId;
                            CustomerSMSObj.SMSSubject = customerSMS.SMSSubject;
                            CustomerSMSObj.SMSText = customerSMS.SMSText;
                            CustomerSMSObj.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                            CustomerSMSObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                            CustomerSMSObj.AddUser = UserId;
                            CustomerSMSObj.BranchId = BranchId;
                            CustomerSMSObj.AddDate = DateTime.Now;
                            _TaamerProContext.CustomerSMS.Add(CustomerSMSObj);

                            _TaamerProContext.SaveChanges();
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = " ارسال الرسالة جديد الي" + _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == cus).CustomerNameAr;
                            _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, result.ReasonPhrase, "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = result.ReasonPhrase };
                            //String url = "https://api.txtlocal.com/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
                            ////refer to parameters to complete correct url string

                            //StreamWriter myWriter = null;
                            //HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

                            //objRequest.Method = "POST";
                            //objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                            //objRequest.ContentType = "application/x-www-form-urlencoded";

                            //myWriter = new StreamWriter(objRequest.GetRequestStream());
                            //myWriter.Write(url);




                            //HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                            //using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                            //{
                            //    result = sr.ReadToEnd();
                            //    // Close and clean up the StreamReader
                            //    sr.Close();
                            //}
                            //return result;
                            // TwilioClient.Init(Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"), Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN"));
                            // MessageResource.Create(to: new PhoneNumber("+0201008479533"), from: new PhoneNumber("+0201008479533"), body: "This message is just to test TameerPro SMS Messaging Service");
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال الرسالة الب" + _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == cus).CustomerNameAr;
                            _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1,Resources.messaging_service_settings, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                            return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.messaging_service_settings };

                        }
                    }
                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote5 = "فشل في ارسال الرسالة الي" + _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == cus).CustomerNameAr;
                    _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, Resources.no_message_recipients, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.no_message_recipients };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote6 = "فشل في ارسال الرسالة";
               _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, Resources.faild_messaging_service_settings, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.faild_messaging_service_settings };
            }
            //-----------------------------------------------------------------------------------------------------------------
            string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            string ActionNote = "فشل في ارسال الرسالة الي" + _TaamerProContext.Customer.FirstOrDefault(x => x.CustomerId == cus).CustomerNameAr;
            _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, Resources.faild_messaging_service_settings, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
            //-----------------------------------------------------------------------------------------------------------------

            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.faild_messaging_service_settings };
        }
        public GeneralMessage SaveCustomerSMS_Notification(string ReceiveNumber, string Message, int UserId, int BranchId)
        {
            if (!string.IsNullOrEmpty(ReceiveNumber))
            {
                try
                {
                    //SMS Providers
                    var smsSettings = _sMSSettingsRepository.GetsmsSetting(BranchId).Result;
                    if (smsSettings != null)
                    {
                        SMSProviders Provider = new SMSProviders();
                        string apiLink = smsSettings.ApiUrl;
                        string apiKey = smsSettings.Password; //"9REnB596OLw-Z8gA1w3S4F2QF1b6H3RtBQ4RWjRjDN";
                        string numbers = ReceiveNumber; // in a comma seperated list
                        string message = Message; //"This message is just to test TameerPro SMS Messaging Service";
                        string sender = smsSettings.SenderName; //"Tameer Pro Egypt Team";
                        string userName = smsSettings.UserName;

                        GeneralMessage result = null;

                        if (apiLink.Contains("taqnyat"))
                        {
                            result = Provider.Connect_Taqnyat(apiKey, sender, apiKey, numbers, message);
                        }
                        else if (apiLink.Contains("4jawaly"))
                        {
                            result = Provider.Connect_4Jawaley(apiLink, userName, apiKey, sender, numbers, message);
                        }
                        else if (apiLink.Contains("msegat"))
                        {
                            var baseAddress = new Uri("https://www.msegat.com");

                            if (numbers.Substring(0, 1) == "0")
                                numbers = numbers.Substring(1, numbers.Length - 1);

                            var httpClient = new HttpClient { BaseAddress = baseAddress };

                            string contectStr = @"{" +
                                  "\"userName\": \"" + userName + "\"," +
                                  "\"numbers\": \"966" + numbers + "\"," +
                                  "\"userSender\": \"" + sender + "\"," +
                                  "\"apiKey\": \"" + apiKey + "\"," +
                                  "\"msg\": \"" + message + "\"," +
                                  "\"msgEncoding\": \"UTF8\"" +
                                "}";
                            var content = new StringContent(contectStr); //, System.Text.Encoding.Default, "application/json");

                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                            var response = httpClient.PostAsync("/gw/sendsms.php", content).Result;

                            string responseHeaders = response.Headers.ToString();
                            string responseData = response.Content.ReadAsStringAsync().Result;

                            string[] resData = responseData.Split('"');

                            if ((int)response.StatusCode == 200 && resData[3] == "1")
                            {
                                //insert sms to our system
                                result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
                            }
                            else
                                result = new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = resData[7] };
                            //result = new GeneralMessage() { Result = true, Message = Resources.sent_succesfully }; //Provider.Connect_Mesegat(userName, numbers, sender, apiKey, message);
                        }
                        else if (apiLink.Contains("gateway"))
                        {
                            result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "" };//Provider.Connect_gateway(userName, apiKey, numbers, sender, message);
                        }
                        else if (apiLink.Contains("twilio"))
                        {
                            result = Provider.Connect_Twilio(userName, apiKey, smsSettings.MobileNo, numbers, message);
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في إرسال الرسالة (إشعارات)" + ReceiveNumber;
                           _SystemAction.SaveAction("SaveCustomerSMS_Notification", "CustomerSMSService", 1,Resources.messaging_service_settings, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =Resources.messaging_service_settings };
                        }

                        ///////////////////////////////////////////////

                        //var CustomerSMSObj = new CustomerSMS();
                        //CustomerSMSObj.CustomerId = CustomersmsId;
                        //CustomerSMSObj.SenderUser = UserId;
                        //CustomerSMSObj.SMSSubject = customerSMS.SMSSubject;
                        //CustomerSMSObj.SMSText = customerSMS.SMSText;
                        //CustomerSMSObj.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                        //CustomerSMSObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                        //CustomerSMSObj.AddUser = UserId;
                        //CustomerSMSObj.BranchId = BranchId;
                        //CustomerSMSObj.AddDate = DateTime.Now;
                        //_CustomerSMSRepository.Add(CustomerSMSObj);

                        //_uow.SaveChanges();

                        if (result.StatusCode==HttpStatusCode.OK)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = " ارسال الرسالة (إشعارات) جديد" + ReceiveNumber;
                           _SystemAction.SaveAction("SaveCustomerSMS_Notification", "CustomerSMSService", 1, result.ReasonPhrase, "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
                            //-----------------------------------------------------------------------------------------------------------------
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote6 = "(إشعارات) فشل في ارسال الرسالة" + ReceiveNumber;
                            _SystemAction.SaveAction("SaveCustomerSMS_Notification", "CustomerSMSService", 1, result.ReasonPhrase, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }
                        return result;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في ارسال الرسالة (إشعارات)" + ReceiveNumber;
                        _SystemAction.SaveAction("SaveCustomerSMS_Notification", "CustomerSMSService", 1,Resources.messaging_service_settings, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =Resources.messaging_service_settings };

                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote6 = "(إشعارات) فشل في ارسال الرسالة" + ReceiveNumber;
                    _SystemAction.SaveAction("SaveCustomerSMS_Notification", "CustomerSMSService", 1, Resources.faild_messaging_service_settings, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.faild_messaging_service_settings };
                }
            }
            else
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.numberDoesNotExist };
        }

        public GeneralMessage SendWhatsApp_Notification(string ReceiveNumber, string Message, int UserId, int BranchId, string environmentURL, string PDFURL)
        {
            if (!string.IsNullOrEmpty(ReceiveNumber))
            {
                try
                {
                    //WhatsApp Providers
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

                            return  new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.messaging_service_settings };
                        }

                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote3 = " ارسال الرسالة (إشعارات) جديد" + ReceiveNumber;
                            _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, result.ReasonPhrase, "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
                            //-----------------------------------------------------------------------------------------------------------------
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote6 = "(إشعارات) فشل في ارسال الرسالة" + ReceiveNumber;
                            _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, result.ReasonPhrase, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                            //-----------------------------------------------------------------------------------------------------------------
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

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.faild_messaging_service_settings };
                }
            }
            else
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.numberDoesNotExist };
        }

        public async Task<int> SendWhatsApp_Notification_Task(string ReceiveNumber, string Message, int UserId, int BranchId, string environmentURL, string PDFURL)
        {
            if (!string.IsNullOrEmpty(ReceiveNumber))
            {
                try
                {
                    //WhatsApp Providers
                    var whatsAppSettings = _whatsAppSettingsRepository.GetWhatsAppSetting(BranchId).Result;
                    if (whatsAppSettings != null)
                    {
                        SMSProviders Provider = new SMSProviders();
                        var result = Task.FromResult(2);

                        if (whatsAppSettings.TypeName.Contains("ultramsg"))
                        {
                            result = Provider.SendWhatsApp_UltraMsg_Task(whatsAppSettings.InstanceId, whatsAppSettings.Token, whatsAppSettings.MobileNo, ReceiveNumber, Message, environmentURL, PDFURL);
                        }
                        else if (whatsAppSettings.TypeName.Contains("twilio"))
                        {
                            result = Provider.SendWhatsApp_Twilio_Task(whatsAppSettings.InstanceId, whatsAppSettings.Token, whatsAppSettings.MobileNo, ReceiveNumber, Message, environmentURL, PDFURL);
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في إرسال الرسالة (إشعارات)" + ReceiveNumber;
                            _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, Resources.messaging_service_settings, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------

                            return 2;
                        }
                        return await result;
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في ارسال الرسالة (إشعارات)" + ReceiveNumber;
                        _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, Resources.messaging_service_settings, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return 2;

                    }
                }
                catch (Exception ex)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote6 = "(إشعارات) فشل في ارسال الرسالة" + ReceiveNumber;
                    _SystemAction.SaveAction("SendWhatsApp_Notification", "CustomerSMSService", 1, Resources.faild_messaging_service_settings, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return 2;
                }
            }
            else
                return 2;
        }

        public GeneralMessage DeleteCustomerSMS(int SMSId, int UserId, int BranchId)
        {
            try
            {
                CustomerSMS customerSMS = _TaamerProContext.CustomerSMS.Where(x=>x.SMSId==SMSId).FirstOrDefault();

                customerSMS.IsDeleted = true;
                customerSMS.DeleteDate = DateTime.Now;
                customerSMS.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف رسالة رقم " + SMSId;
                _SystemAction.SaveAction("DeleteCustomerSMS", "CustomerSMSService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف رسالة رقم " + SMSId; ;
                _SystemAction.SaveAction("DeleteCustomerSMS", "CustomerSMSService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }




        public GeneralMessage SaveCustomerSMS2(CustomerSMS customerSMS, int UserId, int BranchId)
        {
            try
            {
                GeneralMessage result = null;
                if (customerSMS.AssignedCustomersSMSIds.Count() > 0)
                {
                    var smsSettings = _sMSSettingsRepository.GetsmsSetting(BranchId).Result;
                    if (smsSettings != null)
                    {
                        foreach (var CustomersmsId in customerSMS.AssignedCustomersSMSIds)
                        {
                            //SMS Providers

                            SMSProviders Provider = new SMSProviders();
                            string apiLink = smsSettings.ApiUrl;
                            string apiKey = smsSettings.Password; //"9REnB596OLw-Z8gA1w3S4F2QF1b6H3RtBQ4RWjRjDN";
                            string numbers = _CustomerRepository.GetById(CustomersmsId).CustomerMobile; // in a comma seperated list
                            if (_CustomerRepository.GetById(CustomersmsId).CustomerMobile != null)
                            {
                                string message = customerSMS.SMSText; //"This message is just to test TameerPro SMS Messaging Service";
                                string sender = smsSettings.SenderName; //"Tameer Pro Egypt Team";
                                string userName = smsSettings.UserName;



                                if (apiLink.Contains("taqnyat"))
                                {
                                    result = Provider.Connect_Taqnyat(apiKey, sender, apiKey, numbers, message);
                                }
                                else if (apiLink.Contains("4jawaly"))
                                {
                                    result = Provider.Connect_4Jawaley(apiLink, userName, apiKey, sender, numbers, message);
                                }
                                else if (apiLink.Contains("msegat"))
                                {
                                    var baseAddress = new Uri("https://www.msegat.com");

                                    if (numbers.Substring(0, 1) == "0")
                                        numbers = numbers.Substring(1, numbers.Length - 1);

                                    var httpClient = new HttpClient { BaseAddress = baseAddress };

                                    string contectStr = @"{" +
                                          "\"userName\": \"" + userName + "\"," +
                                          "\"numbers\": \"966" + numbers + "\"," +
                                          "\"userSender\": \"" + sender + "\"," +
                                          "\"apiKey\": \"" + apiKey + "\"," +
                                          "\"msg\": \"" + message + "\"," +
                                          "\"msgEncoding\": \"UTF8\"" +
                                        "}";
                                    var content = new StringContent(contectStr); //, System.Text.Encoding.Default, "application/json");

                                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                                    var response = httpClient.PostAsync("/gw/sendsms.php", content).Result;

                                    string responseHeaders = response.Headers.ToString();
                                    string responseData = response.Content.ReadAsStringAsync().Result;

                                    string[] resData = responseData.Split('"');

                                    if ((int)response.StatusCode == 200 && (resData[3] == "1" || resData[3] == "1061"))
                                    {
                                        //insert sms to our system
                                        result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.sent_succesfully };
                                    }
                                    else
                                        result = new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = resData[7] };
                                    //result = new GeneralMessage() { Result = true, Message = Resources.sent_succesfully }; //Provider.Connect_Mesegat(userName, numbers, sender, apiKey, message);
                                }
                                else if (apiLink.Contains("gateway"))
                                {
                                    result = new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "" };//Provider.Connect_gateway(userName, apiKey, numbers, sender, message);
                                }
                                else if (apiLink.Contains("twilio"))
                                {
                                    result = Provider.Connect_Twilio(userName, apiKey, smsSettings.MobileNo, numbers, message);
                                }
                                else
                                {
                                    //-----------------------------------------------------------------------------------------------------------------
                                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                    string ActionNote2 = "فشل في ارسال الرسالة";
                                    _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1,Resources.messaging_service_settings, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                                    //-----------------------------------------------------------------------------------------------------------------

                                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase =Resources.messaging_service_settings };
                                }

                                ///////////////////////////////////////////////

                                var CustomerSMSObj = new CustomerSMS();
                                CustomerSMSObj.CustomerId = CustomersmsId;
                                CustomerSMSObj.SenderUser = UserId;
                                CustomerSMSObj.SMSSubject = customerSMS.SMSSubject;
                                CustomerSMSObj.SMSText = customerSMS.SMSText;
                                CustomerSMSObj.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));
                                CustomerSMSObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")); ;
                                CustomerSMSObj.AddUser = UserId;
                                CustomerSMSObj.BranchId = BranchId;
                                CustomerSMSObj.AddDate = DateTime.Now;
                                _TaamerProContext.CustomerSMS.Add(CustomerSMSObj);

                                _TaamerProContext.SaveChanges();
                            }
                        }
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = " ارسال الرسالة جديد";
                        _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, result.ReasonPhrase, "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = result.ReasonPhrase };
                        //String url = "https://api.txtlocal.com/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
                        ////refer to parameters to complete correct url string

                        //StreamWriter myWriter = null;
                        //HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

                        //objRequest.Method = "POST";
                        //objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                        //objRequest.ContentType = "application/x-www-form-urlencoded";

                        //myWriter = new StreamWriter(objRequest.GetRequestStream());
                        //myWriter.Write(url);




                        //HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                        //using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                        //{
                        //    result = sr.ReadToEnd();
                        //    // Close and clean up the StreamReader
                        //    sr.Close();
                        //}
                        //return result;
                        // TwilioClient.Init(Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"), Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN"));
                        // MessageResource.Create(to: new PhoneNumber("+0201008479533"), from: new PhoneNumber("+0201008479533"), body: "This message is just to test TameerPro SMS Messaging Service");
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في ارسال الرسالة";
                        _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1,Resources.messaging_service_settings, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =Resources.messaging_service_settings };

                    }

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote5 = "فشل في ارسال الرسالة";
                    _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, Resources.no_message_recipients, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.no_message_recipients };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote6 = "فشل في ارسال الرسالة";
                _SystemAction.SaveAction("SaveCustomerSMS", "CustomerSMSService", 1, Resources.faild_messaging_service_settings, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.faild_messaging_service_settings };
            }
           
        }

        //private bool SendMail(CustomerSMS customerSMS, int BranchId, int UserId)
        //{
        //    try
        //    {
        //        DateTime date = new DateTime();
        //        var DateOfComplaint = customerSMS.AddDate.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));


        //        string textBody = " <div>"+ customerSMS.SMSText +" in"+ DateOfComplaint + "</div>" ;
        //        textBody = textBody + "<br/>";
        //        textBody = textBody + "تم ارسال هذه الرسالة بشكل آلي، الرجاء عدم الرد عليها";
        //        textBody = textBody + "<br/>";
        //        textBody = textBody + "-----------------------------------";
        //        textBody = textBody + "<br/>";
        //        textBody = textBody + "Disclaimer: This message and its attachment, if any, are confidential and may contain legally privileged information. If you are not the intended recipient, please contact the sender immediately and delete this message and its attachment, if any, from your system. You should not copy this message or disclose its contents to any other person or use it for any purpose. Statements and opinions expressed in this e-mail are those of the sender, and do not necessarily reflect those of bayanatech for IT services accepts no liability for damage caused by any virus transmitted by this email";
        //        textBody = textBody + "<br/>";
        //        textBody = textBody + "هذه الرسالة و مرفقاتها (إن وجدت) تمثل وثيقة سرية قد تحتوي على معلومات تتمتع بحماية وحصانة قانونية. إذا لم تكن الشخص المعني بهذه الرسالة يجب عليك تنبيه المُرسل بخطأ وصولها إليك، و حذف الرسالة و مرفقاتها (إن وجدت) من الحاسب الآلي الخاص بك. ولا يجوز لك نسخ هذه الرسالة أو مرفقاتها (إن وجدت) أو أي جزئ منها، أو البوح بمحتوياتها لأي شخص أو استعمالها لأي غرض. علماً بأن الإفادات و الآراء التي تحويها هذه الرسالة تعبر فقط عميل برنامج تعمير السحابي ،  و ليس بالضرورة رأي مؤسسة بياناتك لتقنية المعلومات ، ولا تتحمل مؤسسة بياناتك لتقنية المعلومات أي مسئولية عن الأضرار الناتجة عن أي فيروسات قد يحملها هذا البريد";
        //        textBody = textBody + "<br/>";

        //        var mail = new MailMessage();
        //        //var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail, _EmailSettingRepository.GetEmailSetting(branch).Password);
        //        var loginInfo = new NetworkCredential("support@bayanatech.com.sa", "noreply-tameer2030");

        //        //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
        //        mail.From = new MailAddress("support@bayanatech.com.sa");
        //        //mail.From = new MailAddress("support@bayanatech.com.sa");
        //        //mail.To.Add(new MailAddress(_UsersRepository.GetById(ProjectPhasesTasks.UserId).Email));
        //        mail.To.Add(new MailAddress("info@bayanatech.com.sa"));

        //        mail.Subject = customerSMS.SMSSubject;
        //        try
        //        {
        //            mail.Body = textBody;// "لديك مهمه جديدة : " + ProjectPhasesTasks.DescriptionAr + ":" + ProjectPhasesTasks.Notes + " علي مشروع رقم " + ProjectPhasesTasks.Project.ProjectNo + " للعميل " + ProjectPhasesTasks.Project.customer.CustomerNameAr;
        //            mail.IsBodyHtml = true;
        //        }
        //        catch (Exception)
        //        {
        //            mail.Body = "Wrong message";
        //        }



        //        var smtpClient = new SmtpClient("mail.bayanatech.com.sa", 587);
        //        //smtpClient.Timeout = 100000;
        //        //var smtpClient= new SmtpClient("smtp.gmail.com", 587);
        //        //var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Host, Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch).Port));//("smtp.gmail.com", 587);
        //        //smtpClient.EnableSsl = true;
        //        //smtpClient.Credentials = loginInfo;
        //        //smtpClient.Send(mail);
        //        //var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch).Host);
        //        smtpClient.EnableSsl = true;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = loginInfo;
        //        smtpClient.Send(mail);
        //        return true;
        //    }
        //    catch (Exception wx)
        //    {
        //        var w = wx.Message;
        //        return false;
        //    }
        //}
    }
}
