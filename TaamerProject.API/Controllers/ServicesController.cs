using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net;
using System.Net.Mail;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ServicesController : ControllerBase
    {
        private IServiceService _serviceservice;
        public GlobalShared _globalshared;
        public ServicesController(IServiceService serviceservice)
        {
            _serviceservice = serviceservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllServices")]
        public IActionResult GetAllServices()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_serviceservice.GetAllServices(_globalshared.BranchId_G));
        }
        [HttpGet("FillServicesSelect")]
        public IActionResult FillServicesSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_serviceservice.GetAllServices(_globalshared.BranchId_G).Result.Select(s => new {
                Id = s.ServiceId,
                Name = s.Number
            }));
        }
        [HttpPost("SaveService")]
        public IActionResult SaveService([FromForm]TaamerProject.Models.Service service, List<IFormFile> postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("/Uploads/Service/");
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
                        service.AttachmentUrl = "/Uploads/Service/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            var result = _serviceservice.SaveService(service, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }



        [HttpPost("SaveService2")]
        public IActionResult SaveService2([FromForm] TaamerProject.Models.Service service, IFormFile? postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            try
            {

                if (postedFiles != null)
                {
                    System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                    string path = System.IO.Path.Combine("Uploads/", "Service/");
                    string pathW = System.IO.Path.Combine("/Uploads/", "Service/");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();
                    string pathes = "";
                    //foreach (IFormFile postedFile in postedFiles)
                    //{
                    string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + postedFiles.FileName);
                    //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                    var path2 = Path.Combine(path, fileName);
                    if (System.IO.File.Exists(path2))
                    {
                        System.IO.File.Delete(path2);
                    }
                    using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                    {


                        postedFiles.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        // string returnpath = host + path + fileName;
                        //pathes.Add(pathW + fileName);
                        pathes = pathW + fileName;
                    }


                    if (pathes != null)
                    {
                        service.AttachmentUrl = pathes;

                    }
                }


            }
            catch (Exception)
            {
                var massege = "فشل في رفع الملفات";
                return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
            }

            var result = _serviceservice.SaveService(service, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteService")]
        public IActionResult DeleteService(int ServiceId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _serviceservice.DeleteService(ServiceId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("GetServicesSearch")]
        public IActionResult GetServicesSearch([FromBody]ServicesVM ServiceSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_serviceservice.GetServicesSearch(ServiceSearch, _globalshared.BranchId_G));
        }
        [HttpGet("GetServicesToNotified")]
        public IActionResult GetServicesToNotified()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _serviceservice.GetServicesToNotified(_globalshared.BranchId_G, _globalshared.Lang_G);
            return Ok(result);
        }
        [HttpGet("GenerateServicesNumber")]
        public IActionResult GenerateServicesNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_serviceservice.GenerateServicesNumber(_globalshared.BranchId_G));
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
