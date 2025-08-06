using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using System.Net;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AttachmentController : ControllerBase
    {
        private IAttachmentService _attachmentservice;
        public GlobalShared _globalshared;

        public AttachmentController(IAttachmentService attachmentservice)
        {
            _attachmentservice = attachmentservice;
            HttpContext httpContext = HttpContext;_globalshared = new GlobalShared(httpContext);
        }
        [HttpGet("GetAllAttachments")]

        public IActionResult GetAllAttachments(int? EmpId, string SearchText)
        {
            return Ok(_attachmentservice.GetAllAttachments(EmpId, SearchText ?? ""));
        }

        [HttpGet("GetAllAttachments2")]

        public IActionResult GetAllAttachment2(int? EmpId)
        {
            return Ok(_attachmentservice.GetAllAttachments(EmpId, ""));
        }
        [HttpPost("SaveAttachment")]

        public IActionResult SaveAttachment([FromForm]Attachment attachment,IFormFile postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            string fileName = "";
            string pathes = "";
            if (postedFiles != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Attachment/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Attachment/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();

                //foreach (IFormFile postedFile in postedFiles)
                //{
                fileName = System.IO.Path.GetFileName(Guid.NewGuid() + postedFiles.FileName);
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
                    attachment.AttachmentUrl = pathes;
                }
            }

            var result = _attachmentservice.SaveAttachment(attachment, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("DeleteAttachment")]

        public IActionResult DeleteAttachment(int AttachmentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _attachmentservice.DeleteAttachment(AttachmentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpGet("GenerateRandomNo")]

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
