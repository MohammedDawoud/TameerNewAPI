using System.Net;
using System.Text;
using TaamerProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SMSManager;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using TaamerP.Service.LocalResources;
using Twilio.Types;
using System.Security.Policy;
using RestSharp;
using Twilio.TwiML.Messaging;

namespace TaamerProject.Service.Services
{
    public class SMSProviders
    {
        public SMSProviders() { }
        public GeneralMessage Connect_4Jawaley(string apiUrl, string UserName, string Password, string SenderName, string Numbers, string Message, bool RemoveDuplicated = true)
        {
            bool result = false;
            string Msg = "";

            Provider.Domain = apiUrl;
            Provider.Connect();
            if (Provider.CanContinue)
            {
                User.UserName = UserName;
                User.Password = Password;
                if (Functions.CheckUser())
                {
                    string Msgresult = SMS.Send(Numbers, SenderName, Message, RemoveDuplicated.ToString());
                    StringReader strReader = new StringReader(Msgresult);
                    DataSet ds = new DataSet();
                    ds.ReadXml(strReader);
                    DataTable dt = ds.Tables[0];

                    Msg = dt.Rows[0]["MessageIs"].ToString();
                    if (dt.Rows[0]["Code"].ToString() != "100")
                        result = false;
                    else
                        result = true;
                }
                else
                {
                    result = false;
                    Msg = "بيانات المستخدم غير صحيحة";
                }
            }
            else
                Msg = "لم نستطع الاتصال بمقدم الخدمة";
            result = false;
            return new GeneralMessage() { StatusCode = result==true? HttpStatusCode.OK: HttpStatusCode.BadRequest, ReasonPhrase = Msg };
        }
        public GeneralMessage Connect_Taqnyat(string apiUrl, string SenderName, string bearerTokens, string Numbers, string Message)
        {
            Taqnyat.Taqnyat taqnyt = new Taqnyat.Taqnyat();
            string bearer = bearerTokens;
            string body = Message;
            string recipients = Numbers;
            string sender = SenderName;


            var message = taqnyt.SendMessage(bearer, recipients, sender, body);
            string[] arr = message.Split(',');
            string msg = arr[1].Replace("\"message\":\"", "").Replace("\"}", "");
            if (arr[0].Replace("statusCode" + ":", "").Contains("200") || arr[0].Replace("statusCode" + ":", "").Contains("201"))
                return new GeneralMessage() {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.sent_succesfully };
            else
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = msg };
        }

        public GeneralMessage Connect_Mesegat(string UserName, string Numbers, string UserSender, string ApiKey, string Message)
        {
            var baseAddress = new Uri("https://www.msegat.com");
            var httpClient = new HttpClient { BaseAddress = baseAddress };

            //using var httpClient = new HttpClient { BaseAddress = baseAddress };
            //{
            string ContentString =
                @"{ 
                  'userName': '" + UserName + @"', 
                  'numbers': '" + Numbers + @"',                  
                  'userSender': '" + UserSender + @"',
                  'apiKey': '" + ApiKey + @"',
                  'msg': '" + Message + "'}";

