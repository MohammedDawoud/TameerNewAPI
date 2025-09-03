
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using Dropbox.Api;
using Dropbox.Api.Files;
using static Dropbox.Api.Files.SearchMatchType;

namespace TaamerProject.Service.Services
{
    public class DropBoxService: IDropBoxService
    {
        private readonly TaamerProjectContext _TaamerProContext;




        public DropBoxService(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public  GeneralMessage DownloadFile(DropboxClient dbx, string folder, string file)
        {
            try
            {

                using (var response =  dbx.Files.DownloadAsync(folder + "/" + file))
                {
                    
                    //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\downloadtest\\" + file;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"+ file;

                    using (var fileStream = File.Create(path))
                    {
                        ( response.Result.GetContentAsStreamAsync()).Result.CopyTo(fileStream);
                    }
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }


        }
        public GeneralMessage UploadFile(DropboxClient dbx, string folder, string file, string content, byte[]? bytes)
        {
            try

            {
                //using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(bytes)))
                if (bytes != null && bytes.Length > 0)
                {
                    using (var mem = new MemoryStream(bytes))
                    {
                        var updated = dbx.Files.UploadAsync(
                            folder + "/" + file,
                            WriteMode.Overwrite.Instance,
                            body: mem).Result;
                        //Console.WriteLine("Saved {0}/{1} rev {2}", folder, file, updated.Rev);
                    }
                }
                else
                {
                    content = content.Substring(1);
                    using (var ms = new FileStream(content, FileMode.Open, FileAccess.Read))
                    {
                        FileMetadata updated = dbx.Files.UploadAsync(
                        folder + "/" + file,
                        WriteMode.Overwrite.Instance,
                        body: ms).Result;

                        var shareLinkInfo = new Dropbox.Api.Sharing.CreateSharedLinkWithSettingsArg(folder + "/" + file);
                        var reponseShare = dbx.Sharing.CreateSharedLinkWithSettingsAsync(shareLinkInfo).Result;
                        var a = reponseShare.Url;

                        
                    }
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
           

        }
        public GeneralMessage DeleteFile(DropboxClient dbx, string folder, string file)
        {
            try
            {
                var response = dbx.Files.DeleteAsync(folder + "/" + file).Result;
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
    }
}
