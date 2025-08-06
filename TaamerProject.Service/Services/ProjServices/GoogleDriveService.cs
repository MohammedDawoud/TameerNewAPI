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

namespace TaamerProject.Service.Services
{
    public class GoogleDriveService: IGoogleDriveService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private IFilesAuthService _FilesAuthService;
        private IFileService _fileservice;



        public static string[] Scopes = { Google.Apis.Drive.v3.DriveService.Scope.Drive };
        //static string ApplicationName = "TestApp";
        static string ApplicationName = "testproj-432419";

        public GoogleDriveService(IFileService fileservice, TaamerProjectContext dataContext, IFilesAuthService filesAuthService)
        {
            _fileservice = fileservice;
            _TaamerProContext = dataContext;
            _FilesAuthService = filesAuthService;


        }
        public GeneralMessage UploadFile(int FileId, int type, string folder, string file, string content, byte[]? bytes)
        {
            try
            {
                //UserCredential credential;

                //credential = GetCredentials();

                //// Create Drive API service.
                //var service = new DriveService(new BaseClientService.Initializer()
                //{
                //    HttpClientInitializer = credential,
                //    ApplicationName = ApplicationName,
                //});

                var appname = "TestProj";
                //appname = "testproj-432419";
                appname = "tameer";
                DriveService service =  GetServiceCustom(Environment.UserName, appname);

                // CreateFolder("YouTube123", service);
                content = content.Substring(1);
                UploadFile(FileId,type, content, service);

                //string pageToken = null;

                //do
                //{
                //    ListFiles(service,ref pageToken);

                //} while (pageToken!=null);

                //DeleteFileFolder("1gFgQsrIFTxe222222", service);

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

                var appname = "TestProj";
                appname = "tameer";
                DriveService service = GetServiceCustom(Environment.UserName, appname);
                DeleteFileFolder(FileId, service);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public GeneralMessage DownloadFile(string FileId, string UploadName)
        {
            try
            {

                var appname = "TestProj";
                appname = "tameer";
                DriveService service = GetServiceCustom(Environment.UserName, appname);
                var stream=DownloadFileFolder(FileId, service);
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + UploadName;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + UploadName;
                //string path2 = Environment.GetFolderPath(Environment.SpecialFolder.) + "\\" + UploadName;
                //string path3 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + UploadName;
                //string path4 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + UploadName;





                //using (var fileStream = File.Create(path))
                //{
                //    stream.CopyTo(fileStream);
                //}
                using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    stream.WriteTo(file);

                    //file.CopyTo(stream);

                    ////stream.CopyTo(file);
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedStr= path };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage CreateFolder_Func(string folderName)
        {
            try
            {
                UserCredential credential;

                credential = GetCredentials();

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                CreateFolder(folderName, service);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }
        private static void CreateFolder(string folderName, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("Folder ID: " + file.Id);

        }
        private static void DeleteFileFolder(string id, DriveService service)
        {
            var request = service.Files.Delete(id);

            request.Execute();

        }
        private MemoryStream DownloadFileFolder(string id, DriveService service)
        {
            try
            {
                var request = service.Files.Get(id);
                var stream = new MemoryStream();
                request.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine(progress.BytesDownloaded);
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    Console.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Console.WriteLine("Download failed.");
                                    break;
                                }
                        }
                    };
                request.Download(stream);
                return stream;
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
        private void UploadFile(int FileId, int type,string path, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path); 
            fileMetadata.MimeType = "image/jpeg";
            //fileMetadata.MimeType = Path.GetExtension(path);

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            var result = _fileservice.UpdateFileUploadFileId(FileId, file.Id, type);

            //Console.WriteLine("File ID: " + file.Id);

        }
        private static void ListFiles(DriveService service, ref string pageToken)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            //listRequest.Fields = "nextPageToken, files(id, name)";
            listRequest.Fields = "nextPageToken, files(name)";
            listRequest.PageToken = pageToken;
            listRequest.Q = "mimeType='image/jpeg'";

            // List files.
            var request = listRequest.Execute();


            if (request.Files != null && request.Files.Count > 0)
            {

                foreach (var file in request.Files)
                {
                    Console.WriteLine("{0}", file.Name);
                }

                pageToken = request.NextPageToken;

                if (request.NextPageToken != null)
                {
                    Console.WriteLine("Press any key to conti...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }


        }
        private static UserCredential GetCredentials()
        {
            UserCredential credential;

            using (var stream = new FileStream("Reports/Credential.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = "Reports/";

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                // Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }

        //------------------------------------------------------------------------------------------
        public static DriveService GetServiceProject(string Cspath, string FolserPath)
        {
            //get Credentials from client_secret.json file 

            UserCredential credential;
            //Root Folder of project
            var CSPath = Cspath;// System.Web.Hosting.HostingEnvironment.MapPath("~/");
            using (var stream = new FileStream(Path.Combine(CSPath, "Credential.json"), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = FolserPath;// System.Web.Hosting.HostingEnvironment.MapPath("~/"); ;
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }

            //create Drive API service.
            DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApiKey= "AIzaSyDMBqumxSeJ03L5LCymoi_x6MPZ0jnUXOM",
                ApplicationName = ApplicationName,
            });
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

            return service;

        }


        public  DriveService GetServiceCustom(string usern,string appName)
        {
            var FAuth = _FilesAuthService.GetFilesAuthByTypeId(2).Result;

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
                    resultRef = RefreshTokenDrive(FAuth.AppKey, FAuth.AppSecret, FAuth.RefreshToken);
                    FAuth = _FilesAuthService.GetFilesAuthByTypeId(2).Result;
                }
            }


            var tokenResponse = new TokenResponse
            {
                AccessToken = FAuth.AccessToken,
                RefreshToken = FAuth.RefreshToken,
            };

            var applicationName = ApplicationName;// Use the name of the project in Google Cloud
            var username = usern; // Use your email


            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = FAuth.AppKey,
                    ClientSecret = FAuth.AppSecret
                },
                Scopes = Scopes,
                DataStore = new FileDataStore(FAuth.FolderName, true)
            });


            var credential = new UserCredential(apiCodeFlow, username, tokenResponse);


            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });
            return service;
        }

        public HttpResponseMessage RefreshTokenDrive(string appkey, string appsecret, string refreshtoken)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            FilesAuth FileAuth = new FilesAuth();
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://oauth2.googleapis.com/token"))
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
                    var result = _FilesAuthService.UpdateTokenData(FileAuth, 2);
                    message.ReasonPhrase = "تم الحفظ";
                    message.StatusCode = HttpStatusCode.OK;
                    return message;
                    // process the response
                }
            }
        }


        public GeneralMessage CreateFolder_Func2(string folderName)
        {
            try
            {
                var service= new DriveService();
                //DriveService service = GetServiceProject("Reports/", "Reports/");

                //UserCredential credential;

                //credential = GetCredentials();

                //// Create Drive API service.
                //var service = new DriveService(new BaseClientService.Initializer()
                //{
                //    HttpClientInitializer = credential,
                //    ApplicationName = ApplicationName,
                //});
                CreateFolder(folderName, service);
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }


        }

        public GeneralMessage UploadFileNew(Stream file, string fileName, string fileMime, string folder, string fileDescription)
        {
            var service = new DriveService();
            var driveFile = new Google.Apis.Drive.v3.Data.File();
                    driveFile.Name = fileName;
                    driveFile.Description = fileDescription;
                    driveFile.MimeType = fileMime;
            driveFile.Parents = new string[] { folder };


            var request = service.Files.Create(driveFile, file, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed ,ReturnedStr= response.Exception.Message };

            return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully,ReturnedStr= request.ResponseBody.Id };
        }
        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetFiles(DriveService service, string folder)
        {
            var fileList = service.Files.List();
            fileList.Q = $"mimeType!='application/vnd.google-apps.folder' and '{folder}' in parents";
            fileList.Fields = "nextPageToken, files(id, name, size, mimeType)";

            var result = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                fileList.PageToken = pageToken;
                var filesResult = fileList.Execute();
                var files = filesResult.Files;
                pageToken = filesResult.NextPageToken;
                result.AddRange(files);
            } while (pageToken != null);

            return result;
        }
        public void DeleteFile(DriveService service, string fileId)
        {
            var command = service.Files.Delete(fileId);
            var result = command.Execute();
        }
    }
}
