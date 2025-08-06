using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net.Mail;
using System.Net;
using TaamerProject.Service.Services;
using System.Net.Http;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FileController : ControllerBase
    {
        private IFileService _fileservice;
        private IOrganizationsService _OrganizationService;
        public GlobalShared _globalshared;
        public FileController(IFileService fileservice, IOrganizationsService organizationService)
        {
            _fileservice = fileservice;
            _OrganizationService = organizationService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllFiles")]
        public IActionResult GetAllFiles(int? ProjectId, string? SearchText, string? DateFrom, string? DateTo,int? Filetype)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fileservice.GetAllFiles(ProjectId, SearchText ?? "", DateFrom??"", DateTo??"", Filetype, _globalshared.BranchId_G).Result);
        }
        [HttpPost("GetAllFilesTree")]
        public IActionResult GetAllFilesTree([FromForm] int? ProjectId, [FromForm] string? SearchText, [FromForm] bool? IsCertified, [FromForm] string? DateFrom, [FromForm] string? DateTo, [FromForm] int? Filetype)
        {
            if(IsCertified == false) IsCertified = null;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fileservice.GetAllFilesTree(ProjectId, SearchText, IsCertified, DateFrom ?? "", DateTo ?? "", Filetype, _globalshared.BranchId_G).Result);
        }
        [HttpGet("GetAllTaskFiles")]
        public IActionResult GetAllTaskFiles(int TaskId, string? SearchText)
        {
            return Ok(_fileservice.GetAllTaskFiles(TaskId, SearchText ?? "").Result);
        }
        [HttpGet("GetAllCertificateFiles")]
        public IActionResult GetAllCertificateFiles(int? ProjectId, bool IsCertified)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fileservice.GetAllCertificateFiles(ProjectId, IsCertified, _globalshared.BranchId_G).Result);
        }
        [HttpGet("GetAllFilesByDateSearch")]
        public IActionResult GetAllFilesByDateSearch(int? ProjectId, DateTime DateFrom, DateTime DateTo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_fileservice.GetAllFilesByDateSearch(ProjectId, DateFrom, DateTo, _globalshared.BranchId_G).Result);
        }
        [HttpPost("SaveFile")]
        public IActionResult SaveFile(ProjectFiles file)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _fileservice.SaveFile(file, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("UpdateFileShare")]
        public IActionResult UpdateFileShare(ProjectFiles file)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Login/ResetPassword/" + resetCode;
            //var UrlS = Request.Url.AbsoluteUri;
            var UrlS = httpContext.Request.PathBase;

            StringBuilder sb = new StringBuilder(UrlS);
            
            // sb.ToString().Contains()
            sb.Replace("UpdateFileShare", "services/");
            sb.Replace("File", "login");

            var link = sb.ToString() + resetCode;
            var link2 = "http://144.91.68.47:1010/login/ResetPassword/" + resetCode;
            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");

            var org = _OrganizationService.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var fileimg = Server.MapPath("~") + org.LogoUrl;
            var fileimg = Path.Combine(org.Result.LogoUrl??"");

            var result = _fileservice.UpdateFileShare(file, _globalshared.BranchId_G, link, _globalshared.UserId_G, fileimg, url);

            return Ok(result);
        }
        [HttpPost("NotUpdateFileShare")]
        public IActionResult NotUpdateFileShare(ProjectFiles file)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Login/ResetPassword/" + resetCode;


            //var UrlS = Request.Url.AbsoluteUri;
            var UrlS = httpContext.Request.PathBase;

            StringBuilder sb = new StringBuilder(UrlS);

            // sb.ToString().Contains()
            sb.Replace("UpdateFileShare", "LogOut/");
            sb.Replace("File", "login");

            var link = sb.ToString() + resetCode;
            var link2 = "http://144.91.68.47:1010/login/ResetPassword/" + resetCode;

            //var url = Server.MapPath("~/Email/MailStamp.html");
            var url = Path.Combine("/Email/MailStamp.html");


            var org = _OrganizationService.GetOrganizationDataLogin(_globalshared.Lang_G);
            //var fileimg = Server.MapPath("~") + org.LogoUrl;
            var fileimg = Path.Combine(org.Result.LogoUrl ?? "");
            var result = _fileservice.NotUpdateFileShare(file, _globalshared.BranchId_G, link, _globalshared.UserId_G, fileimg, url);

            return Ok(result);
        }
        [HttpPost("SaveProjectFiles")]

        public IActionResult SaveProjectFiles(ProjectFiles Pfile, List<IFormFile> postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("/Files/ProjectFiles/");
            string fileName = ""; string fname = ""; string fnamepath = "";

            foreach (IFormFile postedFile in postedFiles)
            {
                fname = postedFile.FileName;
                fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
                fnamepath = Path.Combine(path, fileName);

                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        Pfile.FileUrl = "/Files/ProjectFiles/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            var result = _fileservice.SaveFile(Pfile, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpPost("DeleteFile")]
        public IActionResult DeleteFile(int FileId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _fileservice.DeleteFile(FileId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteFileGoogleDrive")]
        public IActionResult DeleteFileGoogleDrive(int FileId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _fileservice.DeleteFileDrive(FileId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteFileOneDrive")]
        public IActionResult DeleteFileOneDrive(int FileId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _fileservice.DeleteFileOneDrive(FileId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetFilesById")]
        public IActionResult GetFilesById(int FileId)
        {
            return Ok(_fileservice.GetFilesById(FileId).Result);
        }
        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
