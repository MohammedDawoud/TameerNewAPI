using Dropbox.Api;
using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]
    public class FilesAuthController : ControllerBase
    {
        private readonly IFilesAuthService _FilesAuthService;
        private IDropBoxService _dropBoxService;
        private IGoogleDriveService _googleDriveService;

        public GlobalShared _globalshared;

        public FilesAuthController(IFilesAuthService FilesAuthService, IDropBoxService dropBoxService, IGoogleDriveService googleDriveService)
        {
            _FilesAuthService = FilesAuthService;
            _dropBoxService = dropBoxService;
            _googleDriveService = googleDriveService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllFilesAuth")]

        public IActionResult GetAllFilesAuth()
        {
            var FAuth = _FilesAuthService.GetAllFilesAuth();
            return FAuth == null ? NotFound() : Ok(FAuth);
        }
        [HttpGet("GetFilesAuthByTypeId")]
        public IActionResult GetFilesAuthByTypeId(int TypeId)
        {
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(TypeId);
            return FAuth == null ? NotFound() : Ok(FAuth);
        }
        [HttpPost("SaveFileAuth")]

        public IActionResult SaveFileAuth(FilesAuth FileAuth)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _FilesAuthService.SaveFileAuth(FileAuth, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteFileAuth")]

        public IActionResult DeleteFileAuth(int FilesAuthid)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _FilesAuthService.DeleteFileAuth(FilesAuthid, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpGet("AuthAsync")]
        public HttpResponseMessage AuthAsync()
        {
            HttpResponseMessage message = new HttpResponseMessage();
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(1).Result;
            if(FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات الدروب بوكس";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;
            try
            {
                var response = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Code, AppKey, RedirectUri, null, false, false, null, false, TokenAccessType.Offline);
                var url = response.AbsoluteUri;
                message.ReasonPhrase = url;
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
        }
        //dropbox
        //------------------
        [HttpGet("authorizeDropBox")]
        public HttpResponseMessage authorizeDropBox([FromQuery] string? code)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(1).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase =  "تأكد من حفظ اعدادات الدروب بوكس";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;
            try
            {
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> dictionary = new Dictionary<string, string>
                {
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", RedirectUri },
                    { "client_id", AppKey },
                    { "client_secret", AppSecret },
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(dictionary);
                HttpResponseMessage responsee = httpClient.PostAsync("https://api.dropbox.com/oauth2/token", content).Result;
                JObject jObject = JObject.Parse(responsee.Content.ReadAsStringAsync().Result);
                if (responsee.StatusCode != HttpStatusCode.OK)
                {
                    throw new OAuth2Exception(jObject["error"].ToString(), jObject.Value<string>("error_description"));
                }

                string refreshToken = null;
                if (jObject.Value<string>("refresh_token") != null)
                {
                    FileAuth.RefreshToken = jObject["refresh_token"].ToString();
                }

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
                var result=_FilesAuthService.UpdateTokenData(FileAuth,1);
                message.ReasonPhrase = "تم الحفظ";
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }

        }
        [HttpGet("AuthAsyncDrive")]
        public HttpResponseMessage AuthAsyncDrive()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,DriveService.Scope.DriveFile};
            HttpResponseMessage message = new HttpResponseMessage();
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(2).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات جوجل درايف";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;
            try
            {
                var Link = "https://accounts.google.com/o/oauth2/auth?scope=https://www.googleapis.com/auth/drive&response_type=code&access_type=offline&redirect_uri=" + RedirectUri + "&client_id=" + AppKey + "";
                message.ReasonPhrase = Link;
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
        }
        [HttpGet("AuthAsyncOneDrive")]
        public HttpResponseMessage AuthAsyncOneDrive()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
            HttpResponseMessage message = new HttpResponseMessage();
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(3).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات ون درايف";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
            var RedirectUri = FAuth.RedirectUri;
            var AppKey = FAuth.AppKey;
            var AppSecret = FAuth.AppSecret;
            try
            {
                string ALL_SCOPE_AUTHORIZATIONS = "user.read calendars.readwrite calendars.readwrite.shared offline_access openid place.read.all";

                var Link2 = "https://login.live.com/oauth20_authorize.srf?scope=Files.Read.All&response_type=code&access_type=offline&redirect_uri=" + RedirectUri + "&client_id=" + AppKey + "";
                //var Link = "https://accounts.google.com/o/oauth2/auth?scope=https://www.googleapis.com/auth/drive&response_type=code&access_type=offline&redirect_uri=" + RedirectUri + "&client_id=" + AppKey + "";
                var Link3 = "https://login.microsoftonline.com/common/oauth2/authorize?response_type=code&access_type=offline&redirect_uri=" + RedirectUri + "&client_id=" + AppKey + "";
                var Link4 = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize?scope=Files.Read.All&response_type=code&access_type=offline&redirect_uri=" + RedirectUri + "&client_id=" + AppKey + "";
                var Link = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize?scope=" + ALL_SCOPE_AUTHORIZATIONS + "&response_type=code&access_type=offline&redirect_uri=" + RedirectUri + "&client_id=" + AppKey + "";

                message.ReasonPhrase = Link;
                message.StatusCode = HttpStatusCode.OK;
                return message;

            }
            catch (Exception e)
            {
                message.ReasonPhrase = "فشل في الحفظ";
                message.StatusCode = HttpStatusCode.BadRequest;
                return message;
            }
        }
        //dropbox
        //------------------


        [HttpGet("DownloadFileDropBox")]
        public IActionResult DownloadFileDropBox(string localFilePath)
        {
            GeneralMessage? message = new GeneralMessage();
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(1).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات الدروب بوكس";
                message.StatusCode = HttpStatusCode.BadRequest;
                return Ok(message);
            }

            //var result = RefreshToken(app_key, app_secret, refreshtoken);
            var dbx = new DropboxClient(FAuth.AccessToken);
            string folder = "/TestFolder";
            string file = "servicesPrice.txt";
            //file = "TgrebeMswda.pdf";

            file = "إضافة شاشات في تعمير.docx";
            var Res = _dropBoxService.DownloadFile(dbx, folder, file);
            return Ok(Res);
        }
        [HttpGet("UploadFileDropBox")]
        public IActionResult UploadFileDropBox(string localFilePath)
        {
            GeneralMessage? message = new GeneralMessage();
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(1).Result;
            if (FAuth == null)
            {
                message.ReasonPhrase = "تأكد من حفظ اعدادات الدروب بوكس";message.StatusCode = HttpStatusCode.BadRequest;
                return Ok(message);
            }
            var resultRef = new HttpResponseMessage();var DateTimeDiff = 0;
            if(FAuth.CreationDate!=null)
            {
                DateTimeDiff = ((DateTime.Now) - (FAuth.CreationDate?? DateTime.Now)).Hours;
            }
            if (!(FAuth.RefreshToken==null || FAuth.RefreshToken==""))
            {
                if(DateTimeDiff>=3)
                {
                    resultRef = RefreshToken(FAuth.AppKey, FAuth.AppSecret, FAuth.RefreshToken);
                    FAuth = _FilesAuthService.GetFilesAuthByTypeId(1).Result;
                }
            }

            var dbx = new DropboxClient(FAuth.AccessToken);

            string folder = "/TestFolder";
            string file = "TgrebeMswda.pdf";
            //file = "servicesPrice.txt";
            //file = "إضافة شاشات في تعمير.docx";
            string content = "C:\\Users\\Sky Hawk\\Desktop\\" + file;
            string content2 = "C:/Users/Sky Hawk/Desktop/" + file;


            byte[] bytes2 = { };
            var Res = _dropBoxService.UploadFile(dbx, folder, file, content, bytes2);
            return Ok(Res);
        }
        [HttpGet("RefreshToken")]
        public HttpResponseMessage RefreshToken(string appkey, string appsecret, string refreshtoken)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://api.dropbox.com/oauth2/token"))
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
                    var result = _FilesAuthService.UpdateTokenData(FileAuth,1);
                    message.ReasonPhrase = "تم الحفظ";
                    message.StatusCode = HttpStatusCode.OK;
                    return message;
                    // process the response
                }
            }
        }
        [HttpGet("RefreshTokenOneDrive")]
        public HttpResponseMessage RefreshTokenOneDrive(string appkey, string appsecret, string refreshtoken)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://login.live.com/oauth20_token.srf"))
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
                    var result = _FilesAuthService.UpdateTokenData(FileAuth, 1);
                    message.ReasonPhrase = "تم الحفظ";
                    message.StatusCode = HttpStatusCode.OK;
                    return message;
                    // process the response
                }
            }
        }

    }
}
