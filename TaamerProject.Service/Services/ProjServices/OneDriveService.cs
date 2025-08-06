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
using TaamerP.Service.LocalResources;
using Dropbox.Api;
using Dropbox.Api.Files;
using static Dropbox.Api.Files.SearchMatchType;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Web.ApplicationServices;
using Google.Apis.Download;
using System.IO;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Azure.Core;
using Microsoft.Graph.Models;
//using RestSharp.Interceptors;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using Microsoft.Kiota.Abstractions.Authentication;
using Google.Apis.Drive.v3.Data;
using static Dropbox.Api.Files.SearchMatchTypeV2;
using System.IO.Pipes;

namespace TaamerProject.Service.Services
{
    public class OneDriveService: IOneDriveService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private IFilesAuthService _FilesAuthService;
        private IFileService _fileservice;



        public static string[] Scopes = { Google.Apis.Drive.v3.DriveService.Scope.Drive };
        //static string ApplicationName = "TestApp";
        static string ApplicationName = "testproj-432419";

        public OneDriveService(IFileService fileservice, TaamerProjectContext dataContext, IFilesAuthService filesAuthService)
        {
            _fileservice = fileservice;
            _TaamerProContext = dataContext;
            _FilesAuthService = filesAuthService;


        }
        public GeneralMessage UploadFile(int FileId, int type, string folder, string file, string content, byte[]? bytes)
        {
            try
            {
                GraphServiceClient service = InitializeGraphClientAsync();
                content = content.Substring(1);
                UploadFile_Func(FileId, type, content, file, service);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }
        public GeneralMessage DeleteFile(string FileId)
        {
            try
            {

                GraphServiceClient service = InitializeGraphClientAsync();
                DeleteFileFolder(FileId, service);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DownloadFile(string filename)
        {
            try
            {

                GraphServiceClient service = InitializeGraphClientAsync();
                var stream = DownloadFileFolder(filename, service);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + filename;
                //using (var fileStream = File.Create(path))
                //{
                //    stream.CopyTo(fileStream);
                //}
                using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    //stream.WriteTo(file);
                    stream.CopyTo(file);

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        private Stream DownloadFileFolder(string nameFile, GraphServiceClient graphClient)
        {
            try
            {
                var stream = new MemoryStream();

                var drive = graphClient.Me.Drive.GetAsync().Result;
                var result = graphClient.Drives[drive.Id].Items["root"].Children.GetAsync().Result;
                var item = result.Value.Where(s => s.Name == nameFile).FirstOrDefault();
                var res=graphClient.Drives[drive.Id].Items[item.Id].Content.GetAsync().Result;
                //foreach (var item in )
                //{
                //    using (var fileStream = new FileStream(item.Name, FileMode.Create, System.IO.FileAccess.Write))
                //        fileContent.CopyTo(fileStream);
                //}

                //stream = res?? new MemoryStream();
                return res;
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else
                {
                }
            }
            return null;
        }

        private async void UploadFile_Func(int FileId, int type, string path,string filename, GraphServiceClient graphClient)
        {


            try
            {
                string pathf = path;
                byte[] data = System.IO.File.ReadAllBytes(pathf);
                //Task? Item;
                var drive =  graphClient.Me.Drive.GetAsync().Result;
                using (Stream stream = new MemoryStream(data))
                {
                    graphClient.Drives[drive.Id].Root.ItemWithPath(filename).Content.PutAsync(stream);
                    //var result = _fileservice.UpdateFileUploadFileId(FileId, Item.Id, type);
                }
                //var stream = new MemoryStream(data);
                //var item = graphClient.Drives[drive.Id].Root.ItemWithPath(filename).Content.PutAsync(stream);
                //var aaa =  item.IsCompletedSuccessfully;
                //if (item.IsCompletedSuccessfully) {
                //    var aasw = item;
                //}


            }
            catch (ServiceException ex)
            {
                
            }
        }

        private static void DeleteFileFolder(string nameFile, GraphServiceClient graphClient)
        {
            var drive = graphClient.Me.Drive.GetAsync().Result;
            var result =  graphClient.Drives[drive.Id].Items["root"].Children.GetAsync().Result;
            var item= result.Value.Where(s => s.Name == nameFile).FirstOrDefault() ;

            graphClient.Drives[drive.Id].Items[item.Id].DeleteAsync();
        }

        public GraphServiceClient InitializeGraphClientAsync()
        {
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(3).Result;

            var resultRef = new HttpResponseMessage(); var DateTimeDiff = 0;
            if (FAuth.CreationDate != null)
            {
                DateTimeDiff = (int)(DateTime.Now).Subtract((FAuth.CreationDate ?? DateTime.Now)).TotalSeconds;
                //DateTimeDiff = ((DateTime.Now) - (FAuth.CreationDate ?? DateTime.Now)).Minutes;
            }
            if (!(FAuth.RefreshToken == null || FAuth.RefreshToken == ""))
            {
                if (DateTimeDiff >= 3000)
                {
                    resultRef = RefreshTokenOneDrive(FAuth.AppKey, FAuth.AppSecret, FAuth.RefreshToken);
                    FAuth = _FilesAuthService.GetFilesAuthByTypeId(3).Result;
                }
            }

            StringValues authorizationToken;
            string incomingToken = FAuth.AccessToken;
            TokenProvider provider = new TokenProvider();
            provider.token = incomingToken;
            var authenticationProvider = new BaseBearerTokenAuthenticationProvider(provider);
            var graphServiceClient = new GraphServiceClient(authenticationProvider);
            //var user =  graphServiceClient.Users.GetAsync().Result;

            return graphServiceClient;
        }
        public class TokenProvider : IAccessTokenProvider
        {
            public string token { get; set; }
            public AllowedHostsValidator AllowedHostsValidator => throw new NotImplementedException();

            public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
            {
                return  Task.FromResult(token);
            }
        }


        public HttpResponseMessage RefreshTokenOneDrive(string appkey, string appsecret, string refreshtoken)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://login.microsoftonline.com/common/oauth2/v2.0/token"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("" + appkey + ":" + appsecret + ""));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var contentList = new List<string>();
                    contentList.Add("grant_type=refresh_token");
                    contentList.Add("refresh_token=" + refreshtoken + "");
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var responsee = httpClient.SendAsync(request).Result;

                    JObject jObject = JObject.Parse(responsee.Content.ReadAsStringAsync().Result);
                    if (responsee.StatusCode != HttpStatusCode.OK)
                    {
                        throw new OAuth2Exception(jObject["error"].ToString(), jObject.Value<string>("error_description"));
                    }

                    //string refreshToken = null;
                    //if (jObject.Value<string>("refresh_token") != null)
                    //{
                    //    FileAuth.RefreshToken = jObject["refresh_token"].ToString();
                    //}

                    int num = -1;
                    if (jObject.Value<string>("expires_in") != null)
                    {
                        FileAuth.ExpiresIn = jObject["expires_in"].ToObject<int>();
                    }

                    string[] scopeList = null;
                    if (jObject.Value<string>("scope") != null)
                    {
                        scopeList = jObject["scope"].ToString().Split(new char[1] { ' ' });
                    }
                    FileAuth.AccessToken = jObject["access_token"].ToString();
                    var result = _FilesAuthService.UpdateTokenData(FileAuth, 3);
                    message.ReasonPhrase = "تم الحفظ";
                    message.StatusCode = HttpStatusCode.OK;
                    return message;
                    // process the response
                }
            }
        }
    }
}
