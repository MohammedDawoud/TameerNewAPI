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

    public class CustomerFilesController : ControllerBase
    {
        private ICustomerFilesService _customerFilesservice;
        public GlobalShared _globalshared;
        public CustomerFilesController(ICustomerFilesService customerFilesservice)
        {
            _customerFilesservice = customerFilesservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllCustomerFilesUploaded")]
        public IActionResult GetAllCustomerFilesUploaded(int CustomerId)
        {
            return Ok(_customerFilesservice.GetAllCustomerFilesUploaded(CustomerId, ""));
        }
        [HttpGet("GetAllCustomerFiles")]
        public IActionResult GetAllCustomerFiles(int? CustomerId)
        {
            return Ok(_customerFilesservice.GetAllCustomerFiles(CustomerId));
        }
        [HttpPost("UploadCustomerFiles")]
        public IActionResult UploadCustomerFiles([FromForm]string FileId, [FromForm] string? Description, [FromForm] string? TypeId,
            [FromForm] string? UploadDate, [FromForm] string? CustomerId, [FromForm] List<IFormFile> postedFiles)
        {
            CustomerFiles customerFiles = new CustomerFiles();
            customerFiles.FileId = Convert.ToInt32(FileId??"0");
            customerFiles.Description = Description;
            customerFiles.TypeId = Convert.ToInt32(TypeId ?? "0");
            customerFiles.UploadDate = UploadDate;
            customerFiles.CustomerId = Convert.ToInt32(CustomerId ?? "0");



            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string fileName = "";
            var FileResult = new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الرفع", ReturnedStr = "" };
            try
            {
                if (postedFiles != null)
                {
                    string path = System.IO.Path.Combine("Uploads/", "Customers/Files/");
                    string pathW = System.IO.Path.Combine("/Uploads/", "Customers/Files/");

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();

                    for (int i = 0; i < postedFiles.Count; i++)
                    {
                        fileName = System.IO.Path.GetFileName(Guid.NewGuid() + postedFiles[i].FileName);

                        var path2 = Path.Combine(path, fileName);
                        if (System.IO.File.Exists(path2))
                        {
                            System.IO.File.Delete(path2);
                        }
                        using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                        {
                            postedFiles[i].CopyTo(stream);
                            uploadedFiles.Add(fileName);
                            string atturl = Path.Combine(path, fileName);
                            customerFiles.FileUrl = pathW + fileName;
                            customerFiles.FileName = fileName;
                        }
                    }

                    FileResult = _customerFilesservice.UploadCustomerFiles(customerFiles, _globalshared.UserId_G, _globalshared.BranchId_G);
                }

                return Ok(FileResult);
            }
            catch (Exception ex)
            {
                return Ok(FileResult);
            }


        }
        [HttpPost("DeleteCustomerFiles")]
        public IActionResult DeleteCustomerFiles(int FileId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _customerFilesservice.DeleteUpoladCustomerFiles(FileId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveCustomerFiles")]
        public IActionResult SaveCustomerFiles([FromForm]CustomerFiles customerFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _customerFilesservice.SaveCustomerFiles(customerFiles, _globalshared.UserId_G, _globalshared.BranchId_G);
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