            using (var content = new StringContent(ContentString, System.Text.Encoding.Default, "application/json"))
            {
                using (var response = httpClient.PostAsync("/gw/sendsms.php", content))
                {

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
                    string responseHeaders = response.Result.Headers.ToString();
                    string responseData = response.Result.Content.ReadAsStringAsync().Result;

                    if ((int)response.Result.StatusCode == 200)
                        return new GeneralMessage() {StatusCode = HttpStatusCode.OK,ReasonPhrase = "تم الإرسال بنجاح" };
                    else
                        return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "فشل في الإرسال" };

                    //Console.WriteLine("Status " + (int)response.Result.StatusCode);
                    //    Console.WriteLine("Headers " + responseHeaders);
                    //    Console.WriteLine("Data " + responseData);
                }
            }
            //}
        }
        public GeneralMessage Connect_gateway(string UserName, string Password, string Numbers, string UserSender, string Message)
        {
            string baseAddress = "https://apps.gateway.sa/vendorsms/pushsms.aspx?user=" + UserName + "&password=" + Password +
                "&msisdn=" + Numbers + "&sid=" + UserSender + "&msg=" + Message + "&fl=0";

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(baseAddress);

            var byteStr = Encoding.UTF8.GetBytes(baseAddress);
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(baseAddress);
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                using (var str = objRequest.GetRequestStream())
                    str.Write(byteStr, 0, byteStr.Length);
            }
            catch (Exception e)
            {
                // Log the exception.
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = "فشل في الارسال" };
            }
            //myWriter = new StreamWriter(objRequest.GetRequestStream());
            //myWriter.Write(baseAddress);

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {

                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }
            string[] ErrCode = result.Split(',');
            if (ErrCode[0].Contains("000"))
                return new GeneralMessage() {StatusCode = HttpStatusCode.OK,ReasonPhrase = "تم الإرسال بنجاح" };
            else
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = ErrCode[1].Replace("\"ErrorMessage\":\"", "").Replace("\"", "") };
        }

        public GeneralMessage Connect_Twilio(string AccountId, string AuthToken, string sendPhoneNumber, string RecievePhoneNumber, string Message) 
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
            string accountSid = Environment.GetEnvironmentVariable(AccountId);
            string authToken = Environment.GetEnvironmentVariable(AuthToken);

            TwilioClient.Init(AccountId, AuthToken);
          
            var message = MessageResource.Create(
                body: Message,
                from: new Twilio.Types.PhoneNumber(sendPhoneNumber),
                to: new Twilio.Types.PhoneNumber(RecievePhoneNumber)
            );

            //Console.WriteLine(message.Sid);
            if (message.Status == MessageResource.StatusEnum.Failed)
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = message.ErrorMessage };
            else
                return new GeneralMessage() {StatusCode = HttpStatusCode.OK,ReasonPhrase = string.IsNullOrEmpty(message.ErrorMessage)? "تم الإرسال بنجاح": message.ErrorMessage };
        }

        public GeneralMessage SendWhatsApp_Twilio(string AccountId, string AuthToken, string sendPhoneNumber, string RecievePhoneNumber, string Message, string environmentURL, string PDFURL)
        {

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
            try
            {


                //var accountSid = "AC21b75a6b7a6020096317985cb7fc0781";
                //var authToken = "51083bb163926b234a2e39cd1cca4229";
                string accountSid2 = Environment.GetEnvironmentVariable(AccountId);
                string authToken2 = Environment.GetEnvironmentVariable(AuthToken);
                var accountSid = AccountId;
                var authToken = AuthToken;

                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                //new PhoneNumber("whatsapp:+201020412606"));
                //new PhoneNumber("whatsapp:" + "+20" + RecievePhoneNumber));
                new PhoneNumber("whatsapp:" + "+966" + RecievePhoneNumber));

                //new PhoneNumber("whatsapp:+966503326610"));
                //new PhoneNumber("whatsapp:" + "+966" + RecievePhoneNumber));

                //messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                messageOptions.From = new PhoneNumber("whatsapp:" + sendPhoneNumber);

                messageOptions.Body = Message;

                if(PDFURL!="")
                {
                    var URLC = environmentURL + PDFURL;
                    //Uri uri = new Uri("https://api.tameercloud.com/TempFiles/NewInvoice.pdf");
                    //Uri uri = new Uri("https://api.tameercloud.com/TempFiles/OldInvoice.pdf");
                    Uri uri = new Uri(URLC);
                    messageOptions.MediaUrl.Add(uri);
                }
                else
                {
                    //string startupPath = Environment.CurrentDirectory;
                    string imageFilePathLogo = environmentURL + "/Reports/PdfTest.pdf";
                    Uri uri = new Uri(imageFilePathLogo);
                    //Uri uri = new Uri("https://api.tameercloud.com/TempFiles/NewInvoice.pdf");

                    messageOptions.MediaUrl.Add(uri);
                }

                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                GeneralMessage msg = new GeneralMessage();

                if (message.Status == MessageResource.StatusEnum.Failed)
                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = message.ErrorMessage };
                else
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الارسال بنجاح" };
            }
            catch (Exception ex)
            {
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الإرسال" };
            }
        }
        public GeneralMessage SendWhatsApp_UltraMsg(string AccountId, string AuthToken, string sendPhoneNumber, string RecievePhoneNumber, string Message, string environmentURL, string PDFURL)
        {

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
            try
            {
                //var File= "https://api.tameercloud.com/TempFiles/OldInvoice.pdf";
                var File = "https://api.tameercloud.com/TempFiles/NewInvoice.pdf";
                var File2 = "";


                if (PDFURL != "")
                {
                    File2 = environmentURL + PDFURL;
                }
                else
                {
                    File2 = environmentURL + "/Reports/PdfTest.pdf";
                }

                //string instanceId = "instance85943"; // your instanceId
                //string token = "y0znyu4yfdkifzfs";         //instance Token
                //string mobile = "+20" + "1020412606";


                //string instanceId = "instance85270"; // your instanceId
                //string token = "749v41rrvo6cs27f";         //instance Token
                //string mobile = "+20" + "1020412606";

                string instanceId = AccountId;
                string token = AuthToken;
                string mobile = "+966" + RecievePhoneNumber;


                var url = "https://api.ultramsg.com/" + instanceId + "/messages/document";
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", token);
                request.AddParameter("to", mobile);
                request.AddParameter("filename", "File.pdf");
                request.AddParameter("document", File2);
                //request.AddParameter("caption", "فاتورة ضريبية \r\n شكرا لك");
                request.AddParameter("caption", Message);


                RestResponse response = client.ExecuteAsync(request).Result;
                var output = response.Content;
                Console.WriteLine(output);
                var ResultError=response!.Content!.Contains("error");
                if (response.StatusCode == HttpStatusCode.OK && ResultError==false)
                    return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الارسال بنجاح" };
                else if (response.StatusCode == HttpStatusCode.OK && ResultError == true)
                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = response.Content.ToString() };
                else
                    return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = response.ErrorMessage };


            }
            catch (Exception ex)
            {
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الإرسال" };
            }
        }
        public GeneralMessage SendWhatsApp_UltraMsg()
        {
            try
            {
                // Your instance ID and token from UltraMsg
                string instanceId = "instance85943";
                string token = "y0znyu4yfdkifzfs";

                // The API URL
                string apiUrl = $"https://api.ultramsg.com/{instanceId}/messages/chat";

                // The message parameters
                var messageParams = new
                {
                    to = "01144289894",
                    body = "Hello, this is a test message from UltraMsg!"
                };

                // Create a new RestClient and RestRequest
                var client = new RestClient(apiUrl);
                var request = new RestRequest(apiUrl, Method.Post);

                // Add headers
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);

                // Add the JSON body
                request.AddJsonBody(messageParams);
                return new GeneralMessage() { StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم الارسال بنجاح" };

            }
            catch (Exception ex)
            {
                return new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الإرسال" };
            }

        }

        public async Task<int> SendWhatsApp_Twilio_Task(string AccountId, string AuthToken, string sendPhoneNumber, string RecievePhoneNumber, string Message, string environmentURL, string PDFURL)
        {

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
            try
            {


                //var accountSid = "AC21b75a6b7a6020096317985cb7fc0781";
                //var authToken = "51083bb163926b234a2e39cd1cca4229";
                string accountSid2 = Environment.GetEnvironmentVariable(AccountId);
                string authToken2 = Environment.GetEnvironmentVariable(AuthToken);
                var accountSid = AccountId;
                var authToken = AuthToken;

                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                //new PhoneNumber("whatsapp:+201020412606"));
                //new PhoneNumber("whatsapp:" + "+20" + RecievePhoneNumber));
                new PhoneNumber("whatsapp:" + "+966" + RecievePhoneNumber));

                //new PhoneNumber("whatsapp:+966503326610"));
                //new PhoneNumber("whatsapp:" + "+966" + RecievePhoneNumber));

                //messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                messageOptions.From = new PhoneNumber("whatsapp:" + sendPhoneNumber);

                messageOptions.Body = Message;

                if (PDFURL != "")
                {
                    var URLC = environmentURL + PDFURL;
                    //Uri uri = new Uri("https://api.tameercloud.com/TempFiles/NewInvoice.pdf");
                    //Uri uri = new Uri("https://api.tameercloud.com/TempFiles/OldInvoice.pdf");
                    Uri uri = new Uri(URLC);
                    messageOptions.MediaUrl.Add(uri);
                }
                else
                {
                    string imageFilePathLogo = environmentURL + "/Reports/PdfTest.pdf";
                    Uri uri = new Uri(imageFilePathLogo);
                    //Uri uri = new Uri("https://api.tameercloud.com/TempFiles/NewInvoice.pdf");

                    messageOptions.MediaUrl.Add(uri);
                }

                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                GeneralMessage msg = new GeneralMessage();

                if (message.Status == MessageResource.StatusEnum.Failed)
                    return 2;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                return 2;
            }
        }

        public async Task<int> SendWhatsApp_UltraMsg_Task(string AccountId, string AuthToken, string sendPhoneNumber, string RecievePhoneNumber, string Message, string environmentURL, string PDFURL)
        {

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
            try
            {
                //var File= "https://api.tameercloud.com/TempFiles/OldInvoice.pdf";
                var File = "https://api.tameercloud.com/TempFiles/NewInvoice.pdf";
                var File2 = "";


                if (PDFURL != "")
                {
                    File2 = environmentURL + PDFURL;

                }
                else
                {
                    File2 = environmentURL + "/Reports/PdfTest.pdf";

                }

                //string instanceId = "instance85943"; // your instanceId
                //string token = "y0znyu4yfdkifzfs";         //instance Token
                //string mobile = "+20" + "1020412606";



                //string instanceId = "instance85270"; // your instanceId
                //string token = "749v41rrvo6cs27f";         //instance Token
                //string mobile = "+20" + "1020412606";

                string instanceId = AccountId;
                string token = AuthToken;
                string mobile = "+966" + RecievePhoneNumber;  //


                var url = "https://api.ultramsg.com/" + instanceId + "/messages/document";
                var client = new RestClient(url);
                var request = new RestRequest(url, Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("token", token);
                request.AddParameter("to", mobile);
                request.AddParameter("filename", "File.pdf");
                request.AddParameter("document", File2);
                //request.AddParameter("caption", "فاتورة ضريبية \r\n شكرا لك");
                request.AddParameter("caption", Message);


                RestResponse response = client.ExecuteAsync(request).Result;
                var output = response.Content;
                Console.WriteLine(output);
                var ResultError = response!.Content!.Contains("error");
                if (response.StatusCode == HttpStatusCode.OK && ResultError == false)
                    return 1;
                else if (response.StatusCode == HttpStatusCode.OK && ResultError == true)
                    return 2;
                else
                    return 2;


            }
            catch (Exception ex)
            {
                return 2;
            }
        }

    }
}
