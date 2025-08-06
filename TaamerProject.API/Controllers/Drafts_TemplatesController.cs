using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class Drafts_TemplatesController : ControllerBase
    {
        private IDrafts_TemplatesService _drafts_TemplatesService;

        public GlobalShared _globalshared;
        public Drafts_TemplatesController(IDrafts_TemplatesService drafts_TemplatesService  )
        {

            _drafts_TemplatesService = drafts_TemplatesService;
           
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }

        [HttpGet("GetAllDrafts_templates")]
        public IActionResult GetAllDrafts_templates()
        {
            return Ok(_drafts_TemplatesService.GetAllDrafts_templates() );
        }
        
        [HttpPost("SaveDraft")]
        public IActionResult SaveDraft([FromForm]Drafts_Templates draft, List<IFormFile> postedFiles )
        {
            
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string path = Path.Combine("Uploads/Drafts/");
            string fileName = ""; string fname = ""; string fnamepath = "";

            for (int i = 0; i < postedFiles.Count; i++)
            {
                fname = postedFiles[0].FileName;
                fileName = System.IO.Path.GetFileName(GenerateRandomNo() + fname);
                fnamepath = Path.Combine(path, fileName);
                try
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fnamepath, System.IO.FileMode.Create))
                    {
                        postedFiles[0].CopyTo(stream);
                        string atturl = Path.Combine(path, fname);
                        draft.DraftUrl = "/Uploads/Drafts/" + fileName;
                        draft.Name = fileName;
                    }
                }
                catch (Exception)
                {
                    var massege = "فشل في رفع الملفات";
                    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                }           
            }
            var result = _drafts_TemplatesService.SaveDraft_Templates(draft, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);

            
        }


        [HttpGet("ConnectDraft_Templates_WithProject")]
        public IActionResult ConnectDraft_Templates_WithProject(int DraftId, int ProjectTypeId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _drafts_TemplatesService.ConnectDraft_Templates_WithProject(DraftId, ProjectTypeId, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok( result );
        }

        [HttpGet("FillAllDraft_Template")]
        public IActionResult FillAllDraft_Template()
        {
            var someProject = _drafts_TemplatesService.GetAllDrafts_templates().Result.GroupBy(x => x.Name).Select(x => x.FirstOrDefault()).Select(s => new {
                Id = s.DraftTempleteId,
                Name = s.Name
            });
            return Ok(someProject );
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

