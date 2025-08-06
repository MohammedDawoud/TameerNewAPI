using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net.Mail;
using System.Net;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ContactUsController : ControllerBase
    {
        private ISocialMediaLinksService _socialMediaLinksService;
        private IContact_BranchesService _Contact_Branches;
        private INewsService _NewsService;
        public GlobalShared _globalshared;
        public ContactUsController(ISocialMediaLinksService socialMediaLinksService, IContact_BranchesService contact_Branches
            , INewsService newsService)
        {
            _socialMediaLinksService = socialMediaLinksService;
            _Contact_Branches = contact_Branches;
            _NewsService = newsService;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllSocialMediaLinks")]
        public IActionResult GetAllSocialMediaLinks()
        {
            var Links = _socialMediaLinksService.GetAllSocialMediaLinks();
            return Ok(Links);
        }
        [HttpPost("SaveSocialMediaLinks")]
        public IActionResult SaveSocialMediaLinks(SocialMediaLinks Slinks)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _socialMediaLinksService.SaveSocialMediaLinks(Slinks, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }

        [HttpGet("GetAllContactBranches")]
        public IActionResult GetAllContactBranches()
        {
            var Contact = _Contact_Branches.GetAllContactBranches();
            return Ok(_Contact_Branches.GetAllContactBranches());
        }

        [HttpPost("SaveContactBranches")]
        public IActionResult SaveContactBranches(Contact_Branches contactBranches)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Contact_Branches.SaveContactBranches(contactBranches, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteContactBranches")]
        public IActionResult DeleteContactBranches(int ContactId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _Contact_Branches.DeleteContactBranches(ContactId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GetAllNews")]
        public IActionResult GetAllNews()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var news = _NewsService.GetAllNews(_globalshared.Lang_G);
            return Ok(news);
        }
        [HttpPost("SaveNews")]
        public IActionResult SaveNews([FromForm]News news, List<IFormFile> postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("/Uploads/News/");
            string pathF = Path.Combine("Uploads/", "News/");

            string fileName = ""; string fname = ""; string fnamepath = "";

            foreach (IFormFile postedFile in postedFiles)
            {
                fname = postedFile.FileName;
                fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
                fnamepath = Path.Combine(pathF, fileName);

                try
                {
                    if (System.IO.File.Exists(fnamepath))
                    {
                        System.IO.File.Delete(fnamepath);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        news.NewsImg = "/Uploads/News/" + fileName;
                    }
                }
                catch (Exception ex)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            var result = _NewsService.SaveNews(news, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("ScaleImage")]

        public static Image ScaleImage(Image image, int height, int width)
        {
            int newWidth = width;
            int newHeight = height;
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }
        [HttpPost("DeleteNews")]
        public IActionResult DeleteNews(int NewsId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _NewsService.DeleteNews(NewsId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
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
