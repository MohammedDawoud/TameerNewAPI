using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;
using System.Net;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OfficialDocumentsController : ControllerBase
    {
        private IOfficialDocumentsService _officialDocumentsservice;
        public GlobalShared _globalshared;
        public OfficialDocumentsController(IOfficialDocumentsService officialDocumentsservice)
        {
             _officialDocumentsservice = officialDocumentsservice;
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetAllOfficialDocuments")]
        public IActionResult GetAllOfficialDocuments()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_officialDocumentsservice.GetAllOfficialDocuments(_globalshared.Lang_G) );
        }
        [HttpGet("FillOfficialDocumentsSelect")]
        public IActionResult FillOfficialDocumentsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_officialDocumentsservice.GetAllOfficialDocuments(_globalshared.Lang_G).Result.Select(s => new {
                Id = s.DocumentId,
                Name = s.Number
            }));
        }

        //public IActionResult SaveOfficialDocuments(OfficialDocuments officialDocuments)
        //{
        //    var result = _officialDocumentsservice.SaveOfficialDocuments(officialDocuments);
        //    return Ok(new { result.Result, result.Message }, OkRequestBehavior.AllowGet);
        //}
        [HttpPost("SaveOfficialDocuments")]
        public IActionResult SaveOfficialDocuments([FromForm]OfficialDocuments officialDocuments, List<IFormFile>? postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("/Uploads/Projects/Documents/");
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
                        officialDocuments.AttachmentUrl = "/Uploads/Projects/Documents/" + fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }
            }
            var result = _officialDocumentsservice.SaveOfficialDocuments(officialDocuments, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);

           
             
        }


        [HttpPost("SaveOfficialDocuments2")]
        public IActionResult SaveOfficialDocuments2([FromForm] OfficialDocuments officialDocuments, IFormFile? postedFiles)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


                try
                {

                if (postedFiles != null)
                {
                    System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                    string path = System.IO.Path.Combine("Uploads/Projects/", "Documents/");
                    string pathW = System.IO.Path.Combine("/Uploads/Projects/", "Documents/");

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
                        officialDocuments.AttachmentUrl = pathes;

                    }
                }


            }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }


            
            var result = _officialDocumentsservice.SaveOfficialDocuments(officialDocuments, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);



        }

        [HttpPost("DeleteOfficialDocuments")]
        public IActionResult DeleteOfficialDocuments(int DocumentId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _officialDocumentsservice.DeleteOfficialDocuments(DocumentId, _globalshared.UserId_G, _globalshared.BranchId_G);
            
            return Ok(result);
        }
        [HttpGet("GetNotifiedDocuments")]
        public IActionResult GetNotifiedDocuments()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_officialDocumentsservice.GetDocumentToNotified(_globalshared.BranchId_G, _globalshared.Lang_G) );
        }
        [HttpPost("SearchOfficialDocuments")]
        public IActionResult SearchOfficialDocuments([FromBody]OfficialDocumentsVM OfficialDocumentsSearch)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_officialDocumentsservice.SearchOfficialDocuments(OfficialDocumentsSearch, _globalshared.BranchId_G) );
        }
        [HttpGet("GetMaxOfficialDocumentsNumber")]
        public IActionResult GetMaxOfficialDocumentsNumber()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            return Ok(_officialDocumentsservice.GetMaxOfficialDocumentsNumber(_globalshared.BranchId_G) );
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
